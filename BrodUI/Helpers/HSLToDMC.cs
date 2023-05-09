using System;
using System.IO;

namespace BrodUI.Helpers
{
    /// <summary>
    /// class to convert HSL to DMC
    /// </summary>
    public class HSLToDMC
    {
        //array for DMC colors 0 = H, 1 = S, 2 = L, 3 = DMC
        private readonly int[,] _dmc = new int[500, 4];
        private int _nbDmc;

        /// <summary>
        /// Initialize the array with the DMC colors
        /// </summary>
        private void Initialization()
        {
            StreamReader sr = new("./Assets/DMC_hsl.txt");
            int comp = 0;
            string? line = sr.ReadLine();
            string nb = "";
            int state = 3;
            while (line != null)
            {
                int count;
                foreach (char t in line)
                {
                    if (t == ' ')
                    {
                        count = int.Parse(nb);
                        switch (state)
                        {
                            case 0:
                                _dmc[comp, 0] = count;
                                state = 1;
                                break;
                            case 1:
                                _dmc[comp, 1] = count;
                                state = 2;
                                break;
                            case 3:
                                _dmc[comp, 3] = count;
                                state = 0;
                                break;
                        }
                        nb = "";
                    }
                    else
                    {
                        nb += t;
                    }
                }
                count = int.Parse(nb);
                nb = "";
                _dmc[comp, 2] = count;
                state = 3;
                comp++;
                _nbDmc++;
                line = sr.ReadLine();
            }
            sr.Close();
        }

        /// <summary>
        /// constructor of the class
        /// </summary>
        public HSLToDMC()
        {
            Initialization();
            for (int i = 0; i < _nbDmc; i++)
            {
                string s = "DMC:" + _dmc[i, 3] + " H:" + _dmc[i, 0] + " S:" + _dmc[i, 1] + " L:" + _dmc[i, 2];
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// Get the DMC value of a color in HSL
        /// </summary>
        /// <param name="r">R value of the passed color</param>
        /// <param name="g">G value of the passed color</param>
        /// <param name="b">B value of the passed color</param>
        /// <returns></returns>
        public int GetValDmc(int r, int g, int b)
        {
            int val = 0;
            int valMin = 100000;
            int h= 0;
            int s = 0;
            int l = 0;
            float r1 = r / 255f;
            float g1 = g / 255f;
            float b1 = b / 255f;
            float max = Math.Max(r1, Math.Max(g1, b1));
            float min = Math.Min(r1, Math.Min(g1, b1));
            float delta = max - min;
            float h1 = 0;
            float s1 = 0;
            float l1 = (max + min) / 2f;
            if (delta > 0)
            {
                h=0;
                s1= 0;
            }
            else
            {
                s1=(l1<=0.5f)?(delta/(max+min)):(delta/(2-max-min));
                if (r1 == max)
                {
                    h1 = ((g - b) / 6) / delta;
                }
                else if (g == max)
                {
                    h1= (1 / 3) + ((b - r) / 6) / delta;
                }
                else
                {
                    h1= (2 / 3) + ((r - g) / 6) / delta;
                }
                if (h1 < 0)
                {
                    h1 += 1;
                }
                if (h1 > 1)
                {
                    h1 -= 1;
                }
                h=(int)(h1 *360);
                s=(int)(s1 * 100);
                l=(int)(l1 * 100);
            }

            for (int i = 0; i < _nbDmc; i++)
            {
                int valTemp = (_dmc[i, 0] - h) * (_dmc[i, 0] - h) + (_dmc[i, 1] - s) * (_dmc[i, 1] - s) + (_dmc[i, 2] - l) * (_dmc[i, 2] - l);
                if (valTemp >= valMin) continue;
                valMin = valTemp;
                val = _dmc[i, 3];
            }
            return val;
        }
    }
}
