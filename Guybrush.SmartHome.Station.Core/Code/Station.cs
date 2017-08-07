using AllJoyn.Dsb;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Station.Core.AllJoyn.Devices;
using Guybrush.SmartHome.Station.Core.Enums;
using Guybrush.SmartHome.Station.Core.Helpers;
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

        public StationStatus Status { get; private set; }

        public Station()
        {
            _devices = new List<TurnOnOffDevice>();
            Status = StationStatus.Down;
        }

        public async Task Initialize()
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

            _devices = new List<TurnOnOffDevice>();
            _readings = new ReadingsDevice();
            _displays = new DisplaysDevice();

        }

        public void RegisterDeviceTurnOnOffDevice(string name, string VendorName, string model, string version,
            string serialNumber, string description, ITurnOnOffModule module)
        {
            _devices.Add(new TurnOnOffDevice(name, VendorName, model, version, serialNumber, description, module));
        }


        public void RegisterReadingDevice(string readingTitle, string unit, string annotationKey, string annotationDescription, IReaderModule module)
        {

            _readings.RegisterReader(readingTitle, unit, annotationKey, annotationDescription, module);

        }

        public void RegisterDisplayDevice(string displayDeviceName, string annotationKey, string annotationDescription, IDisplayModule module)
        {

            _displays.RegisterDisplay(displayDeviceName, annotationKey, annotationDescription, module);

        }

        public void Start()
        {
            foreach (var device in _devices)
            {
                AllJoynDsbServiceManager.Current.AddDevice(device);
            }

            AllJoynDsbServiceManager.Current.AddDevice(_readings);
            AllJoynDsbServiceManager.Current.AddDevice(_displays);

            Status = StationStatus.Active;
        }

        public async Task Shutdown()
        {
            await AllJoynDsbServiceManager.Current.ShutdownAsync();
            Status = StationStatus.Down;

        }
        //private void Test()
        //{
        //    var delay = Task.Run(async () =>
        //    {
        //        TurnOnOffDevice deva = null;
        //        var rand = new Random();
        //        while (true)
        //        {
        //            await Task.Delay(15000);
        //
        //            if (deva == null)
        //            {
        //                deva = new TurnOnOffDevice("Blinds 2", "Guybrush Inc", "Blinds 2", "1", Guid.NewGuid().ToString(), "Guybrush blinds 2", new Blinds());
        //                AllJoynDsbServiceManager.Current.AddDevice(deva);
        //            }
        //            else
        //            {
        //                _devices.Remove(deva);
        //                AllJoynDsbServiceManager.Current.RemoveDevice(deva);
        //                deva = null;
        //
        //            }
        //
        //        }
        //    });
        //}

    }
}
