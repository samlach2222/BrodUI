using System;
using System.IO;

namespace BrodUI.Helpers
{
    /// <summary>
    /// class to convert DMC value into their Name
    /// </summary>
    public class DmcToString
    {
        /// <summary>
        /// array for DMC colors 0 = DMC, 1 = name of the DMC
        /// </summary>
        private readonly string[,] _dmcString = new string[500, 2];

        /// <summary>
        /// dmc number
        /// </summary>
        private int _nbDmc;

        /// <summary>
        /// Fill the array with the DMC value and its name
        /// </summary>
        private void Initialization()
        {
            StreamReader sr = new("./Assets/DmcToString.txt");
            int comp = 0;
            sr.ReadLine(); // skip first line
            string? line = sr.ReadLine();

            while (line != null)
            {
                _dmcString[comp, 0] = line[..line.IndexOf("-", StringComparison.Ordinal)];
                _dmcString[comp, 1] = line[(line.IndexOf("-", StringComparison.Ordinal) + 1)..];
                comp++;
                line = sr.ReadLine();
            }

            _nbDmc = comp;
            sr.Close();
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public DmcToString()
        {
            Initialization();
        }

        /// <summary>
        /// Get the name of a DMC value 
        /// </summary>
        /// <param name="dmc">DMC value of the passed color</param>
        /// <returns></returns>
        public string GetNameDmc(int dmc)
        {
            string val = "";
            for (int i = 0; i < _nbDmc; i++)
            {
                // Some DMC numbers like 05 have leading zeros
                if (int.Parse(_dmcString[i, 0]) == dmc)
                {
                    val = _dmcString[i, 1];
                    break; // Exit the for loop
                }
            }
            return val;
        }
    }
}