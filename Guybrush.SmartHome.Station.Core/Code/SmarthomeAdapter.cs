using AllJoyn.Dsb;

namespace Guybrush.SmartHome.Station
{
    public class SmarthomeAdapter : Adapter
    {
        private AdapterBusObject _abo;

        public SmarthomeAdapter(BridgeConfiguration configuration) : base(configuration)
        {
            _abo = new AdapterBusObject("Guybrush Smart Home");

            BusObjects.Add(_abo);

        }
    }
}
