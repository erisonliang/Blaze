using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Configurator;
using System.Net;
using System.IO;

namespace BlazeUpdater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected bool _supress = false;

        protected override void OnStartup(StartupEventArgs e)
        {
            Version version_on_server = GetBlazeVersion();
            Version local_version = CommonInfo.BlazeVersion;

            if (e.Args != null && e.Args.Count() > 0 && e.Args[0] == "-supress")
            {
                _supress = true;
            }

            if (version_on_server >= local_version)
            {
                MessageBox.Show("There is a new version of Blaze available. Would you like to download it now?", "Blaze Updater", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            else
            {
                if (!_supress)
                {

                    MessageBox.Show("Blaze is up to date. No update is required.", "Blaze Updater", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Shutdown();
                }
            }
            base.OnStartup(e);
        }

        protected Version GetBlazeVersion()
        {
            return new Version(GetStringFromServer(CommonInfo.BlazeVersionUrl));
        }

        protected string GetStringFromServer(string url)
        {
            // Create a request for the URL.         
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            HttpWebResponse response = null;
            Stream dataStream = null;
            StreamReader reader = null;
            string responseFromServer = null;

            try
            {
                // Get the response.
                response = (HttpWebResponse)request.GetResponse();
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
            }
            catch
            {
                if (!_supress)
                    MessageBox.Show("The server couldn't be reached. Please try again later.", "Blaze Updater", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Shutdown();
            }
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
    }
}
