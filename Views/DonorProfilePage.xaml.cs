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
            if (_account.Image != null) usrProfilePicture.ImageSource = ImageConverter.ByteArraytoImage(_account.Image);
        }

        private void displayRecentDonations()
        {
            if (_account.Donations.Count == 0) return;
            if (_account.Donations[0] != null)
            {

                // content control
                ContentControl contentControl = new ContentControl();
                contentControl.Template = (ControlTemplate)FindResource("DonationContentTemplate");

                // grid
                Grid grid = new Grid();
                grid.Margin = new Thickness(0,0,16,0);
                grid.SetValue(Grid.ColumnProperty, 0);
                grid.Children.Add(contentControl);

                usrRecentDonations.Children.Add(grid);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            formatData();
        }
    }
}
