using Guybrush.SmartHome.Modules.Delegates;

namespace Guybrush.SmartHome.Modules.Interfaces
{
    public interface IDisplayModule
    {
        string Text { get; set; }

        //This event must be triggered as soon as value changes 
        event DisplayEventArgs ValueChanged;
    }
}
