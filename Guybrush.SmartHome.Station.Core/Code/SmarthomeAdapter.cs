using AllJoyn.Dsb;
using Guybrush.SmartHome.Shared.Builders;
using Guybrush.SmartHome.Shared.Enums;
using Guybrush.SmartHome.Shared.Mappers;
using Guybrush.SmartHome.Station.Core.Managers;
using System.Collections.Generic;

namespace Guybrush.SmartHome.Station
{
    public class SmarthomeAdapter : Adapter
    {
        private AdapterBusObject _abo;
        ConditionManager _conditionManager;

        public SmarthomeAdapter(BridgeConfiguration configuration, ConditionManager conditionMgr) : base(configuration)
        {
            _conditionManager = conditionMgr;

            _abo = new AdapterBusObject("Guybrush");
            AdapterInterface conditionInterface = new AdapterInterface("com.guybrush.station.conditions");

            var addConInputParams = new List<AdapterValue>();
            addConInputParams.Add(new AdapterValue("SourceDeviceType", -1));
            addConInputParams.Add(new AdapterValue("SourceDeviceName", ""));
            addConInputParams.Add(new AdapterValue("TargetDeviceName", ""));
            addConInputParams.Add(new AdapterValue("RequiredValue", -1));
            addConInputParams.Add(new AdapterValue("ConditionType", -1));
            addConInputParams.Add(new AdapterValue("TargetValue", -1));

            var addConOutParams = new List<AdapterValue>();
            addConOutParams.Add(new AdapterValue("Response", ""));

            AdapterMethod addCondAttr = new AdapterMethod("AddCondition", "Calling this method will add new condition.", AddCondition, addConInputParams, addConOutParams);
            conditionInterface.Methods.Add(addCondAttr);



            var getCondOutParams = new List<AdapterValue>();
            getCondOutParams.Add(new AdapterValue("MyArray", new int[0]));
            getCondOutParams.Add(new AdapterValue("SourceDeviceType", ""));
            getCondOutParams.Add(new AdapterValue("SourceDevice", ""));
            getCondOutParams.Add(new AdapterValue("TargetDevice", ""));
            getCondOutParams.Add(new AdapterValue("RequiredValue", ""));
            getCondOutParams.Add(new AdapterValue("ConditionType", ""));
            getCondOutParams.Add(new AdapterValue("TargetValue", ""));

            AdapterMethod getCondAttr = new AdapterMethod("GetConditions", "Calling this method returns list of conditions", GetConditions, null, getCondOutParams);
            conditionInterface.Methods.Add(getCondAttr);

            var removeCondInputParams = new List<AdapterValue>();
            removeCondInputParams.Add(new AdapterValue("SourceDevice", ""));
            removeCondInputParams.Add(new AdapterValue("TargetDevice", ""));

            var removeConOutParams = new List<AdapterValue>();
            removeConOutParams.Add(new AdapterValue("Response", ""));

            AdapterMethod removeCondAttr = new AdapterMethod("RemoveCondition", "Calling this method will remove a condition", RemoveCondition, removeCondInputParams, removeConOutParams);
            conditionInterface.Methods.Add(removeCondAttr);

            _abo.Interfaces.Add(conditionInterface);
            BusObjects.Add(_abo);

        }

        private void RemoveCondition(AdapterMethod sender, IReadOnlyDictionary<string, object> inputParams, IDictionary<string, object> outputParams)
        {
            _conditionManager.RemoveCondition((string)inputParams["SourceDevice"], (string)inputParams["TargetDevice"]);
        }

        private void GetConditions(AdapterMethod sender, IReadOnlyDictionary<string, object> inputParams, IDictionary<string, object> outputParams)
        {
            var conditions = _conditionManager.Conditions;

            var condArrays = new ConditionMapper().MapToParams(conditions);
            outputParams["SourceDeviceType"] = condArrays.SourceDeviceTypes;
            outputParams["SourceDevice"] = condArrays.SourceDeviceNames;
            outputParams["TargetDevice"] = condArrays.TargetDeviceNames;
            outputParams["RequiredValue"] = condArrays.RequiredValues;
            outputParams["ConditionType"] = condArrays.ConditionTypes;
            outputParams["TargetValue"] = condArrays.TargetValues;

        }

        public void AddCondition(AdapterMethod sender, IReadOnlyDictionary<string, object> inputParams, IDictionary<string, object> outputParams)
        {
            var condition = new ConditionBuilder().BuildCondition((DeviceType)inputParams["SourceDeviceType"],
                (string)inputParams["SourceDevice"], (string)inputParams["TargetDevice"],
                (int)inputParams["RequiredValue"], (ConditionType)inputParams["ConditionType"], (int)inputParams["TargetValue"]);

            _conditionManager.RegisterCondition(condition);
        }
    }
}
