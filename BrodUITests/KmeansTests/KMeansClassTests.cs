using BrodUI.KMeans;
using System.Reflection;
using Xunit;

namespace BrodUITests.KMeansTests
{
    public class KMeansClassTests
    {

        [Fact]
        public void NearestClusterKMeansTest()
        {
            KMeans km = new();
            MethodInfo? getNearestCluster = typeof(KMeans).GetMethod("GetNearestCluster", BindingFlags.NonPublic | BindingFlags.Instance);
            Dictionary<int, GenericVector> dict = new();
            GenericVector cen1 = new();
            cen1.Add(0);
            cen1.Add(0);
            cen1.Add(255);
            dict.Add(0, cen1);
            GenericVector cen2 = new();
            cen2.Add(0);
            cen2.Add(255);
            cen2.Add(0);
            dict.Add(1, cen2);
            GenericVector cen3 = new();
            cen3.Add(255);
            cen3.Add(0);
            cen3.Add(0);
            dict.Add(2, cen3);
            km.Centroids = dict;
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
            if (getNearestCluster == null) return;
            int actual1 = (int)getNearestCluster.Invoke(km, new object[] { val1 })!;
            int actual2 = (int)getNearestCluster.Invoke(km, new object[] { val2 })!;
            int actual3 = (int)getNearestCluster.Invoke(km, new object[] { val3 })!;
            Assert.Equal(0, actual1);
            Assert.Equal(1, actual2);
            Assert.Equal(2, actual3);
        }

        [Fact]
        public void AssignDataSetKMeansTest()
        {
            KMeans km = new();
            MethodInfo? assignDataSet = typeof(KMeans).GetMethod("AssignDataset", BindingFlags.NonPublic | BindingFlags.Instance);
            Dictionary<int, GenericVector> dict = new();
            GenericVector cen1 = new();
            cen1.Add(0);
            cen1.Add(0);
            cen1.Add(255);
            dict.Add(0, cen1);
            GenericVector cen2 = new();
            cen2.Add(0);
            cen2.Add(255);
            cen2.Add(0);
            dict.Add(1, cen2);
            GenericVector cen3 = new();
            cen3.Add(255);
            cen3.Add(0);
            cen3.Add(0);
            dict.Add(2, cen3);
            km.Centroids = dict;
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
            km.DataSet = new List<GenericVector>
            {
                val1,
                val2,
                val3
            };
            if (assignDataSet != null)
            {
                assignDataSet.Invoke(km, new object[] { });
            }
            Assert.Equal(0, km.DataSet[0].Cluster);
            Assert.Equal(1, km.DataSet[1].Cluster);
            Assert.Equal(2, km.DataSet[2].Cluster);
        }


        [Fact]
        public void RandomVectorKMeansTest()
        {
            KMeans km = new();
            MethodInfo? randomVector = typeof(KMeans).GetMethod("RandomVector", BindingFlags.NonPublic | BindingFlags.Instance);
            //GenericVector? actual = (GenericVector)randomVector!.Invoke(km, new object[] { })!;
            //Assert.Null(actual);
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            km.DataSet = new() { val1 };
            GenericVector? actual = (GenericVector)randomVector!.Invoke(km, new object[] { })!;
            Assert.Equal(val1.Points[0], actual.Points[0]);
            Assert.Equal(val1.Points[1], actual.Points[1]);
            Assert.Equal(val1.Points[2], actual.Points[2]);
            km.Centroids.Add(0, actual);
            GenericVector val2 = new();
            val2.Add(0);
            val2.Add(200);
            val2.Add(0);
            km.DataSet.Add(val2);
            bool check = false;
            actual = (GenericVector)randomVector.Invoke(km, new object[] { })!;
            check = check || (val2.Points[0] == actual.Points[0] && val2.Points[1] == actual.Points[1] && val2.Points[2] == actual.Points[2]);
            Assert.True(check);
        }

        [Fact]
        public void RandomCentroidsKMeansTest()
        {
            KMeans km = new();
            MethodInfo GenerateRandomCentroids = typeof(KMeans).GetMethod("GenerateRandomCentroids", BindingFlags.NonPublic | BindingFlags.Instance);
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            GenericVector val2 = new();
            val2.Add(0);
            val2.Add(200);
            val2.Add(0);
            km.DataSet = new();
            km.DataSet.Add(val1);
            km.DataSet.Add(val2);
            Dictionary<int, GenericVector> dict = (Dictionary<int, GenericVector>)GenerateRandomCentroids.Invoke(km, new object[] { 2 });
            Assert.Equal(2, dict.Count);
            GenericVector cen1 = dict[0];
            bool verif = cen1.Points[0] == val1.Points[0] && cen1.Points[1] == val1.Points[1] && cen1.Points[2] == val1.Points[2];
            verif = verif || (cen1.Points[0] == val2.Points[0] && cen1.Points[1] == val2.Points[1] && cen1.Points[2] == val2.Points[2]);
            Assert.True(verif);
            GenericVector cen2 = dict[0];
            verif = cen2.Points[0] == val1.Points[0] && cen2.Points[1] == val1.Points[1] && cen2.Points[2] == val1.Points[2];
            verif = verif || (cen2.Points[0] == val2.Points[0] && cen2.Points[1] == val2.Points[1] && cen2.Points[2] == val2.Points[2]);
            Assert.True(verif);
        }

