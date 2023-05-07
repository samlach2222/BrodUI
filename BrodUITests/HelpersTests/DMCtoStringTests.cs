using Xunit;
using BrodUI.Helpers;
using System;

namespace BrodUITests.HelpersTests
{ 
    public class DMCtoStringTests
	{
		[Fact]
		public void GetNameDmcTests()
		{
			DMCtoString d = new DMCtoString();
			var actual = d.GetNameDmc(517);
			string expected = "Dark Wedgwood";
			Assert.Equal(actual, expected);
		}
	}
}