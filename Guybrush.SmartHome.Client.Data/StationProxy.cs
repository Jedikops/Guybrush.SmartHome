using DeviceProviders;
using Guybrush.SmartHome.Client.Data.Models;
using System.Collections.Generic;
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
                lock (Context.Current.Locks["Global"])
                {
                    if (name != "Readings")
                    {
                        lock (Context.Current.Locks["Devices"])
                        {
                            var device = Context.Current.Devices.FirstOrDefault(x => x.Title == name);
                            if (device != null)
                                Context.Current.Devices.Remove(device);
                        }
                    }
                    else
                    {
                        lock (Context.Current.Locks["Readings"])
                        {
                            Context.Current.Readings.Clear();
                        }
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

                if (name == "Readings")
                {
                    IBusObject objStation = args.Service.Objects.FirstOrDefault();
                    if (objStation != null)
                    {
                        List<IInterface> ifaces = objStation.Interfaces.Where(x => x.Name.Contains("com.guybrush.station.readings")).ToList();
                        if (ifaces.Count > 0)
                        {
                            foreach (var iface in ifaces)
                            {
                                lock (Context.Current.Locks["Readings"])
                                {

                                    var reading = new Reading(iface);

                                    Context.Current.Readings.Add(reading);
                                }

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


                                lock (Context.Current.Locks["Devices"])
                                {
                                    var device = new Device(iface, name);

                                    Context.Current.Devices.Add(device);
                                }


                            }
                        }

                    }

                }
            }

            System.Diagnostics.Debug.WriteLine($"Found device '{name}' : ID = {id}");
        }
    }
}
