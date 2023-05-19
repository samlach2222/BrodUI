using System;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;

namespace BrodUI.Services
{
    /// <summary>
    /// Service that provides pages for navigation.
    /// </summary>
    public class PageService : IPageService
    {
        /// <summary>
        /// Service which provides the instances of pages.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates new instance and attaches the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The ServiceProvider</param>
        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Method to get a page.
        /// </summary>
        /// <typeparam name="T">Page you want</typeparam>
        /// <returns>the aim page</returns>
        /// <exception cref="InvalidOperationException">return an exception if the page is not a WPF control.</exception>
        public T? GetPage<T>() where T : class
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException("The page should be a WPF control.");

            return (T?)_serviceProvider.GetService(typeof(T));
        }

        /// <summary>
        /// Method to get a page.
        /// </summary>
        /// <param name="pageType">Page you want to get</param>
        /// <returns>the aim page</returns>
        /// <exception cref="InvalidOperationException">return an exception if the page is not a WPF control.</exception>
        public FrameworkElement? GetPage(Type pageType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
                throw new InvalidOperationException("The page should be a WPF control.");

            return _serviceProvider.GetService(pageType) as FrameworkElement;
        }
    }
}
