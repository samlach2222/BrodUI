using System.Windows;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BrodUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for TutorialPage.xaml
    /// </summary>
    public partial class TutorialPage : INavigableView<ViewModels.TutorialViewModel> // TODO : ADD SCROLLBAR TO THE PAGE
    {
        /// <summary>
        /// Getter of the view model
        /// </summary>
        public ViewModels.TutorialViewModel ViewModel
        {
            get;
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="viewModel">viewModel</param>
        public TutorialPage(ViewModels.TutorialViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

        /// <summary>
        /// Event handler to go to the Convert page
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Routed event args</param>
        private void GoConvertPageFromText(object sender, RoutedEventArgs e)
        {
            INavigation? navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
            if (navigationService != null)
            {
                _ = navigationService.Navigate(typeof(ConvertPage)); // Navigate to the Convert page.
            }
        }

        /// <summary>
        /// Event handler to go to the Export page
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Routed event args</param>
        private void GoExportPageFromText(object sender, RoutedEventArgs e)
        {
            INavigation? navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
            if (navigationService != null)
            {
                _ = navigationService.Navigate(typeof(ExportPage)); // Navigate to the Export page.
            }
        }

        /// <summary>
        /// Event handler to go to the Settings page
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Routed event args</param>
        private void GoSettingsPageFromText(object sender, RoutedEventArgs e)
        {
            INavigation? navigationService = (Application.Current.MainWindow as INavigationWindow)?.GetNavigation(); // Get the navigation service from the window.
            if (navigationService != null)
            {
                _ = navigationService.Navigate(typeof(SettingsPage)); // Navigate to the Settings page.
            }
        }
    }
}