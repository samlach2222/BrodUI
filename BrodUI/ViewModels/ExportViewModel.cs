using BrodUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows;
using Wpf.Ui.Common.Interfaces;

namespace BrodUI.ViewModels
{
    public partial class ExportViewModel : ObservableObject, INavigationAware
    {
        private Uri? _loadedImage = null;

        private ImageManagement Im { get; set; }

        public Uri? LoadedImage
        {
            get { MessageBox.Show(_loadedImage.ToString()); return _loadedImage; }
            set
            {
                
                _loadedImage = value;
                Im.ImageLink = value;
                OnPropertyChanged();
            }
        }

        public void OnNavigatedTo()
        {
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
