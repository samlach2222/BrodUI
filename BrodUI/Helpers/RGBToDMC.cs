using System;
using System.IO;

namespace BrodUI.Helpers
{
    /// <summary>
    /// class to convert RGB to DMC
    /// </summary>
    internal class RGBToDMC
    {
        //array for DMC colors 0 = R, 1 = G, 2 = B, 3 = DMC
        int[,] DMC = new int[500, 4];
        int nbDMC = 0;

        private void initialisation()
        {
            StreamReader sr = new StreamReader("DMC.txt");
            int comp = 0;
            if (sr != null)
            {
                string line = sr.ReadLine();
                line = sr.ReadLine();
                string nb = "";
                int etat = 3;
                int nbS = 0;
                while (line != null)
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == ' ')
                        {
                            nbS = int.Parse(nb);
                            if (etat == 0)
                            {
                                DMC[comp, 0] = nbS;
                                etat = 1;
                            }
                            else
                            {
                                if (etat == 1)
                                {
                                    DMC[comp, 1] = nbS;
                                    etat = 2;
                                }
                                else
                                {
                                    if (etat == 3)
                                    {
                                        DMC[comp, 3] = nbS;
                                        etat = 0;
                                    }
                                }
                            }
                            nb = "";
                        }
                        else
                        {
                            nb = nb + line[i];
                        }
                    }
                    nbS = int.Parse(nb);
                    nb = "";
                    DMC[comp, 2] = nbS;
                    etat = 3;
                    comp++;
                    nbDMC++;
                    line = sr.ReadLine();
                }
                sr.Close();
            }
        }

        /// <summary>
        /// constructeur of the class
        /// </summary>
        public RGBToDMC()
        {
            initialisation();
            string s = "";
            for (int i = 0; i < nbDMC; i++)
            {
                s = "DMC:" + DMC[i, 3] + " R:" + DMC[i, 0] + " G:" + DMC[i, 1] + " B:" + DMC[i, 2];
                Console.WriteLine(s);
            }
        }
        public int getvalDMC(int R, int G, int B)
        {
            int val = 0;
            int valMin = 100000;
            int valTemp = 0;
            for (int i = 0; i < nbDMC; i++)
            {
                valTemp = (DMC[i, 0] - R) * (DMC[i, 0] - R) + (DMC[i, 1] - G) * (DMC[i, 1] - G) + (DMC[i, 2] - B) * (DMC[i, 2] - B);
                if (valTemp < valMin)
                {
                    valMin = valTemp;
                    val = DMC[i, 3];
                }
            }
            return val;
        }

    }
}
