using BrodUI.Assets.Languages;
using System;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using Wpf.Ui.Appearance;

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
        private static string _path;

        /// <summary>
        /// Create the config file if it doesn't exist and set the default values who are :
        /// Theme=System
        /// Language=English
        /// Terminal=false
        /// </summary>
        public static void CreateConfigFileIfNotExists()
        {
            // get Appdata Roaming folder in a string
            string? appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // create the folder "BrodUI" in AppData if it doesn't exist
            if (!Directory.Exists(appData + "\\BrodUI"))
            {
                Directory.CreateDirectory(appData + "\\BrodUI");
            }
            // path combine with the file name
            _path = Path.Combine(appData + "\\BrodUI", "settings.cfg");
            if (File.Exists(_path)) return;
            // Get the language to use by default
            string language = GetSystemLanguageOrDefault();

            // Get whether we should activate the terminal by default
            bool terminal = false;
#if DEBUG
            terminal = true;
#endif

            File.WriteAllText(_path, "Theme=System\nLanguage=" + language + "\nTerminal=" + terminal + "\nEmbroiderySize=15");
        }

        /// <summary>
        /// Get the name of the Windows display language, or English if the language is not supported
        /// </summary>
        /// <returns>Name of the language in its language (ex: Français, English), or English if not supported</returns>
        public static string GetSystemLanguageOrDefault()
        {
            CultureInfo windowsLangage = CultureInfo.CurrentUICulture; // Get Windows display language
            string twoLettersIso = windowsLangage.TwoLetterISOLanguageName.ToLower(); // Convert to two letters ISO (ex: en-US => en)
            ResourceManager rm = new(typeof(Resource));

            // For each language in all languages
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo culture in cultures)
            {
                // If the language is supported in this app and correspond to the two letters ISO
                // "Supported" means we have a .resx file for this specific language (if we have fr but not fr-FR, only fr is supported)
                ResourceSet? rs = rm.GetResourceSet(culture, true, false);
                if (rs != null && culture.TwoLetterISOLanguageName == twoLettersIso)
                {
                    string nativeName = culture.NativeName;

                    // Some languages use capitalized languages and some don't, so we force it to be capitalized
                    return string.Concat(nativeName[0].ToString().ToUpperInvariant(), nativeName.AsSpan(1));
                }
            }

            return "English";
        }

        /// <summary>
        /// Delete the config file
        /// </summary>
        public static void DeleteConfigFile()
        {
            File.Delete(_path);
        }

        /// <summary>
        /// Set the theme of the app from the config file. There is 3 themes :
        /// Light
        /// Dark
        /// System (Light or Dark depending on the system theme)
        /// </summary>
        public static void SetTheme()
        {
            string[] settings = File.ReadAllLines(_path);
            // Set theme
            switch (settings[0].Split('=')[1])
            {
                case "System":
                    SystemThemeType theme = Theme.GetSystemTheme();
                    // Apply it
                    Theme.Apply(theme == SystemThemeType.Light ? ThemeType.Light : ThemeType.Dark);
                    break;
                case "Light":
                    Theme.Apply(ThemeType.Light);
                    break;
                case "Dark":
                    Theme.Apply(ThemeType.Dark);
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
            string[] settings = File.ReadAllLines(_path);
            // Set language
            switch (settings[1].Split('=')[1])
            {
                case "English":
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                    break;
                case "Français":
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr");
                    break;
            }
        }

        /// <summary>
        /// Get the theme from the config file
        /// </summary>
        /// <returns>string with the name of the theme in the config file</returns>
        public static string? GetThemeFromConfigFile()
        {
            string[] settings = File.ReadAllLines(_path);
            return settings[0].Split('=')[1];
        }

        /// <summary>
        /// Get the language from the config file
        /// </summary>
        /// <returns>string with the name of the language in the config file</returns>
        public static string? GetLanguageFromConfigFile()
        {
            string[] settings = File.ReadAllLines(_path);
            return settings[1].Split('=')[1];
        }

        /// <summary>
        /// Get the terminal from the config file
        /// </summary>
        /// <returns>bool which is true if the terminal is active, or false if not active in the config file</returns>
        public static bool GetTerminalFromConfigFile()
        {
            string[] settings = File.ReadAllLines(_path);
            return Convert.ToBoolean(settings[2].Split('=')[1]);
        }

        /// <summary>
        /// Get the embroidery point size from the config file
        /// </summary>
        /// <returns>bool which is true if the terminal is active, or false if not active in the config file</returns>
        public static string? GetEmbroiderySizeFromConfigFile()
        {
            string[] settings = File.ReadAllLines(_path);
            return settings[3].Split('=')[1];
        }

        /// <summary>
        /// Set the theme in the config file
        /// </summary>
        /// <param name="theme">theme you want to put in the config file</param>
        public static void SetThemeToConfigFile(string? theme)
        {
            // save the language in the file "settings.cfg" in the first row
            string[] settings = File.ReadAllLines(_path);
            settings[0] = $"Theme={theme}";
            File.WriteAllLines(_path, settings);
        }

        /// <summary>
        /// Set the language in the config file
        /// </summary>
        /// <param name="language">language you want to put in the config file</param>
        public static void SetLanguageToConfigFile(string? language)
        {
            string[] settings = File.ReadAllLines(_path);
            settings[1] = $"Language={language}";
            File.WriteAllLines(_path, settings);
        }

        /// <summary>
        /// Set the terminal in the config file
        /// </summary>
        /// <param name="terminal">bool you want to put in the config file (true if terminal is activated)</param>
        public static void SetTerminalToConfigFile(bool terminal)
        {
            string[] settings = File.ReadAllLines(_path);
            settings[2] = $"Terminal={terminal}";
            File.WriteAllLines(_path, settings);
        }

        /// <summary>
        /// Set the embroidery size in the config file
        /// </summary>
        /// <param name="embroiderySize">string you want to put in the config file</param>
        public static void SetEmbroiderySizeToConfigFile(string embroiderySize)
        {
            string[] settings = File.ReadAllLines(_path);
            settings[3] = $"EmbroiderySize={embroiderySize}";
            File.WriteAllLines(_path, settings);
        }

        /// <summary>
        /// Set the theme from the settings
        /// </summary>
        /// <param name="theme">theme you want to apply in the app</param>
        public static void SetThemeFromSettings(string? theme)
        {
            switch (theme)
            {
                case "Light":
                    Theme.Apply(ThemeType.Light, BackgroundType.None);
                    break;
                case "Dark":
                    Theme.Apply(ThemeType.Dark, BackgroundType.None);
                    break;
                default:
                    // Get current theme from Windows 11
                    SystemThemeType sysTheme = Theme.GetSystemTheme();
                    // Apply it
                    Theme.Apply(
                        sysTheme == SystemThemeType.Light
                            ? ThemeType.Light
                            : ThemeType.Dark, BackgroundType.None);
                    break;
            }
        }
    }
}
