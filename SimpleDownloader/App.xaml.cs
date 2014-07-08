using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleDownloader
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow w = new MainWindow();
            ViewModel vm = new ViewModel();
            w.DataContext = vm;
            w.ClipboardChanged += vm.ClipboardChanged;
            w.Show();
        }
    }
}
