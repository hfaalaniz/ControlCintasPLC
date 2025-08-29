using System;
using System.Windows.Forms;
using S7.Net;
using System.Threading.Tasks;

namespace ControlCintasPLC
{
    public partial class Form1 : Form
    {
        private Plc plc;
        private bool enMarcha = false; // Estado de marcha/parada

        public Form1()
        {
            InitializeComponent();
            txtIP.Text = "192.168.1.11"; // IP de la CPU
            txtPort.Text = "102"; // Puerto S7
            txtDelay.Text = "1000"; // Delay inicial en ms (1 segundo)
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                plc = new Plc(CpuType.S71200, txtIP.Text, int.Parse(txtPort.Text), 0, 1); // Rack 0, Slot 1
                plc.Open();
                if (plc.IsConnected)
                {
                    timerLectura.Start();
                    MessageBox.Show("Conectado al PLC!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}");
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            try
            {
                timerLectura.Stop();
                plc.Close();
                MessageBox.Show("Desconectado del PLC!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al desconectar: {ex.Message}");
            }
        }

        private void timerLectura_Tick(object sender, EventArgs e)
        {
            try
            {
                bool cinta1 = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 0));
                bool cinta2 = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 1));
                bool cinta3 = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 2));
                lblCinta1.Text = cinta1 ? "Encendida" : "Apagada";
                lblCinta2.Text = cinta2 ? "Encendida" : "Apagada";
                lblCinta3.Text = cinta3 ? "Encendida" : "Apagada";

                bool q0_0 = Convert.ToBoolean(plc.Read(DataType.Output, 0, 0, VarType.Bit, 1));
                bool q0_1 = Convert.ToBoolean(plc.Read(DataType.Output, 0, 1, VarType.Bit, 1));
                bool q0_2 = Convert.ToBoolean(plc.Read(DataType.Output, 0, 2, VarType.Bit, 1));
                stCinta1.Text = $"Q0.0 = {q0_0}";
                stCinta2.Text = $"Q0.1 = {q0_1}";
                stCinta3.Text = $"Q0.2 = {q0_2}";

                bool i0_3 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 3, VarType.Bit, 1));
                bool i0_4 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 4, VarType.Bit, 1));
                bool i0_5 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 5, VarType.Bit, 1));
                bool i1_1 = Convert.ToBoolean(plc.Read(DataType.Input, 1, 1, VarType.Bit, 1));
                stCinta1.Text = $"I0.3 = {i0_3}";
                stCinta2.Text = $"I0.4 = {i0_4}";
                stCinta3.Text = $"I0.5 = {i0_5}";
                lblEmergencia.Text = $"I1.1 = {i1_1}";

                // Leer entradas analógicas AI4 x RTD (asumiendo direcciones IW64, IW66, IW68, IW70)
                ushort ai1 = (ushort)plc.Read(DataType.Input, 0, 64, VarType.Word, 1);
                ushort ai2 = (ushort)plc.Read(DataType.Input, 0, 66, VarType.Word, 1);
                ushort ai3 = (ushort)plc.Read(DataType.Input, 0, 68, VarType.Word, 1);
                ushort ai4 = (ushort)plc.Read(DataType.Input, 0, 70, VarType.Word, 1);
                lblAI1.Text = ai1.ToString();
                lblAI2.Text = ai2.ToString();
                lblAI3.Text = ai3.ToString();
                lblAI4.Text = ai4.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer: {ex.Message}");
            }
        }

        private async void btnMarchaParada_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtDelay.Text, out int delay) || delay < 0)
                {
                    MessageBox.Show("Ingrese un retraso válido (número positivo en ms)");
                    return;
                }

                if (!enMarcha)
                {
                    // Arranque: Cinta 3 → Cinta 2 → Cinta 1
                    btnMarchaParada.Text = "Parada";
                    enMarcha = true;
                    plc.Write(DataType.Input, 0, 5, false); // %I0.5
                    plc.Write(DataType.Input, 0, 4, false); // %I0.4
                    plc.Write(DataType.Input, 0, 3, false); // %I0.3
                    plc.Write(DataType.Input, 1, 1, false); // %I1.1

                    plc.Write(DataType.DataBlock, 1, 0, true, 2); // Cinta3_On
                    await Task.Delay(delay);
                    plc.Write(DataType.DataBlock, 1, 0, true, 1); // Cinta2_On
                    await Task.Delay(delay);
                    plc.Write(DataType.DataBlock, 1, 0, true, 0); // Cinta1_On
                    MessageBox.Show("Arranque automático completado");
                }
                else
                {
                    // Parada: Cinta 1 → Cinta 2 → Cinta 3
                    btnMarchaParada.Text = "Marcha";
                    enMarcha = false;
                    plc.Write(DataType.DataBlock, 1, 0, false, 0); // Cinta1_On
                    await Task.Delay(delay);
                    plc.Write(DataType.DataBlock, 1, 0, false, 1); // Cinta2_On
                    await Task.Delay(delay);
                    plc.Write(DataType.DataBlock, 1, 0, false, 2); // Cinta3_On
                    MessageBox.Show("Parada automática completada");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en marcha/parada: {ex.Message}");
            }
        }

        private void btnCinta1_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 0));
                bool newState = !currentState;
                plc.Write(DataType.DataBlock, 1, 0, newState, 0);
                MessageBox.Show($"Cinta1_On cambiado a: {newState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al escribir Cinta1: {ex.Message}");
            }
        }

        private void btnCinta2_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 1));
                bool newState = !currentState;
                plc.Write(DataType.DataBlock, 1, 0, newState, 1);
                MessageBox.Show($"Cinta2_On cambiado a: {newState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al escribir Cinta2: {ex.Message}");
            }
        }

        private void btnCinta3_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 2));
                bool newState = !currentState;
                plc.Write(DataType.DataBlock, 1, 0, newState, 2);
                MessageBox.Show($"Cinta3_On cambiado a: {newState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al escribir Cinta3: {ex.Message}");
            }
        }

        private void btnEmergencia_Click(object sender, EventArgs e)
        {
            try
            {
                plc.Write(DataType.Input, 1, 1, true); // Forzar %I1.1
                MessageBox.Show("Emergencia activada");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al activar emergencia: {ex.Message}");
            }
        }

        private void btnDesactivarEmergencia_Click(object sender, EventArgs e)
        {
            try
            {
                plc.Write(DataType.Input, 1, 1, false); // Desactivar %I1.1
                MessageBox.Show("Emergencia desactivada");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al desactivar emergencia: {ex.Message}");
            }
        }

        private void btnSensorCinta1_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 3, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 3, !currentState); // Toggle %I0.3
                MessageBox.Show($"Sensor Cinta1 (%I0.3) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.3: {ex.Message}");
            }
        }

        private void btnSensorCinta2_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 4, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 4, !currentState); // Toggle %I0.4
                MessageBox.Show($"Sensor Cinta2 (%I0.4) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.4: {ex.Message}");
            }
        }

        private void btnSensorCinta3_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 5, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 5, !currentState); // Toggle %I0.5
                MessageBox.Show($"Sensor Cinta3 (%I0.5) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.5: {ex.Message}");
            }
        }

        private void btnAplicarAQ_Click(object sender, EventArgs e)
        {
            try
            {
                plc.Write(DataType.Output, 0, 64, (ushort)numericUpDownAQ1.Value); // AQ1 %QW64
                plc.Write(DataType.Output, 0, 66, (ushort)numericUpDownAQ2.Value); // AQ2 %QW66
                plc.Write(DataType.Output, 0, 68, (ushort)numericUpDownAQ3.Value); // AQ3 %QW68
                plc.Write(DataType.Output, 0, 70, (ushort)numericUpDownAQ4.Value); // AQ4 %QW70
                MessageBox.Show("Salidas analógicas aplicadas");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aplicar salidas analógicas: {ex.Message}");
            }
        }
    }
}














