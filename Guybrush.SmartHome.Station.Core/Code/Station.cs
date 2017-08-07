using AllJoyn.Dsb;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Station.Core.AllJoyn.Devices;
using Guybrush.SmartHome.Station.Core.Enums;
using Guybrush.SmartHome.Station.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Station
{


    public class Station
    {
        public IList<ITurnOnOffModule> Devices { get; private set; }
        public IList<IReaderModule> Readings { get; private set; }
        public IList<IDisplayModule> Displays { get; private set; }

        SmarthomeAdapter _homeDevice;
        IList<TurnOnOffDevice> _devices;
        ReadingsDevice _readings;
        DisplaysDevice _displays;

        public StationStatus Status { get; private set; }

        public Station()
        {
            _devices = new List<TurnOnOffDevice>();
            Status = StationStatus.Stopped;
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

            Devices = new List<ITurnOnOffModule>();
            Readings = new List<IReaderModule>();
            Displays = new List<IDisplayModule>();

            Status = StationStatus.Running;
        }

        public void RegisterTurnOnOffDevice(string name, string VendorName, string model, string version,
            string serialNumber, string description, ITurnOnOffModule module)
        {
            var device = new TurnOnOffDevice(name, VendorName, model, version, serialNumber, description, module);

            AllJoynDsbServiceManager.Current.AddDevice(device);
            _devices.Add(device);
            Devices.Add(module);
        }

        public void UnregisterTurnOnOffDevice(string name, Guid id)
        {
            var device = _devices.FirstOrDefault(x => x.Name == name);
            if (device != null)
            {
                AllJoynDsbServiceManager.Current.RemoveDevice(device);
                _devices.Remove(device);
                Devices.Remove(Devices.First(x => x.Id == id));
            }
        }

        public void RegisterReadingDevice(string readingTitle, string unit, string annotationKey, string annotationDescription, IReaderModule module)
        {
            AllJoynDsbServiceManager.Current.RemoveDevice(_readings);
            _readings.RegisterReader(readingTitle, unit, annotationKey, annotationDescription, module);
            Readings.Add(module);
            AllJoynDsbServiceManager.Current.AddDevice(_readings);
        }


        public void UnregisterReadingDevice(string name, Guid id)
        {
            AllJoynDsbServiceManager.Current.RemoveDevice(_readings);
            _readings.UnregisterReader(name);
            var reading = Readings.FirstOrDefault(x => x.Id == id);
            if (reading != null)
                Readings.Remove(reading);

            AllJoynDsbServiceManager.Current.AddDevice(_readings);
        }

        public void RegisterDisplayDevice(string displayDeviceName, string annotationKey, string annotationDescription, IDisplayModule module)
        {

            AllJoynDsbServiceManager.Current.RemoveDevice(_displays);
            _displays.RegisterDisplay(displayDeviceName, annotationKey, annotationDescription, module);
            Displays.Add(module);
            AllJoynDsbServiceManager.Current.AddDevice(_displays);
        }

        public async Task Shutdown()
        {

            await AllJoynDsbServiceManager.Current.ShutdownAsync();
            Status = StationStatus.Stopped;

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
