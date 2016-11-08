using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED_Strip_Controller
{
    class Music
    {
        MainForm mainForm;
        int offset = 0;
        bool sensMusique = false;
        int pattern = 0;

        byte[] tmpPattern = new byte[MainForm.numLed * 3];

        public int getOffset() { return offset; }
        public bool getSensMusique() { return sensMusique; }
        public int getPattern() { return pattern; }

        public void setSensMusique(bool sensMusique) { this.sensMusique = sensMusique; }
        public void setOffset(int offset) { this.offset = offset; }
        public void setPattern(int pattern) { this.pattern = pattern; }

        public Music(MainForm mainForm)
        {
            this.mainForm = mainForm;
            
            //Offset
            offset = Properties.Settings.Default.Offset;

            //SensMusique
            sensMusique = Properties.Settings.Default.SensMusique;

            //Pattern
            pattern = Properties.Settings.Default.Pattern;          
        }

        public int getVolumeValue()
        {
            var device = (MMDevice)mainForm.getCbPerifSonSelectedValue();
            return (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 100));
        }

        public byte[] tickTimer(byte[] donnees)
        {
            int value = getVolumeValue();


            switch (pattern)
            {
                case 0:

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
                        MainForm.RotateRight(donnees, 3);
                    else
                        MainForm.RotateLeft(donnees, 3);

                    break;

                case 1:

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
                        MainForm.RotateRight(donnees, 3);
                    else
                        MainForm.RotateLeft(donnees, 3);

                    break;

                case 2:

                    //Décalage de 3 du tableau donnees
                    Buffer.BlockCopy(donnees, 0, donnees, 3, donnees.Length - 3);

                    //Copie de donnees dans un tableau temporaire
                    //Buffer.BlockCopy(donnees, 0, tmpPattern, 0, donnees.Length);


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
                    for (int i = 0; i < MainForm.numLed / 2; i++)
                    {
                        donnees[((MainForm.numLed - 1) - i) * 3] = donnees[(i * 3)];
                        donnees[(((MainForm.numLed - 1) - i) * 3) + 1] = donnees[(i * 3) + 1];
                        donnees[(((MainForm.numLed - 1) - i) * 3) + 2] = donnees[(i * 3) + 2];
                    }

                    mainForm.setDonnees(donnees);

                    //Copie dans le tableau temporaire
                    Buffer.BlockCopy(donnees, 0, tmpPattern, 0, donnees.Length);


                    //Rotation du tableau temporaire
                    MainForm.RotateLeft(tmpPattern, offset * 3);


                    return tmpPattern;

                //break;


                default: 
                    
                    if (value < 50)
                    {
                        donnees[(offset * 3) + 1] = (byte)(5.1 * value);
                        donnees[offset * 3] = (byte)(-5.1 * value + 255);
                    }
                    else if (value > 50)
                    {
                        donnees[(offset * 3) + 1] = (byte)(-5.1 * value + 255);
                        donnees[(offset * 3) + 2] = (byte)(Math.Exp(value) / ((1.055) * Math.Pow(10, 41)));
                    }

                    if (sensMusique)
                        MainForm.RotateRight(donnees, 3);
                    else
                        MainForm.RotateLeft(donnees, 3);

                    break;

            }

            return donnees;
        }
    }
}
