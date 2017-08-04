using Guybrush.SmartHome.Client.UWP.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Guybrush.SmartHome.Client.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public Shell()
        {
            this.InitializeComponent();

        }


        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
            ResizeOptions();
        }

        ///<summary>
        /// Fix the clipped focus rectangle when SplitView DisplayMode is Compact
        /// </summary>

        private void ResizeOptions()
        {
            // calculate the actual width of the navigation pane

            var width = NavigationPane.CompactPaneLength;
            if (NavigationPane.IsPaneOpen)
                width = NavigationPane.OpenPaneLength;

            // change the width of all control in the navigation pane

            HamburgerButton.Width = width;

            foreach (var option in NavigationMenu.Children)
            {
                var radioButton = (option as RadioButton);
                if (radioButton != null)
                    radioButton.Width = width;
            }
        }

        private void Page_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            FocusNavigationDirection direction = FocusNavigationDirection.None;

            switch (e.Key)
            {
                // both Space and Enter will trigger navigation

                case Windows.System.VirtualKey.Space:
                case Windows.System.VirtualKey.Enter:
                    {
                        var control = FocusManager.GetFocusedElement() as Control;
                        var option = control as RadioButton;
                        if (option != null)
                        {
                            var automation = new RadioButtonAutomationPeer(option);
                            automation.Select();
                        }
                    }
                    return;

                // otherwise, find next focusable element in the appropriate direction

                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                case Windows.System.VirtualKey.GamepadLeftThumbstickLeft:
                case Windows.System.VirtualKey.NavigationLeft:
                    direction = FocusNavigationDirection.Left;
                    break;
                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.GamepadDPadRight:
                case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                case Windows.System.VirtualKey.NavigationRight:
                    direction = FocusNavigationDirection.Right;
                    break;

                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.GamepadDPadUp:
                case Windows.System.VirtualKey.GamepadLeftThumbstickUp:
                case Windows.System.VirtualKey.NavigationUp:
                    direction = FocusNavigationDirection.Up;
                    break;

                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                case Windows.System.VirtualKey.NavigationDown:
                    direction = FocusNavigationDirection.Down;
                    break;
            }

            if (direction != FocusNavigationDirection.None)
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                if (control != null)
                {
                    control.Focus(FocusState.Programmatic);
                    e.Handled = true;
                }
            }
        }

        private void DevicesOption_Checked(object sender, RoutedEventArgs e)
        {
            Content.Navigate(typeof(DevicesPage));
        }

        private void ReadingsOption_Checked(object sender, RoutedEventArgs e)
        {
            Content.Navigate(typeof(Readings));
        }

        private void ConditionsOption_Checked(object sender, RoutedEventArgs e)
        {
            Content.Navigate(typeof(Conditions));
        }

        //private void UsersOption_Checked(object sender, RoutedEventArgs e)
        //{
        //    Content.Navigate(typeof(Devices));
        //}

        private void PermissionsOption_Checked(object sender, RoutedEventArgs e)
        {
            Content.Navigate(typeof(Permissions));
        }
    }
}
