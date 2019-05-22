using CarouselView.UWP;
using JWChinese.UWP.Renderers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace JWChinese.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
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
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            // launch app with a consistent window size for screenshotting
            ApplicationView.PreferredLaunchViewSize = new Size { Width = 1364, Height = 735 };
            //ApplicationView.PreferredLaunchViewSize = new Size { Width = 640, Height = 1024 };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 350));
#endif

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;

                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = Helper.GetColorFromHexa("#2f64a8");
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.BackgroundColor = Helper.GetColorFromHexa("#2f64a8");
                    titleBar.ForegroundColor = Colors.White;
                }
            }

            // If we have a phone contract, hide the status bar
            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                var statusBar = StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                List<Assembly> rendererAssemblies = new List<Assembly>();
                rendererAssemblies.Add(typeof(CarouselViewRenderer).GetTypeInfo().Assembly);
                rendererAssemblies.Add(typeof(ExtendedButtonRender).GetTypeInfo().Assembly);
                rendererAssemblies.Add(typeof(GridViewRenderer).GetTypeInfo().Assembly);
                rendererAssemblies.Add(typeof(JWWebViewRenderer).GetTypeInfo().Assembly);
                rendererAssemblies.Add(typeof(MasterDetailPageRender).GetTypeInfo().Assembly);
                rendererAssemblies.Add(typeof(SQLiteService).GetTypeInfo().Assembly);
                rendererAssemblies.Add(typeof(BaseUrl).GetTypeInfo().Assembly);
                rendererAssemblies.Add(typeof(Plugin.Settings.CrossSettings).GetTypeInfo().Assembly);
                rendererAssemblies.Add(typeof(Plugin.Settings.SettingsImplementation).GetTypeInfo().Assembly);

                Xamarin.Forms.Forms.Init(e, rendererAssemblies);

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
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