        [Fact]
        public void CentroidsChangedKMeansTest()
        {
            KMeans km = new();
            MethodInfo CentroidsChanged = typeof(KMeans).GetMethod("CentroidsChanged", BindingFlags.NonPublic | BindingFlags.Static);
            Dictionary<int, GenericVector> dict = new();
            GenericVector cen1 = new();
            cen1.Add(0);
            cen1.Add(0);
            cen1.Add(255);
            dict.Add(0, cen1);
            GenericVector cen2 = new();
            cen2.Add(0);
            cen2.Add(255);
            cen2.Add(0);
            dict.Add(1, cen2);
            GenericVector cen3 = new();
            cen3.Add(255);
            cen3.Add(0);
            cen3.Add(0);
            dict.Add(2, cen3);
            km.Centroids = dict;
            bool actual = (bool)CentroidsChanged.Invoke(km, new object[] { dict.Values.ToList(), km.Centroids.Values.ToList() });
            Assert.False(actual);
            Dictionary<int, GenericVector> dict2 = new();
            GenericVector cen4 = new();
            cen4.Add(0);
            cen4.Add(0);
            cen4.Add(200);
            dict2.Add(0, cen4);
            dict2.Add(1, cen2);
            dict2.Add(2, cen3);
            actual = (bool)CentroidsChanged.Invoke(km, new object[] { dict2.Values.ToList(), km.Centroids.Values.ToList() });
            Assert.True(actual);
        }

        [Fact]
        public void RecalculateCentroidsKMeansTest()
        {
            KMeans km = new();
            MethodInfo RecalculateCentroids = typeof(KMeans).GetMethod("RecalculateCentroids", BindingFlags.NonPublic | BindingFlags.Instance);
            RecalculateCentroids.Invoke(km, new object[] { });
            Assert.Null(km.Centroids);
            Dictionary<int, GenericVector> dict = new();
            GenericVector cen1 = new();
            cen1.Add(0);
            cen1.Add(0);
            cen1.Add(255);
            dict.Add(0, cen1);
            GenericVector cen2 = new();
            cen2.Add(0);
            cen2.Add(255);
            cen2.Add(0);
            dict.Add(1, cen2);
            km.Centroids = dict;
            RecalculateCentroids.Invoke(km, new object[] { });
            Assert.False(GenericVector.NotEqual(cen1, km.Centroids[0]));
            Assert.False(GenericVector.NotEqual(cen2, km.Centroids[1]));
            km.DataSet = new();
            GenericVector val1_1 = new();
            val1_1.Add(0);
            val1_1.Add(0);
            val1_1.Add(200);
            val1_1.Cluster = 0;
            GenericVector val1_2 = new();
            val1_2.Add(0);
            val1_2.Add(0);
            val1_2.Add(100);
            val1_2.Cluster = 0;
            GenericVector exp1 = new();
            exp1.Add(0);
            exp1.Add(0);
            exp1.Add(150);
            GenericVector val2_1 = new();
            val2_1.Add(0);
            val2_1.Add(50);
            val2_1.Add(0);
            val2_1.Cluster = 1;
            GenericVector val2_2 = new();
            val2_2.Add(0);
            val2_2.Add(150);
            val2_2.Add(0);
            val2_2.Cluster = 1;
            GenericVector exp2 = new();
            exp2.Add(0);
            exp2.Add(100);
            exp2.Add(0);
            km.DataSet.Add(val1_1);
            km.DataSet.Add(val1_2);
            km.DataSet.Add(val2_1);
            km.DataSet.Add(val2_2);
            RecalculateCentroids.Invoke(km, new object[] { });
            cen1 = km.Centroids[0];
            cen2 = km.Centroids[1];
            Assert.False(GenericVector.NotEqual(cen1, exp1));
            Assert.False(GenericVector.NotEqual(cen2, exp2));
        }

        [Fact]
        public void ErrorsCalculationsKMeansTest()
        {
            List<GenericVector> data = new();
            Dictionary<int, GenericVector> dict = new();
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            val1.Cluster = 0;
            data.Add(val1);
            GenericVector cen1 = new();
            cen1.Add(0);
            cen1.Add(0);
            cen1.Add(255);
            dict.Add(0, cen1);
            KMeans km = new() { DataSet = data, Centroids = dict };
            Assert.True(Math.Abs(km.CalculateSumOfSquaredErrors()) < 0.0000001);//Egal à 0
            GenericVector val2 = new();
            val2.Add(245);
            val2.Add(0);
            val2.Add(0);
            val2.Cluster = 1;
            data.Add(val2);
            GenericVector cen2 = new();
            cen2.Add(255);
            cen2.Add(0);
            cen2.Add(0);
            dict.Add(1, cen2);
            Assert.True(Math.Abs(100 - km.CalculateSumOfSquaredErrors()) < 0.0000001);//Egal à 100
            GenericVector val3 = new();
            val3.Add(10);
            val3.Add(10);
            val3.Add(10);
            val3.Cluster = 2;
            data.Add(val3);
            GenericVector cen3 = new();
            cen3.Add(0);
            cen3.Add(0);
            cen3.Add(0);
            dict.Add(2, cen3);
            Assert.True(Math.Abs(400 - km.CalculateSumOfSquaredErrors()) < 0.0000001);//Egal à 400
        }

