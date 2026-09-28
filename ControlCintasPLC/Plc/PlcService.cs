using ControlCintasPLC.Config;
using S7.Net;

namespace ControlCintasPLC.Plc
{
    /// <summary>
    /// Acceso al PLC serializado (S7.Net no es thread-safe). Lectura por bloques: 1 petición por DB.
    /// </summary>
    public sealed class PlcService : IDisposable
    {
        private readonly SemaphoreSlim _gate = new(1, 1);
        private S7.Net.Plc? _plc;

        public PlcConfig Config { get; private set; } = new();
        public bool IsConnected => _plc?.IsConnected == true;

        public async Task ConectarAsync(PlcConfig cfg, CancellationToken ct = default)
        {
            await _gate.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                _plc?.Close();
                Config = cfg;
                _plc = new S7.Net.Plc(CpuType.S71200, cfg.Ip, cfg.Puerto, cfg.Rack, cfg.Slot)
                {
                    ReadTimeout = 2000,
                    WriteTimeout = 2000
                };
                await _plc.OpenAsync(ct).ConfigureAwait(false);
            }
            finally { _gate.Release(); }
        }

        public void Desconectar()
        {
            bool tomado = _gate.Wait(3000);
            try { _plc?.Close(); }
            finally { if (tomado) _gate.Release(); }
        }

        private S7.Net.Plc Plc => _plc is { IsConnected: true } p ? p : throw new InvalidOperationException("PLC no conectado");

        private async Task<T> Exec<T>(Func<S7.Net.Plc, Task<T>> op, CancellationToken ct)
        {
            await _gate.WaitAsync(ct).ConfigureAwait(false);
            try { return await op(Plc).ConfigureAwait(false); }
            finally { _gate.Release(); }
        }

        private Task Exec(Func<S7.Net.Plc, Task> op, CancellationToken ct = default) =>
            Exec(async p => { await op(p).ConfigureAwait(false); return true; }, ct);

        public Task<PlantaSnapshot> LeerAsync(int nCintas, int nTemps, int nVfds, CancellationToken ct = default) =>
            Exec(async p =>
            {
                var g = await p.ReadBytesAsync(DataType.DataBlock, Config.DbGeneral, 0, DbLayout.General.Size, ct).ConfigureAwait(false);
                var c = await LeerDb(p, Config.DbCintas, nCintas, DbLayout.Cinta.Size, ct).ConfigureAwait(false);
                var t = await LeerDb(p, Config.DbTemperaturas, nTemps, DbLayout.Temperatura.Size, ct).ConfigureAwait(false);
                var v = await LeerDb(p, Config.DbVariadores, nVfds, DbLayout.Variador.Size, ct).ConfigureAwait(false);
                return new PlantaSnapshot(
                    GeneralEstado.Parse(g),
                    Trocear(c, nCintas, DbLayout.Cinta.Size, CintaEstado.Parse),
                    Trocear(t, nTemps, DbLayout.Temperatura.Size, TemperaturaEstado.Parse),
                    Trocear(v, nVfds, DbLayout.Variador.Size, VariadorEstado.Parse));
            }, ct);

        private static async Task<byte[]> LeerDb(S7.Net.Plc p, int db, int n, int size, CancellationToken ct) =>
            n == 0 ? Array.Empty<byte>() : await p.ReadBytesAsync(DataType.DataBlock, db, 0, n * size, ct).ConfigureAwait(false);

        private delegate T Parser<out T>(ReadOnlySpan<byte> b);

        private static List<T> Trocear<T>(byte[] data, int n, int size, Parser<T> parse)
        {
            var r = new List<T>(n);
            for (int i = 0; i < n; i++) r.Add(parse(data.AsSpan(i * size, size)));
            return r;
        }

        // ---- Escrituras (offset relativo al inicio del elemento indice 0..n-1)
        public Task EscribirBitAsync(int db, int adr, int bit, bool valor) =>
            Exec(p => p.WriteBitAsync(DataType.DataBlock, db, adr, bit, valor));

        public Task EscribirIntAsync(int db, int adr, short valor) =>
            Exec(p => p.WriteBytesAsync(DataType.DataBlock, db, adr, DbLayout.ToBytes(valor)));

