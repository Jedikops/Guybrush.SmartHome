using DeviceProviders;
using Guybrush.SmartHome.Shared.Enums;
using Guybrush.SmartHome.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Client.Data.Managers
{
    public class ClientConditionManager
    {
        private IInterface _conditionInterface;
        private IMethod _getConditions;
        private IMethod _removeCondition;
        private IMethod _addCondition;
        private string name;

        public IList<Condition> Conditions { get; private set; }
        public ClientConditionManager()
        {
            Conditions = new List<Condition>();
        }

        public void Configure(IInterface conditionInterface, string name)
        {
            this._conditionInterface = conditionInterface;
            if (_conditionInterface != null)
            {
                _getConditions = _conditionInterface.Methods.First(x => x.Name == "GetConditions");
                _removeCondition = _conditionInterface.Methods.First(x => x.Name == "RemoveCondition");
                _addCondition = _conditionInterface.Methods.First(x => x.Name == "AddCondition");
            }
            this.name = name;
        }

        public async Task<IList<Condition>> GetConditions()
        {
            lock (Context.Current.Locks["Conditions"])
            {
                Conditions.Clear();
            }
            if (_getConditions != null)
            {
                InvokeMethodResult result = await _getConditions.InvokeAsync(new List<object>());
                var values = result.Values;
                if (!string.IsNullOrEmpty(values[0] as string))
                {
                    string[] sourceTypes = ((string)values[0]).Split(new[] { ";" }, StringSplitOptions.None);
                    string[] sourceNames = ((string)values[1]).Split(new[] { ";" }, StringSplitOptions.None);
                    string[] targetNames = ((string)values[2]).Split(new[] { ";" }, StringSplitOptions.None);
                    string[] requiredValues = ((string)values[3]).Split(new[] { ";" }, StringSplitOptions.None);
                    string[] conditionTypes = ((string)values[4]).Split(new[] { ";" }, StringSplitOptions.None);
                    string[] targetValues = ((string)values[5]).Split(new[] { ";" }, StringSplitOptions.None);

                    if (sourceTypes != null)
                    {
                        lock (Context.Current.Locks["Conditions"])
                        {
                            for (int i = 0; i < sourceTypes.Length - 1; i++)
                            {
                                Conditions.Add(new Condition()
                                {
                                    SourceDeviceType = (DeviceType)Convert.ToInt32(sourceTypes[i]),
                                    SourceDeviceName = sourceNames[i],
                                    TargetDeviceName = targetNames[i],
                                    RequiredValue = Convert.ToInt32(requiredValues[i]),
                                    ConditionType = (ConditionType)Convert.ToInt32(conditionTypes[i]),
                                    TargetValue = Convert.ToInt32(targetValues[i])
                                });
                            }
                        }
                    }

                }
            }
            return Conditions;
        }

        public async Task DeleteCondition(string sourceName, string targetName)
        {
            await _removeCondition.InvokeAsync(new List<object>() { sourceName, targetName });

        }

        public async Task AddCondition(int deviceType, string sourceName, string targetName, int requiredValue, int conditionType, int targetValue)
        {
            await _addCondition.InvokeAsync(new List<object>() { deviceType, sourceName, targetName, requiredValue, conditionType, targetValue });
        }
    }
}
