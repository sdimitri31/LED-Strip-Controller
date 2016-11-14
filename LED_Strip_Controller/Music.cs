using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LED_Strip_Controller
{
    class Music
    {
        MainForm mainForm;
        int offset = 0;
        bool sensMusique = false;
        int pattern = 0;

        byte[] tmpPattern;

        public int getOffset() { return offset; }
        public bool getSensMusique() { return sensMusique; }
        public int getPattern() { return pattern; }

        public void setSensMusique(bool sensMusique) { this.sensMusique = sensMusique; }
        public void setOffset(int offset) { this.offset = offset; }
        public void setPattern(int pattern) { this.pattern = pattern; }

        public Music(MainForm mainForm)
        {
            this.mainForm = mainForm;

            tmpPattern = new byte[mainForm.numLed * 3];

            //Offset
            offset = Properties.Settings.Default.Offset;

            //SensMusique
            sensMusique = Properties.Settings.Default.SensMusique;

            //Pattern
            pattern = Properties.Settings.Default.Pattern;          
        }

        public int getVolumeValue()
        {
            return mainForm.analyser._getLevel();
        }

        public byte[] tickTimer(byte[] donnees)
        {
            int value = getVolumeValue();

            List<byte> spectrum = mainForm.analyser._getSpectrum();

            switch (pattern)
            {
                //Pattern 1 (Exp)
                case 0:
                    _doPattern1(donnees, value);
                    break;

                //Pattern 2
                case 1:
                    _doPattern2(donnees, value);
                    break;

                //Mirror
                case 2:
                    return _doPattern3(donnees, value);

                //Frequency
                case 3:
                    _doPattern4(donnees, value, spectrum);
                    break;

                //Frequency Mirror
                case 4:
                   return _doPattern5(donnees, value, spectrum);

                default:
                    _doPattern1(donnees, value);
                    break;

            }

            return donnees;
        }

        private void _doPattern1(byte[] donnees, int value)
        {
            donnees[offset * 3] = 0; //Bleu
            donnees[(offset * 3) + 1] = 0; //Vert
            donnees[(offset * 3) + 2] = 0; //Rouge 

            if (value < 50)
            {
                donnees[offset * 3] = (byte)(-5.1 * value + 255);
                donnees[(offset * 3) + 1] = (byte)(5.1 * value);
            }
            else if (value > 50)
            {
                donnees[(offset * 3) + 1] = (byte)(-5.1 * value + 255);
                donnees[(offset * 3) + 2] = (byte)(Math.Exp(value) / ((1.055) * Math.Pow(10, 41)));
            }


            if (sensMusique)
                mainForm.RotateRight(donnees, 3);
            else
                mainForm.RotateLeft(donnees, 3);

        }
        private void _doPattern2(byte[] donnees, int value)
        {
            donnees[offset * 3] = 0; //Bleu
            donnees[(offset * 3) + 1] = 0; //Vert
            donnees[(offset * 3) + 2] = 0; //Rouge

            if (value < 50)
            {
                donnees[offset * 3] = (byte)(-5.1 * value + 255);
                donnees[(offset * 3) + 1] = (byte)(5.1 * value);
            }
            else if (value > 50)
            {
                donnees[(offset * 3) + 1] = (byte)(-5.1 * value + 255);
                donnees[(offset * 3) + 2] = (byte)(5.1 * value);
            }

            if (sensMusique)
                mainForm.RotateRight(donnees, 3);
            else
                mainForm.RotateLeft(donnees, 3);
        }

        private byte[] _doPattern3(byte[] donnees, int value)
        {
            //Décalage de 3 du tableau donnees
            Buffer.BlockCopy(donnees, 0, donnees, 3, donnees.Length - 3);

            donnees[0] = 0; //Bleu
            donnees[1] = 0; //Vert
            donnees[2] = 0; //Rouge

            //Ecriture de la couleur
            if (value < 50)
            {
                donnees[0] = (byte)(-5.1 * value + 255);
                donnees[1] = (byte)(5.1 * value);
            }
            else if (value > 50)
            {
                donnees[1] = (byte)(-5.1 * value + 255);
                donnees[2] = (byte)(5.1 * value);
            }

            //Miroir
            for (int i = 0; i < mainForm.numLed / 2; i++)
            {
                donnees[((mainForm.numLed - 1) - i) * 3] = donnees[(i * 3)];
                donnees[(((mainForm.numLed - 1) - i) * 3) + 1] = donnees[(i * 3) + 1];
                donnees[(((mainForm.numLed - 1) - i) * 3) + 2] = donnees[(i * 3) + 2];
            }

            mainForm.setDonnees(donnees);

            //Copie dans le tableau temporaire
            Buffer.BlockCopy(donnees, 0, tmpPattern, 0, donnees.Length);

            //Rotation du tableau temporaire
            mainForm.RotateLeft(tmpPattern, offset * 3);

            return tmpPattern;
        }

        private void _doPattern4(byte[] donnees, int value, List<byte> spectrum)
        {
            if (value == 0)
            {
                donnees[(offset * 3)] = 0; //Bleu
                donnees[(offset * 3) + 1] = 0; //Vert
                donnees[(offset * 3) + 2] = 0; //Rouge
            }
            else
            {

                int iMax = 0;
                int iTmp = 0;
                byte tmpValue = 0;
                foreach (byte freqValue in spectrum)
                {
                    if (freqValue > tmpValue)
                    {
                        iMax = iTmp;
                        tmpValue = freqValue;
                    }
                    iTmp++;
                }

                string[] _color = { "#ff0000", "#ff6600", "#ff9900", "#ffcc00",
                                        "#ffff00", "#99ff00", "#66ff00", "#33ff00",
                                        "#00ff33", "#00ff66", "#00ff99", "#00ffff",
                                        "#00ccff", "#0099ff", "#0033ff", "#0000ff",};

                Color color = System.Drawing.ColorTranslator.FromHtml(_color[iMax]);

                donnees[(offset * 3)] = color.B; //Bleu
                donnees[(offset * 3) + 1] = color.G; //Vert
                donnees[(offset * 3) + 2] = color.R; //Rouge

            }

            if (sensMusique)
                mainForm.RotateRight(donnees, 3);
            else
                mainForm.RotateLeft(donnees, 3);
        }

        private byte[] _doPattern5(byte[] donnees, int value, List<byte> spectrum)
        {
            //Décalage de 3 du tableau donnees
            Buffer.BlockCopy(donnees, 0, donnees, 3, donnees.Length - 3);

            if (value == 0)
            {
                donnees[0] = 0; //Bleu
                donnees[1] = 0; //Vert
                donnees[2] = 0; //Rouge
            }
            else
            {

                int iMax = 0;
                int iTmp = 0;
                byte tmpValue = 0;
                foreach (byte freqValue in spectrum)
                {
                    if (freqValue > tmpValue)
                    {
                        iMax = iTmp;
                        tmpValue = freqValue;
                    }
                    iTmp++;
                }

                string[] _color = { "#ff0000", "#ff6600", "#ff9900", "#ffcc00",
                                        "#ffff00", "#99ff00", "#66ff00", "#33ff00",
                                        "#00ff33", "#00ff66", "#00ff99", "#00ffff",
                                        "#00ccff", "#0099ff", "#0033ff", "#0000ff",};

                Color color = System.Drawing.ColorTranslator.FromHtml(_color[iMax]);

                donnees[0] = color.B; //Bleu
                donnees[1] = color.G; //Vert
                donnees[2] = color.R; //Rouge

            }

            //Miroir
            for (int i = 0; i < mainForm.numLed / 2; i++)
            {
                donnees[((mainForm.numLed - 1) - i) * 3] = donnees[(i * 3)];
                donnees[(((mainForm.numLed - 1) - i) * 3) + 1] = donnees[(i * 3) + 1];
                donnees[(((mainForm.numLed - 1) - i) * 3) + 2] = donnees[(i * 3) + 2];
            }

            mainForm.setDonnees(donnees);

            //Copie dans le tableau temporaire
            Buffer.BlockCopy(donnees, 0, tmpPattern, 0, donnees.Length);

            //Rotation du tableau temporaire
            mainForm.RotateLeft(tmpPattern, offset * 3);

            return tmpPattern;
        }
    }
}
