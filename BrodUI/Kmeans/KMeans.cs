using System;
using System.Collections.Generic;
using System.Linq;

namespace BrodUI.KMeans
{

    public class KMeans
    {
        /// <summary>
        /// Maximal number of iterations for the execution of the algorithm
        /// </summary>
        public int Iterations;

        /// <summary>
        /// Number of clusters
        /// </summary>
        public int Clusters;

        /// <summary>
        /// Sum of Squared Errors
        /// </summary>
        public double Sse;

        /// <summary>
        /// List of Vector containing the data
        /// </summary>
        public List<GenericVector>? DataSet;

        /// <summary>
        /// Dictionnary containing the centroid of each cluster indexing by the cluster id (just a number to link the data vectors to the centroid of its cluster)
        /// </summary>
        public Dictionary<int, GenericVector>? Centroids;

        /// <summary>
        /// Random object for random generation (used to select a random vector among dataset)
        /// </summary>
        private readonly Random _random = new();

        /// <summary>
        /// Number of iterations used during the algorithm (increase during execution)
        /// </summary>
        private int _i;

        /// <summary>
        /// Start the Kmeans algorithm with the information in the attributes of the object
        /// </summary>
        public void Run()
        {
            Centroids = GenerateRandomCentroids(Clusters);

            //for loop Iterations
            for (_i = 0; _i < Iterations; _i++)
            {
                //storing the old centroids to compare it later on.
                List<GenericVector> oldCentroids = Centroids.Values.ToList();

                //assign data set
                AssignDataSet();

                Console.WriteLine("Iterations n°"+_i);
                foreach(var cid in Centroids.Keys)
                {
                    var nb = DataSet.Where(i=>i.Cluster==cid).Count();
                    Console.WriteLine("\tCluster n°"+cid+" has "+Centroids[cid]+" as centroid. This cluster contains "+nb+" data vectors");
                }

                //recalculate clusters
                RecalculateCentroids();

                //check if the centroids are still changing
                if (!CentroidsChanged(oldCentroids, Centroids.Values.ToList()))
                    break;
            }
            Sse = CalculateSumOfSquaredErrors();
        }

        /// <summary>
        /// Assign each data vector to its nearest cluster
        /// </summary>
        private void AssignDataSet()
        {
            DataSet?.ForEach(vector => vector.Cluster = GetNearestCluster(vector));
        }

        /// <summary>
        /// Find the nearest cluster to the vector parameter
        /// </summary>
        /// <param name="vector"> the vector for which we search the nearest cluster </param>
        private int GetNearestCluster(GenericVector vector)
        {
            int clusterId = Centroids!
                .OrderBy(v => GenericVector.Distance(vector, v.Value))
                .Select(v => v.Key)
                .FirstOrDefault();
            return clusterId;
        }

        /// <summary>
        /// Generate a number of random centroids
        /// </summary>
        /// <param name="clusters"> the number of centroids we want </param>
        private Dictionary<int, GenericVector> GenerateRandomCentroids(int clusters)
        {
            Dictionary<int, GenericVector> centroids = new();
            int index = 0;
            clusters.Times(() => centroids.Add(index++, RandomVector()));
            return centroids;
        }

        /// <summary>
        /// Verify if the centroids have changed (for use in between iterations of the kmeans algorithm)
        /// </summary>
        /// <param name="a"> The previous centroids </param>
        /// <param name="b"> The current centroids </param>
        private static bool CentroidsChanged(IEnumerable<GenericVector> a, IReadOnlyList<GenericVector> b)
        {
            return a.Where((item, index) => GenericVector.NotEqual(item, b[index])).Any();
        }


        /// <summary>
        /// Recalculate the centroids as the mean of all vectors in its cluster
        /// </summary>
        private void RecalculateCentroids()
        {
            if (Centroids == null) return;
            foreach (int centroidKey in Centroids.Keys.ToList())
            {
                if (DataSet == null) continue;
                List<GenericVector> cluster = DataSet.Where(v => v.Cluster == centroidKey).ToList();

                //generating a new genericVector with the mean of the existing vectors
                Centroids[centroidKey] = cluster
                    .Aggregate(new GenericVector(DataSet.First().Size),
                        (current, y) => current.Sum(y))
                    .Divide(cluster.Count);
            }
        }

