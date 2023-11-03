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
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresUri"].ConnectionString;
            DataAccessProvider.Instance.Connect(connectionString);
        }
    }
}
