using BrodUI.Helpers;
using Xunit;

namespace BrodUITests.HelpersTests
{
    public class DmcToStringTests
    {
        [Fact]
        public void GetNameDmcTests()
        {
            DmcToString d = new();
            string actual = d.GetNameDmc(517);
            const string expected = "Dark Wedgwood";
            Assert.Equal(expected, actual);
        }
    }
}