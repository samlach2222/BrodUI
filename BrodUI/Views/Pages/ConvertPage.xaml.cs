using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BrodUI.Models;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using TextBox = Wpf.Ui.Controls.TextBox;

namespace BrodUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ConvertPage : INavigableView<ViewModels.ConvertViewModel>
    {
        public ViewModels.ConvertViewModel ViewModel
        {
            get;
        }

        public ConvertPage(ViewModels.ConvertViewModel viewModel)
        {
            ConfigManagement.SetLanguage();

            ViewModel = viewModel;

            InitializeComponent();
        }

        private void YRes_OnDecremented(object sender, RoutedEventArgs e)
        {
            ViewModel.ImageHeight -= 1;
        }

        private void YRes_OnIncremented(object sender, RoutedEventArgs e)
        {
            ViewModel.ImageHeight += 1;
        }

        private void XRes_OnIncremented(object sender, RoutedEventArgs e)
        {
            ViewModel.ImageWidth += 1;
        }

        private void XRes_OnDecremented(object sender, RoutedEventArgs e)
        {
            ViewModel.ImageHeight -= 1;
        }

        private void YRes_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "")
            {
                textBox.Text = "0";
            }
            else if (textBox.Text.Length > 1 && int.Parse(textBox.Text) > 10)
            {
                ViewModel.ImageHeight = int.Parse(textBox.Text);
            }
        }

        private void XRes_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "")
            {
                textBox.Text = "0";
            }
            else if (textBox.Text.Length > 1 && int.Parse(textBox.Text) > 10)
            {
                ViewModel.ImageWidth = int.Parse(textBox.Text);
            }
        }
    }
}
