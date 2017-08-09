using AllJoyn.Dsb;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Shared.Models;
using Guybrush.SmartHome.Station.Core.AllJoyn.Devices;
using Guybrush.SmartHome.Station.Core.Code.AllJoyn.Devices;
using Guybrush.SmartHome.Station.Core.Enums;
using Guybrush.SmartHome.Station.Core.Helpers;
using Guybrush.SmartHome.Station.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Station
{


    public class Station
    {
        public IList<ITurnOnOffModule> Devices { get; private set; }
        public IList<IReaderModule> Readers { get; private set; }
        public IList<IDisplayModule> Displays { get; private set; }

        SmarthomeAdapter _homeDevice;

        ConditionManager _conditionManager;
        List<Condition> _conditions;
        IList<TurnOnOffDevice> _devices;
        IList<ReaderDevice> _readers;
        IList<DisplayDevice> _displays;

        public StationStatus Status { get; private set; }

        public Station()
        {
            _devices = new List<TurnOnOffDevice>();
            Status = StationStatus.Stopped;
        }

        public async Task Initialize()
        {
            _devices = new List<TurnOnOffDevice>();
            _readers = new List<ReaderDevice>();
            _displays = new List<DisplayDevice>();

            Devices = new List<ITurnOnOffModule>();
            Readers = new List<IReaderModule>();
            Displays = new List<IDisplayModule>();
            _conditions = new List<Condition>();
            _conditionManager = new ConditionManager(_conditions, Devices);

            var config = new BridgeConfiguration(StationHelper.GetDeviceID(), "com.guybrush")
            {
                ModelName = "Guybrush Bridge",
                DeviceName = "Guybrush Smart Home",
                ApplicationName = "Guybrush Station",
                Vendor = "Guybrush"

            };
            _homeDevice = new SmarthomeAdapter(config, _conditionManager);
            await AllJoynDsbServiceManager.Current.StartAsync(_homeDevice);

            Status = StationStatus.Running;
        }

        public void RegisterTurnOnOffDevice(string vendorName, string model, string version,
            string serialNumber, string description, ITurnOnOffModule module)
        {
            var device = new TurnOnOffDevice(vendorName, model, version, serialNumber, description, module);

            AllJoynDsbServiceManager.Current.AddDevice(device);
            _devices.Add(device);
            module.ValueChanged += _conditionManager.TurnOnOffModule_ValueChanged;
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

        public void RegisterReadingDevice(string vendorName, string model, string version,
            string serialNumber, string description, IReaderModule module)
        {
            var device = new ReaderDevice(vendorName, model, version, serialNumber, description, module);
            AllJoynDsbServiceManager.Current.AddDevice(device);
            _readers.Add(device);
            module.ValueChanged += _conditionManager.ReaderModule_ValueChanged;
            Readers.Add(module);
        }



        public void UnregisterReadingDevice(string name, Guid id)
        {
            var device = _readers.FirstOrDefault(x => x.Name == name);
            if (device != null)
            {
                AllJoynDsbServiceManager.Current.RemoveDevice(device);
                _readers.Remove(device);
                Readers.Remove(Readers.First(x => x.Id == id));
            }
        }

        public void RegisterDisplayDevice(string name, string vendorName, string model, string version,
            string serialNumber, string description, IDisplayModule module)
        {
            var device = new DisplayDevice(name, vendorName, model, version, serialNumber, description, module);
            AllJoynDsbServiceManager.Current.AddDevice(device);
            _displays.Add(device);
            Displays.Add(module);

        }

        public void UnregisterDisplayDevice(string name, Guid id)
        {
            var device = _displays.FirstOrDefault(x => x.Name == name);
            if (device != null)
            {
                AllJoynDsbServiceManager.Current.RemoveDevice(device);
                _displays.Remove(device);
                Displays.Remove(Displays.First(x => x.Id == id));
            }
        }

        public async Task Shutdown()
        {

            await AllJoynDsbServiceManager.Current.ShutdownAsync();
            Status = StationStatus.Stopped;

        }
    }
}
