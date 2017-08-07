using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using System;

namespace Guybrush.SmartHome.Station.Tests.Mocks
{
    public class Display : IDisplayModule
    {
        private Guid _id = Guid.NewGuid();
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        string _text = "Hello!";
        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
                if (ValueChanged != null)
                    ValueChanged(this, value);
            }
        }

        public event DisplayEventArgs ValueChanged;
    }
}
