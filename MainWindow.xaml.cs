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

        // database
        // private string connectionString = "Host=floppy.db.elephantsql.com;Username=wnnyslmp;Password=8KGndrH9LfiTFYOp9kzLSRp6NhV67gDf;Database=wnnyslmp";
        private string connectionString = ConfigurationManager.ConnectionStrings["PostgresUri"].ConnectionString;


        public void Log(string message)
        {
            Trace.WriteLine(message);
        }
        
        private void ClearFrame()
        {
            while (MainFrame.NavigationService.RemoveBackEntry() != null) ;
            MainFrame.Content = null;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(User account)
        {
            InitializeComponent();
            _user = account;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataAccessProvider.Instance.Connect(connectionString);
            if (_user == null) _user = DataAccessProvider.Instance.FetchDonor(1);

            HomeButton.IsChecked = true;

            if (_user.Image != null) uiProfilePicture.Fill = new ImageBrush(ImageConverter.ResizeBitmap(ImageConverter.ByteArraytoImage(_user.Image), 0.25));
        }

        private void ProfileButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new DonorProfilePage(this, (Donor)_user));
        }

        private void HomeButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        private void uiProfilePicture_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProfileButton.IsChecked = true;
        }
    }
}
