﻿using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Modules.TestInterface;

namespace Guybrush.SmartHome.Modules.Mocks
{
    public class LightSensor : IReaderModule, ITestReadModule
    {
        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (ValueChanged != null)
                    ValueChanged(this, value);
            }
        }

        private readonly string _unit = "Lux";

        public string Unit
        {
            get { return _unit; }
        }

        public event ReaderEventArgs ValueChanged;
    }
}
