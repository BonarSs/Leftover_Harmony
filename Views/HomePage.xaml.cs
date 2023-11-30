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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private MainWindow _mainWindow;
        private List<Request> requestList = new List<Request>();

        public HomePage(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private void DisplayRequest(Request request)
        {
            _mainWindow.SwitchPage(request);
        }

        private void AddRequest(Request request)
        {
            if (request == null) return;

            int index = requestList.Count;

            ContentControl contentControl = new ContentControl();
            contentControl.SetValue(Grid.ColumnProperty, index % 3);
            contentControl.SetValue(Grid.RowProperty, index / 3);
            contentControl.Margin = new Thickness(16,16,16,16);

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

            contentControl.MouseUp += (sender, e) => DisplayRequest(request);

            RequestList.Children.Add(contentControl);
            requestList.Add(request);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Request> requests = DataAccessProvider.Instance.FetchAllRequests();
            foreach (Request request in requests)
            {
                this.AddRequest(request);
            }
        }
    }
}
