using DeviceProviders;
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

        private void ServiceDropped(IProvider sender, ServiceDroppedEventArgs args)
        {
            var name = args.Service.AboutData?.DeviceName;
            var id = args.Service.AboutData?.DeviceId;
            var appName = args.Service.AboutData?.AppName;
            if (appName == "Guybrush Station")
            {
                if (name != "Station")
                {
                    lock (Context.Current.Locks["Devices"])
                    {
                        var device = Context.Current.Devices.FirstOrDefault(x => x.Title == name);
                        if (device != null)
                            Context.Current.Devices.Remove(device);
                    }
                }
                System.Diagnostics.Debug.WriteLine($"Lost device '{name}' : ID = {id}");
            }
        }

        private void ServiceJoined(IProvider sender, ServiceJoinedEventArgs args)
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
                                        Context.Current.Readings.Add(reading);
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
                                    lock (Context.Current.Locks["Devices"])
                                    {
                                        var device = new Device(iface, name);
                                        Context.Current.Devices.Add(device);
                                    }

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





    }
}
