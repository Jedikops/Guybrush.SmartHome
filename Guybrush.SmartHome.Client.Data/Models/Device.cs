using System;

namespace Guybrush.SmartHome.Client.Data.Models
{


    public class Device
    {
        private bool _status;
        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public Action<int> Method { get; set; }

        public Device()
        {
            Title = "";
            Status = false;
        }

        public int GetCurrentValue()
        {
            return Status ? 1 : 0;
        }

        public void StatusChange(int status)
        {

            Method(status);
            Status = Convert.ToBoolean(status);
        }


    }
}
