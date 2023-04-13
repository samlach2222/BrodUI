using System;
using System.IO;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;

namespace BrodUI.Models
{
    public class ImageManagement
    {
        public Uri? ImageLink { get; set; }

        public BitmapImage Image { get; set; }

        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        public double Ratio { get; set; }

        public ImageManagement()
        {
            ImageLink = null;
            ImageWidth = -1;
            ImageHeight = -1;
            Ratio = 1;
        }

        public void LoadImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif|All files (*.*)|*.*"
            };
            bool ok = (bool)dialog.ShowDialog()!;
            if (ok)
            {
                ImageLink = new Uri(dialog.FileName);
                // Get image size
                Image = new BitmapImage();
                Image.BeginInit();
                Image.UriSource = ImageLink;
                Image.EndInit();
                Ratio = (double)Image.PixelWidth / (double)Image.PixelHeight;
                ImageWidth = Image.PixelWidth;
                ImageHeight = Image.PixelHeight;
            }
            else
            {
                MessageBox.Show("Invalid image file");
            }
        }

        public void LoadImageFromTemp()
        {
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, "BrodUI_CurentImage.png");
            if (File.Exists(tempFile))
            {
                ImageLink = new Uri(tempFile);
                // Get image size
                Image = new BitmapImage();
                Image.BeginInit();
                Image.UriSource = ImageLink;
                Image.EndInit();
                Ratio = (double)Image.PixelWidth / (double)Image.PixelHeight;
                ImageWidth = Image.PixelWidth;
                ImageHeight = Image.PixelHeight;
            }
            else
            {
                MessageBox.Show("No image in the temporary folder");
            }
        }

        public void UnloadImage()
        {
            ImageLink = null;
            ImageWidth = 0;
            ImageHeight = 0;
            Ratio = 1;
            // if "BrodUI_CurentImage.png" exists in the temp folder, delete it
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, "BrodUI_CurentImage.png");
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }

        public void ResizeImage()
        {
            // Change width and height of the image
            Image = new BitmapImage();
            Image.BeginInit();
            Image.UriSource = ImageLink;
            Image.DecodePixelWidth = ImageWidth;
            Image.DecodePixelHeight = ImageHeight;
            Image.EndInit();

            // Save image in a temporary folder
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(Image));
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, "BrodUI_CurentImage.png");
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
            using (var fileStream = File.Create(tempFile))
            {
                encoder.Save(fileStream);
            }
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_ImageWidthAndHeight + " (" + ImageWidth + ImageHeight + ") " + Assets.Languages.Resource.Terminal_ImageSaveIn + tempFile);
        }
    }
}
