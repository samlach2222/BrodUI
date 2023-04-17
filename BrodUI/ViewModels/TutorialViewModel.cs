using BrodUI.Models;
using BrodUI.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Application = System.Windows.Application;

namespace BrodUI.ViewModels
{
    /// <summary>
    /// Class TutorialViewModel
    /// </summary>
    public partial class TutorialViewModel : ObservableObject, INavigationAware
    {
        // TODO : Add a tutorial page to explain how to use the application.

        /// <summary>
        /// Tutorial text of the app.
        /// </summary>
        [ObservableProperty]
        private string _tutorialText = "Coucou";

        /// <summary>
        /// Function called when the page is navigated to.
        /// </summary>
        public void OnNavigatedTo()
        {
            LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_TutorialPage);
        }

        /// <summary>
        /// Function called when the page is navigated from.
        /// </summary>
        public void OnNavigatedFrom()
        {

        }

        /// <summary>
        /// Go to the Convert page.
        /// </summary>
        [RelayCommand]
        public static void GoConvertPage()
        {
            INavigation? navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
            if (navigationService != null)
            {
                _ = navigationService.Navigate(typeof(ConvertPage)); // Navigate to the Convert page.
            }
        }
    }
}
