using AllJoyn.Dsb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.Code.Devices
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

        public ReaderInterface RegisterReader(string readingTitle, string unit, string annotationKey, string annotationDescription)
        {
            ReaderInterface reader = new ReaderInterface(readingTitle, unit, annotationKey, annotationDescription);
            _busObject.Interfaces.Add(reader.Interface);
            _readers.Add(reader);
            CreateEmitSignalChangedSignal();
            return reader;

        }

        public void UpdateValue(string readingTitle, int value)
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
