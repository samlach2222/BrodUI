using System.Windows;
using Wpf.Ui.Controls;

namespace BrodUI.Services
{
    /// <summary>
    /// Redefinition of the WPF UI message box to have only one button
    /// </summary>
    public static class WpfMessageBox
    {
        /// <summary>
        /// Method to show a message box with only one button
        /// </summary>
        /// <param name="title">title of the message box</param>
        /// <param name="content">content of the message box</param>
        public static void Show(string title, string content)
        {
            Wpf.Ui.Controls.MessageBox mb = new()
            {
                Title = title,
                Content = content
            };

            // add button in footer
            Button btn = new()
            {
                Content = "Ok",
                HorizontalAlignment = HorizontalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            btn.Click += (sender, args) => { mb.Close(); };
            mb.Footer = btn;
            mb.Show();
        }
    }
}