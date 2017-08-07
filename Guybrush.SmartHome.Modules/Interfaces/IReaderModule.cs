using Guybrush.SmartHome.Modules.Delegates;
using System;

namespace Guybrush.SmartHome.Modules.Interfaces
{

    public interface IReaderModule
    {
        Guid Id { get; }
        int Value { get; }
        string Unit { get; }

        //This event must be triggered as soon as value changes 
        event ReaderEventArgs ValueChanged;

    }
}
