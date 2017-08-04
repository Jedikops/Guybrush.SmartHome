using Guybrush.SmartHome.Client.Data.Enums;
using Guybrush.SmartHome.Client.Data.Interface;

namespace Guybrush.SmartHome.Client.Data.Models
{
    public class Condition
    {

        public IDevice Source { get; set; }

        public string TargetDeviceId { get; set; }
        public int TargetValue { get; set; }

        public ConditionType ConditionType { get; set; }



    }
}
