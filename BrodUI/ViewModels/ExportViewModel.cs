using BrodUI.Helpers;
using BrodUI.Models;
using BrodUI.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using MessageBox = System.Windows.MessageBox;

namespace BrodUI.ViewModels
{
    /// <summary>
    /// ViewModel for the ExportPage
    /// </summary>
    public partial class ExportViewModel : ObservableObject, INavigationAware
    {
        /// <summary>
        /// Image loaded
        /// </summary>
        private BitmapImage? _loadedImage;

        /// <summary>
        /// Percentage of the progress bar
        /// </summary>
        private static sbyte _lastPercentage = -1;

        /// <summary>
        /// List of wires
        /// </summary>
        private List<Wire> _wireArray = new();

        /// <summary>
        /// Grid of the image
        /// </summary>
        private Grid? _gridImage;

        /// <summary>
        /// Getter and setter for the Grid of the image
        /// </summary>
        public Grid? GridImage
        {
            get => _gridImage;
            set
            {
                _gridImage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Image management
        /// </summary>
        private ImageManagement? Im { get; set; }

        /// <summary>
        /// Getter and setter for the loaded image
        /// </summary>
        public BitmapImage? LoadedImage
        {
            get => _loadedImage;
            set
            {

                _loadedImage = value;
                if (Im != null)
                {
                    Im.Image = value;
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Getter and setter for the list of wires
        /// </summary>
        public List<Wire> WireArray
        {
            get => _wireArray;
            set
            {
                _wireArray = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Function called when the user navigates to the page
        /// Load the image from the temp folder and convert it to a grid
        /// </summary>
        public void OnNavigatedTo()
        {
            LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_ExportPage);
            Im ??= new ImageManagement(new Win32OpenFileDialogAdapter());
            Im.LoadCurrentImage();
            LoadedImage = Im.Image;

            // If no converted image found, redirect to the convert page
            if (LoadedImage == null)
            {
                MessageBox.Show(Assets.Languages.Resource.Export_NoImageMessage);
                INavigation? navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
                if (navigationService != null)
                {
                    _ = navigationService.Navigate(typeof(ConvertPage)); // Navigate to the Convert page.
                }
            }
            else
            {
                int width = Im.ImageWidth;
                int height = Im.ImageHeight;
                BitmapImage? img = Im.Image;
                Brush[,] wireTable = ImageTo2DArrayBrushes.ConvertTo2dArray(img!);
                // add Wires to WireArray from wireTable for each color

                // LOADING COUNT
                int count = 0;
                int imageSize = width * height;

                LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Export_ConvertToCrossStitchEmbroidery);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        count++;
                        ShowProgression(count, imageSize);

                        bool found = false;
                        foreach (Wire? wire in from wire in WireArray let color1 = ((SolidColorBrush)wireTable[i, j]).Color let color2 = ((SolidColorBrush)wire.Color).Color where color1 == color2 select wire)
                        {
                            found = true;
                            wire.Quantity++;
                        }

                        if (!found)
                        {
                            // TODO add color name, Type and Number when the work is done
                            WireArray.Add(new Wire(wireTable[i, j], 404, "DMC", "White", 1));
                        }
                    }
                }
                Console.WriteLine(); // Line break
                LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Export_ConvertToCrossStitchEmbroideryDone);

                // GridImage creation part
                // reset GridImage
                GridImage!.Children.Clear();
                GridImage!.RowDefinitions.Clear();
                GridImage!.ColumnDefinitions.Clear();

                int cellSize = int.Parse(ConfigManagement.GetEmbroiderySizeFromConfigFile()!);

                // create rows and columns for GridImage
                for (int i = 0; i < height; i++)
                {
                    RowDefinition row = new()
                    {
                        Height = new GridLength(cellSize, GridUnitType.Pixel)
                    };
                    GridImage!.RowDefinitions.Add(row);
                }

                for (int i = 0; i < width; i++)
                {
                    ColumnDefinition col = new()
                    {
                        Width = new GridLength(cellSize, GridUnitType.Pixel)
                    };
                    GridImage!.ColumnDefinitions.Add(col);
                }

                // add rectangle for each row/column
                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        Rectangle rect = new()
                        {
                            Fill = wireTable[j, i],
                        };
                        Border border = new()
                        {
                            BorderBrush = new SolidColorBrush(Colors.Black)
                        };

                        double borderLeft = 0.5;
                        const double borderRight = 0.5;
                        double borderTop = 0.5;
                        const double borderBottom = 0.5;

                        if (j % 5 == 0)
                        {
                            borderLeft += 1;
                        }
                        if (j % 10 == 0)
                        {
                            borderLeft += 1;
                        }
                        if (i % 5 == 0)
                        {
                            borderTop += 1;
                        }
                        if (i % 10 == 0)
                        {
                            borderTop += 1;
                        }
                        border.BorderThickness = new Thickness(borderLeft, borderTop, borderRight, borderBottom);
                        border.Child = rect;

                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);
                        GridImage.Children.Add(border);
                    }
                }
            }
        }

        /// <summary>
        /// Show the progression using a bar progressively filled with asterisks
        /// </summary>
        /// <param name="value">Value to calculate a percentage of progression</param>
        /// <param name="max">Maximum value of value to calculate a percentage of progression</param>
        public static void ShowProgression(int value, int max)
        {
            int percentage = value * 100 / max;
            if (percentage < _lastPercentage)  // A new progression is happening
            {
                _lastPercentage = -1;
            }
            if (percentage > _lastPercentage)
            {
                _lastPercentage = (sbyte)percentage;
                string bar = "";
                for (int i = 0; i < 10; i++)
                {
                    if (percentage / 10 > i)
                    {
                        bar += '*';
                    }
                    else
                    {
                        bar += ' ';
                    }
                }
                Console.Write("\r[" + bar + "] " + percentage + '%');
            }
        }

        /// <summary>
        /// Function called when you go to other page
        /// </summary>
        public void OnNavigatedFrom()
        {

        }

        [RelayCommand]
        private void ExportToPdf()
        {
            PdfManagement pdf = new(WireArray);
        }
    }
}
