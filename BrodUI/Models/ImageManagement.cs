﻿using BrodUI.Helpers;
using BrodUI.KMeans;
using BrodUI.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.TaskBar;

namespace BrodUI.Models
{
    /// <summary>
    /// Class to manage loaded image and its properties
    /// </summary>
    public class ImageManagement
    {
        /// <summary>
        /// Dialog to open a file
        /// </summary>
        private readonly IOpenFileDialog _openFileDialog;

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
        /// Path to the current image
        /// </summary>
        public static string ImagePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BrodUI", "current image.png");

        /// <summary>
        /// Path to the current K-Meansed image
        /// </summary>
        public static string ImagePathKMeansed => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BrodUI", "current image K-Meansed.png");

        /// <summary>
        /// Constructor that set the default values
        /// </summary>
        /// <param name="openFileDialog">fileDialog of the image loading popup (windows)</param>
        public ImageManagement(IOpenFileDialog openFileDialog)
        {
            Image = null;
            ImageWidth = -1;
            ImageHeight = -1;
            Ratio = 1;
            _openFileDialog = openFileDialog;
        }

        /// <summary>
        /// Interface for OpenFileDialog to be able to mock it
        /// </summary>
        public interface IOpenFileDialog
        {
            /// <summary>
            /// Name of the file
            /// </summary>
            string? FileName { get; set; }

            /// <summary>
            /// Function to show the dialog
            /// </summary>
            /// <returns>return if it's Ok or not</returns>
            bool? ShowDialog();

            /// <summary>
            /// Name of the files
            /// </summary>
            string[]? FileNames { get; }

            /// <summary>
            /// Filters for the dialog (image filter)
            /// </summary>
            string? Filter { get; set; }
        }


        /// <summary>
        /// Load an image from a file with a dialog and set Image, ImageWidth, ImageHeight and Ratio
        /// </summary>
        public void LoadImageDialog()
        {
            _openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            bool? dialogResult = _openFileDialog.ShowDialog();

            if (dialogResult == true)
            {
                // Get image size
                Image = new BitmapImage();
                FileStream stream = File.OpenRead(_openFileDialog.FileName!);
                Image.BeginInit();
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.StreamSource = stream;
                Image.EndInit();
                stream.Close();
                stream.Dispose();

                // We only support images in Bgr32 or Bgra32 format (see Helpers/ImageTo2DArrayBrushes)
                // Convert to Bgra32 if needed
                if (Image.Format != PixelFormats.Bgr32 && Image.Format != PixelFormats.Bgra32)
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

                // We don't need to check if the file exists, it will be overwritten if needed
                using (FileStream fs = new(ImagePath, FileMode.Create, FileAccess.Write))
                {
                    PngBitmapEncoder encoder = new();
                    encoder.Frames.Add(BitmapFrame.Create(Image));
                    encoder.Save(fs);
                }

                Ratio = (double)Image.PixelWidth / Image.PixelHeight;
                ImageWidth = Image.PixelWidth;
                ImageHeight = Image.PixelHeight;
            }
            else
            {
                WpfMessageBox.Show("", Assets.Languages.Resource.ImageManagement_InvalidImageFile);
            }
        }

        /// <summary>
        /// Load the current image and set Image, ImageWidth, ImageHeight and Ratio
        /// </summary>
        public void LoadCurrentImage()
        {
            // Use FileStream to check if the file exists
            try
            {
                using FileStream stream = File.OpenRead(ImagePath);
                // Get image size
                Image = new BitmapImage();

                Image.BeginInit();
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.StreamSource = stream;

                Image.EndInit();
                Image.Freeze();
                stream.Close();
                stream.Dispose();

                // We only support images in Bgr32 or Bgra32 format (see Helpers/ImageTo2DArrayBrushes)
                // Convert to Bgra32 if needed
                if (Image.Format != PixelFormats.Bgr32 && Image.Format != PixelFormats.Bgra32)
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

                Ratio = (double)Image.PixelWidth / Image.PixelHeight;
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
        /// Load the current image and set Image, ImageWidth, ImageHeight and Ratio
        /// </summary>
        public void LoadCurrentImageKMeansed()
        {
            // Use FileStream to check if the file exists
            try
            {
                using FileStream stream = File.OpenRead(ImagePathKMeansed);
                // Get image size
                Image = new BitmapImage();

                Image.BeginInit();
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.StreamSource = stream;

                Image.EndInit();
                Image.Freeze();
                stream.Close();
                stream.Dispose();

                // We only support images in Bgr32 or Bgra32 format (see Helpers/ImageTo2DArrayBrushes)
                // Convert to Bgra32 if needed
                if (Image.Format != PixelFormats.Bgr32 && Image.Format != PixelFormats.Bgra32)
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

                Ratio = (double)Image.PixelWidth / Image.PixelHeight;
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
        /// Unload the image and delete the file
        /// </summary>
        public void UnloadImage()
        {
            ImageWidth = 0;
            ImageHeight = 0;
            Ratio = 1;
            Image = null;
            // if a current image exists, delete it
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (File.Exists(ImagePath))
            {
                File.Delete(ImagePath);
            }
        }

        /// <summary>
        /// Resize the image to ImageWidth and ImageHeight and save it
        /// </summary>
        /// <param name="kMeansColorNumber">number of color of the result image</param>
        /// <param name="kmeansIterationNumber">number of KMeans iterations</param>
        /// <param name="bw">Background worker to report percentage of completion</param>
        public void ResizeImage(int kMeansColorNumber, int kmeansIterationNumber, BackgroundWorker? bw = null)
        {
            // Set taskBar progress to indeterminate (we can't know the progress of the resize)
            LogManagement.UpdateProgression(TaskBarProgressState.Indeterminate);

            BitmapImage? old;
            lock (this)
            {
                old = Image;
                Image = new BitmapImage();
            }

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

            Brush[,] kMeansArray;
            lock (this)
            {
                kMeansArray = ImageTo2DArrayBrushes.ConvertTo2dArray(Image);
            }
            kMeansArray = KMeansRun.StartKMeans(kMeansArray, kMeansColorNumber, kmeansIterationNumber, bw);
            lock (this)
            {
                Image = ImageTo2DArrayBrushes.ConvertToBitmapImage(kMeansArray);
            }

            // Save resized image
            GC.Collect();
            GC.WaitForPendingFinalizers();
            // We don't need to check if the file exists, it will be overwritten if needed
            using (FileStream fs = new(ImagePathKMeansed, FileMode.Create, FileAccess.Write))
            {
                PngBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(Image));
                encoder.Save(fs);
            }
            Console.WriteLine("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_ImageWidthAndHeight + " (" + ImageWidth + "x" + ImageHeight + ") " + Assets.Languages.Resource.Terminal_ImageSaveIn + ImagePath);
        }
    }
}
