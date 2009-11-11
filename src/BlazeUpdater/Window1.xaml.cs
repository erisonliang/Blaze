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
        protected Color _default_color;
        protected Color _expanded_color;

        public Window1()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            expander1.Expanded += new RoutedEventHandler(expander1_Expanded);
            expander1.Collapsed += new RoutedEventHandler(expander1_Collapsed);
            expander1.IsExpanded = false;
            SystemCore.SystemAbstraction.WindowManagement.GlassExtender.GlassBackground(this);
            {
                SolidColorBrush actual_brush = (SolidColorBrush)expander1.Background;
                _default_color = actual_brush.Color;
                System.Drawing.Color actual_color = System.Drawing.Color.FromArgb(1, actual_brush.Color.R, actual_brush.Color.G, actual_brush.Color.B);
                float current_hue = actual_color.GetHue();
                float current_saturation = actual_color.GetSaturation();
                float current_brightness = actual_color.GetBrightness();

                Color new_color = HsvToRgb(current_hue, current_saturation, current_brightness/1.25);
                new_color.A = 100;
                _expanded_color = new_color;
            }
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

        /// <summary>
        /// Convert HSV to RGB
        /// h is from 0-360
        /// s,v values are 0-1
        /// r,g,b values are 0-255
        /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
        /// </summary>
        private Color HsvToRgb(double h, double S, double V)
        {
            // ######################################################################
            // T. Nathan Mundhenk
            // mundhenk@usc.edu
            // C/C++ Macro HSV to RGB

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            return Color.FromArgb(1, (byte)Clamp((int)(R * 255.0)), (byte)Clamp((int)(G * 255.0)), (byte)Clamp((int)(B * 255.0)));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
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
            Duration animation_duration = new Duration(TimeSpan.FromSeconds(0.25));

            DoubleAnimation expanderAnimation = new DoubleAnimation(23, animation_duration);
            ColorAnimation color_animation = new ColorAnimation(_default_color, animation_duration);
            ThicknessAnimation border_animation = new ThicknessAnimation(new Thickness(0), animation_duration);
            DoubleAnimation windowAnimation = new DoubleAnimation(125, animation_duration);

            if (expander1.Background.IsFrozen)
                expander1.Background = expander1.Background.Clone();
            expander1.BeginAnimation(Expander.HeightProperty, expanderAnimation);
            expander1.Background.BeginAnimation(SolidColorBrush.ColorProperty, color_animation);
            expander1.BeginAnimation(Expander.BorderThicknessProperty, border_animation);
            expander1.Header = "More details";
            mainWindow.BeginAnimation(Window.HeightProperty, windowAnimation);
        }

        void expander1_Expanded(object sender, RoutedEventArgs e)
        {
            Duration animation_duration = new Duration(TimeSpan.FromSeconds(0.25));

            DoubleAnimation expanderAnimation = new DoubleAnimation(103, animation_duration);
            ColorAnimation color_animation = new ColorAnimation(_expanded_color, animation_duration);
            ThicknessAnimation border_animation = new ThicknessAnimation(new Thickness(1), animation_duration);
            DoubleAnimation windowAnimation = new DoubleAnimation(240, animation_duration);

            if (expander1.Background.IsFrozen)
                expander1.Background = expander1.Background.Clone();
            expander1.BeginAnimation(Expander.HeightProperty, expanderAnimation);
            expander1.Background.BeginAnimation(SolidColorBrush.ColorProperty, color_animation);
            expander1.BeginAnimation(Expander.BorderThicknessProperty, border_animation);
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
