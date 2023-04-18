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
        public ViewModels.SettingsViewModel ViewModel
        {
            get;
        }

        public SettingsPage(ViewModels.SettingsViewModel viewModel)
        {
            ConfigManagement.SetLanguage();

            ViewModel = viewModel;

            InitializeComponent();
        }

        private void EmbroiderySize_OnIncremented_OnIncremented(object sender, RoutedEventArgs e)
        {
            string es = ViewModel.CurEmbroiderySize;
            int embroiderySize = int.Parse(es);
            embroiderySize++;
            ViewModel.CurEmbroiderySize = embroiderySize.ToString();
        }

        private void EmbroiderySize_OnDecremented(object sender, RoutedEventArgs e)
        {
            string es = ViewModel.CurEmbroiderySize;
            int embroiderySize = int.Parse(es);
            if (embroiderySize > 0)
            {
                embroiderySize--;
            }
            ViewModel.CurEmbroiderySize = embroiderySize.ToString();
        }

        private void EmbroiderySize_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string es = ViewModel.CurEmbroiderySize;
            int embroiderySize = int.Parse(es);
            if (embroiderySize < 0)
            {
                embroiderySize = 0;
            }
            ViewModel.CurEmbroiderySize = embroiderySize.ToString();
        }
    }
}