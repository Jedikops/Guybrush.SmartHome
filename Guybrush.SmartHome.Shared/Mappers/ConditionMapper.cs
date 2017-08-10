using Guybrush.SmartHome.Shared.Models;
using Guybrush.SmartHome.Station.Core.Models;
using System.Collections.Generic;

namespace Guybrush.SmartHome.Shared.Mappers
{
    public class ConditionMapper
    {
        public ConditionContainer MapToParams(IList<Condition> conditions)
        {
            var container = new ConditionContainer();
            IList<int> asd = new List<int>();
            foreach (var cond in conditions)
            {
                if (cond != null)
                {
                    container.SourceDeviceTypes += (int)cond.SourceDeviceType + ";";
                    container.SourceDeviceNames += cond.SourceDeviceName + ";";
                    container.TargetDeviceNames += cond.TargetDeviceName + ";";
                    container.RequiredValues += cond.RequiredValue + ";";
                    container.ConditionTypes += (int)cond.ConditionType + ";";
                    container.TargetValues += cond.TargetValue + ";";
                }
            }

            return container;
        }
    }
}
