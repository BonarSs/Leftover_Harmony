using Leftover_Harmony.Helpers;
using Leftover_Harmony.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Leftover_Harmony.Views
{
    /// <summary>
    /// Interaction logic for DonorProfilePage.xaml
    /// </summary>
    public partial class DonorProfilePage : Page
    {
        MainWindow _mainWindow;
        Donor _account;
        public DonorProfilePage(MainWindow mainWindow, Donor account)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _account = account;
        }

        private void FormatAccountInfo()
        {
            usrDisplayName.Text = _account.DisplayName;
            usrBio.Text = _account.Bio;
            usrEmail.Text = _account.Email;
            usrPhoneNumber.Text = _account.PhoneNumber;
            if (_account.Image != null)
            {
                BitmapImage image = ImageConverter.ByteArraytoImage(_account.Image);
                usrProfilePicture.Fill = new ImageBrush
                {
                    ImageSource = image,
                    Stretch = Stretch.UniformToFill
                };
                usrProfilePicture.Cursor = Cursors.Hand;
                usrProfilePictureDisplayImage.Source = image;
            }
        }

        private void DisplayRecentDonations()
        {
            usrRecentDonationsSpinner.Visibility = Visibility.Visible;

            List<Donation> donations = _account.Donations.OrderByDescending(donation => donation.Id).ToList();
            if (donations.Count == 0) return;

            Border GetBorder(int index)
            {
                Border border = new Border();
                border.SetValue(Grid.ColumnProperty, index);
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(1, 1, 1, 1);
                border.CornerRadius = new CornerRadius(10, 10, 10, 10);

                if (index == 0) border.Margin = new Thickness(0, 0, 16, 0);
                else if (index == 1) border.Margin = new Thickness(8, 0, 8, 0);
                else border.Margin = new Thickness(16, 0, 0, 0);

                return border;
            }

            for (int i = 0; i < 3; i++)
            {
                if (donations[i] == null) break;

                Border border = GetBorder(i);
                FormatDonation(donations[i], border);
                usrRecentDonations.Children.Add(border);
            }

            usrRecentDonationsSpinner.Visibility = Visibility.Hidden;
        }

        private async Task DisplayRecentDonationsAsync()
        {
            usrRecentDonationsSpinner.Visibility = Visibility.Visible;

            List<Donation> donations = (await _account.GetDonationsAsync()).OrderByDescending(donation => donation.Id).ToList();

            Border GetBorder(int index)
            {
                Border border = new Border();
                border.SetValue(Grid.ColumnProperty, index);
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(1, 1, 1, 1);
                border.CornerRadius = new CornerRadius(10, 10, 10, 10);

                if (index == 0) border.Margin = new Thickness(0, 0, 16, 0);
                else if (index == 1) border.Margin = new Thickness(8, 0, 8, 0);
                else border.Margin = new Thickness(16, 0, 0, 0);

                return border;
            }

            if (donations.Count > 0) NoDonation.Visibility = Visibility.Collapsed;

            for (int i = 0; i < 3; i++)
            {
                if (donations.Count == i) break;

                Border border = GetBorder(i);
                FormatDonation(donations[i], border);
                usrRecentDonations.Children.Add(border);
            }

            usrRecentDonationsSpinner.Visibility = Visibility.Hidden;
        }

        private void FormatDonation(Donation donation, Border container)
        {
            Leftover leftover = donation.Leftover;
            Request request = donation.Request;

            ContentControl contentControl = new ContentControl();
            contentControl.Template = (ControlTemplate)FindResource("DonationContentTemplate");

            contentControl.Loaded += (sender, e) =>
            {
                TextBlock donationDate = (TextBlock)contentControl.Template.FindName("dnDate", contentControl);
                Image donationStatus = (Image)contentControl.Template.FindName("dnStatus", contentControl);
                TextBlock donationDescription = (TextBlock)contentControl.Template.FindName("dnDescription", contentControl);

                TextBlock leftoverName = (TextBlock)contentControl.Template.FindName("lfName", contentControl);
                TextBlock leftoverDescription = (TextBlock)contentControl.Template.FindName("lfDescription", contentControl);
                TextBlock leftoverQuantity = (TextBlock)contentControl.Template.FindName("lfQuantity", contentControl);
                Rectangle leftoverImage = (Rectangle)contentControl.Template.FindName("lfImage", contentControl);

                TextBlock requestName = (TextBlock)contentControl.Template.FindName("rqName", contentControl);
                Rectangle requestImage = (Rectangle)contentControl.Template.FindName("rqImage", contentControl);

                donationDate.Text = donation.DateDonated.ToString("dd MMMM yyyy");
                donationDescription.Text = donation.Description;
                if (donation.Status == DonationStatus.Approved) donationStatus.Source = (DrawingImage)FindResource("check_circle_fill");
                else if (donation.Status == DonationStatus.Pending) donationStatus.Source = (DrawingImage)FindResource("pending_fill");
                else donationStatus.Source = (DrawingImage)FindResource("x_circle_fill");

                leftoverName.Text = leftover.Name;
                leftoverDescription.Text = leftover.Description;
                leftoverQuantity.Text = leftoverQuantity.Text.Replace("{qty}", donation.Amount.ToString());
                if (leftover.Image != null) leftoverImage.Fill = new ImageBrush { 
                    ImageSource = ImageConverter.ByteArraytoImage(leftover.Image), 
                    Stretch = Stretch.UniformToFill
                };

                requestName.Text = request.Title;
                if (request.Image != null) requestImage.Fill = new ImageBrush { 
                    ImageSource = ImageConverter.ByteArraytoImage(request.Image), 
                    Stretch = Stretch.UniformToFill 
                };
            };

            container.Child = contentControl;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FormatAccountInfo();
            await DisplayRecentDonationsAsync();
        }

        private void usrProfilePicture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            usrProfilePictureDisplay.Visibility = Visibility.Visible;
        }

        private void usrProfilePictureDisplayBackground_MouseDown(object sender, MouseButtonEventArgs e)
        {
            usrProfilePictureDisplay.Visibility = Visibility.Hidden;
        }
    }
}
