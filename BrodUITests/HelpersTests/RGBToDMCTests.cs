using BrodUI.Helpers;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xunit;
using rgbDMC=BrodUI.Helpers.RgbToDmc;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace BrodUITests.HelpersTests
{
    public class RGBToDMCTests
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
            rgbDMC rGBToDMC = new();
            dmc2 = rGBToDMC.GetValDmc(red1, green1, blue1);
            Assert.Equal(dmc1, dmc2);
        }
    }
}