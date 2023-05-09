using Xunit;
using hslDMC = BrodUI.Helpers.HSLToDMC;

namespace BrodUITests.HelpersTests
{
    public class HSLToDMCTests
    {
        [Fact]
        public void InitializationTests()
        {

        }
        [Fact]
        public void GetValDmcTests()
        {
            int red1 = 255;
            int green1 = 226;
            int blue1 = 226;
            int dmc1 = 3713;
            int dmc2 = 0;
            hslDMC hslTodmc = new();
            dmc2 = hslTodmc.GetValDmc(red1, green1, blue1);
            Assert.Equal(dmc1, dmc2);
        }
    }
}
