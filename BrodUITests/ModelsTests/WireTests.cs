using BrodUI.Models;
using System.Windows.Media;
using Xunit;

namespace BrodUITests.ModelsTests
{
    public class WireTests
    {
        [Fact]
        public void WireTest()
        {
            Wire wire = new(new SolidColorBrush(Color.FromRgb(255, 0, 0)), 255, "DMC", "RED", 20);
            Assert.Equal(new SolidColorBrush(Color.FromRgb(255, 0, 0)).ToString(), wire.Color.ToString());
            Assert.Equal(255, wire.Number);
            Assert.Equal("DMC", wire.Type);
            Assert.Equal("RED", wire.Name);
            Assert.Equal(20, wire.Length);
        }

        [Fact]
        public void ColorTest()
        {
            Wire wire = new(new SolidColorBrush(Color.FromRgb(255, 0, 0)), 255, "DMC", "RED", 20);
            Assert.Equal(new SolidColorBrush(Color.FromRgb(255, 0, 0)).ToString(), wire.Color.ToString());
            wire.Color = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            Assert.Equal(new SolidColorBrush(Color.FromRgb(0, 255, 0)).ToString(), wire.Color.ToString());
        }

        [Fact]
        public void NumberTest()
        {
            Wire wire = new(new SolidColorBrush(Color.FromRgb(255, 0, 0)), 255, "DMC", "RED", 20);
            Assert.Equal(255, wire.Number);
            wire.Number = 0;
            Assert.Equal(0, wire.Number);
        }

        [Fact]
        public void TypeTest()
        {
            Wire wire = new(new SolidColorBrush(Color.FromRgb(255, 0, 0)), 255, "DMC", "RED", 20);
            Assert.Equal("DMC", wire.Type);
            wire.Type = "Anchor";
            Assert.Equal("Anchor", wire.Type);
        }

        [Fact]
        public void NameTest()
        {
            Wire wire = new(new SolidColorBrush(Color.FromRgb(255, 0, 0)), 255, "DMC", "RED", 20);
            Assert.Equal("RED", wire.Name);
            wire.Name = "GREEN";
            Assert.Equal("GREEN", wire.Name);
        }

        [Fact]
        public void QuantityTest()
        {
            Wire wire = new(new SolidColorBrush(Color.FromRgb(255, 0, 0)), 255, "DMC", "RED", 20);
            Assert.Equal(20, wire.Length);
            wire.Length = 0;
            Assert.Equal(0, wire.Length);
        }
    }
}

