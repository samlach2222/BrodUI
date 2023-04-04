using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Wpf.Ui.Common.Interfaces;

namespace BrodUI.ViewModels
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        // THEME STUFF
        [ObservableProperty]
        private string[] _themes = new string[] {"System", "Light", "Dark" };
        private string _curTheme;
        public string CurTheme
        {
            get => _curTheme;
            set
            {
                ChangeTheme(value);
                SetProperty(ref _curTheme, value);
            }
        }
        private void ChangeTheme(string localTheme)
        {
            switch (localTheme)
            {
                case "Light":
                    if (CurTheme != "Light")
                    {
                        Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light, Wpf.Ui.Appearance.BackgroundType.None);
                    }
                    break;
                case "Dark":
                    if (CurTheme != "Dark")
                    {
                        Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark, Wpf.Ui.Appearance.BackgroundType.None);
                    }
                    break;
                default:
                    if (CurTheme != "System")
                    {
                        // Get current theme from Windows 11
                        var theme = Wpf.Ui.Appearance.Theme.GetSystemTheme();
                        // Apply it
                        if (theme == Wpf.Ui.Appearance.SystemThemeType.Light)
                        {
                            Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light, Wpf.Ui.Appearance.BackgroundType.None);
                        }
                        else
                        {
                            Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark, Wpf.Ui.Appearance.BackgroundType.None);
                        }
                    }
                    break;
            }
        }

        // LANGUAGE STUFF
        [ObservableProperty]
        private string[] _languages = new string[] {"English", "Français"};
        private string _curLanguage;
        public string CurLanguage
        {
            get => _curLanguage;
            set
            {
                SetProperty(ref _curLanguage, value);
                ChangeLanguage(_curLanguage);
            }
        }
        private void ChangeLanguage(string curLanguage)
        {
            // TODO : Manage language change
        }

        [ObservableProperty]
        private string _appVersion = String.Empty;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            // TODO : Load data from a file or something
            CurTheme = "System";
            CurLanguage = "English";
            AppVersion = $"BrodUI - {GetAssemblyVersion()}";

            _isInitialized = true;
            
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
        }
    }
}