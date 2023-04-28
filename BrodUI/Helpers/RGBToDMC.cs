using System;
using System.IO;

namespace BrodUI.Helpers
{
    /// <summary>
    /// class to convert RGB to DMC
    /// </summary>
    internal class RgbToDmc
    {
        //array for DMC colors 0 = R, 1 = G, 2 = B, 3 = DMC
        private readonly int[,] _dmc = new int[500, 4];
        private int _nbDmc;

        /// <summary>
        /// Initialize the array with the DMC colors
        /// </summary>
        private void Initialization()
        {
            StreamReader sr = new("./Assets/DMC.txt");
            int comp = 0;
            string? line = sr.ReadLine();
            string nb = "";
            int state = 3;
            while (line != null)
            {
                int count;
                foreach (var t in line)
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
        public RgbToDmc()
        {
            Initialization();
            for (int i = 0; i < _nbDmc; i++)
            {
                string s = "DMC:" + _dmc[i, 3] + " R:" + _dmc[i, 0] + " G:" + _dmc[i, 1] + " B:" + _dmc[i, 2];
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// Get the DMC value of a color in RGB
        /// </summary>
        /// <param name="r">R value of the passed color</param>
        /// <param name="g">G value of the passed color</param>
        /// <param name="b">B value of the passed color</param>
        /// <returns></returns>
        public int GetValDmc(int r, int g, int b)
        {
            int val = 0;
            int valMin = 100000;
            for (int i = 0; i < _nbDmc; i++)
            {
                int valTemp = (_dmc[i, 0] - r) * (_dmc[i, 0] - r) + (_dmc[i, 1] - g) * (_dmc[i, 1] - g) + (_dmc[i, 2] - b) * (_dmc[i, 2] - b);
                if (valTemp >= valMin) continue;
                valMin = valTemp;
                val = _dmc[i, 3];
            }
            return val;
        }

    }
}
