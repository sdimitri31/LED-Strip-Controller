using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using Microsoft.Win32;

namespace LED_Strip_Controller
{

    public partial class MainForm : Form
    {
        private readonly byte[] _messagePreamble = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 };
        private const string appName = "LED Strip Controller";
        public const int numLed = 38;
        byte[] donnees = new byte[numLed * 3];
        byte[] donneesWithPreamble = new byte[(numLed * 3) + 10];
        Music musique;
        Ambilight ambilight;

        string adrilightPath = "";

        public void setDonnees(byte[] donnees) { this.donnees = donnees; }

        public ContextMenu contextMenuNotifyIcon = new ContextMenu();
        
        public MainForm()
        {
            InitializeComponent();
            contextMenuNotifyIcon.MenuItems.Add("Exit", (s, e) => Application.Exit());
            notifyIcon.ContextMenu = contextMenuNotifyIcon;

            checkbMinimize.Checked = Properties.Settings.Default.StartMinimized;
            checkbAutoStart.Checked = Properties.Settings.Default.AutoStart;

            for (int i = 0; i < donnees.Length; i++)
                donnees[i] = 0;

            //Port Série
            cbSerialPort.Items.AddRange(SerialPort.GetPortNames());
            cbSerialPort.SelectedItem = Properties.Settings.Default.LastCOM;
                        
            //Ambilight
            ambilight = new Ambilight(this);

            //Adrilight
            adrilightPath = Properties.Settings.Default.AdrilightPath;

            //Initialisation de la LED strip
            //serialPort.Open();
            Array.Copy(_messagePreamble, 0, donneesWithPreamble, 0, _messagePreamble.Length); //Copie du Preamble dans donnesWithPreamble
            //Array.Copy(donnees, 0, donneesWithPreamble, _messagePreamble.Length, donnees.Length); //Ajout des données après le Preamble
            //serialPort.Write(donneesWithPreamble, 0, donneesWithPreamble.Length);
            //serialPort.Close();

            //Musique
            musique = new Music(this);
            nbOffset.Value = musique.getOffset();

            if (musique.getSensMusique())
                rbMusiqueDroite.Checked = true;
            else
                rbMusiqueGauche.Checked = true;

            //Périphérique Son
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            cbPerifSon.Items.AddRange(devices.ToArray());
            int perif = Properties.Settings.Default.LastPerifSon;
            cbPerifSon.SelectedIndex = perif;

            //Pattern Music
            string[] patterns = { "Pattern 1 (Exp)", "Pattern 2", "Mirroir" };
            cbPatternMusic.Items.AddRange(patterns);
            cbPatternMusic.SelectedIndex = musique.getPattern();

            //Couleur
            cdCouleur.Color = Properties.Settings.Default.LastColor;
            bCouleur.BackColor = cdCouleur.Color;

            //Mode
            rbModeOff.CheckedChanged += new EventHandler(rbMode_CheckedChanged);
            rbModeMusique.CheckedChanged += new EventHandler(rbMode_CheckedChanged);
            rbModeCouleur.CheckedChanged += new EventHandler(rbMode_CheckedChanged);
            rbModeGlobalAmbilight.CheckedChanged += new EventHandler(rbMode_CheckedChanged);
            rbModeAdrilight.CheckedChanged += new EventHandler(rbMode_CheckedChanged);

            loadLastMode();

            if (Properties.Settings.Default.StartMinimized)
                this.WindowState = FormWindowState.Minimized;
        }

        //##############################################################################
        //                                 MainForm
        //##############################################################################

        private void loadLastMode()
        {
            string lastMode = Properties.Settings.Default.LastMode;

            switch (lastMode)
            {
                case "off":
                    rbModeOff.Checked = true;
                    break;

                case "musique":
                    rbModeMusique.Checked = true;
                    break;

                case "couleur":
                    rbModeCouleur.Checked = true;
                    break;

                case "ambilight":
                    rbModeGlobalAmbilight.Checked = true;
                    break;

                case "adrilight":
                    rbModeAdrilight.Checked = true;
                    break;
            }
        }

        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            string lastMode = "off";

