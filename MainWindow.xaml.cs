using Leftover_Harmony.Helpers;
using Leftover_Harmony.Models;
using Leftover_Harmony.Services;
using Leftover_Harmony.Views;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

namespace Leftover_Harmony
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _user;

        public void Log(string message)
        {
            Trace.WriteLine(message);
        }
        
        private void ClearFrame()
        {
            while (MainFrame.NavigationService.RemoveBackEntry() != null) ;
            MainFrame.Content = null;
        }

        public MainWindow(User account)
        {
            InitializeComponent();
            _user = account;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (_user == null) _user = DataAccessProvider.Instance.FetchDonor(1);

            HomeButton.IsChecked = true;

            if (_user.Image != null) uiProfilePicture.Fill = new ImageBrush
            {
                ImageSource = ImageConverter.ResizeBitmap(ImageConverter.ByteArraytoImage(_user.Image), 0.25),
                Stretch = Stretch.UniformToFill
            };

            CustomQueries();

            MainFrame.NavigationService.Navigate(new HomePage());
        }

        private void ProfileButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new DonorProfilePage(this, (Donor)_user));
        }

        private void HomeButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new HomePage());
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        private void uiProfilePicture_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProfileButton.IsChecked = true;
        }

        private void CustomQueries()
        {
            /*
            Leftover leftover1 = DataAccessProvider.Instance.FetchLeftover(1);
            Leftover leftover2 = DataAccessProvider.Instance.FetchLeftover(2);
            Leftover leftover3 = DataAccessProvider.Instance.FetchLeftover(3);

            leftover1.SetImage(ResourceHandler.GetResource("Leftover_Harmony.Resources.Images.roti.jpg"));
            leftover2.SetImage(ResourceHandler.GetResource("Leftover_Harmony.Resources.Images.telur.jpg"));
            leftover3.SetImage(ResourceHandler.GetResource("Leftover_Harmony.Resources.Images.tepung.jpg"));

            DataAccessProvider.Instance.UpdateLeftover(leftover1);
            DataAccessProvider.Instance.UpdateLeftover(leftover2);
            DataAccessProvider.Instance.UpdateLeftover(leftover3);
            

            Request request1 = DataAccessProvider.Instance.FetchRequest(1);
            Request request2 = DataAccessProvider.Instance.FetchRequest(2);

            request1.SetImage(ResourceHandler.GetResource("Leftover_Harmony.Resources.Images.bahan-pangan.jpg"));
            request2.SetImage(ResourceHandler.GetResource("Leftover_Harmony.Resources.Images.pakaian.jpg"));

            DataAccessProvider.Instance.UpdateRequest(request1);
            DataAccessProvider.Instance.UpdateRequest(request2);
            
            _user.ChangeImage(ResourceHandler.GetResource("Leftover_Harmony.Resources.Images.rehan.png"));
            DataAccessProvider.Instance.UpdateDonor((Donor)_user);
            */

        }
    }
}
