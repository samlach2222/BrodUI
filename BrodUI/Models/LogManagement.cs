using System;
using System.IO;

namespace BrodUI.Models
{
    public static class LogManagement
    {
        private static string _logPath;

        public static string LogPath
        {
            get { return _logPath; }
            set
            {
                _logPath = value;
            }
        }

        public static void CreateLogFileIfNotExists()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // create the folder "BrodUI" in AppData if it doesn't exist
            if (!Directory.Exists(appData + "\\BrodUI"))
            {
                Directory.CreateDirectory(appData + "\\BrodUI");
            }
            // path combine with the file name
            LogPath = System.IO.Path.Combine(appData + "\\BrodUI", "terminal.log");
            if (!File.Exists(LogPath))
            {
                // Create the file
                FileStream newFile = File.Create(LogPath);
                newFile.Close();
            }
        }

        public static void WriteToLog(string text)
        {
            Console.WriteLine(text);
            using (StreamWriter sw = File.AppendText(LogPath))
            {
                sw.WriteLine(text);
            }
        }

        public static void ClearLog()
        {
            // Delete the file
            File.Delete(LogPath);
        }

        public static string WriteAllLogsInTerminal()
        {
            string[] lines = File.ReadAllLines(LogPath);
            string text = "";
            foreach (string line in lines)
            {
                text += line + Environment.NewLine;
            }
            return text;
        }
    }
}
