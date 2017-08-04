using AllJoyn.Dsb;
using Guybrush.SmartHome.Station.Core.Code.Devices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Station
{
    public class SmarthomeAdapter : Adapter
    {
        private AdapterBusObject _abo;
        private AdapterInterface _interface;

        private IDictionary<string, ReaderDevice> _readings;
        private DisplayDevice _display;

        public SmarthomeAdapter(BridgeConfiguration configuration) : base(configuration)
        {
            _readings = new Dictionary<string, ReaderDevice>();

            _abo = new AdapterBusObject("Guybrush Smart Home");
            _interface = new AdapterInterface("com.guybrush.station");

            RegisterReader("Temperature", "Temperature reading", "Current temperature value in Celcious");
            RegisterReader("Light Intensity", "Light intensity reading", "Current light intensity value in Lux");
            RegisterReader("Humidity", "Humidity reading", "Current humidity");

            RegisterDisplay("Display", "Display device", "Display device last message");

            _abo.Interfaces.Add(_interface);
            BusObjects.Add(_abo);

            var delay = Task.Run(async () =>
            {
                while (true)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    await Task.Delay(5000);

                    if (_display.GetCurrentValue() == "I'm Done!")
                        _display.SetCurrentValue("Working...");
                    else
                        _display.SetCurrentValue("I'm Done!");

                    NotifySignalListener(_display.GetSignal());

                    sw.Stop();
                    

                }
                //return sw.ElapsedMilliseconds;
            });

        }

        public void RegisterReader(string readingTitle, string annotationKey, string annotationDescription)
        {
            ReaderDevice reader = new ReaderDevice(readingTitle, annotationKey, annotationDescription);
            _interface.Properties.Add(reader.GetAttribute());
            _readings.Add(readingTitle, reader);
        }

        public void RegisterDisplay(string displayDeviceName, string annotationKey, string annotationDescription)
        {
            _display = new DisplayDevice(displayDeviceName, annotationKey, annotationDescription);
            _interface.Properties.Add(_display.GetAttribute());
            _interface.Signals.Add(_display.GetSignal());

        }
    }
}
