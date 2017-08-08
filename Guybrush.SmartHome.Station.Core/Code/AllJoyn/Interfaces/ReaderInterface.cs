using AllJoyn.Dsb;
using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.AllJoyn.Interfaces
{
    public class ReaderInterface
    {
        public string Name { get; private set; }
        public IReaderModule Module { get; private set; }
        public AdapterInterface Interface { get; set; }
        public ReaderInterface(string readingTitle, string unit, string annotationKey, string annotationDescription, IReaderModule module)
        {
            Name = readingTitle;
            Module = module;
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

            var attrValue = new AdapterAttribute("Value", Module.Value, (o) =>
            {
                int newValue = (int)o;
                return AllJoynStatusCode.Ok;
            });
            attrValue.Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ;
            attrValue.COVBehavior = BridgeRT.SignalBehavior.Always;
            attrValue.Annotations.Add(annotationKey, annotationDescription);

            Interface.Properties.Add(attrValue);

            var attrUnit = new AdapterAttribute("Unit", Module.Unit, (o) =>
            {
                string newValue = (string)o;
                return AllJoynStatusCode.Ok;
            });
            attrUnit.Access = BridgeRT.E_ACCESS_TYPE.ACCESS_READ;
            attrUnit.COVBehavior = BridgeRT.SignalBehavior.Always;
            attrUnit.Annotations.Add(annotationKey, annotationDescription);
            Interface.Properties.Add(attrUnit);
            Module.ValueChanged += Module_ValueChanged;
        }

        private void Module_ValueChanged(object sender, int value)
        {
            ValueChanged(this, value);
        }

        internal event ReaderEventArgs ValueChanged;

        internal AdapterAttribute UpdateValue(int value)
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
