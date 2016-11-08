namespace LED_Strip_Controller
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.timerMusic = new System.Windows.Forms.Timer(this.components);
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.gbPort = new System.Windows.Forms.GroupBox();
            this.cbSerialPort = new System.Windows.Forms.ComboBox();
            this.gbMusic = new System.Windows.Forms.GroupBox();
            this.cbPatternMusic = new System.Windows.Forms.ComboBox();
            this.rbMusiqueDroite = new System.Windows.Forms.RadioButton();
            this.rbMusiqueGauche = new System.Windows.Forms.RadioButton();
            this.lOffset = new System.Windows.Forms.Label();
            this.nbOffset = new System.Windows.Forms.NumericUpDown();
            this.lValeurSon = new System.Windows.Forms.Label();
            this.lNiveauSon = new System.Windows.Forms.Label();
            this.pbMusic = new System.Windows.Forms.ProgressBar();
            this.cbPerifSon = new System.Windows.Forms.ComboBox();
            this.gbCouleur = new System.Windows.Forms.GroupBox();
            this.bCouleur = new System.Windows.Forms.Button();
            this.cdCouleur = new System.Windows.Forms.ColorDialog();
            this.timerAmbilightGlobal = new System.Windows.Forms.Timer(this.components);
            this.gbMode = new System.Windows.Forms.GroupBox();
            this.rbModeAdrilight = new System.Windows.Forms.RadioButton();
            this.rbModeGlobalAmbilight = new System.Windows.Forms.RadioButton();
            this.rbModeCouleur = new System.Windows.Forms.RadioButton();
            this.rbModeOff = new System.Windows.Forms.RadioButton();
            this.rbModeMusique = new System.Windows.Forms.RadioButton();
            this.timerCouleur = new System.Windows.Forms.Timer(this.components);
            this.gbAdrilight = new System.Windows.Forms.GroupBox();
            this.bAdrilight = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.gbStart = new System.Windows.Forms.GroupBox();
            this.checkbMinimize = new System.Windows.Forms.CheckBox();
            this.checkbAutoStart = new System.Windows.Forms.CheckBox();
            this.gbPort.SuspendLayout();
            this.gbMusic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbOffset)).BeginInit();
            this.gbCouleur.SuspendLayout();
            this.gbMode.SuspendLayout();
            this.gbAdrilight.SuspendLayout();
            this.gbStart.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerMusic
            // 
            this.timerMusic.Interval = 25;
            this.timerMusic.Tick += new System.EventHandler(this.timerMusic_Tick);
            // 
            // serialPort
            // 
            this.serialPort.BaudRate = 1000000;
            // 
            // gbPort
            // 
            this.gbPort.Controls.Add(this.cbSerialPort);
            this.gbPort.Location = new System.Drawing.Point(12, 67);
            this.gbPort.Name = "gbPort";
            this.gbPort.Size = new System.Drawing.Size(200, 53);
            this.gbPort.TabIndex = 0;
            this.gbPort.TabStop = false;
            this.gbPort.Text = "Port COM";
            // 
            // cbSerialPort
            // 
            this.cbSerialPort.FormattingEnabled = true;
            this.cbSerialPort.Location = new System.Drawing.Point(7, 20);
            this.cbSerialPort.Name = "cbSerialPort";
            this.cbSerialPort.Size = new System.Drawing.Size(187, 21);
            this.cbSerialPort.TabIndex = 0;
            this.cbSerialPort.SelectedIndexChanged += new System.EventHandler(this.cbSerialPort_SelectedIndexChanged);
            // 
            // gbMusic
            // 
            this.gbMusic.Controls.Add(this.cbPatternMusic);
            this.gbMusic.Controls.Add(this.rbMusiqueDroite);
            this.gbMusic.Controls.Add(this.rbMusiqueGauche);
            this.gbMusic.Controls.Add(this.lOffset);
            this.gbMusic.Controls.Add(this.nbOffset);
            this.gbMusic.Controls.Add(this.lValeurSon);
            this.gbMusic.Controls.Add(this.lNiveauSon);
            this.gbMusic.Controls.Add(this.pbMusic);
            this.gbMusic.Controls.Add(this.cbPerifSon);
            this.gbMusic.Location = new System.Drawing.Point(12, 127);
            this.gbMusic.Name = "gbMusic";
            this.gbMusic.Size = new System.Drawing.Size(200, 177);
            this.gbMusic.TabIndex = 1;
            this.gbMusic.TabStop = false;
            this.gbMusic.Text = "Musique";
            // 
            // cbPatternMusic
            // 
            this.cbPatternMusic.FormattingEnabled = true;
            this.cbPatternMusic.Location = new System.Drawing.Point(7, 149);
            this.cbPatternMusic.Name = "cbPatternMusic";
            this.cbPatternMusic.Size = new System.Drawing.Size(187, 21);
            this.cbPatternMusic.TabIndex = 9;
            this.cbPatternMusic.SelectedIndexChanged += new System.EventHandler(this.cbPatternMusic_SelectedIndexChanged);
            // 
            // rbMusiqueDroite
            // 
            this.rbMusiqueDroite.AutoSize = true;
            this.rbMusiqueDroite.Checked = true;
            this.rbMusiqueDroite.Location = new System.Drawing.Point(101, 116);
            this.rbMusiqueDroite.Name = "rbMusiqueDroite";
            this.rbMusiqueDroite.Size = new System.Drawing.Size(53, 17);
            this.rbMusiqueDroite.TabIndex = 8;
            this.rbMusiqueDroite.TabStop = true;
            this.rbMusiqueDroite.Text = "Droite";
            this.rbMusiqueDroite.UseVisualStyleBackColor = true;
            // 
            // rbMusiqueGauche
            // 
            this.rbMusiqueGauche.AutoSize = true;
            this.rbMusiqueGauche.Location = new System.Drawing.Point(10, 116);
            this.rbMusiqueGauche.Name = "rbMusiqueGauche";
            this.rbMusiqueGauche.Size = new System.Drawing.Size(63, 17);
            this.rbMusiqueGauche.TabIndex = 7;
            this.rbMusiqueGauche.Text = "Gauche";
            this.rbMusiqueGauche.UseVisualStyleBackColor = true;
            this.rbMusiqueGauche.CheckedChanged += new System.EventHandler(this.rbMusiqueGauche_CheckedChanged);
            // 
            // lOffset
            // 
            this.lOffset.AutoSize = true;
            this.lOffset.Location = new System.Drawing.Point(7, 19);
            this.lOffset.Name = "lOffset";
            this.lOffset.Size = new System.Drawing.Size(41, 13);
            this.lOffset.TabIndex = 6;
            this.lOffset.Text = "Offset :";
            // 
            // nbOffset
            // 
            this.nbOffset.Location = new System.Drawing.Point(63, 17);
            this.nbOffset.Name = "nbOffset";
            this.nbOffset.Size = new System.Drawing.Size(59, 20);
            this.nbOffset.TabIndex = 5;
            this.nbOffset.ValueChanged += new System.EventHandler(this.nbOffset_ValueChanged);
            // 
            // lValeurSon
            // 
            this.lValeurSon.AutoSize = true;
            this.lValeurSon.Location = new System.Drawing.Point(60, 90);
            this.lValeurSon.Name = "lValeurSon";
            this.lValeurSon.Size = new System.Drawing.Size(0, 13);
            this.lValeurSon.TabIndex = 3;
            // 
            // lNiveauSon
            // 
            this.lNiveauSon.AutoSize = true;
            this.lNiveauSon.Location = new System.Drawing.Point(7, 90);
            this.lNiveauSon.Name = "lNiveauSon";
            this.lNiveauSon.Size = new System.Drawing.Size(47, 13);
            this.lNiveauSon.TabIndex = 2;
            this.lNiveauSon.Text = "Niveau :";
            // 
            // pbMusic
            // 
            this.pbMusic.Location = new System.Drawing.Point(7, 71);
            this.pbMusic.Name = "pbMusic";
            this.pbMusic.Size = new System.Drawing.Size(187, 12);
            this.pbMusic.TabIndex = 1;
            // 
            // cbPerifSon
            // 
            this.cbPerifSon.FormattingEnabled = true;
            this.cbPerifSon.Location = new System.Drawing.Point(7, 43);
            this.cbPerifSon.Name = "cbPerifSon";
            this.cbPerifSon.Size = new System.Drawing.Size(187, 21);
            this.cbPerifSon.TabIndex = 0;
            this.cbPerifSon.SelectedIndexChanged += new System.EventHandler(this.cbPerifSon_SelectedIndexChanged);
            // 
            // gbCouleur
            // 
            this.gbCouleur.Controls.Add(this.bCouleur);
            this.gbCouleur.Location = new System.Drawing.Point(218, 67);
            this.gbCouleur.Name = "gbCouleur";
            this.gbCouleur.Size = new System.Drawing.Size(200, 53);
            this.gbCouleur.TabIndex = 2;
            this.gbCouleur.TabStop = false;
            this.gbCouleur.Text = "Couleur";
            // 
            // bCouleur
            // 
            this.bCouleur.Location = new System.Drawing.Point(10, 20);
            this.bCouleur.Name = "bCouleur";
            this.bCouleur.Size = new System.Drawing.Size(184, 21);
            this.bCouleur.TabIndex = 0;
            this.bCouleur.Text = "Couleur";
            this.bCouleur.UseVisualStyleBackColor = true;
            this.bCouleur.Click += new System.EventHandler(this.bCouleur_Click);
            // 
            // timerAmbilightGlobal
            // 
            this.timerAmbilightGlobal.Interval = 200;
            this.timerAmbilightGlobal.Tick += new System.EventHandler(this.timerAmbilightGlobal_Tick);
            // 
            // gbMode
            // 
            this.gbMode.Controls.Add(this.rbModeAdrilight);
            this.gbMode.Controls.Add(this.rbModeGlobalAmbilight);
            this.gbMode.Controls.Add(this.rbModeCouleur);
            this.gbMode.Controls.Add(this.rbModeOff);
            this.gbMode.Controls.Add(this.rbModeMusique);
            this.gbMode.Location = new System.Drawing.Point(12, 13);
            this.gbMode.Name = "gbMode";
            this.gbMode.Size = new System.Drawing.Size(399, 48);
            this.gbMode.TabIndex = 5;
            this.gbMode.TabStop = false;
            this.gbMode.Text = "Mode";
            // 
            // rbModeAdrilight
            // 
            this.rbModeAdrilight.AutoSize = true;
            this.rbModeAdrilight.Location = new System.Drawing.Point(312, 19);
            this.rbModeAdrilight.Name = "rbModeAdrilight";
            this.rbModeAdrilight.Size = new System.Drawing.Size(62, 17);
            this.rbModeAdrilight.TabIndex = 4;
            this.rbModeAdrilight.Text = "Adrilight";
            this.rbModeAdrilight.UseVisualStyleBackColor = true;
            // 
            // rbModeGlobalAmbilight
            // 
            this.rbModeGlobalAmbilight.AutoSize = true;
            this.rbModeGlobalAmbilight.Location = new System.Drawing.Point(206, 19);
            this.rbModeGlobalAmbilight.Name = "rbModeGlobalAmbilight";
            this.rbModeGlobalAmbilight.Size = new System.Drawing.Size(100, 17);
            this.rbModeGlobalAmbilight.TabIndex = 3;
            this.rbModeGlobalAmbilight.Text = "Ambilight Global";
            this.rbModeGlobalAmbilight.UseVisualStyleBackColor = true;
            // 
            // rbModeCouleur
            // 
            this.rbModeCouleur.AutoSize = true;
            this.rbModeCouleur.Location = new System.Drawing.Point(139, 19);
            this.rbModeCouleur.Name = "rbModeCouleur";
            this.rbModeCouleur.Size = new System.Drawing.Size(61, 17);
            this.rbModeCouleur.TabIndex = 2;
            this.rbModeCouleur.Text = "Couleur";
            this.rbModeCouleur.UseVisualStyleBackColor = true;
            // 
            // rbModeOff
            // 
            this.rbModeOff.AutoSize = true;
            this.rbModeOff.Checked = true;
            this.rbModeOff.Location = new System.Drawing.Point(10, 19);
            this.rbModeOff.Name = "rbModeOff";
            this.rbModeOff.Size = new System.Drawing.Size(39, 17);
            this.rbModeOff.TabIndex = 1;
            this.rbModeOff.TabStop = true;
            this.rbModeOff.Text = "Off";
            this.rbModeOff.UseVisualStyleBackColor = true;
            // 
            // rbModeMusique
            // 
            this.rbModeMusique.AutoSize = true;
            this.rbModeMusique.Location = new System.Drawing.Point(63, 19);
            this.rbModeMusique.Name = "rbModeMusique";
            this.rbModeMusique.Size = new System.Drawing.Size(65, 17);
            this.rbModeMusique.TabIndex = 0;
            this.rbModeMusique.Text = "Musique";
            this.rbModeMusique.UseVisualStyleBackColor = true;
            // 
            // timerCouleur
            // 
            this.timerCouleur.Interval = 1000;
            this.timerCouleur.Tick += new System.EventHandler(this.timerCouleur_Tick);
            // 
            // gbAdrilight
            // 
            this.gbAdrilight.Controls.Add(this.bAdrilight);
            this.gbAdrilight.Location = new System.Drawing.Point(219, 127);
            this.gbAdrilight.Name = "gbAdrilight";
            this.gbAdrilight.Size = new System.Drawing.Size(200, 53);
            this.gbAdrilight.TabIndex = 6;
            this.gbAdrilight.TabStop = false;
            this.gbAdrilight.Text = "Adrilight";
            // 
            // bAdrilight
            // 
            this.bAdrilight.Location = new System.Drawing.Point(10, 20);
            this.bAdrilight.Name = "bAdrilight";
            this.bAdrilight.Size = new System.Drawing.Size(184, 21);
            this.bAdrilight.TabIndex = 0;
            this.bAdrilight.Text = "Adrilight.exe";
            this.bAdrilight.UseVisualStyleBackColor = true;
            this.bAdrilight.Click += new System.EventHandler(this.bAdrilight_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "LED Strip Controller";
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // gbStart
            // 
            this.gbStart.Controls.Add(this.checkbMinimize);
            this.gbStart.Controls.Add(this.checkbAutoStart);
            this.gbStart.Location = new System.Drawing.Point(220, 182);
            this.gbStart.Name = "gbStart";
            this.gbStart.Size = new System.Drawing.Size(200, 53);
            this.gbStart.TabIndex = 7;
            this.gbStart.TabStop = false;
            this.gbStart.Text = "Paramètres";
            // 
            // checkbMinimize
            // 
            this.checkbMinimize.AutoSize = true;
            this.checkbMinimize.Location = new System.Drawing.Point(95, 19);
            this.checkbMinimize.Name = "checkbMinimize";
            this.checkbMinimize.Size = new System.Drawing.Size(96, 17);
            this.checkbMinimize.TabIndex = 1;
            this.checkbMinimize.Text = "Start minimized";
            this.checkbMinimize.UseVisualStyleBackColor = true;
            this.checkbMinimize.CheckedChanged += new System.EventHandler(this.checkbMinimize_CheckedChanged);
            // 
            // checkbAutoStart
            // 
            this.checkbAutoStart.AutoSize = true;
            this.checkbAutoStart.Location = new System.Drawing.Point(9, 19);
            this.checkbAutoStart.Name = "checkbAutoStart";
            this.checkbAutoStart.Size = new System.Drawing.Size(70, 17);
            this.checkbAutoStart.TabIndex = 0;
            this.checkbAutoStart.Text = "AutoStart";
            this.checkbAutoStart.UseVisualStyleBackColor = true;
            this.checkbAutoStart.CheckedChanged += new System.EventHandler(this.checkbAutoStart_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 316);
            this.Controls.Add(this.gbStart);
            this.Controls.Add(this.gbAdrilight);
            this.Controls.Add(this.gbMode);
            this.Controls.Add(this.gbCouleur);
            this.Controls.Add(this.gbMusic);
            this.Controls.Add(this.gbPort);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(419, 355);
            this.Name = "MainForm";
            this.Text = "LED Strip Controller";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.gbPort.ResumeLayout(false);
            this.gbMusic.ResumeLayout(false);
            this.gbMusic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbOffset)).EndInit();
            this.gbCouleur.ResumeLayout(false);
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            this.gbAdrilight.ResumeLayout(false);
            this.gbStart.ResumeLayout(false);
            this.gbStart.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerMusic;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.GroupBox gbPort;
        private System.Windows.Forms.ComboBox cbSerialPort;
        private System.Windows.Forms.GroupBox gbMusic;
        private System.Windows.Forms.Label lValeurSon;
        private System.Windows.Forms.Label lNiveauSon;
        private System.Windows.Forms.ProgressBar pbMusic;
        private System.Windows.Forms.ComboBox cbPerifSon;
        private System.Windows.Forms.GroupBox gbCouleur;
        private System.Windows.Forms.ColorDialog cdCouleur;
        private System.Windows.Forms.Button bCouleur;
        private System.Windows.Forms.Label lOffset;
        private System.Windows.Forms.NumericUpDown nbOffset;
        private System.Windows.Forms.RadioButton rbMusiqueDroite;
        private System.Windows.Forms.RadioButton rbMusiqueGauche;
        private System.Windows.Forms.Timer timerAmbilightGlobal;
        private System.Windows.Forms.ComboBox cbPatternMusic;
        private System.Windows.Forms.GroupBox gbMode;
        private System.Windows.Forms.RadioButton rbModeAdrilight;
        private System.Windows.Forms.RadioButton rbModeGlobalAmbilight;
        private System.Windows.Forms.RadioButton rbModeCouleur;
        private System.Windows.Forms.RadioButton rbModeOff;
        private System.Windows.Forms.RadioButton rbModeMusique;
        private System.Windows.Forms.Timer timerCouleur;
        private System.Windows.Forms.GroupBox gbAdrilight;
        private System.Windows.Forms.Button bAdrilight;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.GroupBox gbStart;
        private System.Windows.Forms.CheckBox checkbMinimize;
        private System.Windows.Forms.CheckBox checkbAutoStart;
    }
}

