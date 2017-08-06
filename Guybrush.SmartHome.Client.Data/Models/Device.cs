using DeviceProviders;
using Guybrush.SmartHome.Client.Data.Base;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Client.Data.Models
{


    public class Device : Observable
    {
        private readonly IInterface _iface;
        private readonly IProperty _prop;
        private readonly IMethod _method;


        private bool _status;
        public bool Status
        {
            get { return _status; }
            set
            {
                _status = value;
                //OnPropertyChanged();
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                //OnPropertyChanged();
            }
        }

        public Device(IInterface iface, string name)
        {
            _title = name;
            _status = false;

            _iface = iface;
            _prop = iface.Properties.FirstOrDefault(x => x.Name == "Status");
            if (_prop != null)
            {
                _prop.ValueChanged += _prop_ValueChanged;
                _method = iface.Methods.First(x => x.Name == "Switch");

                LoadValue();
            }
        }



        public void LoadValue()
        {
            _prop.ReadValueAsync().Completed += (info, status) =>
            {
                var result = info.GetResults();
                _status = (bool)result.Value;

            };
        }

        private void _prop_ValueChanged(IProperty sender, object args)
        {
            _prop.ReadValueAsync().Completed += (info, status) =>
            {
                var result = info.GetResults();

                lock (Context.Current.Locks["Devices"])
                {
                    _status = (bool)result.Value;
                    //var newMe = new Device(_iface, _title);
                    int index = Context.Current.Devices.IndexOf(this);
                    Context.Current.Devices[index] = this;
                }
            };
        }

        public void UpdateValue(bool value)
        {
            _method.InvokeAsync(new List<object> { value }).Completed += (info, status) =>
            {
                LoadValue();
            };

        }
    }
}
