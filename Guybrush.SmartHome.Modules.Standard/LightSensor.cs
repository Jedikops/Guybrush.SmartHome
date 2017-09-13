using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Modules.TestInterface;
using System;

namespace Guybrush.SmartHome.Modules.Standard
{
    public class LightSensor : IReaderModule, ITestReadModule
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
                Boolean Gain = false;
                uint MS = (uint)TSL2561Sensor.Current.SetTiming(false, 2);
                uint[] Data = TSL2561Sensor.Current.GetData();
                double lux = TSL2561Sensor.Current.GetLux(Gain, MS, Data[0], Data[1]);

                //int val = Convert.ToInt32(BME280Sensor.Current.ReadHumidity().Result);
                if (_value != lux)
                {
                    _value = Convert.ToInt32(lux);
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

        private string _unit = "Lux";
        public string Unit
        {
            get { return _unit; }
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