            if (rbModeOff.Checked)
            {
                //Mode OFF on désactive les timers
                timerAmbilightGlobal.Enabled = false;
                timerMusic.Enabled = false;
                timerCouleur.Enabled = false;
                lastMode = "off";
            }
            else if (rbModeMusique.Checked)
            {
                stopAdrilight();
                //Mode Musique on active juste le timer Musique
                timerAmbilightGlobal.Enabled = false;
                timerMusic.Enabled = true;
                timerCouleur.Enabled = false;
                lastMode = "musique";
            }
            else if (rbModeCouleur.Checked)
            {
                stopAdrilight();
                //Mode Couleur on active juste le timer Couleur
                timerAmbilightGlobal.Enabled = false;
                timerMusic.Enabled = false;
                timerCouleur.Enabled = true;
                lastMode = "couleur";

                this.bCouleur.BackColor = cdCouleur.Color;
                for (int i = 0; i < numLed; i++)
                {
                    donnees[0] = cdCouleur.Color.B;
                    donnees[1] = cdCouleur.Color.G;
                    donnees[2] = cdCouleur.Color.R;
                    RotateRight(donnees, 3);
                }

            }
            else if (rbModeGlobalAmbilight.Checked)
            {
                stopAdrilight();
                //Mode Global Ambilight on active juste le timer Ambilight Global
                timerAmbilightGlobal.Enabled = true;
                timerMusic.Enabled = false;
                timerCouleur.Enabled = false;
                lastMode = "ambilight";
            }
            else if (rbModeAdrilight.Checked)
            {
                //Mode Adrilight on désative les timer
                timerAmbilightGlobal.Enabled = false;
                timerMusic.Enabled = false;
                timerCouleur.Enabled = false;

                //Et on lance Adrilight
                startAdrilight();
                lastMode = "adrilight";
            }

