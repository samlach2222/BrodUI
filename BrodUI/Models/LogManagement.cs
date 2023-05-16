using System;
using System.IO;
using System.Windows;
using Wpf.Ui.TaskBar;

namespace BrodUI.Models
{
    /// <summary>
    /// Class to manage the log file
    /// </summary>
    public static class LogManagement
    {
        /// <summary>
        /// Path to the log file
        /// </summary>
        public static string LogPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BrodUI", "terminal.log");

        /// <summary>
        /// Last known percentage of the progression
        /// </summary>
        private static sbyte lastPercentage = -1;

        /// <summary>
        /// Create the log file if it doesn't exist
        /// </summary>
        public static void CreateLogFileIfNotExists()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appData, "BrodUI");
            // create the folder "BrodUI" in AppData if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

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
            string text = string.Join(Environment.NewLine, lines);
            Console.WriteLine(text);
        }

        /// <summary>
        /// Update the progression in the taskbar
        /// </summary>
        /// <param name="value">Value to calculate a percentage of progression</param>
        /// <param name="max">Maximum value of value to calculate a percentage of progression</param>
        public static void UpdateProgression(int value, int max)
        {
            int percentage = value * 100 / max;
            if (percentage < lastPercentage)  // A new progression is happening
            {
                lastPercentage = -1;
            }
            if (percentage > lastPercentage)
            {
                lastPercentage = (sbyte)percentage;

                // Stop taskbar progress if the progression is finished
                if (percentage == 100)
                {
                    TaskBarProgress.SetValue(Application.Current.MainWindow, TaskBarProgressState.None, percentage, 100);
                    return;
                }

                // Update taskbar progress
                TaskBarProgress.SetValue(Application.Current.MainWindow, TaskBarProgressState.Normal, percentage, 100);
            }
        }

        /// <summary>
        /// Update the taskbar progression state
        /// </summary>
        /// <param name="taskBarProgressState">Taskbar progression state</param>
        public static void UpdateProgression(TaskBarProgressState taskBarProgressState)
        {
            if (Application.Current == null) return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                TaskBarProgress.SetState(Application.Current.MainWindow, taskBarProgressState);
            });
        }
    }
}
