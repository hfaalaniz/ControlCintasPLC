using ControlCintasPLC.Config;
using ControlCintasPLC.Plc;
using static ControlCintasPLC.Plc.DbLayout;

namespace ControlCintasPLC.Controles
{
    public sealed class VariadorControl : GroupBox
    {
        private readonly int _idx;
        private readonly PlcService _plc;
        private readonly Ejecutor _ejecutar;

        private readonly Led _ledReady, _ledRun, _ledFault, _ledWarn;
        private readonly Label _lblHz, _lblCorriente, _lblModo;
        private readonly TrackBar _trkSetpoint;
        private readonly NumericUpDown _numSetpoint, _numRampa;
        private readonly Button _btnMarcha, _btnParo, _btnReset;
        private readonly CheckBox _chkInversion;
        private bool _actualizando, _consignaCargada;
        private float _frecMax;

        public VariadorControl(int idx, VariadorConfig cfg, PlcService plc, Ejecutor ejecutar)
        {
            _idx = idx;
            _plc = plc;
            _ejecutar = ejecutar;
            _frecMax = cfg.FrecMaxHz > 0 ? cfg.FrecMaxHz : 50f;

            Text = $"V{idx + 1}. {cfg.Nombre}";
            Size = new Size(270, 250);
            Margin = new Padding(6);

            _ledReady = Ui.NuevoLed("Listo", Color.DodgerBlue, 10, 22);
            _ledRun = Ui.NuevoLed("Marcha", Color.LimeGreen, 10, 42);
            _ledFault = Ui.NuevoLed("Fallo", Color.Red, 10, 62);
            _ledWarn = Ui.NuevoLed("Alarma", Color.Orange, 10, 82);
            _lblHz = Ui.Etiqueta("0.0 Hz", 110, 20, 150, 18f, true);
            _lblCorriente = Ui.Etiqueta("0.0 A", 110, 60, 150, 11f);
            _lblModo = Ui.Etiqueta("", 110, 84, 150);

            _trkSetpoint = new TrackBar
            {
                Location = new Point(10, 106),
                Size = new Size(250, 30),
                Maximum = (int)(_frecMax * 10),
                TickFrequency = (int)(_frecMax * 10 / 10),
                SmallChange = 5,
                LargeChange = 50
            };
            var lblSp = Ui.Etiqueta("Consigna Hz", 10, 142, 80);
            _numSetpoint = Ui.Numero(10, 162, 0, (decimal)_frecMax);
            var lblRampa = Ui.Etiqueta("Rampa s", 90, 142, 60);
            _numRampa = Ui.Numero(90, 162, 0.1m, 600m, 1, 60);
            var btnAplicar = Ui.Boton("Aplicar", 160, 161, 60, 25);

            _btnMarcha = Ui.Boton("Marcha", 10, 196, 70, 30);
            _btnParo = Ui.Boton("Paro", 85, 196, 60, 30);
            _btnReset = Ui.Boton("Reset", 150, 196, 55, 30);
            _chkInversion = new CheckBox { Text = "Inv.", Location = new Point(210, 201), Size = new Size(55, 22) };

            _numSetpoint.Value = Ui.Clamp(_numSetpoint, cfg.SetpointHz);
            _numRampa.Value = Ui.Clamp(_numRampa, cfg.RampaS);
            _trkSetpoint.Value = (int)(_numSetpoint.Value * 10);

            _trkSetpoint.Scroll += (_, _) => _numSetpoint.Value = Ui.Clamp(_numSetpoint, _trkSetpoint.Value / 10.0);
            _numSetpoint.ValueChanged += (_, _) =>
                _trkSetpoint.Value = Math.Clamp((int)(_numSetpoint.Value * 10), _trkSetpoint.Minimum, _trkSetpoint.Maximum);

            btnAplicar.Click += (_, _) =>
            {
                float sp = (float)_numSetpoint.Value, rampa = (float)_numRampa.Value;
                _ = _ejecutar(async () =>
                {
                    await _plc.VfdRealAsync(_idx, Variador.SetpointHz, sp);
                    await _plc.VfdRealAsync(_idx, Variador.RampaS, rampa);
                }, $"{Text}: consigna");
            };
            _btnMarcha.Click += (_, _) => _ = _ejecutar(async () =>
            {
                await _plc.VfdRealAsync(_idx, Variador.SetpointHz, (float)_numSetpoint.Value);
                await _plc.VfdBitAsync(_idx, Variador.BitCmdRun, true);
            }, $"{Text}: marcha");
            _btnParo.Click += (_, _) => _ = _ejecutar(
                () => _plc.VfdBitAsync(_idx, Variador.BitCmdRun, false), $"{Text}: paro");
            _btnReset.Click += (_, _) => _ = _ejecutar(
                () => _plc.VfdBitAsync(_idx, Variador.BitCmdReset, true), $"{Text}: reset");
            _chkInversion.CheckedChanged += (_, _) =>
            {
                if (_actualizando) return;
                bool v = _chkInversion.Checked;
                _ = _ejecutar(() => _plc.VfdBitAsync(_idx, Variador.BitCmdReverse, v), $"{Text}: sentido");
            };

            Controls.AddRange(new Control[]
            {
                _ledReady, _ledRun, _ledFault, _ledWarn, _lblHz, _lblCorriente, _lblModo, _trkSetpoint,
                lblSp, _numSetpoint, lblRampa, _numRampa, btnAplicar, _btnMarcha, _btnParo, _btnReset, _chkInversion
            });
            Actualizar(null);
        }

        public void Actualizar(VariadorEstado? e)
        {
            if (e is null)
            {
                _lblModo.Text = "Sin datos";
                return;
            }

            // Escalado real del PLC (puede diferir del JSON)
            if (e.FrecMaxHz > 0 && Math.Abs(e.FrecMaxHz - _frecMax) > 0.01f)
            {
                _frecMax = e.FrecMaxHz;
                _numSetpoint.Maximum = (decimal)_frecMax;
                _trkSetpoint.Maximum = (int)(_frecMax * 10);
                _trkSetpoint.TickFrequency = Math.Max(1, _trkSetpoint.Maximum / 10);
            }
            if (!_consignaCargada && e.FrecMaxHz > 0)
            {
                _numSetpoint.Value = Ui.Clamp(_numSetpoint, e.SetpointHz);
                if (e.RampaS > 0) _numRampa.Value = Ui.Clamp(_numRampa, e.RampaS);
                _consignaCargada = true;
            }

            _ledReady.On = e.Ready;
            _ledRun.On = e.Running;
            _ledFault.On = e.Fault;
            _ledWarn.On = e.Warning;
            _lblHz.Text = $"{e.ActualHz:0.0} Hz";
            _lblCorriente.Text = $"{e.CorrienteA:0.0} A" + (e.Fault && e.CodigoFallo != 0 ? $"  F{e.CodigoFallo}" : "");

            bool esclavo = e.CintaAsociada > 0;
            _lblModo.Text = esclavo ? $"Esclavo de cinta {e.CintaAsociada}" : (e.CmdRun ? "Manual: ON" : "Manual: OFF");
            _btnMarcha.Enabled = _btnParo.Enabled = !esclavo;
            _btnMarcha.BackColor = e.CmdRun ? Color.LightGreen : SystemColors.Control;

            _actualizando = true;
            _chkInversion.Checked = e.CmdReverse;
            _actualizando = false;
        }
    }
}
