using ControlCintasPLC.Config;
using ControlCintasPLC.Plc;
using static ControlCintasPLC.Plc.DbLayout;

namespace ControlCintasPLC.Controles
{
    public sealed class TemperaturaControl : GroupBox
    {
        private readonly int _idx;
        private readonly float _escalaMax;
        private readonly PlcService _plc;
        private readonly Ejecutor _ejecutar;

        private readonly Label _lblValor, _lblEstado, _lblInfo;
        private readonly ProgressBar _barra;
        private readonly NumericUpDown _numAviso, _numDisparo;
        private readonly CheckBox _chkHabilitado;
        private bool _actualizando, _limitesCargados;

        public TemperaturaControl(int idx, TemperaturaConfig cfg, PlcService plc, Ejecutor ejecutar)
        {
            _idx = idx;
            _plc = plc;
            _ejecutar = ejecutar;
            _escalaMax = cfg.EscalaMaxima > 0 ? cfg.EscalaMaxima : 150f;

            Text = $"T{idx + 1}. {cfg.Nombre}";
            Size = new Size(230, 190);
            Margin = new Padding(6);

            _lblValor = Ui.Etiqueta("---.- °C", 10, 20, 140, 18f, true);
            _chkHabilitado = new CheckBox { Text = "Activo", Location = new Point(155, 26), Size = new Size(65, 22) };
            _barra = new ProgressBar { Location = new Point(10, 62), Size = new Size(210, 14), Maximum = 1000 };
            _lblEstado = Ui.Etiqueta("—", 10, 80, 210, 9f, true);
            _lblInfo = Ui.Etiqueta(cfg.CintaAsociada > 0 ? $"Detiene cinta {cfg.CintaAsociada}" : "Sólo indicación", 10, 100, 210);

            var lblA = Ui.Etiqueta("Aviso °C", 10, 128, 60);
            _numAviso = Ui.Numero(10, 150, -50, 850);
            var lblD = Ui.Etiqueta("Disparo °C", 85, 128, 70);
            _numDisparo = Ui.Numero(85, 150, -50, 850);
            var btnAplicar = Ui.Boton("Aplicar", 160, 149, 60, 25);

            _numAviso.Value = Ui.Clamp(_numAviso, cfg.LimiteAviso);
            _numDisparo.Value = Ui.Clamp(_numDisparo, cfg.LimiteDisparo);

            btnAplicar.Click += (_, _) =>
            {
                float a = (float)_numAviso.Value, d = (float)_numDisparo.Value;
                if (a >= d)
                {
                    MessageBox.Show("El límite de aviso debe ser menor que el de disparo.", Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _ = _ejecutar(async () =>
                {
                    await _plc.TempRealAsync(_idx, Temperatura.LimiteAviso, a);
                    await _plc.TempRealAsync(_idx, Temperatura.LimiteDisparo, d);
                }, $"{Text}: límites");
            };
            _chkHabilitado.CheckedChanged += (_, _) =>
            {
                if (_actualizando) return;
                bool v = _chkHabilitado.Checked;
                _ = _ejecutar(() => _plc.TempBitAsync(_idx, Temperatura.BitHabilitado, v), $"{Text}: habilitar");
            };

            Controls.AddRange(new Control[]
            {
                _lblValor, _chkHabilitado, _barra, _lblEstado, _lblInfo, lblA, _numAviso, lblD, _numDisparo, btnAplicar
            });
            Actualizar(null);
        }

        public void Actualizar(TemperaturaEstado? e)
        {
            if (e is null)
            {
                _lblEstado.Text = "Sin datos";
                _lblEstado.ForeColor = Color.Gray;
                return;
            }

            _actualizando = true;
            _chkHabilitado.Checked = e.Habilitado;
            _actualizando = false;

            // Los límites del PLC se cargan una vez; luego manda el operador
            if (!_limitesCargados && e.LimiteDisparo > 0)
            {
                _numAviso.Value = Ui.Clamp(_numAviso, e.LimiteAviso);
                _numDisparo.Value = Ui.Clamp(_numDisparo, e.LimiteDisparo);
                _limitesCargados = true;
            }

            if (!e.Habilitado)
            {
                _lblValor.Text = "---.- °C";
                _barra.Value = 0;
                (_lblEstado.Text, _lblEstado.ForeColor) = ("Deshabilitado", Color.Gray);
                return;
            }

            _lblValor.Text = e.Rotura ? "ROTURA" : $"{e.Valor,6:0.0} °C";
            _barra.Value = (int)Math.Clamp(e.Valor / _escalaMax * 1000f, 0f, 1000f);

            (_lblEstado.Text, _lblEstado.ForeColor) = e switch
            {
                { Rotura: true } => ("Rotura de sensor / fuera de rango", Color.Red),
                { Disparo: true } => ($"DISPARO (≥ {e.LimiteDisparo:0.#} °C)", Color.Red),
                { Aviso: true } => ($"Aviso (≥ {e.LimiteAviso:0.#} °C)", Color.DarkOrange),
                _ => ("Normal", Color.Green)
            };
            _lblValor.ForeColor = _lblEstado.ForeColor;
        }
    }
}
