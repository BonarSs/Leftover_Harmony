using Leftover_Harmony.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Leftover_Harmony
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App Instance => ((App)Application.Current);
        public Theme ApplicationTheme = Theme.Default;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresUriLocal"].ConnectionString;
            DataAccessProvider.Instance.Connect(connectionString);

            if (!Enum.TryParse(Leftover_Harmony.Properties.Settings.Default.Theme, out Theme theme)) theme = Theme.Default;
            ChangeTheme(theme);
        }

        public enum Theme
        {
            Default,
            Coffee
        }

        public void ChangeTheme(Theme theme)
        {
            ResourceDictionary themeResource = new ResourceDictionary();

            App.Current.Resources.MergedDictionaries.Clear();
            if (theme == Theme.Default) themeResource.Source = new Uri("Resources/Themes/DefaultTheme.xaml", UriKind.Relative);
            else if(theme == Theme.Coffee) themeResource.Source = new Uri("Resources/Themes/CoffeeTheme.xaml", UriKind.Relative);
            
            App.Current.Resources.MergedDictionaries.Add(themeResource);
            ApplicationTheme = theme;

            Leftover_Harmony.Properties.Settings.Default.Theme = theme.ToString();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Leftover_Harmony.Properties.Settings.Default.Save();
        }
    }
}