        public Task EscribirDIntAsync(int db, int adr, int valor) =>
            Exec(p => p.WriteBytesAsync(DataType.DataBlock, db, adr, DbLayout.ToBytes(valor)));

        public Task EscribirRealAsync(int db, int adr, float valor) =>
            Exec(p => p.WriteBytesAsync(DataType.DataBlock, db, adr, DbLayout.ToBytes(valor)));

        public Task EscribirByteAsync(int db, int adr, byte valor) =>
            Exec(p => p.WriteBytesAsync(DataType.DataBlock, db, adr, new[] { valor }));

        // ---- Atajos por tipo de elemento
        public Task CintaBitAsync(int idx, int byteRel, int bit, bool v) =>
            EscribirBitAsync(Config.DbCintas, idx * DbLayout.Cinta.Size + byteRel, bit, v);

        public Task TempBitAsync(int idx, int bit, bool v) =>
            EscribirBitAsync(Config.DbTemperaturas, idx * DbLayout.Temperatura.Size + DbLayout.Temperatura.Bits, bit, v);

        public Task TempRealAsync(int idx, int adrRel, float v) =>
            EscribirRealAsync(Config.DbTemperaturas, idx * DbLayout.Temperatura.Size + adrRel, v);

        public Task VfdBitAsync(int idx, int bit, bool v) =>
            EscribirBitAsync(Config.DbVariadores, idx * DbLayout.Variador.Size + DbLayout.Variador.Bits, bit, v);

        public Task VfdRealAsync(int idx, int adrRel, float v) =>
            EscribirRealAsync(Config.DbVariadores, idx * DbLayout.Variador.Size + adrRel, v);

        public Task GeneralBitAsync(int bit, bool v) =>
            EscribirBitAsync(Config.DbGeneral, DbLayout.General.Bits, bit, v);

        /// <summary>Descarga al PLC todos los parámetros de plantconfig.json.</summary>
        public async Task EnviarConfiguracionAsync(PlantaConfig cfg)
        {
            await EscribirDIntAsync(Config.DbGeneral, DbLayout.General.RetardoSecuenciaMs, cfg.RetardoSecuenciaMs);
            await EscribirIntAsync(Config.DbGeneral, DbLayout.General.NumCintas, (short)cfg.Cintas.Count);

            for (int i = 0; i < cfg.Cintas.Count; i++)
            {
                var c = cfg.Cintas[i];
                int b = i * DbLayout.Cinta.Size;
                await EscribirIntAsync(Config.DbCintas, b + DbLayout.Cinta.TiempoAtascoS, c.TiempoAtascoS);
                await CintaBitAsync(i, DbLayout.Cinta.BitsA, DbLayout.Cinta.BitHabilitada, c.Habilitada);
            }

            for (int i = 0; i < cfg.Temperaturas.Count; i++)
            {
                var t = cfg.Temperaturas[i];
                int b = i * DbLayout.Temperatura.Size;
                await TempRealAsync(i, DbLayout.Temperatura.LimiteAviso, t.LimiteAviso);
                await TempRealAsync(i, DbLayout.Temperatura.LimiteDisparo, t.LimiteDisparo);
                await EscribirByteAsync(Config.DbTemperaturas, b + DbLayout.Temperatura.CintaAsociada, t.CintaAsociada);
                await TempBitAsync(i, DbLayout.Temperatura.BitHabilitado, t.Habilitado);
            }

            for (int i = 0; i < cfg.Variadores.Count; i++)
            {
                var v = cfg.Variadores[i];
                int b = i * DbLayout.Variador.Size;
                await EscribirByteAsync(Config.DbVariadores, b + DbLayout.Variador.CintaAsociada, v.CintaAsociada);
                await VfdRealAsync(i, DbLayout.Variador.FrecMaxHz, v.FrecMaxHz);
                await VfdRealAsync(i, DbLayout.Variador.SetpointHz, v.SetpointHz);
                await VfdRealAsync(i, DbLayout.Variador.RampaS, v.RampaS);
            }
        }

        public void Dispose()
        {
            _plc?.Close();
            _gate.Dispose();
        }
    }
}
