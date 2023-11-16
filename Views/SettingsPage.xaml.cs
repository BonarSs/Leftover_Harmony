using Leftover_Harmony.Helpers;
using Leftover_Harmony.Models;
using Leftover_Harmony.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
