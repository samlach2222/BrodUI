using System.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BrodUI.Models;

namespace BrodUI.Helpers
{
    internal class ImageToDataTable
    {
        // Create 2D Array of Brush
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
                    byte a = pixels[index + 3];
                    array[x, y] = new SolidColorBrush(Color.FromArgb(r, g, b, a));
                }
            }

            return array;
        }

        public static DataTable Convert2dArrayToDataTable(Brush[,] table)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < table.GetLength(0); i++)
            {
                dt.Columns.Add();
            }

            for (int i = 0; i < table.GetLength(1); i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < table.GetLength(0); j++)
                {
                    dr[j] = table[j, i];
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
