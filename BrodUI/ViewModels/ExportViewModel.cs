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
using Wpf.Ui.Extensions;
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
                        LogManagement.UpdateProgression(count, countMax);

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

                // Initialise variables for the colors to DMC conversion
                List<Wire> TempWireArray = new();
                Dictionary<Color, SolidColorBrush> colorToDmcBrush = new(colorQuantity.Count); // Used to convert colors at the grid creation
                dynamic colorToDmc = ConfigManagement.GetColorModelFromConfigFile() switch
                {
                    "HSL" => new HslToDmc(),
                    "RGB" or _ => new RgbToDmc(),
                };
                colorToDmc.PrintFileContent();
                DmcToString dmcToString = new();
                dmcToString.PrintFileContent();

                // Convert colors to DMC and add the wires to TempWireArray (to later merge the wires with same colors)
                foreach (KeyValuePair<Color, int> color in colorQuantity) // TODO : ERROR WHEN COLOR IS "6" (RGB TESTED) 
                {
                    SolidColorBrush scbColor;
                    int dmc;
                    switch (ConfigManagement.GetColorModelFromConfigFile())
                    {
                        case "HSL":
                            HslToDmc hslToDmc = (HslToDmc)colorToDmc;

                            // Get DMC number
                            (float hue, float saturation, float lightness) = color.Key.ToHsl();
                            dmc = hslToDmc.GetValDmc((((int)hue) % 360), ((int)saturation), ((int)lightness));

                            // Get color from DMC number
                            (int RHslInt, int GHslInt, int BHslInt) = ColorExtensions.FromHslToRgb(hslToDmc.getHue(dmc), hslToDmc.getSaturation(dmc), hslToDmc.getLightness(dmc));
                            (byte RHsl, byte GHsl, byte BHsl) = ((byte)RHslInt, (byte)GHslInt, (byte)BHslInt);
                            scbColor = new(Color.FromRgb(RHsl, GHsl, BHsl));
                            break;
                        default:
                            RgbToDmc rgbToDmc = (RgbToDmc)colorToDmc;

                            // Get DMC number
                            dmc = rgbToDmc.GetValDmc(color.Key.R, color.Key.G, color.Key.B);

                            // Get color from DMC number
                            (byte RRgb, byte GRgb, byte BRgb) = ((byte)rgbToDmc.getRed(dmc), (byte)rgbToDmc.getGreen(dmc), (byte)rgbToDmc.getBlue(dmc));
                            scbColor = new(Color.FromRgb(RRgb, GRgb, BRgb));
                            break;
                    }
                    string colorName = dmcToString.GetNameDmc(dmc);

                    // Add the old color and new DMC color (as brush) to the color conversion dictionary
                    colorToDmcBrush.Add(color.Key, scbColor);

                    TempWireArray.Add(new Wire(scbColor, dmc, "DMC", colorName, color.Value));
                }

                // Add the wires to WireArray, by merging the wires with same colors
                foreach (Wire tempWire in TempWireArray)
                {
                    // If the color is already in WireArray, add the quantity to the existing wire
                    foreach (Wire wire in WireArray)
                    {
                        // We have to compare the colors (comparing the brushes don't work) 
                        if (((SolidColorBrush)wire.Color).Color == ((SolidColorBrush)tempWire.Color).Color)
                        {
                            wire.Quantity += tempWire.Quantity;
                            goto NextWire; // Skip to next TempWireArray wire
                        }
                    }
                    // The color was not in WireArray, so we add it
                    WireArray.Add(tempWire);

                NextWire:;
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
                        LogManagement.UpdateProgression(count, countMax);

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
                            Background = colorToDmcBrush[((SolidColorBrush)wireTable[i, j]).Color]
                        };

                        Grid.SetRow(border, j);
                        Grid.SetColumn(border, i);
                        GridImage.Children.Add(border);
                    }
                }
                LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Export_ConvertToCrossStitchEmbroideryDone);
            }
        }

        /// <summary>
        /// Function called when you go to other page
        /// </summary>
        public void OnNavigatedFrom()
        {
            // Clear the page elements
            GridImage?.Children.Clear();
            GridImage?.RowDefinitions.Clear();
            GridImage?.ColumnDefinitions.Clear();
            LoadedImage = null;
            WireArray = new List<Wire>();
        }

        [RelayCommand]
        private void ExportToPdf() // TODO : LOADING ANIMATION USING PROGRESSRING OR PROGRESBAR (BETTER) AND TASKBARPROGRESS 
        {
            _ = new PdfManagement(WireArray);
        }
    }
}
