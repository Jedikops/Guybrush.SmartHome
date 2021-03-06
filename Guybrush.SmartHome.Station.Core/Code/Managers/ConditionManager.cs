﻿using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Shared.Enums;
using Guybrush.SmartHome.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Station.Core.Managers
{
    public class ConditionManager
    {
        public object Locker = new object();
        IList<Condition> _conditions;
        IList<ITurnOnOffModule> _devices;
        //IList<IReaderModule> _readers;

        public IList<Condition> Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }


        public ConditionManager(IList<Condition> conditions, IList<ITurnOnOffModule> devices)
        {
            _devices = devices;
            lock (Locker)
            {
                _conditions = conditions;

                // _conditions.Add(new Condition()
                // {
                //     SourceDeviceType = DeviceType.TurnOnOffDevice,
                //     SourceDeviceName = "Light",
                //     TargetDeviceName = "Blinds",
                //     RequiredValue = 1,
                //     ConditionType = ConditionType.Equals,
                //     TargetValue = 1
                // });

                _conditions.Add(new Condition()
                {
                    SourceDeviceType = DeviceType.ReaderDevice,
                    SourceDeviceName = "Termomethre",
                    TargetDeviceName = "Air Conditioner",
                    RequiredValue = 14,
                    ConditionType = ConditionType.More,
                    TargetValue = 1
                });

                _conditions.Add(new Condition()
                {
                    SourceDeviceType = DeviceType.ReaderDevice,
                    SourceDeviceName = "Light Sensor",
                    TargetDeviceName = "Light",
                    RequiredValue = 500,
                    ConditionType = ConditionType.Less,
                    TargetValue = 1
                });
            }
        }

        public void RegisterCondition(Condition cond)
        {
            var searchedCondition = _conditions.FirstOrDefault(x => x.SourceDeviceName == cond.SourceDeviceName && x.TargetDeviceName == cond.TargetDeviceName);
            if (searchedCondition == null)
            {
                lock (Locker)
                {
                    _conditions.Add(cond);
                }
            }
            else
            {
                lock (Locker)
                {
                    _conditions[_conditions.IndexOf(searchedCondition)] = cond;
                }
            }
        }

        public void RemoveCondition(string sourceDeviceName, string targetDeviceName)
        {
            var condition = _conditions.FirstOrDefault(x => x.SourceDeviceName == sourceDeviceName && x.TargetDeviceName == targetDeviceName);
            if (condition != null)
            {
                lock (Locker)
                {
                    _conditions.Remove(condition);
                }
            }
        }

        public void TurnOnOffModule_ValueChanged(object sender, bool value)
        {
            var sourceDevice = (ITurnOnOffModule)sender;
            var sourceName = sourceDevice.Name;
            Condition condition = null;
            lock (Locker)
            {
                condition = _conditions.FirstOrDefault(x => x.SourceDeviceName == sourceName);
            }
            if (condition != null)
            {
                if (condition.SourceDeviceType == DeviceType.TurnOnOffDevice)
                {
                    var targetDevice = _devices.FirstOrDefault(x => x.Name == condition.TargetDeviceName);
                    if (targetDevice != null)
                    {

                        var currValue = sourceDevice.Status;
                        bool sourceValue = Convert.ToBoolean(condition.RequiredValue);

                        bool isFulfilled = (condition.ConditionType == ConditionType.Equals) ?
                            (currValue == sourceValue) : (currValue != sourceValue);

                        var targetValue = Convert.ToBoolean(condition.TargetValue);

                        if (isFulfilled)
                        {
                            if (targetValue != targetDevice.Status)
                                targetDevice.Status = targetValue;
                        }
                        else
                        {
                            if (targetValue == targetDevice.Status)
                                targetDevice.Status = !targetValue;
                        }
                    }
                }
            }
        }
        public void ReaderModule_ValueChanged(object sender, int value)
        {
            var sourceDevice = (IReaderModule)sender;
            var sourceName = sourceDevice.Name;
            var condition = _conditions.FirstOrDefault(x => x.SourceDeviceName == sourceName);
            if (condition != null)
            {
                if (condition.SourceDeviceType == DeviceType.ReaderDevice)
                {
                    var targetDevice = _devices.FirstOrDefault(x => x.Name == condition.TargetDeviceName);
                    if (targetDevice != null)
                    {
                        var currValue = value;
                        int sourceValue = condition.RequiredValue;

                        bool isFulfilled = false;

                        switch (condition.ConditionType)
                        {
                            case ConditionType.Equals:
                                isFulfilled = (currValue == sourceValue);
                                break;
                            case ConditionType.NotEquals:
                                isFulfilled = (currValue != sourceValue);
                                break;
                            case ConditionType.Less:
                                isFulfilled = (currValue < sourceValue);
                                break;
                            case ConditionType.More:
                                isFulfilled = (currValue > sourceValue);
                                break;

                        }

                        var targetValue = Convert.ToBoolean(condition.TargetValue);

                        if (isFulfilled)
                        {
                            if (targetValue != targetDevice.Status)
                                targetDevice.Status = targetValue;
                        }
                        else
                        {
                            if (targetValue == targetDevice.Status)
                                targetDevice.Status = !targetValue;
                        }

                    }
                }
            }
        }
    }
}
