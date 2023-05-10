using BrodUI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace BrodUI.KMeans
{

    public static class KMeansRun
    {
        /// <summary>
        /// Reduction of the number of color and recoloration with the new colors
        /// </summary>
        /// <param name="image"> is array containing the color of each pixel in a Brush object </param>
        /// <param name="nbClusters"> nbClusters is the number of color at the end of the reduction </param>
        /// <param name="nbKmeans"> nbKmeans is the number of times we execute Kmeans algorithm to get the best result, higher value means longer execution times but better result </param>
        public static Brush[,] StartKMeans(Brush[,] image, int nbClusters, int nbKMeans, BackgroundWorker bw)
        {
            Dictionary<int, GenericVector> dict = Brush2DtoColorDict.BrushToDict(image);
            List<KMeans> kMeansList = new();
            for (int i = 0; i < nbKMeans; i++)
            {
                Console.WriteLine("KMeans execution n°" + i + "\n");
                KMeans kMeans = new()
                {
                    Iterations = 100,
                    DataSet = dict.Values.ToList(),
                    Clusters = nbClusters
                };
                kMeans.Run();
                Console.WriteLine("Sum of squared errors : " + kMeans.Sse + "\n\n");
                kMeansList.Add(kMeans);
                if (bw.IsBusy)
                {
                    bw.ReportProgress((int)((double)i / nbKMeans * 100));
                }
            }
            // We keep the lowest SSE
            KMeans lowestKMeans = kMeansList.Aggregate((minItem, nextItem) => minItem.Sse < nextItem.Sse ? minItem : nextItem);
            return Brush2DtoColorDict.DictToBrush2D(dict, lowestKMeans.Centroids!, image.GetLength(0), image.GetLength(1));
        }
    }
}