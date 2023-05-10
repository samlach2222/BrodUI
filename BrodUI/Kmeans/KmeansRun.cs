using BrodUI.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace BrodUI.KMeans
{
    // TODO : MISSING DOCUMENTATION

    public static class KMeansRun
    {
        public static Brush[,] StartKMeans(Brush[,] image, int nbClusters, int nbKMeans)
        {
            Dictionary<int, GenericVector> dict = Brush2DtoColorDict.BrushToDict(image);
            List<KMeans> kMeansList = new();
            for (int i = 0; i < nbKMeans; i++)
            {
                KMeans kMeans = new()
                {
                    Iterations = 100,
                    DataSet = dict.Values.ToList(),
                    Clusters = nbClusters
                };
                kMeans.Run();
                kMeansList.Add(kMeans);
            }
            // We keep the lowest SSE
            KMeans lowestKMeans = kMeansList.Aggregate((minItem, nextItem) => minItem.Sse < nextItem.Sse ? minItem : nextItem);
            return Brush2DtoColorDict.DictToBrush2D(dict, lowestKMeans.Centroids!, image.GetLength(0), image.GetLength(1));
        }
    }
}