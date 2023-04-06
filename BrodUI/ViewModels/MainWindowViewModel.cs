using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using BrodUI.Models;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using System.Globalization;
using System.IO;
using System.Threading;

namespace BrodUI.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

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
