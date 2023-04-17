using BrodUI.Models;
using BrodUI.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using MessageBox = System.Windows.MessageBox;

namespace BrodUI.ViewModels
{
    /// <summary>
    /// ViewModel for the ConvertPage
    /// </summary>
    public partial class ConvertViewModel : ObservableObject, INavigationAware
    {
        /// <summary>
        /// Ratio of the image
        /// </summary>
        private double _ratio = 1;

        /// <summary>
        /// Visibility of the button to choose an image
        /// </summary>
        private string _isChooseImageButtonEnabled = "Visible";

        /// <summary>
        /// bool to know if an image is loaded
        /// </summary>
        private bool _isImageLoaded = false;

        /// <summary>
        /// Width of the image
        /// </summary>
        private int _imageWidth = -1;

        /// <summary>
        /// Height of the image
        /// </summary>
        private int _imageHeight = -1;

        /// <summary>
        /// bool to know if the ratio is ok
        /// </summary>
        private bool _ratioNotOk = true;

        /// <summary>
        /// Loaded image
        /// </summary>
        private BitmapImage? _loadedImage = null;

        /// <summary>
        /// Image management class to manage the image
        /// </summary>
        private ImageManagement? Im { get; set; }

        /// <summary>
        /// Getter and setter for the loaded image that manage also the button visibility 
        /// </summary>
        public BitmapImage? LoadedImage
        {
            get => _loadedImage;
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

                if (Im != null)
                {
                    Im.Image = value;
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Getter and setter for the visibility of the button to choose an image
        /// </summary>
        public string IsChooseImageButtonEnabled
        {
            get => _isChooseImageButtonEnabled;
            set
            {
                _isChooseImageButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Getter and setter for the bool to know if an image is loaded
        /// </summary>
        public bool IsImageLoaded
        {
            get => _isImageLoaded;
            set
            {
                _isImageLoaded = value;
                OnPropertyChanged();
            }
        }

        // TODO : FIND A WAY TO UPDATE THIS VALUE EACH TIME THERE IS A MODIFICATION ON NUMBER DISPLAY
        /// <summary>
        /// Getter and setter for the Width of the image that also manage the ratio and the min value for width and height
        /// </summary>
        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                if (value > 10 || Im?.ImageWidth == -1)
                {
                    if (Im != null && ((int)(value / _ratio) > 10 || Im.ImageWidth == -1))
                    {
                        LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_WidthChanged + _imageWidth + Assets.Languages.Resource.Terminal_To + value);
                        _imageWidth = value;
                        // Set Height according to the ratio
                        if (_ratioNotOk)
                        {
                            _ratioNotOk = false;
                            ImageHeight = (int)(value / _ratio);
                            _ratioNotOk = true;
                        }
                        Im.ImageWidth = value;
                        OnPropertyChanged();
                    }
                    else
                    {
                        MessageBox.Show("Width will be " + (int)(value / _ratio) + ", but Width must be greater than 10");
                    }
                }
                else
                {
                    MessageBox.Show("Width must be greater than 10");

                }
            }
        }

        /// <summary>
        /// Getter and setter for the Height of the image that also manage the ratio and the min value for width and height
        /// </summary>
        public int ImageHeight
        {
            get => _imageHeight;
            set
            {
                if (Im != null && (value > 10 || Im.ImageHeight == -1))
                {
                    if ((int)(value * _ratio) > 10 || Im.ImageHeight == -1)
                    {
                        LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_HeightChanged + _imageWidth + Assets.Languages.Resource.Terminal_To + value);
                        _imageHeight = value;
                        // Set Width according to the ratio
                        if (_ratioNotOk)
                        {
                            _ratioNotOk = false;
                            ImageWidth = (int)(value * _ratio);
                            _ratioNotOk = true;
                        }
                        Im.ImageHeight = value;
                        OnPropertyChanged();
                    }
                    else
                    {
                        MessageBox.Show("Width will be " + (int)(value * _ratio) + ", but Width must be greater than 10");
                    }

                }
                else
                {
                    MessageBox.Show("Height must be greater than 10");
                }
            }
        }

        /// <summary>
        /// Function called when user navigate to this page. It initialize the view model and display a message in the terminal
        /// </summary>
        public void OnNavigatedTo()
        {
            LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_ConvertPage);
            InitializeViewModel();
        }

        /// <summary>
        /// Initialize the view model and load the image if there is one in the temp folder
        /// </summary>
        private void InitializeViewModel()
        {
            // if not already initialized
            Im ??= new ImageManagement();

            Im.LoadImageFromTemp();
            if (Im.Image == null) return;
            LoadedImage = Im.Image;
            _ratio = Im.Ratio;
            ImageWidth = Im.ImageWidth;
            ImageHeight = Im.ImageHeight;
        }

        /// <summary>
        /// Function called when user click on the button to load an image from the file system
        /// </summary>
        [RelayCommand]
        private void LoadImage()
        {
            // Open file dialog
            Im?.LoadImage();
            LoadedImage = Im.Image;
            _ratio = Im.Ratio;
            ImageWidth = Im.ImageWidth;
            ImageHeight = Im.ImageHeight;
        }

        /// <summary>
        /// Function called when user click on the button to remove the image
        /// </summary>
        [RelayCommand]
        private void RemoveImage()
        {
            LoadedImage = null;
            _imageWidth = 0;
            _imageHeight = 0;
            Im?.UnloadImage();

        }

        /// <summary>
        /// Function called when user click on the button to convert the image. It also navigate to the Export page
        /// </summary>
        [RelayCommand]
        private void ConvertImage()
        {
            Im.ResizeImage();
            var navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
            if (navigationService != null)
            {
                _ = navigationService.Navigate(typeof(ExportPage)); // Navigate to the Convert page.
            }
            LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_ImageConvertedOk);
        }

        /// <summary>
        /// Function called when user click on the button to go back to the main page
        /// </summary>
        /// <exception cref="NotImplementedException">This method is not implemented because useless for the moment</exception>
        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }
    }
}