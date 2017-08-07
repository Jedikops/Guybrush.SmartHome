using AllJoyn.Dsb;
using Guybrush.SmartHome.Station.Core.Code.Devices;
using Guybrush.SmartHome.Station.Core.Helpers;
using Guybrush.SmartHome.Station.Devices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Station
{
    public class Station
    {
        IList<TurnOnOffDevice> _devices;
        ReadingsDevice _readings;
        DisplaysDevice _displays;

        SmarthomeAdapter _homeDevice;

        public Station()
        {
            _devices = new List<TurnOnOffDevice>();

        }

        public async void Initialize()
        {
            var config = new BridgeConfiguration(StationHelper.GetDeviceID(), "com.guybrush")
            {
                ModelName = "Guybrush Bridge",
                DeviceName = "Station",
                ApplicationName = "Guybrush Station",
                Vendor = "Guybrush"

            };
            _homeDevice = new SmarthomeAdapter(config);
            await AllJoynDsbServiceManager.Current.StartAsync(_homeDevice);

            _readings = new ReadingsDevice();
            _readings.RegisterReader("Light Intensity", "Lux", "Light intensity reading", "Current light intensity value in Lux");
            _readings.RegisterReader("Temperature", "C", "Temperature reading", "Current temperature value in Celcious");
            _readings.RegisterReader("Humidity", "%", "Humidity reading", "Current humidity");
            AllJoynDsbServiceManager.Current.AddDevice(_readings);

            _displays = new DisplaysDevice();
            _displays.RegisterDisplay("Display", "Display device", "Display device last message");
            AllJoynDsbServiceManager.Current.AddDevice(_displays);

            AllJoynDsbServiceManager.Current.AddDevice(new TurnOnOffDevice("Light", "Guybrush Inc", "Light", "1", Guid.NewGuid().ToString(), "Guybrush Light"));
            AllJoynDsbServiceManager.Current.AddDevice(new TurnOnOffDevice("Air Conditioner", "Guybrush Inc", "Air Conditioner", "1", Guid.NewGuid().ToString(), "Guybrush air conditioner"));
            AllJoynDsbServiceManager.Current.AddDevice(new TurnOnOffDevice("Blinds", "Guybrush Inc", "Blinds", "1", Guid.NewGuid().ToString(), "Guybrush blinds"));

            Test();

        }


        private void Test()
        {
            var delay = Task.Run(async () =>
            {
                var read = 0;
                TurnOnOffDevice deva = null;
                var rand = new Random();
                while (true)
                {
                    await Task.Delay(15000);

                    if (deva == null)
                    {
                        deva = new TurnOnOffDevice("Blinds 2", "Guybrush Inc", "Blinds 2", "1", Guid.NewGuid().ToString(), "Guybrush blinds 2");
                        _devices.Add(deva);
                        AllJoynDsbServiceManager.Current.AddDevice(deva);
                    }
                    else
                    {
                        _devices.Remove(deva);
                        AllJoynDsbServiceManager.Current.RemoveDevice(deva);
                        deva = null;

                    }

                    var dev = _devices[rand.Next(_devices.Count - 1)];
                    dev.CurrentValue = !dev.CurrentValue;

                    read += 15;

                    _readings.UpdateValue("Light Intensity", read);

                }
            });
        }

    }
}
