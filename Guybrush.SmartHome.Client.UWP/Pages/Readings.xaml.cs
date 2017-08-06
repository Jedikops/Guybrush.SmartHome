using Guybrush.SmartHome.Client.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Guybrush.SmartHome.Client.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Readings : Page
    {
        public ReadingsViewModel ViewModel { get; private set; }

        public Readings()
        {
            this.InitializeComponent();
            this.Loaded += Readings_Loaded; ;
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

        }

        private void Readings_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel = new ReadingsViewModel();
            Bindings.Update();
        }
    }
}
