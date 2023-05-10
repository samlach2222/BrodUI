using BrodUI.Models;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace BrodUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : INavigableView<ViewModels.SettingsViewModel>
    {
        /// <summary>
        /// Getter of the view model
        /// </summary>
        public ViewModels.SettingsViewModel ViewModel
        {
            get;
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="viewModel">viewModel</param>
        public SettingsPage(ViewModels.SettingsViewModel viewModel)
        {
            ConfigManagement.ApplyLanguage();

            ViewModel = viewModel;

            InitializeComponent();
        }

        /// <summary>
        /// Function called when embroidery size is incremented
        /// </summary>
        /// <param name="sender">controller</param>
        /// <param name="e">routed event args</param>
        private void EmbroiderySize_OnIncremented_OnIncremented(object sender, RoutedEventArgs e)
        {
            string? es = ViewModel.CurEmbroiderySize;
            if (es == null) return;
            int embroiderySize = int.Parse(es);
            embroiderySize++;
            ViewModel.CurEmbroiderySize = embroiderySize.ToString();
        }

        /// <summary>
        /// Function called when embroidery size is decremented
        /// </summary>
        /// <param name="sender">controller</param>
        /// <param name="e">routed event args</param>
        private void EmbroiderySize_OnDecremented(object sender, RoutedEventArgs e)
        {
            string? es = ViewModel.CurEmbroiderySize;
            if (es == null) return;
            int embroiderySize = int.Parse(es);
            if (embroiderySize > 0)
            {
                embroiderySize--;
            }
            ViewModel.CurEmbroiderySize = embroiderySize.ToString();
        }

        /// <summary>
        /// Function called when embroidery size text is changed
        /// </summary>
        /// <param name="sender">controller</param>
        /// <param name="e">text changed event args</param>
        private void EmbroiderySize_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string? es = ViewModel.CurEmbroiderySize;
            if (es == null) return;
            int embroiderySize = int.Parse(es);
            if (embroiderySize < 0)
            {
                embroiderySize = 0;
            }
            ViewModel.CurEmbroiderySize = embroiderySize.ToString();
        }
    }
}