/*using System;
using System.Windows.Forms;
using S7.Net;
using System.Threading.Tasks;

namespace ControlCintasPLC
{
    public partial class Form1 : Form
    {
        private Plc plc;
        private bool enMarcha = false; // Estado de marcha/parada

        public Form1()
        {
            InitializeComponent();
            txtIP.Text = "192.168.1.11"; // Ajustar según CPU o NetToPLCSim
            txtPort.Text = "102"; // Puerto S7
            txtDelay.Text = "1000"; // Retardo inicial en ms (1 segundo)
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                plc = new Plc(CpuType.S71200, txtIP.Text, int.Parse(txtPort.Text), 0, 1);
                plc.Open();
                if (plc.IsConnected)
                {
                    timerLectura.Start();
                    MessageBox.Show("Conectado al PLC!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}");
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            try
            {
                timerLectura.Stop();
                plc.Close();
                MessageBox.Show("Desconectado del PLC!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al desconectar: {ex.Message}");
            }
        }

        private void timerLectura_Tick(object sender, EventArgs e)
        {
            try
            {
                bool cinta1 = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 0));
                bool cinta2 = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 1));
                bool cinta3 = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 2));
                lblCinta1.Text = cinta1 ? "Encendida" : "Apagada";
                lblCinta2.Text = cinta2 ? "Encendida" : "Apagada";
                lblCinta3.Text = cinta3 ? "Encendida" : "Apagada";

                bool q0_0 = Convert.ToBoolean(plc.Read(DataType.Output, 0, 0, VarType.Bit, 1));
                bool q0_1 = Convert.ToBoolean(plc.Read(DataType.Output, 0, 1, VarType.Bit, 1));
                bool q0_2 = Convert.ToBoolean(plc.Read(DataType.Output, 0, 2, VarType.Bit, 1));
                stCinta1.Text = $"Q0.0 = {q0_0}";
                stCinta2.Text = $"Q0.1 = {q0_1}";
                stCinta3.Text = $"Q0.2 = {q0_2}";

                bool i0_0 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 0, VarType.Bit, 1));
                bool i0_1 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 1, VarType.Bit, 1));
                bool i0_2 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 2, VarType.Bit, 1));
                bool i0_3 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 3, VarType.Bit, 1));
                bool i0_4 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 4, VarType.Bit, 1));
                bool i0_5 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 5, VarType.Bit, 1));
                bool i0_6 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 6, VarType.Bit, 1));
                bool i0_7 = Convert.ToBoolean(plc.Read(DataType.Input, 0, 7, VarType.Bit, 1));
                bool i1_0 = Convert.ToBoolean(plc.Read(DataType.Input, 1, 0, VarType.Bit, 1));
                bool i1_1 = Convert.ToBoolean(plc.Read(DataType.Input, 1, 1, VarType.Bit, 1));
                lblSensorCinta1On.Text = $"I0.0 = {i0_0}";
                lblSensorCinta1Off.Text = $"I0.1 = {i0_1}";
                lblSensorCinta2On.Text = $"I0.2 = {i0_2}";
                lblSensorCinta2Off.Text = $"I0.3 = {i0_3}";
                lblSensorCinta3On.Text = $"I0.4 = {i0_4}";
                lblSensorCinta3Off.Text = $"I0.5 = {i0_5}";
                lblSensorCinta2Extra.Text = $"I0.6 = {i0_6}";
                lblSensorCinta2Cinta3.Text = $"I0.7 = {i0_7}";
                lblSensorCinta3Extra.Text = $"I1.0 = {i1_0}";
                lblEmergencia.Text = $"I1.1 = {i1_1}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer: {ex.Message}");
            }
        }

        private void btnCinta1_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 0));
                bool newState = !currentState;
                plc.Write(DataType.DataBlock, 1, 0, newState, 0);
                MessageBox.Show($"Cinta1_On cambiado a: {newState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al escribir Cinta1: {ex.Message}");
            }
        }

        private void btnCinta2_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 1));
                bool newState = !currentState;
                plc.Write(DataType.DataBlock, 1, 0, newState, 1);
                MessageBox.Show($"Cinta2_On cambiado a: {newState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al escribir Cinta2: {ex.Message}");
            }
        }

        private void btnCinta3_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 1, 2));
                bool newState = !currentState;
                plc.Write(DataType.DataBlock, 1, 0, newState, 2);
                MessageBox.Show($"Cinta3_On cambiado a: {newState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al escribir Cinta3: {ex.Message}");
            }
        }

        private void btnEmergencia_Click(object sender, EventArgs e)
        {
            try
            {
                plc.Write(DataType.Input, 1, 1, true);
                MessageBox.Show("Emergencia activada");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al activar emergencia: {ex.Message}");
            }
        }

        private void btnDesactivarEmergencia_Click(object sender, EventArgs e)
        {
            try
            {
                plc.Write(DataType.Input, 1, 1, false);
                MessageBox.Show("Emergencia desactivada");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al desactivar emergencia: {ex.Message}");
            }
        }

        private async void btnMarchaParada_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtDelay.Text, out int delayMs) || delayMs < 0)
                {
                    MessageBox.Show("Por favor, ingrese un valor válido para el retardo (ms).");
                    return;
                }

                if (!enMarcha)
                {
                    btnMarchaParada.Enabled = false;
                    plc.Write(DataType.DataBlock, 1, 0, true, 2); // Cinta3_On
                    await Task.Delay(delayMs);
                    plc.Write(DataType.DataBlock, 1, 0, true, 1); // Cinta2_On
                    await Task.Delay(delayMs);
                    plc.Write(DataType.DataBlock, 1, 0, true, 0); // Cinta1_On
                    enMarcha = true;
                    btnMarchaParada.Text = "Parada";
                    MessageBox.Show("Arranque automático completado");
                }
                else
                {
                    btnMarchaParada.Enabled = false;
                    plc.Write(DataType.DataBlock, 1, 0, false, 0); // Cinta1_On
                    await Task.Delay(delayMs);
                    plc.Write(DataType.DataBlock, 1, 0, false, 1); // Cinta2_On
                    await Task.Delay(delayMs);
                    plc.Write(DataType.DataBlock, 1, 0, false, 2); // Cinta3_On
                    enMarcha = false;
                    btnMarchaParada.Text = "Marcha";
                    MessageBox.Show("Parada automática completada");
                }
                btnMarchaParada.Enabled = true;
            }
            catch (Exception ex)
            {
                btnMarchaParada.Enabled = true;
                MessageBox.Show($"Error en arranque/parada: {ex.Message}");
            }
        }

        private void btnSensorCinta1On_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 0, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 0, !currentState);
                MessageBox.Show($"Sensor Cinta1_On (%I0.0) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.0: {ex.Message}");
            }
        }

        private void btnSensorCinta1Off_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 1, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 1, !currentState);
                MessageBox.Show($"Sensor Cinta1_Off (%I0.1) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.1: {ex.Message}");
            }
        }

        private void btnSensorCinta2On_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 2, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 2, !currentState);
                MessageBox.Show($"Sensor Cinta2_On (%I0.2) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.2: {ex.Message}");
            }
        }

        private void btnSensorCinta2Off_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 3, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 3, !currentState);
                MessageBox.Show($"Sensor Cinta2_Off (%I0.3) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.3: {ex.Message}");
            }
        }

        private void btnSensorCinta3On_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 4, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 4, !currentState);
                MessageBox.Show($"Sensor Cinta3_On (%I0.4) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.4: {ex.Message}");
            }
        }

        private void btnSensorCinta3Off_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 5, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 5, !currentState);
                MessageBox.Show($"Sensor Cinta3_Off (%I0.5) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.5: {ex.Message}");
            }
        }

        private void btnSensorCinta2Extra_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 6, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 6, !currentState);
                MessageBox.Show($"Sensor Extra Cinta2 (%I0.6) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.6: {ex.Message}");
            }
        }

        private void btnSensorCinta2Cinta3_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 0, 7, VarType.Bit, 1));
                plc.Write(DataType.Input, 0, 7, !currentState);
                MessageBox.Show($"Sensor Cinta2/Cinta3 (%I0.7) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I0.7: {ex.Message}");
            }
        }

        private void btnSensorCinta3Extra_Click(object sender, EventArgs e)
        {
            try
            {
                bool currentState = Convert.ToBoolean(plc.Read(DataType.Input, 1, 0, VarType.Bit, 1));
                plc.Write(DataType.Input, 1, 0, !currentState);
                MessageBox.Show($"Sensor Extra Cinta3 (%I1.0) cambiado a: {!currentState}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar %I1.0: {ex.Message}");
            }
        }

        private void btnActivarEntradas_Click(object sender, EventArgs e)
        {
            try
            {
                plc.Write(DataType.Input, 0, 0, true);
                plc.Write(DataType.Input, 0, 2, true);
                plc.Write(DataType.Input, 0, 4, true);
                plc.Write(DataType.Input, 0, 6, true);
                plc.Write(DataType.Input, 0, 7, true);
                plc.Write(DataType.Input, 0, 1, false);
                plc.Write(DataType.Input, 0, 3, false);
                plc.Write(DataType.Input, 0, 5, false);
                plc.Write(DataType.Input, 1, 0, false);
                plc.Write(DataType.Input, 1, 1, false);
                MessageBox.Show("Entradas forzadas para activar cintas");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al forzar entradas: {ex.Message}");
            }
        }
    }
}*/