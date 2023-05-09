using BrodUI.Helpers;
using Xunit;

namespace BrodUITests.HelpersTests
{
    public class DMCtoStringTests
    {
        [Fact]
        public void GetNameDmcTests()
        {
            DMCtoString d = new();
            string actual = d.GetNameDmc(517);
            string expected = "Dark Wedgwood";
            Assert.Equal(expected, actual);
        }
    }
}