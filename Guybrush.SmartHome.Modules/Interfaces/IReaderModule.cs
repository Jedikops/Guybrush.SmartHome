using Guybrush.SmartHome.Modules.Delegates;

namespace Guybrush.SmartHome.Modules.Interfaces
{

    public interface IReaderModule
    {
        int Value { get; }
        string Unit { get; }

        //This event must be triggered as soon as value changes 
        event ReaderEventArgs ValueChanged;

    }
}
