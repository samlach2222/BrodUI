using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;

namespace BrodUI.ViewModels
{
    public partial class ConvertViewModel : ObservableObject, INavigationAware
    {
        private double ratio = 1;

        // Load Image Buttons Management PART
        private string _isChooseImageButtonEnabled = "Visible";
        private bool _isImageLoaded = false;
        private int _imageWidth = 0;
        private int _imageHeight = 0;
        private Uri? _loadedImage = null;
        
        public Uri? LoadedImage
        {
            get { return _loadedImage; }
            set
            {
                _loadedImage = value;
                // Disable the button
                if (_loadedImage != null)
                {
                    IsChooseImageButtonEnabled = "Hidden";
                    IsImageLoaded = true;
                }
                else
                {
                    IsChooseImageButtonEnabled = "Visible";
                    IsImageLoaded = false;
                }
                OnPropertyChanged();
            }
        }
        public string IsChooseImageButtonEnabled
        {
            get { return _isChooseImageButtonEnabled; }
            set
            {
                _isChooseImageButtonEnabled = value;
                OnPropertyChanged();
            }
        }
        public bool IsImageLoaded
        {
            get { return _isImageLoaded; }
            set
            {
                _isImageLoaded = value;
                OnPropertyChanged();
            }
        }

        private bool ratioNotOK = true;

        public int ImageWidth
        {
            get { return _imageWidth; }
            set
            {
                _imageWidth = value;
                // Set Height according to the ratio
                if (ratioNotOK && value != 0)
                {
                    ratioNotOK = false;
                    ImageHeight = (int)(value / ratio);
                    ratioNotOK = true;
                }
                MessageBox.Show("ImageWidth : " + value);
                OnPropertyChanged();
            }
        }

        public int ImageHeight
        {
            get { return _imageHeight; }
            set
            {
                _imageHeight = value;
                // Set Width according to the ratio
                if (ratioNotOK && value != 0)
                {
                    ratioNotOK = false;
                    ImageWidth = (int)(value * ratio);
                    ratioNotOK = true;
                }
                MessageBox.Show("ImageHeight : " + value);
                OnPropertyChanged();
            }
        }
        // Load Image Buttons Management PART END



        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {

        }

        private void InitializeViewModel()
        {
            
        }

        [RelayCommand]
        private void LoadImage()
        {
            // Open file dialog
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif|All files (*.*)|*.*"
            };
            bool ok = (bool)dialog.ShowDialog()!;
            if (ok)
            {
                LoadedImage = new Uri(dialog.FileName);
                // Get image size
                var image = new System.Windows.Media.Imaging.BitmapImage();
                image.BeginInit();
                image.UriSource = LoadedImage;
                image.EndInit();
                ratio = (double)image.PixelWidth / (double)image.PixelHeight; // TODO : Create an Image management class
                ImageWidth = image.PixelWidth;
                ImageHeight = image.PixelHeight;
            }
            else
            {
                MessageBox.Show("Invalid image file");
            }
        }

        [RelayCommand]
        private void RemoveImage()
        {
            LoadedImage = null;
            // Reset image size
            ImageWidth = 0;
            ImageHeight = 0;
        }
    }
}
