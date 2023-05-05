using System;
using System.Collections.Generic;
using System.Linq;

namespace KmeansAlgorithm
{
    public class GenericVector
    {
        //integer to display the cluster it is in.
        public int Cluster;

        //list of floats that's creating the GenericVector
        public List<float> Points = new List<float>();

        //CONSTRUCTORS
        //Creates a new GenericVector with the points given
        public GenericVector(List<float> points)
        {
            Points = points;
        }

        //Creates a new GeneriVector with the points as long as the size given
        public GenericVector(int size)
        {
            for (var i = 0; i < size; i++)
            {
                Points.Add(0);
            }
        }

        public GenericVector()
        {
        }

        //GenericVector METHODS
        public void Add(float point)
        {
            Points.Add(point);
        }

        public int Size => Points.Count;

        public GenericVector Sum(GenericVector vectorToSum)
        {
            if (Size != vectorToSum.Size)
                throw new Exception("GenericVector size of vectorToSum not equal to instance vector size");

            for (var i = 0; i < Size; i++)
            {
                Points[i] += vectorToSum.Points[i];
            }
            return this;
        }

        //Returns the length of the Vector
        public double VectorLength()
        {
            return (float) Math.Sqrt(Points.Sum(item => Math.Pow(item, 2)));
        }

        //Override ToString-Method to show the content of the GenericVector
        public override string ToString()
        {
            return string.Join("\t", Points.Select(x => x.ToString()).ToArray());
        }

        public float BiggestPoint()
        {
            return Points.Max();
        }

        public GenericVector Divide(int divider)
        {
            for (var i = 0; i < Size; i++)
            {
                Points[i] = Points[i] / divider;
            }
            return this;
        }

        //STATIC GENERICVECTOR METHODS
        public static GenericVector Sum(GenericVector vectorA, GenericVector vectorB)
        {
            if (vectorA.Size != vectorB.Size)
                throw new Exception("GenericVector size of vectorToSum not equal to instance vector size");

            var summedVector = new GenericVector(vectorA.Size);

            for (var i = 0; i < summedVector.Size; i++)
            {
                summedVector.Points[i] = vectorA.Points[i] + vectorB.Points[i];
            }
            return summedVector;
        }

        public static bool NotEqual(GenericVector a, GenericVector b)
        {
            return a.Points.Where((value, index) => value != b.Points[index]).Any();
        }

        public static float DotProduct(GenericVector vectorA, GenericVector vectorB)
        {
            if (vectorA.Size != vectorB.Size)
                throw new Exception("GenericVector a size of dotProduct not equal to GenericVector b size");
            var aTimesBpoints = vectorA.Points.Select((t, i) => t * vectorB.Points[i]).ToList();

            return aTimesBpoints.Sum();
        }

        public static float GetAngle(GenericVector a, GenericVector b)
        {
            var x = DotProduct(a, b) / (a.VectorLength() * b.VectorLength());
            if (x > 1 || x < -1)
                return 0;
            return (float) Math.Acos(x);
        }

        public static double Distance(GenericVector a, GenericVector b)
        {
            var aMinusBpoints = a.Points.Select((t, i) => t - b.Points[i]).ToList();

            return Math.Sqrt(aMinusBpoints.Sum(item => Math.Pow(item, 2)));
        }
    }
}