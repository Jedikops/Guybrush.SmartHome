using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Modules.Standard.Mocks;
using Guybrush.SmartHome.Shared;
using System.Collections.Generic;

namespace Guybrush.SmartHome.Station.UWP.Tests.Code
{
    public class StationViewModel : Observable
    {
        public Light light;
        public Blinds blinds;
        public AirConditioner air;
        public Termomethre term;
        public HumiditySensor humi;
        public LightSensor ligsens;
        public Display disp;
        public List<ITurnOnOffModule> Modules;
        public StationViewModel()
        {
            light = new Light();
            blinds = new Blinds();
            air = new AirConditioner();

            Modules = new List<ITurnOnOffModule>();
            Modules.Add(light);
            Modules.Add(blinds);
            Modules.Add(air);


            term = new Termomethre();
            humi = new HumiditySensor();
            ligsens = new LightSensor();
            disp = new Display();
        }
    }
}
