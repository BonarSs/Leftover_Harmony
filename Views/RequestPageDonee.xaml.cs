using Leftover_Harmony.Helpers;
using Leftover_Harmony.Models;
using Leftover_Harmony.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RequestPage.xaml
    /// </summary>
    public partial class RequestPageDonee : Page
    {
        private Request request;
        private MainWindow _mainWindow;
        private Donee donee;
        public RequestPageDonee(MainWindow mainWindow, Request request, Donee donee)
        {
            InitializeComponent();
            this.request = request;
            this._mainWindow = mainWindow;
            this.donee = donee;
        }

        private void ToggleLeftoverListSpinner()
        {
            if (LeftoverSpinner.Visibility == Visibility.Visible) LeftoverSpinner.Visibility = Visibility.Hidden;
            else LeftoverSpinner.Visibility = Visibility.Visible;
        }
        private void ToggleDoneeSpinner()
        {
            if (DoneeSpinner.Visibility == Visibility.Visible) DoneeSpinner.Visibility = Visibility.Hidden;
            else DoneeSpinner.Visibility = Visibility.Visible;
        }
        private void ToggleButtonSpinner(ref Button button)
        {
            ContentPresenter content = (ContentPresenter)button.Template.FindName("ContentPresenter", button);
            FrameworkElement spinner = (FrameworkElement)button.Template.FindName("Spinner", button);

            if (content == null || spinner == null) return;

            if (spinner.Visibility == Visibility.Visible)
            {
                spinner.Visibility = Visibility.Hidden;
                content.Visibility = Visibility.Visible;
            }
            else
            {
                spinner.Visibility = Visibility.Visible;
                content.Visibility = Visibility.Hidden;
            }
        }
        private void ResetNewLeftoverPanel()
        {
            newlfTitle.Text = "";
            newlfDescription.Text = "";
            newlfAmount.Text = "0";
            ((ImageBrush)newlfImageButton.Template.FindName("newlfImage", newlfImageButton)).ImageSource = null;
            NewLeftoverInvalidTextBox.Visibility = Visibility.Hidden;

            NewLeftoverButton.Visibility = Visibility.Visible;
            NewLeftoverPanel.Visibility = Visibility.Collapsed;
        }
        private async void RefreshRequest()
        {
            request = await DataAccessProvider.Instance.FetchRequestAsync(request.Id);
            LoadRequest();
        }
        private void ClearLeftovers()
        {
            LeftoverList.Children.Clear();
        }
        private async void LoadRequest()
        {
            rqTitle.Text = request.Title;
            rqDescription.Text = request.Description;
            rqDate.Text = rqDate.Text.Replace("{date}", request.Date.ToString("d"));
            if (request.Image != null) rqImage.ImageSource = ImageConverter.ByteArraytoImage(request.Image);

            ToggleLeftoverListSpinner();
            ToggleDoneeSpinner();

            Donee donee = request.Donee;
            dnDisplayName.Text = donee.DisplayName;
            dnOrganization.Text = dnOrganization.Text.Replace("{organization}", donee.Organization);
            if (donee.Image != null) dnImage.ImageSource = ImageConverter.ByteArraytoImage(donee.Image);

            ToggleDoneeSpinner();

            ClearLeftovers();

            List<object> leftovers = await DataAccessProvider.Instance.FetchRequestLeftoverAmountsAsync(request);

            foreach (object item in leftovers)
            {
                Leftover? leftover = (Leftover?)item.GetType()?.GetProperty("Leftover")?.GetValue(item);
                int? requestedAmount = (int?)item.GetType()?.GetProperty("RequestedAmount")?.GetValue(item);
                int? donatedAmount = (int?)item.GetType()?.GetProperty("DonatedAmount")?.GetValue(item);

                Trace.WriteLine(donatedAmount);

                if (leftover == null || requestedAmount == null || donatedAmount == null) continue;

                AddLeftover(leftover, (int)requestedAmount, (int)donatedAmount);
            }

            if ((await request.GetDoneeAsync()).Id == donee.Id) NewLeftoverButton.Visibility = Visibility.Visible;

            ToggleLeftoverListSpinner();
        }
        private void LoadLeftover(ref ContentControl contentControl, Leftover leftover, int requested_amount, int donated_amount)
        {
            TextBlock lfTitle = (TextBlock)contentControl.Template.FindName("lfTitle", contentControl);
            TextBlock lfDescription = (TextBlock)contentControl.Template.FindName("lfDescription", contentControl);
            ImageBrush lfImage = (ImageBrush)contentControl.Template.FindName("lfImage", contentControl);

            TextBlock lfProgressNumerator = (TextBlock)contentControl.Template.FindName("lfProgressNumerator", contentControl);
            TextBlock lfProgressDenominator = (TextBlock)contentControl.Template.FindName("lfProgressDenominator", contentControl);

            Border lfProgressBar = (Border)contentControl.Template.FindName("lfProgressBar", contentControl);
            Border lfDonating = (Border)contentControl.Template.FindName("lfDonating", contentControl);
            Border lfDonated = (Border)contentControl.Template.FindName("lfDonated", contentControl);

            lfTitle.Text = leftover.Name;
            lfDescription.Text = leftover.Description;
            if (leftover.Image != null) lfImage.ImageSource = ImageConverter.ByteArraytoImage(leftover.Image);

            lfProgressNumerator.Text = donated_amount.ToString();
            lfProgressDenominator.Text = requested_amount.ToString();

            float donation_progress = donated_amount / (float)requested_amount;

            lfDonated.Width = lfProgressBar.ActualWidth * donation_progress;
        }
        private void AddLeftover(Leftover leftover, int requested_amount, int donated_amount)
        {
            if (leftover == null) return;

            ContentControl contentControl = new ContentControl();
            contentControl.Margin = new Thickness(0, 0, 0, 16);
            contentControl.Template = (ControlTemplate)FindResource("LeftoverContentTemplate");

            contentControl.Loaded += (sender, e) => { LoadLeftover(ref contentControl, leftover, requested_amount, donated_amount); };

            LeftoverList.Children.Add(contentControl);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRequest();
        }
        private void NewLeftoverButton_Click(object sender, RoutedEventArgs e)
        {
            NewLeftoverButton.Visibility = Visibility.Collapsed;
            NewLeftoverPanel.Visibility = Visibility.Visible;
        }
        private void CancelNewLeftoverButton_Click(object sender, RoutedEventArgs e)
        {
            NewLeftoverButton.Visibility = Visibility.Visible;
            NewLeftoverPanel.Visibility = Visibility.Collapsed;
        }
        private async void AddNewLeftoverButton_Click(object sender, RoutedEventArgs e)
        {
            if (newlfTitle.Text == "" || newlfDescription.Text == "" || newlfAmount.Text == "0")
            {
                NewLeftoverInvalidTextBox.Visibility = Visibility.Visible;
                return;
            }

            ImageSource imagesource = ((ImageBrush)newlfImageButton.Template.FindName("newlfImage", newlfImageButton)).ImageSource;

            byte[]? image = (imagesource == null)? null : ImageConverter.ImageSourcetoByteArray(imagesource);

            ToggleButtonSpinner(ref AddNewLeftoverButton);

            await DataAccessProvider.Instance.AddLeftoverAsync(request.Id, newlfTitle.Text, newlfDescription.Text, int.Parse(newlfAmount.Text), (image != null) ? image : null);

            ToggleButtonSpinner(ref AddNewLeftoverButton);
            ResetNewLeftoverPanel();
            RefreshRequest();
        }
        private void newlfAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
        private void newlfAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (newlfAmount.Text == "") newlfAmount.Text = "0";
            else newlfAmount.Text = Math.Min(int.Parse(newlfAmount.Text.Replace(" ", "")), 9999).ToString();
        }
        private void newlfImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "PNG Images (.png)|*.png";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    BitmapImage transformedBitmap = ImageConverter.ResizeBitmapUniformToFill(ImageConverter.StreamtoImage(dialog.OpenFile()), 500);
                    ImageBrush image = (ImageBrush)newlfImageButton.Template.FindName("newlfImage", newlfImageButton);
                    image.ImageSource = transformedBitmap;
                }
                catch (InvalidOperationException)
                {
                    return;
                }
            }
        }
        private async void DoneeProfile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Donee request_donee = await request.GetDoneeAsync();
            if (request_donee != null) _mainWindow.SwitchPage(request_donee);
        }
    }
}
