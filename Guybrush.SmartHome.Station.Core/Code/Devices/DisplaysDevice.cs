using AllJoyn.Dsb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.Code.Devices
{
    class DisplaysDevice : AdapterDevice
    {
        private IList<DisplayInterface> _displays;
        private AdapterBusObject _busObject;

        public DisplaysDevice()
            : base("Displays", "Guybrush Inc", "Displays", "1", Guid.NewGuid().ToString(), "Guybrush display collector device.")
        {
            _busObject = new AdapterBusObject("Readings");
            _displays = new List<DisplayInterface>();
            BusObjects.Add(_busObject);

        }

        public DisplayInterface RegisterDisplay(string displayDeviceName, string annotationKey, string annotationDescription)
        {
            var display = new DisplayInterface(displayDeviceName, annotationKey, annotationDescription);
            _busObject.Interfaces.Add(display.Interface);
            _displays.Add(display);
            CreateEmitSignalChangedSignal();
            return display;

        }

        public void UpdateValue(string displayTitle, string value)
        {
            var display = _displays.FirstOrDefault(x => x.Name == displayTitle);
            if (display != null)
            {
                var attr = display.UpdateValue(value);
                if (attr != null)
                    SignalChangeOfAttributeValue(display.Interface, attr);
            }
        }
    }
}
