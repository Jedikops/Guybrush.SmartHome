using AllJoyn.Dsb;
using BridgeRT;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Station.Devices
{
    public class TurnOnOffDevice : AdapterDevice
    {

        private bool _currentValue;
        private AdapterInterface Interface { get; set; }

        public bool CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                UpdateValue(_currentValue);
            }
        }

        public TurnOnOffDevice(string Name, string VendorName, string Model, string Version, string SerialNumber, string Description)
            : base(Name, VendorName, Model, Version, SerialNumber, Description)
        {

            AdapterBusObject busObject = new AdapterBusObject(Name);

            Interface = new AdapterInterface("com.guybrush.devices.OnOffControl");
            var attr = new AdapterAttribute("Status", false) { COVBehavior = BridgeRT.SignalBehavior.Always, Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ };
            attr.Annotations.Add("com.guybrush.devices.OnOffControl.Status", "The device status");
            Interface.Properties.Add(attr);

            List<IAdapterValue> inputs = new List<IAdapterValue>(1);
            inputs.Add(new AdapterValue("TargetStatus", false));

            AdapterMethod method = new AdapterMethod("Switch", "Switches devices on or off.", ChangeStatus, inputs);
            Interface.Methods.Add(method);

            busObject.Interfaces.Add(Interface);
            BusObjects.Add(busObject);
            CreateEmitSignalChangedSignal();

            _currentValue = false;

        }

        private void ChangeStatus(AdapterMethod sender, IReadOnlyDictionary<string, object> inputParams, IDictionary<string, object> outputParams)
        {
            bool targetStatus = (bool)inputParams["TargetStatus"];

            CurrentValue = targetStatus;


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
