using ControlCintasPLC.Config;
using ControlCintasPLC.Plc;
using static ControlCintasPLC.Plc.DbLayout;

namespace ControlCintasPLC.Controles
{
    public sealed class CintaControl : GroupBox
    {
        private readonly int _idx;
        private readonly PlcService _plc;
        private readonly Ejecutor _ejecutar;

        private readonly Led _ledMarcha, _ledFallo, _ledPermiso, _ledSensE, _ledSensS;
        private readonly Label _lblEstado, _lblContador;
        private readonly Button _btnMarcha, _btnReset, _btnSimE, _btnSimS;
        private readonly CheckBox _chkSecuencia;
        private CintaEstado? _estado;
        private bool _actualizando;

        public CintaControl(int idx, CintaConfig cfg, PlcService plc, Ejecutor ejecutar)
        {
            _idx = idx;
            _plc = plc;
            _ejecutar = ejecutar;

            Text = $"{idx + 1}. {cfg.Nombre}";
            Size = new Size(250, 190);
            Margin = new Padding(6);

            _ledMarcha = Ui.NuevoLed("Marcha", Color.LimeGreen, 10, 22);
            _ledFallo = Ui.NuevoLed("Fallo", Color.Red, 10, 42);
            _ledPermiso = Ui.NuevoLed("Permiso", Color.DodgerBlue, 10, 62);
            _ledSensE = Ui.NuevoLed("Sensor E", Color.Gold, 130, 22);
            _ledSensS = Ui.NuevoLed("Sensor S", Color.Gold, 130, 42);
            _lblContador = Ui.Etiqueta("Piezas: 0", 130, 62, 110);
            _lblEstado = Ui.Etiqueta("—", 10, 84, 230, 9f, true);

            _btnMarcha = Ui.Boton("Marcha", 10, 110, 75, 30);
            _btnReset = Ui.Boton("Reset", 90, 110, 60, 30);
            _chkSecuencia = new CheckBox { Text = "Secuencia", Location = new Point(158, 115), Size = new Size(85, 22) };

            _btnSimE = Ui.Boton("Sim E", 10, 148, 60, 26);
            _btnSimS = Ui.Boton("Sim S", 75, 148, 60, 26);

            _btnMarcha.Click += (_, _) => _ = _ejecutar(
                () => _plc.CintaBitAsync(_idx, Cinta.BitsA, Cinta.BitCmdOn, !(_estado?.CmdOn ?? false)),
                $"{Text}: marcha/paro");
            _btnReset.Click += (_, _) => _ = _ejecutar(
                () => _plc.CintaBitAsync(_idx, Cinta.BitsA, Cinta.BitCmdReset, true), $"{Text}: reset");
            _chkSecuencia.CheckedChanged += (_, _) =>
            {
                if (_actualizando) return;
                bool v = _chkSecuencia.Checked;
                _ = _ejecutar(() => _plc.CintaBitAsync(_idx, Cinta.BitsA, Cinta.BitHabilitada, v), $"{Text}: habilitar");
            };
            _btnSimE.Click += (_, _) => _ = _ejecutar(
                () => _plc.CintaBitAsync(_idx, Cinta.BitsB, Cinta.BitSimSensorEntrada, !(_estado?.SimSensorEntrada ?? false)),
                $"{Text}: simular sensor entrada");
            _btnSimS.Click += (_, _) => _ = _ejecutar(
                () => _plc.CintaBitAsync(_idx, Cinta.BitsB, Cinta.BitSimSensorSalida, !(_estado?.SimSensorSalida ?? false)),
                $"{Text}: simular sensor salida");

            Controls.AddRange(new Control[]
            {
                _ledMarcha, _ledFallo, _ledPermiso, _ledSensE, _ledSensS, _lblContador, _lblEstado,
                _btnMarcha, _btnReset, _chkSecuencia, _btnSimE, _btnSimS
            });
            Actualizar(null, false);
        }

        public void Actualizar(CintaEstado? e, bool simulacion)
        {
            _estado = e;
            _btnSimE.Visible = _btnSimS.Visible = simulacion;
            if (e is null)
            {
                _lblEstado.Text = "Sin datos";
                _lblEstado.ForeColor = Color.Gray;
                return;
            }

            _ledMarcha.On = e.Running;
            _ledFallo.On = e.Fault;
            _ledPermiso.On = e.Permiso;
            _ledSensE.On = e.SensorEntrada;
            _ledSensS.On = e.SensorSalida;
            _lblContador.Text = $"Piezas: {e.Contador}";
            _btnMarcha.Text = e.CmdOn ? "Paro" : "Marcha";
            _btnSimE.BackColor = e.SimSensorEntrada ? Color.Gold : SystemColors.Control;
            _btnSimS.BackColor = e.SimSensorSalida ? Color.Gold : SystemColors.Control;

            _actualizando = true;
            _chkSecuencia.Checked = e.Habilitada;
            _actualizando = false;

            (_lblEstado.Text, _lblEstado.ForeColor) = e switch
            {
                { Atasco: true } => ($"ATASCO (>{e.TiempoAtascoS} s)", Color.Red),
                { Termico: true } => ("DISPARO TÉRMICO", Color.Red),
                { Fault: true } => ("FALLO (temp./variador)", Color.Red),
                { Running: true } => ("En marcha", Color.Green),
                { CmdOn: true, Permiso: false } => ("Esperando cinta aguas abajo", Color.DarkOrange),
                _ => ("Parada", Color.Black)
            };
        }
    }
}
