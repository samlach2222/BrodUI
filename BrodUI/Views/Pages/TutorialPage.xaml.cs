using Wpf.Ui.Common.Interfaces;

namespace BrodUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for TutorialPage.xaml
    /// </summary>
    public partial class TutorialPage : INavigableView<ViewModels.TutorialViewModel>
    {
        /// <summary>
        /// Getter of the view model
        /// </summary>
        public ViewModels.TutorialViewModel ViewModel
        {
            get;
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="viewModel">viewModel</param>
        public TutorialPage(ViewModels.TutorialViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}