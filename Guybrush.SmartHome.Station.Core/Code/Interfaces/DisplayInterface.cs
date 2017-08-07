using AllJoyn.Dsb;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.Code.Devices
{
    public class DisplayInterface
    {
        public string Name { get; private set; }
        public AdapterInterface Interface { get; set; }
        public DisplayInterface(string displayDeviceName, string annotationKey, string annotationDescription)
        {
            Name = displayDeviceName;
            Interface = new AdapterInterface("com.guybrush.station.displays." + displayDeviceName.ToLower().Replace(' ', '_'));

            var _attr = new AdapterAttribute("Value", "Hello!", (o) =>
                {
                    string newValue = (string)o;
                    return AllJoynStatusCode.Ok;
                });
            _attr.Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ;
            _attr.Annotations.Add(annotationKey, annotationDescription);
            _attr.COVBehavior = BridgeRT.SignalBehavior.Always;

            Interface.Properties.Add(_attr);

        }
        public AdapterAttribute UpdateValue(string value)
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
