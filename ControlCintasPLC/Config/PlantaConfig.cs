using System.Text.Json;
using ControlCintasPLC.Plc;

namespace ControlCintasPLC.Config
{
    public sealed class PlcConfig
    {
        public string Ip { get; set; } = "192.168.1.11";
        public int Puerto { get; set; } = 102;
        public short Rack { get; set; } = 0;
        public short Slot { get; set; } = 1;
        public int DbGeneral { get; set; } = 10;
        public int DbCintas { get; set; } = 11;
        public int DbTemperaturas { get; set; } = 12;
        public int DbVariadores { get; set; } = 13;
    }

    public sealed class CintaConfig
    {
        public string Nombre { get; set; } = "Cinta";
        public bool Habilitada { get; set; } = true;
        public short TiempoAtascoS { get; set; } = 10;
    }

    public sealed class TemperaturaConfig
    {
        public string Nombre { get; set; } = "Temperatura";
        public bool Habilitado { get; set; } = true;
        public float LimiteAviso { get; set; } = 70f;
        public float LimiteDisparo { get; set; } = 85f;
        /// <summary>0 = ninguna, 1..16 = cinta que se detiene en disparo.</summary>
        public byte CintaAsociada { get; set; }
        public float EscalaMaxima { get; set; } = 150f;
    }

    public sealed class VariadorConfig
    {
        public string Nombre { get; set; } = "Variador";
        /// <summary>0 = mando manual, 1..16 = arranca/para con la cinta indicada.</summary>
        public byte CintaAsociada { get; set; }
        public float FrecMaxHz { get; set; } = 50f;
        public float SetpointHz { get; set; } = 50f;
        public float RampaS { get; set; } = 5f;
    }

    public sealed class PlantaConfig
    {
        public PlcConfig Plc { get; set; } = new();
        public int IntervaloLecturaMs { get; set; } = 500;
        public int RetardoSecuenciaMs { get; set; } = 1000;
        public List<CintaConfig> Cintas { get; set; } = new();
        public List<TemperaturaConfig> Temperaturas { get; set; } = new();
        public List<VariadorConfig> Variadores { get; set; } = new();

        private static readonly JsonSerializerOptions JsonOpts = new() { WriteIndented = true };

        public static string RutaPorDefecto => Path.Combine(AppContext.BaseDirectory, "plantconfig.json");

        public static PlantaConfig Cargar(string ruta)
        {
            if (!File.Exists(ruta))
            {
                var def = PorDefecto();
                def.Guardar(ruta);
                return def;
            }
            var cfg = JsonSerializer.Deserialize<PlantaConfig>(File.ReadAllText(ruta), JsonOpts)
                      ?? throw new InvalidDataException("plantconfig.json vacío");
            cfg.Validar();
            return cfg;
        }

        public void Guardar(string ruta) => File.WriteAllText(ruta, JsonSerializer.Serialize(this, JsonOpts));

        public void Validar()
        {
            void Max(int n, string que)
            {
                if (n > DbLayout.MaxElementos)
                    throw new InvalidDataException($"{que}: {n} > máximo {DbLayout.MaxElementos} (ampliar arrays en PLC y DbLayout.MaxElementos)");
            }
            if (Cintas.Count == 0) throw new InvalidDataException("Debe haber al menos una cinta");
            Max(Cintas.Count, "Cintas");
            Max(Temperaturas.Count, "Temperaturas");
            Max(Variadores.Count, "Variadores");
            foreach (var t in Temperaturas)
                if (t.CintaAsociada > Cintas.Count)
                    throw new InvalidDataException($"{t.Nombre}: cinta asociada {t.CintaAsociada} no existe");
            foreach (var v in Variadores)
                if (v.CintaAsociada > Cintas.Count)
                    throw new InvalidDataException($"{v.Nombre}: cinta asociada {v.CintaAsociada} no existe");
        }

        public static PlantaConfig PorDefecto() => new()
        {
            Cintas = Enumerable.Range(1, 6).Select(i => new CintaConfig { Nombre = $"Cinta {i}" }).ToList(),
            Temperaturas = Enumerable.Range(1, 8).Select(i => new TemperaturaConfig
            {
                Nombre = i <= 6 ? $"Motor cinta {i}" : $"Ambiente {i - 6}",
                CintaAsociada = (byte)(i <= 6 ? i : 0)
            }).ToList(),
            Variadores = Enumerable.Range(1, 4).Select(i => new VariadorConfig
            {
                Nombre = $"VFD {i}",
                CintaAsociada = (byte)i
            }).ToList()
        };
    }
}
