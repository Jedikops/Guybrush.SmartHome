using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;

namespace Guybrush.SmartHome.Modules.Standard
{
    public class Display : IDisplayModule
    {
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
