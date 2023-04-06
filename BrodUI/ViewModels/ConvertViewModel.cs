using CommunityToolkit.Mvvm.ComponentModel;
using System;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;

namespace BrodUI.ViewModels
{
    public partial class ConvertViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        // Load Image Buttons Management PART START
        private string _isChooseImageButtonEnabled = "Visible";
        private bool _isImageLoaded = false;

        private Uri _loadedImage;

        public Uri LoadedImage
        {
            get
            {
                return _loadedImage;
            }
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
            get
            {
                return _isChooseImageButtonEnabled;
            }
            set
            {
                _isChooseImageButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsImageLoaded
        {
            get
            {
                return _isImageLoaded;
            }
            set
            {
                _isImageLoaded = value;
                OnPropertyChanged();
            }
        }
        // Load Image Buttons Management PART END



        public void OnNavigatedTo()
        {
            if (!_isInitialized)
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
            bool ok = (bool)dialog.ShowDialog();
            if (ok)
            {
                LoadedImage = new Uri(dialog.FileName);
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid image file");
            }
        }

        [RelayCommand]
        private void RemoveImage()
        {
            LoadedImage = null;
        }
    }
}
