using BrodUI.Models;
using Wpf.Ui.Common.Interfaces;

namespace BrodUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportPage : INavigableView<ViewModels.ExportViewModel>
    {
        public ViewModels.ExportViewModel ViewModel
        {
            get;
        }

        public ExportPage(ViewModels.ExportViewModel viewModel)
        {
            ConfigManagement.SetLanguage();

            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
