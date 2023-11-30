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
    /// Interaction logic for DoneeProfilePage.xaml
    /// </summary>
    public partial class DoneeProfilePage : Page
    {
        MainWindow _mainWindow;
        Donee _account;
        public DoneeProfilePage(MainWindow mainWindow, Donee account)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _account = account;
        }

        private void FormatAccountInfo()
        {
            usrDisplayName.Text = _account.DisplayName;
            usrSubtitle.Text = $"Donee of {_account.Organization}";
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

        private async Task DisplayRecentRequestsAsync()
        {
            usrRecentRequestsSpinner.Visibility = Visibility.Visible;

            List<Request> requests = (await _account.GetRequestsAsync()).OrderByDescending(request => request.Id).ToList();

            Border GetBorder(int index)
            {
                Border border = new Border();
                border.SetValue(Grid.ColumnProperty, index);
                border.CornerRadius = new CornerRadius(10, 10, 10, 10);
                border.Cursor = Cursors.Hand;

                if (index == 0) border.Margin = new Thickness(0, 0, 16, 0);
                else border.Margin = new Thickness(16, 0, 0, 0);

                return border;
            }

            if (requests.Count > 0) NoRequest.Visibility = Visibility.Collapsed;

            for (int i = 0; i < 2; i++)
            {
                if (i >= requests.Count) break;

                Border border = GetBorder(i);
                FormatRequest(requests[i], border);
                usrRecentRequests.Children.Add(border);
            }

            usrRecentRequestsSpinner.Visibility = Visibility.Hidden;
        }

        private void FormatRequest(Request request, Border container)
        {
            ContentControl contentControl = new ContentControl();
            contentControl.Template = (ControlTemplate)FindResource("RequestContentTemplate");

            contentControl.Loaded += (sender, e) =>
            {
                Border rqImage = (Border)contentControl.Template.FindName("rqImage", contentControl);
                TextBlock rqTitle = (TextBlock)contentControl.Template.FindName("rqTitle", contentControl);
                TextBlock rqDescription = (TextBlock)contentControl.Template.FindName("rqDescription", contentControl);
                TextBlock rqDate = (TextBlock)contentControl.Template.FindName("rqDate", contentControl);

                rqTitle.Text = request.Title;
                rqDescription.Text = request.Description;
                rqDate.Text = rqDate.Text.Replace("{date}", request.Date.ToString("dd MMMM yyyy"));
                if (request.Image != null) rqImage.Background = new ImageBrush
                {
                    ImageSource = ImageConverter.ByteArraytoImage(request.Image),
                    Stretch = Stretch.UniformToFill
                };
            };

            container.Child = contentControl;

            container.MouseUp += (sender, e) => { _mainWindow.SwitchPage(request); };
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FormatAccountInfo();
            await DisplayRecentRequestsAsync();
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
