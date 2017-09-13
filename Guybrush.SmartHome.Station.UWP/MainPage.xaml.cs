using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Modules.Standard;
using Guybrush.SmartHome.Station.UWP.Code;
using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Guybrush.SmartHome.Station.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StationViewModel ViewModel;

        ITurnOnOffModule light = new Light() { Name = "Light" };
        ITurnOnOffModule blinds = new Blinds() { Name = "Blinds" };
        ITurnOnOffModule air = new AirConditioner() { Name = "Air Conditioner" };

        IReaderModule term = new Termomethre() { Name = "Termomethre" };
        IReaderModule humi = new HumiditySensor() { Name = "Humidity Sensor" };
        IReaderModule ligsens = new LightSensor() { Name = "Light Sensor" };
        IDisplayModule disp = new Display();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += Devices_Loaded;
            LaunchStation();

        }

        private void Devices_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new StationViewModel();
            ViewModel.AddModule(light);
            ViewModel.AddModule(blinds);
            ViewModel.AddModule(air);

            ViewModel.ligsens = ligsens;
            ViewModel.term = term;
            ViewModel.air = air;
            Bindings.Update();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            ShutdownStation();
        }

        internal Station Station { get; private set; }
        private void LaunchStation()
        {
            //SmartHome code
            Task.Run(async () =>
            {
                Station = new Station();
                await Station.Initialize();

                Station.RegisterTurnOnOffDevice("Guybrush Inc", "Light", "1", light.Id.ToString(), "Guybrush Light", light);
                Station.RegisterTurnOnOffDevice("Guybrush Inc", "Air Conditioner", "1", air.Id.ToString(), "Guybrush air conditioner", air);
                Station.RegisterTurnOnOffDevice("Guybrush Inc", "Blinds", "1", blinds.Id.ToString(), "Guybrush blinds", blinds);

                Station.RegisterReadingDevice("Guybrush Inc", "Light Intensity", "1", ligsens.Id.ToString(), "Guybrush light intensity sensor", ligsens);
                Station.RegisterReadingDevice("Guybrush Inc", "Temperature", "1", term.Id.ToString(), "Guybrush termomether", term);
                Station.RegisterReadingDevice("Guybrush Inc", "Humidity", "1", humi.Id.ToString(), "Guybrush humidity sensor", humi);

                Station.RegisterDisplayDevice("Display", "Guybrush Inc", "Display", "1", disp.Id.ToString(), "Guybrush display device", disp);


            }).Wait();

            var delay = Task.Run(async () =>
            {
                //bool blind2Added = false;
                //var blinds = new Blinds() { Name = "Blinds 2" };
                bool reading2Added = false;
                var lightSens = new LightSensor() { Name = "Light Intensity 2" };


                while (true)
                {
                    await Task.Delay(15000);

                    //if (blind2Added)
                    //{
                    //    Station.UnregisterTurnOnOffDevice("Blinds 2", blinds.Id);
                    //    blind2Added = false;
                    //}
                    //else
                    //{
                    //    Station.RegisterTurnOnOffDevice("Guybrush Inc", "Blinds 2", "1", blinds.Id.ToString(), "Guybrush blinds 2", blinds);
                    //    blind2Added = true;
                    //}

                    //if (reading2Added)
                    //{
                    //    Station.UnregisterReadingDevice("Light Intensity 2", lightSens.Id);
                    //    reading2Added = false;
                    //}
                    //else
                    //{
                    //    Station.RegisterReadingDevice("Guybrush Inc", "Light Intensity", "1", lightSens.Id.ToString(), "Guybrush light intensity sensor", lightSens);
                    //    reading2Added = true;
                    //}

                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                  () =>
                  {

                      int val = term.Value;
                      val = humi.Value;
                      val = lightSens.Value;

                      //if (term.Value == 15)
                      //    term.Value = 0;
                      //else
                      //    term.Value = 15;

                      if (disp.Text == "Chupacabra")
                          disp.Text = "Zlo";
                      else disp.Text = "Chupacabra";
                  });
                }
            });
        }

        private void ShutdownStation()
        {
            Task.Run(async () =>
            {
                if (Station != null)
                    await Station.Shutdown();
            }).Wait();
        }
    }
}