            Properties.Settings.Default.LastMode = lastMode;
            Properties.Settings.Default.Save();

        }

        private void cbSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastCOM = cbSerialPort.SelectedItem.ToString();
            Properties.Settings.Default.Save();

            serialPort.PortName = cbSerialPort.SelectedItem.ToString();
        }


        //##############################################################################
        //                                 Musique
        //##############################################################################

        private void timerMusic_Tick(object sender, EventArgs e)
        {
            if (cbPerifSon.SelectedItem != null)
            {
                int value = musique.getVolumeValue();
                pbMusic.Value = value;
                lValeurSon.Text = value.ToString();

                //Ajout des données après le Preamble
                Array.Copy(musique.tickTimer(donnees), 0, donneesWithPreamble, _messagePreamble.Length, donnees.Length); 

                //Envoi des données
                serialPort.Open();
                serialPort.Write(donneesWithPreamble, 0, donneesWithPreamble.Length);
                serialPort.Close();
            }
        }
        
        private void cbPerifSon_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastPerifSon = cbPerifSon.SelectedIndex;
            Properties.Settings.Default.Save();

        }

        public MMDevice getCbPerifSonSelectedValue() { return (MMDevice)cbPerifSon.SelectedItem; }
        
        private void nbOffset_ValueChanged(object sender, EventArgs e)
        {
            musique.setOffset((int)nbOffset.Value);

            Properties.Settings.Default.Offset = musique.getOffset();
            Properties.Settings.Default.Save();
        }

        private void rbMusiqueGauche_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMusiqueGauche.Checked)
                musique.setSensMusique(false);
            else
                musique.setSensMusique(true);

            Properties.Settings.Default.SensMusique = musique.getSensMusique();
            Properties.Settings.Default.Save();
        }        

        private void cbPatternMusic_SelectedIndexChanged(object sender, EventArgs e)
        {
            musique.setPattern(cbPatternMusic.SelectedIndex);

            Properties.Settings.Default.Pattern = musique.getPattern();
            Properties.Settings.Default.Save();
        }


        //##############################################################################
        //                                 Couleur
        //##############################################################################

        private void bCouleur_Click(object sender, EventArgs e)
        {
            DialogResult result = cdCouleur.ShowDialog();

            if (result == DialogResult.OK)
            {
                rbModeCouleur.Checked = true;

                this.bCouleur.BackColor = cdCouleur.Color;
                for (int i = 0; i < numLed; i++)
                {
                    donnees[0] = cdCouleur.Color.B;
                    donnees[1] = cdCouleur.Color.G;
                    donnees[2] = cdCouleur.Color.R;
                    RotateRight(donnees, 3);
                }

                Properties.Settings.Default.LastColor = cdCouleur.Color;
                Properties.Settings.Default.Save();

            }
        }

        private void timerCouleur_Tick(object sender, EventArgs e)
        { 
            //Ajout des données après le Preamble
            Array.Copy(donnees, 0, donneesWithPreamble, _messagePreamble.Length, donnees.Length);
            serialPort.Open();
            serialPort.Write(donneesWithPreamble, 0, donneesWithPreamble.Length);
            serialPort.Close();
        }


        //##############################################################################
        //                           Global Ambilight
        //##############################################################################
        
        private void timerAmbilightGlobal_Tick(object sender, EventArgs e)
        {
            Color tmp = ambilight.getAverageColorSpot(0, 0, 1920, 1080);

            for (int j = 0; j < numLed; j++)
            {
                donnees[j * 3] = tmp.B;
                donnees[(j * 3) + 1] = tmp.G;
                donnees[(j * 3) + 2] = tmp.R;
            }

            //Ajout des données après le Preamble
            Array.Copy(donnees, 0, donneesWithPreamble, _messagePreamble.Length, donnees.Length);
            serialPort.Open();
            serialPort.Write(donneesWithPreamble, 0, donneesWithPreamble.Length);
            serialPort.Close();
        }


        //##############################################################################
        //                                  Adrilight
        //##############################################################################

        private void bAdrilight_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                adrilightPath = openFileDialog.FileName;

                Properties.Settings.Default.AdrilightPath = adrilightPath;
                Properties.Settings.Default.Save();
            }
        }

        private void startAdrilight()
        {
            if(!IsProcessOpen("adrilight"))
                Process.Start(adrilightPath);
        }

        private void stopAdrilight()
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("adrilight"))
                {
                    proc.Kill();
                    //MessageBox.Show(proc.ProcessName);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        public bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }
            return false;
        }

        //##############################################################################
        //                                  Tools
        //##############################################################################

        public static void RotateLeft<T>(T[] array, int places)
        {
            T[] temp = new T[places];
            Array.Copy(array, 0, temp, 0, places);
            Array.Copy(array, places, array, 0, array.Length - places);
            Array.Copy(temp, 0, array, array.Length - places, places);
        }

        public static void RotateRight<T>(T[] array, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if (count == 0)
                return;

            // If (count == array.Length) there is nothing to do.
            // So we need the remainder (count % array.Length):
            count %= array.Length;

            // Create a temp array to store the tail of the source array
            T[] tmp = new T[count];

            // Copy tail of the source array to the temp array
            Array.Copy(array, array.Length - count, tmp, 0, count);

            // Shift elements right in the source array
            Array.Copy(array, 0, array, count, array.Length - count);

            // Copy saved tail to the head of the source array
            Array.Copy(tmp, array, count);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon.Visible = false;
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void checkbAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (checkbAutoStart.Checked)
                rk.SetValue(appName, Application.ExecutablePath.ToString());
            else
                rk.DeleteValue(appName,false);

            Properties.Settings.Default.AutoStart = checkbAutoStart.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkbMinimize_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartMinimized = checkbMinimize.Checked;
            Properties.Settings.Default.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.StartMinimized)
                this.Hide();
        }
    }
}
