using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows;
using BrodUI.Models;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using BrodUI.Views.Pages;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;
using MessageBox = System.Windows.MessageBox;

namespace BrodUI.ViewModels
{
    public partial class ConvertViewModel : ObservableObject, INavigationAware
    {
        private double ratio = 1;

        // Load Image Buttons Management PART
        private string _isChooseImageButtonEnabled = "Visible";
        private bool _isImageLoaded = false;
        private int _imageWidth = -1;
        private int _imageHeight = -1;
        private bool _ratioNotOk = true;
        private Uri? _loadedImage = null;

        private ImageManagement Im { get; set; }
        
        public Uri? LoadedImage
        {
            get { return _loadedImage; }
            set
            {
                LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_ImageLinkChanged + _loadedImage + Assets.Languages.Resource.Terminal_To + value);
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

                Im.ImageLink = value;
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

        // TODO : FIND A WAY TO UPDATE THIS VALUE EACH TIME THERE IS A MODIFICATION ON NUMBER DISPLAY
        public int ImageWidth
        {
            get { return _imageWidth; }
            set
            {
                if (value > 10 || Im.ImageWidth == -1)
                {
                    if ((int)(value / ratio) > 10 || Im.ImageWidth == -1)
                    {
                        LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_WidthChanged + _imageWidth + Assets.Languages.Resource.Terminal_To + value);
                        _imageWidth = value;
                        // Set Height according to the ratio
                        if (_ratioNotOk)
                        {
                            _ratioNotOk = false;
                            ImageHeight = (int)(value / ratio);
                            _ratioNotOk = true;
                        }
                        Im.ImageWidth = value;
                        OnPropertyChanged();
                    }
                    else
                    {
                       MessageBox.Show("Width will be " + (int)(value / ratio) + ", but Width must be greater than 10");
                    }
                }
                else
                {
                    MessageBox.Show("Width must be greater than 10");
                    
                }
            }
        }

        public int ImageHeight
        {
            get { return _imageHeight; }
            set
            {
                if (value > 10 || Im.ImageHeight == -1)
                {
                    if ((int)(value * ratio) > 10 || Im.ImageHeight == -1)
                    {
                        LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_HeightChanged + _imageWidth + Assets.Languages.Resource.Terminal_To + value);
                        _imageHeight = value;
                        // Set Width according to the ratio
                        if (_ratioNotOk)
                        {
                            _ratioNotOk = false;
                            ImageWidth = (int)(value * ratio);
                            _ratioNotOk = true;
                        }
                        Im.ImageHeight = value;
                        OnPropertyChanged();
                    }
                    else
                    {
                        MessageBox.Show("Width will be " + (int)(value * ratio) + ", but Width must be greater than 10");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Height must be greater than 10");
                }
            }
        }
        // Load Image Buttons Management PART END



        public void OnNavigatedTo()
        {
            LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_ConvertPage);
            InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {

        }

        private void InitializeViewModel()
        {
            // if not already initialized
            if (Im == null)
            {
                Im = new ImageManagement();
            }
        }

        [RelayCommand]
        private void LoadImage()
        {
            // Open file dialog
            Im.LoadImage();
            LoadedImage = Im.ImageLink;
            ratio = Im.Ratio;
            ImageWidth = Im.ImageWidth;
            ImageHeight = Im.ImageHeight;
        }

        [RelayCommand]
        private void RemoveImage()
        {
            LoadedImage = null;
            Im.UnloadImage();
        }

        [RelayCommand]
        private void ConvertImage()
        {
            Im.ResizeImage();
            var navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
            if (navigationService != null)
            {
                _ = navigationService.Navigate(typeof(ExportPage)); // Navigate to the Convert page.
            }
            LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_ImageConvertedOk);
        }
    }
}
