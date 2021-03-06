﻿using Guybrush.SmartHome.Modules.Delegates;
using System;

namespace Guybrush.SmartHome.Modules.Interfaces
{

    public interface IReaderModule
    {
        Guid Id { get; }
        string Name { get; }
        int Value { get; set; }
        string Unit { get; }

        //This event must be triggered as soon as value changes 
        event ReaderValueEventArgs ValueChanged;
        event ReaderUnitEventArgs UnitChanged;

    }
}
