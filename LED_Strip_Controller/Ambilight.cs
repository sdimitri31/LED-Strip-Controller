using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LED_Strip_Controller
{
    class Ambilight
    {
        MainForm mainForm;

        public Ambilight(MainForm mainForm)
        {
            this.mainForm = mainForm; 
        }

        public Color getAverageColorSpot(int x, int y, int width, int height)
        {
            Rectangle rect = new Rectangle(x, y, width, height);
            using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
                }

                return CalculateAverageColor(bmp);
            }
        }

        private System.Drawing.Color CalculateAverageColor(Bitmap bm)
        {
            int width = bm.Width;
            int height = bm.Height;
            int red = 0;
            int green = 0;
            int blue = 0;
            int minDiversion = -1; // drop pixels that do not differ by at least minDiversion between color values (white, gray or black)
            int dropped = 0; // keep track of dropped pixels
            long[] totals = new long[] { 0, 0, 0 };
            int bppModifier = bm.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4; // cutting corners, will fail on anything else but 32 and 24 bit images

            BitmapData srcData = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, bm.PixelFormat);
            int stride = srcData.Stride;
            IntPtr Scan0 = srcData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int idx = (y * stride) + x * bppModifier;
                        red = p[idx + 2];
                        green = p[idx + 1];
                        blue = p[idx];
                        if (Math.Abs(red - green) > minDiversion || Math.Abs(red - blue) > minDiversion || Math.Abs(green - blue) > minDiversion)
                        {
                            totals[2] += red;
                            totals[1] += green;
                            totals[0] += blue;
                        }
                        else
                        {
                            dropped++;
                        }
                    }
                }
            }

            int count = width * height - dropped;

            int avgR = 0;
            int avgG = 0;
            int avgB = 0;
            if (count != 0)
            {
                avgR = (int)(totals[2] / count);
                avgG = (int)(totals[1] / count);
                avgB = (int)(totals[0] / count);
            }
            return System.Drawing.Color.FromArgb(avgR, avgG, avgB);
        }

    }
}
