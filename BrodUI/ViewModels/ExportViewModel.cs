using BrodUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;

namespace BrodUI.ViewModels
{
    public partial class ExportViewModel : ObservableObject, INavigationAware
    {
        private Uri? _loadedImage = null;
        private List<Wire> _wireArray = new List<Wire>();

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

        public void OnNavigatedTo()
        {
            // HERE IS TEMPLATE VALUES
            WireArray.Add(new Wire(new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)255, (byte)255)), 0, "DMC", "White", 0));
            WireArray.Add(new Wire(new SolidColorBrush(Color.FromArgb(255, (byte)0, (byte)255, (byte)255)), 0, "DMC", "White", 0));
            WireArray.Add(new Wire(new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)0, (byte)255)), 0, "DMC", "White", 0));
            WireArray.Add(new Wire(new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)255, (byte)0)), 0, "DMC", "White", 0));
            // END OF TEMPLATE VALUES
            LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_ExportPage);
            if (Im == null) // if not already initialized
            {
                Im = new ImageManagement();
            }
            Im.LoadImageFromTemp();
            LoadedImage = Im.ImageLink;
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
        }
    }
}
