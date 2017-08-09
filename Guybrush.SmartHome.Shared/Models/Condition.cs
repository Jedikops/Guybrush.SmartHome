using Guybrush.SmartHome.Shared.Enums;

namespace Guybrush.SmartHome.Shared.Models
{
    public class Condition
    {
        public int item { get; set; }
        public DeviceType SourceDeviceType { get; set; }

        public string SourceDeviceName { get; set; }

        public string TargetDeviceName { get; set; }

        public int RequiredValue { get; set; }

        public ConditionType ConditionType { get; set; }

        public int TargetValue { get; set; }

    }
}
