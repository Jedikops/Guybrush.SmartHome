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

        public TurnOnOffDevice(string Name, string VendorName, string Model, string Version,
            string SerialNumber, string Description, ITurnOnOffModule module)
            : base(Name, VendorName, Model, Version, SerialNumber, Description)
        {
            Module = module;
            AdapterBusObject busObject = new AdapterBusObject(Name);

            Interface = new AdapterInterface("com.guybrush.devices.OnOffControl");
            var attr = new AdapterAttribute("Status", false) { COVBehavior = SignalBehavior.Always, Access = E_ACCESS_TYPE.ACCESS_READ };
            attr.Annotations.Add("com.guybrush.devices.OnOffControl.Status", "The device status");
            Interface.Properties.Add(attr);

            List<IAdapterValue> inputs = new List<IAdapterValue>(1);
            inputs.Add(new AdapterValue("TargetStatus", false));

            AdapterMethod method = new AdapterMethod("Switch", "Switches devices on or off.", ChangeStatus, inputs);
            Interface.Methods.Add(method);

            busObject.Interfaces.Add(Interface);
            BusObjects.Add(busObject);
            CreateEmitSignalChangedSignal();

            Module.Status = false;
            Module.ValueChanged += Module_ValueChanged;
        }

        private void Module_ValueChanged(object sender, bool value)
        {
            UpdateValue(value);
        }

        private void ChangeStatus(AdapterMethod sender, IReadOnlyDictionary<string, object> inputParams, IDictionary<string, object> outputParams)
        {
            bool targetStatus = (bool)inputParams["TargetStatus"];

            Module.Status = targetStatus;
        }

        public void UpdateValue(bool value)
        {
            var attr = Interface.Properties.Where(a => a.Value.Name == "Status").First();


            if (attr.Value.Data != (object)value)
            {
                attr.Value.Data = value;
                SignalChangeOfAttributeValue(Interface, attr);
            }
        }
    }
}
