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
        Light light = new Light();
        Blinds blinds = new Blinds();
        AirConditioner air = new AirConditioner();

        Termomethre term = new Termomethre();
        HumiditySensor humi = new HumiditySensor();
        LightSensor ligsens = new LightSensor();
        Display disp = new Display();

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

                Station.RegisterTurnOnOffDevice("Light", "Guybrush Inc", "Light", "1", Guid.NewGuid().ToString(), "Guybrush Light", light);
                Station.RegisterTurnOnOffDevice("Air Conditioner", "Guybrush Inc", "Air Conditioner", "1", Guid.NewGuid().ToString(), "Guybrush air conditioner", air);
                Station.RegisterTurnOnOffDevice("Blinds", "Guybrush Inc", "Blinds", "1", Guid.NewGuid().ToString(), "Guybrush blinds", blinds);

                Station.RegisterReadingDevice("Light Intensity", "Lux", "Light intensity reading", "Current light intensity value in Lux", ligsens);
                Station.RegisterReadingDevice("Temperature", "C", "Temperature reading", "Current temperature value in Celcious", term);
                Station.RegisterReadingDevice("Humidity", "%", "Humidity reading", "Current humidity", humi);

                Station.RegisterDisplayDevice("Display", "Display device", "Display device last message", disp);

            }).Wait();

            var delay = Task.Run(async () =>
            {
                bool blind2Added = false;
                var blinds = new Blinds();
                bool reading2Added = false;
                var lightSens = new LightSensor();


                while (true)
                {
                    await Task.Delay(5000);

                    if (blind2Added)
                    {
                        Station.UnregisterTurnOnOffDevice("Blinds 2", blinds.Id);
                        blind2Added = false;
                    }
                    else
                    {
                        Station.RegisterTurnOnOffDevice("Blinds 2", "Guybrush Inc", "Blinds 2", "1", blinds.Id.ToString(), "Guybrush blinds 2", blinds);
                        blind2Added = true;
                    }

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

                    light.Status = !light.Status;

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
