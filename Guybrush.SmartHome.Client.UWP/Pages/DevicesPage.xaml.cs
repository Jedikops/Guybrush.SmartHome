using Guybrush.SmartHome.Client.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Guybrush.SmartHome.Client.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DevicesPage : Page
    {

        public DevicesPage()
        {
            this.InitializeComponent();
            this.Loaded += Devices_Loaded;
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

        }

        private void Devices_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //DataContext = new DevicesViewModel();
            ViewModel = new DevicesViewModel();
            Bindings.Update();
        }

        public DevicesViewModel ViewModel { get; private set; }



    }
}
