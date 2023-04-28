using System;
using System.IO;


//les longueurs sont calculées en centimètres
//un carré fait une taille de 0.5x0.5 cm

namespace BrodUI.Helpers
{
    internal class LengthThread 
    {
        //longueur totale de fil nécessaire
		private double longueur_tot;

        //longueur de fil nécessaire pour réaliser un noeud au début et à la fin de l'utilisation du fil
        private double knot = 2;
        public double square = Math.Sqrt(0.5);
        //longueur de fil nécessaire pour réaliser une croix
        private double cross;


        

        public double Longueur_tot
        {
            get { return longueur_tot; }
            set { longueur_tot = value; }
        }

        public LengthThread(int color, int[,] image)
        {

            cross = (2 * square) + 1;

            longueur_tot = 0;
            longueur_tot = Taille_Fil(color, image);

        }

        public double Taille_Fil(int color, int[,] image)
        {
            //compteur pour savoir le nombre de pixel de même couleur consécutif
            int longueur = 1;

            //on regarde sur l'image les pixels où la couleur color est présente
            for (int i = 0; i < image.GetLongLength(1); i++)
            {
                for (int j = 0; j < image.GetLongLength(2); j++)
                {
                    //si le pixel est de couleur color
                    if (image[i,j] == color)
                    {
                        //et que le pixel voisin est de couleur color 
                        if (image[i,j + 1] == color)
                            longueur += longueur;

                        //une fois qu'on a plus de pixels consécutifs de la couleur color : calcul longueur total et remise à 1 de longueur
                        else
                        {
                            longueur_tot = longueur_tot + (cross * longueur) + (2 * knot);
                            longueur = 1;
                        }
                    }
                }
            }

            return longueur_tot;
        }

    }

}



