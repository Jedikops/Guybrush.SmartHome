using AllJoyn.Dsb;

namespace Guybrush.SmartHome.Station.Core.Code.Devices
{
    public class DisplayDevice
    {

        AdapterAttribute _attr;
        AdapterSignal _signal;

        public DisplayDevice(string displayDeviceName, string annotationKey, string annotationDescription)
        {

            _attr = new AdapterAttribute(displayDeviceName, "Hello!", (o) =>
                {
                    string newValue = (string)o;
                    return AllJoynStatusCode.Ok;
                });
            _attr.Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ;
            _attr.Annotations.Add(annotationKey, annotationDescription);
            _attr.COVBehavior = BridgeRT.SignalBehavior.Always;

            _signal = new AdapterSignal(displayDeviceName + "-signal");
            _signal.Params.Add(_attr.Value);

        }

        public AdapterAttribute GetAttribute()
        {
            return _attr;
        }

        public AdapterSignal GetSignal()
        {
            return _signal;
        }

        public string GetCurrentValue()
        {
            return (string)_attr.Value.Data;
        }

        public void SetCurrentValue(string value)
        {
            _attr.Value.Data = value;
            

        }
    }
}
