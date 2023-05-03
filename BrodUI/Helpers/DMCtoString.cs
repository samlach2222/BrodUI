using System;
using System.IO;

namespace BrodUI.Helpers
{
    /// <summary>
    /// class to convert DMC value into their Name
    /// </summary>
    internal class DMCtoString
    {
        //array for DMC colors 0 = DMC, 1 = name of the DMC
        private readonly string[,] _dmcString = new string[500, 2];
        private int _nbDmc;

        /// <summary>
        /// Fill the array with the DMC value and its name
        /// </summary>
        private void Initialization()
        {
            StreamReader sr = new("./Assets/DMCtoString.txt");
            int comp = 0;
            string? line = sr.ReadLine();

            while (line != null)
            {
                _dmcString[comp,0] = line.Substring(0, line.IndexOf("-"));
                _dmcString[comp, 1] = line.Substring(line.IndexOf("-") + 2);
                comp++;
                line = sr.ReadLine();
            }

            _nbDmc = comp;
            sr.Close();
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public DMCtoString()
        {
            Initialization();
            for (int i = 0; i < _nbDmc; i++)
            {
                string s = "DMC:" + _dmcString[i, 0] + " Name:" + _dmcString[i, 1];
                Console.WriteLine(s);
            }
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
                if (_dmcString[i, 0] == dmc.ToString()) 
                    val = _dmcString[i, 1];
            }
            return val;
        }

    }
}








