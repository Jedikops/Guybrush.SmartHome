namespace GuyBrush.SmartHome.Station.Devices.Mocks
{
    public class Termomethre : IReaderModule
    {
        int reading;
        public static readonly string Unit = "C";

        public Termomethre()
        {

        }

        public int Read()
        {
            return reading;
        }


    }
}
