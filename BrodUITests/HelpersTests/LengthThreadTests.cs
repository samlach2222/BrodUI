using BrodUI.Helpers;
using Xunit;

namespace BrodUITests.HelpersTests
{
    public class LengthThreadTests
    {
        [Fact]
        public void WireSizeTest()
        {
            int[,] imageTest = { { 3, 4, 3 }, { 3, 3, 5 }, { 5, 4, 3 }, { 4, 3, 3 } };
            LengthThread l = new(3, imageTest);
            double actual = l.TotalLength;
            double expected = 27 + (7 * Math.Sqrt(2));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WireSizeDirectionTest()
        {
            int[,] imageTest = {
                { 1, 1, 2 },
                { 2, 3, 1 }
            };
            LengthThread l = new(1, imageTest);
            double actual = l.TotalLength;
            // Calculating length horizontally
            double expected = (LengthThread.Knot + (2 * LengthThread.Cross) + LengthThread.Knot) + (LengthThread.Knot + LengthThread.Cross + LengthThread.Knot);
            // Calculating length vertically : wrong !
            double notExpected = 3 * (LengthThread.Knot + LengthThread.Cross + LengthThread.Knot);
            Assert.NotEqual(notExpected, actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WireSizeConsecutivePixelsTest()
        {
            // One consecutive pixel
            int[,] imageTest = {
                { 1 }
            };
            LengthThread l = new(1, imageTest);
            double actual = l.TotalLength;
            double expected = (LengthThread.Knot + (1 * LengthThread.Cross) + LengthThread.Knot);
            Assert.Equal(expected, actual);

            // Two consecutive pixels
            imageTest = new int[,] {
                { 1,1 }
            };
            l = new(1, imageTest);
            actual = l.TotalLength;
            expected = (LengthThread.Knot + (2 * LengthThread.Cross) + LengthThread.Knot);
            Assert.Equal(expected, actual);

            // Three consecutive pixels
            imageTest = new int[,] {
                { 1,1,1 }
            };
            l = new(1, imageTest);
            actual = l.TotalLength;
            expected = (LengthThread.Knot + (3 * LengthThread.Cross) + LengthThread.Knot);
            Assert.Equal(expected, actual); // TODO : actual value is wrong, fix bug!
        }
    }
}