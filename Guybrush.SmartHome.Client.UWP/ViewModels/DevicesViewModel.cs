using Guybrush.SmartHome.Client.UWP.Base;
using Guybrush.SmartHome.Client.UWP.Models;
using System.Collections.ObjectModel;

namespace Guybrush.SmartHome.Client.UWP.ViewModels
{

    public class DevicesViewModel : Observable
    {
        private Device _selectedDevice;

        public DevicesViewModel()
        {
            Devices = new ObservableCollection<Device>
            {
                new Device { Title = "Lights", Status = 0 },
                new Device { Title = "Air conditioner", Status = 0 },
                new Device { Title = "Blinds", Status = 1}
            };


        }

        public ObservableCollection<Device> Devices { get; private set; }

        public Device SelectedDevice
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
