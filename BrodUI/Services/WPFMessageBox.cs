using System.Windows;
using Wpf.Ui.Controls;

namespace BrodUI.Services;

/// <summary>
/// Redefinition of the WPF UI message box to have only one button
/// </summary>
public static class WPFMessageBox
{
    public static void Show(string title, string content)
    {
        Wpf.Ui.Controls.MessageBox mb = new();

        mb.Title = title;
        mb.Content = content;

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