using AllJoyn.Dsb;
using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.AllJoyn.Interfaces
{
    public class DisplayInterface
    {
        public string Name { get; private set; }
        public IDisplayModule Module { get; private set; }
        public AdapterInterface Interface { get; set; }
        public DisplayInterface(string displayDeviceName, string annotationKey, string annotationDescription, IDisplayModule module)
        {
            Name = displayDeviceName;
            Module = module;
            Interface = new AdapterInterface("com.guybrush.station.displays." + displayDeviceName.ToLower().Replace(' ', '_'));

            var _attr = new AdapterAttribute("Value", Module.Text, (o) =>
                {
                    string newValue = (string)o;
                    return AllJoynStatusCode.Ok;
                });
            _attr.Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ;
            _attr.Annotations.Add(annotationKey, annotationDescription);
            _attr.COVBehavior = BridgeRT.SignalBehavior.Always;

            Interface.Properties.Add(_attr);
            Module.ValueChanged += Module_ValueChanged;
        }

        private void Module_ValueChanged(object sender, string text)
        {
            ValueChanged(this, text);
        }
        internal event DisplayEventArgs ValueChanged;

        internal AdapterAttribute UpdateValue(string value)
        {
            var attr = Interface.Properties.FirstOrDefault(x => x.Value.Name == "Value") as AdapterAttribute;
            if (attr != null)
            {
                if (attr.Value.Data != (object)value)
                {
                    attr.Value.Data = value;
                }
                return attr;
            }
            return null;
        }

    }
}
