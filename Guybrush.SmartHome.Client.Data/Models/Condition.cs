﻿using Guybrush.SmartHome.Client.Data.Enums;

namespace Guybrush.SmartHome.Client.Data.Models
{
    public class Condition
    {

        public string SourceDeviceId { get; set; }

        public string TargetDeviceId { get; set; }
        public int TargetValue { get; set; }

        public ConditionType ConditionType { get; set; }



    }
}
