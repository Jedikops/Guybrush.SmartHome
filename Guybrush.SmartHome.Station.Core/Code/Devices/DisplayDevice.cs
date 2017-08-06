using AllJoyn.Dsb;

namespace Guybrush.SmartHome.Station.Core.Code.Devices
{
    public class DisplayDevice
    {

        public AdapterInterface Interface { get; set; }
        public DisplayDevice(string displayDeviceName, string annotationKey, string annotationDescription)
        {
            Interface = new AdapterInterface("com.guybrush.station.display." + displayDeviceName.ToLower().Replace(' ', '_'));

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

    }
}
