using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Shared;
using System.Collections.Generic;

namespace Guybrush.SmartHome.Station.UWP.Code
{
    public class StationViewModel : Observable
    {
        public ITurnOnOffModule light;
        public ITurnOnOffModule blinds;
        public ITurnOnOffModule air;


        public IReaderModule term;
        public IReaderModule humi;
        public IReaderModule ligsens;
        public IDisplayModule disp;

        public List<ITurnOnOffModule> Modules;
        public StationViewModel()
        {
            //light = new Light();
            //blinds = new Blinds();
            //air = new AirConditioner();

            Modules = new List<ITurnOnOffModule>();
            //Modules.Add(light);
            //Modules.Add(blinds);
            //Modules.Add(air);


            //term = new Termomethre();
            //humi = new HumiditySensor();
            //ligsens = new LightSensor();
            //disp = new Display();
        }

        internal void AddModule(ITurnOnOffModule module)
        {
            Modules.Add(module);
        }
    }
}