        /// <summary>
        /// Get a random data vector that is not already use as centroid
        /// </summary>
        private GenericVector RandomVector()
        {
            while (true)
            {
                if (DataSet == null) continue;
                GenericVector genericVector = DataSet[_random.Next(DataSet.Count)];
                bool found = false;
                if (Centroids != null)
                {
                    for (int i = 0; i < Centroids.Values.ToList().Count; i++)
                    {
                        if (Centroids.Values.ToList()[i] == genericVector)
                            found = true;
                    }
                    if (!found)
                        return genericVector;
                }
                else
                    return genericVector;
            }
        }

        //print the clusters to the console.
        public void PrintClusters()
        {
            if (DataSet == null) return;
            IOrderedEnumerable<IGrouping<int, GenericVector>> clusters = DataSet.GroupBy(v => v.Cluster).ToList().OrderBy(v => v.Key);
            foreach (IGrouping<int, GenericVector> cluster in clusters)
            {
                // TODO : Change strings to use Assets
                Console.WriteLine("Cluster " + cluster.ElementAt(0).Cluster + " has " + cluster.Count());
            }
        }

        public void PrintClustersInLine()
        {
            if (DataSet != null)
            {
                IOrderedEnumerable<IGrouping<int, GenericVector>> clusters = DataSet.GroupBy(v => v.Cluster).ToList().OrderBy(v => v.Key);
                foreach (IGrouping<int, GenericVector> cluster in clusters)
                {
                    Console.Write(cluster.ElementAt(0).Cluster + " -> " + cluster.Count() + " || ");
                }
            }

            Console.WriteLine();
        }

        public void PrintClusterInfo()
        {
            // TODO : Change strings to use Assets
            Console.WriteLine("KMEANS COMPLETED");
            Console.WriteLine("SSE = \t\t\t\t" + Sse);
            Console.WriteLine("amount of clusters: \t\t" + Clusters);
            Console.WriteLine("amount of max iterations: \t" + Iterations);
            Console.WriteLine("amount of actual iterations: \t" + _i);
            Console.WriteLine();

            if (DataSet != null)
            {
                List<IGrouping<int, GenericVector>> clusters = DataSet.GroupBy(v => v.Cluster).ToList().OrderBy(v => v.Key).ToList();

                foreach (IGrouping<int, GenericVector> cluster in clusters)
                {
                    // TODO : Change strings to use Assets
                    Console.WriteLine("***************************************************");
                    Console.WriteLine("cluster " + cluster.Key + " has " + cluster.Count() + " items");
                    Console.WriteLine("***************************************************");
                    Dictionary<int, int> offersBoughtXTimes = new();
                    List<GenericVector> clusterpoints = cluster.ToList();
                    foreach (GenericVector clusterpoint in clusterpoints)
                    {
                        for (int j = 0; j < clusterpoint.Size; j++)
                        {
                            if (!(Math.Abs(clusterpoint.Points[j] - 1) < 0.001)) continue;
                            if (offersBoughtXTimes.ContainsKey(j))
                                offersBoughtXTimes[j]++;
                            else
                                offersBoughtXTimes.Add(j, 1);
                        }
                    }
                    offersBoughtXTimes =
                        (from entry in offersBoughtXTimes orderby entry.Value descending select entry).ToDictionary(
                            v => v.Key,
                            v => v.Value);
                    foreach (KeyValuePair<int, int> offerBought in offersBoughtXTimes.Where(offerBought => offerBought.Value >= 3))
                    {
                        // TODO : Change strings to use Assets
                        Console.WriteLine("Offer " + (offerBought.Key + 1) + " \t-> bought " + offerBought.Value + " times ");
                    }
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// The sum of the squared distance of each centroids with all points of its cluster (for result comparison)
        /// </summary>
        public double CalculateSumOfSquaredErrors()
        {
            List<IGrouping<int, GenericVector>> orderedClusters = DataSet!.GroupBy(v => v.Cluster).OrderBy(v => v.Key).ToList();
            double sse = (from cluster in orderedClusters
                          let clusterCenter = Centroids[cluster.Key]
                          from point in cluster
                          select Math.Pow(GenericVector.Distance(clusterCenter, point), 2)).Sum();
            return sse;
        }
    }
}