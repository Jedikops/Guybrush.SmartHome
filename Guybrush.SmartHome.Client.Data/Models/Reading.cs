using DeviceProviders;
using System.Linq;

namespace Guybrush.SmartHome.Client.Data.Models
{
    public class Reading
    {

        private readonly IInterface _iface;
        private readonly IProperty _propTitle;
        private readonly IProperty _propValue;
        private readonly IProperty _propUnit;


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
            _iface = iface;
            _propTitle = iface.Properties.First(x => x.Name == "Title");
            _propValue = iface.Properties.First(x => x.Name == "Value");
            _propUnit = iface.Properties.First(x => x.Name == "Unit");

            _propValue.ValueChanged += _propValue_ValueChanged;
            _propUnit.ValueChanged += _propUnit_ValueChanged;
            LoadValues();
        }

        private void LoadValues()
        {
            _propTitle.ReadValueAsync().Completed += (info, status) =>
            {
                var result = info.GetResults();
                _title = (string)result.Value;
            };

            _propUnit.ReadValueAsync().Completed += (info, status) =>
            {
                var result = info.GetResults();
                _unit = (string)result.Value;

            };
            _propValue.ReadValueAsync().Completed += (info, status) =>
            {
                var result = info.GetResults();
                _value = (int)result.Value;

            };
        }

        private void _propUnit_ValueChanged(IProperty sender, object args)
        {
            _propUnit.ReadValueAsync().Completed += (info, status) =>
            {
                var result = info.GetResults();

                lock (Context.Current.Locks["Readings"])
                {
                    _unit = (string)result.Value;
                    int index = Context.Current.Readings.IndexOf(this);
                    Context.Current.Readings[index] = this;
                }
            };

        }

        private void _propValue_ValueChanged(IProperty sender, object args)
        {
            _propValue.ReadValueAsync().Completed += (info, status) =>
            {
                var result = info.GetResults();

                lock (Context.Current.Locks["Readings"])
                {
                    _value = (int)result.Value;
                    int index = Context.Current.Readings.IndexOf(this);
                    Context.Current.Readings[index] = this;
                }
            };
        }

    }
}
