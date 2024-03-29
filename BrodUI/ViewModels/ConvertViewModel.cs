using BrodUI.Helpers;
using BrodUI.Models;
using BrodUI.Services;
using BrodUI.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BrodUI.ViewModels
{
    /// <summary>
    /// ViewModel for the ConvertPage
    /// </summary>
    public partial class ConvertViewModel : ObservableObject, INavigationAware
    {
        /// <summary>
        /// List of color models displayed in the UI.
        /// </summary>
        [ObservableProperty]
        private string[] _colorModels = ConfigManagement.AllColorModels;

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
        private bool _isImageLoaded;

        /// <summary>
        /// Width of the image
        /// </summary>
        private int _imageWidth = -1;

        /// <summary>
        /// Height of the image
        /// </summary>
        private int _imageHeight = -1;

        /// <summary>
        /// Number of color for the kmeans algorithm
        /// </summary>
        private int _kmeansColorNumber;

        /// <summary>
        /// Number of iteration for the kmeans algorithm
        /// </summary>
        private int _kmeansIterationNumber;

        /// <summary>
        /// bool to know if the ratio is ok
        /// </summary>
        private bool _ratioNotOk = true;

        /// <summary>
        /// Current language of the application
        /// </summary>
        private string? _curColorModel;

        /// <summary>
        /// Loaded image
        /// </summary>
        private BitmapImage? _loadedImage;

        /// <summary>
        /// Percentage of the progress bar
        /// </summary>
        private int _progressConvert;

        /// <summary>
        /// Percentage show next to the progress bar
        /// </summary>
        private string _progressTb = "0%";

        /// <summary>
        /// Visibility of the progress grid
        /// </summary>
        private string _progressVisibility = "Hidden";

        /// <summary>
        /// Visibility of the global grid
        /// </summary>
        private bool _globalGridVisibility = true;

        /// <summary>
        /// Number of different colors in the loaded image
        /// </summary>
        private int _loadedImageNumberColors = int.MaxValue;

        /// <summary>
        /// Image management class to manage the image
        /// </summary>
        private ImageManagement? Im { get; set; }

        /// <summary>
        /// Minimum value for width and height
        /// </summary>
        private const int MinimumWidthHeight = 10;

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
        /// Getter and Setter for the value of the progress bar
        /// </summary>
        public int ProgressConvert
        {
            get => _progressConvert;
            set
            {
                _progressConvert = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Getter and Setter for the value of the progress text block
        /// </summary>
        public string ProgressTb
        {
            get => _progressTb;
            set
            {
                _progressTb = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Getter and Setter for the visibility of the progress grid
        /// </summary>
        public string ProgressVisibility
        {
            get => _progressVisibility;
            set
            {
                _progressVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Getter and Setter for the visibility of the global grid
        /// </summary>
        public bool GlobalGridVisibility
        {
            get => _globalGridVisibility;
            set
            {
                _globalGridVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Getter and setter for the current color model
        /// </summary>
        public string? CurColorModel
        {
            get => _curColorModel;
            set
            {
                if (value == null)
                {
                    return;
                }
                ConfigManagement.SetColorModelToConfigFile(value);
                SetProperty(ref _curColorModel, value);
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

        /// <summary>
        /// Getter and setter for the Width of the image that also manage the ratio and the min value for width and height
        /// </summary>
        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                if (value > MinimumWidthHeight || Im?.ImageWidth == -1)
                {
                    if (Im != null && ((int)(value / _ratio) > MinimumWidthHeight || Im.ImageWidth == -1))
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
                        string badValue = ((int)(value / _ratio)).ToString();
                        WpfMessageBox.Show("", string.Format(Assets.Languages.Resource.Convert_MinimumWidth_WithValue, badValue) + MinimumWidthHeight);
                    }
                }
                else
                {
                    WpfMessageBox.Show("", Assets.Languages.Resource.Convert_MinimumWidth + MinimumWidthHeight);

                }
            }
        }

        /// <summary>
        /// Getter and setter for the number of color for the kMeans algorithm
        /// </summary>
        public int KMeansColorNumber
        {
            get => _kmeansColorNumber;
            set
            {
                // Only allow up to "number of color in the image - 1"
                if (value < _loadedImageNumberColors)
                {
                    ConfigManagement.SetKMeansClustersToConfigFile(value);
                    SetProperty(ref _kmeansColorNumber, value);
                }
                else // value bigger or equal to the number of color in the image
                {
                    ConfigManagement.SetKMeansClustersToConfigFile(_loadedImageNumberColors - 1);
                    SetProperty(ref _kmeansColorNumber, _loadedImageNumberColors - 1);
                }
            }
        }

        /// <summary>
        /// Getter and setter for the number of iteration for the kMeans algorithm
        /// </summary>
        public int KMeansIterationNumber
        {
            get => _kmeansIterationNumber;
            set
            {
                ConfigManagement.SetKMeansIterationsToConfigFile(value);
                SetProperty(ref _kmeansIterationNumber, value);
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
                if (Im != null && (value > MinimumWidthHeight || Im.ImageHeight == -1))
                {
                    if ((int)(value * _ratio) > MinimumWidthHeight || Im.ImageHeight == -1)
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
                        string badValue = ((int)(value * _ratio)).ToString();
                        WpfMessageBox.Show("", string.Format(Assets.Languages.Resource.Convert_MinimumHeight_WithValue, badValue) + MinimumWidthHeight);
                    }

                }
                else
                {
                    WpfMessageBox.Show("", Assets.Languages.Resource.Convert_MinimumHeight + MinimumWidthHeight);
                }
            }
        }

        /// <summary>
        /// Function called when user navigate to this page. It initialize the view model and display a message in the terminal
        /// </summary>
        public void OnNavigatedTo()
        {
            LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_ConvertPage);

            // Set the default value for the kmeans algorithm
            IsImageLoaded = true;
            KMeansColorNumber = ConfigManagement.GetKMeansClustersFromConfigFile();
            KMeansIterationNumber = ConfigManagement.GetKMeansIterationsFromConfigFile();
            CurColorModel = ConfigManagement.GetColorModelFromConfigFile();
            if (LoadedImage == null)
            {
                IsImageLoaded = false;
            }
            InitializeViewModel();
        }

        /// <summary>
        /// Initialize the view model and load the image if there is one in the temp folder
        /// </summary>
        private void InitializeViewModel()
        {
            // If not already initialized
            if (Im != null) return;
            Im = new ImageManagement(new Win32OpenFileDialogAdapter());

            // Try to load the last saved image
            Im.LoadCurrentImage();
            if (Im.Image == null) return;
            LoadedImage = Im.Image;
            _ratio = Im.Ratio;
            ImageWidth = Im.ImageWidth;
            ImageHeight = Im.ImageHeight;
            UpdateLoadedImageNumberColors();
        }

        /// <summary>
        /// Function called when user click on the button to load an image from the file system
        /// </summary>
        [RelayCommand]
        private void LoadImage()
        {
            // Open file dialog
            Im?.LoadImageDialog();
            LoadedImage = Im?.Image;
            if (Im == null || LoadedImage == null) return;
            _ratio = Im.Ratio;
            ImageWidth = Im.ImageWidth;
            ImageHeight = Im.ImageHeight;
            UpdateLoadedImageNumberColors();
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
            _loadedImageNumberColors = int.MaxValue;
        }


        /// <summary>
        /// Function called when user click on the button to convert the image. It also navigate to the Export page
        /// </summary>
        [RelayCommand]
        private void ConvertImage()
        {
            // Warn the user if the image is too big
            if (ImageWidth * ImageHeight >= 20000) // More than 20000 stitches
            {
                Wpf.Ui.Controls.MessageBox messageBox = new()
                {
                    Content = new System.Windows.Controls.TextBlock()
                    {
                        Text = Assets.Languages.Resource.Convert_BigEmbroideryWarning,
                        TextWrapping = TextWrapping.Wrap
                    },
                    ButtonLeftName = "Ok",
                    ButtonRightName = Assets.Languages.Resource.Terminal_Cancel,
                    // Change DialogResult in TemplateButtonCommand depending on if we press the left or right button
                };
                messageBox.ButtonLeftClick += (sender, args) => messageBox.DialogResult = true;
                messageBox.ButtonRightClick += (sender, args) => messageBox.DialogResult = false;
                bool? confirm = messageBox.ShowDialog();

                if (confirm != true) return; // Don't convert the image and stay on convert page
            }

            // Change grid visibility
            GlobalGridVisibility = false;
            ProgressVisibility = "Visible";

            // create new BackgroundWorker
            BackgroundWorker worker = new()
            {
                Site = null,
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            worker.WorkerReportsProgress = true;

            // Create Timer here
            Stopwatch sw = new();
            sw.Start();

            worker.DoWork += (sender, args) =>
            {
                // Check if the object was successfully passed
                if (args.Argument is not ImageManagement imageManagement) return;
                imageManagement.ResizeImage(KMeansColorNumber, KMeansIterationNumber, worker);

            };

            worker.ProgressChanged += (sender, args) =>
            {
                LogManagement.UpdateProgression(args.ProgressPercentage, 100);
                ProgressConvert = args.ProgressPercentage;
                ProgressTb = args.ProgressPercentage + "%";
            };

            worker.RunWorkerCompleted += (sender, args) =>
            {
                sw.Stop();

                GlobalGridVisibility = true;
                // ProgressBar Percentage
                ProgressTb = "0%";
                ProgressConvert = 0;
                ProgressVisibility = "Hidden";
                LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_ImageConvertedOk + Assets.Languages.Resource.Terminal_InTime + sw.ElapsedMilliseconds + "ms");
                INavigation? navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
                if (navigationService != null)
                {
                    _ = navigationService.Navigate(typeof(ExportPage)); // Navigate to the Convert page.
                }
            };

            // start worker
            worker.RunWorkerAsync(Im);

        }

        /// <summary>
        /// Function called when user click on the button to go back to the main page
        /// </summary>
        public void OnNavigatedFrom()
        {

        }

        /// <summary>
        /// Update the _loadedImageNumberColors field using the current LoadedImage
        /// </summary>
        private void UpdateLoadedImageNumberColors()
        {
            if (LoadedImage != null)
            {
                HashSet<Color> colors = new();
                Brush[,] imageBrushes = ImageTo2DArrayBrushes.ConvertTo2dArray(LoadedImage);
                int pixelWidth = LoadedImage.PixelWidth;
                int pixelHeight = LoadedImage.PixelHeight;
                for (int i = 0; i < pixelWidth; i++)
                {
                    LogManagement.UpdateProgression(i, pixelWidth);
                    for (int j = 0; j < pixelHeight; j++)
                    {
                        colors.Add(((SolidColorBrush)imageBrushes[i, j]).Color);
                    }
                }
                LogManagement.UpdateProgression(Wpf.Ui.TaskBar.TaskBarProgressState.None);
                _loadedImageNumberColors = colors.Count;
            }
            else
            {
                _loadedImageNumberColors = int.MaxValue;
            }

            // Change back to default KMeansColorNumber only if superior to the current max
            // This allows to load the saved number of colors for K-Means, when possible
            if (KMeansColorNumber >= _loadedImageNumberColors)
            {
                KMeansColorNumber = 5;
            }
        }
    }
}