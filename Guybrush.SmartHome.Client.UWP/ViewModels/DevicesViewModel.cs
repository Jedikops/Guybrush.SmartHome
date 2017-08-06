using Guybrush.SmartHome.Client.Data;
using Guybrush.SmartHome.Client.Data.Base;
using Guybrush.SmartHome.Client.UWP.Handlers;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Guybrush.SmartHome.Client.UWP.ViewModels
{

    public class DevicesViewModel : Observable
    {

        public ObservableCollection<DeviceViewModel> Devices { get; private set; }

        public DevicesViewModel()
        {
            Devices = new ObservableCollection<DeviceViewModel>();


            lock (Context.Current.Locks["Devices"])
            {
                foreach (var device in Context.Current.Devices)
                {
                    if (device != null)
                    {
                        var devVM = new DeviceViewModel()
                        {
                            Title = device.Title,
                            Status = Convert.ToInt32(device.Status)
                        };
                        Devices.Add(devVM);
                    }
                }

                CollectionHandler<DeviceViewModel> handler = new CollectionHandler<DeviceViewModel>(Devices, SelectedDevice);
                Context.Current.Devices.CollectionChanged += handler.Devices_CollectionChanged;
            }
        }

        #region Selected Device

        private DeviceViewModel _selectedDevice;
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

        internal void ChangeStatus(DeviceViewModel clickedDevice = null)
        {
            if (SelectedDevice != null && (SelectedDevice == clickedDevice || clickedDevice == null))
            {
                var device = Context.Current.Devices.FirstOrDefault(x => x.Title == SelectedDevice.Title);
                if (device != null)
                    device.UpdateValue(!Convert.ToBoolean(SelectedDevice.Status));

            }
        }
        #endregion

    }
}
