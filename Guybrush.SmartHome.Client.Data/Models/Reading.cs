using Guybrush.SmartHome.Client.Data.Interface;

namespace Guybrush.SmartHome.Client.Data.Models
{
    public class Reading : IDevice
    {
        public int Value { get; set; }

        public string Title { get; private set; }

        public string Unit { get; private set; }

        public Reading(string title, string unit)
        {
            Title = title;
            Unit = unit;
            Value = 0;
        }

        public int GetCurrentValue()
        {
            return Value;
        }
    }
}
