using System;
using System.IO;

namespace BrodUI.Helpers
{
    /// <summary>
    /// class to convert HSL to DMC
    /// </summary>
    public class HslToDmc
    {
        /// <summary>
        /// array for DMC colors 0 = H, 1 = S, 2 = L, 3 = DMC
        /// </summary>
        private readonly int[,] _dmc = new int[500, 4];

        /// <summary>
        /// dmc number
        /// </summary>
        private int _nbDmc;

        /// <summary>
        /// Initialize the array with the DMC colors
        /// </summary>
        public void Initialization()
        {
            StreamReader sr = new("./Assets/DmcToHsl.txt");
            int comp = 0;
            sr.ReadLine(); // skip first line
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
        public HslToDmc()
        {
            Initialization();
        }
        /// <summary>
        /// Get the hue value of a DMC color
        /// </summary>
        /// <param name="d">DMC color</param>
        /// <returns></returns>
        public int getHue(int d)
        {
            int h = 0;
            for (int i = 0; i < _nbDmc; i++)
            {
                if (_dmc[i, 3] == d) h= _dmc[i, 0];
            }
            return h;
        }
        /// <summary>
        /// Get the saturation value of a DMC color
        /// </summary>
        /// <param name="d">DMC color</param>
        /// <returns></returns>
        public int getSaturation(int d)
        {
            int s = 0;
            for (int i = 0; i < _nbDmc; i++)
            {
                if (_dmc[i, 3] == d) s = _dmc[i, 1];
            }
            return s;
        }
        /// <summary>
        /// Get the lightness value of a DMC color
        /// </summary>
        /// <param name="d">DMC color</param>
        /// <returns></returns>
        public int getLightness(int d)
        {
            int l = 0;
            for (int i = 0; i < _nbDmc; i++)
            {
                if (_dmc[i, 3] == d) l = _dmc[i, 2];
            }
            return l;
        }
        /// <summary>
        /// Get the DMC value of a color in HSL
        /// </summary>
        /// <param name="h">H value of the passed color</param>
        /// <param name="s">S value of the passed color</param>
        /// <param name="l">L value of the passed color</param>
        /// <returns></returns>
        public int GetValDmc(int h, int s, int l)
        {
            int val = 0;
            int valMin = 100000;
            for (int i = 0; i < _nbDmc; i++)
            {
                int valTemp = (_dmc[i, 0] - h) * (_dmc[i, 0] - h) + (_dmc[i, 1] - s) * (_dmc[i, 1] - s) + (_dmc[i, 2] - l) * (_dmc[i, 2] - l);
                if (valTemp >= valMin) continue;
                valMin = valTemp;
                val = _dmc[i, 3];
            }
            return val;
        }

        /// <summary>
        /// Print all DMC in the format "DMC:01 H:240 S:6 L:90"
        /// </summary>
        public void PrintFileContent()
        {
            for (int i = 0; i < _nbDmc; i++)
            {
                string s = "DMC:" + _dmc[i, 3] + " H:" + _dmc[i, 0] + " S:" + _dmc[i, 1] + " L:" + _dmc[i, 2];
                Console.WriteLine(s);
            }
        }
    }
}
