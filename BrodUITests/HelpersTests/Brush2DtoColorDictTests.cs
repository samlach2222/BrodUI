using Xunit;
using BrodUI.Helpers;
using BrodUI.Kmeans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace BrodUITests.HelpersTests
{
    public class Brush2DtoColorDictTests
    {
        [Fact]
        public void BrushToDictTest()
        {
            Brush[,] start = new Brush[2,2];
            var expected = new Dictionary<int, GenericVector>();
            start[0,0]= new SolidColorBrush(Color.FromRgb(255,0,0));
            GenericVector val1 = new();
            val1.Add(255);
            val1.Add(0);
            val1.Add(0);
            expected.Add(0,val1);
            start[1,0]= new SolidColorBrush(Color.FromRgb(0,255,0));
            GenericVector val2 = new();
            val2.Add(0);
            val2.Add(255);
            val2.Add(0);
            expected.Add(1,val2);
            start[0,1]= new SolidColorBrush(Color.FromRgb(0,0,255));
            GenericVector val3 = new();
            val3.Add(0);
            val3.Add(0);
            val3.Add(255);
            expected.Add(2,val3);
            start[1,1]= new SolidColorBrush(Color.FromRgb(0,0,0));
            GenericVector val4 = new();
            val4.Add(0);
            val4.Add(0);
            val4.Add(0);
            expected.Add(3,val4);
            var actual = Brush2DtoColorDict.BrushToDict(start);
            Assert.True(!GenericVector.NotEqual(expected[0],actual[0]));
            Assert.True(!GenericVector.NotEqual(expected[1],actual[1]));
            Assert.True(!GenericVector.NotEqual(expected[2],actual[2]));
            Assert.True(!GenericVector.NotEqual(expected[3],actual[3]));
        }

        [Fact]
        public void DictToBrushTest()
        {
            SolidColorBrush[,] expected = new SolidColorBrush[2,2];
            var data = new Dictionary<int, GenericVector>();
            var centroids = new Dictionary<int, GenericVector>();
            expected[0,0]= new SolidColorBrush(Color.FromRgb(200,0,0));
            GenericVector val1 = new();
            val1.Add(255);
            val1.Add(0);
            val1.Add(0);
            data.Add(0,val1);
            expected[1,0]= new SolidColorBrush(Color.FromRgb(0,200,0));
            GenericVector val2 = new();
            val2.Add(0);
            val2.Add(255);
            val2.Add(0);
            data.Add(1,val2);
            expected[0,1]= new SolidColorBrush(Color.FromRgb(0,0,200));
            GenericVector val3 = new();
            val3.Add(0);
            val3.Add(0);
            val3.Add(255);
            data.Add(2,val3);
            expected[1,1]= new SolidColorBrush(Color.FromRgb(0,0,200));
            GenericVector val4 = new();
            val4.Add(0);
            val4.Add(0);
            val4.Add(180);
            data.Add(3,val4);
            GenericVector cen1 = new();
            cen1.Add(200);
            cen1.Add(0);
            cen1.Add(0);
            centroids.Add(0,cen1);
            GenericVector cen2 = new();
            cen2.Add(0);
            cen2.Add(200);
            cen2.Add(0);
            centroids.Add(1,cen2);
            GenericVector cen3 = new();
            cen3.Add(0);
            cen3.Add(0);
            cen3.Add(200);
            centroids.Add(2,cen3);
            Brush[,] actual = Brush2DtoColorDict.DictToBrush2D(data,centroids,2,2);
            
            BrushConverter converter = new();
            for(var i=0; i<2; i++)
            {
                for(var j=0; j<2 ;j++)
                {
                    Brush brush = actual[i,j];
                    SolidColorBrush col = (SolidColorBrush)converter.ConvertFromString(brush.ToString())!;
                    Assert.Equal(expected[i,j].Color.R,col.Color.R);
                    Assert.Equal(expected[i,j].Color.G,col.Color.G);
                    Assert.Equal(expected[i,j].Color.B,col.Color.B);
                }
            }
            
        }
    }
}