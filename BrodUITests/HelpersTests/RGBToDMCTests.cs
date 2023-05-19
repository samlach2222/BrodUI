using BrodUI.Helpers;
using Xunit;

namespace BrodUITests.HelpersTests
{
    public class RgbToDmcTests
    {
        [Fact]
        public void InitializationTests()
        {
            // Expected
            const string expected = "0 252 251 248";
            // Actual
            RgbToDmc rgbToDmc = new();
            int d = rgbToDmc.GetValDmc(252, 251, 248);
            string actual = d.ToString();
            actual = actual + " " + rgbToDmc.GetRed(d).ToString() + " " + rgbToDmc.GetGreen(d).ToString() + " " + rgbToDmc.GetBlue(d).ToString();
            // Assert
            Assert.Equal(expected, actual);
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
        [Fact]
        public void GetBlueTests()
        {
            const int blue = 226;
            const int dmc1 = 3713;
            RgbToDmc rgbToDmc = new();
            int blue2 = rgbToDmc.GetBlue(dmc1);
            Assert.Equal(blue, blue2);
        }
        [Fact]
        public void GetGreenTests()
        {
            const int green = 226;
            const int dmc1 = 3713;
            RgbToDmc rgbToDmc = new();
            int g = rgbToDmc.GetGreen(dmc1);
            Assert.Equal(green, g);
        }
        [Fact]
        public void GetRedTests()
        {
            const int red = 255;
            const int dmc1 = 3713;
            RgbToDmc rgbToDmc = new();
            int red2 = rgbToDmc.GetRed(dmc1);
            Assert.Equal(red, red2);
        }
    }
}