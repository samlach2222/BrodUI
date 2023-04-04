using BrodUI.Models;
using Xunit;

namespace BrodUITests
{
    public class TemplateClassTests
    {
        [Fact]
        public void NameTest()
        {
            // Arrange
            var templateClass = new TemplateClass();
            var expected = "Test";
            // Act
            templateClass.Name = expected;
            var actual = templateClass.Name;
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}