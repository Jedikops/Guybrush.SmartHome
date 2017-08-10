using Guybrush.SmartHome.Client.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Guybrush.SmartHome.Client.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConditionsPage : Page
    {

        public ConditionsViewModel ViewModel { get; private set; }
        public ConditionsPage()
        {
            this.InitializeComponent();
            ViewModel = new ConditionsViewModel();
            this.Loaded += ConditionsPage_Loaded;
            ConditionControl.Page = this;
        }

        private async void ConditionsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            await ViewModel.LoadConditions();
            ConditionControl.ViewModel.Conditions = ViewModel.Conditions;
            Bindings.Update();
        }

        private void ConditionsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowControl();
            ConditionControl.ViewModel.UpdateSourceOrTargetDeviceIndexes();
        }

        public void ShowControl()
        {
            ViewModel.IsControlActive = true;
        }
        public void HideControl()
        {
            ViewModel.IsControlActive = false;
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ConditionControl.ViewModel.Condition = null;
            ShowControl();
            ConditionControl.ViewModel.UpdateSourceOrTargetDeviceIndexes();
        }
    }
}
