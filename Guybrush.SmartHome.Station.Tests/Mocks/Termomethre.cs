﻿using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Modules.TestInterface;
using System;

namespace Guybrush.SmartHome.Station.Tests.Mocks
{
    public class Termomethre : IReaderModule, ITestReadModule
    {
        private Guid _id = Guid.NewGuid();
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

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

        private readonly string _unit = "C";

        public string Unit
        {
            get { return _unit; }
        }

        public event ReaderEventArgs ValueChanged;
    }
}