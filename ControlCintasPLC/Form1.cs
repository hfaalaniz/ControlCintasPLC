using ControlCintasPLC.Config;
using ControlCintasPLC.Controles;
using ControlCintasPLC.Plc;
using static ControlCintasPLC.Plc.DbLayout;

namespace ControlCintasPLC
{
    public partial class Form1 : Form
    {
        private readonly PlcService _plc = new();
        private PlantaConfig _cfg = PlantaConfig.PorDefecto();

        private readonly List<CintaControl> _cintas = new();
        private readonly List<TemperaturaControl> _temps = new();
        private readonly List<VariadorControl> _vfds = new();

        private bool _leyendo;
        private bool _actualizando;
        private int _erroresSeguidos;
        private DateTime _ultimoReintento = DateTime.MinValue;

        public Form1()
        {
            InitializeComponent();
            CargarConfiguracion();
        }

        // ------------------------------------------------------------ Configuración
        private void CargarConfiguracion()
        {
            try
            {
                _cfg = PlantaConfig.Cargar(PlantaConfig.RutaPorDefecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en plantconfig.json: {ex.Message}\nSe usa la configuración por defecto.",
                    "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _cfg = PlantaConfig.PorDefecto();
            }

            txtIP.Text = _cfg.Plc.Ip;
            txtPort.Text = _cfg.Plc.Puerto.ToString();
            numRetardo.Value = Math.Clamp(_cfg.RetardoSecuenciaMs, 0, (int)numRetardo.Maximum);
            timerLectura.Interval = Math.Max(100, _cfg.IntervaloLecturaMs);
            ConstruirPaneles();
        }

        private void ConstruirPaneles()
        {
            foreach (var flp in new[] { flpCintas, flpTemperaturas, flpVariadores })
            {
                flp.SuspendLayout();
                foreach (Control c in flp.Controls) c.Dispose();
                flp.Controls.Clear();
            }
            _cintas.Clear();
            _temps.Clear();
            _vfds.Clear();

            Ejecutor ej = EjecutarAsync;
            for (int i = 0; i < _cfg.Cintas.Count; i++) _cintas.Add(new CintaControl(i, _cfg.Cintas[i], _plc, ej));
            for (int i = 0; i < _cfg.Temperaturas.Count; i++) _temps.Add(new TemperaturaControl(i, _cfg.Temperaturas[i], _plc, ej));
            for (int i = 0; i < _cfg.Variadores.Count; i++) _vfds.Add(new VariadorControl(i, _cfg.Variadores[i], _plc, ej));

            flpCintas.Controls.AddRange(_cintas.ToArray<Control>());
            flpTemperaturas.Controls.AddRange(_temps.ToArray<Control>());
            flpVariadores.Controls.AddRange(_vfds.ToArray<Control>());
            foreach (var flp in new[] { flpCintas, flpTemperaturas, flpVariadores }) flp.ResumeLayout();

            ActualizarTitulosTabs(null);
        }

        private void btnRecargarConfig_Click(object sender, EventArgs e) => CargarConfiguracion();

        private async void btnEnviarConfig_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Sobrescribir los parámetros del PLC con plantconfig.json?", "Enviar configuración",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _cfg.RetardoSecuenciaMs = (int)numRetardo.Value;
            await EjecutarAsync(() => _plc.EnviarConfiguracionAsync(_cfg), "configuración enviada");
            ConstruirPaneles(); // los controles recargan límites/consignas desde el PLC
        }

