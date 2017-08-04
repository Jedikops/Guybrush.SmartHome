using AllJoyn.Dsb;
using Guybrush.SmartHome.Station.Devices;
using System;
using System.Collections.Generic;

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

            _devices.Add(new TurnOnOffDevice("Light", "Guybrush Inc", "Light", "1", Guid.NewGuid().ToString(), "Guybrush Light", "com.guybrush.devices.light"));
            _devices.Add(new TurnOnOffDevice("Air Conditioner", "Guybrush Inc", "Air Conditioner", "1", Guid.NewGuid().ToString(), "Guybrush air conditioner", "com.guybrush.devices.airconditioner"));
            _devices.Add(new TurnOnOffDevice("Blinds", "Guybrush Inc", "Blinds", "1", Guid.NewGuid().ToString(), "Guybrush blinds", "com.guybrush.devices.blinds"));
            foreach (var device in _devices)
            {

                AllJoynDsbServiceManager.Current.AddDevice(device);
            }

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
