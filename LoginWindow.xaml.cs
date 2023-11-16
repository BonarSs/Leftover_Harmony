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
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void usrLoginButton_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement spinner = (FrameworkElement)usrLoginButton.Template.FindName("Spinner", usrLoginButton);

            spinner.Visibility = Visibility.Visible;
            usrLoginButton.Content = "";

            string username = usrUsername.Text;
            string password = usrPassword.Password;

            Trace.WriteLine("Fetching account...");

            User? user = await DataAccessProvider.Instance.FetchUserAsync(username, password);

            if (user == null)
            {
                InvalidLabel.Opacity = 1;
                spinner.Visibility = Visibility.Hidden;
                usrLoginButton.Content = "Login";
                return;
            }

            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }
    }
}
