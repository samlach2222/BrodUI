using BrodUI.Helpers;
using BrodUI.Models;
using BrodUI.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
                int countMax = width * height * 2; // "* 2" to have the first 50% for the color counting and the second 50% for the grid creation

                LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Export_ConvertToCrossStitchEmbroidery);

                // Use a dictionary to count the quantity of each color
                Dictionary<Color, int> colorQuantity = new();
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        count++;
                        ShowProgression(count, countMax);

                        // We can't use wireTable[i, j] directly,
                        // because even those with the same color are considered not the same object
                        Color color = ((SolidColorBrush)wireTable[i, j]).Color;

                        if (colorQuantity.ContainsKey(color))
                        {
                            colorQuantity[color]++;
                        }
                        else
                        {
                            colorQuantity.Add(color, 1);
                        }
                    }
                }

                // Add the wires to the WireArray
                foreach (KeyValuePair<Color, int> color in colorQuantity)
                {
                    SolidColorBrush scbColor = new(color.Key);
                    // TODO use the RGB values of color.Key to add color number, type and name here when the work is done

                    WireArray.Add(new Wire(scbColor, 404, "DMC", "White", color.Value));
                }

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

                // Add a square for each row/column
                GridImage.Children.Capacity = width * height;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        count++;
                        ShowProgression(count, countMax);

                        double borderLeft = 0.5;
                        const double borderRight = 0.5;
                        double borderTop = 0.5;
                        const double borderBottom = 0.5;

                        if (i % 5 == 0)
                        {
                            borderLeft += 1;

                            if (i % 10 == 0)
                            {
                                borderLeft += 1;
                            }
                        }
                        if (j % 5 == 0)
                        {
                            borderTop += 1;

                            if (j % 10 == 0)
                            {
                                borderTop += 1;
                            }
                        }

                        Border border = new()
                        {
                            BorderBrush = new SolidColorBrush(Colors.Black),
                            BorderThickness = new Thickness(borderLeft, borderTop, borderRight, borderBottom),
                            Background = wireTable[i, j]
                        };

                        Grid.SetRow(border, j);
                        Grid.SetColumn(border, i);
                        GridImage.Children.Add(border);
                    }
                }

                Console.WriteLine(); // Line break
                LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Export_ConvertToCrossStitchEmbroideryDone);
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
