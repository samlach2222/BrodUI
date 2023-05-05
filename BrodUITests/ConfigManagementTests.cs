using System.Drawing;
using System.Globalization;
using BrodUI.Helpers;
using BrodUI.Models;
using Wpf.Ui.Appearance;
using Wpf.Ui.Markup;
using Xunit;

namespace BrodUITests
{
    public class ConfigManagementTests
    {
        private void ResetConfigFile()
        {
            // Check if config file exists
            string? appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(appData + "\\BrodUI", "settings.cfg");
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
            string expected = "Theme=System\nLanguage="+ language + "\nTerminal=" + terminal + "\nEmbroiderySize=15";

            // Actual
            string? appData = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
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
            string? appData = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
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

        //[Fact]
        //public void SetThemeTest()
        //{
        //    // Expected
        //    ConfigManagement.DeleteConfigFile();
        //    ConfigManagement.CreateConfigFileIfNotExists();
        //    const string expected = "Light";
        //    ConfigManagement.SetThemeToConfigFile(expected);
        //    // Actual
        //    ConfigManagement.SetTheme();
        //    // Get current WPFUi theme
        //    string? actual = ThemeManager.CurrentTheme.Name;
        //    // Assert
        //    //Assert.Equal(expected, actual);
        //}
    }
}