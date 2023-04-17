using BrodUI.Models;
using BrodUI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace BrodUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // CONSOLE DISPLAY VARIABLES
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private const uint WmClose = 0x10;
        private const int SwHide = 0;
        private static bool _showConsole = false;
        private static IntPtr _consoleWindow;

        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost Host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                // App Host
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddScoped<INavigationWindow, Views.Windows.MainWindow>();
                services.AddScoped<ViewModels.MainWindowViewModel>();

                // Views and ViewModels
                services.AddScoped<Views.Pages.TutorialPage>();
                services.AddScoped<ViewModels.TutorialViewModel>();
                services.AddScoped<Views.Pages.ConvertPage>();
                services.AddScoped<ViewModels.ConvertViewModel>();
                services.AddScoped<Views.Pages.ExportPage>();
                services.AddScoped<ViewModels.ExportViewModel>();
                services.AddScoped<Views.Pages.SettingsPage>();
                services.AddScoped<ViewModels.SettingsViewModel>();

                // Configuration
                services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
            }).Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T? GetService<T>()
            where T : class
        {
            return Host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            ShowConsole(); // Show console if needed
            LogManagement.CreateLogFileIfNotExists(); // Create log file if it doesn't exist
            Console.WriteLine(Assets.Languages.Resource.Terminal_ImportLog);
            LogManagement.WriteAllLogsInTerminal(); // Write all logs in console
            Console.WriteLine(Assets.Languages.Resource.Terminal_ImportLogOk);
            Console.WriteLine("-------------------------------------------------------");

            await Host.StartAsync();

            // Create settings.cfg file if it doesn't exist with lines for each setting
            ConfigManagement.CreateConfigFileIfNotExists();
            // Load settings.cfg file
            ConfigManagement.SetTheme();
            // Set language
            ConfigManagement.SetLanguage();
        }

        /// <summary>
        /// Create console if needed (to display infos to power users).
        /// </summary>
        public static void ShowConsole()
        {
            ConfigManagement.CreateConfigFileIfNotExists();
            _showConsole = ConfigManagement.GetTerminalFromConfigFile();

            // Because "Windows Terminal" have tabs, hiding the console directly doesn't work
            // Instead we set the console to the foreground, get the foreground window and hide it
            SetForegroundWindow(GetConsoleWindow());
            _consoleWindow = GetForegroundWindow(); // consoleWindow is needed when exiting the app
            if (!_showConsole)
            {
                // Hide the console window, but don't close it (closing it would close the app)
                ShowWindow(_consoleWindow, SwHide);
            }
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            // Close the console window
            SendMessage(_consoleWindow, WmClose, IntPtr.Zero, IntPtr.Zero);

            await Host.StopAsync();

            Host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}