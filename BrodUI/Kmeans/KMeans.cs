using System;
using System.Collections.Generic;
using System.Linq;

namespace KmeansAlgorithm
{
    public class KMeans
    {
        public int Iterations;
        public int Clusters;
        public double Sse;
        public List<GenericVector> Dataset;
        public Dictionary<int, GenericVector> Centroids;
        private readonly Random _random = new Random();
        private int _i;

        //the method to run the KMeans algorithm
        public void Run()
        {
            Centroids = GenerateRandomCentroids(Clusters);

            //for loop Iterations
            for (_i = 0; _i < Iterations; _i++)
            {
                //storing the old centroids to compare it later on.
                var oldCentroids = Centroids.Values.ToList();

                //assign data set
                AssignDataset();

                //recalculate clusters
                RecalculateCentroids();

                //check if the centroids are still changing
                if (!CentroidsChanged(oldCentroids, Centroids.Values.ToList()))
                    break;
            }
            Sse = CalculateSumofSquaredErrors();
        }

        //assign the vectors to the clusters nearby
        private void AssignDataset()
        {
            Dataset.ForEach(vector => vector.Cluster = GetNearestCluster(vector));
        }

        //get nearest cluster
        private int GetNearestCluster(GenericVector vector)
        {
            var clusterid = Centroids
                .OrderBy(v => GenericVector.Distance(vector, v.Value))
                .Select(v => v.Key)
                .FirstOrDefault();
            return clusterid;
        }

        //generate random centroids for the first time
        private Dictionary<int, GenericVector> GenerateRandomCentroids(int clusters)
        {
            var centroids = new Dictionary<int, GenericVector>();
            var index = 0;
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
            foreach (var centroidkey in Centroids.Keys.ToList())
            {
                var cluster = Dataset.Where(v => v.Cluster == centroidkey).ToList();

                //generating a new genericvector with the mean of the existing vectors
                Centroids[centroidkey] = cluster
                    .Aggregate(new GenericVector(Dataset.First().Size),
                        (current, y) => current.Sum(y))
                    .Divide(cluster.Count);
            }
        }

        //get a random vector
        private GenericVector RandomVector()
        {
            while (true)
            {
                var genericVector = Dataset[_random.Next(Dataset.Count)];
                var found = false;
                if (Centroids != null)
                {
                    for (var i = 0; i < Centroids.Values.ToList().Count; i++)
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
            var clusters = Dataset.GroupBy(v => v.Cluster).ToList().OrderBy(v => v.Key);
            foreach (var cluster in clusters)
            {
                Console.WriteLine("Cluster " + cluster.ElementAt(0).Cluster + " has " + cluster.Count());
            }
        }

        public void PrintClustersInLine()
        {
            var clusters = Dataset.GroupBy(v => v.Cluster).ToList().OrderBy(v => v.Key);
            foreach (var cluster in clusters)
            {
                Console.Write(cluster.ElementAt(0).Cluster + " -> " + cluster.Count() + " || ");
            }
            Console.WriteLine();
        }

        public void PrintClusterInfo()
        {
            Console.WriteLine("KMEANS COMPLETED");
            Console.WriteLine("SSE = \t\t\t\t" + Sse);
            Console.WriteLine("amount of clusters: \t\t" + Clusters);
            Console.WriteLine("amount of max iterations: \t" + Iterations);
            Console.WriteLine("amount of actual iterations: \t" + _i);
            Console.WriteLine();

            var clusters = Dataset.GroupBy(v => v.Cluster).ToList().OrderBy(v => v.Key).ToList();

            foreach (var cluster in clusters)
            {
                Console.WriteLine("***************************************************");
                Console.WriteLine("cluster " + cluster.Key + " has " + cluster.Count() + " items");
                Console.WriteLine("***************************************************");
                var offersBoughtXTimes = new Dictionary<int, int>();
                var clusterpoints = cluster.ToList();
                foreach (var clusterpoint in clusterpoints)
                {
                    for (var j = 0; j < clusterpoint.Size; j++)
                    {
                        if (Math.Abs(clusterpoint.Points[j] - 1) < 0.001)
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
                foreach (var offerBought in offersBoughtXTimes)
                {
                    if (offerBought.Value >= 3)
                        Console.WriteLine(
                            "Offer " + (offerBought.Key + 1) + " \t-> bought " + offerBought.Value + " times ");
                }
            }
            Console.WriteLine();
        }

        public double CalculateSumofSquaredErrors()
        {
            var orderedclusters = Dataset.GroupBy(v => v.Cluster).OrderBy(v => v.Key).ToList();
            var sse = (from cluster in orderedclusters
                let clustercenter = Centroids[cluster.Key]
                from point in cluster
                select Math.Pow(GenericVector.Distance(clustercenter, point), 2)).Sum();
            return sse;
        }
    }
}