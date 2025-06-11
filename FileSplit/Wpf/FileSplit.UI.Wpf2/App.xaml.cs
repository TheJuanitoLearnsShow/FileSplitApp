using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FileSplit.UI.Wpf2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string filePath = null;
            if (e.Args != null && e.Args.Length > 0 && System.IO.File.Exists(e.Args[0]))
            {
                filePath = e.Args[0];
            }
            var mainWindow = new MainWindow();
            if (!string.IsNullOrEmpty(filePath))
            {
                mainWindow.LoadFileFromPath(filePath);
            }
            mainWindow.Show();
        }
    }
}
