using AllJoyn.Dsb;
using BridgeRT;
using Guybrush.SmartHome.Modules.Interfaces;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.AllJoyn.Devices
{
    public class DisplayDevice : AdapterDevice
    {
        internal IDisplayModule Module { get; private set; }
        //private bool _currentValue;
        internal AdapterInterface Interface { get; set; }

        public DisplayDevice(string name, string vendorName, string model, string version,
           string serialNumber, string description, IDisplayModule module)
            : base(name, vendorName, model, version, serialNumber, description)
        {
            Module = module;
            AdapterBusObject busObject = new AdapterBusObject("Guybrush");

            Interface = new AdapterInterface("com.guybrush.display");

            var attr = new AdapterAttribute("Text", module.Text) { COVBehavior = SignalBehavior.Always, Access = E_ACCESS_TYPE.ACCESS_READ };
            attr.Annotations.Add("com.guybrush.devices.display.text", "The device status");
            Interface.Properties.Add(attr);

            busObject.Interfaces.Add(Interface);
            BusObjects.Add(busObject);
            CreateEmitSignalChangedSignal();

            Module.TextChanged += Module_TextChanged;
        }

        private void Module_TextChanged(object sender, string text)
        {
            var attr = Interface.Properties.FirstOrDefault(a => a.Value.Name == "Text");
            if (attr.Value.Data != (object)text)
            {
                attr.Value.Data = text;
                SignalChangeOfAttributeValue(Interface, attr);
            };
        }
    }
}
