using BrodUI.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace BrodUI.Kmeans
{
    public static class KmeansRun
    {
        public static Brush[,] StartKmeans(Brush[,] image, int nbClusters, int nbKmeans)
        {
            Dictionary<int, GenericVector> dict = Brush2DtoColorDict.BrushToDict(image);
            List<KMeans> kMeansList = new();
            for (int i = 0; i < nbKmeans; i++)
            {
                KMeans kMeans = new()
                {
                    Iterations = 100,
                    Dataset = dict.Values.ToList(),
                    Clusters = nbClusters
                };
                kMeans.Run();
                kMeansList.Add(kMeans);
            }
            // We keep the lowest SSE
            KMeans lowestKmeans = kMeansList.Aggregate((minItem, nextItem) => minItem.Sse < nextItem.Sse ? minItem : nextItem);
            return Brush2DtoColorDict.DictToBrush2D(dict, lowestKmeans.Centroids!, image.GetLength(0), image.GetLength(1));
        }
    }
}