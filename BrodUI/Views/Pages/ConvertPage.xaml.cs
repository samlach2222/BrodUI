using BrodUI.Models;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using TextBox = Wpf.Ui.Controls.TextBox;

namespace BrodUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ConvertPage : INavigableView<ViewModels.ConvertViewModel> // TODO : ADD SCROLLBAR TO THE PAGE
    {
        public ViewModels.ConvertViewModel ViewModel
        {
            get;
        }

        public ConvertPage(ViewModels.ConvertViewModel viewModel)
        {
            ConfigManagement.ApplyLanguage();

            ViewModel = viewModel;

            InitializeComponent();
        }

        /// <summary>
        /// Method called when the user clicks the Down arrow on the Y Resolution TextBox
        /// It decrements the Y resolution by 1
        /// </summary>
        /// <param name="sender">TextBox for the Y resolution</param>
        /// <param name="e">Routed event args</param>
        private void YRes_OnDecremented(object sender, RoutedEventArgs e)
        {
            ViewModel.ImageHeight -= 1;
        }

        /// <summary>
        /// Method called when the user clicks the Up arrow on the Y Resolution TextBox
        /// It increments the Y resolution by 1
        /// </summary>
        /// <param name="sender">TextBox for the Y resolution</param>
        /// <param name="e">Routed event args</param>
        private void YRes_OnIncremented(object sender, RoutedEventArgs e)
        {
            ViewModel.ImageHeight += 1;
        }

        /// <summary>
        /// Method called when the user clicks the Up arrow on the X Resolution TextBox
        /// It increments the X resolution by 1
        /// </summary>
        /// <param name="sender">TextBox for the Y resolution</param>
        /// <param name="e">Routed event args</param>
        private void XRes_OnIncremented(object sender, RoutedEventArgs e)
        {
            ViewModel.ImageWidth += 1;
        }

        /// <summary>
        /// Method called when the user clicks the Down arrow on the X Resolution TextBox
        /// It will decrease the value of the X resolution by 1
        /// </summary>
        /// <param name="sender">TextBox for the Y resolution</param>
        /// <param name="e">Routed event args</param>
        private void XRes_OnDecremented(object sender, RoutedEventArgs e)
        {
            ViewModel.ImageHeight -= 1;
        }

        /// <summary>
        /// Method called when the user type anything in the Y Resolution TextBox
        /// It will update the Y resolution value only if the value is greater than 10
        /// </summary>
        /// <param name="sender">TextBox for the Y resolution</param>
        /// <param name="e">Routed event args</param>
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

        /// <summary>
        /// Method called when the user type anything in the X Resolution TextBox
        /// It will update the X resolution value only if the value is greater than 10
        /// </summary>
        /// <param name="sender">TextBox for the X resolution</param>
        /// <param name="e">Routed event args</param>
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
