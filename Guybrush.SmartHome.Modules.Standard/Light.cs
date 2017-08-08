using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Station.Core.Base;
using System;

namespace Guybrush.SmartHome.Modules.Standard
{
    public class Light : Observable, ITurnOnOffModule
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
                ValueChanged?.Invoke(this, value);
                OnPropertyChanged();

            }
        }

        public event DeviceOnOffEventArgs ValueChanged;
    }
}
