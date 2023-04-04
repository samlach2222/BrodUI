using Wpf.Ui.Common.Interfaces;

namespace BrodUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for TutorialPage.xaml
    /// </summary>
    public partial class TutorialPage : INavigableView<ViewModels.TutorialViewModel>
    {
        public ViewModels.TutorialViewModel ViewModel
        {
            get;
        }

        public TutorialPage(ViewModels.TutorialViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}