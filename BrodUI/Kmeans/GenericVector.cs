using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BrodUI.KMeans
{
    /// <summary>
    /// Class that creates a GenericVector with a list of floats and a cluster integer.
    /// </summary>
    public class GenericVector
    {
        /// <summary>
        /// integer to display the cluster it is in.
        /// </summary>
        public int Cluster;

        /// <summary>
        /// list of floats that's creating the GenericVector
        /// </summary>
        public List<float> Points = new();

        /// <summary>
        /// Creates a new GenericVector with the points as long as the size given
        /// </summary>
        /// <param name="size"></param>
        public GenericVector(int size)
        {
            for (int i = 0; i < size; i++)
            {
                Points.Add(0);
            }
        }

        /// <summary>
        /// Empty constructor for GenericVector
        /// </summary>
        public GenericVector()
        {
        }

        /// <summary>
        /// Method to add a point to the GenericVector
        /// </summary>
        /// <param name="point">Point to add</param>
        public void Add(float point)
        {
            Points.Add(point);
        }

        /// <summary>
        /// Method to add a list of points to the GenericVector
        /// </summary>
        public int Size => Points.Count;

        /// <summary>
        /// Method to add a list of points to the GenericVector
        /// </summary>
        /// <param name="vectorToSum">List of point to add</param>
        /// <returns></returns>
        /// <exception cref="Exception">return an exception if the size of vectorToSum not equal to instance vector size</exception>
        public GenericVector Sum(GenericVector vectorToSum)
        {
            if (Size != vectorToSum.Size)
                throw new Exception("GenericVector size of vectorToSum not equal to instance vector size");

            for (int i = 0; i < Size; i++)
            {
                Points[i] += vectorToSum.Points[i];
            }
            return this;
        }

        /// <summary>
        /// Method to substract a list of points to the GenericVector
        /// </summary>
        /// <returns>Returns the length of the Vector</returns>
        public double VectorLength()
        {
            return (float)Math.Sqrt(Points.Sum(item => Math.Pow(item, 2)));
        }

        /// <summary>
        /// Override ToString-Method to show the content of the GenericVector
        /// </summary>
        /// <returns>result of toString</returns>
        public override string ToString()
        {
            return string.Join("\t", Points.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
        }

        /// <summary>
        /// Method to substract a list of points to the GenericVector
        /// </summary>
        /// <param name="divider">divider of the division</param>
        /// <returns>result of the division</returns>
        public GenericVector Divide(int divider)
        {
            for (int i = 0; i < Size; i++)
            {
                Points[i] /= divider;
            }
            return this;
        }

        /// <summary>
        /// Method to know if two generic vectors are equals
        /// </summary>
        /// <param name="a">first generic vector to compare with the second</param>
        /// <param name="b">second generic vector to compare with the first</param>
        /// <returns>if the 2 parameters are equals</returns>
        public static bool NotEqual(GenericVector a, GenericVector b)
        {
            // return true if two points are not equals
            for (int i = 0; i < a.Size; i++)
            {
                // "NaN != NaN" returns true so we check NaN explicitly
                if (a.Points[i] != b.Points[i] && !(float.IsNaN(a.Points[i]) && float.IsNaN(b.Points[i])))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method that returns the dot product of two GenericVectors
        /// </summary>
        /// <param name="vectorA">first generic vector to to the dot product</param>
        /// <param name="vectorB">second generic vector to to the dot product</param>
        /// <returns>the dot product of the 2 parameters</returns>
        /// <exception cref="Exception">return an exception if GenericVector a size of dotProduct not equal to GenericVector b size</exception>
        public static float DotProduct(GenericVector vectorA, GenericVector vectorB)
        {
            if (vectorA.Size != vectorB.Size)
                throw new Exception("GenericVector a size of dotProduct not equal to GenericVector b size");
            List<float> aTimesBPoints = vectorA.Points.Select((t, i) => t * vectorB.Points[i]).ToList();

            return aTimesBPoints.Sum();
        }

        /// <summary>
        /// Method to calculate the distance between two GenericVectors
        /// </summary>
        /// <param name="a">first generic vector</param>
        /// <param name="b">second generic vector</param>
        /// <returns>the distance between a and b</returns>
        public static double Distance(GenericVector a, GenericVector b)
        {
            int size = a.Size;
            double sum = 0;
            for (int i = 0; i < size; i++)
            {
                double aMinusBPoint = a.Points[i] - b.Points[i];

                sum += aMinusBPoint * aMinusBPoint;
            }

            return Math.Sqrt(sum);
        }
    }
}