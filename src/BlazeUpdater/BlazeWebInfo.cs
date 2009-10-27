using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Configurator;
using System.Windows;

namespace BlazeUpdater
{
    public static class BlazeWebInfo
    {
        public static Version GetBlazeVersion()
        {
            return new Version(GetStringFromServer(CommonInfo.BlazeVersionUrl));
        }

        public static string GetBlazeDownloadUrl()
        {
            return GetStringFromServer(CommonInfo.BlazeDownloadUrl);
        }

        public static string GetStringFromServer(string url)
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
                throw;
            }
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Configurator;
using System.Windows;

namespace BlazeUpdater
{
    public static class BlazeWebInfo
    {
        public static Version GetBlazeVersion()
        {
            return new Version(GetStringFromServer(CommonInfo.BlazeVersionUrl));
        }

        public static string GetBlazeDownloadUrl()
        {
            return GetStringFromServer(CommonInfo.BlazeDownloadUrl);
        }

        public static string GetStringFromServer(string url)
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
                throw;
            }
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
    }
}
