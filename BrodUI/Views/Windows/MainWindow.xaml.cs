using System;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BrodUI.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INavigationWindow
    {
        /// <summary>
        /// Getter for the view model.
        /// </summary>
        public ViewModels.MainWindowViewModel ViewModel
        {
            get;
        }

        /// <summary>
        /// Creates new instance of the <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="viewModel">viewModel of the window</param>
        /// <param name="pageService">pageService of the window</param>
        /// <param name="navigationService">navigationService of the window</param>
        public MainWindow(ViewModels.MainWindowViewModel viewModel, IPageService pageService, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            SetPageService(pageService);

            navigationService.SetNavigationControl(RootNavigation);
        }

        #region INavigationWindow methods

        /// <summary>
        /// Method to get the frame of the window.
        /// </summary>
        /// <returns>the frame</returns>
        public Frame GetFrame()
            => RootFrame;

        /// <summary>
        /// Method to get the navigation of the window.
        /// </summary>
        /// <returns>Navigation of the window</returns>
        public INavigation GetNavigation()
            => RootNavigation;

        /// <summary>
        /// Method to navigate to a page.
        /// </summary>
        /// <param name="pageType">Page you want to navigate</param>
        /// <returns>true if you well navigated to</returns>
        public bool Navigate(Type pageType)
            => RootNavigation.Navigate(pageType);

        /// <summary>
        /// Method to set the page service.
        /// </summary>
        /// <param name="pageService">pageService you want to set</param>
        public void SetPageService(IPageService pageService)
            => RootNavigation.PageService = pageService;

        /// <summary>
        /// Method to show the window.
        /// </summary>
        public void ShowWindow()
            => Show();

        /// <summary>
        /// Method to close the window.
        /// </summary>
        public void CloseWindow()
            => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }
    }
}