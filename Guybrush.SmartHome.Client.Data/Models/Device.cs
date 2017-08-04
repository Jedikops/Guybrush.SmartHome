using Guybrush.SmartHome.Client.Data.Interface;

namespace Guybrush.SmartHome.Client.Data.Models
{


    public class Device : IDevice
    {
        public int Status { get; set; }
        public string Title { get; private set; }

        public Device(string title)
        {
            Title = title;
            Status = 0;
        }

        public int GetCurrentValue()
        {
            return Status;
        }
    }
}
