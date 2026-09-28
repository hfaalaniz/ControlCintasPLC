using System.Buffers.Binary;

namespace ControlCintasPLC.Plc
{
    /// <summary>
    /// Mapa de offsets de los DB de intercambio. Debe coincidir con PLC/01_UDTs.udt y PLC/02_DBs.db
    /// (DB con acceso NO optimizado).
    /// </summary>
    public static class DbLayout
    {
        public const int MaxElementos = 16;

        public static class General
        {
            public const int Size = 10;
            public const int Bits = 0;
            public const int BitEmergencia = 0;
            public const int BitCmdEmergenciaHmi = 1;
            public const int BitCmdResetGeneral = 2;
            public const int BitSimulacion = 3;
            public const int BitSecuenciaActiva = 4;
            public const int BitCmdMarchaSecuencia = 5;
            public const int BitCmdParadaSecuencia = 6;
            public const int RetardoSecuenciaMs = 2; // DInt
            public const int NumCintas = 6;          // Int
            public const int PasoSecuencia = 8;      // Int
        }

        public static class Cinta
        {
            public const int Size = 8;
            public const int BitsA = 0;
            public const int BitCmdOn = 0;
            public const int BitCmdReset = 1;
            public const int BitRunning = 2;
            public const int BitFault = 3;
            public const int BitSensorEntrada = 4;
            public const int BitSensorSalida = 5;
            public const int BitAtasco = 6;
            public const int BitHabilitada = 7;
            public const int BitsB = 1;
            public const int BitSimSensorEntrada = 0;
            public const int BitSimSensorSalida = 1;
            public const int BitPermiso = 2;
            public const int BitTermico = 3;
            public const int TiempoAtascoS = 2; // Int
            public const int Contador = 4;      // DInt
        }

        public static class Temperatura
        {
            public const int Size = 16;
            public const int Raw = 0;           // Int
            public const int Valor = 2;         // Real
            public const int LimiteAviso = 6;   // Real
            public const int LimiteDisparo = 10;// Real
            public const int Bits = 14;
            public const int BitAviso = 0;
            public const int BitDisparo = 1;
            public const int BitRotura = 2;
            public const int BitHabilitado = 3;
            public const int CintaAsociada = 15;// USInt
        }

        public static class Variador
        {
            public const int Size = 24;
            public const int Bits = 0;
            public const int BitCmdRun = 0;
            public const int BitCmdReverse = 1;
            public const int BitCmdReset = 2;
            public const int BitRunning = 3;
            public const int BitFault = 4;
            public const int BitReady = 5;
            public const int BitWarning = 6;
            public const int CintaAsociada = 1; // USInt
            public const int SetpointHz = 2;    // Real
            public const int ActualHz = 6;      // Real
            public const int CorrienteA = 10;   // Real
            public const int FrecMaxHz = 14;    // Real
            public const int RampaS = 18;       // Real
            public const int CodigoFallo = 22;  // Int
        }

        // ---- Decodificación big-endian (formato S7)
        public static bool Bit(ReadOnlySpan<byte> b, int adr, int bit) => (b[adr] & (1 << bit)) != 0;
        public static short Int(ReadOnlySpan<byte> b, int adr) => BinaryPrimitives.ReadInt16BigEndian(b[adr..]);
        public static int DInt(ReadOnlySpan<byte> b, int adr) => BinaryPrimitives.ReadInt32BigEndian(b[adr..]);
        public static float Real(ReadOnlySpan<byte> b, int adr) => BinaryPrimitives.ReadSingleBigEndian(b[adr..]);

        public static byte[] ToBytes(short v) { var r = new byte[2]; BinaryPrimitives.WriteInt16BigEndian(r, v); return r; }
        public static byte[] ToBytes(int v) { var r = new byte[4]; BinaryPrimitives.WriteInt32BigEndian(r, v); return r; }
        public static byte[] ToBytes(float v) { var r = new byte[4]; BinaryPrimitives.WriteSingleBigEndian(r, v); return r; }
    }
}
