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
            Assert.Equal(actual, expected);
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
            Assert.NotEqual(actual, notExpected);
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void WireSizeBigImageTest()
        {
            // TODO : define a big imageTest (preferably one we know gives wrong values in the export page)
            throw new NotImplementedException();
        }
    }
}