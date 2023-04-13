using BrodUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Common.Interfaces;

namespace BrodUI.ViewModels
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        // THEME STUFF
        [ObservableProperty]
        private string[] _themes = new string[] { "System", "Light", "Dark" };
        private string _curTheme;
        public string CurTheme
        {
            get => _curTheme;
            set
            {
                // Get current time and date
                LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_ThemeChanged + _curTheme + Assets.Languages.Resource.Terminal_To + value);
                SetProperty(ref _curTheme, value);
                if (_curTheme != null)
                {
                    ChangeTheme(value);
                }
            }
        }
        private void ChangeTheme(string theme)
        {
            ConfigManagement.SetThemeFromSettings(theme);

            // save the language in the file "settings.cfg" in the first row
            ConfigManagement.SetThemeToConfigFile(theme);
        }

        // LANGUAGE STUFF
        [ObservableProperty]
        private string[] _languages = new string[] { "English", "Français" };
        private string _curLanguage;
        public string CurLanguage
        {
            get => _curLanguage;
            set
            {
                LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_LanguageChanged + _curLanguage + Assets.Languages.Resource.Terminal_To + value);
                if (_curLanguage != null && value != _curLanguage)
                {
                    ChangeLanguage(value);
                }
                SetProperty(ref _curLanguage, value);
            }
        }
        private void ChangeLanguage(string curLanguage)
        {
            // save the language in the file "settings.cfg" in the second row
            ConfigManagement.SetLanguageToConfigFile(curLanguage);

            // restart the BrodUI application
            var process = Process.GetCurrentProcess();
            var startInfo = new ProcessStartInfo(process.MainModule.FileName);
            startInfo.Arguments = process.Id.ToString();
            Process.Start(startInfo);
            Application.Current.Shutdown();
        }

        // TERMINAL STUFF
        private bool _curTerminal;
        public bool CurTerminal
        {
            get => _curTerminal;
            set
            {
                LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_TerminalChanged + _curTerminal + Assets.Languages.Resource.Terminal_To + value);
                if (_curTerminal != null && value != _curTerminal)
                {
                    ChangeTerminal(value);
                }
                SetProperty(ref _curTerminal, value);
            }
        }
        private void ChangeTerminal(bool curTerminal)
        {
            // save the language in the file "settings.cfg" in the second row
            ConfigManagement.SetTerminalToConfigFile(curTerminal);

            //restart the BrodUI application
            if (_initTerminalDone)
            {
                var process = Process.GetCurrentProcess();
                var startInfo = new ProcessStartInfo(process.MainModule.FileName);
                startInfo.Arguments = process.Id.ToString();
                Process.Start(startInfo);
                Application.Current.Shutdown();
            }
        }

        // SETTINGS PAGE STUFF

        [ObservableProperty]
        private string _appVersion = String.Empty;

        public void OnNavigatedTo()
        {
            LogManagement.WriteToLog("[" + DateTime.Now.ToString() + "] " + Assets.Languages.Resource.Terminal_SettingsPage);
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private bool _initTerminalDone = false;
        private void InitializeViewModel()
        {
            // Load settings from file
            CurTheme = ConfigManagement.GetThemeFromConfigFile();
            CurLanguage = ConfigManagement.GetLanguageFromConfigFile();
            CurTerminal = ConfigManagement.GetTerminalFromConfigFile();
            _initTerminalDone = true;
            AppVersion = $"BrodUI - {GetAssemblyVersion()}";

            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
        }
    }
}