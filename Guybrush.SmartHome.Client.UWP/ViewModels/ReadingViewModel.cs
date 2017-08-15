using Guybrush.SmartHome.Shared;

namespace Guybrush.SmartHome.Client.UWP.ViewModels
{
    public class ReadingViewModel : Observable
    {
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

        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        private string _unit;

        public string Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                OnPropertyChanged();
            }
        }


    }
}
