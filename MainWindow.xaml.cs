using Leftover_Harmony.Helpers;
using Leftover_Harmony.Models;
using Leftover_Harmony.Services;
using Leftover_Harmony.Views;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        private string connectionString = "Host=localhost;Username=postgres;Password=Munyamunya;Database=Leftover_Harmony";

        private void Log(string message)
        {
            UserConsole.Document.Blocks.Add(new Paragraph(new Run(message)));
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
            _user = DataAccessProvider.Instance.FetchDonor(1);

            // _user.ChangeImage(ResourceHandler.GetResource("Leftover_Harmony.Resources.Images.yae.png"));
            // DataAccessProvider.Instance.UpdateDonor((Donor)_user);

            Log(((Donor)_user).Donations[0].Request.Title);
        }

        private void ProfileButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new DonorProfilePage(this, (Donor)_user));
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
        }
    }
}
