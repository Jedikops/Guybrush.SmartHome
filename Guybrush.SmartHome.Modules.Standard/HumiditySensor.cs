using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Modules.TestInterface;
using System;

namespace Guybrush.SmartHome.Modules.Standard
{
    public class HumiditySensor : IReaderModule, ITestReadModule
    {
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

        private int _value;
        public int Value
        {
            get
            {
                int val = Convert.ToInt32(BME280Sensor.Current.ReadHumidity().Result);
                if (_value != val)
                {
                    _value = val;
                    ValueChanged?.Invoke(this, Value);
                }
                return _value;

            }
            set
            {
                //_value = value;
                //ValueChanged?.Invoke(this, value);
            }
        }

        private string _unit = "%";
        public string Unit
        {
            get
            {

                return _unit;
            }
            set
            {
                _unit = value;
                UnitChanged?.Invoke(this, value);
            }
        }

        public event ReaderValueEventArgs ValueChanged;
        public event ReaderUnitEventArgs UnitChanged;

    }
}
