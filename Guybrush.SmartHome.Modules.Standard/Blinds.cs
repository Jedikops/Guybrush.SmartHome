using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using System;

namespace Guybrush.SmartHome.Modules.Standard
{
    public class Blinds : ITurnOnOffModule
    {
        private bool _status;

        private Guid _id = Guid.NewGuid();
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

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
