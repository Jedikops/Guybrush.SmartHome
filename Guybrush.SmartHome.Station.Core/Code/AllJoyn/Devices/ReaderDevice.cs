using AllJoyn.Dsb;
using BridgeRT;
using Guybrush.SmartHome.Modules.Interfaces;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.Code.AllJoyn.Devices
{
    public class ReaderDevice : AdapterDevice
    {
        internal IReaderModule Module { get; private set; }
        //private bool _currentValue;
        internal AdapterInterface Interface { get; set; }

        public ReaderDevice(string name, string unit, string vendorName, string model, string version,
            string serialNumber, string description, IReaderModule module)
            : base(name, vendorName, model, version, serialNumber, description)
        {
            Module = module;
            AdapterBusObject busObject = new AdapterBusObject(name);

            Interface = new AdapterInterface("com.guybrush.devices.reader");

            var attrValue = new AdapterAttribute("Value", module.Value) { COVBehavior = SignalBehavior.Always, Access = E_ACCESS_TYPE.ACCESS_READ };
            attrValue.Annotations.Add("com.guybrush.devices.reader.value", "The device value");
            Interface.Properties.Add(attrValue);

            var attrUnit = new AdapterAttribute("Unit", module.Unit) { COVBehavior = SignalBehavior.Always, Access = E_ACCESS_TYPE.ACCESS_READ };
            attrUnit.Annotations.Add("com.guybrush.devices.reader.unit", "The device unit");
            Interface.Properties.Add(attrUnit);

            busObject.Interfaces.Add(Interface);
            BusObjects.Add(busObject);
            CreateEmitSignalChangedSignal();

            Module.ValueChanged += Module_ValueChanged;
            Module.UnitChanged += Module_UnitChanged;

        }

        private void Module_UnitChanged(object sender, string value)
        {
            var attr = Interface.Properties.FirstOrDefault(a => a.Value.Name == "Unit");
            if (attr != null && attr.Value.Data != (object)value)
            {
                attr.Value.Data = value;
                SignalChangeOfAttributeValue(Interface, attr);
            }
        }

        private void Module_ValueChanged(object sender, int value)
        {
            var attr = Interface.Properties.FirstOrDefault(a => a.Value.Name == "Value");
            if (attr != null && attr.Value.Data != (object)value)
            {
                attr.Value.Data = value;
                SignalChangeOfAttributeValue(Interface, attr);
            }
        }
    }
}
