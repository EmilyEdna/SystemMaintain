using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace System.Maintain
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0 && e.Args.FirstOrDefault().Equals("true"))
                this.StartupUri = new Uri("DataWindow.xaml", UriKind.Relative);
            else
                this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            base.OnStartup(e);
        }
    }
}
