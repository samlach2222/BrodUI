using BrodUI.Models;
using Wpf.Ui.Common.Interfaces;

namespace BrodUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportPage : INavigableView<ViewModels.ExportViewModel>
    {
        /// <summary>
        /// Getter of the view model
        /// </summary>
        public ViewModels.ExportViewModel ViewModel
        {
            get;
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="viewModel">viewModel</param>
        public ExportPage(ViewModels.ExportViewModel viewModel)
        {
            ConfigManagement.SetLanguage();

            ViewModel = viewModel;

            InitializeComponent();

            ViewModel.GridImage = GridImage;
        }
    }
}
