using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Guybrush.SmartHome.Client.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (UserNameTextBox.Text.ToLower() == "andrew" && PasswordBox.Password == "asd")
            {
                Frame.Navigate(typeof(Shell));
            }
            else
            {
                ValdationTextBlock.Text = "Username or password is invalid. Try again.";
                ValdationTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            loginPanel.Visibility = Visibility.Visible;
            ConnectionPanel.Visibility = Visibility.Collapsed;
        }
    }
}
