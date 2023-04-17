using BrodUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BrodUI.ViewModels
{
    /// <summary>
    /// Class MainWindowViewModel.
    /// </summary>
    public partial class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        /// bool to check if the viewmodel is initialized
        /// </summary>
        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets the application title.
        /// </summary>
        [ObservableProperty]
        private string _applicationTitle = string.Empty;

        /// <summary>
        /// Gets or sets the navigation items.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        /// <summary>
        /// Gets or sets the navigation footer.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        /// <summary>
        /// Gets or sets the tray menu items.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        /// <summary>
        /// Gets or sets the selected navigation item.
        /// </summary>
        /// <param name="navigationService"></param>
        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        private void InitializeViewModel()
        {
            ApplicationTitle = "BrodUI";

            ConfigManagement.CreateConfigFileIfNotExists();
            ConfigManagement.SetLanguage();

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = Assets.Languages.Resource.PageName_Tutorial,
                    PageTag = "tutorial",
                    Icon = SymbolRegular.QuestionCircle24, // QuestionCircle is the icon, 24 is the size
                    PageType = typeof(Views.Pages.TutorialPage)
                },
                new NavigationItem()
                {
                    Content = Assets.Languages.Resource.PageName_Convert,
                    PageTag = "convert",
                    Icon = SymbolRegular.ConvertRange24,
                    PageType = typeof(Views.Pages.ConvertPage)
                },
                new NavigationItem()
                {
                    Content = Assets.Languages.Resource.PageName_Export,
                    PageTag = "export",
                    Icon = SymbolRegular.ArrowExportLtr24,
                    PageType = typeof(Views.Pages.ExportPage)
                }
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = Assets.Languages.Resource.PageName_Settings,
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;
        }
    }
}
