using Leftover_Harmony.Models;
using Leftover_Harmony.Resources.Components;
using Leftover_Harmony.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Leftover_Harmony
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void ToggleButtonSpinner(ref Button button)
        {
            ContentPresenter content = (ContentPresenter)button.Template.FindName("ContentPresenter", button);
            FrameworkElement spinner = (FrameworkElement)button.Template.FindName("Spinner", button);

            if (content == null || spinner == null) return;

            if (spinner.Visibility == Visibility.Visible)
            {
                spinner.Visibility = Visibility.Hidden;
                content.Visibility = Visibility.Visible;
            }
            else
            {
                spinner.Visibility = Visibility.Visible;
                content.Visibility = Visibility.Hidden;
            }
        }
        /*
        private async void usrLoginButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButtonSpinner(ref usrLoginButton);

            string username = usrUsername.Text;
            string password = usrPassword.Password;

            Trace.WriteLine("Fetching account...");

            User? user = await DataAccessProvider.Instance.FetchUserAsync(username, password);

            if (user == null)
            {
                InvalidLabel.Opacity = 1;
                ToggleButtonSpinner(ref usrLoginButton);
                return;
            }

            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }
        */
    }
}
