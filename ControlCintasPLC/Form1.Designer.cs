namespace ControlCintasPLC
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _plc.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            grpConectar = new GroupBox();
            label1 = new Label();
            txtIP = new TextBox();
            label2 = new Label();
            txtPort = new TextBox();
            btnConectar = new Button();
            btnDesconectar = new Button();
            btnRecargarConfig = new Button();
            btnEnviarConfig = new Button();
            grpGeneral = new GroupBox();
            btnMarchaSecuencia = new Button();
            btnParadaSecuencia = new Button();
            label3 = new Label();
            numRetardo = new NumericUpDown();
            btnAplicarRetardo = new Button();
            btnEmergencia = new Button();
            btnDesactivarEmergencia = new Button();
            btnResetGeneral = new Button();
            chkSimulacion = new CheckBox();
            lblSecuencia = new Label();
            tabPlanta = new TabControl();
            tabCintas = new TabPage();
            flpCintas = new FlowLayoutPanel();
            tabTemperaturas = new TabPage();
            flpTemperaturas = new FlowLayoutPanel();
            tabVariadores = new TabPage();
            flpVariadores = new FlowLayoutPanel();
            stBar = new StatusStrip();
            stConexion = new ToolStripStatusLabel();
            stEmergencia = new ToolStripStatusLabel();
            stSecuencia = new ToolStripStatusLabel();
            stMensaje = new ToolStripStatusLabel();
            timerLectura = new System.Windows.Forms.Timer(components);
            grpConectar.SuspendLayout();
            grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRetardo).BeginInit();
            tabPlanta.SuspendLayout();
            tabCintas.SuspendLayout();
            tabTemperaturas.SuspendLayout();
            tabVariadores.SuspendLayout();
            stBar.SuspendLayout();
            SuspendLayout();
            //
            // grpConectar
            //
            grpConectar.Controls.Add(label1);
            grpConectar.Controls.Add(txtIP);
            grpConectar.Controls.Add(label2);
            grpConectar.Controls.Add(txtPort);
            grpConectar.Controls.Add(btnConectar);
            grpConectar.Controls.Add(btnDesconectar);
            grpConectar.Controls.Add(btnRecargarConfig);
            grpConectar.Controls.Add(btnEnviarConfig);
            grpConectar.Dock = DockStyle.Top;
            grpConectar.Name = "grpConectar";
            grpConectar.Size = new Size(1084, 58);
            grpConectar.TabIndex = 0;
            grpConectar.TabStop = false;
            grpConectar.Text = "Conexión al PLC S7-1200";
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(10, 26);
            label1.Name = "label1";
            label1.Text = "Dirección IP:";
            //
            // txtIP
            //
            txtIP.Location = new Point(90, 22);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(120, 23);
            txtIP.TabIndex = 1;
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new Point(222, 26);
            label2.Name = "label2";
            label2.Text = "Puerto:";
            //
            // txtPort
            //
            txtPort.Location = new Point(272, 22);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(55, 23);
            txtPort.TabIndex = 2;
            //
            // btnConectar
            //
            btnConectar.Location = new Point(340, 21);
            btnConectar.Name = "btnConectar";
            btnConectar.Size = new Size(90, 26);
            btnConectar.TabIndex = 3;
            btnConectar.Text = "Conectar";
            btnConectar.UseVisualStyleBackColor = true;
            btnConectar.Click += btnConectar_Click;
            //
            // btnDesconectar
            //
            btnDesconectar.Enabled = false;
            btnDesconectar.Location = new Point(436, 21);
            btnDesconectar.Name = "btnDesconectar";
            btnDesconectar.Size = new Size(90, 26);
            btnDesconectar.TabIndex = 4;
            btnDesconectar.Text = "Desconectar";
            btnDesconectar.UseVisualStyleBackColor = true;
            btnDesconectar.Click += btnDesconectar_Click;
            //
            // btnRecargarConfig
            //
            btnRecargarConfig.Location = new Point(560, 21);
            btnRecargarConfig.Name = "btnRecargarConfig";
            btnRecargarConfig.Size = new Size(130, 26);
            btnRecargarConfig.TabIndex = 5;
            btnRecargarConfig.Text = "Recargar config.";
            btnRecargarConfig.UseVisualStyleBackColor = true;
            btnRecargarConfig.Click += btnRecargarConfig_Click;
            //
            // btnEnviarConfig
            //
            btnEnviarConfig.Location = new Point(696, 21);
            btnEnviarConfig.Name = "btnEnviarConfig";
            btnEnviarConfig.Size = new Size(150, 26);
            btnEnviarConfig.TabIndex = 6;
            btnEnviarConfig.Text = "Enviar config. al PLC";
            btnEnviarConfig.UseVisualStyleBackColor = true;
            btnEnviarConfig.Click += btnEnviarConfig_Click;
            //
            // grpGeneral
            //
            grpGeneral.Controls.Add(btnMarchaSecuencia);
            grpGeneral.Controls.Add(btnParadaSecuencia);
            grpGeneral.Controls.Add(label3);
            grpGeneral.Controls.Add(numRetardo);
            grpGeneral.Controls.Add(btnAplicarRetardo);
            grpGeneral.Controls.Add(btnEmergencia);
            grpGeneral.Controls.Add(btnDesactivarEmergencia);
            grpGeneral.Controls.Add(btnResetGeneral);
            grpGeneral.Controls.Add(chkSimulacion);
            grpGeneral.Controls.Add(lblSecuencia);
            grpGeneral.Dock = DockStyle.Top;
            grpGeneral.Name = "grpGeneral";
            grpGeneral.Size = new Size(1084, 70);
            grpGeneral.TabIndex = 1;
            grpGeneral.TabStop = false;
            grpGeneral.Text = "Mando general";
            //
            // btnMarchaSecuencia
            //
            btnMarchaSecuencia.BackColor = Color.PaleGreen;
            btnMarchaSecuencia.Location = new Point(10, 22);
            btnMarchaSecuencia.Name = "btnMarchaSecuencia";
            btnMarchaSecuencia.Size = new Size(110, 38);
            btnMarchaSecuencia.TabIndex = 0;
            btnMarchaSecuencia.Text = "Marcha línea";
            btnMarchaSecuencia.UseVisualStyleBackColor = false;
            btnMarchaSecuencia.Click += btnMarchaSecuencia_Click;
            //
            // btnParadaSecuencia
            //
            btnParadaSecuencia.BackColor = Color.Khaki;
            btnParadaSecuencia.Location = new Point(126, 22);
            btnParadaSecuencia.Name = "btnParadaSecuencia";
            btnParadaSecuencia.Size = new Size(110, 38);
            btnParadaSecuencia.TabIndex = 1;
            btnParadaSecuencia.Text = "Parada línea";
            btnParadaSecuencia.UseVisualStyleBackColor = false;
            btnParadaSecuencia.Click += btnParadaSecuencia_Click;
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Location = new Point(248, 32);
            label3.Name = "label3";
            label3.Text = "Retardo (ms):";
            //
            // numRetardo
            //
            numRetardo.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numRetardo.Location = new Point(330, 29);
            numRetardo.Maximum = new decimal(new int[] { 600000, 0, 0, 0 });
            numRetardo.Name = "numRetardo";
            numRetardo.Size = new Size(80, 23);
            numRetardo.TabIndex = 2;
            //
            // btnAplicarRetardo
            //
            btnAplicarRetardo.Location = new Point(416, 28);
            btnAplicarRetardo.Name = "btnAplicarRetardo";
            btnAplicarRetardo.Size = new Size(60, 25);
            btnAplicarRetardo.TabIndex = 3;
            btnAplicarRetardo.Text = "Aplicar";
            btnAplicarRetardo.UseVisualStyleBackColor = true;
            btnAplicarRetardo.Click += btnAplicarRetardo_Click;
            //
            // btnEmergencia
            //
            btnEmergencia.BackColor = Color.Red;
            btnEmergencia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEmergencia.ForeColor = Color.White;
            btnEmergencia.Location = new Point(496, 22);
            btnEmergencia.Name = "btnEmergencia";
            btnEmergencia.Size = new Size(110, 38);
            btnEmergencia.TabIndex = 4;
            btnEmergencia.Text = "EMERGENCIA";
            btnEmergencia.UseVisualStyleBackColor = false;
            btnEmergencia.Click += btnEmergencia_Click;
            //
            // btnDesactivarEmergencia
            //
            btnDesactivarEmergencia.Location = new Point(612, 22);
            btnDesactivarEmergencia.Name = "btnDesactivarEmergencia";
            btnDesactivarEmergencia.Size = new Size(110, 38);
            btnDesactivarEmergencia.TabIndex = 5;
            btnDesactivarEmergencia.Text = "Rearmar emerg.";
            btnDesactivarEmergencia.UseVisualStyleBackColor = true;
            btnDesactivarEmergencia.Click += btnDesactivarEmergencia_Click;
            //
            // btnResetGeneral
            //
            btnResetGeneral.Location = new Point(728, 22);
            btnResetGeneral.Name = "btnResetGeneral";
            btnResetGeneral.Size = new Size(100, 38);
            btnResetGeneral.TabIndex = 6;
            btnResetGeneral.Text = "Reset fallos";
            btnResetGeneral.UseVisualStyleBackColor = true;
            btnResetGeneral.Click += btnResetGeneral_Click;
            //
            // chkSimulacion
            //
            chkSimulacion.AutoSize = true;
            chkSimulacion.Location = new Point(840, 31);
            chkSimulacion.Name = "chkSimulacion";
            chkSimulacion.TabIndex = 7;
            chkSimulacion.Text = "Simulación (PLCSIM)";
            chkSimulacion.UseVisualStyleBackColor = true;
            chkSimulacion.CheckedChanged += chkSimulacion_CheckedChanged;
            //
            // lblSecuencia
            //
            lblSecuencia.AutoSize = true;
            lblSecuencia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSecuencia.Location = new Point(980, 32);
            lblSecuencia.Name = "lblSecuencia";
            lblSecuencia.Text = "Línea: —";
            //
            // tabPlanta
            //
            tabPlanta.Controls.Add(tabCintas);
            tabPlanta.Controls.Add(tabTemperaturas);
            tabPlanta.Controls.Add(tabVariadores);
            tabPlanta.Dock = DockStyle.Fill;
            tabPlanta.Name = "tabPlanta";
            tabPlanta.SelectedIndex = 0;
            tabPlanta.TabIndex = 2;
            //
            // tabCintas
            //
            tabCintas.Controls.Add(flpCintas);
            tabCintas.Name = "tabCintas";
            tabCintas.Padding = new Padding(3);
            tabCintas.Text = "Cintas";
            tabCintas.UseVisualStyleBackColor = true;
            //
            // flpCintas
            //
            flpCintas.AutoScroll = true;
            flpCintas.Dock = DockStyle.Fill;
            flpCintas.Name = "flpCintas";
            //
            // tabTemperaturas
            //
            tabTemperaturas.Controls.Add(flpTemperaturas);
            tabTemperaturas.Name = "tabTemperaturas";
            tabTemperaturas.Padding = new Padding(3);
            tabTemperaturas.Text = "Temperaturas";
            tabTemperaturas.UseVisualStyleBackColor = true;
            //
            // flpTemperaturas
            //
            flpTemperaturas.AutoScroll = true;
            flpTemperaturas.Dock = DockStyle.Fill;
            flpTemperaturas.Name = "flpTemperaturas";
            //
            // tabVariadores
            //
            tabVariadores.Controls.Add(flpVariadores);
            tabVariadores.Name = "tabVariadores";
            tabVariadores.Padding = new Padding(3);
            tabVariadores.Text = "Variadores";
            tabVariadores.UseVisualStyleBackColor = true;
            //
            // flpVariadores
            //
            flpVariadores.AutoScroll = true;
            flpVariadores.Dock = DockStyle.Fill;
            flpVariadores.Name = "flpVariadores";
            //
            // stBar
            //
            stBar.Items.AddRange(new ToolStripItem[] { stConexion, stEmergencia, stSecuencia, stMensaje });
            stBar.Name = "stBar";
            stBar.TabIndex = 3;
            //
            // stConexion
            //
            stConexion.Name = "stConexion";
            stConexion.Text = "Desconectado";
            //
            // stEmergencia
            //
            stEmergencia.Name = "stEmergencia";
            stEmergencia.Text = "Emergencia: —";
            //
            // stSecuencia
            //
            stSecuencia.Name = "stSecuencia";
            stSecuencia.Text = "Paso: —";
            //
            // stMensaje
            //
            stMensaje.Name = "stMensaje";
            stMensaje.Spring = true;
            stMensaje.TextAlign = ContentAlignment.MiddleLeft;
            //
            // timerLectura
            //
            timerLectura.Interval = 500;
            timerLectura.Tick += timerLectura_Tick;
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 661);
            Controls.Add(tabPlanta);
            Controls.Add(grpGeneral);
            Controls.Add(grpConectar);
            Controls.Add(stBar);
            MinimumSize = new Size(900, 500);
            Name = "Form1";
            Text = "Control de Cintas Transportadoras con PLC";
            grpConectar.ResumeLayout(false);
            grpConectar.PerformLayout();
            grpGeneral.ResumeLayout(false);
            grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numRetardo).EndInit();
            tabPlanta.ResumeLayout(false);
            tabCintas.ResumeLayout(false);
            tabTemperaturas.ResumeLayout(false);
            tabVariadores.ResumeLayout(false);
            stBar.ResumeLayout(false);
            stBar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpConectar;
        private Label label1;
        private TextBox txtIP;
        private Label label2;
        private TextBox txtPort;
        private Button btnConectar;
        private Button btnDesconectar;
        private Button btnRecargarConfig;
        private Button btnEnviarConfig;
        private GroupBox grpGeneral;
        private Button btnMarchaSecuencia;
        private Button btnParadaSecuencia;
        private Label label3;
        private NumericUpDown numRetardo;
        private Button btnAplicarRetardo;
        private Button btnEmergencia;
        private Button btnDesactivarEmergencia;
        private Button btnResetGeneral;
        private CheckBox chkSimulacion;
        private Label lblSecuencia;
        private TabControl tabPlanta;
        private TabPage tabCintas;
        private FlowLayoutPanel flpCintas;
        private TabPage tabTemperaturas;
        private FlowLayoutPanel flpTemperaturas;
        private TabPage tabVariadores;
        private FlowLayoutPanel flpVariadores;
        private StatusStrip stBar;
        private ToolStripStatusLabel stConexion;
        private ToolStripStatusLabel stEmergencia;
        private ToolStripStatusLabel stSecuencia;
        private ToolStripStatusLabel stMensaje;
        private System.Windows.Forms.Timer timerLectura;
    }
}
