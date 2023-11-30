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
            InitializeAccountTab();

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

        public void InitializeAccountTab()
        {
            User user = _mainWindow.CurrentUser;
            usrAccountUsername.Text = user.Username;
            usrAccountEmail.Text = user.Email;
            usrAccountPhone.Text = user.PhoneNumber;

            if (user is Donee) newOrganizationForm.Visibility = Visibility.Visible;
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
        private void DisplayErrorMessage(ref TextBlock textbox, string message)
        {
            textbox.Text = message;
            textbox.Visibility = Visibility.Visible;
        }
        private async void UpdateUser()
        {
            if (_mainWindow.CurrentUser is Donee) await DataAccessProvider.Instance.UpdateDoneeAsync((Donee)_mainWindow.CurrentUser);
            else await DataAccessProvider.Instance.UpdateDonorAsync((Donor)_mainWindow.CurrentUser);
        }

        #region ProfileSettings
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

        #endregion


        #region AppearanceSettings
        private void DefaultTheme_Checked(object sender, RoutedEventArgs e)
        {
            App.Instance.ChangeTheme(Theme.Default);
        }

        private void CoffeeTheme_Checked(object sender, RoutedEventArgs e)
        {
            App.Instance.ChangeTheme(Theme.Coffee);
        }

        #endregion

        #region AccountSettings
        private async void newUsernameChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string username = newUsernameTextBox.Text;

            // length check
            if (username.Length == 0) { DisplayErrorMessage(ref newUsernameInvalid, "*Username cannot be empty."); return; }
            if (username.Length < 4 || username.Length > 16) { DisplayErrorMessage(ref newUsernameInvalid, "*Username must be between 4 and 16 characters."); return; }

            ToggleButtonSpinner(ref newUsernameChangeButton);

            bool usernameExists = await DataAccessProvider.Instance.IsUsernameExistsAsync(username);
            if (usernameExists) DisplayErrorMessage(ref newUsernameInvalid, "*Username already exists.");
            else { _mainWindow.CurrentUser.ChangeUsername(username); UpdateUser(); }

            ToggleButtonSpinner(ref newUsernameChangeButton);
        }

        private async void newEmailChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string email = newEmailTextBox.Text;
            
            ToggleButtonSpinner(ref newEmailChangeButton);

            bool emailExists = await DataAccessProvider.Instance.IsEmailExistsAsync(email);

            if (emailExists) DisplayErrorMessage(ref newEmailInvalid, "Email already in use");
            else { _mainWindow.CurrentUser.ChangeEmail(email); UpdateUser(); }

            ToggleButtonSpinner(ref newEmailChangeButton);
        }

        private void newPasswordChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!newPasswordTextBox.Password.Equals(newPasswordConfirmTextBox.Password)) { DisplayErrorMessage(ref newPasswordInvalid, "*Password do not match."); return; }

            string password = newPasswordTextBox.Password;

            ToggleButtonSpinner(ref newPasswordChangeButton);

            _mainWindow.CurrentUser.ChangePassword(password);
            UpdateUser();

            ToggleButtonSpinner(ref newPasswordChangeButton);
        }

        private void newPhoneChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string phone = newPhoneTextBox.Text;

            ToggleButtonSpinner(ref newPhoneChangeButton);

            _mainWindow.CurrentUser.ChangePhoneNumber(phone);
            UpdateUser();

            ToggleButtonSpinner(ref newPhoneChangeButton);
        }

        private void newOrganizationChangeButton_Click(object sender, RoutedEventArgs e)
        {
            
            string organization = newOrganizationTextBox.Text;

            ToggleButtonSpinner(ref newOrganizationChangeButton);

            ((Donee)_mainWindow.CurrentUser).ChangeOrganization(organization);
            UpdateUser();

            ToggleButtonSpinner(ref newOrganizationChangeButton);
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            _mainWindow.Close();
        }
        #endregion
    }
}
