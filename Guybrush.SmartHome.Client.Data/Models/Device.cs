using DeviceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Guybrush.SmartHome.Client.Data.Models
{

    public class Device
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
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
            }
        }

        public Device(IInterface iface, string name)
        {
            Task.Run(async () =>
            {
                _title = name;

                _iface = iface;
                _prop = iface.Properties.FirstOrDefault(x => x.Name == "Status");
                if (_prop != null)
                {
                    _prop.ValueChanged += _prop_ValueChanged;
                    _method = iface.Methods.First(x => x.Name == "Switch");
                    await LoadValue();
                }
            }).Wait();
        }



        public async Task LoadValue()
        {
            var result = await _prop.ReadValueAsync();
            if (result.Value != null)
                _status = (bool)result.Value;

        }

        private async void _prop_ValueChanged(IProperty sender, object args)
        {
            var result = await _prop.ReadValueAsync();

            lock (Context.Current.Locks["Devices"])
            {
                _status = (bool)result.Value;
                int index = Context.Current.Devices.IndexOf(this);
                Context.Current.Devices[index] = this;
            }

        }

        public async void UpdateValue(bool value)
        {
            var result = await _method.InvokeAsync(new List<object> { value });
            if (result.Status == AllJoynStatus.Succeeded)
                await LoadValue();


        }
    }
}
