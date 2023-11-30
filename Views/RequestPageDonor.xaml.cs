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
    public partial class RequestPageDonor : Page
    {
        private Request request;
        private MainWindow _mainWindow;
        public RequestPageDonor(MainWindow mainWindow, Request request)
        {
            InitializeComponent();
            this.request = request;
            this._mainWindow = mainWindow;
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
            rqImage.ImageSource = ImageConverter.ByteArraytoImage(request.Image);

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

                if (leftover == null || requestedAmount == null || donatedAmount == null) continue;

                AddLeftover(leftover, (int)requestedAmount, (int)donatedAmount);
            }

            ToggleLeftoverListSpinner();
        }
        private void CounterPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
        private void UpdateLeftoverBar(ref ContentControl contentControl)
        {
            if (contentControl == null) return;

            TextBox lfCounterText = (TextBox)contentControl.Template.FindName("lfCounterText", contentControl);
            TextBlock lfProgressNumerator = (TextBlock)contentControl.Template.FindName("lfProgressNumerator", contentControl);
            TextBlock lfProgressDenominator = (TextBlock)contentControl.Template.FindName("lfProgressDenominator", contentControl);
            Border lfProgressBar = (Border)contentControl.Template.FindName("lfProgressBar", contentControl);
            Border lfDonating = (Border)contentControl.Template.FindName("lfDonating", contentControl);
            Border lfDonated = (Border)contentControl.Template.FindName("lfDonated", contentControl);

            int requested = int.Parse(lfProgressDenominator.Text);
            int donated = int.Parse(lfProgressNumerator.Text);
            int donating = int.Parse(lfCounterText.Text);

            if (donating > 0 && Application.Current.TryFindResource("Info500") is SolidColorBrush info500Brush) lfProgressNumerator.Foreground = info500Brush;
        }
        private void LoadLeftover(ref ContentControl contentControl, Leftover leftover, int requested_amount, int donated_amount)
        {
            TextBlock lfTitle = (TextBlock)contentControl.Template.FindName("lfTitle", contentControl);
            TextBlock lfDescription = (TextBlock)contentControl.Template.FindName("lfDescription", contentControl);
            ImageBrush lfImage = (ImageBrush)contentControl.Template.FindName("lfImage", contentControl);

            TextBox lfCounterText = (TextBox)contentControl.Template.FindName("lfCounterText", contentControl);
            Button lfCounterAdd = (Button)contentControl.Template.FindName("lfCounterAdd", contentControl);
            Button lfCounterSubtract = (Button)contentControl.Template.FindName("lfCounterSubtract", contentControl);

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

            lfCounterText.PreviewTextInput += CounterPreviewTextInput;
            lfCounterText.LostFocus += (sender, e) => { lfCounterText.Text = int.Parse(lfCounterText.Text.Replace(" ", "")).ToString(); };

            lfCounterAdd.Click += (sender, e) => { lfCounterText.Text = (int.Parse(lfCounterText.Text.Replace(" ", "")) + 1).ToString(); };
            lfCounterSubtract.Click += (sender, e) => { lfCounterText.Text = Math.Max(int.Parse(lfCounterText.Text.Replace(" ", "")) - 1, 0).ToString(); };
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
    }
}
