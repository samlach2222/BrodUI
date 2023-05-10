using System;
using System.IO;

namespace BrodUI.Helpers
{
    /// <summary>
    /// class to convert RGB to DMC
    /// </summary>
    public class RgbToDmc
    {
        /// <summary>
        /// array for DMC colors 0 = R, 1 = G, 2 = B, 3 = DMC
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
            StreamReader sr = new("./Assets/DmcToRgb.txt");
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
        /// Get the red value of a DMC color
        /// </summary>
        /// <param name="d">DMC color</param>
        /// <returns></returns>
        public int getRed(int d)
        {
            int r = 0;
            for (int i = 0; i < _nbDmc; i++)
            {
                if (_dmc[i, 3] == d) r = _dmc[i, 0];
            }
            return r;
        }
        /// <summary>
        /// Get the green value of a DMC color
        /// </summary>
        /// <param name="d">DMC color</param>
        /// <returns></returns>
        public int getGreen(int d)
        {
            int g = 0;
            for (int i = 0; i < _nbDmc; i++)
            {
                if (_dmc[i, 3] == d) g = _dmc[i, 1];
            }
            return g;
        }
        /// <summary>
        /// Get the blue value of a DMC color
        /// </summary>
        /// <param name="d">DMC color</param>
        /// <returns></returns>
        public int getBlue(int d)
        {
            int b = 0;
            for (int i = 0; i < _nbDmc; i++)
            {
                if (_dmc[i, 3] == d) b = _dmc[i, 2];
            }
            return b;
        }
        /// <summary>
        /// constructor of the class
        /// </summary>
        public RgbToDmc()
        {
            Initialization();
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

        /// <summary>
        /// Print all DMC in the format "DMC:01 R:227 G:227 B:230"
        /// </summary>
        public void PrintFileContent()
        {
            for (int i = 0; i < _nbDmc; i++)
            {
                string s = "DMC:" + _dmc[i, 3] + " R:" + _dmc[i, 0] + " G:" + _dmc[i, 1] + " B:" + _dmc[i, 2];
                Console.WriteLine(s);
            }
        }
    }
}
