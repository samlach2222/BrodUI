using BrodUI.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace BrodUI.Kmeans
{
    public static class KmeansRun
    {
        public static Brush[,] StartKmeans(Brush[,] image, int nb_clusters)
        {
            Dictionary<int, GenericVector> dict = Brush2DtoColorDict.BrushToDict(image);
            var kMeanses = new List<KMeans>();
            for (var i = 0; i < 2; i++)
            {
                var kMeans = new KMeans
                {
                    Iterations = 100,
                    Dataset = dict.Values.ToList(),
                    Clusters = nb_clusters
                };
                kMeans.Run();
                kMeanses.Add(kMeans);
            }
            // We keep the lowest SSE
            var lowestKmeans = kMeanses.Aggregate((minItem, nextItem) => minItem.Sse < nextItem.Sse ? minItem : nextItem);
            return Brush2DtoColorDict.DictToBrush2D(dict, lowestKmeans.Centroids,image.GetLength(0),image.GetLength(1));
        }
    }
}