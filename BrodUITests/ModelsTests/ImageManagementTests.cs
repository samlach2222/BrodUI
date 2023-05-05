using System.Drawing;
using System.Globalization;
using System.Windows.Media.Imaging;
using BrodUI.Helpers;
using BrodUI.Models;
using Xunit;
using Moq;
using static BrodUI.Models.ImageManagement;

namespace BrodUITests.ModelsTests;

public class ImageManagementTests
{
    [Fact]
    public void ImageTest()
    {
        // ---------
        // Get tests
        // ---------

        ImageManagement im = new(new Win32OpenFileDialogAdapter());
        Assert.Null(im.Image);

        // ---------
        // Set tests
        // ---------

        BitmapImage bi = new();
        bi.BeginInit();
        // get the application icon in Assets folder
        string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\BrodUI\Assets\applicationIcon-1024.png");
        bi.UriSource = new Uri(path);
        bi.EndInit();


        // Set image
        im.Image = bi;
        Assert.NotNull(im.Image);

        // ---------
        // Get tests
        // ---------

        BitmapImage bi2 = new();
        bi2.BeginInit();
        string path2 = Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\BrodUI\Assets\applicationIcon-1024.png");
        bi2.UriSource = new Uri(path2);
        bi2.EndInit();

        Assert.Equal(bi2.ToString(), bi.ToString());
    }

    [Fact]
    public void ImageWidthTest()
    {
        // ---------
        // Get tests
        // ---------

        ImageManagement im = new(new Win32OpenFileDialogAdapter());
        Assert.Equal(-1, im.ImageWidth);

        // ---------
        // Set tests
        // ---------

        im.ImageWidth = 100;
        Assert.Equal(100, im.ImageWidth);
    }

    [Fact]
    public void ImageHeightTest()
    {
        // ---------
        // Get tests
        // ---------

        ImageManagement im = new(new Win32OpenFileDialogAdapter());
        Assert.Equal(-1, im.ImageHeight);

        // ---------
        // Set tests
        // ---------

        im.ImageHeight = 100;
        Assert.Equal(100, im.ImageHeight);
    }

    [Fact]
    public void RatioTest()
    {
        // ---------
        // Get tests
        // ---------

        ImageManagement im = new(new Win32OpenFileDialogAdapter());
        Assert.Equal(1, im.Ratio);

        // ---------
        // Set tests
        // ---------

        im.Ratio = 2;
        Assert.Equal(2, im.Ratio);
    }

    [Fact]
    public void ImageManagementTest()
    {
        ImageManagement im = new(new Win32OpenFileDialogAdapter());
        Assert.Null(im.Image);
        Assert.Equal(-1, im.ImageWidth);
        Assert.Equal(-1, im.ImageHeight);
        Assert.Equal(1, im.Ratio);
    }

    [Fact]
    public void LoadImageDialogTest()
    {
        // ----------------------
        // Load Image Result true
        // ----------------------

        Mock<IOpenFileDialog> openFileDialogMock = new();
        ImageManagement im = new(openFileDialogMock.Object);

        string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\BrodUI\Assets\applicationIcon-1024.png");
        openFileDialogMock.Setup(x => x.ShowDialog()).Returns(true);
        openFileDialogMock.Setup(x => x.FileName).Returns(path);

        im.LoadImageDialog();

        Assert.NotNull(im.Image);
        Assert.Equal(1024, im.ImageWidth);
        Assert.Equal(1024, im.ImageHeight);
        Assert.Equal(1, im.Ratio);

        // -----------------------
        // Load Image Result false
        // -----------------------

        Mock<IOpenFileDialog> openFileDialogMock2 = new();
        ImageManagement im2 = new(openFileDialogMock2.Object)
        {
            Image = null
        };

        openFileDialogMock2.Setup(y => y.ShowDialog()).Returns(false);

        im2.LoadImageDialog();

        Assert.Null(im2.Image);
        Assert.Equal(-1, im2.ImageWidth);
        Assert.Equal(-1, im2.ImageHeight);
        Assert.Equal(1, im2.Ratio);
    }
}