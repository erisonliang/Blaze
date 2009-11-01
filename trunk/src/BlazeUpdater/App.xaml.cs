using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Configurator;

namespace BlazeUpdater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected bool _suppress = false;

        protected override void OnStartup(StartupEventArgs e)
        {
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

            if (e.Args != null && e.Args.Count() > 0 && e.Args[0] == "-supress")
            {
                _suppress = true;
            }
             
            if (version_on_server >= local_version)
            {
                if (MessageBox.Show("There is a new version of Blaze available. Would you like to download it now?", "Blaze Updater", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.No)
                    this.Shutdown();
            }
            else
            {
                if (!_suppress)
                {

                    MessageBox.Show("Blaze is up to date. No update is required.", "Blaze Updater", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Shutdown();
                }
            }
            base.OnStartup(e);
        }
    }
}
