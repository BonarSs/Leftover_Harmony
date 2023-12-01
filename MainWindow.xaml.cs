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
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
        public User CurrentUser { get { return _user; } }
        private bool searching = false;

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

        /// <summary>
        /// Refreshes user image
        /// </summary>
        public void Refresh()
        {
            if (_user.Image != null) uiProfilePicture.Fill = new ImageBrush
            {
                ImageSource = Helpers.ImageConverter.ResizeBitmapUniformToFill(Helpers.ImageConverter.ByteArraytoImage(_user.Image), 50),
                Stretch = Stretch.UniformToFill
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HomeButton.IsChecked = true;

            Refresh();

            CustomQueries();

            MainFrame.NavigationService.Navigate(new HomePage(this));

            if (_user is Donor) RequestsButton.Visibility = Visibility.Collapsed;
            else DonationsButton.Visibility = Visibility.Collapsed;
        }

        private void ResetSidebar()
        {
            HomeButton.IsChecked = false;
            RequestsButton.IsChecked = false;
            DonationsButton.IsChecked = false;
            ProfileButton.IsChecked = false;
            SettingsButton.IsChecked = false;
        }
        private async Task AddSearchResult(string type, int id)
        {
            ContentControl contentControl = new ContentControl();
            contentControl.Template = (ControlTemplate)FindResource("SearchResultTemplate");

            string resultType = "";
            string resultName = "";
            SolidColorBrush? solidBrush = null;

            if (type == "donor")
            {
                Donor donor = await DataAccessProvider.Instance.FetchDonorAsync(id);
                resultType = "Donor";
                if (Application.Current.TryFindResource("Success700") is SolidColorBrush brush) solidBrush = brush;
                resultName = (donor.DisplayName != null)? donor.DisplayName : donor.Username;

                contentControl.MouseUp += (sender, e) => { SwitchPage(donor); };
            }
            else if (type == "donee")
            {
                Donee donee = await DataAccessProvider.Instance.FetchDoneeAsync(id);
                resultType = "Donee";
                if (Application.Current.TryFindResource("Info700") is SolidColorBrush brush) solidBrush = brush;
                resultName = (donee.DisplayName != null) ? donee.DisplayName : donee.Username;

                contentControl.MouseUp += (sender, e) => { SwitchPage(donee); };
            }
            else if (type == "leftover")
            {
                Leftover leftover = await DataAccessProvider.Instance.FetchLeftoverAsync(id);
                resultType = "Leftover";
                if (Application.Current.TryFindResource("Neutral700") is SolidColorBrush brush) solidBrush = brush;
                resultName = leftover.Name;

                Request leftover_request = await DataAccessProvider.Instance.FetchRequestAsync(leftover);

                contentControl.MouseUp += (sender, e) => { SwitchPage(leftover_request); };
            }
            else
            {
                Request request = await DataAccessProvider.Instance.FetchRequestAsync(id);
                resultType = "Request";
                if (Application.Current.TryFindResource("Danger700") is SolidColorBrush brush) solidBrush = brush;
                resultName = request.Title;

                contentControl.MouseUp += (sender, e) => { SwitchPage(request); };
            }

            contentControl.Loaded += (sender, e) =>
            {
                TextBlock ResultType = (TextBlock)contentControl.Template.FindName("ResultType", contentControl);
                TextBlock ResultName = (TextBlock)contentControl.Template.FindName("ResultName", contentControl);

                ResultType.Text = resultType;
                ResultName.Text = resultName;

                if (solidBrush != null) ResultType.Foreground = solidBrush;
            };

            SearchResults.Children.Add(contentControl);
        }
        private async void Search(string query)
        {
            SearchPanel.Visibility = Visibility.Visible;

            SearchResults.Children.Clear();

            List<(string, int)> results = await DataAccessProvider.Instance.GlobalSearchAsync(query);
            
            foreach ((string, int) result in results)
            {
                await AddSearchResult(result.Item1, result.Item2);
            }
        }

        private void ProfileButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
            if (_user is Donor) MainFrame.NavigationService.Navigate(new DonorProfilePage(this, (Donor)_user));
            else MainFrame.NavigationService.Navigate(new DoneeProfilePage(this, (Donee)_user));
        }

        private void HomeButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new HomePage(this));
        }

        private void SettingsButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new SettingsPage(this));
        }
        private void RequestsButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new RequestListPage(this));
        }
        private void DonationsButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new DonationListPage(this));
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
            SearchPanel.Visibility = Visibility.Collapsed;
        }

        private void uiProfilePicture_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProfileButton.IsChecked = true;
        }
        private void uiSearchBar_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) Search(uiSearchBar.Text);
        }


        public bool IsCurrentUser(User user)
        {
            return _user == user;
        }

        /// <summary>
        /// Switches to Request Page
        /// </summary>
        /// <param name="request"></param>
        public void SwitchPage(Request request)
        {
            ClearFrame();
            if (_user is Donor) MainFrame.NavigationService.Navigate(new RequestPageDonor(this, request, (Donor)_user));
            else if (_user is Donee) MainFrame.NavigationService.Navigate(new RequestPageDonee(this, request, (Donee)_user));
            ResetSidebar();
            SearchPanel.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Switches to Donee Page
        /// </summary>
        /// <param name="donee"></param>
        public void SwitchPage(Donee donee)
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new DoneeProfilePage(this, donee));
            ResetSidebar();
            SearchPanel.Visibility = Visibility.Collapsed;
        }
        public void SwitchPage(Donor donor)
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new DonorProfilePage(this, donor));
            ResetSidebar();
            SearchPanel.Visibility = Visibility.Collapsed;
        }
        public void NewRequestPage()
        {
            ClearFrame();
            MainFrame.NavigationService.Navigate(new NewRequestPage(this));
            ResetSidebar();
        }
        public void RequestListPage()
        {
            RequestsButton.IsChecked = true;
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

            _user.ChangeImage(ResourceHandler.GetResource("Leftover_Harmony.Resources.Images.yae.png"));
            DataAccessProvider.Instance.UpdateDonee((Donee)_user);
            */

        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        
    }
}
