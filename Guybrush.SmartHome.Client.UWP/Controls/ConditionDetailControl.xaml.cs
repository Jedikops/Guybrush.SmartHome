using Guybrush.SmartHome.Client.Data;
using Guybrush.SmartHome.Client.UWP.Pages;
using Guybrush.SmartHome.Client.UWP.ViewModels;
using Guybrush.SmartHome.Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Guybrush.SmartHome.Client.UWP.Controls
{
    public sealed partial class ConditionDetailControl : UserControl
    {
        public ConditionsPage Page { get; internal set; }
        public ConditionUserControlViewModel ViewModel { get; set; }

        public ConditionViewModel Condition
        {
            get { return ViewModel.Condition; }
            set { ViewModel.Condition = value; }
        }

        public ConditionDetailControl()
        {
            this.InitializeComponent();
            ViewModel = new ConditionUserControlViewModel();
            ViewModel.Control = this;
        }

        private void ButtonCancel_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Page.HideControl();
        }

        private async void SourceDevice_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ViewModel.SourceSelectionChanged();
        }


        private async void ButtonDelete_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string source = (string)SourceDevice_ComboBox.SelectedValue;
            string target = (string)TargetDevice_ComboBox.SelectedValue;

            if (Context.Current.ConditionManager.IsConnected)
            {
                await Context.Current.ConditionManager.DeleteCondition(source, target);
                await Page.ViewModel.LoadConditions();
            }
            else
            {
                lock (Context.Current.Locks["Conditions"])
                {
                    Context.Current.ConditionManager.Conditions.Clear();
                    Page.ViewModel.Conditions.Clear();
                }
            }
            await Reload();
        }


        private async void ButtonSave_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            try
            {
                var sourceName = (string)SourceDevice_ComboBox.SelectedValue;
                var targetName = (string)TargetDevice_ComboBox.SelectedValue;

                var sourceDevice = Context.Current.Devices.FirstOrDefault(x => x.Title == sourceName);
                var sourceReader = Context.Current.Readings.FirstOrDefault(x => x.Title == sourceName);

                var targetDevice = Context.Current.Devices.FirstOrDefault(x => x.Title == targetName);
                if (Context.Current.ConditionManager.IsConnected && (sourceDevice != null || sourceReader != null) && targetName != null)
                {

                    DeviceType sourceDeviceType = (sourceDevice != null) ? DeviceType.TurnOnOffDevice : DeviceType.ReaderDevice;



                    int conitionType = (sourceDeviceType == DeviceType.TurnOnOffDevice)
                        ? Convert.ToInt32(((ComboBoxItem)OperatorDevice.SelectedItem).Tag)
                        : Convert.ToInt32(((ComboBoxItem)OperatorReading.SelectedItem).Tag);

                    int requiredValue = (sourceDeviceType == DeviceType.TurnOnOffDevice)
                        ? Convert.ToInt32(((ComboBoxItem)RequiredValueDevice.SelectedItem).Tag)
                        : Convert.ToInt32(RequiredValueReading.Text);


                    int targetValue = Convert.ToInt32(((ComboBoxItem)TargetValueDevice.SelectedItem).Tag);


                    await Context.Current.ConditionManager.AddCondition((int)sourceDeviceType, sourceName, targetName, requiredValue, conitionType, targetValue);
                }
                await Reload();
            }
            catch
            {
                //Validation required
            }
        }

        public async Task Reload()
        {
            await Page.ViewModel.LoadConditions();
            Page.HideControl();
        }
    }
}

