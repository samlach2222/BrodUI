using BrodUI.Helpers;
using Xunit;

namespace BrodUITests.HelpersTests
{
    public class HslToDmcTests
    {
        [Fact]
        public void InitializationTests()
        {

        }

        [Fact]
        public void GetValDmcTests()
        {
            const int hue = 0;
            const int saturation = 100;
            const int lightness = 94;
            int dmc1 = 3713;
            HslToDmc hslToDmc = new();
            int dmc2 = hslToDmc.GetValDmc(hue, saturation, lightness);
            Assert.Equal(dmc1, dmc2);
        }
    }
}
