using Leftover_Harmony.Helpers;
using Leftover_Harmony.Models;
using Leftover_Harmony.Services;
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
    /// Interaction logic for NewRequestPage.xaml
    /// </summary>
    public partial class NewRequestPage : Page
    {
        MainWindow _mainWindow;
        public NewRequestPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
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

        private void rqImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "PNG Images (.png)|*.png";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    BitmapImage transformedBitmap = ImageConverter.ResizeBitmapUniformToFill(ImageConverter.StreamtoImage(dialog.OpenFile()), 500);
                    ImageBrush image = (ImageBrush)rqImageButton.Template.FindName("rqImage", rqImageButton);
                    image.ImageSource = transformedBitmap;
                }
                catch (InvalidOperationException)
                {
                    return;
                }
            }
        }

        private async void AddRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (rqTitle.Text == "" || rqDescription.Text == "") { InvalidLabel.Visibility = Visibility.Visible; return; }

            ToggleButtonSpinner(ref AddRequestButton);

            ImageSource imagesource = ((ImageBrush)rqImageButton.Template.FindName("rqImage", rqImageButton)).ImageSource;

            byte[]? image = (imagesource == null) ? null : ImageConverter.ImageSourcetoByteArray(imagesource);

            await DataAccessProvider.Instance.AddRequestAsync(rqTitle.Text, rqDescription.Text, ((Donee)_mainWindow.CurrentUser).Id, (image != null) ? image : null);

            ToggleButtonSpinner(ref AddRequestButton);
            _mainWindow.RequestListPage();
        }
    }
}
