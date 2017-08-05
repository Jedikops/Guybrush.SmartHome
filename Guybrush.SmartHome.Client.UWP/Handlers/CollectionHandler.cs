using Guybrush.SmartHome.Client.Data.Base;
using Guybrush.SmartHome.Client.Data.Models;
using Guybrush.SmartHome.Client.UWP.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Core;

namespace Guybrush.SmartHome.Client.UWP.Handlers
{
    public class CollectionHandler<T> where T : Observable
    {

        protected ObservableCollection<T> Collection { get; private set; }
        protected T SelectedItem { get; private set; }
        public CollectionHandler(ObservableCollection<T> collection, T selectedItem)
        {
            Collection = collection;
            SelectedItem = selectedItem;
        }

        public async void Devices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                        {
                            foreach (Device device in e.NewItems.OfType<Device>())
                            {
                                var dev = new DeviceViewModel() { Title = device.Title, Status = Convert.ToInt32(device.Status) } as T;

                                Collection.Add(dev);

                            }

                        }
                        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                        {
                            foreach (Device device in e.OldItems.OfType<Device>())
                            {

                                var deviceVM = Collection.FirstOrDefault(x => (x as DeviceViewModel).Title == device.Title);
                                if (deviceVM != null)
                                    Collection.Remove(deviceVM);
                            }
                        }
                        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
                        {
                            //foreach(Device oldDevice in e.OldItems)
                            for (int i = 0; i < e.OldItems.Count; i++)
                            {
                                var oldDevVM = Collection.FirstOrDefault(x => (x as DeviceViewModel).Title == ((Device)e.OldItems[i]).Title);
                                if (oldDevVM != null)
                                {
                                    var newDev = (Device)e.NewItems[i];
                                    int index = Collection.IndexOf(oldDevVM);

                                    Collection[index] = new DeviceViewModel() { Title = newDev.Title, Status = Convert.ToInt32(newDev.Status) } as T;
                                    SelectedItem = Collection[index];
                                }
                            }

                        }

                    });


        }


    }
}
