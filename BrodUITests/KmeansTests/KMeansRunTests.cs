using BrodUI.KMeans;
using System.Windows.Media;
using Xunit;

namespace BrodUITests.KMeansTests
{
    public class KMeansRunTests
    {
        [Fact]
        public void CompleteKMeansRunTest()
        {
            SolidColorBrush[,] expected = new SolidColorBrush[2, 2];
            Brush[,] image = new Brush[2, 2];
            expected[0, 0] = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            expected[1, 0] = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            expected[0, 1] = new SolidColorBrush(Color.FromRgb(0, 0, 218));
            expected[1, 1] = new SolidColorBrush(Color.FromRgb(0, 0, 218));
            image[0, 0] = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            image[1, 0] = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            image[0, 1] = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            image[1, 1] = new SolidColorBrush(Color.FromRgb(0, 0, 180));
            Brush[,] actual = KMeansRun.StartKMeans(image, 3, 30); //We ask a new coloration of the picture with 3 colors using 30 kmeans to calculate the best results
            BrushConverter converter = new();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Brush brush = actual[i, j];
                    SolidColorBrush col = (SolidColorBrush)converter.ConvertFromString(brush.ToString())!;
                    Assert.Equal(expected[i, j].Color.R, col.Color.R);
                    Assert.Equal(expected[i, j].Color.G, col.Color.G);
                    Assert.Equal(expected[i, j].Color.B, col.Color.B);
                }
            }
        }
    }
}