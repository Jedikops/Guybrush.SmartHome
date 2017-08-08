using Guybrush.SmartHome.Modules.Standard;
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

                Station.RegisterTurnOnOffDevice("Light", "Guybrush Inc", "Light", "1", light.Id.ToString(), "Guybrush Light", light);
                Station.RegisterTurnOnOffDevice("Air Conditioner", "Guybrush Inc", "Air Conditioner", "1", air.Id.ToString(), "Guybrush air conditioner", air);
                Station.RegisterTurnOnOffDevice("Blinds", "Guybrush Inc", "Blinds", "1", blinds.Id.ToString(), "Guybrush blinds", blinds);

                Station.RegisterDisplayDevice("Display", "Guybrush Inc", "Display", "1", disp.Id.ToString(), "Guybrush display device", disp);

                Station.RegisterReadingDevice("Light Intensity", "Lux", "Guybrush Inc", "Light Intensity", "1", ligsens.Id.ToString(), "Guybrush light intensity sensor", ligsens);
                Station.RegisterReadingDevice("Temperature", "C", "Guybrush Inc", "Temperature", "1", term.Id.ToString(), "Guybrush termomether", term);
                Station.RegisterReadingDevice("Humidity", "%", "Guybrush Inc", "Humidity", "1", humi.Id.ToString(), "Guybrush humidity sensor", humi);




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
                        Station.RegisterReadingDevice("Light Intensity", "Lux", "Guybrush Inc", "Light Intensity", "1", lightSens.Id.ToString(), "Guybrush light intensity sensor", lightSens);
                        reading2Added = true;
                    }

                    light.Status = !light.Status;
                    ligsens.Value += 15;

                    if (disp.Text == "Chupacabra")
                        disp.Text = "Zlo";
                    else disp.Text = "Chupacabra";

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
