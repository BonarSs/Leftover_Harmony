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
    /// Interaction logic for DonationListPage.xaml
    /// </summary>
    public partial class DonationListPage : Page
    {
        MainWindow _mainWindow;
        public DonationListPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private async Task AddDonation(Donation donation)
        {
            Leftover leftover = await DataAccessProvider.Instance.FetchLeftoverAsync(donation);
            Request request = await DataAccessProvider.Instance.FetchRequestAsync(donation);

            ContentControl contentControl = new ContentControl();
            contentControl.Height = Container.ActualHeight / 2;

            contentControl.Template = (ControlTemplate)FindResource("DonationContentTemplate");

            contentControl.Loaded += (sender, e) =>
            {
                TextBlock donationDate = (TextBlock)contentControl.Template.FindName("dnDate", contentControl);
                ImageBrush donationStatus = (ImageBrush)contentControl.Template.FindName("dnStatus", contentControl);
                TextBlock donationDescription = (TextBlock)contentControl.Template.FindName("dnDescription", contentControl);

                TextBlock leftoverName = (TextBlock)contentControl.Template.FindName("lfName", contentControl);
                TextBlock leftoverDescription = (TextBlock)contentControl.Template.FindName("lfDescription", contentControl);
                TextBlock leftoverQuantity = (TextBlock)contentControl.Template.FindName("lfQuantity", contentControl);
                Rectangle leftoverImage = (Rectangle)contentControl.Template.FindName("lfImage", contentControl);

                TextBlock requestName = (TextBlock)contentControl.Template.FindName("rqName", contentControl);
                Rectangle requestImage = (Rectangle)contentControl.Template.FindName("rqImage", contentControl);

                donationDate.Text = donation.DateDonated.ToString("dd MMMM yyyy");
                donationDescription.Text = donation.Description;
                if (donation.Status == DonationStatus.Approved) donationStatus.ImageSource = (DrawingImage)FindResource("check_circle_fill");
                else if (donation.Status == DonationStatus.Pending) donationStatus.ImageSource = (DrawingImage)FindResource("pending_fill");
                else donationStatus.ImageSource = (DrawingImage)FindResource("x_circle_fill");

                leftoverName.Text = leftover.Name;
                leftoverDescription.Text = leftover.Description;
                leftoverQuantity.Text = leftoverQuantity.Text.Replace("{qty}", donation.Amount.ToString());
                if (leftover.Image != null) leftoverImage.Fill = new ImageBrush
                {
                    ImageSource = ImageConverter.ByteArraytoImage(leftover.Image),
                    Stretch = Stretch.UniformToFill
                };

                requestName.Text = request.Title;
                if (request.Image != null) requestImage.Fill = new ImageBrush
                {
                    ImageSource = ImageConverter.ByteArraytoImage(request.Image),
                    Stretch = Stretch.UniformToFill
                };
            };

            contentControl.MouseUp += (sender, e) => { _mainWindow.SwitchPage(request); };

            DonationList.Children.Add(contentControl);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Donation> donations = await DataAccessProvider.Instance.FetchDonorDonationsAsync((Donor)_mainWindow.CurrentUser);
            donations = donations.OrderByDescending(donation => donation.Id).ToList();

            foreach (Donation donation in donations)
            {
                await AddDonation(donation);
            }

            Spinner.Visibility = Visibility.Collapsed;
        }
    }
}
