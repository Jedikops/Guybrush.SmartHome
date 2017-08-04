using Guybrush.SmartHome.Client.Data;
using Guybrush.SmartHome.Client.Data.Models;
using Guybrush.SmartHome.Client.UWP.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Guybrush.SmartHome.Client.UWP.ViewModels
{

    public class DevicesViewModel : Observable
    {
        private DeviceViewModel _selectedDevice;
        private StationProxy _proxy;
        public DevicesViewModel()
        {
            Devices = new ObservableCollection<DeviceViewModel>();

            _proxy = StationProxy.Current;

            var devs = _proxy.Devices.Select(x => new DeviceViewModel()
            {
                Title = x.Title,
                Status = x.GetCurrentValue()
            });

            foreach (DeviceViewModel model in devs)
            {
                Devices.Add(model);
            }

            _proxy.Devices.CollectionChanged += Devices_CollectionChanged;
            //{
            //    new DeviceViewModel { Title = "Lights", Status = 0 },
            //    new DeviceViewModel { Title = "Air conditioner", Status = 0 },
            //    new DeviceViewModel { Title = "Blinds", Status = 1}
            //};


        }

        private void Devices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                IList<DeviceViewModel> items =
                    ((IList<DeviceViewModel>)e.NewItems)
                    .Select(x => new DeviceViewModel() { Title = x.Title, Status = x.Status }).ToList();

                foreach (var item in items)
                    Devices.Add(item);

            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                IList<Device> items = ((IList<Device>)e.OldItems);

                foreach (var item in items)
                {
                    var device = Devices.FirstOrDefault(x => x.Title == item.Title);
                    if (device != null)
                        Devices.Remove(device);
                }

            }

        }

        public ObservableCollection<DeviceViewModel> Devices { get; private set; }

        public DeviceViewModel SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                if (_selectedDevice != value)
                {
                    _selectedDevice = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsDeviceSelected));
                    OnPropertyChanged(nameof(DeviceButtonText));
                }

            }
        }
        public bool IsDeviceSelected
        {
            get { return SelectedDevice != null; }
        }

        public string DeviceButtonText
        {
            get
            {
                if (SelectedDevice != null)
                {
                    if (SelectedDevice.Status == 1)
                        return "Disable";

                }
                return "Enable";

            }
        }

    }
}
