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
        private void ShowInvalid(ref TextBlock textBlock, string message)
        {
            textBlock.Text = message;
            textBlock.Visibility = Visibility.Visible;
        }
        private void ResetSignUp()
        {
            newUsername.Text = "";
            newEmail.Text = "";
            newPassword.Password = "";
            newPhone.Text = "";
            newOrganization.Text = "";
            SignUpInvalidLabel.Visibility = Visibility.Hidden;
        }

        private async void usrLoginButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButtonSpinner(ref usrLoginButton);

            string username = usrUsername.Text;
            string password = usrPassword.Password;

            Trace.WriteLine("Fetching account...");

            User? user = await DataAccessProvider.Instance.FetchUserAsync(username, password);

            if (user == null)
            {
                ShowInvalid(ref LoginInvalidLabel, "*Username or password is incorrect");
                ToggleButtonSpinner(ref usrLoginButton);
                return;
            }

            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }

        private async void usrSignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (newUsername.Text == "" || newEmail.Text == "" || newPassword.Password == "" || newPhone.Text == "")
            {
                ShowInvalid(ref SignUpInvalidLabel, "*Please fill all fields.");
                return;
            }
            if (OrganizationPanel.Visibility == Visibility.Visible && newOrganization.Text == "")
            {
                ShowInvalid(ref SignUpInvalidLabel, "*Please fill all fields.");
                return;
            }

            ToggleButtonSpinner(ref usrSignUpButton);

            bool usernameExists = await DataAccessProvider.Instance.IsUsernameExistsAsync(newUsername.Text);

            if (usernameExists) { ShowInvalid(ref SignUpInvalidLabel, "*Username already exists."); }
            else if (OrganizationPanel.Visibility == Visibility.Visible) await DataAccessProvider.Instance.AddDoneeAsync(newUsername.Text, newEmail.Text, newPassword.Password, newPhone.Text, newOrganization.Text);
            else await DataAccessProvider.Instance.AddDonorAsync(newUsername.Text, newEmail.Text, newPassword.Password, newPhone.Text);

            ToggleButtonSpinner(ref usrSignUpButton);
            SwitchToLoginButton_Click(sender, e);
        }

        private void SwitchToSignUpButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RoleSelectPanel.Visibility = Visibility.Collapsed;
            SignUpPanel.Visibility = Visibility.Visible;
        }
        private void SwitchToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Visible;
            RoleSelectPanel.Visibility = Visibility.Collapsed;
            SignUpPanel.Visibility = Visibility.Collapsed;
            ResetSignUp();
        }

        private void SwitchToRoleSelectButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RoleSelectPanel.Visibility = Visibility.Visible;
            SignUpPanel.Visibility = Visibility.Collapsed;
        }

        private void SetRoleDonor_Checked(object sender, RoutedEventArgs e)
        {
            SwitchToSignUpButton.Visibility = Visibility.Visible;
            OrganizationPanel.Visibility = Visibility.Collapsed;
        }

        private void SetRoleDonee_Checked(object sender, RoutedEventArgs e)
        {
            SwitchToSignUpButton.Visibility = Visibility.Visible;
            OrganizationPanel.Visibility = Visibility.Visible;
        }
    }
}
