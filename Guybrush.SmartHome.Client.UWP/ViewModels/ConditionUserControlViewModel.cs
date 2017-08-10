using Guybrush.SmartHome.Client.Data;
using Guybrush.SmartHome.Client.Data.Base;
using Guybrush.SmartHome.Client.Data.Models;
using Guybrush.SmartHome.Client.UWP.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Guybrush.SmartHome.Client.UWP.ViewModels
{
    public class ConditionUserControlViewModel : Observable
    {
        public ObservableCollection<string> Devices = new ObservableCollection<string>();
        public ObservableCollection<string> DevicesAndReadings = new ObservableCollection<string>();
        public ObservableCollection<ComboBoxItem> RequiredValueDeviceValues = new ObservableCollection<ComboBoxItem> { new ComboBoxItem() { Tag = "0", Content = "False" }, new ComboBoxItem() { Tag = "1", Content = "True" } };
        public ObservableCollection<ComboBoxItem> OperatorReadingValues = new ObservableCollection<ComboBoxItem> { new ComboBoxItem() { Tag = "1", Content = "Less" }, new ComboBoxItem() { Tag = "2", Content = "Equals" }, new ComboBoxItem() { Tag = "3", Content = "More" }, new ComboBoxItem() { Tag = "4", Content = "Not Equals" } };
        public ObservableCollection<ComboBoxItem> OperatorDeviceValues = new ObservableCollection<ComboBoxItem> { new ComboBoxItem() { Tag = "2", Content = "Equals" }, new ComboBoxItem() { Tag = "4", Content = "Not Equals" } };
        public ObservableCollection<ComboBoxItem> TargetValueDeviceValues = new ObservableCollection<ComboBoxItem> { new ComboBoxItem() { Tag = "0", Content = "False" }, new ComboBoxItem() { Tag = "1", Content = "True" } };



        private ConditionViewModel _condition;
        public ConditionViewModel Condition
        {
            get { return _condition; }
            set
            {
                _condition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsConditionSet));
                OnPropertyChanged(nameof(IsConditionNotSet));
            }
        }

        public bool IsConditionNotSet
        {
            get { return _condition == null; }
        }

        public bool IsConditionSet
        {
            get { return _condition != null; }
        }

        private bool _isSourceEnabled;
        public bool IsSourceEnabled
        {
            get { return _isSourceEnabled; }
            set { _isSourceEnabled = value; OnPropertyChanged(); }
        }

        private bool _isSourceAReading;
        public bool IsSourceAReading
        {
            get { return _isSourceAReading; }
            set { _isSourceAReading = value; OnPropertyChanged(); }
        }




        private bool _isSourceADevice;
        public bool IsSourceADevice
        {
            get { return _isSourceADevice; }
            set { _isSourceADevice = value; OnPropertyChanged(); }

        }

        private int _sourceDeviceIndex = -1;
        public int SourceDeviceIndex
        {
            get { return _sourceDeviceIndex; }
            set { _sourceDeviceIndex = value; OnPropertyChanged(); }
        }

        private int _targetDeviceIndex = -1;
        public int TargetDeviceIndex
        {
            get { return _targetDeviceIndex; }
            set { _targetDeviceIndex = value; OnPropertyChanged(); }
        }

        private int _requiredValueReading = -1;

        public int RequiredValueReading
        {
            get { return _requiredValueReading; }
            set { _requiredValueReading = value; OnPropertyChanged(); }
        }

        private int _requiredValueIndex = -1;
        public int RequiredValueDeviceIndex
        {
            get { return _requiredValueIndex; }
            set
            {
                _requiredValueIndex = value; OnPropertyChanged(); ;
            }
        }

        private int _operatorReadingIndex = -1;
        public int OperatorReadingIndex
        {
            get { return _operatorReadingIndex; }
            set { _operatorReadingIndex = value; OnPropertyChanged(); }
        }

        private int _operatorDeviceIndex = -1;
        public int OperatorDeviceIndex
        {
            get { return _operatorDeviceIndex; }
            set { _operatorDeviceIndex = value; OnPropertyChanged(); }
        }


        private int _targetDeviceValue = -1;
        public int TargetDeviceValueIndex
        {
            get { return _targetDeviceValue; }
            set { _targetDeviceValue = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ConditionViewModel> Conditions { get; internal set; }
        public ConditionDetailControl Control { get; internal set; }

        public ConditionUserControlViewModel()
        {
            lock (Context.Current.Locks["Devices"])
            {
                foreach (Device device in Context.Current.Devices)
                {
                    DevicesAndReadings.Add(device.Title);
                    Devices.Add(device.Title);
                }
            }

            lock (Context.Current.Locks["Readings"])
            {
                foreach (Reading device in Context.Current.Readings)
                    DevicesAndReadings.Add(device.Title);
            }

        }

        public void UpdateSourceOrTargetDeviceIndexes()
        {
            if (Condition != null)
            {
                IsSourceEnabled = false;
                int sourceIndex = DevicesAndReadings.TakeWhile(x => x != Condition.SourceDeviceName).Count();
                if (sourceIndex == DevicesAndReadings.Count)
                    SourceDeviceIndex = -1;
                SourceDeviceIndex = sourceIndex;

                int targetIndex = DevicesAndReadings.TakeWhile(x => x != Condition.TargetDeviceName).Count();
                if (targetIndex == DevicesAndReadings.Count)
                    TargetDeviceIndex = -1;
                TargetDeviceIndex = targetIndex;

                IsSourceAReading = Condition.SourceDeviceType == Shared.Enums.DeviceType.ReaderDevice;
                IsSourceADevice = !IsSourceAReading;
                if (Condition.SourceDeviceType == Shared.Enums.DeviceType.TurnOnOffDevice)
                {
                    RequiredValueDeviceIndex = Condition.RequiredValue == 1 ? 1 : 0;

                    OperatorDeviceIndex = (int)Condition.ConditionType == 2 ? 0 : 1;
                }
                else
                {
                    RequiredValueReading = Condition.RequiredValue;
                    OperatorReadingIndex = (int)Condition.ConditionType - 1;
                }

                TargetDeviceValueIndex = Condition.TargetValue == 1 ? 1 : 0;
            }
            else
            {
                IsSourceEnabled = true;
                IsSourceAReading = false;
                IsSourceADevice = false;
                SourceDeviceIndex = -1;
                TargetDeviceIndex = -1;
                RequiredValueDeviceIndex = -1;
                OperatorReadingIndex = -1;
                OperatorDeviceIndex = -1;
                TargetDeviceValueIndex = -1;
            }

        }
        internal async Task SourceSelectionChanged()
        {
            if (Context.Current.ConditionManager.IsConnected)
            {
                if (SourceDeviceIndex != -1)
                {
                    var deviceName = DevicesAndReadings[SourceDeviceIndex];
                    var device = Context.Current.Devices.FirstOrDefault(x => x.Title == deviceName);
                    if (device != null)
                    {

                        IsSourceAReading = false;
                        IsSourceADevice = true;
                    }
                    else
                    {
                        IsSourceAReading = true;
                        IsSourceADevice = false;
                    }
                }
            }
            else
            {
                Control.Reload();
            }
        }
    }
}
