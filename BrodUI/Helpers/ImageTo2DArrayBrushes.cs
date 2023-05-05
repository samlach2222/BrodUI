using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BrodUI.Helpers
{
    /// <summary>
    /// Class to convert an image to a 2D array of brushes
    /// </summary>
    public class ImageTo2DArrayBrushes
    {
        /// <summary>
        /// Convert an image to a 2D array of brushes
        /// </summary>
        /// <param name="bitmapSource">Source image</param>
        /// <returns>2D array of Brushes where the size is the same as the image and also colors</returns>
        public static Brush[,] ConvertTo2dArray(BitmapSource bitmapSource)
        {
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
            byte[] pixels = new byte[height * stride];
            bitmapSource.CopyPixels(pixels, stride, 0);

            Brush[,] array = new Brush[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * stride + x * 4;
                    byte b = pixels[index];
                    byte g = pixels[index + 1];
                    byte r = pixels[index + 2];
                    array[x, y] = new SolidColorBrush(Color.FromRgb(r, g, b));
                }
            }
            return array;
        }
    }
}
