using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace BrodUI.Models
{
    /// <summary>
    /// Class to manage the config file
    /// </summary>
    public static class ConfigManagement
    {
        /// <summary>
        /// Path of the config file
        /// </summary>
        private static string path;

        /// <summary>
        /// Create the config file if it doesn't exist and set the default values who are :
        /// Theme=System
        /// Language=English
        /// Terminal=false
        /// </summary>
        public static void CreateConfigFileIfNotExists()
        {
            // get Appdata Roaming folder in a string
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // create the folder "BrodUI" in AppData if it doesn't exist
            if (!Directory.Exists(appData + "\\BrodUI"))
            {
                Directory.CreateDirectory(appData + "\\BrodUI");
            }
            // path combine with the file name
            path = Path.Combine(appData + "\\BrodUI", "settings.cfg");
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "Theme=System\nLanguage=English\nTerminal=false");
            }
        }

        /// <summary>
        /// Delete the config file
        /// </summary>
        public static void DeleteConfigFile()
        {
            File.Delete(path);
        }

        /// <summary>
        /// Set the theme of the app from the config file. There is 3 themes :
        /// Light
        /// Dark
        /// System (Light or Dark depending on the system theme)
        /// </summary>
        public static void SetTheme()
        {
            var settings = File.ReadAllLines(path);
            // Set theme
            switch (settings[0].Split('=')[1])
            {
                case "System":
                    var theme = Wpf.Ui.Appearance.Theme.GetSystemTheme();
                    // Apply it
                    if (theme == Wpf.Ui.Appearance.SystemThemeType.Light)
                    {
                        Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
                    }
                    else
                    {
                        Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
                    }
                    break;
                case "Light":
                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
                    break;
                case "Dark":
                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
                    break;
            }
        }

        /// <summary>
        /// Set the language of the app from the config file. There is 2 languages for the moment :
        /// French
        /// English
        /// </summary>
        public static void SetLanguage()
        {
            var settings = File.ReadAllLines(path);
            // Set language
            switch (settings[1].Split('=')[1])
            {
                case "System":
                    // Get system language
                    var language = CultureInfo.CurrentCulture.Name;
                    // Set it
                    switch (language)
                    {
                        case "fr":
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr");
                            break;
                        default:
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                            break;
                    }
                    break;
                case "English":
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                    break;
                case "French":
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr");
                    break;
            }
        }

        /// <summary>
        /// Get the theme from the config file
        /// </summary>
        /// <returns>string with the name of the theme in the config file</returns>
        public static string GetThemeFromConfigFile()
        {
            var settings = File.ReadAllLines(path);
            return settings[0].Split('=')[1];
        }

        /// <summary>
        /// Get the language from the config file
        /// </summary>
        /// <returns>string with the name of the language in the config file</returns>
        public static string GetLanguageFromConfigFile()
        {
            var settings = File.ReadAllLines(path);
            return settings[1].Split('=')[1];
        }

        /// <summary>
        /// Get the terminal from the config file
        /// </summary>
        /// <returns>bool which is true if the terminal is active, or false if not active in the config file</returns>
        public static bool GetTerminalFromConfigFile()
        {
            var settings = File.ReadAllLines(path);
            return Convert.ToBoolean(settings[2].Split('=')[1]);
        }

        /// <summary>
        /// Set the theme in the config file
        /// </summary>
        /// <param name="theme">theme you want to put in the config file</param>
        public static void SetThemeToConfigFile(string theme)
        {
            // save the language in the file "settings.cfg" in the first row
            var settings = File.ReadAllLines(path);
            settings[0] = $"Theme={theme}";
            File.WriteAllLines(path, settings);
        }

        /// <summary>
        /// Set the language in the config file
        /// </summary>
        /// <param name="language">language you want to put in the config file</param>
        public static void SetLanguageToConfigFile(string language)
        {
            var settings = File.ReadAllLines(path);
            settings[1] = $"Language={language}";
            File.WriteAllLines(path, settings);
        }

        /// <summary>
        /// Set the terminal in the config file
        /// </summary>
        /// <param name="terminal">bool you want to put in the config file (true if terminal is activated)</param>
        public static void SetTerminalToConfigFile(bool terminal)
        {
            var settings = File.ReadAllLines(path);
            settings[2] = $"Terminal={terminal}";
            File.WriteAllLines(path, settings);
        }

        /// <summary>
        /// Set the theme from the settings
        /// </summary>
        /// <param name="theme">theme you want to apply in the app</param>
        public static void SetThemeFromSettings(string theme)
        {
            switch (theme)
            {
                case "Light":
                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light, Wpf.Ui.Appearance.BackgroundType.None);
                    break;
                case "Dark":
                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark, Wpf.Ui.Appearance.BackgroundType.None);
                    break;
                default:
                    // Get current theme from Windows 11
                    var sysTheme = Wpf.Ui.Appearance.Theme.GetSystemTheme();
                    // Apply it
                    if (sysTheme == Wpf.Ui.Appearance.SystemThemeType.Light)
                    {
                        Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light, Wpf.Ui.Appearance.BackgroundType.None);
                    }
                    else
                    {
                        Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark, Wpf.Ui.Appearance.BackgroundType.None);
                    }
                    break;
            }
        }
    }
}
