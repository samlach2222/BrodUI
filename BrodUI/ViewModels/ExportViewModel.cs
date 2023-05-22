using BrodUI.Helpers;
using BrodUI.Models;
using BrodUI.Services;
using BrodUI.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Extensions;
using Wpf.Ui.Mvvm.Contracts;

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
                WpfMessageBox.Show("", Assets.Languages.Resource.Export_NoImageMessage);
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

                // Fill a list of all unique colors in the image
                List<Color> colors = new();
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        count++;
                        LogManagement.UpdateProgression(count, countMax);

                        // We can't use wireTable[i, j] directly,
                        // because brushes with the same color are considered not equals
                        Color color = ((SolidColorBrush)wireTable[i, j]).Color;

                        if (!colors.Contains(color))
                        {
                            colors.Add(color);
                        }
                    }
                }

                // Initialise variables for the colors to DMC conversion
                Dictionary<Color, Color> colorToDmcColor = new(colors.Count); // Used to convert colors at the grid creation
                dynamic colorToDmc = ConfigManagement.GetColorModelFromConfigFile() switch
                {
                    "HSL" => new HslToDmc(),
                    "RGB" or _ => new RgbToDmc(),
                };
                DmcToString dmcToString = new();

                // Convert colors to DMC and add the wires to TempWireArray (to later merge the wires with same colors)
                foreach (Color color in colors)
                {
                    StringBuilder terminalOutput = new();
                    Color dmcColor;
                    int dmc;
                    switch (ConfigManagement.GetColorModelFromConfigFile())
                    {
                        case "HSL":
                            HslToDmc hslToDmc = (HslToDmc)colorToDmc;

                            // Get DMC number
                            (float hue, float saturation, float lightness) = color.ToHsl();
                            (int hueInt, int saturationInt, int lightnessInt) = ((int)hue % 360, (int)saturation, (int)lightness);
                            dmc = hslToDmc.GetValDmc(hueInt, saturationInt, lightnessInt);

                            // Get color from DMC number
                            (int hueHsl, int saturationHsl, int lightnessHsl) = (hslToDmc.GetHue(dmc), hslToDmc.GetSaturation(dmc), hslToDmc.GetLightness(dmc));
                            (int rHslInt, int gHslInt, int bHslInt) = ColorExtensions.FromHslToRgb(hueHsl, saturationHsl, lightnessHsl);
                            dmcColor = Color.FromRgb((byte)rHslInt, (byte)gHslInt, (byte)bHslInt);

                            // Construct terminal output (spaces needed before → to have the arrow at the same position everytime)
                            terminalOutput.Append("H:" + hueInt + " S:" + saturationInt + " L:" + lightnessInt + "     \t→\tDMC:" + dmc + " H:" + hueHsl + " S:" + saturationHsl + " L:" + lightnessHsl);
                            break;
                        default:
                            RgbToDmc rgbToDmc = (RgbToDmc)colorToDmc;

                            // Get DMC number
                            dmc = rgbToDmc.GetValDmc(color.R, color.G, color.B);

                            // Get color from DMC number
                            (byte rRgb, byte gRgb, byte bRgb) = ((byte)rgbToDmc.GetRed(dmc), (byte)rgbToDmc.GetGreen(dmc), (byte)rgbToDmc.GetBlue(dmc));
                            dmcColor = Color.FromRgb(rRgb, gRgb, bRgb);

                            // Construct terminal output (spaces needed before → to have the arrow at the same position everytime)
                            terminalOutput.Append("R:" + color.R + " G:" + color.G + " B:" + color.B + "     \t→\tDMC:" + dmc + " R:" + rRgb + " G:" + gRgb + " B:" + bRgb);
                            break;
                    }
                    string colorName = dmcToString.GetNameDmc(dmc);
                    terminalOutput.Append(" Name:" + colorName); // Add name to terminal output

                    // Add to WireArray only if the DMC color wasn't already obtained from another converted color
                    if (!colorToDmcColor.ContainsValue(dmcColor))
                    {
                        WireArray.Add(new Wire(new SolidColorBrush(dmcColor), dmc, "DMC", colorName, 0));
                    }

                    // Output the color conversion to the terminal and add it to the dictionary
                    colorToDmcColor.Add(color, dmcColor);
                    Console.WriteLine(terminalOutput.ToString());
                }

                // Convert image to 2D int array of color index
                int[,] dmcImage = new int[height, width];
                ImmutableArray<Color> DmcColorArray = colorToDmcColor.Values.Distinct().ToImmutableArray();
                for (int row = 0; row < height; row++)
                {
                    for (int column = 0; column < width; column++)
                    {
                        dmcImage[row, column] = DmcColorArray.IndexOf(colorToDmcColor[((SolidColorBrush)wireTable[column, row]).Color]);
                    }
                }

                // Get length for each wire
                for (int i = 0; i < WireArray.Count; i++)
                {
                    // Displaying a little more length than needed is better than opposite, so we round to nearest greater integer
                    double length = Math.Ceiling(new LengthThread(i, dmcImage).TotalLength);
                    WireArray[i].Length = (long)length;
                }

                // GridImage creation part
                // reset GridImage
                GridImage!.Children.Clear();
                GridImage!.RowDefinitions.Clear();
                GridImage!.ColumnDefinitions.Clear();

                int cellSize = int.Parse(ConfigManagement.GetEmbroiderySizeFromConfigFile());

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
                            Background = new SolidColorBrush(colorToDmcColor[((SolidColorBrush)wireTable[i, j]).Color])
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
        private void ExportToPdf() // TODO : LOADING ANIMATION USING PROGRESSRING OR PROGRESBAR (BETTER)
        {
            _ = new PdfManagement(WireArray);
        }
    }
}
