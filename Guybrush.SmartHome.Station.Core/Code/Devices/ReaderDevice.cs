using AllJoyn.Dsb;

namespace Guybrush.SmartHome.Station.Core.Code.Devices
{
    public class ReaderDevice
    {
        AdapterAttribute _attr;
        public ReaderDevice(string readingTitle, string annotationKey, string annotationDescription)
        {

            _attr = new AdapterAttribute(readingTitle, 0, (o) =>
            {
                int newValue = (int)o;
                return AllJoynStatusCode.Ok;
            });
            _attr.Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ;
            _attr.Annotations.Add(annotationKey, annotationDescription);

        }

        public int GetCurrentValue()
        {
            return (int)_attr.Value.Data;
        }

        public AdapterAttribute GetAttribute()
        {
            return _attr;
        }
    }
}
