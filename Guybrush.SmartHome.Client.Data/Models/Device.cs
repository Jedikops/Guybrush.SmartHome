using DeviceProviders;
using Guybrush.SmartHome.Client.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Client.Data.Models
{


    public class Device : Observable
    {
        private IInterface _iface;
        private IProperty _prop;
        private IMethod _method;


        private bool _status;
        public bool Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public Device(IInterface iface, string name)
        {
            _title = name;
            _status = false;

            _prop = iface.Properties.First(x => x.Name == "Status");
            _method = iface.Methods.First(x => x.Name == "Switch");
            LoadValue();
        }

        public bool LoadValue()
        {
            _prop.ReadValueAsync().Completed += (info, status) =>
            {
                var result = info.GetResults();
                _status = (bool)result.Value;
            };
            return _status;
        }

        public void UpdateValue(int value)
        {
            _method.InvokeAsync(new List<object> { value }).Completed += (info, status) =>
            {
                Status = Convert.ToBoolean(value);
                LoadValue();
            };
        }

        public int GetCurrentValue()
        {
            return Status ? 1 : 0;
        }

    }
}
