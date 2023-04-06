using BrodUI.Models;
using Wpf.Ui.Common.Interfaces;

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
    }
}
