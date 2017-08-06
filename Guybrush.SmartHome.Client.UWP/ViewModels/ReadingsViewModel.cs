using Guybrush.SmartHome.Client.Data;
using Guybrush.SmartHome.Client.UWP.Handlers;
using System.Collections.ObjectModel;

namespace Guybrush.SmartHome.Client.UWP.ViewModels
{
    public class ReadingsViewModel
    {
        public ObservableCollection<ReadingViewModel> Readings { get; private set; }

        public ReadingsViewModel()
        {
            Readings = new ObservableCollection<ReadingViewModel>();

            lock (Context.Current.Locks["Readings"])
            {
                foreach (var reading in Context.Current.Readings)
                {
                    if (reading != null)
                    {
                        var readVM = new ReadingViewModel()
                        {
                            Title = reading.Title,
                            Value = reading.Value,
                            Unit = reading.Unit

                        };
                        Readings.Add(readVM);
                    }
                }

                CollectionHandler<ReadingViewModel> handler = new CollectionHandler<ReadingViewModel>(Readings, null);
                Context.Current.Readings.CollectionChanged += handler.Readings_CollectionChanged;
            }
        }
    }
}
