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
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace LED_Strip_Controller
{

    public partial class MainForm : Form
    {
        private readonly byte[] _messagePreamble = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 };
        private const string appName = "LED Strip Controller";

        public int numLed;
        byte[] donnees;
        byte[] donneesWithPreamble;

        Music musique;
        Ambilight ambilight;
        public Analyzer analyser;

        string adrilightPath = "";

        public void setDonnees(byte[] donnees) { this.donnees = donnees; }

        public ContextMenu contextMenuNotifyIcon = new ContextMenu();
        
        public MainForm()
        {
            InitializeComponent();
            detecOtherInstance();

            contextMenuNotifyIcon.MenuItems.Add("Exit", (s, e) => Application.Exit());
            notifyIcon.ContextMenu = contextMenuNotifyIcon;

            checkbMinimize.Checked = Properties.Settings.Default.StartMinimized;
            checkbAutoStart.Checked = Properties.Settings.Default.AutoStart;

            nbLED.Value = Properties.Settings.Default.nbLED;

            numLed = (int)nbLED.Value;
            donnees = new byte[numLed * 3];
            donneesWithPreamble = new byte[(numLed * 3) + 10];

            for (int i = 0; i < donnees.Length; i++)
                donnees[i] = 0;

            //Port Série
            cbSerialPort.Items.AddRange(SerialPort.GetPortNames());
            cbSerialPort.SelectedItem = Properties.Settings.Default.LastCOM;
                        
            //Ambilight
            ambilight = new Ambilight(this);

            //Adrilight
            adrilightPath = Properties.Settings.Default.AdrilightPath;
            tbAdrilightPath.Text = adrilightPath;

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

            ////Périphérique Son
            //MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            //var devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            //cbPerifSon.Items.AddRange(devices.ToArray());
            //int perif = Properties.Settings.Default.LastPerifSon;
            //cbPerifSon.SelectedIndex = perif;

            //Analyser
            analyser = new Analyzer(cbPerifSon, spectrum1); 
            int perif = Properties.Settings.Default.LastPerifSon;
            cbPerifSon.SelectedIndex = perif;

            //Pattern Music
            string[] patterns = { "Pattern 1 (Exp)", "Pattern 2", "Mirroir", "Frequency", "Frequency Mirror" };
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

        private void detecOtherInstance()
        {
            // get the name of our process
                string proc=Process.GetCurrentProcess().ProcessName;
                // get the list of all processes by that name
                Process[] processes=Process.GetProcessesByName(proc);
                // if there is more than one process...
                if (processes.Length > 1)
                {

                // exit our process
                Process.GetCurrentProcess().Kill();
                return;
                }
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

            //Par défaut on désactive les timers
            string lastMode = "off";
            timerAmbilightGlobal.Enabled = false;
            timerMusic.Enabled = false;
            timerCouleur.Enabled = false;
            analyser.Enable = false;
            cbPerifSon.Enabled = true;

            if (rbModeOff.Checked)
            {
                //Mode OFF
                lastMode = "off";
                stopAdrilight();
            }
            else if (rbModeMusique.Checked)
            {
                //Mode Musique
                analyser.Enable = true;
                timerMusic.Enabled = true;
                cbPerifSon.Enabled = false;
                lastMode = "musique";
                stopAdrilight();
            }
            else if (rbModeCouleur.Checked)
            {
                //Mode Couleur
                timerCouleur.Enabled = true;
                lastMode = "couleur";
                stopAdrilight();

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
                //Mode Global Ambilight 
                timerAmbilightGlobal.Enabled = true;
                lastMode = "ambilight";
                stopAdrilight();
            }
            else if (rbModeAdrilight.Checked)
            {
                //Mode Adrilight
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
                //int value = musique.getVolumeValue();
                int value = analyser._getLevel();
                pbMusic.Value = (value > 0 && value <= 100) ? value : 0;
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
                tbAdrilightPath.Text = adrilightPath;

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
            catch 
            {
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

        public  void RotateLeft<T>(T[] array, int places)
        {
            try {
                T[] temp = new T[places];
                Array.Copy(array, 0, temp, 0, places);
                Array.Copy(array, places, array, 0, array.Length - places);
                Array.Copy(temp, 0, array, array.Length - places, places);
            }
            catch
            {
                rbModeOff.Checked = true;
                MessageBox.Show("You need to set the correct values for the Offset and the LED Number");
            }
        }

        public  void RotateRight<T>(T[] array, int count)
        {
            try
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
            catch
            {
                rbModeOff.Checked = true;
                MessageBox.Show("You need to set the correct values for the Offset and the LED Number");
            }
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

        private void nbLED_ValueChanged(object sender, EventArgs e)
        {
        }

        private void bValNumLED_Click(object sender, EventArgs e)
        {
            numLed = (int)nbLED.Value;

            Properties.Settings.Default.nbLED = (int)nbLED.Value;
            Properties.Settings.Default.Save();

            DialogResult result =  MessageBox.Show("You need to restart to apply changes. Restart Now ?", "Information", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                numLed = (int)nbLED.Value;

                Properties.Settings.Default.nbLED = (int)nbLED.Value;
                Properties.Settings.Default.Save();
                Application.Restart();
            }
            else if (result == DialogResult.Cancel)
            {
                nbLED.Value = Properties.Settings.Default.nbLED;
                numLed = (int)nbLED.Value;
            }
            else
            {
                numLed = (int)nbLED.Value;

                Properties.Settings.Default.nbLED = (int)nbLED.Value;
                Properties.Settings.Default.Save();
            }

        }
    }

    public static class WindowHelper
    {
        public static void BringProcessToFront(Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            if (IsIconic(handle))
            {
                ShowWindow(handle, SW_RESTORE);
            }

            SetForegroundWindow(handle);
        }

        const int SW_RESTORE = 9;

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);
    }
}
