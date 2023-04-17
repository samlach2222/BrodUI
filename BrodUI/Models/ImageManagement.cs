using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;

namespace BrodUI.Models
{
    /// <summary>
    /// Class to manage loaded image and its properties
    /// </summary>
    public class ImageManagement
    {
        /// <summary>
        /// Image used in the app
        /// </summary>
        public BitmapImage? Image { get; set; }

        /// <summary>
        /// Width of the image
        /// </summary>
        public int ImageWidth { get; set; }

        /// <summary>
        /// Height of the image
        /// </summary>
        public int ImageHeight { get; set; }

        /// <summary>
        /// Ratio of the image (width / height)
        /// </summary>
        public double Ratio { get; set; }

        /// <summary>
        /// Constructor that set the default values
        /// </summary>
        public ImageManagement()
        {
            Image = null;
            ImageWidth = -1;
            ImageHeight = -1;
            Ratio = 1;
        }

        /// <summary>
        /// Load an image from a file with a dialog and set Image, ImageWidth, ImageHeight and Ratio
        /// </summary>
        public void LoadImage()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif|All files (*.*)|*.*"
            };
            bool ok = (bool)dialog.ShowDialog()!;
            if (ok)
            {
                // Get image size
                Image = new BitmapImage();
                FileStream stream = File.OpenRead(dialog.FileName);
                Image.BeginInit();
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.StreamSource = stream;
                Image.EndInit();
                stream.Close();
                stream.Dispose();
                Ratio = (double)Image.PixelWidth / (double)Image.PixelHeight;
                ImageWidth = Image.PixelWidth;
                ImageHeight = Image.PixelHeight;
            }
            else
            {
                MessageBox.Show("Invalid image file");
            }
        }

        /// <summary>
        /// Load an image from a file in temp folder of the system and set Image, ImageWidth, ImageHeight and Ratio
        /// </summary>
        public void LoadImageFromTemp()
        {
            string tempPath = Path.GetTempPath();
            string tempFile = Path.Combine(tempPath, "BrodUI_CurentImage.png");
            // Use FileStream to check if the file exists
            try
            {
                using FileStream fs = new (tempFile, FileMode.Open, FileAccess.Read);
                // Get image size
                Image = new BitmapImage();
                FileStream stream = File.OpenRead(tempFile);

                Image.BeginInit();
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.StreamSource = stream;

                Image.EndInit();
                Image.Freeze();
                stream.Close();
                stream.Dispose();

                Ratio = (double)Image.PixelWidth / (double)Image.PixelHeight;
                ImageWidth = Image.PixelWidth;
                ImageHeight = Image.PixelHeight;
            }
            catch (FileNotFoundException)
            {
                Image = null;
                ImageWidth = 0;
                ImageHeight = 0;
            }
        }

        /// <summary>
        /// Unload the image and delete the file in temp folder
        /// </summary>
        public void UnloadImage()
        {
            ImageWidth = 0;
            ImageHeight = 0;
            Ratio = 1;
            Image = null;
            // if "BrodUI_CurentImage.png" exists in the temp folder, delete it
            string tempPath = Path.GetTempPath();
            string tempFile = Path.Combine(tempPath, "BrodUI_CurentImage.png");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }

        /// <summary>
        /// Resize the image to ImageWidth and ImageHeight and save it in temp folder
        /// </summary>
        public void ResizeImage()
        {
            BitmapImage? old = Image;

            Image = new BitmapImage();
            Image.BeginInit();
            Image.StreamSource = new MemoryStream();
            PngBitmapEncoder resizedImage = new();
            if (old != null)
            {
                resizedImage.Frames.Add(BitmapFrame.Create(old));
            }
            resizedImage.Save(Image.StreamSource);
            Image.DecodePixelWidth = ImageWidth;
            Image.DecodePixelHeight = ImageHeight;
            Image.EndInit();

            // If image use a palette (indexed colors), convert it to BGRA
            if (Image.Palette != null)
            {
                FormatConvertedBitmap imageBgra = new(Image, PixelFormats.Bgra32, null, 0);
                Image = new BitmapImage();
                Image.BeginInit();
                Image.StreamSource = new MemoryStream();
                PngBitmapEncoder encoderRgba = new();
                encoderRgba.Frames.Add(BitmapFrame.Create(imageBgra));
                encoderRgba.Save(Image.StreamSource);
                Image.EndInit();
            }

            // Save image in temp folder
            string tempPath = Path.GetTempPath();
            string tempFile = Path.Combine(tempPath, "BrodUI_CurentImage.png");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
            using (FileStream fs = new(tempFile, FileMode.Create, FileAccess.Write))
            {
                PngBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(Image));
                encoder.Save(fs);
            }
            Console.WriteLine("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_ImageWidthAndHeight + " (" + ImageWidth + "x" + ImageHeight + ") " + Assets.Languages.Resource.Terminal_ImageSaveIn + tempFile);
        }
    }
}
