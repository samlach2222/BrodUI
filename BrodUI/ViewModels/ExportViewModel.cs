using BrodUI.Helpers;
using BrodUI.Models;
using BrodUI.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BrodUI.ViewModels
{
    public partial class ExportViewModel : ObservableObject, INavigationAware
    {
        private Uri? _loadedImage = null;
        private List<Wire> _wireArray = new List<Wire>();
        private DataTable _brushArray;

        private ImageManagement Im { get; set; }

        public Uri? LoadedImage
        {
            get { return _loadedImage; }
            set
            {

                _loadedImage = value;
                Im.ImageLink = value;
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
            // END OF TEMPLATE VALUES
            LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_ExportPage);
            if (Im == null) // if not already initialized
            {
                Im = new ImageManagement();
            }
            Im.LoadImageFromTemp();
            LoadedImage = Im.ImageLink;

            // If no converted image found, redirect to the convert page
            if (LoadedImage == null)
            {
                MessageBox.Show("No converted image found, please convert an image first");
                var navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
                if (navigationService != null)
                {
                    _ = navigationService.Navigate(typeof(ConvertPage)); // Navigate to the Convert page.
                }
            }
            else
            {
                // HERE IS TEMPLATE VALUES
                WireArray.Add(new Wire(new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)255, (byte)255)), 0, "DMC", "White", 0));
                WireArray.Add(new Wire(new SolidColorBrush(Color.FromArgb(255, (byte)0, (byte)255, (byte)255)), 0, "DMC", "White", 0));
                WireArray.Add(new Wire(new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)0, (byte)255)), 0, "DMC", "White", 0));
                WireArray.Add(new Wire(new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)255, (byte)0)), 0, "DMC", "White", 0));

                int width = Im.ImageWidth;
                int height = Im.ImageHeight;
                var img = Im.Image;
                Brush[,] wireTable = ImageToDataTable.ConvertTo2dArray(img);
                DataTable dt = ImageToDataTable.Convert2dArrayToDataTable(wireTable);
                BrushArray = dt;
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
