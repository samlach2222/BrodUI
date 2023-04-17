using System;
using System.IO;
using System.Linq;

namespace BrodUI.Models
{
    /// <summary>
    /// Class to manage the log file
    /// </summary>
    public static class LogManagement
    {
        /// <summary>
        /// Path of the log file
        /// </summary>
        private static string? _logPath;

        /// <summary>
        /// Getter and setter of the log file path
        /// </summary>
        public static string? LogPath
        {
            get => _logPath;
            set => _logPath = value;
        }

        /// <summary>
        /// Create the log file if it doesn't exist
        /// </summary>
        public static void CreateLogFileIfNotExists()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // create the folder "BrodUI" in AppData if it doesn't exist
            if (!Directory.Exists(appData + "\\BrodUI"))
            {
                Directory.CreateDirectory(appData + "\\BrodUI");
            }
            // path combine with the file name
            LogPath = System.IO.Path.Combine(appData + "\\BrodUI", "terminal.log");
            if (File.Exists(LogPath)) return;
            // Create the file
            FileStream newFile = File.Create(LogPath);
            newFile.Close();
        }

        /// <summary>
        /// Write a text line in the log file and in the terminal
        /// </summary>
        /// <param name="text">text to write in the log file and the terminal</param>
        public static void WriteToLog(string text)
        {
            Console.WriteLine(text);
            if (LogPath == null) return;
            using StreamWriter sw = File.AppendText(LogPath);
            sw.WriteLine(text);
        }

        /// <summary>
        /// Clear the log file
        /// </summary>
        public static void ClearLog()
        {
            // Delete the file
            if (LogPath != null)
            {
                File.Delete(LogPath);
            }
        }

        /// <summary>
        /// Write all the logs in the terminal
        /// </summary>
        public static void WriteAllLogsInTerminal()
        {
            if (LogPath == null) return;
            string[] lines = File.ReadAllLines(LogPath);
            string text = lines.Aggregate("", (current, line) => current + (line + Environment.NewLine));
            Console.WriteLine(text);
        }
    }
}
