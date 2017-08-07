using Guybrush.SmartHome.Modules.Standard;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Guybrush.SmartHome.Station.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            LaunchStation();

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

                Station.RegisterTurnOnOffDevice("Light", "Guybrush Inc", "Light", "1", Guid.NewGuid().ToString(), "Guybrush Light", new Light());
                Station.RegisterTurnOnOffDevice("Air Conditioner", "Guybrush Inc", "Air Conditioner", "1", Guid.NewGuid().ToString(), "Guybrush air conditioner", new AirConditioner());
                Station.RegisterTurnOnOffDevice("Blinds", "Guybrush Inc", "Blinds", "1", Guid.NewGuid().ToString(), "Guybrush blinds", new Blinds());

                Station.RegisterReadingDevice("Light Intensity", "Lux", "Light intensity reading", "Current light intensity value in Lux", new LightSensor());
                Station.RegisterReadingDevice("Temperature", "C", "Temperature reading", "Current temperature value in Celcious", new Termomethre());
                Station.RegisterReadingDevice("Humidity", "%", "Humidity reading", "Current humidity", new HumiditySensor());

                Station.RegisterDisplayDevice("Display", "Display device", "Display device last message", new Display());

            }).Wait();

            var delay = Task.Run(async () =>
            {
                bool blind2Added = false;
                var blinds = new Blinds();
                bool reading2Added = false;
                var lightSens = new LightSensor();


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
                    //    Station.RegisterTurnOnOffDevice("Blinds 2", "Guybrush Inc", "Blinds 2", "1", blinds.Id.ToString(), "Guybrush blinds 2", blinds);
                    //    blind2Added = true;
                    //}

                    if (reading2Added)
                    {
                        Station.UnregisterReadingDevice("Light Intensity 2", lightSens.Id);
                        reading2Added = false;
                    }
                    else
                    {
                        Station.RegisterReadingDevice("Light Intensity 2", "Lux", "Light intensity reading", "Current light intensity value in Lux", lightSens);
                        reading2Added = true;
                    }

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
