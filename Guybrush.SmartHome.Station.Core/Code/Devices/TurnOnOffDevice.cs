using AllJoyn.Dsb;
using BridgeRT;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Station.Devices
{
    public class TurnOnOffDevice : AdapterDevice
    {

        private bool _currentValue;
        AdapterInterface _iface;
        AdapterAttribute _attr;
        AdapterSignal _signal;

        public bool CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                UpdateValue(_currentValue);
            }
        }

        public TurnOnOffDevice(string Name, string VendorName, string Model, string Version, string SerialNumber, string Description, string path)
            : base(Name, VendorName, Model, Version, SerialNumber, Description)
        {

            AdapterBusObject abo = new AdapterBusObject(Name);

            _iface = new AdapterInterface(path + ".OnOffControl");
            _attr = new AdapterAttribute("Status", false) { COVBehavior = BridgeRT.SignalBehavior.Always, Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ };
            _attr.Annotations.Add(path + ".OnOffControl.Status", "The device status");
            _iface.Properties.Add(_attr);
            List<IAdapterValue> inputs = new List<IAdapterValue>(1);
            inputs.Add(new AdapterValue("TargetStatus", false));
            AdapterMethod method = new AdapterMethod("Switch", "Switches devices on or off.", switchDevice, inputs);
            _iface.Methods.Add(method);

            _signal = new AdapterSignal("StatusChanged");
            _signal.Params.Add(_attr.Value);
            _iface.Signals.Add(_signal);

            abo.Interfaces.Add(_iface);
            BusObjects.Add(abo);
            CreateEmitSignalChangedSignal();
            _currentValue = false;

        }

        private void switchDevice(AdapterMethod sender, IReadOnlyDictionary<string, object> inputParams, IDictionary<string, object> outputParams)
        {
            bool targetStatus = (bool)inputParams["TargetStatus"];

            CurrentValue = targetStatus;


        }

        public void UpdateValue(bool value)
        {
            var attr = _iface.Properties.Where(a => a.Value.Name == "Status").First();


            if (attr.Value.Data != (object)value)
            {
                attr.Value.Data = value;
                SignalChangeOfAttributeValue(_iface, attr);
                NotifySignalListener(_iface.Signals[0]);
            }
        }

        public AdapterSignal GetSignal()
        {
            return _signal;
        }

    }
}
