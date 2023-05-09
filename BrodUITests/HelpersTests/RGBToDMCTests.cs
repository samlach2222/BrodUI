using BrodUI.Helpers;
using Xunit;

namespace BrodUITests.HelpersTests
{
    public class RgbToDmcTests
    {
        [Fact]
        public void InitializationTests()
        {

        }
        [Fact]
        public void GetValDmcTests()
        {
            const int red = 255;
            const int green = 226;
            const int blue = 226;
            const int dmc1 = 3713;
            RgbToDmc rgbToDmc = new();
            int dmc2 = rgbToDmc.GetValDmc(red, green, blue);
            Assert.Equal(dmc1, dmc2);
        }
    }
}