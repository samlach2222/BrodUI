using BrodUI.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Application = System.Windows.Application;

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
