using AllJoyn.Dsb;
using BridgeRT;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Station.Core.AllJoyn.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.AllJoyn.Devices
{
    public class ReadingsDevice : AdapterDevice
    {

        private IList<ReaderInterface> _readers;
        private AdapterBusObject _busObject;
        public ReadingsDevice()
            : base("Readings", "Guybrush Inc", "Readings", "1", Guid.NewGuid().ToString(), "Guybrush readings collector device.")
        {
            _busObject = new AdapterBusObject("Readings");
            _readers = new List<ReaderInterface>();
            BusObjects.Add(_busObject);

        }

        public void RegisterReader(string readingTitle, string unit, string annotationKey, string annotationDescription, IReaderModule module)
        {
            var oldiface = _busObject.Interfaces.FirstOrDefault(x => x.Name == "com.guybrush.station.readings." + readingTitle.ToLower().Replace(' ', '_'));
            if (oldiface == null)
            {
                ReaderInterface reader = new ReaderInterface(readingTitle, unit, annotationKey, annotationDescription, module);
                _busObject.Interfaces.Add(reader.Interface);
                _readers.Add(reader);
                CreateEmitSignalChangedSignal();
                reader.ValueChanged += Reader_ValueChanged;
            }
        }

        internal void UnregisterReader(string readingTitle)
        {
            IAdapterInterface iface = _busObject.Interfaces.FirstOrDefault(x => x.Name == "com.guybrush.station.readings." + readingTitle.ToLower().Replace(' ', '_'));
            if (iface != null)
            {
                _busObject.Interfaces.Remove(iface);
                var reader = _readers.First(x => x.Name == readingTitle);
                _readers.Remove(reader);
                CreateEmitSignalChangedSignal();
            }
        }

        private void Reader_ValueChanged(object sender, int value)
        {
            UpdateValue(((ReaderInterface)sender).Name, value);
        }

        internal void UpdateValue(string readingTitle, int value)
        {
            var reader = _readers.FirstOrDefault(x => x.Name == readingTitle);
            if (reader != null)
            {
                var attr = reader.UpdateValue(value);
                if (attr != null)
                    SignalChangeOfAttributeValue(reader.Interface, attr);
            }
        }


    }
}
