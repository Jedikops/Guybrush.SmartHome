﻿using DeviceProviders;
using Guybrush.SmartHome.Client.Data.Models;
using System.Linq;

namespace Guybrush.SmartHome.Client.Data
{
    public class StationProxy
    {

        public static StationProxy Current = new StationProxy();

        private AllJoynProvider _provider;

        private StationProxy()
        {
            _provider = new AllJoynProvider();
            _provider.ServiceJoined += ServiceJoined;
            _provider.ServiceDropped += ServiceDropped;


        }
        public void Activate()
        {
            _provider.Start();
        }

        private async void ServiceDropped(IProvider sender, ServiceDroppedEventArgs args)
        {
            var name = args.Service.AboutData?.DeviceName;
            var id = args.Service.AboutData?.DeviceId;
            var appName = args.Service.AboutData?.AppName;
            if (appName == "Guybrush Station")
            {
                lock (Context.Current.Locks["Global"])
                {

                    var device = Context.Current.Devices.FirstOrDefault(x => x.Title == name);
                    var reading = Context.Current.Readings.FirstOrDefault(x => x.Title == name);
                    if (device != null)
                    {
                        lock (Context.Current.Locks["Devices"])
                        {

                            if (device != null)
                                Context.Current.Devices.Remove(device);
                        }
                    }
                    else if (reading != null)
                    {
                        lock (Context.Current.Locks["Readings"])
                        {
                            var reader = Context.Current.Readings.FirstOrDefault(x => x.Title == name);
                            if (reader != null)
                                Context.Current.Readings.Remove(reader);
                        }
                    }
                    else if (name == "Guybrush Smart Home")
                    {
                        lock (Context.Current.Locks["Conditions"])
                        {
                            Context.Current.ConditionManager.Disconnect();
                        }
                    }
                }
                System.Diagnostics.Debug.WriteLine($"Lost device '{name}' : ID = {id}");
            }
        }

        private async void ServiceJoined(IProvider sender, ServiceJoinedEventArgs args)
        {
            var name = args.Service.AboutData?.DeviceName;
            var id = args.Service.AboutData?.DeviceId;
            var appName = args.Service.AboutData?.AppName;
            if (appName == "Guybrush Station")
            {
                var obj = args.Service.Objects.FirstOrDefault(x => x.Path == "/Guybrush");
                if (obj != null)
                {
                    var readInterface = obj.Interfaces.FirstOrDefault(x => x.Name == "com.guybrush.devices.reader");
                    var deviceInterface = obj.Interfaces.FirstOrDefault(x => x.Name == "com.guybrush.devices.onoffcontrol");
                    var conditionInterface = obj.Interfaces.FirstOrDefault(x => x.Name == "com.guybrush.station.conditions");
                    if (readInterface != null)
                    {
                        lock (Context.Current.Locks["Readings"])
                        {
                            //if (Context.Current.Readings.All(x => x.Title != name))
                            {
                                var reading = new Reading(readInterface, name);
                                Context.Current.Readings.Add(reading);
                            }
                        }
                    }
                    else if (deviceInterface != null)
                    {
                        lock (Context.Current.Locks["Devices"])
                        {
                            //if (Context.Current.Devices.All(x => x.Title != name))
                            {
                                var device = new Device(deviceInterface, name);
                                Context.Current.Devices.Add(device);
                            }
                        }
                    }
                    else if (conditionInterface != null)
                    {
                        var getCondition = conditionInterface.Methods.FirstOrDefault(x => x.Name == "GetConditions");
                        if (getCondition != null)
                        {
                            Context.Current.ConditionManager.Configure(conditionInterface, name);


                        }
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine($"Found device '{name}' : ID = {id}");
        }
    }
}
