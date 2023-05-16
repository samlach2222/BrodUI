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
        /// Path to the config file
        /// </summary>
        public static string ConfigPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BrodUI", "settings.cfg");

        /// <summary>
        /// Default values for the config file
        /// </summary>
        public static string[] DefaultSettings => new string[] {
            "Theme=System",
            "Language=" + GetSystemLanguageOrDefault(),
#if DEBUG
            "Terminal=" + true,
#else
            "Terminal=" + false,
#endif
            "EmbroiderySize=15",
            "KMeansClusters=5",
            "KMeansIterations=10",
            "ColorModel=RGB"
        };

        /// <summary>
        /// Create the config file if it doesn't exist with its default values
        /// </summary>
        public static void CreateConfigFileIfNotExists()
        {
            // get AppData Roaming folder in a string
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appData, "BrodUI");
            // create the folder "BrodUI" in AppData if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // if the file already exists, check if we have the correct number of lines
            if (File.Exists(ConfigPath) && File.ReadAllLines(ConfigPath).Length == DefaultSettings.Length) return;

            File.WriteAllLines(ConfigPath, DefaultSettings);
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
            File.Delete(ConfigPath);
        }

        /// <summary>
        /// Set the theme of the app from the config file. There is 3 themes :
        /// Light
        /// Dark
        /// System (Light or Dark depending on the system theme)
        /// </summary>
        public static void ApplyTheme()
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            // Set theme
            switch (settings[0].Split('=')[1])
            {
                case "System":
                    SystemThemeType theme = Theme.GetSystemTheme();
                    // Apply it
                    Theme.Apply(theme == SystemThemeType.Light ? ThemeType.Light : ThemeType.Dark, BackgroundType.None);
                    break;
                case "Light":
                    Theme.Apply(ThemeType.Light, BackgroundType.None);
                    break;
                case "Dark":
                    Theme.Apply(ThemeType.Dark, BackgroundType.None);
                    break;
            }
        }

        /// <summary>
        /// Set the language of the app from the config file. There is 2 languages for the moment :
        /// French
        /// English
        /// </summary>
        public static void ApplyLanguage()
        {
            string[] settings = File.ReadAllLines(ConfigPath);
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
        public static string GetThemeFromConfigFile()
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            return settings[0].Split('=')[1];
        }

        /// <summary>
        /// Get the language from the config file
        /// </summary>
        /// <returns>string with the name of the language in the config file</returns>
        public static string GetLanguageFromConfigFile()
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            return settings[1].Split('=')[1];
        }

        /// <summary>
        /// Get the terminal from the config file
        /// </summary>
        /// <returns>bool which is true if the terminal is active, or false if not active in the config file</returns>
        public static bool GetTerminalFromConfigFile()
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            return Convert.ToBoolean(settings[2].Split('=')[1]);
        }

        /// <summary>
        /// Get the embroidery point size from the config file
        /// </summary>
        /// <returns>embroidery size</returns>
        public static string GetEmbroiderySizeFromConfigFile()
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            return settings[3].Split('=')[1];
        }

        /// <summary>
        /// Get the number of clusters for KMeans from the config file
        /// </summary>
        /// <returns>number of clusters for KMeans</returns>
        public static int GetKMeansClustersFromConfigFile()
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            return Convert.ToInt32(settings[4].Split('=')[1]);
        }

        /// <summary>
        /// Get the number of iterations for KMeans from the config file
        /// </summary>
        /// <returns>number of iterations for KMeans</returns>
        public static int GetKMeansIterationsFromConfigFile()
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            return Convert.ToInt32(settings[5].Split('=')[1]);
        }

        /// <summary>
        /// Get the color model from the config file
        /// </summary>
        /// <returns>color model (RGB, HSL, etc.) in uppercase</returns>
        public static string GetColorModelFromConfigFile()
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            return (settings[6].Split('=')[1]).ToUpperInvariant(); // Convert to uppercase
        }

        /// <summary>
        /// Set the theme in the config file
        /// </summary>
        /// <param name="theme">theme you want to put in the config file</param>
        public static void SetThemeToConfigFile(string theme)
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            settings[0] = $"Theme={theme}";
            File.WriteAllLines(ConfigPath, settings);
        }

        /// <summary>
        /// Set the language in the config file
        /// </summary>
        /// <param name="language">language you want to put in the config file</param>
        public static void SetLanguageToConfigFile(string language)
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            settings[1] = $"Language={language}";
            File.WriteAllLines(ConfigPath, settings);
        }

        /// <summary>
        /// Set the terminal in the config file
        /// </summary>
        /// <param name="terminal">bool you want to put in the config file (true if terminal is activated)</param>
        public static void SetTerminalToConfigFile(bool terminal)
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            settings[2] = $"Terminal={terminal}";
            File.WriteAllLines(ConfigPath, settings);
        }

        /// <summary>
        /// Set the embroidery size in the config file
        /// </summary>
        /// <param name="embroiderySize">string you want to put in the config file</param>
        public static void SetEmbroiderySizeToConfigFile(string embroiderySize)
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            settings[3] = $"EmbroiderySize={embroiderySize}";
            File.WriteAllLines(ConfigPath, settings);
        }

        /// <summary>
        /// Set the number of clusters for KMeans in the config file
        /// </summary>
        /// <param name="kmeansClusters">number of clusters for KMeans</param>
        public static void SetKMeansClustersToConfigFile(int kmeansClusters)
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            settings[4] = $"KMeansClusters={kmeansClusters}";
            File.WriteAllLines(ConfigPath, settings);
        }

        /// <summary>
        /// Set the number of iterations for KMeans in the config file
        /// </summary>
        /// <param name="kmeansIterations">number of iterations for KMeans</param>
        public static void SetKMeansIterationsToConfigFile(int kmeansIterations)
        {
            string[] settings = File.ReadAllLines(ConfigPath);
            settings[5] = $"KMeansIterations={kmeansIterations}";
            File.WriteAllLines(ConfigPath, settings);
        }

        /// <summary>
        /// Set the color model in the config file
        /// </summary>
        /// <param name="colorModel">color model (RGB, HSL, etc.)</param>
        public static void SetColorModelToConfigFile(string colorModel)
        {
            colorModel = colorModel.ToUpperInvariant(); // Convert to uppercase
            string[] settings = File.ReadAllLines(ConfigPath);
            settings[6] = $"ColorModel={colorModel}";
            File.WriteAllLines(ConfigPath, settings);
        }
    }
}
