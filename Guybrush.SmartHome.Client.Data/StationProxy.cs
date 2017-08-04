using DeviceProviders;
using Guybrush.SmartHome.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Guybrush.SmartHome.Client.Data
{
    public class StationProxy
    {


        public static StationProxy Current = new StationProxy();


        private AllJoynProvider _provider;
        private StationProxy()
        {
            _devices = new ObservableCollection<Device>();
            _readings = new ObservableCollection<Reading>();
            _conditions = new ObservableCollection<Condition>();
            _users = new ObservableCollection<User>();

        }

        public void Initialize()
        {
            _provider = new DeviceProviders.AllJoynProvider();
            _provider.ServiceJoined += ServiceJoined;
            _provider.ServiceDropped += ServiceDropped;
            _provider.Start();
        }

        private void ServiceDropped(DeviceProviders.IProvider sender, DeviceProviders.ServiceDroppedEventArgs args)
        {
            var name = args.Service.AboutData?.DeviceName;
            var id = args.Service.AboutData?.DeviceId;




            System.Diagnostics.Debug.WriteLine($"Lost device '{name}' : ID = {id}");
        }

        private async void ServiceJoined(DeviceProviders.IProvider sender, DeviceProviders.ServiceJoinedEventArgs args)
        {
            var name = args.Service.AboutData?.DeviceName;
            var id = args.Service.AboutData?.DeviceId;
            var appName = args.Service.AboutData?.AppName;
            if (appName == "Guybrush Station")
            {

                if (name == "Station")
                {
                    IBusObject objStation = args.Service.Objects.Where(x => x.Path == "/Guybrush_Smart_Home").FirstOrDefault();
                    if (objStation != null)
                    {
                        IInterface iface = objStation.Interfaces.Where(x => x.Name == "com.guybrush.station").FirstOrDefault();
                        if (iface != null)
                        {
                            foreach (var prop in iface.Properties)
                            {
                                prop.ReadValueAsync().Completed += (info, status) =>
                                {
                                    var typeinfo = prop.TypeInfo;
                                    if (typeinfo.Type == TypeId.Int32)
                                    {
                                        var reading = new Reading(prop.Name, "unit");
                                        var result = info.GetResults();
                                        reading.Value = (int)result.Value;
                                        Readings.Add(reading);
                                    }

                                };
                            }

                        }

                    }

                }
                else
                {
                    var deviceObj = args.Service.Objects.FirstOrDefault();
                    if (deviceObj != null)
                    {


                        var iface = deviceObj.Interfaces.FirstOrDefault(x => x.Name == "com.guybrush.devices.OnOffControl");
                        if (iface != null)
                        {
                            //Add method delegate

                            var prop = iface.Properties.FirstOrDefault(x => x.Name == "Status");
                            if (prop != null)
                            {

                                prop.ReadValueAsync().Completed += (info, status) =>
                                {
                                    var device = new Device();
                                    var result = info.GetResults();
                                    device.Title = name;
                                    device.Status = (bool)result.Value;
                                    var methodInfo = iface.Methods.First(x => x.Name == "Switch");

                                    Action<int> method = new Action<int>(x =>
                                    {
                                        //This is not validated;
                                        methodInfo.InvokeAsync(new List<object> { x });

                                    });

                                    device.Method = method;
                                    Devices.Add(device);

                                };

                            }
                        }

                    }
                }



            }

            System.Diagnostics.Debug.WriteLine($"Found device '{name}' : ID = {id}");
        }

        public void Interrogate(string serviceName)
        {
            IService service = _provider.Services.Where(x => x.Name == serviceName).FirstOrDefault();
            if (service != null)
                foreach (var obj in service.Objects)
                {
                    foreach (var i in obj.Interfaces)
                    {
                        var properties = i.Properties;
                        var methods = i.Methods;
                        var events = i.Signals;
                    }
                }
        }



        private ObservableCollection<Device> _devices;
        private ObservableCollection<Reading> _readings;
        private ObservableCollection<Condition> _conditions;
        private ObservableCollection<User> _users;
        public ObservableCollection<Device> Devices
        {
            get { return _devices; }
            private set { _devices = value; }
        }

        public ObservableCollection<Reading> Readings
        {
            get { return _readings; }
            private set { _readings = value; }
        }
    }
}
