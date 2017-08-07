using Guybrush.SmartHome.Modules.Delegates;
using System;

namespace Guybrush.SmartHome.Modules.Interfaces
{
    public interface ITurnOnOffModule
    {
        Guid Id { get; }
        bool Status { get; set; }

        //This event must be triggered as soon as value changes 
        event DeviceOnOffEventArgs ValueChanged;
    }
}