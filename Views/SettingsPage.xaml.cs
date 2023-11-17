using Leftover_Harmony.Helpers;
using Leftover_Harmony.Models;
using Leftover_Harmony.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Leftover_Harmony.App;

namespace Leftover_Harmony.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private MainWindow _mainWindow;

        public SettingsPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;

            InitializeProfileTab();
            InitializeAppearanceTab();

            ProfileSettingsTab.IsChecked = true;
        }

        public enum Tab
        {
            Profile,
            Appearance,
            Account
        }

        public void SwitchTab(Tab tab)
        {
            ProfileSettingPage.Visibility = Visibility.Hidden;
            AppearanceSettingPage.Visibility = Visibility.Hidden;
            AccountSettingPage.Visibility = Visibility.Hidden;

            if (tab == Tab.Profile) ProfileSettingPage.Visibility = Visibility.Visible;
            if (tab == Tab.Appearance) AppearanceSettingPage.Visibility = Visibility.Visible;
            if (tab == Tab.Account) AccountSettingPage.Visibility = Visibility.Visible;
        }

        public void InitializeProfileTab()
        {
            User user = _mainWindow.CurrentUser;
            
            usrDisplayName.Text = user.DisplayName;
            usrBio.Text = user.Bio;
            if (user.Image != null) usrProfilePicture.Fill = new ImageBrush
            {
                ImageSource = ImageConverter.ByteArraytoImage(user.Image),
                Stretch = Stretch.UniformToFill
            };
            usrEmail.Text = user.Email;
            usrPhoneNumber.Text = user.PhoneNumber;
            if (user is Donee) usrSubtitle.Text = "Donee of " + ((Donee)user).Organization;
            else usrSubtitle.Text = "Donor";

            usrDisplayNameChange.Text = user.DisplayName;
            usrBioChange.Text = user.Bio;
        }

        public void InitializeAppearanceTab()
        {
            Theme theme = App.Instance.ApplicationTheme;

            switch (theme)
            {
                case Theme.Default:
                    DefaultTheme.IsChecked = true;
                    break;
                case Theme.Coffee:
                    CoffeeTheme.IsChecked = true;
                    break;
            }
        }

        private void usrProfilePictureChange_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "PNG Images (.png)|*.png";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    BitmapImage transformedBitmap = ImageConverter.ResizeBitmapUniformToFill(ImageConverter.StreamtoImage(dialog.OpenFile()), 500);
                    _mainWindow.Log(transformedBitmap.PixelWidth + " " + transformedBitmap.PixelHeight);
                    usrProfilePicture.Fill = new ImageBrush
                    {
                        ImageSource = transformedBitmap,
                        Stretch = Stretch.UniformToFill
                    };
                }
                catch (InvalidOperationException)
                {
                    return;
                }
            }
        }

        private void usrDisplayNameChange_TextChanged(object sender, TextChangedEventArgs e)
        {
            usrDisplayName.Text = usrDisplayNameChange.Text;
        }

        private void usrBioChange_TextChanged(object sender, TextChangedEventArgs e)
        {
            usrBio.Text = usrBioChange.Text;
        }

        private void ProfileSettingsTab_Checked(object sender, RoutedEventArgs e)
        {
            SwitchTab(Tab.Profile);
        }

        private void AppearanceSettingsTab_Checked(object sender, RoutedEventArgs e)
        {
            SwitchTab(Tab.Appearance);
        }

        private void AccountSettingsTab_Checked(object sender, RoutedEventArgs e)
        {
            SwitchTab(Tab.Account);
        }

        private async void usrSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement spinner = (FrameworkElement)usrSaveButton.Template.FindName("Spinner", usrSaveButton);

            spinner.Visibility = Visibility.Visible;
            usrSaveButton.Content = "";

            User user = _mainWindow.CurrentUser;
            user.ChangeDisplayName(usrDisplayNameChange.Text);
            user.ChangeBio(usrBioChange.Text);
            user.ChangeImage(ImageConverter.ImageSourcetoByteArray(((ImageBrush)usrProfilePicture.Fill).ImageSource));

            _mainWindow.Refresh();

            if (user is Donor) await DataAccessProvider.Instance.UpdateDonorAsync((Donor)user);
            else await DataAccessProvider.Instance.UpdateDoneeAsync((Donee)user);

            spinner.Visibility = Visibility.Hidden;
            usrSaveButton.Content = "Save";
        }

        private async void newUsernameChangeButton_Click(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            string username = newUsernameTextBox.Text;
            FrameworkElement spinner = (FrameworkElement)newUsernameChangeButton.Template.FindName("Spinner", newUsernameChangeButton);

            // length check
            if (username.Length == 0)
            {
                newUsernameInvalid.Text = "*Username cannot be empty.";
                valid = false;
            }
            if (username.Length < 4 || username.Length > 16)
            {
                newUsernameInvalid.Text = "*Username must be between 4 and 16 characters.";
                valid = false;
            }

            // break before calling data to prevent unnecessary wait
            if (!valid)
            {
                newUsernameInvalid.Visibility = Visibility.Visible;
                return;
            }

            spinner.Visibility = Visibility.Visible;
            newUsernameChangeButton.Content = "";

            bool usernameExists = await DataAccessProvider.Instance.IsUsernameExistsAsync(username);
            if (usernameExists)
            {
                newUsernameInvalid.Text = "*Username already exists.";
                valid = false;
            }

            // modify data if valid
            if (valid)
            {
                _mainWindow.CurrentUser.ChangeUsername(username);

                if (_mainWindow.CurrentUser is Donee) await DataAccessProvider.Instance.UpdateDoneeAsync((Donee)_mainWindow.CurrentUser);
                else await DataAccessProvider.Instance.UpdateDonorAsync((Donor)_mainWindow.CurrentUser);
            }

            spinner.Visibility = Visibility.Hidden;
            newUsernameChangeButton.Content = "Change";
        }

        private void newEmailChangeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void newPasswordChangeButton_Click(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            if (!newPasswordTextBox.Password.Equals(newPasswordConfirmTextBox.Password))
            {
                newPasswordInvalid.Text = "*Password do not match.";
                valid = false;
            }

            // break before calling data to prevent unnecessary wait
            if (!valid)
            {
                newPasswordInvalid.Visibility = Visibility.Visible;
                return;
            }

            string password = newPasswordTextBox.Password;
            FrameworkElement spinner = (FrameworkElement)newPasswordChangeButton.Template.FindName("Spinner", newUsernameChangeButton);

            spinner.Visibility = Visibility.Visible;
            newPasswordChangeButton.Content = "";

            _mainWindow.CurrentUser.ChangePassword(password);

            if (_mainWindow.CurrentUser is Donee) await DataAccessProvider.Instance.UpdateDoneeAsync((Donee)_mainWindow.CurrentUser);
            else await DataAccessProvider.Instance.UpdateDonorAsync((Donor)_mainWindow.CurrentUser);

            spinner.Visibility = Visibility.Hidden;
            newPasswordChangeButton.Content = "Change";
        }

        private void newPhoneChangeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newOrganizationChangeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            _mainWindow.Close();
        }

        private void DefaultTheme_Checked(object sender, RoutedEventArgs e)
        {
            App.Instance.ChangeTheme(Theme.Default);
        }

        private void CoffeeTheme_Checked(object sender, RoutedEventArgs e)
        {
            App.Instance.ChangeTheme(Theme.Coffee);
        }
    }
}
