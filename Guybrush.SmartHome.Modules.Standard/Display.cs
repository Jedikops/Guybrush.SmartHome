using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using System;

namespace Guybrush.SmartHome.Modules.Standard
{
    public class Display : IDisplayModule
    {
        string _text = "Hello!";

        private Guid _id = Guid.NewGuid();
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
                TextChanged?.Invoke(this, value);
            }
        }

        public event DisplayEventArgs TextChanged;
    }
}
