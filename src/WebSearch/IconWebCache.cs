using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Threading;
using System.Text.RegularExpressions;
using Configurator;
using System.IO;

namespace WebSearch
{
    [Serializable]
    public class IconWebCache
    {
        #region Properties
        private Dictionary<SearchEngine, Bitmap> _icons;
        private Dictionary<SearchEngine, DateTime> _time_stamps;
        private Thread _worker_thread = null;
        private DateTime _worker_thread_timestamp; 
        private TimeSpan _worker_thread_ttl = new TimeSpan(15 * TimeSpan.TicksPerSecond);
        private Bitmap _search_icon = Properties.Resources.search.ToBitmap();
        private TimeSpan _ttl;
        private Regex _wwwImageRegex = new Regex(@"\.(png|jpg)$");
        private string _cache_path = CommonInfo.UserFolder + Properties.Resources.Name + @"\" + "Cache";
        #endregion

        #region Accessors
        public Dictionary<SearchEngine, Bitmap> Icons { get { return _icons; } }
        public Dictionary<SearchEngine, DateTime> TimeStamps { get { return _time_stamps; } }
        public TimeSpan TTL { get { return _ttl; } }
        public string CachePath { get { return _cache_path; } }
        #endregion

        #region Constructors
        public IconWebCache(int ttl)
        {
            _icons = new Dictionary<SearchEngine, Bitmap>();
            _time_stamps = new Dictionary<SearchEngine, DateTime>();
            _ttl = new TimeSpan(ttl * TimeSpan.TicksPerHour);
        }

        public IconWebCache(SerializationInfo info, StreamingContext context)
        {
            _icons = (Dictionary<SearchEngine, Bitmap>)info.GetValue("Icons", typeof(Dictionary<SearchEngine, Bitmap>));
            _time_stamps = (Dictionary<SearchEngine, DateTime>)info.GetValue("TimeStamps", typeof(Dictionary<SearchEngine, DateTime>));
            _ttl = (TimeSpan)info.GetValue("TTL", typeof(TimeSpan));
        }
        #endregion

        #region Public Methods
        public int GetTTLhours()
        {
            return (int)((long)_ttl.Ticks / TimeSpan.TicksPerHour);
        }

        public void SetTTLhours(int hours)
        {
            _ttl = new TimeSpan(hours * TimeSpan.TicksPerHour);
        }

        public void Clear()
        {
            _icons.Clear();
            _time_stamps.Clear();
        }

        public Bitmap GetIcon(SearchEngine engine)
        {
            Bitmap icon = _search_icon;
            if (!string.IsNullOrEmpty(engine.IconSource))
            {
                if (!_icons.ContainsKey(engine) ||
                    (_ttl == TimeSpan.Zero ? false :
                     DateTime.Now - _time_stamps[engine] >= _ttl))
                {
                    RetrieveIcon(engine);
                }
                else
                {
                    icon = _icons[engine];
                }
            }
            return icon;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Icons", Icons);
            info.AddValue("TimeStamps", TimeStamps);
            info.AddValue("TTL", TTL);
        }
        #endregion

        #region Private Methods
        private void RetrieveIcon(SearchEngine engine)
        {
            if (_worker_thread != null && (
                _worker_thread.ThreadState != ThreadState.Background ||
                _worker_thread.ThreadState != ThreadState.Running ||
                _worker_thread.ThreadState != ThreadState.Unstarted ||
                _worker_thread.ThreadState != ThreadState.WaitSleepJoin ||
                DateTime.Now - _worker_thread_timestamp >= _worker_thread_ttl))
            {
                _worker_thread = null;
            }
            if (_worker_thread == null)
            {
                ThreadStart ts = new ThreadStart(delegate()
                    {
                        Bitmap icon = null;
                        bool found_in_hd = false;

                        try
                        {
                            // Retrieve from the hard drive
                            string icon_file = _cache_path + @"\" + engine.Name + ".png";
                            if (Directory.Exists(_cache_path) && File.Exists(icon_file))
                            {
                                icon = (Bitmap)Bitmap.FromFile(icon_file);
                                found_in_hd = true;
                            }
                            //

                            // Retrieve from the web
                            if (icon == null)
                            {
                                string iconUrl;
                                if (_wwwImageRegex.IsMatch(engine.IconSource))
                                {
                                    iconUrl = engine.IconSource;
                                }
                                else
                                {
                                    iconUrl = @"http://www.getfavicon.org/?url=" + engine.IconSource + @"/favicon.32.png";
                                }
                                System.Net.HttpWebRequest wr = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(iconUrl);
                                System.Net.HttpWebResponse ws = (System.Net.HttpWebResponse)wr.GetResponse();
                                System.IO.Stream stream = ws.GetResponseStream();
                                icon = (Bitmap)Bitmap.FromStream(stream);
                            }
                            //

                            if (icon != null)
                            {
                                if (_icons.ContainsKey(engine))
                                {
                                    _icons[engine] = icon;
                                    _time_stamps[engine] = DateTime.Now;
                                    if (!found_in_hd)
                                    {
                                        if (!Directory.Exists(_cache_path))
                                            Directory.CreateDirectory(_cache_path);
                                        icon.Save(icon_file, System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }
                                else
                                {
                                    _icons.Add(engine, icon);
                                    _time_stamps.Add(engine, DateTime.Now);
                                    if (!found_in_hd)
                                    {
                                        if (!Directory.Exists(_cache_path))
                                            Directory.CreateDirectory(_cache_path);
                                        icon.Save(icon_file, System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // TODO
                        }
                        _worker_thread_timestamp = DateTime.Now;
                    });
                _worker_thread = new Thread(ts);
                _worker_thread.Start();
            }
        }
        #endregion
    }
}
