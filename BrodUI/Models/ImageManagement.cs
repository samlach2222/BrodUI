using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;

namespace BrodUI.Models
{
    public class ImageManagement
    {
        public BitmapImage Image { get; set; }

        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        public double Ratio { get; set; }

        public ImageManagement()
        {
            Image = null;
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
                // Get image size
                Image = new BitmapImage();
                var stream = File.OpenRead(dialog.FileName);
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

        public void LoadImageFromTemp()
        {
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, "BrodUI_CurentImage.png");
            // Use filestream to check if the file exists
            try
            {
                using (FileStream fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    // Get image size
                    Image = new BitmapImage();
                    var stream = File.OpenRead(tempFile);

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
            }
            catch (FileNotFoundException)
            {
                Image = null;
                ImageWidth = 0;
                ImageHeight = 0;
            }
        }

        public void UnloadImage()
        {
            ImageWidth = 0;
            ImageHeight = 0;
            Ratio = 1;
            Image = null;
            // if "BrodUI_CurentImage.png" exists in the temp folder, delete it
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, "BrodUI_CurentImage.png");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }

        public void ResizeImage()
        {
            BitmapImage old = Image;

            Image = new BitmapImage();
            Image.BeginInit();
            Image.StreamSource = new MemoryStream();
            PngBitmapEncoder resizedImage = new PngBitmapEncoder();
            resizedImage.Frames.Add(BitmapFrame.Create(old));
            resizedImage.Save(Image.StreamSource);
            Image.DecodePixelWidth = ImageWidth;
            Image.DecodePixelHeight = ImageHeight;
            Image.EndInit();

            // If image use a palette (indexed colors), convert it to BGRA
            if (Image.Palette != null)
            {
                FormatConvertedBitmap ImageBGRA = new FormatConvertedBitmap(Image, PixelFormats.Bgra32, null, 0);
                Image = new BitmapImage();
                Image.BeginInit();
                Image.StreamSource = new MemoryStream();
                PngBitmapEncoder encoderRGBA = new PngBitmapEncoder();
                encoderRGBA.Frames.Add(BitmapFrame.Create(ImageBGRA));
                encoderRGBA.Save(Image.StreamSource);
                Image.EndInit();
            }

            // Save image in temp folder
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, "BrodUI_CurentImage.png");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
            using (FileStream fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(Image));
                encoder.Save(fs);
            }
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_ImageWidthAndHeight + " (" + ImageWidth + "x" + ImageHeight + ") " + Assets.Languages.Resource.Terminal_ImageSaveIn + tempFile);
        }
    }
}
