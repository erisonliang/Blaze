using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace SystemCore.SystemAbstraction.ImageHandling
{
    [Serializable]
    public class IconCache
    {
        #region Properties
        private List<string> _names;
        private Dictionary<string, Image> _icons;
        private object _syncRoot;
        #endregion

        #region Accessors
        public List<string> Names { get { return _names; } }
        public Dictionary<string, Image> Icons { get { return _icons; } }
        #endregion

        #region Constructors
        public IconCache()
        {
            _names = new List<string>();
            _icons = new Dictionary<string, Image>();
            _syncRoot = new Object();
        }

        public IconCache(IconCache cache)
        {
            _names = new List<string>(cache.Names);
            _icons = new Dictionary<string, Image>(cache.Icons);
            _syncRoot = new Object();
        }

        public IconCache(List<string> names, List<Icon> icon_pool, Dictionary<string, Image> icons)
        {
            _names = new List<string>(names);
            _icons = new Dictionary<string, Image>(icons);
            _syncRoot = new Object();
        }
        #endregion

        #region Public Methods
        public void RegisterIcon(string name, Image icon)
        {
            Monitor.Enter(_syncRoot);
            if (!_icons.ContainsKey(name))
            {
                _names.Add(name);
                _icons.Add(name, icon);
            }
            else
            {
                //Win32.DestroyIcon(_icons[name].Handle);
                _icons[name].Dispose();
                _icons[name] = null;
                //GC.Collect();
                _icons[name] = icon;
            }
            Monitor.Exit(_syncRoot);
        }

        public void UnregisterIcon(string name)
        {
            Monitor.Enter(_syncRoot);
            if (_icons.ContainsKey(name))
            {
                _names.Remove(name);
                //Win32.DestroyIcon(_icons[name].Handle);
                _icons[name].Dispose();
                _icons[name] = null;
                _icons.Remove(name);
            }
            Monitor.Exit(_syncRoot);
        }

        public Image RetrieveIcon(string name)
        {
            Monitor.Enter(_syncRoot);
            if (_icons.ContainsKey(name))
            {
                Monitor.Exit(_syncRoot);
                return _icons[name];
            }
            else
            {
                Monitor.Exit(_syncRoot);
                return null;
            }
        }

        public bool Contains(string name)
        {
            return _icons.ContainsKey(name);
        }
        #endregion
    }
}
