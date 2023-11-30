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
        private Donor donor;
        private Donee? donee;
        public RequestPageDonor(MainWindow mainWindow, Request request, Donor donor)
        {
            InitializeComponent();
            this.request = request;
            this._mainWindow = mainWindow;
            this.donor = donor;
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
        private async Task<bool> RefreshRequest()
        {
            request = await DataAccessProvider.Instance.FetchRequestAsync(request.Id);
            LoadRequest();

            return true;
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

            donee = await request.GetDoneeAsync();
            dnDisplayName.Text = donee.DisplayName;
            dnOrganization.Text = dnOrganization.Text.Replace("{organization}", donee.Organization);
            if (donee.Image != null) dnImage.ImageSource = ImageConverter.ByteArraytoImage(donee.Image);

            ToggleDoneeSpinner();

            ClearLeftovers();

            List<object> leftovers = await DataAccessProvider.Instance.FetchRequestLeftoverAmountsAsync(request);

            if (leftovers.Count > 0)
            {
                LeftoverListPanel.Visibility = Visibility.Visible;
                NoLeftover.Visibility = Visibility.Collapsed;
            }

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
        private void FormatLeftoverTextBox(ref TextBox textBox)
        {
            if (textBox.Text == "") textBox.Text = "0";
            else textBox.Text = int.Parse(textBox.Text.Replace(" ", "")).ToString();
        }
        private void UpdateLeftoverBar(ref ContentControl contentControl)
        {
            if (contentControl == null) return;

            TextBlock lfDonatedValue = (TextBlock)contentControl.Template.FindName("lfDonatedValue", contentControl);
            TextBox lfCounterText = (TextBox)contentControl.Template.FindName("lfCounterText", contentControl);
            TextBlock lfProgressNumerator = (TextBlock)contentControl.Template.FindName("lfProgressNumerator", contentControl);
            TextBlock lfProgressDenominator = (TextBlock)contentControl.Template.FindName("lfProgressDenominator", contentControl);
            Border lfProgressBar = (Border)contentControl.Template.FindName("lfProgressBar", contentControl);
            Border lfDonating = (Border)contentControl.Template.FindName("lfDonating", contentControl);
            Border lfDonated = (Border)contentControl.Template.FindName("lfDonated", contentControl);

            int requested = int.Parse(lfProgressDenominator.Text);
            int donating = (lfCounterText.Text != "") ? int.Parse(lfCounterText.Text) : 0;
            int donated = int.Parse(lfDonatedValue.Text);

            if ((donated + donating) > requested) { donating = requested - donated; lfCounterText.Text = (requested - donated).ToString(); }

            if (donating > 0 && Application.Current.TryFindResource("Info500") is SolidColorBrush info500Brush) lfProgressNumerator.Foreground = info500Brush;
            else if (Application.Current.TryFindResource("DarkColor") is SolidColorBrush darkColorBrush) lfProgressNumerator.Foreground = darkColorBrush;

            lfProgressNumerator.Text = (donated + donating).ToString();

            float donated_progress = donated / (float)requested;
            float donating_progress = (donated + donating) / (float)requested;

            lfDonated.Width = lfProgressBar.ActualWidth * donated_progress;
            lfDonating.Width = lfProgressBar.ActualWidth * donating_progress;
        }
        private void LoadLeftover(ref ContentControl ContentControl, Leftover leftover, int requested_amount, int donated_amount)
        {
            ContentControl contentControl = ContentControl;
            TextBlock lfId = (TextBlock)contentControl.Template.FindName("lfId", contentControl);
            TextBlock lfTitle = (TextBlock)contentControl.Template.FindName("lfTitle", contentControl);
            TextBlock lfDescription = (TextBlock)contentControl.Template.FindName("lfDescription", contentControl);
            ImageBrush lfImage = (ImageBrush)contentControl.Template.FindName("lfImage", contentControl);

            TextBlock lfDonatedValue = (TextBlock)contentControl.Template.FindName("lfDonatedValue", contentControl);
            TextBox lfCounterText = (TextBox)contentControl.Template.FindName("lfCounterText", contentControl);
            Button lfCounterAdd = (Button)contentControl.Template.FindName("lfCounterAdd", contentControl);
            Button lfCounterSubtract = (Button)contentControl.Template.FindName("lfCounterSubtract", contentControl);

            TextBlock lfProgressNumerator = (TextBlock)contentControl.Template.FindName("lfProgressNumerator", contentControl);
            TextBlock lfProgressDenominator = (TextBlock)contentControl.Template.FindName("lfProgressDenominator", contentControl);

            Border lfProgressBar = (Border)contentControl.Template.FindName("lfProgressBar", contentControl);
            Border lfDonating = (Border)contentControl.Template.FindName("lfDonating", contentControl);
            Border lfDonated = (Border)contentControl.Template.FindName("lfDonated", contentControl);

            lfId.Text = leftover.Id.ToString();
            lfTitle.Text = leftover.Name;
            lfDescription.Text = leftover.Description;
            if (leftover.Image != null) lfImage.ImageSource = ImageConverter.ByteArraytoImage(leftover.Image);

            lfDonatedValue.Text = donated_amount.ToString();
            lfProgressNumerator.Text = donated_amount.ToString();
            lfProgressDenominator.Text = requested_amount.ToString();

            UpdateLeftoverBar(ref contentControl);

            lfCounterText.PreviewTextInput += CounterPreviewTextInput;
            lfCounterText.LostFocus += (sender, e) => { FormatLeftoverTextBox(ref lfCounterText); UpdateLeftoverBar(ref contentControl); };
            lfCounterText.TextChanged += (sender, e) => { if (!lfCounterText.IsFocused) UpdateLeftoverBar(ref contentControl); };

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
        private async void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButtonSpinner(ref DonateButton);

            foreach (var contentControl in LeftoverList.Children)
            {
                if (contentControl is ContentControl leftover)
                {
                    TextBlock lfId = (TextBlock)leftover.Template.FindName("lfId", leftover);
                    TextBox lfCounterText = (TextBox)leftover.Template.FindName("lfCounterText", leftover);

                    if (int.Parse(lfCounterText.Text) > 0)
                    {
                        await DataAccessProvider.Instance.AddDonationAsync(int.Parse(lfId.Text), donor.Id, request.Id, int.Parse(lfCounterText.Text));
                    }
                }
            }

            await RefreshRequest();

            ToggleButtonSpinner(ref DonateButton);
        }
        private void DoneeProfile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (donee != null) _mainWindow.SwitchPage(donee);
        }
    }
}
