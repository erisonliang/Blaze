using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Net;
using System.IO;
using Configurator;
using System.Diagnostics;
using SystemCore.SystemAbstraction;

namespace BlazeUpdater
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        protected BackgroundWorker _backgroundWorker = null;
        protected double _fileSize;
        protected DateTime _beginning;
        protected bool _completed = true;
        protected DateTime _last_update = DateTime.Now;

        public Window1()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            expander1.Expanded += new RoutedEventHandler(expander1_Expanded);
            expander1.Collapsed += new RoutedEventHandler(expander1_Collapsed);
            expander1.IsExpanded = false;
            //SystemCore.SystemAbstraction.WindowManagement.GlassExtender.GlassBackground(this);
            StartDownload();
        }

        public void InitializeBackgroundWorker()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(_backgroundWorker_ProgressChanged);
        }

        #region BackgroundWorker Event Handlers
        void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            double downloaded = (double)((double)e.ProgressPercentage / (double)100) * _fileSize;
            double left = _fileSize - downloaded;
            double speed = downloaded / (DateTime.Now - _beginning).TotalSeconds;
            TimeSpan remainging = (speed == 0? TimeSpan.Zero : TimeSpan.FromSeconds(left / speed));
            progressBar1.Value = e.ProgressPercentage;
            percentageLabel.Content = "Progress: " + e.ProgressPercentage.ToString() + "% (" + downloaded.ToString("#.#") + " out of " + _fileSize.ToString() + " KBytes)";
            if ((DateTime.Now - _last_update) >= TimeSpan.FromSeconds(.5))
            {
                speedLabel.Content = "Speed: " + speed.ToString("#.#") + " KB/s";
                _last_update = DateTime.Now;
                timeRemainingLabel.Content = "Time Remaining: " + (speed == 0 ? "being estimated..." :
                                                remainging.Hours + "h " +
                                                remainging.Minutes + "m " +
                                                remainging.Seconds + "s ");
            }
        }

        void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _completed = true;
            cancelButton.Content = "Close";
            if (e.Error != null)
                MessageBox.Show("A error ocurred while downloading Blaze:" + Environment.NewLine + Environment.NewLine + e.Error.Message + Environment.NewLine + Environment.NewLine + "Please try again later.", "Blaze Updater", MessageBoxButton.OK, MessageBoxImage.Error);
            if (e.Cancelled)
                OnCancelled();
            else
                OnCompleted();
        }

        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            Stream resStream = null;
            Stream localStream = null;

            try
            {
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(BlazeWebInfo.GetBlazeDownloadUrl());
                }
                catch
                {
                    MessageBox.Show("The server could not be reached. Please try again later.", "Blaze Updater", MessageBoxButton.OK, MessageBoxImage.Error);
                    _backgroundWorker.CancelAsync();
                }
                req.ContentType = "Application";
                req.Credentials = CredentialCache.DefaultCredentials;
                res = (HttpWebResponse)req.GetResponse();

                Int64 fileSize = res.ContentLength;
                _fileSize = Math.Ceiling((double)(fileSize) / (double)1024);

                string filename = res.Headers.Get("Content-Disposition");

                resStream = res.GetResponseStream();

                localStream = new FileStream(CommonInfo.BlazeTempPath, FileMode.Create, FileAccess.Write, FileShare.None);

                int bytesSize = 0;

                byte[] downBuffer = new byte[2048];

                _beginning = DateTime.Now;
                while ((bytesSize = resStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
                {
                    localStream.Write(downBuffer, 0, bytesSize);

                    _backgroundWorker.ReportProgress(Convert.ToInt32((localStream.Length * 100) / fileSize));

                    if (_backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("A error ocurred while downloading Blaze:" + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine + "Please try again later.", "Blaze Updater", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (req != null)
                    req = null;
                if (res != null)
                    res.Close();
                if (resStream != null)
                    resStream.Close();
                if (localStream != null)
                    localStream.Close();
            }
        }
        #endregion

        #region BackgroundWorker Methods
        public void StartDownload()
        {
            _backgroundWorker.RunWorkerAsync();
        }

        public bool CancelDownload()
        {
            if (MessageBox.Show("Do you really want to cancel the download?", "Blaze Updater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _backgroundWorker.CancelAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnCancelled()
        {
            this.Close();
        }

        public void OnCompleted()
        {
            MessageBoxResult result = MessageBox.Show("Blaze Updater will now launch the installer. Would you like it to shutdown Blaze for you?", "Blaze Updater", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result != MessageBoxResult.Cancel)
            {
                if (result == MessageBoxResult.Yes)
                    Win32.SendMessageA(
                      (IntPtr)Win32.HWND_BROADCAST,
                      Win32.WM_KILLME,
                      IntPtr.Zero,
                      IntPtr.Zero);
                Process.Start(CommonInfo.BlazeTempPath);

            }
            this.Close();

        }
        #endregion

        #region Expander Event Handlers
        void expander1_Collapsed(object sender, RoutedEventArgs e)
        {
            DoubleAnimation expanderAnimation = new DoubleAnimation();
            expanderAnimation.To = 23;
            expanderAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));

            DoubleAnimation windowAnimation = new DoubleAnimation();
            windowAnimation.To = 125;
            windowAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));

            expander1.BeginAnimation(Expander.HeightProperty, expanderAnimation);
            expander1.Header = "More details";
            mainWindow.BeginAnimation(Window.HeightProperty, windowAnimation);
        }

        void expander1_Expanded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation expanderAnimation = new DoubleAnimation();
            expanderAnimation.To = 103;
            expanderAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));

            DoubleAnimation windowAnimation = new DoubleAnimation();
            windowAnimation.To = 240;
            windowAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));

            expander1.BeginAnimation(Expander.HeightProperty, expanderAnimation);
            expander1.Header = "Less details";
            mainWindow.BeginAnimation(Window.HeightProperty, windowAnimation);
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _completed = false;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_completed && !CancelDownload())
                e.Cancel = true;
            base.OnClosing(e);
        }
    }
}
