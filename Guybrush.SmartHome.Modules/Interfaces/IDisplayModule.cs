using Guybrush.SmartHome.Modules.Delegates;
using System;

namespace Guybrush.SmartHome.Modules.Interfaces
{
    public interface IDisplayModule
    {
        Guid Id { get; }
        string Text { get; set; }

        //This event must be triggered as soon as value changes 
        event DisplayEventArgs TextChanged;
    }
}
