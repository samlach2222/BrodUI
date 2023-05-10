using BrodUI.KMeans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace BrodUI.Helpers
{
    /// <summary>
    /// class to convert 2D Array of Brush to Dictionary containing the colors
    /// </summary>
    public static class Brush2DtoColorDict
    {
        /// <summary>
        /// convert DMC 2D Array of Brush to Dictionary containing the colors
        /// </summary>
        /// <param name="image"> 2D array of Brush object containing the color of each pixel of the picture </param>
        public static Dictionary<int, GenericVector> BrushToDict(Brush[,] image)
        {
            Dictionary<int, GenericVector> dict = new();
            int x = image.GetLength(0);
            BrushConverter converter = new();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    GenericVector vec = new();
                    Brush brush = image[i, j];
                    SolidColorBrush col = (SolidColorBrush)converter.ConvertFromString(brush.ToString())!;
                    vec.Add(col.Color.R);
                    vec.Add(col.Color.G);
                    vec.Add(col.Color.B);
                    dict.Add(j * x + i, vec);
                }
            }
            return dict;
        }

        /// <summary>
        /// convert Dictionary containing the colors to DMC 2D Array of Brush
        /// </summary>
        /// <param name="dict"> Dictionnary containing the color of each pixel in the original picture </param>
        /// <param name="centroids"> Dictionnary containing the new color of the pictures </param>
        /// <param name="sizeX"> height of picture </param>
        /// <param name="sizeY"> width of picture </param>
        public static Brush[,] DictToBrush2D(Dictionary<int, GenericVector> dict, Dictionary<int, GenericVector> centroids, int sizeX, int sizeY)
        {
            Brush[,] res = new Brush[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    GenericVector gv = dict[i + j * sizeX];
                    int centroidId = centroids
                        .OrderBy(v => GenericVector.Distance(gv, v.Value))
                        .Select(v => v.Key)
                        .FirstOrDefault();
                    GenericVector centroid = centroids[centroidId];
                    res[i, j] = new SolidColorBrush(Color.FromRgb((byte)Math.Round(centroid.Points[0]), (byte)Math.Round(centroid.Points[1]), (byte)Math.Round(centroid.Points[2])));
                }
            }
            return res;
        }
    }
}