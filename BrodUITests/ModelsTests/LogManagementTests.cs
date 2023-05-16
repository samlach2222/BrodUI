using BrodUI.Models;
using Xunit;

namespace BrodUITests.ModelsTests
{
    public class LogManagementTests
    {
        [Fact]
        public void CreateLogFileIfNotExistsTest()
        {
            // Delete log file in path if exists
            string? appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(appData, "BrodUI", "terminal.log");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            Assert.False(File.Exists(path));

            // Create log file
            LogManagement.CreateLogFileIfNotExists();

            // Assert
            Assert.True(File.Exists(path));
        }

        [Fact]
        public void WriteToLogTest()
        {
            // Delete log file in path if exists
            string? appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(appData, "BrodUI", "terminal.log");
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Create log file
            LogManagement.CreateLogFileIfNotExists();

            // Write to log
            const string expected = "test";
            LogManagement.WriteToLog("test");
            // Read log file
            string actual = File.ReadAllText(path);

            // Assert
            Assert.True(File.Exists(path));
            Assert.Equal(expected + "\r\n", actual);
        }

        [Fact]
        public void ClearLogTest()
        {
            // Delete log file in path if exists
            string? appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(appData, "BrodUI", "terminal.log");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            // Create log file
            LogManagement.CreateLogFileIfNotExists();
            // Write to log
            LogManagement.WriteToLog("test");
            // Clear log
            LogManagement.ClearLog();
            // Assert
            Assert.False(File.Exists(path));
        }

        [Fact]
        public void WriteAllLogsInTerminalTest()
        {
            // clear log file
            LogManagement.CreateLogFileIfNotExists();
            LogManagement.ClearLog();
            LogManagement.CreateLogFileIfNotExists();

            // Write to log
            LogManagement.WriteToLog("test1");
            LogManagement.WriteToLog("test2");

            // Create a StringWriter to capture console output
            using StringWriter sw = new();
            Console.SetOut(sw);

            // Act
            LogManagement.WriteAllLogsInTerminal();

            // Assert
            const string expectedOutput = "test1\r\ntest2\r\n";
            Assert.Equal(expectedOutput, sw.ToString());
        }
    }
}