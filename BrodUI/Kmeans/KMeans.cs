using System;
using System.Collections.Generic;
using System.Linq;

namespace BrodUI.KMeans
{
    // TODO : MISSING DOCUMENTATION

    public class KMeans
    {
        public int Iterations;
        public int Clusters;
        public double Sse;
        public List<GenericVector>? DataSet;
        public Dictionary<int, GenericVector>? Centroids;
        private readonly Random _random = new();
        private int _i;

        //the method to run the KMeans algorithm
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

                //recalculate clusters
                RecalculateCentroids();

                //check if the centroids are still changing
                if (!CentroidsChanged(oldCentroids, Centroids.Values.ToList()))
                    break;
            }
            Sse = CalculateSumOfSquaredErrors();
        }

        //assign the vectors to the clusters nearby
        private void AssignDataSet()
        {
            DataSet?.ForEach(vector => vector.Cluster = GetNearestCluster(vector));
        }

        //get nearest cluster
        private int GetNearestCluster(GenericVector vector)
        {
            int clusterId = Centroids!
                .OrderBy(v => GenericVector.Distance(vector, v.Value))
                .Select(v => v.Key)
                .FirstOrDefault();
            return clusterId;
        }

        //generate random centroids for the first time
        private Dictionary<int, GenericVector> GenerateRandomCentroids(int clusters)
        {
            Dictionary<int, GenericVector> centroids = new();
            int index = 0;
            clusters.Times(() => centroids.Add(index++, RandomVector()));
            return centroids;
        }


        private static bool CentroidsChanged(IEnumerable<GenericVector> a, IReadOnlyList<GenericVector> b)
        {
            return a.Where((item, index) => GenericVector.NotEqual(item, b[index])).Any();
        }


        //recalculate the new centroids based on the mean
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

        //get a random vector
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