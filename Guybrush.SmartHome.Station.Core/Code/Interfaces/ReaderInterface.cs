using AllJoyn.Dsb;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.Code.Devices
{
    public class ReaderInterface
    {
        public string Name { get; private set; }
        public AdapterInterface Interface { get; private set; }
        public ReaderInterface(string readingTitle, string unit, string annotationKey, string annotationDescription)
        {
            Name = readingTitle;
            Interface = new AdapterInterface("com.guybrush.station.readings." + readingTitle.ToLower().Replace(' ', '_'));

            var attrTitle = new AdapterAttribute("Title", readingTitle, (o) =>
            {
                int newValue = (int)o;
                return AllJoynStatusCode.Ok;
            });
            attrTitle.Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ;
            attrTitle.COVBehavior = BridgeRT.SignalBehavior.Always;
            attrTitle.Annotations.Add(annotationKey, annotationDescription);

            Interface.Properties.Add(attrTitle);

            var attrValue = new AdapterAttribute("Value", 0, (o) =>
            {
                int newValue = (int)o;
                return AllJoynStatusCode.Ok;
            });
            attrValue.Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ;
            attrValue.COVBehavior = BridgeRT.SignalBehavior.Always;
            attrValue.Annotations.Add(annotationKey, annotationDescription);

            Interface.Properties.Add(attrValue);

            var attrUnit = new AdapterAttribute("Unit", unit, (o) =>
            {
                string newValue = (string)o;
                return AllJoynStatusCode.Ok;
            });
            attrUnit.Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ;
            attrUnit.COVBehavior = BridgeRT.SignalBehavior.Always;
            attrUnit.Annotations.Add(annotationKey, annotationDescription);
            Interface.Properties.Add(attrUnit);
        }
        public AdapterAttribute UpdateValue(int value)
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
