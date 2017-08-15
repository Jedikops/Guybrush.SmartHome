using Guybrush.SmartHome.Shared;
using Guybrush.SmartHome.Shared.Enums;

namespace Guybrush.SmartHome.Client.UWP.ViewModels
{
    public class ConditionViewModel : Observable
    {
        private DeviceType _sourceDeviceType;
        public DeviceType SourceDeviceType
        {
            get { return _sourceDeviceType; }
            set
            {
                _sourceDeviceType = value; OnPropertyChanged();
            }
        }

        private string _sourceDeviceName;
        public string SourceDeviceName
        {
            get { return _sourceDeviceName; }
            set { _sourceDeviceName = value; OnPropertyChanged(); }
        }

        string _targetDeviceName;
        public string TargetDeviceName
        {
            get { return _targetDeviceName; }
            set { _targetDeviceName = value; OnPropertyChanged(); }
        }

        private int _requiredValue;
        public int RequiredValue
        {
            get { return _requiredValue; }
            set { _requiredValue = value; OnPropertyChanged(); }
        }

        private ConditionType _conditionType;
        public ConditionType ConditionType
        {
            get { return _conditionType; }
            set { _conditionType = value; OnPropertyChanged(); }
        }

        private int _targetValue;
        public int TargetValue
        {
            get { return _targetValue; }
            set { _targetValue = value; OnPropertyChanged(); }
        }
    }

}
