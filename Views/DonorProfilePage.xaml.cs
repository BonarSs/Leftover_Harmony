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

        private void formatData()
        {
            usrDisplayName.Text = _account.DisplayName;
            usrBio.Text = _account.Bio;
            usrEmail.Text = _account.Email;
            usrPhoneNumber.Text = _account.PhoneNumber;
            if (_account.Image != null) usrProfilePicture.Fill = new ImageBrush(ImageConverter.ByteArraytoImage(_account.Image));
        }

        private void displayRecentDonations()
        {
            List<Donation> donations = _account.Donations.OrderByDescending(donation => donation.Id).ToList();
            if (donations.Count == 0) return;
            if (donations[0] != null)
            {
                Border border = new Border();
                border.Margin = new Thickness(0,0,16,0);
                border.SetValue(Grid.ColumnProperty, 0);
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(1, 1, 1, 1);
                border.CornerRadius = new CornerRadius(10, 10, 10, 10);

                formatDonation(donations[0], border);

                usrRecentDonations.Children.Add(border);
            }
            if (donations[1] != null)
            {
                Border border = new Border();
                border.Margin = new Thickness(8, 0, 8, 0);
                border.SetValue(Grid.ColumnProperty, 1);
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(1, 1, 1, 1);
                border.CornerRadius = new CornerRadius(10, 10, 10, 10);

                formatDonation(donations[1], border);

                usrRecentDonations.Children.Add(border);
            }
            if (donations[2] != null)
            {
                Border border = new Border();
                border.Margin = new Thickness(16, 0, 0, 0);
                border.SetValue(Grid.ColumnProperty, 2);
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(1, 1, 1, 1);
                border.CornerRadius = new CornerRadius(10, 10, 10, 10);

                formatDonation(donations[2], border);

                usrRecentDonations.Children.Add(border);
            }
        }

        private void formatDonation(Donation donation, Border container)
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
                if (leftover.Image != null) leftoverImage.Fill = new ImageBrush(ImageConverter.ByteArraytoImage(leftover.Image));

                requestName.Text = request.Title;
                if (request.Image != null) requestImage.Fill = new ImageBrush(ImageConverter.ByteArraytoImage(request.Image));
            };

            container.Child = contentControl;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            formatData();
            displayRecentDonations();
        }
    }
}
