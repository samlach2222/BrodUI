using BrodUI.Kmeans;
using Xunit;

namespace BrodUITests.KmeansTests
{
    public class GenericVectorsTests
    {
        [Fact]
        public void Add1PointToVectorTest()
        {
            GenericVector vec = new();
            float expected = 125;
            vec.Add(expected);
            float actual = vec.Points[0];
            Assert.Equal(expected, actual);
            Assert.Single(vec.Points);
            Assert.Equal(1, vec.Size);
        }

        [Fact]
        public void SumOfVectorsTest()
        {
            GenericVector vec1 = new();
            GenericVector vec2 = new();
            GenericVector expected = new();
            vec1.Add(1);
            vec1.Add(2);
            vec2.Add(2);
            vec2.Add(4);
            expected.Add(3);
            expected.Add(6);
            GenericVector actual = vec1.Sum(vec2);
            Assert.Equal(expected.Points[0], actual.Points[0]);
            Assert.Equal(expected.Points[1], actual.Points[1]);
        }

        [Fact]
        public void VectorLengthTest()
        {
            GenericVector vec = new();
            vec.Add(3);
            vec.Add(4);
            double actual = vec.VectorLength();
            Assert.Equal(5, actual);
        }

        [Fact]
        public void VectorDivideTest()
        {
            GenericVector vec = new();
            vec.Add(3);
            vec.Add(4);
            GenericVector actual = vec.Divide(2);
            GenericVector expected = new();
            expected.Add((float)1.5);
            expected.Add(2);
            Assert.Equal(expected.Points[0], actual.Points[0]);
            Assert.Equal(expected.Points[1], actual.Points[1]);
        }

        [Fact]
        public void DifferentVectorsTest()
        {
            GenericVector vec1 = new();
            GenericVector vec2 = new();
            GenericVector vec3 = new();
            vec1.Add(2);
            vec1.Add(3);
            vec2.Add(2);
            vec2.Add(3);
            vec3.Add(3);
            vec3.Add(3);
            Assert.True(GenericVector.NotEqual(vec1, vec3));
            Assert.False(GenericVector.NotEqual(vec1, vec2));
        }

        [Fact]
        public void VectorDotProductTest()
        {
            GenericVector vec1 = new();
            GenericVector vec2 = new();
            vec1.Add(1);
            vec1.Add(1);
            vec1.Add(2);
            vec2.Add(2);
            vec2.Add(1);
            vec2.Add(2);
            float expected = vec1.Points[0] * vec2.Points[0] + vec1.Points[1] * vec2.Points[1] + vec1.Points[2] * vec2.Points[2];
            float actual = GenericVector.DotProduct(vec1, vec2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VectorDistanceTest()
        {
            GenericVector vec1 = new();
            GenericVector vec2 = new();
            vec1.Add(1);
            vec1.Add(1);
            vec1.Add(2);
            vec2.Add(2);
            vec2.Add(1);
            vec2.Add(2);
            double expected = (vec1.Points[0] - vec2.Points[0]) * (vec1.Points[0] - vec2.Points[0]) + (vec1.Points[1] - vec2.Points[1]) * (vec1.Points[1] - vec2.Points[1]) + (vec1.Points[2] - vec2.Points[2]) * (vec1.Points[2] - vec2.Points[2]);
            double actual = GenericVector.Distance(vec1, vec2);
            Assert.Equal(expected, actual);
        }
    }
}