        [Fact]
        public void ThreeColorsKMeansRunTest()
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
            List<KMeans> kMeanses = new();
            for (int i = 0; i < 30; i++)
            {
                KMeans kMeans = new()
                {
                    Iterations = 100,
                    DataSet = data,
                    Clusters = 3
                };
                kMeans.Run();
                kMeanses.Add(kMeans);
            }
            // We keep the lowest SSE
            KMeans lowest = kMeanses.Aggregate((minItem, nextItem) => minItem.Sse < nextItem.Sse ? minItem : nextItem);
            Assert.Equal(0, lowest.Sse); //Signifie que les centroids sont égaux aux données
        }

        [Fact]
        public void OneColorKMeansRunTest()
        {
            List<GenericVector> data = new();
            GenericVector val1 = new();
            val1.Add(0);
            val1.Add(0);
            val1.Add(255);
            data.Add(val1);
            List<KMeans> kMeanses = new();
            for (int i = 0; i < 3; i++)
            {
                KMeans kMeans = new()
                {
                    Iterations = 100,
                    DataSet = data,
                    Clusters = 3
                };
                kMeans.Run();
                kMeanses.Add(kMeans);
            }
            // We keep the lowest SSE
            KMeans lowest = kMeanses.Aggregate((minItem, nextItem) => minItem.Sse < nextItem.Sse ? minItem : nextItem);
            bool verif = (!GenericVector.NotEqual(val1, lowest.Centroids[0]) && GenericVector.NotEqual(val1, lowest.Centroids[1]) && GenericVector.NotEqual(val1, lowest.Centroids[2]))
            || (!GenericVector.NotEqual(val1, lowest.Centroids[1]) && GenericVector.NotEqual(val1, lowest.Centroids[0]) && GenericVector.NotEqual(val1, lowest.Centroids[2]))
            || (!GenericVector.NotEqual(val1, lowest.Centroids[2]) && GenericVector.NotEqual(val1, lowest.Centroids[1]) && GenericVector.NotEqual(val1, lowest.Centroids[0]));
            Assert.True(verif); //Il n'y qu'un centroids qui vaut la couleur, les autres ont des coordonnées NaN
        }

        [Fact]
        public void MoreColorsKMeansRunTest()
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
            for (int i = 0; i < 10; i++)
            {
                GenericVector gen = new();
                gen.Add(a);
                gen.Add(0);
                gen.Add(0);
                data.Add(gen);
                a++;
            }
            a = 205;
            for (int i = 0; i < 10; i++)
            {
                GenericVector gen = new();
                gen.Add(0);
                gen.Add(a);
                gen.Add(0);
                data.Add(gen);
                a++;
            }
            a = 195;
            for (int i = 0; i < 10; i++)
            {
                GenericVector gen = new();
                gen.Add(0);
                gen.Add(0);
                gen.Add(a);
                data.Add(gen);
                a++;
            }
            List<KMeans> kMeanses = new();
            for (int i = 0; i < 30; i++)
            {
                KMeans kMeans = new()
                {
                    Iterations = 100,
                    DataSet = data,
                    Clusters = 3
                };
                kMeans.Run();
                kMeanses.Add(kMeans);
            }
            // We keep the lowest SSE
            KMeans lowest = kMeanses.Aggregate((minItem, nextItem) => minItem.Sse < nextItem.Sse ? minItem : nextItem);

            //To facilitate the test, we associate each data with its centroids so that we can get the 3 centroids in the order we want (for the Asserts) without having to try all the possibilities
            MethodInfo AssignDataset = typeof(KMeans).GetMethod("AssignDataset", BindingFlags.NonPublic | BindingFlags.Instance);
            AssignDataset.Invoke(lowest, new object[] { });

            GenericVector cen1 = lowest.Centroids[lowest.DataSet[1].Cluster];
            GenericVector cen2 = lowest.Centroids[lowest.DataSet[2].Cluster];
            GenericVector cen3 = lowest.Centroids[lowest.DataSet[0].Cluster];
            Assert.True(cen1.Points[0] > 200 && cen1.Points[0] < 255 && cen1.Points[1] == 0 && cen1.Points[2] == 0);
            Assert.True(cen2.Points[1] > 200 && cen2.Points[1] < 255 && cen2.Points[0] == 0 && cen2.Points[2] == 0);
            Assert.True(cen3.Points[2] > 200 && cen1.Points[2] < 255 && cen3.Points[1] == 0 && cen3.Points[0] == 0);
        }
    }
}