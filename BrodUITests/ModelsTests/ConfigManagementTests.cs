using System.Drawing;
using System.Globalization;
using BrodUI.Helpers;
using BrodUI.Models;
using Microsoft.Win32;
using Wpf.Ui.Appearance;
using Wpf.Ui.Markup;
using Xunit;
using static BrodUI.Models.ImageManagement;

namespace BrodUITests.ModelsTests
{
    public class ConfigManagementTests
    {
        private void ResetConfigFile()
        {
            // Check if config file exists
            string? appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(appData + "\\BrodUI", "settings.cfg");
            ConfigManagement.CreateConfigFileIfNotExists();
            if (File.Exists(path))
            {
                ConfigManagement.DeleteConfigFile();

            }
            ConfigManagement.CreateConfigFileIfNotExists();
        }


        [Fact]
        public void CreateConfigFileIfNotExistsTest()
        {
            // Expected
            string language = ConfigManagement.GetSystemLanguageOrDefault();
            bool terminal = false;
#if DEBUG
            terminal = true;
#endif
            string expected = "Theme=System\nLanguage=" + language + "\nTerminal=" + terminal + "\nEmbroiderySize=15";

            // Actual
            string? appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(appData + "\\BrodUI", "settings.cfg");
            File.Delete(path);

            ConfigManagement.CreateConfigFileIfNotExists();

            string actual = File.ReadAllText(path);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSystemLanguageOrDefaultTest()
        {
            // Expected
            // Get the system language
            string expected = CultureInfo.CurrentUICulture.NativeName;
            expected = expected[..(expected.IndexOf('(') - 1)].ToLower();
            // Actual
            string actual = ConfigManagement.GetSystemLanguageOrDefault().ToLower();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeleteFileTest()
        {
            // -----------------------------------
            // Create file and verify if it exists
            // -----------------------------------
            ConfigManagement.CreateConfigFileIfNotExists();
            string? appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(appData + "\\BrodUI", "settings.cfg");
            FileInfo file = new(path);
            Assert.True(file.Exists);

            // -----------------------------------
            // Delete file and verify if not exists
            // -----------------------------------
            ConfigManagement.DeleteConfigFile();
            file = new FileInfo(path);
            Assert.False(file.Exists);
        }

        [Fact]
        public void GetThemeFromConfigFileTest()
        {
            // Expected
            ResetConfigFile();
            const string expected = "System";
            // Actual
            string? actual = ConfigManagement.GetThemeFromConfigFile();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetThemeToConfigFileTest()
        {
            // Expected
            ResetConfigFile();
            const string expected = "Light";
            // Actual
            ConfigManagement.SetThemeToConfigFile(expected);
            string? actual = ConfigManagement.GetThemeFromConfigFile();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetThemeTest()
        {
            // TODO : Test this method (hard to implement because we have to verify the theme of the app)
            throw new NotImplementedException();
        }

        [Fact]
        public void SetThemeFromSettingsTest()
        {
            // TODO : Test this method (hard to implement because we have to verify the theme of the app)
            throw new NotImplementedException();
        }

        [Fact]
        public void GetLanguageFromConfigFileTest()
        {
            // Expected
            ResetConfigFile();
            string expected = CultureInfo.CurrentUICulture.NativeName;
            expected = expected[..(expected.IndexOf('(') - 1)].ToLower();
            // Actual
            string actual = ConfigManagement.GetLanguageFromConfigFile()!.ToLower();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetLanguageToConfigFileTest()
        {
            // Expected
            ResetConfigFile();
            const string expected = "Spanish";
            // Actual
            ConfigManagement.SetLanguageToConfigFile(expected);
            string? actual = ConfigManagement.GetLanguageFromConfigFile();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetLanguageTest()
        {
            // Expected
            ResetConfigFile();
            const string expected = "en";
            ConfigManagement.SetLanguageToConfigFile("English");
            // Actual
            ConfigManagement.SetLanguage();
            // get actual language of the app
            string actual = CultureInfo.CurrentUICulture.Name;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTerminalFromConfigFileTest()
        { 
            // Expected
            ResetConfigFile();
            bool expected = false;
#if DEBUG
            expected = true;
#endif
            // Actual
            bool actual = ConfigManagement.GetTerminalFromConfigFile();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetTerminalToConfigFileTest()
        {
            // Expected
            ResetConfigFile();
            const bool expected = true;
            // Actual
            ConfigManagement.SetTerminalToConfigFile(expected);
            bool actual = ConfigManagement.GetTerminalFromConfigFile();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetEmbroiderySizeFromConfigFileTest()
        {
            // Expected
            ResetConfigFile();
            const int expected = 15;
            // Actual
            int actual = int.Parse(ConfigManagement.GetEmbroiderySizeFromConfigFile() ?? string.Empty);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetEmbroiderySizeToConfigFileTest()
        {
            // Expected
            ResetConfigFile();
            const int expected = 20;
            // Actual
            ConfigManagement.SetEmbroiderySizeToConfigFile(expected.ToString());
            int actual = int.Parse(ConfigManagement.GetEmbroiderySizeFromConfigFile() ?? string.Empty);
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}