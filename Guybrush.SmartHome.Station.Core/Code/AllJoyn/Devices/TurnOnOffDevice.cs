using AllJoyn.Dsb;
using BridgeRT;
using Guybrush.SmartHome.Modules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.AllJoyn.Devices
{
    public class TurnOnOffDevice : AdapterDevice
    {
        internal ITurnOnOffModule Module { get; private set; }
        //private bool _currentValue;
        internal AdapterInterface Interface { get; set; }

        public TurnOnOffDevice(string name, string vendorName, string model, string version,
            string serialNumber, string description, ITurnOnOffModule module)
            : base(name, vendorName, model, version, serialNumber, description)
        {
            Module = module;
            AdapterBusObject busObject = new AdapterBusObject(name);

            Interface = new AdapterInterface("com.guybrush.devices.onoffcontrol");
            var attr = new AdapterAttribute("Status", module.Status) { COVBehavior = SignalBehavior.Always, Access = E_ACCESS_TYPE.ACCESS_READ };
            attr.Annotations.Add("com.guybrush.devices.onoffcontrol.status", "The device status");
            Interface.Properties.Add(attr);

            List<IAdapterValue> inputs = new List<IAdapterValue>(1);
            inputs.Add(new AdapterValue("TargetStatus", false));

            AdapterMethod method = new AdapterMethod("Switch", "Switches devices on or off.", ChangeStatus, inputs);
            Interface.Methods.Add(method);

            busObject.Interfaces.Add(Interface);
            BusObjects.Add(busObject);
            CreateEmitSignalChangedSignal();

            Module.ValueChanged += Module_ValueChanged;
        }

        private void Module_ValueChanged(object sender, bool value)
        {
            var attr = Interface.Properties.FirstOrDefault(a => a.Value.Name == "Status");
            if (attr.Value.Data != (object)value)
            {
                attr.Value.Data = value;
                SignalChangeOfAttributeValue(Interface, attr);
            };
        }

        private void ChangeStatus(AdapterMethod sender, IReadOnlyDictionary<string, object> inputParams, IDictionary<string, object> outputParams)
        {
            bool targetStatus = (bool)inputParams["TargetStatus"];
            Module.Status = targetStatus;
        }
    }
}
