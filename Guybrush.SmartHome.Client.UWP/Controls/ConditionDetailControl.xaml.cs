using Guybrush.SmartHome.Client.Data;
using Guybrush.SmartHome.Client.UWP.Pages;
using Guybrush.SmartHome.Client.UWP.ViewModels;
using Guybrush.SmartHome.Shared.Enums;
using System;
using System.Linq;
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
        }

        private void ButtonCancel_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Page.HideControl();
        }

        private void SourceDevice_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SourceSelectionChanged();
        }


        private async void ButtonDelete_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string source = (string)SourceDevice_ComboBox.SelectedValue;
            string target = (string)TargetDevice_ComboBox.SelectedValue;


            await Context.Current.ConditionManager.DeleteCondition(source, target);
            await Page.ViewModel.LoadConditions();
            Page.HideControl();
        }


        private async void ButtonSave_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            try
            {

                var sourceName = (string)SourceDevice_ComboBox.SelectedValue;
                var device = Context.Current.Devices.FirstOrDefault(x => x.Title == sourceName);
                DeviceType sourceDeviceType = (device != null) ? DeviceType.TurnOnOffDevice : DeviceType.ReaderDevice;

                var targetName = (string)TargetDevice_ComboBox.SelectedValue;

                int conitionType = (sourceDeviceType == DeviceType.TurnOnOffDevice)
                    ? Convert.ToInt32(((ComboBoxItem)OperatorDevice.SelectedItem).Tag)
                    : Convert.ToInt32(((ComboBoxItem)OperatorReading.SelectedItem).Tag);

                int requiredValue = (sourceDeviceType == DeviceType.TurnOnOffDevice)
                    ? Convert.ToInt32(((ComboBoxItem)RequiredValueDevice.SelectedItem).Tag)
                    : Convert.ToInt32(RequiredValueReading.Text);


                int targetValue = Convert.ToInt32(((ComboBoxItem)TargetValueDevice.SelectedItem).Tag);


                await Context.Current.ConditionManager.AddCondition((int)sourceDeviceType, sourceName, targetName, requiredValue, conitionType, targetValue);
                await Page.ViewModel.LoadConditions();
                Page.HideControl();
            }
            catch
            {
                //Validation required
            }
        }
    }
}

