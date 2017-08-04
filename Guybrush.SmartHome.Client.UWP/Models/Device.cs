using Guybrush.SmartHome.Client.UWP.Base;

namespace Guybrush.SmartHome.Client.UWP.Models
{
    public class Device : Observable
    {
        private int _status;
        public int Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string StatusString
        {
            get { return _status == 1 ? "Enabled" : "Disabled"; }
        }

    }
}
