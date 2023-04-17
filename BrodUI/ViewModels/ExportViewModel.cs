using BrodUI.Helpers;
using BrodUI.Models;
using BrodUI.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using MessageBox = System.Windows.MessageBox;

namespace BrodUI.ViewModels
{
    public partial class ExportViewModel : ObservableObject, INavigationAware
    {
        private BitmapImage? _loadedImage = null;
        private List<Wire> _wireArray = new List<Wire>();
        private DataTable _brushArray;
        private Grid _gridImage;
        static sbyte lastPercentage = -1;

        public Grid GridImage
        {
            get { return _gridImage; }
            set
            {
                _gridImage = value;
                OnPropertyChanged();
            }
        }

        private ImageManagement Im { get; set; }

        public BitmapImage? LoadedImage
        {
            get { return _loadedImage; }
            set
            {

                _loadedImage = value;
                Im.Image = value;
                OnPropertyChanged();
            }
        }

        public List<Wire> WireArray
        {
            get { return _wireArray; }
            set
            {
                _wireArray = value;
                OnPropertyChanged();
            }
        }

        public DataTable BrushArray
        {
            get { return _brushArray; }
            set
            {
                _brushArray = value;
                OnPropertyChanged();
            }
        }

        public void OnNavigatedTo()
        {
            LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_ExportPage);
            if (Im == null) // if not already initialized
            {
                Im = new ImageManagement();
            }
            Im.LoadImageFromTemp();
            LoadedImage = Im.Image;

            // If no converted image found, redirect to the convert page
            if (LoadedImage == null)
            {
                MessageBox.Show(Assets.Languages.Resource.Export_NoImageMessage);
                var navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
                if (navigationService != null)
                {
                    _ = navigationService.Navigate(typeof(ConvertPage)); // Navigate to the Convert page.
                }
            }
            else
            {
                // HERE IS TEMPLATE VALUES
                int width = Im.ImageWidth;
                int height = Im.ImageHeight;
                var img = Im.Image;
                Brush[,] wireTable = ImageToDataTable.ConvertTo2dArray(img);
                // add Wires to WireArray from wireTable for each color

                // LOADING COUNT
                int count = 0;
                int imageSize = width * height;

                LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Export_ConvertToCrossStitchEmbroidery);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (wireTable[i, j] != null)
                        {
                            count++;
                            ShowProgression(count, imageSize);

                            bool found = false;
                            foreach (Wire wire in WireArray)
                            {
                                // get Color of wire
                                Color color1 = ((SolidColorBrush)wireTable[i, j]).Color;
                                // get Color of wire in WireArray
                                Color color2 = ((SolidColorBrush)wire.Color).Color;
                                if (color1 == color2)
                                {
                                    wire.Quantity++;
                                    found = true;
                                }
                            }

                            if (!found)
                            {
                                // TODO add color name, Type and Number when the work is done
                                WireArray.Add(new Wire(wireTable[i, j], 404, "DMC", "White", 1));
                            }
                        }
                    }
                }
                Console.WriteLine(); // Line break
                LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Export_ConvertToCrossStitchEmbroideryDone);

                // GridImage creation part
                // reset GridImage
                GridImage.Children.Clear();
                GridImage.RowDefinitions.Clear();
                GridImage.ColumnDefinitions.Clear();
                // Set GridImage size to keep the aspect ratio

                // create rows and columns for GridImage
                for (int i = 0; i < height; i++)
                {
                    RowDefinition row = new RowDefinition();
                    row.Height = new GridLength(15, GridUnitType.Pixel);
                    GridImage.RowDefinitions.Add(row);
                }

                for (int i = 0; i < width; i++)
                {
                    ColumnDefinition col = new ColumnDefinition();
                    col.Width = new GridLength(15, GridUnitType.Pixel);
                    GridImage.ColumnDefinitions.Add(col);
                }

                // add rectangle for each row/column
                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        var rect = new System.Windows.Shapes.Rectangle();
                        rect.Fill = wireTable[j, i];
                        rect.Stroke = new SolidColorBrush(Colors.Black);
                        rect.StrokeThickness = 1;
                        Grid.SetRow(rect, i);
                        Grid.SetColumn(rect, j);
                        GridImage.Children.Add(rect);
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
            int pourcentage = value * 100 / max;
            if (pourcentage < lastPercentage)  // A new progression is happening
            {
                lastPercentage = -1;
            }
            if (pourcentage > lastPercentage)
            {
                lastPercentage = (sbyte)pourcentage;
                string barre = "";
                for (int i = 0; i < 10; i++)
                {
                    if (pourcentage / 10 > i)
                    {
                        barre += '*';
                    }
                    else
                    {
                        barre += ' ';
                    }
                }
                Console.Write("\r[" + barre + "] " + pourcentage + '%');
            }
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
        }
    }
}
