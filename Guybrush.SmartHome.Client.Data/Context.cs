using Guybrush.SmartHome.Client.Data.Managers;
using Guybrush.SmartHome.Client.Data.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Guybrush.SmartHome.Client.Data
{
    public class Context
    {
        public static Context Current;

        private ObservableCollection<Device> _devices;
        private ObservableCollection<Reading> _readings;
        //private ObservableCollection<Condition> _conditions;
        private ConditionManager _condMgr;
        private ObservableCollection<User> _users;
        public Dictionary<string, object> Locks { get; private set; }

        public ObservableCollection<Device> Devices
        {
            get { return _devices; }
            private set { _devices = value; }
        }

        public ObservableCollection<Reading> Readings
        {
            get { return _readings; }
            private set { _readings = value; }
        }

        public ConditionManager ConditionManager
        {
            get { return _condMgr; }
        }

        public static void Initialize()
        {
            if (Current == null)
                Current = new Context();
        }

        private Context()
        {
            _devices = new ObservableCollection<Device>();
            _readings = new ObservableCollection<Reading>();
            _condMgr = new ConditionManager();
            _users = new ObservableCollection<User>();
            Locks = new Dictionary<string, object>();

            Locks.Add("Global", new object());
            Locks.Add("Devices", new object());
            Locks.Add("Readings", new object());
            Locks.Add("Conditions", new object());
            Locks.Add("Users", new object());

        }
    }
}
