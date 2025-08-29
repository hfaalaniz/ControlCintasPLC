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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            btnConectar = new Button();
            txtIP = new TextBox();
            txtPort = new TextBox();
            btnCinta1_ON = new Button();
            btnCinta2_ON = new Button();
            btnCinta3_ON = new Button();
            grpControlCintas = new GroupBox();
            txtDelay = new TextBox();
            label3 = new Label();
            lblCinta3 = new Label();
            lblCinta2 = new Label();
            lblCinta1 = new Label();
            btnMarchaParada = new Button();
            btnAplicarAQ = new Button();
            btnDesactivarEmergencia = new Button();
            btnEmergencia = new Button();
            grpConectar = new GroupBox();
            label2 = new Label();
            label1 = new Label();
            btnDesconectar = new Button();
            timerLectura = new System.Windows.Forms.Timer(components);
            stBar = new StatusStrip();
            stCinta1 = new ToolStripStatusLabel();
            stCinta2 = new ToolStripStatusLabel();
            stCinta3 = new ToolStripStatusLabel();
            lblAI1 = new ToolStripStatusLabel();
            lblAI2 = new ToolStripStatusLabel();
            lblAI3 = new ToolStripStatusLabel();
            lblAI4 = new ToolStripStatusLabel();
            lblSensorCinta3On = new ToolStripStatusLabel();
            lblSensorCinta3Off = new ToolStripStatusLabel();
            lblSensorCinta2Extra = new ToolStripStatusLabel();
            lblSensorCinta2Cinta3 = new ToolStripStatusLabel();
            lblSensorCinta3Extra = new ToolStripStatusLabel();
            lblEmergencia = new ToolStripStatusLabel();
            btnSensorCinta1 = new Button();
            btnSensorCinta2 = new Button();
            btnSensorCinta3 = new Button();
            groupBox1 = new GroupBox();
            numericUpDownAQ4 = new NumericUpDown();
            numericUpDownAQ3 = new NumericUpDown();
            numericUpDownAQ2 = new NumericUpDown();
            numericUpDownAQ1 = new NumericUpDown();
            grpControlCintas.SuspendLayout();
            grpConectar.SuspendLayout();
            stBar.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownAQ4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownAQ3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownAQ2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownAQ1).BeginInit();
            SuspendLayout();
            // 
            // btnConectar
            // 
            btnConectar.Location = new Point(447, 22);
            btnConectar.Name = "btnConectar";
            btnConectar.Size = new Size(75, 23);
            btnConectar.TabIndex = 0;
            btnConectar.Text = "Conectar";
            btnConectar.UseVisualStyleBackColor = true;
            btnConectar.Click += btnConectar_Click;
            // 
            // txtIP
            // 
            txtIP.Location = new Point(102, 22);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(148, 23);
            txtIP.TabIndex = 1;
            txtIP.Text = "192.168.1.11";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(321, 22);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(100, 23);
            txtPort.TabIndex = 2;
            txtPort.Text = "102";
            // 
            // btnCinta1_ON
            // 
            btnCinta1_ON.Location = new Point(8, 46);
            btnCinta1_ON.Name = "btnCinta1_ON";
            btnCinta1_ON.Size = new Size(108, 54);
            btnCinta1_ON.TabIndex = 0;
            btnCinta1_ON.Text = "Encender Cinta 1";
            btnCinta1_ON.UseVisualStyleBackColor = true;
            btnCinta1_ON.Click += btnCinta1_Click;
            // 
            // btnCinta2_ON
            // 
            btnCinta2_ON.Location = new Point(122, 46);
            btnCinta2_ON.Name = "btnCinta2_ON";
            btnCinta2_ON.Size = new Size(108, 54);
            btnCinta2_ON.TabIndex = 0;
            btnCinta2_ON.Text = "Encender Cinta 2";
            btnCinta2_ON.UseVisualStyleBackColor = true;
            btnCinta2_ON.Click += btnCinta2_Click;
            // 
            // btnCinta3_ON
            // 
            btnCinta3_ON.Location = new Point(236, 46);
            btnCinta3_ON.Name = "btnCinta3_ON";
            btnCinta3_ON.Size = new Size(108, 54);
            btnCinta3_ON.TabIndex = 0;
            btnCinta3_ON.Text = "Encender Cinta 3";
            btnCinta3_ON.UseVisualStyleBackColor = true;
            btnCinta3_ON.Click += btnCinta3_Click;
            // 
            // grpControlCintas
            // 
            grpControlCintas.Controls.Add(txtDelay);
            grpControlCintas.Controls.Add(label3);
            grpControlCintas.Controls.Add(lblCinta3);
            grpControlCintas.Controls.Add(lblCinta2);
            grpControlCintas.Controls.Add(lblCinta1);
            grpControlCintas.Controls.Add(btnMarchaParada);
            grpControlCintas.Controls.Add(btnAplicarAQ);
            grpControlCintas.Controls.Add(btnCinta1_ON);
            grpControlCintas.Controls.Add(btnCinta2_ON);
            grpControlCintas.Controls.Add(btnDesactivarEmergencia);
            grpControlCintas.Controls.Add(btnEmergencia);
            grpControlCintas.Controls.Add(btnCinta3_ON);
            grpControlCintas.Location = new Point(12, 81);
            grpControlCintas.Name = "grpControlCintas";
            grpControlCintas.Size = new Size(675, 112);
            grpControlCintas.TabIndex = 3;
            grpControlCintas.TabStop = false;
            grpControlCintas.Text = "Envío de comandos al PLC";
            // 
            // txtDelay
            // 
            txtDelay.Location = new Point(456, 48);
            txtDelay.Name = "txtDelay";
            txtDelay.Size = new Size(48, 23);
            txtDelay.TabIndex = 2;
            txtDelay.Text = "102";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(456, 25);
            label3.Name = "label3";
            label3.Size = new Size(47, 15);
            label3.TabIndex = 1;
            label3.Text = "Tiempo";
            // 
            // lblCinta3
            // 
            lblCinta3.AutoSize = true;
            lblCinta3.Location = new Point(246, 25);
            lblCinta3.Name = "lblCinta3";
            lblCinta3.Size = new Size(86, 15);
            lblCinta3.TabIndex = 1;
            lblCinta3.Text = "Cinta 3: Parada";
            // 
            // lblCinta2
            // 
            lblCinta2.AutoSize = true;
            lblCinta2.Location = new Point(134, 25);
            lblCinta2.Name = "lblCinta2";
            lblCinta2.Size = new Size(86, 15);
            lblCinta2.TabIndex = 1;
            lblCinta2.Text = "Cinta 2: Parada";
            // 
            // lblCinta1
            // 
            lblCinta1.AutoSize = true;
            lblCinta1.Location = new Point(18, 25);
            lblCinta1.Name = "lblCinta1";
            lblCinta1.Size = new Size(86, 15);
            lblCinta1.TabIndex = 1;
            lblCinta1.Text = "Cinta 1: Parada";
            // 
            // btnMarchaParada
            // 
            btnMarchaParada.Location = new Point(350, 76);
            btnMarchaParada.Name = "btnMarchaParada";
            btnMarchaParada.Size = new Size(100, 24);
            btnMarchaParada.TabIndex = 0;
            btnMarchaParada.Text = "Marcha";
            btnMarchaParada.UseVisualStyleBackColor = true;
            btnMarchaParada.Click += btnMarchaParada_Click;
            // 
            // btnAplicarAQ
            // 
            btnAplicarAQ.Location = new Point(350, 46);
            btnAplicarAQ.Name = "btnAplicarAQ";
            btnAplicarAQ.Size = new Size(100, 24);
            btnAplicarAQ.TabIndex = 0;
            btnAplicarAQ.Text = "Aplicar AQ";
            btnAplicarAQ.UseVisualStyleBackColor = true;
            // 
            // btnDesactivarEmergencia
            // 
            btnDesactivarEmergencia.BackColor = Color.Red;
            btnDesactivarEmergencia.ForeColor = SystemColors.ControlText;
            btnDesactivarEmergencia.Location = new Point(587, 46);
            btnDesactivarEmergencia.Name = "btnDesactivarEmergencia";
            btnDesactivarEmergencia.Size = new Size(85, 54);
            btnDesactivarEmergencia.TabIndex = 0;
            btnDesactivarEmergencia.Text = "Desactivar Emergencia";
            btnDesactivarEmergencia.UseVisualStyleBackColor = false;
            btnDesactivarEmergencia.Click += btnDesactivarEmergencia_Click;
            // 
            // btnEmergencia
            // 
            btnEmergencia.BackColor = Color.Red;
            btnEmergencia.ForeColor = SystemColors.ControlText;
            btnEmergencia.Location = new Point(507, 46);
            btnEmergencia.Name = "btnEmergencia";
            btnEmergencia.Size = new Size(77, 54);
            btnEmergencia.TabIndex = 0;
            btnEmergencia.Text = "Activar Emergencia";
            btnEmergencia.UseVisualStyleBackColor = false;
            btnEmergencia.Click += btnEmergencia_Click;
            // 
            // grpConectar
            // 
            grpConectar.Controls.Add(txtPort);
            grpConectar.Controls.Add(txtIP);
            grpConectar.Controls.Add(label2);
            grpConectar.Controls.Add(label1);
            grpConectar.Controls.Add(btnDesconectar);
            grpConectar.Controls.Add(btnConectar);
            grpConectar.Location = new Point(12, 12);
            grpConectar.Name = "grpConectar";
            grpConectar.Size = new Size(675, 63);
            grpConectar.TabIndex = 4;
            grpConectar.TabStop = false;
            grpConectar.Text = "Conectar al PLC";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(270, 26);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 1;
            label2.Text = "Puerto:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 26);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 1;
            label1.Text = "Dirección IP:";
            // 
            // btnDesconectar
            // 
            btnDesconectar.Location = new Point(528, 22);
            btnDesconectar.Name = "btnDesconectar";
            btnDesconectar.Size = new Size(100, 23);
            btnDesconectar.TabIndex = 0;
            btnDesconectar.Text = "Desconectar";
            btnDesconectar.UseVisualStyleBackColor = true;
            btnDesconectar.Click += btnDesconectar_Click;
            // 
            // timerLectura
            // 
            timerLectura.Tick += timerLectura_Tick;
            // 
            // stBar
            // 
            stBar.Items.AddRange(new ToolStripItem[] { stCinta1, stCinta2, stCinta3, lblAI1, lblAI2, lblAI3, lblAI4, lblSensorCinta3On, lblSensorCinta3Off, lblSensorCinta2Extra, lblSensorCinta2Cinta3, lblSensorCinta3Extra, lblEmergencia });
            stBar.Location = new Point(0, 302);
            stBar.Name = "stBar";
            stBar.Size = new Size(693, 22);
            stBar.TabIndex = 5;
            stBar.Text = "statusStrip1";
            // 
            // stCinta1
            // 
            stCinta1.Name = "stCinta1";
            stCinta1.Size = new Size(44, 17);
            stCinta1.Text = "Cinta 1";
            // 
            // stCinta2
            // 
            stCinta2.Name = "stCinta2";
            stCinta2.Size = new Size(44, 17);
            stCinta2.Text = "Cinta 2";
            // 
            // stCinta3
            // 
            stCinta3.Name = "stCinta3";
            stCinta3.Size = new Size(44, 17);
            stCinta3.Text = "Cinta 3";
            // 
            // lblAI1
            // 
            lblAI1.Name = "lblAI1";
            lblAI1.Size = new Size(51, 17);
            lblAI1.Text = "Sensor 1";
            // 
            // lblAI2
            // 
            lblAI2.Name = "lblAI2";
            lblAI2.Size = new Size(51, 17);
            lblAI2.Text = "Sensor 2";
            // 
            // lblAI3
            // 
            lblAI3.Name = "lblAI3";
            lblAI3.Size = new Size(51, 17);
            lblAI3.Text = "Sensor 3";
            // 
            // lblAI4
            // 
            lblAI4.Name = "lblAI4";
            lblAI4.Size = new Size(51, 17);
            lblAI4.Text = "Sensor 4";
            // 
            // lblSensorCinta3On
            // 
            lblSensorCinta3On.Name = "lblSensorCinta3On";
            lblSensorCinta3On.Size = new Size(51, 17);
            lblSensorCinta3On.Text = "Sensor 5";
            // 
            // lblSensorCinta3Off
            // 
            lblSensorCinta3Off.Name = "lblSensorCinta3Off";
            lblSensorCinta3Off.Size = new Size(51, 17);
            lblSensorCinta3Off.Text = "Sensor 6";
            // 
            // lblSensorCinta2Extra
            // 
            lblSensorCinta2Extra.Name = "lblSensorCinta2Extra";
            lblSensorCinta2Extra.Size = new Size(51, 17);
            lblSensorCinta2Extra.Text = "Sensor 7";
            // 
            // lblSensorCinta2Cinta3
            // 
            lblSensorCinta2Cinta3.Name = "lblSensorCinta2Cinta3";
            lblSensorCinta2Cinta3.Size = new Size(51, 17);
            lblSensorCinta2Cinta3.Text = "Sensor 8";
            // 
            // lblSensorCinta3Extra
            // 
            lblSensorCinta3Extra.Name = "lblSensorCinta3Extra";
            lblSensorCinta3Extra.Size = new Size(51, 17);
            lblSensorCinta3Extra.Text = "Sensor 9";
            // 
            // lblEmergencia
            // 
            lblEmergencia.Name = "lblEmergencia";
            lblEmergencia.Size = new Size(69, 17);
            lblEmergencia.Text = "Emergencia";
            // 
            // btnSensorCinta1
            // 
            btnSensorCinta1.BackColor = Color.OliveDrab;
            btnSensorCinta1.Location = new Point(5, 31);
            btnSensorCinta1.Name = "btnSensorCinta1";
            btnSensorCinta1.Size = new Size(63, 42);
            btnSensorCinta1.TabIndex = 0;
            btnSensorCinta1.Text = "Sensor Cinta 1";
            btnSensorCinta1.UseVisualStyleBackColor = false;
            // 
            // btnSensorCinta2
            // 
            btnSensorCinta2.BackColor = Color.IndianRed;
            btnSensorCinta2.Location = new Point(78, 31);
            btnSensorCinta2.Name = "btnSensorCinta2";
            btnSensorCinta2.Size = new Size(60, 42);
            btnSensorCinta2.TabIndex = 0;
            btnSensorCinta2.Text = "Sensor Cinta 2";
            btnSensorCinta2.UseVisualStyleBackColor = false;
            // 
            // btnSensorCinta3
            // 
            btnSensorCinta3.BackColor = Color.OliveDrab;
            btnSensorCinta3.Location = new Point(144, 31);
            btnSensorCinta3.Name = "btnSensorCinta3";
            btnSensorCinta3.Size = new Size(61, 42);
            btnSensorCinta3.TabIndex = 0;
            btnSensorCinta3.Text = "Sensor Cinta 3";
            btnSensorCinta3.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(numericUpDownAQ4);
            groupBox1.Controls.Add(numericUpDownAQ3);
            groupBox1.Controls.Add(numericUpDownAQ2);
            groupBox1.Controls.Add(numericUpDownAQ1);
            groupBox1.Controls.Add(btnSensorCinta3);
            groupBox1.Controls.Add(btnSensorCinta2);
            groupBox1.Controls.Add(btnSensorCinta1);
            groupBox1.Location = new Point(12, 199);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(675, 89);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Envío de comandos al PLC";
            // 
            // numericUpDownAQ4
            // 
            numericUpDownAQ4.Location = new Point(484, 43);
            numericUpDownAQ4.Name = "numericUpDownAQ4";
            numericUpDownAQ4.Size = new Size(85, 23);
            numericUpDownAQ4.TabIndex = 1;
            // 
            // numericUpDownAQ3
            // 
            numericUpDownAQ3.Location = new Point(393, 43);
            numericUpDownAQ3.Name = "numericUpDownAQ3";
            numericUpDownAQ3.Size = new Size(85, 23);
            numericUpDownAQ3.TabIndex = 1;
            // 
            // numericUpDownAQ2
            // 
            numericUpDownAQ2.Location = new Point(302, 43);
            numericUpDownAQ2.Name = "numericUpDownAQ2";
            numericUpDownAQ2.Size = new Size(85, 23);
            numericUpDownAQ2.TabIndex = 1;
            // 
            // numericUpDownAQ1
            // 
            numericUpDownAQ1.Location = new Point(211, 43);
            numericUpDownAQ1.Name = "numericUpDownAQ1";
            numericUpDownAQ1.Size = new Size(85, 23);
            numericUpDownAQ1.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(693, 324);
            Controls.Add(stBar);
            Controls.Add(grpConectar);
            Controls.Add(groupBox1);
            Controls.Add(grpControlCintas);
            Name = "Form1";
            Text = "Control de Cintas Transportadoras con PLC";
            grpControlCintas.ResumeLayout(false);
            grpControlCintas.PerformLayout();
            grpConectar.ResumeLayout(false);
            grpConectar.PerformLayout();
            stBar.ResumeLayout(false);
            stBar.PerformLayout();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDownAQ4).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownAQ3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownAQ2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownAQ1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnConectar;
        private TextBox txtIP;
        private TextBox txtPort;
        private Button btnCinta1_ON;
        private Button btnCinta2_ON;
        private Button btnCinta3_ON;
        private GroupBox grpControlCintas;
        private Label lblCinta3;
        private Label lblCinta2;
        private Label lblCinta1;
        private GroupBox grpConectar;
        private Label label2;
        private Label label1;
        private Button btnDesconectar;
        private System.Windows.Forms.Timer timerLectura;
        private Button btnEmergencia;
        private Button btnAplicarAQ;
        private StatusStrip stBar;
        private ToolStripStatusLabel stCinta1;
        private ToolStripStatusLabel stCinta2;
        private ToolStripStatusLabel stCinta3;
        private Button btnDesactivarEmergencia;
        private ToolStripStatusLabel lblAI1;
        private ToolStripStatusLabel lblAI2;
        private ToolStripStatusLabel lblAI3;
        private ToolStripStatusLabel lblAI4;
        private ToolStripStatusLabel lblSensorCinta3On;
        private ToolStripStatusLabel lblSensorCinta3Off;
        private ToolStripStatusLabel lblSensorCinta2Extra;
        private ToolStripStatusLabel lblSensorCinta2Cinta3;
        private ToolStripStatusLabel lblSensorCinta3Extra;
        private ToolStripStatusLabel lblEmergencia;
        private Button btnSensorCinta1;
        private Button btnSensorCinta2;
        private Button btnSensorCinta3;
        private GroupBox groupBox1;
        private TextBox txtDelay;
        private Label label3;
        private Button btnMarchaParada;
        private NumericUpDown numericUpDownAQ4;
        private NumericUpDown numericUpDownAQ3;
        private NumericUpDown numericUpDownAQ2;
        private NumericUpDown numericUpDownAQ1;
    }
}
