using BrodUI.Helpers;
using Xunit;

namespace BrodUITests.HelpersTests
{
    public class HslToDmcTests
    {
        [Fact]
        public void InitializationTests()
        {
            // Expected
            const string expected = "0 45 40 98";
            // Actual
            HslToDmc hslToDmc = new();
            int d = hslToDmc.GetValDmc(45, 40, 98);
            string actual = d.ToString();
            actual = actual + " " + hslToDmc.GetHue(d).ToString() + " " + hslToDmc.GetSaturation(d).ToString() + " " + hslToDmc.GetLightness(d).ToString();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetValDmcTests()
        {
            const int hue = 0;
            const int saturation = 100;
            const int lightness = 94;
            const int dmc1 = 3713;
            HslToDmc hslToDmc = new();
            int dmc2 = hslToDmc.GetValDmc(hue, saturation, lightness);
            Assert.Equal(dmc1, dmc2);
        }

        [Fact]
        public void GetHueTests()
        {
            const int hue = 0;
            const int dmc1 = 3713;
            HslToDmc hslToDmc = new();
            int h = hslToDmc.GetHue(dmc1);
            Assert.Equal(hue, h);
        }
        [Fact]
        public void GetSaturationTests()
        {
            const int saturation = 100;
            const int dmc1 = 3713;
            HslToDmc hslToDmc = new();
            int s = hslToDmc.GetSaturation(dmc1);
            Assert.Equal(saturation, s);
        }
        [Fact]
        public void GetLightnessTests()
        {
            const int lightness = 94;
            const int dmc1 = 3713;
            HslToDmc hslToDmc = new();
            int l = hslToDmc.GetLightness(dmc1);
            Assert.Equal(lightness, l);
        }
    }
}
