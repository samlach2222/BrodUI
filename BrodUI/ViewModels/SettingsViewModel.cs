using BrodUI.Helpers;
using BrodUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Common.Interfaces;

namespace BrodUI.ViewModels
{
    /// <summary>
    /// Class SettingsViewModel
    /// </summary>
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        /// <summary>
        /// bool to check if the viewmodel is initialized
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        /// Possible themes of the application
        /// </summary>
        [ObservableProperty]
        private string[] _themes = { "System", "Light", "Dark" }; // TODO : SET THEME TO RESOURCES TO HAVE FR AND EN LANGUAGE

        /// <summary>
        /// Current theme of the application
        /// </summary>
        private string? _curTheme;

        /// <summary>
        /// Gets or sets the current theme
        /// </summary>
        public string? CurTheme
        {
            get => _curTheme;
            set
            {
                // Get current time and date
                LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_ThemeChanged + _curTheme + Assets.Languages.Resource.Terminal_To + value);
                SetProperty(ref _curTheme, value);
                if (value != null)
                {
                    ChangeTheme(value);
                }
            }
        }

        /// <summary>
        /// Change the theme of the application
        /// </summary>
        /// <param name="theme">new theme of the application</param>
        private static void ChangeTheme(string theme)
        {
            // save the theme in the file "settings.cfg" in the first row
            ConfigManagement.SetThemeToConfigFile(theme);

            // apply the new theme
            ConfigManagement.ApplyTheme();
        }

        /// <summary>
        /// Possible languages of the application
        /// </summary>
        [ObservableProperty]
        private string[] _languages = new string[] { "English", "Français" };

        /// <summary>
        /// Current language of the application
        /// </summary>
        private string? _curLanguage;

        /// <summary>
        /// Get or set the current language
        /// </summary>
        public string? CurLanguage
        {
            get => _curLanguage;
            set
            {
                LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_LanguageChanged + _curLanguage + Assets.Languages.Resource.Terminal_To + value);
                if (value != null && value != _curLanguage)
                {
                    ChangeLanguage(value);
                }
                SetProperty(ref _curLanguage, value);
            }
        }

        /// <summary>
        /// Change the language of the application
        /// </summary>
        /// <param name="curLanguage">new language of the application</param>
        private static void ChangeLanguage(string curLanguage)
        {
            // save the language in the file "settings.cfg" in the second row
            ConfigManagement.SetLanguageToConfigFile(curLanguage);

            // restart the BrodUI application
            RestartApp();
        }

        /// <summary>
        /// Is the terminal mode enabled
        /// </summary>
        private bool? _curTerminal;

        /// <summary>
        /// Get or set the terminal mode
        /// </summary>
        public bool? CurTerminal
        {
            get => _curTerminal;
            set
            {
                LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_TerminalChanged + _curTerminal + Assets.Languages.Resource.Terminal_To + value);
                if (_curTerminal != null && value != _curTerminal)
                {
                    ChangeTerminal(value != null && ((bool)value));
                }
                SetProperty(ref _curTerminal, value);
            }
        }

        /// <summary>
        /// Change the terminal mode
        /// </summary>
        /// <param name="curTerminal">new terminal mode</param>
        private void ChangeTerminal(bool curTerminal)
        {
            // save the language in the file "settings.cfg" in the second row
            ConfigManagement.SetTerminalToConfigFile(curTerminal);

            //restart the BrodUI application
            if (_initTerminalDone)
            {
                RestartApp();
            }
        }

        /// <summary>
        /// Size of the embroidery point
        /// </summary>
        private string? _curEmbroiderySize;

        /// <summary>
        /// Get or set the embroidery size
        /// </summary>
        public string? CurEmbroiderySize
        {
            get => _curEmbroiderySize;
            set
            {
                ConfigManagement.SetEmbroiderySizeToConfigFile(value);
                SetProperty(ref _curEmbroiderySize, value);
            }

        }

        /// <summary>
        /// Function to restart the application
        /// </summary>
        private static void RestartApp()
        {
            // Start BrodUI as a new process
            ProcessStartInfo startInfo = new(Environment.ProcessPath!)
            {
                UseShellExecute = true
            };
            Process.Start(startInfo);

            // Close the current BrodUI process
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Function called when the user navigates to the page
        /// </summary>
        public void OnNavigatedTo()
        {
            LogManagement.WriteToLog("[" + DateTime.Now + "] " + Assets.Languages.Resource.Terminal_SettingsPage);
            if (!_isInitialized)
                InitializeViewModel();
        }

        /// <summary>
        /// Function called when the user navigates away from the page
        /// </summary>
        public void OnNavigatedFrom()
        {

        }

        /// <summary>
        /// bool to check if the terminal mode is initialized
        /// </summary>
        private bool _initTerminalDone = false;

        /// <summary>
        /// Initialize the viewmodel
        /// </summary>
        private void InitializeViewModel()
        {
            // Load settings from file
            CurTheme = ConfigManagement.GetThemeFromConfigFile();
            CurLanguage = ConfigManagement.GetLanguageFromConfigFile();
            CurTerminal = ConfigManagement.GetTerminalFromConfigFile();
            CurEmbroiderySize = ConfigManagement.GetEmbroiderySizeFromConfigFile();
            _initTerminalDone = true;

            _isInitialized = true;
        }

        /// <summary>
        /// Reset the parameters of the application
        /// </summary>
        [RelayCommand]
        private static void ResetParameters()
        {
            ConfigManagement.DeleteConfigFile();
            ImageManagement im = new(new Win32OpenFileDialogAdapter());
            im.UnloadImage();
            RestartApp();
        }

        /// <summary>
        /// Delete the logs of the application
        /// </summary>
        [RelayCommand]
        private static void DeleteLogs()
        {
            LogManagement.ClearLog();
            RestartApp();
        }
    }
}