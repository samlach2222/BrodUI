using Xunit;
using BrodUI.Kmeans;
using System.Collections.Generic;
using System;

namespace BrodUITests.KmeansTests
{
    public class ExtensionsTests
    {
        [Fact]
        public void TimesSimplesTest()
        {
            int val = 0;
            int nb = 5;
            nb.Times(()=>val++);
            Assert.Equal(nb,val);
        }

        [Fact]
        public void TimesListTest()
        {
            int val = 0;
            List<int> expected = new();
            expected.Add(0);
            expected.Add(1);
            expected.Add(2);
            List<int> actual = new();
            int nb = 3;
            nb.Times(()=>actual.Add(val++));
            Assert.Equal(expected,actual);
        }
    }
}