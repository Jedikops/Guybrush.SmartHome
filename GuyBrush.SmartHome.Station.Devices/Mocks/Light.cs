namespace GuyBrush.SmartHome.Station.Devices.Mocks
{
    public class Light
    {
        bool _status;

        public Light()
        {

        }

        public void Switch(bool status)
        {
            _status = status;
        }
    }
}
