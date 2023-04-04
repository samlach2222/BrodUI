using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BrodUI.Services;
using BrodUI.Views.Pages;
using BrodUI.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace BrodUI.ViewModels
{
    public partial class TutorialViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private string tutorialText = "Coucou";

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        public void GoConvertPage()
        {
            var navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
            if (navigationService != null)
            {
                _ = navigationService.Navigate(typeof(ConvertPage)); // Navigate to the Convert page.
            }
        }
    }
}
