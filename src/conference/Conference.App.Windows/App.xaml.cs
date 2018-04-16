namespace Conference.App.Windows
{
    using System.Threading.Tasks;

    using global::Windows.ApplicationModel;

    using global::Windows.ApplicationModel.Activation;
    using global::Windows.Foundation.Metadata;
    using global::Windows.UI;
    using global::Windows.UI.ViewManagement;
    using global::Windows.UI.Xaml;

    using OpenMVVM.Core.PlatformServices.Navigation;
    using OpenMVVM.WebView.Windows;
    using global::Windows.UI.Core;

    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            WebViewPage currentContent = new WebViewPage(new ViewModelLocator());

            currentContent.AppReady += this.CurrentContentAppReady;

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {

                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Colors.Black;
                    statusBar.ForegroundColor = Colors.White;
                }
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += this.AppBackRequested;

            Window.Current.Content = currentContent;
            Window.Current.Activate();
        }

        private void CurrentContentAppReady(object sender, System.EventArgs e)
        {
            var navigationService = ViewModelLocator.InstanceFactory.GetInstance<INavigationService>();
            navigationService.NavigateTo("MainView");
        }

        private void AppBackRequested(object sender, BackRequestedEventArgs e)
        {
            var navigationService = ViewModelLocator.InstanceFactory.GetInstance<INavigationService>();
            navigationService.GoBack();
            e.Handled = true;
        }


        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}

