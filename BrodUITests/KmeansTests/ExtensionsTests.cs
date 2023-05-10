using BrodUI.KMeans;
using Xunit;

namespace BrodUITests.KMeansTests
{
    public class ExtensionsTests
    {
        [Fact]
        public void TimesSimpleTest()
        {
            int val = 0;
            const int nb = 5;
            nb.Times(() => val++);
            Assert.Equal(nb, val);
        }

        [Fact]
        public void TimesListTest()
        {
            int val = 0;
            List<int> expected = new()
            {
                0,
                1,
                2
            };
            List<int> actual = new();
            const int nb = 3;
            nb.Times(() => actual.Add(val++));
            Assert.Equal(expected, actual);
        }
    }
}