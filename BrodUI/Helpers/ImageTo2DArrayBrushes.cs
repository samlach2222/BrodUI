using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

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

        /// <summary>
        /// Convert a 2D array of brushes to an image
        /// </summary>
        /// <param name="brushes">2D array of brushes</param>
        /// <returns>Bitmap Image to put in the ImageManagement class</returns>
        public static BitmapImage ConvertToBitmapImage(Brush[,] brushes)
        { // TODO : CREATE UNIT TESTS FOR THIS METHOD
            int width = brushes.GetLength(0);
            int height = brushes.GetLength(1);

            // Create a DrawingVisual to draw brushes in a DrawingContext
            DrawingVisual drawingVisual = new();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Rect rect = new(x, y, 1, 1);
                        drawingContext.DrawRectangle(brushes[x, y], null, rect);
                    }
                }
            }

            // Convert the DrawingVisual to a BitmapImage
            RenderTargetBitmap renderTargetBitmap = new(width, height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(drawingVisual);

            BitmapImage bitmapImage = new();
            using (MemoryStream memoryStream = new())
            {
                // Encode the image to a bitmap
                BitmapEncoder bitmapEncoder = new PngBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                bitmapEncoder.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}
