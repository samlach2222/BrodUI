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
            const int red = 255;
            const int green = 226;
            const int blue = 226;
            int dmc1 = 3713;
            HslToDmc hslToDmc = new();
            int dmc2 = hslToDmc.GetValDmc(red, green, blue);
            Assert.Equal(dmc1, dmc2);
        }
    }
}
