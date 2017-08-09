using Guybrush.SmartHome.Shared.Enums;
using Guybrush.SmartHome.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Shared.Builders
{
    public class ConditionBuilder
    {
        public ConditionBuilder()
        {

        }

        public Condition BuildCondition(DeviceType deviceType, string sourceDeviceName, string targetDeviceName,
            int requiredValue, ConditionType conditionType, int targetValue)
        {
            return new Condition()
            {
                SourceDeviceType = deviceType,
                SourceDeviceName = sourceDeviceName,
                TargetDeviceName = targetDeviceName,
                RequiredValue = requiredValue,
                ConditionType = conditionType,
                TargetValue = targetValue

            };
        }

        public IList<object>[] BuildConditionsArray(IList<Condition> conditions)
        {
            List<IList<object>> coll = new List<IList<object>>();
            for (int i = 0; i < 6; i++)
            {
                if (i == 1 || i == 2)
                    coll.Add(new List<string>().Cast<object>().ToList());
                else
                    coll.Add(new List<int>().Cast<object>().ToList());

            }

            foreach (var cond in conditions)
            {
                coll[0].Add(Convert.ChangeType((int)cond.SourceDeviceType, typeof(object)));
                coll[1].Add(Convert.ChangeType(cond.SourceDeviceName, typeof(object)));
                coll[2].Add(Convert.ChangeType(cond.TargetDeviceName, typeof(object)));
                coll[3].Add(Convert.ChangeType(cond.RequiredValue, typeof(object)));
                coll[4].Add(Convert.ChangeType((int)cond.ConditionType, typeof(object)));
                coll[5].Add(Convert.ChangeType(cond.TargetValue, typeof(object)));

            }


            return coll.ToArray();

        }
    }
}