        // ------------------------------------------------------------ Conexión
        private async void btnConectar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text, out int puerto) || puerto is < 1 or > 65535)
            {
                MessageBox.Show("Puerto no válido");
                return;
            }

            _cfg.Plc.Ip = txtIP.Text.Trim();
            _cfg.Plc.Puerto = puerto;
            btnConectar.Enabled = false;
            stConexion.Text = "Conectando...";
            try
            {
                await _plc.ConectarAsync(_cfg.Plc);
                GuardarConfigSilencioso();
                _erroresSeguidos = 0;
                timerLectura.Start();
                btnDesconectar.Enabled = true;
                stConexion.Text = $"Conectado {_cfg.Plc.Ip}";
                stConexion.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                btnConectar.Enabled = true;
                stConexion.Text = "Desconectado";
                MessageBox.Show($"Error de conexión: {ex.Message}", "PLC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            timerLectura.Stop();
            try { _plc.Desconectar(); }
            catch (Exception ex) { stMensaje.Text = ex.Message; }

            btnConectar.Enabled = true;
            btnDesconectar.Enabled = false;
            stConexion.Text = "Desconectado";
            stConexion.ForeColor = SystemColors.ControlText;
            MostrarSnapshot(null);
        }

        private void GuardarConfigSilencioso()
        {
            try { _cfg.Guardar(PlantaConfig.RutaPorDefecto); }
            catch (Exception ex) { stMensaje.Text = $"No se pudo guardar config: {ex.Message}"; }
        }

        // ------------------------------------------------------------ Lectura cíclica
        private async void timerLectura_Tick(object sender, EventArgs e)
        {
            if (_leyendo) return;
            _leyendo = true;
            try
            {
                if (!_plc.IsConnected)
                {
                    await IntentarReconectarAsync();
                    return;
                }

                var s = await _plc.LeerAsync(_cfg.Cintas.Count, _cfg.Temperaturas.Count, _cfg.Variadores.Count);
                MostrarSnapshot(s);
                _erroresSeguidos = 0;
                stConexion.Text = $"Conectado {_cfg.Plc.Ip}";
                stConexion.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                _erroresSeguidos++;
                stConexion.Text = $"Error de lectura ({_erroresSeguidos})";
                stConexion.ForeColor = Color.Red;
                stMensaje.Text = $"{DateTime.Now:HH:mm:ss} {ex.Message}";
                if (_erroresSeguidos >= 3)
                {
                    MostrarSnapshot(null);
                    try { _plc.Desconectar(); } catch { /* se reintenta en el siguiente tick */ }
                }
            }
            finally
            {
                _leyendo = false;
            }
        }

        private async Task IntentarReconectarAsync()
        {
            if (DateTime.Now - _ultimoReintento < TimeSpan.FromSeconds(5)) return;
            _ultimoReintento = DateTime.Now;
            stConexion.Text = "Reconectando...";
            stConexion.ForeColor = Color.DarkOrange;
            try { await _plc.ConectarAsync(_cfg.Plc); }
            catch (Exception ex) { stMensaje.Text = $"{DateTime.Now:HH:mm:ss} Reconexión fallida: {ex.Message}"; }
        }

        private void MostrarSnapshot(PlantaSnapshot? s)
        {
            bool sim = s?.General.Simulacion ?? false;
            for (int i = 0; i < _cintas.Count; i++) _cintas[i].Actualizar(s?.Cintas[i], sim);
            for (int i = 0; i < _temps.Count; i++) _temps[i].Actualizar(s?.Temperaturas[i]);
            for (int i = 0; i < _vfds.Count; i++) _vfds[i].Actualizar(s?.Variadores[i]);
            ActualizarTitulosTabs(s);

            if (s is null)
            {
                stEmergencia.Text = "Emergencia: —";
                stEmergencia.BackColor = SystemColors.Control;
                stSecuencia.Text = "Paso: —";
                lblSecuencia.Text = "Línea: —";
                return;
            }

            var g = s.General;
            _actualizando = true;
            chkSimulacion.Checked = g.Simulacion;
            _actualizando = false;

            stEmergencia.Text = g.Emergencia ? "EMERGENCIA ACTIVA" : "Emergencia: OK";
            stEmergencia.BackColor = g.Emergencia ? Color.Red : Color.PaleGreen;
            stSecuencia.Text = g.PasoSecuencia > 0 ? $"Paso secuencia: cinta {g.PasoSecuencia}" : "Paso: —";

            int enMarcha = s.Cintas.Count(c => c.Running);
            (lblSecuencia.Text, lblSecuencia.ForeColor) = g switch
            {
                { Emergencia: true } => ("Línea: EMERGENCIA", Color.Red),
                { PasoSecuencia: > 0 } => ("Línea: en transición", Color.DarkOrange),
                { SecuenciaActiva: true } => ($"Línea: en marcha ({enMarcha}/{s.Cintas.Count})", Color.Green),
                _ => ($"Línea: parada ({enMarcha}/{s.Cintas.Count})", Color.Black)
            };
        }

        private void ActualizarTitulosTabs(PlantaSnapshot? s)
        {
            int fallosC = s?.Cintas.Count(c => c.Fault) ?? 0;
            int alarmasT = s?.Temperaturas.Count(t => t.Habilitado && (t.Aviso || t.Disparo || t.Rotura)) ?? 0;
            int fallosV = s?.Variadores.Count(v => v.Fault) ?? 0;
            tabCintas.Text = $"Cintas ({_cintas.Count})" + (fallosC > 0 ? $" ⚠{fallosC}" : "");
            tabTemperaturas.Text = $"Temperaturas ({_temps.Count})" + (alarmasT > 0 ? $" ⚠{alarmasT}" : "");
            tabVariadores.Text = $"Variadores ({_vfds.Count})" + (fallosV > 0 ? $" ⚠{fallosV}" : "");
        }

        // ------------------------------------------------------------ Mando general
        private async Task EjecutarAsync(Func<Task> accion, string descripcion)
        {
            if (!_plc.IsConnected)
            {
                stMensaje.Text = "No conectado al PLC";
                return;
            }
            try
            {
                await accion();
                stMensaje.Text = $"{DateTime.Now:HH:mm:ss} OK: {descripcion}";
            }
            catch (Exception ex)
            {
                stMensaje.Text = $"{DateTime.Now:HH:mm:ss} Error: {descripcion}";
                MessageBox.Show($"{descripcion}\n{ex.Message}", "Error PLC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnMarchaSecuencia_Click(object sender, EventArgs e) =>
            await EjecutarAsync(() => _plc.GeneralBitAsync(General.BitCmdMarchaSecuencia, true), "marcha secuencial");

        private async void btnParadaSecuencia_Click(object sender, EventArgs e) =>
            await EjecutarAsync(() => _plc.GeneralBitAsync(General.BitCmdParadaSecuencia, true), "parada secuencial");

        private async void btnAplicarRetardo_Click(object sender, EventArgs e)
        {
            int ms = (int)numRetardo.Value;
            _cfg.RetardoSecuenciaMs = ms;
            await EjecutarAsync(() => _plc.EscribirDIntAsync(_plc.Config.DbGeneral, General.RetardoSecuenciaMs, ms),
                $"retardo {ms} ms");
        }

        private async void btnEmergencia_Click(object sender, EventArgs e) =>
            await EjecutarAsync(() => _plc.GeneralBitAsync(General.BitCmdEmergenciaHmi, true), "emergencia activada");

        private async void btnDesactivarEmergencia_Click(object sender, EventArgs e) =>
            await EjecutarAsync(() => _plc.GeneralBitAsync(General.BitCmdEmergenciaHmi, false), "emergencia rearmada");

        private async void btnResetGeneral_Click(object sender, EventArgs e) =>
            await EjecutarAsync(() => _plc.GeneralBitAsync(General.BitCmdResetGeneral, true), "reset de fallos");

        private async void chkSimulacion_CheckedChanged(object sender, EventArgs e)
        {
            if (_actualizando) return;
            bool v = chkSimulacion.Checked;
            await EjecutarAsync(() => _plc.GeneralBitAsync(General.BitSimulacion, v), v ? "simulación ON" : "simulación OFF");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timerLectura.Stop();
            base.OnFormClosing(e);
        }
    }
}
