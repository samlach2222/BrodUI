using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BrodUI.Helpers;
using Xunit;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace BrodUITests.HelpersTests
{
    public class ImageTo2DArrayBrushesTests
    {
        [Fact]
        public void ConvertTo2dArrayTest()
        {
            // Expected
            Brush[,] expected = new Brush[2, 2];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    expected[i, j] = new SolidColorBrush(Color.FromRgb((byte)(i * 10), (byte)(i * 10), (byte)(i * 10)));
                }

            }

            // Actual
            Bitmap bitmap = new(2, 2);
            bitmap.SetPixel(0, 0, System.Drawing.Color.FromArgb(0, 0, 0));
            bitmap.SetPixel(0, 1, System.Drawing.Color.FromArgb(0, 0, 0));
            bitmap.SetPixel(1, 0, System.Drawing.Color.FromArgb(10, 10, 10));
            bitmap.SetPixel(1, 1, System.Drawing.Color.FromArgb(10, 10, 10));

            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            Brush[,] actual = ImageTo2DArrayBrushes.ConvertTo2dArray(bitmapSource);

            // Assert
            Assert.Equal(expected.ToString(), actual.ToString());
        }
    }
}