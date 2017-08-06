using AllJoyn.Dsb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.Code.Devices
{
    public class ReadingsDevice : AdapterDevice
    {
        private IList<ReaderDevice> _readers;
        private IList<DisplayDevice> _displays;
        private AdapterBusObject _busObject;
        public ReadingsDevice()
            : base("Readings", "Guybrush Inc", "Readings", "1", Guid.NewGuid().ToString(), "Guybrush readings device.")
        {
            _busObject = new AdapterBusObject("Readings");
            _readers = new List<ReaderDevice>();
            _displays = new List<DisplayDevice>();
            BusObjects.Add(_busObject);

        }

        public ReaderDevice RegisterReader(string readingTitle, string unit, string annotationKey, string annotationDescription)
        {
            ReaderDevice reader = new ReaderDevice(readingTitle, unit, annotationKey, annotationDescription);
            _busObject.Interfaces.Add(reader.Interface);
            _readers.Add(reader);
            CreateEmitSignalChangedSignal();
            return reader;

        }

        public DisplayDevice RegisterDisplay(string displayDeviceName, string annotationKey, string annotationDescription)
        {
            var display = new DisplayDevice(displayDeviceName, annotationKey, annotationDescription);
            _busObject.Interfaces.Add(display.Interface);
            _displays.Add(display);
            CreateEmitSignalChangedSignal();
            return display;

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
