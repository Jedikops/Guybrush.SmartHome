using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using System;

namespace Guybrush.SmartHome.Station.Tests.Mocks
{
    public class Blinds : ITurnOnOffModule
    {
        private Guid _id = Guid.NewGuid();
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        private bool _status;
        public bool Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                if (ValueChanged != null)
                    ValueChanged(this, value);
            }
        }

        public event DeviceOnOffEventArgs ValueChanged;
    }
}
