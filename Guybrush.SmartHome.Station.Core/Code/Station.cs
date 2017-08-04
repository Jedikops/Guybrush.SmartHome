using AllJoyn.Dsb;
using Guybrush.SmartHome.Station.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Station
{
    public class Station
    {
        ICollection<AdapterDevice> _devices;
        SmarthomeAdapter _homeDevice;
        public Station()
        {
            _devices = new List<AdapterDevice>();
        }

        public async void Initialize()
        {
            var config = new BridgeConfiguration(GetDeviceID(), "com.guybrush")
            {
                ModelName = "Guybrush Bridge",
                DeviceName = "Station",
                ApplicationName = "Guybrush Station",
                Vendor = "Guybrush"

            };
            _homeDevice = new SmarthomeAdapter(config);
            await AllJoynDsbServiceManager.Current.StartAsync(_homeDevice);

            _devices.Add(new TurnOnOffDevice("Light", "Guybrush Inc", "Light", "1", Guid.NewGuid().ToString(), "Guybrush Light"));
            _devices.Add(new TurnOnOffDevice("Air Conditioner", "Guybrush Inc", "Air Conditioner", "1", Guid.NewGuid().ToString(), "Guybrush air conditioner"));
            _devices.Add(new TurnOnOffDevice("Blinds", "Guybrush Inc", "Blinds", "1", Guid.NewGuid().ToString(), "Guybrush blinds"));
            foreach (var device in _devices)
            {

                AllJoynDsbServiceManager.Current.AddDevice(device);
            }

            var delay = Task.Run(async () =>
            {
                AdapterDevice device = null;
                while (true)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    await Task.Delay(15000);

                    if (device == null)
                    {
                        device = new TurnOnOffDevice("Blinds 2", "Guybrush Inc", "Blinds 2", "1", Guid.NewGuid().ToString(), "Guybrush blinds 2");
                        _devices.Add(device);
                        AllJoynDsbServiceManager.Current.AddDevice(device);
                    }
                    else
                    {
                        _devices.Remove(device);
                        AllJoynDsbServiceManager.Current.RemoveDevice(device);
                        device = null;

                    }

                    sw.Stop();


                }
                //return sw.ElapsedMilliseconds;
            });

        }

        private Guid GetDeviceID()
        {
            if (!Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("DSBDeviceId"))
            {
                Guid deviceId = Guid.NewGuid();
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["DSBDeviceId"] = deviceId;
                return deviceId;
            }
            return (Guid)Windows.Storage.ApplicationData.Current.LocalSettings.Values["DSBDeviceId"];
        }
    }
}
