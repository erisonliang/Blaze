using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Configurator;
using System.Threading;

namespace BlazeUpdater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected bool _suppress = false;
        protected bool _is_safe = false;
        protected static Mutex mutex = new Mutex(true, CommonInfo.GUID + "-updater");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            bool test = false;
            try
            {
                test = mutex.WaitOne(TimeSpan.Zero, true);
            }
            catch
            {
                test = true;
            }
            if (test)
            {
                _is_safe = true;
                if (e.Args != null && e.Args.Count() > 0 && e.Args[0] == "-suppress")
                {
                    _suppress = true;
                }
                Version version_on_server = null;
                try
                {
                    version_on_server = BlazeWebInfo.GetBlazeVersion();
                }
                catch
                {
                    if (!_suppress)
                        MessageBox.Show("The server could not be reached. Please try again later.", "Blaze Updater", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Shutdown();
                }
                Version local_version = CommonInfo.BlazeVersion;

                if (version_on_server > local_version)
                {
                    if (MessageBox.Show("There is a new version of Blaze available. Would you like to download it now?", "Blaze Updater", MessageBoxButton.YesNo, MessageBoxImage.Question)
                        == MessageBoxResult.No)
                        this.Shutdown();
                    else
                        this.StartupUri = new Uri("Window1.xaml", UriKind.Relative);
                }
                else
                {
                    if (!_suppress)
                    {

                        MessageBox.Show("Blaze is up to date. No update is required.", "Blaze Updater", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    this.Shutdown();
                }
            }
            else
            {
                this.Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_is_safe)
                mutex.ReleaseMutex();
            base.OnExit(e);
        }
    }
}
