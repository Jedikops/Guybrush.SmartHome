using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;

namespace Guybrush.SmartHome.Modules.Standard
{
    public class Light : ITurnOnOffModule
    {
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
