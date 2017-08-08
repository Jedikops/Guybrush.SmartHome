using DeviceProviders;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Client.Data.Models
{
    public class Reading
    {

        private IInterface _iface;
        private IProperty _propTitle;
        private IProperty _propValue;
        private IProperty _propUnit;


        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _unit;

        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }


        public Reading(IInterface iface)
        {
            Task.Run(async () =>
            {
                _iface = iface;
                _propTitle = iface.Properties.First(x => x.Name == "Title");
                _propValue = iface.Properties.First(x => x.Name == "Value");
                _propUnit = iface.Properties.First(x => x.Name == "Unit");

                _propValue.ValueChanged += _propValue_ValueChanged;
                _propUnit.ValueChanged += _propUnit_ValueChanged;
                await LoadValues();
            }).Wait();
        }

        private async Task LoadValues()
        {

            var title = await _propTitle.ReadValueAsync();
            _title = (string)title.Value;


            var unit = await _propUnit.ReadValueAsync();
            _unit = (string)unit.Value;

            var value = await _propValue.ReadValueAsync();
            _value = (int)value.Value;
        }

        private async void _propUnit_ValueChanged(IProperty sender, object args)
        {
            var result = await _propUnit.ReadValueAsync();

            lock (Context.Current.Locks["Readings"])
            {
                _unit = (string)result.Value;
                int index = Context.Current.Readings.IndexOf(this);
                Context.Current.Readings[index] = this;
            }


        }

        private async void _propValue_ValueChanged(IProperty sender, object args)
        {
            var result = await _propValue.ReadValueAsync();
            lock (Context.Current.Locks["Readings"])
            {
                _value = (int)result.Value;
                int index = Context.Current.Readings.IndexOf(this);
                Context.Current.Readings[index] = this;
            }

        }

    }
}
