﻿using Xunit;
using BrodUI.Kmeans;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace BrodUITests.KmeansTests
{
    public class KMeansClassTests
    {

        [Fact]
        public void NearestClusterKmeansTest()
        {
            KMeans km = new KMeans();
            MethodInfo GetNearestCluster = typeof(KMeans).GetMethod("GetNearestCluster", BindingFlags.NonPublic | BindingFlags.Instance);
            Dictionary<int, GenericVector> dict = new();
            GenericVector cen1 = new();
            cen1.Add(0);
            cen1.Add(0);
            cen1.Add(255);
            dict.Add(0,cen1);
            GenericVector cen2 = new();
            cen2.Add(0);
            cen2.Add(255);
            cen2.Add(0);
            dict.Add(1,cen2);
            GenericVector cen3 = new();
            cen3.Add(255);
            cen3.Add(0);
            cen3.Add(0);
            dict.Add(2,cen3);
            km.Centroids=dict;
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            GenericVector val2 = new();
            val2.Add(0);
            val2.Add(200);
            val2.Add(0);
            GenericVector val3 = new();
            val3.Add(200);
            val3.Add(100);
            val3.Add(50);
            int actual1 = (int)GetNearestCluster.Invoke(km, new object[] { val1 });
            int actual2 = (int)GetNearestCluster.Invoke(km, new object[] { val2 });
            int actual3 = (int)GetNearestCluster.Invoke(km, new object[] { val3 });
            Assert.Equal(0,actual1);
            Assert.Equal(1,actual2);
            Assert.Equal(2,actual3);
        }

        [Fact]
        public void AssignDatasetKmeansTest()
        {
            KMeans km = new KMeans();
            MethodInfo AssignDataset = typeof(KMeans).GetMethod("AssignDataset", BindingFlags.NonPublic | BindingFlags.Instance);
            Dictionary<int, GenericVector> dict = new();
            GenericVector cen1 = new();
            cen1.Add(0);
            cen1.Add(0);
            cen1.Add(255);
            dict.Add(0,cen1);
            GenericVector cen2 = new();
            cen2.Add(0);
            cen2.Add(255);
            cen2.Add(0);
            dict.Add(1,cen2);
            GenericVector cen3 = new();
            cen3.Add(255);
            cen3.Add(0);
            cen3.Add(0);
            dict.Add(2,cen3);
            km.Centroids=dict;
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            GenericVector val2 = new();
            val2.Add(0);
            val2.Add(200);
            val2.Add(0);
            GenericVector val3 = new();
            val3.Add(200);
            val3.Add(100);
            val3.Add(50);
            km.Dataset = new();
            km.Dataset.Add(val1);
            km.Dataset.Add(val2);
            km.Dataset.Add(val3);
            AssignDataset.Invoke(km, new object[] { });
            Assert.Equal(0,km.Dataset[0].Cluster);
            Assert.Equal(1,km.Dataset[1].Cluster);
            Assert.Equal(2,km.Dataset[2].Cluster);
        }


        [Fact]
        public void RandomVectorKmeansTest()
        {
            KMeans km = new KMeans();
            MethodInfo RandomVector = typeof(KMeans).GetMethod("RandomVector", BindingFlags.NonPublic | BindingFlags.Instance);
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            km.Dataset = new();
            km.Dataset.Add(val1);
            GenericVector actual = (GenericVector)RandomVector.Invoke(km, new object[] { });
            Assert.Equal(val1.Points[0],actual.Points[0]);
            Assert.Equal(val1.Points[1],actual.Points[1]);
            Assert.Equal(val1.Points[2],actual.Points[2]);
            GenericVector val2 = new();
            val2.Add(0);
            val2.Add(200);
            val2.Add(0);
            km.Dataset.Add(val2);
            var verif = false;
            int n = 0;
            while(n<10 && !verif)
            {
                actual = (GenericVector)RandomVector.Invoke(km, new object[] { });
                verif = verif || (val2.Points[0] == actual.Points[0] && val2.Points[1] == actual.Points[1] && val2.Points[2] == actual.Points[2]);
            }
            Assert.True(verif);
        }

        [Fact]
        public void RandomCentroidsKmeansTest()
        {
            KMeans km = new KMeans();
            MethodInfo GenerateRandomCentroids = typeof(KMeans).GetMethod("GenerateRandomCentroids", BindingFlags.NonPublic | BindingFlags.Instance);
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            GenericVector val2 = new();
            val2.Add(0);
            val2.Add(200);
            val2.Add(0);
            km.Dataset = new();
            km.Dataset.Add(val1);
            km.Dataset.Add(val2);
            Dictionary<int, GenericVector> dict = (Dictionary<int, GenericVector>)GenerateRandomCentroids.Invoke(km, new object[] { 2});
            Assert.Equal(2,dict.Count);
            GenericVector cen1 = dict[0];
            var verif = cen1.Points[0] == val1.Points[0] && cen1.Points[1] == val1.Points[1] && cen1.Points[2] == val1.Points[2];
            verif = verif || (cen1.Points[0] == val2.Points[0] && cen1.Points[1] == val2.Points[1] && cen1.Points[2] == val2.Points[2]);
            Assert.True(verif);
            GenericVector cen2 = dict[0];
            verif = cen2.Points[0] == val1.Points[0] && cen2.Points[1] == val1.Points[1] && cen2.Points[2] == val1.Points[2];
            verif = verif || (cen2.Points[0] == val2.Points[0] && cen2.Points[1] == val2.Points[1] && cen2.Points[2] == val2.Points[2]);
            Assert.True(verif);
        }

        [Fact]
        public void CentroidsChangedKmeansTest()
        {
            KMeans km = new KMeans();
            MethodInfo CentroidsChanged = typeof(KMeans).GetMethod("CentroidsChanged", BindingFlags.NonPublic | BindingFlags.Static);
            Dictionary<int, GenericVector> dict = new();
            GenericVector cen1 = new();
            cen1.Add(0);
            cen1.Add(0);
            cen1.Add(255);
            dict.Add(0,cen1);
            GenericVector cen2 = new();
            cen2.Add(0);
            cen2.Add(255);
            cen2.Add(0);
            dict.Add(1,cen2);
            GenericVector cen3 = new();
            cen3.Add(255);
            cen3.Add(0);
            cen3.Add(0);
            dict.Add(2,cen3);
            km.Centroids=dict;
            bool actual = (bool)CentroidsChanged.Invoke(km, new object[]{dict.Values.ToList(), km.Centroids.Values.ToList()});
            Assert.False(actual);
            Dictionary<int, GenericVector> dict2 = new();
            GenericVector cen4 = new();
            cen4.Add(0);
            cen4.Add(0);
            cen4.Add(200);
            dict2.Add(0,cen4);
            dict2.Add(1,cen2);
            dict2.Add(2,cen3);
            actual = (bool)CentroidsChanged.Invoke(km, new object[]{dict2.Values.ToList(), km.Centroids.Values.ToList()});
            Assert.True(actual);
        }

        [Fact]
        public void RecalculateCentroidsKmeansTest()
        {
            KMeans km = new KMeans();
            MethodInfo RecalculateCentroids = typeof(KMeans).GetMethod("RecalculateCentroids", BindingFlags.NonPublic | BindingFlags.Instance);
            Dictionary<int, GenericVector> dict = new();
            GenericVector cen1 = new();
            cen1.Add(0);
            cen1.Add(0);
            cen1.Add(255);
            dict.Add(0,cen1);
            GenericVector cen2 = new();
            cen2.Add(0);
            cen2.Add(255);
            cen2.Add(0);
            dict.Add(1,cen2);
            km.Centroids=dict;
            km.Dataset = new();
            GenericVector val1_1 = new();
            val1_1.Add(0);
            val1_1.Add(0);
            val1_1.Add(200);
            val1_1.Cluster=0;
            GenericVector val1_2 = new();
            val1_2.Add(0);
            val1_2.Add(0);
            val1_2.Add(100);
            val1_2.Cluster=0;
            GenericVector exp1 = new();
            exp1.Add(0);
            exp1.Add(0);
            exp1.Add(150);
            GenericVector val2_1 = new();
            val2_1.Add(0);
            val2_1.Add(50);
            val2_1.Add(0);
            val2_1.Cluster=1;
            GenericVector val2_2 = new();
            val2_2.Add(0);
            val2_2.Add(150);
            val2_2.Add(0);
            val2_2.Cluster=1;
            GenericVector exp2 = new();
            exp2.Add(0);
            exp2.Add(100);
            exp2.Add(0);
            km.Dataset.Add(val1_1);
            km.Dataset.Add(val1_2);
            km.Dataset.Add(val2_1);
            km.Dataset.Add(val2_2);
            RecalculateCentroids.Invoke(km, new object[]{});
            cen1 = km.Centroids[0];
            cen2 = km.Centroids[1];
            Assert.False(GenericVector.NotEqual(cen1,exp1));
            Assert.False(GenericVector.NotEqual(cen2,exp2));
        }

        [Fact]
        public void ErrorsCalculationsKmeansTest()
        {
            List<GenericVector> data = new();
            Dictionary<int,GenericVector> dict = new();
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            val1.Cluster=0;
            data.Add(val1);
            GenericVector cen1 = new();
            cen1.Add(0);
            cen1.Add(0);
            cen1.Add(255);
            dict.Add(0,cen1);
            KMeans km = new KMeans{Dataset = data, Centroids = dict};
            Assert.True(Math.Abs(km.CalculateSumofSquaredErrors())<0.0000001);//Egal à 0
            GenericVector val2 = new();
            val2.Add(245);
            val2.Add(0);
            val2.Add(0);
            val2.Cluster=1;
            data.Add(val2);
            GenericVector cen2 = new();
            cen2.Add(255);
            cen2.Add(0);
            cen2.Add(0);
            dict.Add(1,cen2);
            Assert.True(Math.Abs(100-km.CalculateSumofSquaredErrors())<0.0000001);//Egal à 100
            GenericVector val3 = new();
            val3.Add(10);
            val3.Add(10);
            val3.Add(10);
            val3.Cluster=2;
            data.Add(val3);
            GenericVector cen3 = new();
            cen3.Add(0);
            cen3.Add(0);
            cen3.Add(0);
            dict.Add(2,cen3);
            Assert.True(Math.Abs(400-km.CalculateSumofSquaredErrors())<0.0000001);//Egal à 400
        }

        [Fact]
        public void ThreeColorsKmeansRunTest()
        {
            List<GenericVector> data = new();
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            GenericVector val2 = new();
            val2.Add(255);
            val2.Add(0);
            val2.Add(0);
            GenericVector val3 = new();
            val3.Add(0);
            val3.Add(255);
            val3.Add(0);
            data.Add(val1);
            data.Add(val2);
            data.Add(val3);
            var kMeanses = new List<KMeans>();
            for (var i = 0; i < 30; i++)
            {
                KMeans kMeans = new KMeans{
                    Iterations = 100,
                    Dataset = data,
                    Clusters = 3
                };
                kMeans.Run();
                kMeanses.Add(kMeans);
            }
            // We keep the lowest SSE
            var lowest = kMeanses.Aggregate((minItem, nextItem) => minItem.Sse < nextItem.Sse ? minItem : nextItem);
            Assert.Equal(0,lowest.Sse); //Signifie que les centroids sont égaux aux données
        }

        [Fact]
        public void OneColorKmeansRunTest()
        {
            List<GenericVector> data = new();
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            data.Add(val1);
            var kMeanses = new List<KMeans>();
            for (var i = 0; i < 3; i++)
            {
                KMeans kMeans = new KMeans{
                    Iterations = 100,
                    Dataset = data,
                    Clusters = 3
                };
                kMeans.Run();
                kMeanses.Add(kMeans);
            }
            // We keep the lowest SSE
            var lowest = kMeanses.Aggregate((minItem, nextItem) => minItem.Sse < nextItem.Sse ? minItem : nextItem);
            bool verif = (!GenericVector.NotEqual(val1,lowest.Centroids[0]) && GenericVector.NotEqual(val1,lowest.Centroids[1]) && GenericVector.NotEqual(val1,lowest.Centroids[2]))
            || (!GenericVector.NotEqual(val1,lowest.Centroids[1]) && GenericVector.NotEqual(val1,lowest.Centroids[0]) && GenericVector.NotEqual(val1,lowest.Centroids[2]))
            || (!GenericVector.NotEqual(val1,lowest.Centroids[2]) && GenericVector.NotEqual(val1,lowest.Centroids[1]) && GenericVector.NotEqual(val1,lowest.Centroids[0]));
            Assert.True(verif); //Il n'y qu'un centroids qui vaut la couleur, les autres ont des coordonnées NaN
        }

        [Fact]
        public void MoreColorsKmeansRunTest()
        {
            List<GenericVector> data = new();
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            GenericVector val2 = new();
            val2.Add(255);
            val2.Add(0);
            val2.Add(0);
            GenericVector val3 = new();
            val3.Add(0);
            val3.Add(255);
            val3.Add(0);
            data.Add(val1);
            data.Add(val2);
            data.Add(val3);
            float a = 200;
            for(var i=0; i<10; i++)
            {
                var gen = new GenericVector();
                gen.Add(a);
                gen.Add(0);
                gen.Add(0);
                data.Add(gen);
                a++;
            }
            a = 205;
            for(var i=0; i<10; i++)
            {
                var gen = new GenericVector();
                gen.Add(0);
                gen.Add(a);
                gen.Add(0);
                data.Add(gen);
                a++;  
            }
            a = 195;
            for(var i=0; i<10; i++)
            {
                var gen = new GenericVector();
                gen.Add(0);
                gen.Add(0);
                gen.Add(a);
                data.Add(gen);
                a++;
            }
            var kMeanses = new List<KMeans>();
            for (var i = 0; i < 30; i++)
            {
                KMeans kMeans = new KMeans{
                    Iterations = 100,
                    Dataset = data,
                    Clusters = 3
                };
                kMeans.Run();
                kMeanses.Add(kMeans);
            }
            // We keep the lowest SSE
            var lowest = kMeanses.Aggregate((minItem, nextItem) => minItem.Sse < nextItem.Sse ? minItem : nextItem);

            //To facilitate the test, we associate each data with its centroids so that we can get the 3 centroids in the order we want (for the Asserts) without having to try all the possibilities
            MethodInfo AssignDataset = typeof(KMeans).GetMethod("AssignDataset", BindingFlags.NonPublic | BindingFlags.Instance);
            AssignDataset.Invoke(lowest, new object[] { });

            GenericVector cen1 = lowest.Centroids[lowest.Dataset[1].Cluster];
            GenericVector cen2 = lowest.Centroids[lowest.Dataset[2].Cluster];
            GenericVector cen3 = lowest.Centroids[lowest.Dataset[0].Cluster];
            Assert.True(cen1.Points[0]>200 && cen1.Points[0]<255 && cen1.Points[1]==0 && cen1.Points[2]==0);
            Assert.True(cen2.Points[1]>200 && cen2.Points[1]<255 && cen2.Points[0]==0 && cen2.Points[2]==0);
            Assert.True(cen3.Points[2]>200 && cen1.Points[2]<255 && cen3.Points[1]==0 && cen3.Points[0]==0);
        }
    }
}