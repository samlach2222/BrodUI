using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using BrodUI.Kmeans;

namespace BrodUI.Helpers
{
    /// <summary>
    /// class to convert 2D Array of Brush to Dictionnary containing the colors
    /// </summary>
    public static class Brush2DtoColorDict
    {
        /// <summary>
        /// convert DMC 2D Array of Brush to Dictionnary containing the colors
        /// </summary>
        public static Dictionary<int, GenericVector> BrushToDict(Brush[,] image)
        {
            var dict = new Dictionary<int, GenericVector>();
            var x = image.GetLength(0);
            BrushConverter converter = new();
            for(var i=0; i<x;i++)
            {
                for(var j=0; j<image.GetLength(1);j++)
                {
                    var vec = new GenericVector();
                    Brush brush = image[i,j];
                    SolidColorBrush col = (SolidColorBrush)converter.ConvertFromString(brush.ToString())!;
                    vec.Add(col.Color.R);
                    vec.Add(col.Color.G);
                    vec.Add(col.Color.B);
                    dict.Add(j*x+i,vec);
                }
            }
            return dict;
        }

        public static Brush[,] DictToBrush2D(Dictionary<int,GenericVector> dict, KMeans kmean, int sizeX, int sizeY)
        {
            Brush[,] res = new Brush[sizeX,sizeY];
            for(var i=0; i<sizeX; i++)
            {
                for(var j=0; j<sizeY; j++)
                {
                    GenericVector gv = dict[i+j*sizeX];
                    int centroidid = kmean.Centroids
                        .OrderBy(v => GenericVector.Distance(gv, v.Value))
                        .Select(v => v.Key)
                        .FirstOrDefault();
                    GenericVector centroid = kmean.Centroids[centroidid];
                    res[i,j]=new SolidColorBrush(Color.FromArgb(255,BitConverter.GetBytes(centroid.Points[0])[0],BitConverter.GetBytes(centroid.Points[1])[0],BitConverter.GetBytes(centroid.Points[2])[0]));
                }
            }
            return res;
        }
    }
}