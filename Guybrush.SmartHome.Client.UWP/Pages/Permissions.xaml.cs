using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Guybrush.SmartHome.Client.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Permissions : Page
    {


        public Permissions()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            newUserPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            UsersList.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            if (PasswordBox.Password == PasswordBoxConfirm.Password)
            {

                DevicesListView.Items.Add(new ListViewItem() { Content = UserNameTextBox.Text });
                newUserPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                UsersList.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                ValdationTextBlock.Text = "Passwords does not match.";
                ValdationTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

        }
    }
}
