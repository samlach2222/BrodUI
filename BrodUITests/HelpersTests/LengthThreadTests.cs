using Xunit;
using BrodUI.Helpers;
using System;

namespace BrodUITests.HelpersTests
{
	public class LengthThreadTests
	{
		[Fact]
		public void WireSizeTest()
		{
			int[,] imageTest = new int[,] {{3,4,3}, {3,3,5}, {5,4,3}, {4,3,3}};
			LengthThread l = new LengthThread(3, imageTest);
			var actual = l.TotalLength ;
			double expected = 27 + (7 * Math.Sqrt(2));
			Assert.Equal(actual, expected);
		}
	}
}