using static ControlCintasPLC.Plc.DbLayout;

namespace ControlCintasPLC.Plc
{
    public sealed record GeneralEstado(
        bool Emergencia, bool CmdEmergenciaHmi, bool Simulacion, bool SecuenciaActiva,
        int RetardoSecuenciaMs, int NumCintas, int PasoSecuencia)
    {
        public static GeneralEstado Parse(ReadOnlySpan<byte> b) => new(
            Bit(b, General.Bits, General.BitEmergencia),
            Bit(b, General.Bits, General.BitCmdEmergenciaHmi),
            Bit(b, General.Bits, General.BitSimulacion),
            Bit(b, General.Bits, General.BitSecuenciaActiva),
            DInt(b, General.RetardoSecuenciaMs),
            Int(b, General.NumCintas),
            Int(b, General.PasoSecuencia));
    }

    public sealed record CintaEstado(
        bool CmdOn, bool Running, bool Fault, bool SensorEntrada, bool SensorSalida, bool Atasco,
        bool Habilitada, bool SimSensorEntrada, bool SimSensorSalida, bool Permiso, bool Termico,
        int TiempoAtascoS, int Contador)
    {
        public static CintaEstado Parse(ReadOnlySpan<byte> b) => new(
            Bit(b, Cinta.BitsA, Cinta.BitCmdOn),
            Bit(b, Cinta.BitsA, Cinta.BitRunning),
            Bit(b, Cinta.BitsA, Cinta.BitFault),
            Bit(b, Cinta.BitsA, Cinta.BitSensorEntrada),
            Bit(b, Cinta.BitsA, Cinta.BitSensorSalida),
            Bit(b, Cinta.BitsA, Cinta.BitAtasco),
            Bit(b, Cinta.BitsA, Cinta.BitHabilitada),
            Bit(b, Cinta.BitsB, Cinta.BitSimSensorEntrada),
            Bit(b, Cinta.BitsB, Cinta.BitSimSensorSalida),
            Bit(b, Cinta.BitsB, Cinta.BitPermiso),
            Bit(b, Cinta.BitsB, Cinta.BitTermico),
            Int(b, Cinta.TiempoAtascoS),
            DInt(b, Cinta.Contador));
    }

    public sealed record TemperaturaEstado(
        short Raw, float Valor, float LimiteAviso, float LimiteDisparo,
        bool Aviso, bool Disparo, bool Rotura, bool Habilitado, int CintaAsociada)
    {
        public static TemperaturaEstado Parse(ReadOnlySpan<byte> b) => new(
            Int(b, Temperatura.Raw),
            Real(b, Temperatura.Valor),
            Real(b, Temperatura.LimiteAviso),
            Real(b, Temperatura.LimiteDisparo),
            Bit(b, Temperatura.Bits, Temperatura.BitAviso),
            Bit(b, Temperatura.Bits, Temperatura.BitDisparo),
            Bit(b, Temperatura.Bits, Temperatura.BitRotura),
            Bit(b, Temperatura.Bits, Temperatura.BitHabilitado),
            b[Temperatura.CintaAsociada]);
    }

    public sealed record VariadorEstado(
        bool CmdRun, bool CmdReverse, bool Running, bool Fault, bool Ready, bool Warning,
        int CintaAsociada, float SetpointHz, float ActualHz, float CorrienteA,
        float FrecMaxHz, float RampaS, int CodigoFallo)
    {
        public static VariadorEstado Parse(ReadOnlySpan<byte> b) => new(
            Bit(b, Variador.Bits, Variador.BitCmdRun),
            Bit(b, Variador.Bits, Variador.BitCmdReverse),
            Bit(b, Variador.Bits, Variador.BitRunning),
            Bit(b, Variador.Bits, Variador.BitFault),
            Bit(b, Variador.Bits, Variador.BitReady),
            Bit(b, Variador.Bits, Variador.BitWarning),
            b[Variador.CintaAsociada],
            Real(b, Variador.SetpointHz),
            Real(b, Variador.ActualHz),
            Real(b, Variador.CorrienteA),
            Real(b, Variador.FrecMaxHz),
            Real(b, Variador.RampaS),
            Int(b, Variador.CodigoFallo));
    }

    public sealed record PlantaSnapshot(
        GeneralEstado General,
        IReadOnlyList<CintaEstado> Cintas,
        IReadOnlyList<TemperaturaEstado> Temperaturas,
        IReadOnlyList<VariadorEstado> Variadores);
}
