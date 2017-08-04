using Guybrush.SmartHome.Client.Data.Models;
using System.Collections.ObjectModel;

namespace Guybrush.SmartHome.Client.Data
{
    public class SmartHome
    {
        public ObservableCollection<Device> Devices;
        public ObservableCollection<Reading> Readings;
        public ObservableCollection<Condition> DeviceConditions;
        public ObservableCollection<User> Users;

        public SmartHome()
        {

        }

    }
}
