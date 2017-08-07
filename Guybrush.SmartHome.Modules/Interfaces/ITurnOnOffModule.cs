using Guybrush.SmartHome.Modules.Delegates;

namespace Guybrush.SmartHome.Modules.Interfaces
{
    public interface ITurnOnOffModule
    {
        bool Status { get; set; }

        //This event must be triggered as soon as value changes 
        event DeviceOnOffEventArgs ValueChanged;
    }
}