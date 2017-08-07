﻿using Guybrush.SmartHome.Modules.Standard;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Guybrush.SmartHome.Station.UWP
{

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        #region App code
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
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();

                LaunchStation();


            }
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

            ShutdownStation();

        }

        #endregion


        Station _station;
        private void LaunchStation()
        {
            //SmartHome code
            Task.Run(async () =>
            {
                _station = new Station();
                await _station.Initialize();

                _station.RegisterDeviceTurnOnOffDevice("Light", "Guybrush Inc", "Light", "1", Guid.NewGuid().ToString(), "Guybrush Light", new Light());
                _station.RegisterDeviceTurnOnOffDevice("Air Conditioner", "Guybrush Inc", "Air Conditioner", "1", Guid.NewGuid().ToString(), "Guybrush air conditioner", new AirConditioner());
                _station.RegisterDeviceTurnOnOffDevice("Blinds", "Guybrush Inc", "Blinds", "1", Guid.NewGuid().ToString(), "Guybrush blinds", new Blinds());

                _station.RegisterReadingDevice("Light Intensity", "Lux", "Light intensity reading", "Current light intensity value in Lux", new LightSensor());
                _station.RegisterReadingDevice("Temperature", "C", "Temperature reading", "Current temperature value in Celcious", new Termomethre());
                _station.RegisterReadingDevice("Humidity", "%", "Humidity reading", "Current humidity", new HumiditySensor());

                _station.RegisterDisplayDevice("Display", "Display device", "Display device last message", new Display());

                _station.Start();
            }).Wait();


        }

        private void ShutdownStation()
        {
            Task.Run(async () =>
            {
                if (_station != null)
                    await _station.Shutdown();
            }).Wait();
        }

    }


}
