using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using SystemCore.CommonTypes;
using System.Runtime.Serialization.Formatters.Binary;

namespace SystemCore.SystemAbstraction.ImageHandling
{
    public sealed class IconManager
    {
        #region Properties
        private static volatile IconManager _instance;
        private static object _syncRoot = new Object();
        private IconCache _cache;
        #endregion

        #region Accessors
        public static IconManager Instance { 
            get {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new IconManager();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Constructors
        private IconManager() {
            _cache = new IconCache();
        }
        #endregion

        #region Public Methods
        public void AddIcon(string path)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                Icon icon = RetrieveIcon(path);
                _cache.RegisterIcon(path, icon.ToBitmap());
                icon.Dispose();
            }
            catch (Exception)
            {
                _cache.RegisterIcon(path, null); // problema!!!!
            }
            Monitor.Exit(_syncRoot);
        }

        public void AddIcon(string path, Image icon)
        {
            Monitor.Enter(_syncRoot);
            _cache.RegisterIcon(path, icon);
            Monitor.Exit(_syncRoot);
        }

        public Image GetIcon(string path)
        {
            if (!_cache.Contains(path))
            {
                AddIcon(path);
            }
            return _cache.RetrieveIcon(path);
        }

        public void RemoveIcon(string path)
        {
            _cache.UnregisterIcon(path);
        }

        public void Save()
        {
            Monitor.Enter(_syncRoot);
            Stream streamWrite = File.Create(CommonInfo.PluginsFolder);
            BinaryFormatter binaryWrite = new BinaryFormatter();
            binaryWrite.Serialize(streamWrite, _cache);
            streamWrite.Close();
            Monitor.Exit(_syncRoot);
            streamWrite = null;
            binaryWrite = null;
        }

        public IconCache LoadCache()
        {
            Monitor.Enter(_syncRoot);
            Stream streamRead = File.OpenRead(CommonInfo.PluginsFolder);
            BinaryFormatter binaryRead = new BinaryFormatter();
            IconCache cache = (IconCache)binaryRead.Deserialize(streamRead);
            streamRead.Close();
            Monitor.Exit(_syncRoot);
            streamRead = null;
            binaryRead = null;
            return cache;
        }

        public void Load()
        {
            _cache = LoadCache();
        }
        #endregion

        #region Static Methods
        public static Icon RetrieveIcon(string path)
        {
            if (!Directory.Exists(path))
            {
                //return Icon.ExtractAssociatedIcon(path);
                IntPtr ptr = RetrieveFileIconPtr(path);
                Icon temp_icon = Icon.FromHandle(ptr);
                Icon icon = (Icon)temp_icon.Clone();
                temp_icon.Dispose();
                DestroyIconPtr(ptr);
                return icon;
            }
            else
            {
                IntPtr ptr = RetrieveDirectoryIconPtr(path);
                Icon temp_icon = Icon.FromHandle(ptr);
                Icon icon = (Icon)temp_icon.Clone();
                temp_icon.Dispose();
                DestroyIconPtr(ptr);
                return icon;
            }
        }

        private static IntPtr RetrieveFileIconPtr(string path)
        {
            uint attributes = 0;// Win32.SHELL32_FILE_ATTRIBUTE_NORMAL;
            SHGFI flags = /*SHGFI.UseFileAttributes | SHGFI.Icon |*/ SHGFI.SysIconIndex;
            SHFILEINFO shfileinfo = new SHFILEINFO();
            IntPtr list = Win32.SHGetFileInfo(path, attributes, ref shfileinfo, (uint)Marshal.SizeOf(shfileinfo), (uint)flags);
            return Win32.ImageList_GetIcon(list, shfileinfo.iIcon.ToInt32(), (int)ImageListDrawItemConstants.ILD_TRANSPARENT);
            //return shfileinfo.hIcon;
        }

        private static IntPtr RetrieveDirectoryIconPtr(string path)
        {
            uint attributes = Win32.SHELL32_FILE_ATTRIBUTE_DIRECTORY;
            SHGFI flags = SHGFI.UseFileAttributes | SHGFI.Icon;
            SHFILEINFO shfileinfo = new SHFILEINFO();
            Win32.SHGetFileInfo(path, attributes, ref shfileinfo, (uint)Marshal.SizeOf(shfileinfo), (uint)flags);
            return shfileinfo.hIcon;
        }

        private static IntPtr SearchFileIconPtr(string file_path)
        {
            SHFILEINFO shfileinfo = new SHFILEINFO();
            Win32.SHGetFileInfo(file_path, Win32.SHELL32_FILE_ATTRIBUTE_NORMAL, ref shfileinfo, (uint)Marshal.SizeOf(shfileinfo),
                                                Win32.SHGFI_ICON); // Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON
            return shfileinfo.hIcon;
        }

        private static IntPtr SearchDirectoryIconPtr(string dir_path)
        {
            SHFILEINFO shfileinfo = new SHFILEINFO();
            Win32.SHGetFileInfo(dir_path, Win32.SHELL32_FILE_ATTRIBUTE_DIRECTORY, ref shfileinfo, (uint)Marshal.SizeOf(shfileinfo),
                                                Win32.SHGFI_ICON); //Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON
            return shfileinfo.hIcon;
        }

        private static Icon SearchFileIcon(string file_path)
        {
            return Icon.FromHandle(SearchFileIconPtr(file_path));
        }

        private static Icon SearchDirectoryIcon(string dir_path)
        {
            return Icon.FromHandle(SearchDirectoryIconPtr(dir_path));
        }

        private static bool DestroyIconPtr(IntPtr ptr)
        {
            return Win32.DestroyIcon(ptr);
        }
        #endregion
    }
}
