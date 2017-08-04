using Guybrush.SmartHome.Client.Data;
using Guybrush.SmartHome.Client.Data.Base;
using Guybrush.SmartHome.Client.Data.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Core;

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
                            Status = device.GetCurrentValue()
                        };
                        Devices.Add(devVM);
                    }
                }

                Context.Current.Devices.CollectionChanged += Devices_CollectionChanged;
            }
        }

        public async void Devices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                      
                        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                        {
                            foreach (Device device in e.NewItems.OfType<Device>())
                            {
                                var dev = new DeviceViewModel() { Title = device.Title, Status = Convert.ToInt32(device.Status) };

                                Devices.Add(dev);

                            }

                        }
                        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                        {
                            foreach (Device device in e.OldItems.OfType<Device>())
                            {
                                var deviceVM = Devices.FirstOrDefault(x => x.Title == device.Title);
                                if (deviceVM != null)
                                    Devices.Remove(deviceVM);
                            }
                        }
                       
                    });


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

        #endregion

    }
}
