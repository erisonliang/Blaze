// Blaze: Automated Desktop Experience
// Copyright (C) 2008,2009  Gabriel Barata
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.Settings;
using SystemCore.SystemAbstraction.ImageHandling;
using Configurator;

namespace Blaze.Indexer
{
    class FileIndexer
    {
        #region Properties
        //private Logger _logger;
        private Index _index = null; // inverted index
        private Index _old_index = null; // backup inverted index
        private List<IndexerPlugin> _plugins;
        private Mutex _mutex;
        private bool _loaded = false;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public FileIndexer()
        {
            //_logger = new Logger("log.txt");
            _mutex = new Mutex();
        }
        #endregion

        #region Public Methods
        public void LoadPlugins(List<IndexerPlugin> plugins)
        {
            _plugins = plugins;
        }

        public void BuildIndex()
        {
            if (_index == null)
                _old_index = new Index();
            else
                _old_index = _index;

            ////MessageBox.Show("inicio");
            // wait until its safe to enter an then lock
            try
            {
                _mutex.WaitOne();
            }
            catch (Exception)
            {
                //MessageBox.Show("file indexer error: "+e.Message);
            }

            DateTime iTime = DateTime.Now;
            //_index = new Index();

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = Path.GetFullPath(CommonInfo.IndexerPath); ;
            info.WorkingDirectory = Environment.CurrentDirectory;//Path.GetFullPath(CommonInfo.BinFolder);
            //Process.Start(info);
            Process proc = new Process();
            proc.StartInfo = info;
            proc.Start();
            proc.PriorityClass = ProcessPriorityClass.BelowNormal;
            proc.PriorityBoostEnabled = false;
            //while (!proc.HasExited) ;
            proc.WaitForExit();
            proc.Dispose();
            info = null;
            //LoadIndex();
            UpdateIndex();


            

            //DirInfo info = SettingsManager.Instance.GetDirectories();
            //List<string> directories = info.Directories;
            //Dictionary<string, List<string>> extensions = info.Extensions;
            //Dictionary<string, bool> indexSubdirectories = info.IndexSubdirectories;

            //for (int i = 0; i < directories.Count; i++)
            //{
            //    string directory = directories[i];
            //    List<string> ext = new List<string>(extensions[directory]);
            //    List<string> fullpathes;
            //    List<IndexerPlugin> assistingPlugins = new List<IndexerPlugin>();

            //    foreach (IndexerPlugin plugin in _plugins)
            //    {
            //        if (SettingsManager.Instance.GetDirectories().Plugins[directory].Contains(plugin.Name))
            //        {
            //            assistingPlugins.Add(plugin);
            //            ext.AddRange(plugin.Extensions);
            //        }
            //    }

            //    try
            //    {
            //        fullpathes = new List<string>(FileSearcher.SearchFullNames(directory, ext, indexSubdirectories[directory]));
            //    }
            //    catch (Exception)
            //    {
            //        //_logger.WriteLine("Could not index file: {0}", e.Message);
            //        fullpathes = new List<string>();
            //    }
            //    List<string> filenames = new List<string>();

            //    for (int j = 0; j < fullpathes.Count; j++)
            //    {
            //        string path = fullpathes[j];
            //        string name = FileNameManipulator.GetFileName(path);
            //        //string icon_path = path;
            //        filenames.Add(name);

            //        //FileInfo finfo = new FileInfo(path);
            //        //if (finfo.Extension.ToLower() == ".lnk")
            //        //{
            //        //    string new_path = MsiShortcutParser.ParseShortcut(path);
            //        //    if (new_path == null)
            //        //    {
            //        //        try
            //        //        {
            //        //            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            //        //            IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(path);
            //        //            if (Directory.Exists(link.TargetPath))
            //        //            {
            //        //                new_path = link.TargetPath;
            //        //            }
            //        //            else if (File.Exists(link.TargetPath))
            //        //            {
            //        //                new_path = link.TargetPath;
            //        //            }
            //        //            else
            //        //            {
            //        //                new_path = path;
            //        //            }
            //        //            if (File.Exists(link.IconLocation))
            //        //            {
            //        //                icon_path = link.IconLocation;
            //        //            }
            //        //        }
            //        //        catch
            //        //        {
            //        //            new_path = path;
            //        //            icon_path = path;
            //        //        }
            //        //    }
            //        //    else
            //        //    {
            //        //        icon_path = new_path;
            //        //    }
            //        //    path = new_path;
            //        //}


            //        //IconManager.Instance.AddIcon(path);
            //        //Icon.ExtractAssociatedIcon(path).Dispose();
            //        if (_index.Paths.ContainsKey(name))
            //        {
            //            _index.Paths[name].Add(path);
            //            _index.Icons[name].Add(IconManager.RetrieveIcon(path).ToBitmap());
            //        }
            //        else
            //        {
            //            List<string> newpath = new List<string>();
            //            newpath.Add(path);
            //            _index.Names.Add(name);
            //            _index.Paths.Add(name, newpath);
            //            _index.Icons.Add(name, new List<System.Drawing.Image>(new System.Drawing.Image[] { IconManager.RetrieveIcon(path).ToBitmap() }));
            //            string[] keywords = name.Split(new char[] { ' ', '_', '-', '(', ')', '[', ']', '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            //            //int len = keywords.Length;
            //            //for (int k = 0; k < len; k++)
            //            //    keywords[k] = keywords[k].ToLower();
            //            //string small_name = string.Empty;
            //            //foreach (string keyword in keywords)
            //            //{
            //            //    small_name += keyword[0];
            //            //}
            //            List<string> list_keywords = new List<string>(keywords);
            //            //list_keywords.Add(small_name);
            //            foreach (IndexerPlugin plugin in assistingPlugins)
            //            {
            //                string[] tmp_keywords = plugin.GetFileKeywords(path);
            //                foreach (string key in tmp_keywords)
            //                    list_keywords.AddRange(key.Split(new char[] { ' ', '_', '-', '(', ')', '[', ']', '{', '}' }, StringSplitOptions.RemoveEmptyEntries));
            //            }
            //            bool can_remove_possible_indefinite_article = false;
            //            //bool can_remove_possible_preposition = false;
            //            //bool can_remove_possible_definite_article = false;
            //            if (list_keywords.FindAll(delegate(string s) { return s.Length > 1; }).Count > 0)
            //                can_remove_possible_indefinite_article = true;
            //            //if (list_keywords.FindAll(delegate(string s) { return s.Length > 2; }).Count > 2)
            //            //    can_remove_possible_preposition = true;
            //            //if (list_keywords.FindAll(delegate(string s) { return s.Length > 3; }).Count > 6)
            //            //    can_remove_possible_preposition = true;
            //            list_keywords.RemoveAll(delegate(string s)
            //            {
            //                if (s.Trim() == string.Empty)
            //                    return true;
            //                else if (s.Length == 1 && can_remove_possible_indefinite_article && Char.IsLetter(s[0]))
            //                    return true;
            //                //else if (s.Length == 2 && can_remove_possible_preposition)
            //                //    return true;
            //                //else if (s.Length == 3 && can_remove_possible_definite_article)
            //                //    return true;
            //                else
            //                    return false;
            //            });
            //            //list_keywords.Find(delegate(string s)
            //            //{
            //            //    return s.Length > 1;
            //            //});
            //            for (int k = 0; k < list_keywords.Count; k++)
            //            {
            //                list_keywords[k] = list_keywords[k].ToLower();
            //            }
            //            _index.Keywords.Add(name, list_keywords);
            //        }
            //        //_logger.WriteLine("Added to inverted index <{0} . {1}>", name, path);
            //    }
            //}
            ////_logger.WriteLine("Indexed {0} files.", _index.Paths.Count);

            ////MenuEngine menuEngine = new MenuEngine();
            ////List<string> menu_options = new List<string>();
            ////Dictionary<string, List<string>> menu_keywords = new Dictionary<string, List<string>>();
            ////foreach (string option in menuEngine.Names)
            ////{
            ////    menu_options.Add(option + " " + CommonInfo.GUID);
            ////    menu_keywords.Add(option + " " + CommonInfo.GUID, menuEngine.Keywords[option]);
            ////}

            ////foreach (string option in menu_options)
            ////{
            ////    List<string> option_paths = new List<string>();
            ////    _index.Names.Add(option);
            ////    _index.Paths.Add(option, option_paths);
            ////    _index.Keywords.Add(option, menu_keywords[option]);
            ////    option_paths = null;
            ////}

            //_index.Names.Sort();





            //// clean up
            //directories = null;
            //extensions = null;
            //indexSubdirectories = null;
            ////menuEngine = null;
            ////menu_options = null;
            ////menu_keywords = null;
            ////MessageBox.Show("fim");

            // release mutex
            _mutex.ReleaseMutex();
            SettingsManager.Instance.SetIndexingTime(DateTime.Now - iTime);
            GC.Collect();
        }

        public Index RetrieveItems()
        {
            bool test = true;
            try
            {
                test = _mutex.WaitOne(0, true);
            }
            catch
            {

            }
            if (test)
            {
                _mutex.ReleaseMutex();
                if (_index != null && !_index.IsDisposed && _old_index != null && _old_index != _index && !_old_index.IsDisposed)
                {
                    _old_index.Dispose();
                    _old_index = _index;
                }
                //if (_index != null && _index.IsDisposed == true)
                //    MessageBox.Show("index disposed problem");
                return (_index == null ? new Index() : _index);
            }
            else
            {
                //if (_old_index != null && _old_index.IsDisposed == true)
                //    MessageBox.Show("old index disposed problem");
                return (_old_index == null ? new Index() : _old_index);
            }
        }

        public void Execute(InterpreterItem item, Keys modifiers)
        {
            string cmd = item.AutoComplete;
            int pos = item.Pos;
            bool test = true;
            try
            {
                test = _mutex.WaitOne(0, true);
            }
            catch
            {

            }
            if (test)
            {
                if (_index.Paths.ContainsKey(cmd))
                {
                    string command = _index.Paths[cmd][pos];
                    if ((modifiers & Keys.Shift) == Keys.Shift)
                        command = GetItemFolder(command);
                    try
                    {
                        ProcessStartInfo info = new ProcessStartInfo(command);
                        info.UseShellExecute = true;
                        info.ErrorDialog = true;
                        System.Diagnostics.Process.Start(info);
                        info = null;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    command = null;
                }
                _mutex.ReleaseMutex();
            }
            else
            {
                if (_old_index.Paths.ContainsKey(cmd))
                {
                    string command = _old_index.Paths[cmd][pos];
                    if ((modifiers & Keys.Shift) == Keys.Shift)
                        command = GetItemFolder(command);
                    try
                    {
                        ProcessStartInfo info = new ProcessStartInfo(command);
                        info.UseShellExecute = true;
                        info.ErrorDialog = true;
                        System.Diagnostics.Process.Start(info);
                        info = null;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    command = null;
                }
            }
        }

        private string GetItemFolder(string path)
        {
            if (Path.GetExtension(path) == ".lnk")
            {
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(path);
                if (File.Exists(link.TargetPath))
                {
                    return Path.GetDirectoryName(link.TargetPath);
                }
                else if (Directory.Exists(link.TargetPath))
                {
                    if (link.TargetPath[link.TargetPath.Length - 1] != '\\')
                        return link.TargetPath + "\\";
                    else
                        return link.TargetPath;
                }
            }
            return Path.GetDirectoryName(path);
        }

        public Image GetItemIcon(string item, int pos)
        {
            bool test = true;
            try
            {
                test = _mutex.WaitOne(0, true);
            }
            catch
            {

            }
            if (test)
            {
                if (_index.Paths.ContainsKey(item))
                {
                    _mutex.ReleaseMutex();
                    //return _index.Icons[item][pos];
                    return IconManager.Instance.GetIcon(_index.Paths[item][pos]);
                }
                else
                {
                    _mutex.ReleaseMutex();
                    return null;
                }
            }
            else
            {
                if (_old_index.Paths.ContainsKey(item))
                {
                    //return _old_index.Icons[item][pos];
                    return IconManager.Instance.GetIcon(_old_index.Paths[item][pos]);
                }
                else
                {
                    return null;
                }
            }
        }

        public void SaveIndex()
        {
            Stream streamWrite = File.Create(CommonInfo.IndexCacheFile);
            BinaryFormatter binaryWrite = new BinaryFormatter();
            bool test = true;
            try
            {
                test = _mutex.WaitOne(0, true);
            }
            catch
            {

            }
            if (test)
            {
                binaryWrite.Serialize(streamWrite, _index);
                _mutex.ReleaseMutex();
            }
            else
            {
                binaryWrite.Serialize(streamWrite, _old_index);
            }
            //binaryWrite.Serialize(streamWrite, _index);
            streamWrite.Close();
            streamWrite = null;
            binaryWrite = null;
        }

        public void LoadIndex()
        {
            if (!_loaded)
            {
                bool test = true;
                try
                {
                    test = _mutex.WaitOne(0, true);
                }
                catch
                {

                }
                if (test)
                {
                    if (File.Exists(CommonInfo.IndexCacheFile))
                    {
                        Stream streamRead = File.OpenRead(CommonInfo.IndexCacheFile);
                        BinaryFormatter binaryRead = new BinaryFormatter();
                        _index = (Index)binaryRead.Deserialize(streamRead);
                        foreach (string name in _index.Names)
                        {
                            for (int i = 0; i < _index.Paths[name].Count; i++)
                            {
                                IconManager.Instance.AddIcon(_index.Paths[name][i], _index.Icons[name][i]);
                            }
                        }
                        if (_old_index != null)
                        {
                            if (_old_index != _index)
                            {
                                _old_index.Dispose();
                                _old_index = _index;
                            }
                        }

                        streamRead.Close();
                        streamRead = null;
                        binaryRead = null;
                        SettingsManager.Instance.SetNumberOfIndexedItems(_index.Paths.Count);
                    }
                    _loaded = true;
                    _mutex.ReleaseMutex();
                    GC.Collect();
                }
            }
        }

        private void UpdateIndex()
        {
            if (_loaded)
            {
                if (File.Exists(CommonInfo.IndexCacheFile))
                {
                    Stream streamRead = File.OpenRead(CommonInfo.IndexCacheFile);
                    BinaryFormatter binaryRead = new BinaryFormatter();

                    _old_index = (Index)binaryRead.Deserialize(streamRead);
                    if (_index != null && /*_index != _old_index &&*/ !_index.IsDisposed)
                    {
                        _index.Dispose();
                        _index = null;
                    }
                    _index = _old_index;

                    //Index new_index = (Index)binaryRead.Deserialize(streamRead);
                    //// remove old icons
                    //if (_index != null)
                    //{
                    //    foreach (string name in _index.Names)
                    //    {
                    //        for (int i = 0; i < _index.Paths[name].Count; i++)
                    //        {
                    //            if (new_index.Paths.ContainsKey(name) &&
                    //                new_index.Paths[name].Contains(_index.Paths[name][i]))
                    //            {

                    //            }
                    //            else
                    //                IconManager.Instance.RemoveIcon(_index.Paths[name][i]);
                    //        }
                    //    }
                    //}

                    // add new icons
                    foreach (string name in _index.Names)
                    {
                        for (int i = 0; i < _index.Paths[name].Count; i++)
                        {
                            IconManager.Instance.AddIcon(_index.Paths[name][i], _index.Icons[name][i]);
                        }
                    }

                    // clear old index (!buggy)
                    //if (_old_index != null)
                    //{
                    //    if (_old_index != _index)
                    //    {
                    //        _old_index.Dispose();
                    //        _old_index = _index;
                    //    }
                    //}

                    //// reassign new index
                    //if (_index != null && /*_index != _old_index &&*/ !_index.IsDisposed)
                    //    _index.Dispose();
                    //_index = null;
                    //_index = new_index;

                    SettingsManager.Instance.SetNumberOfIndexedItems(_index.Paths.Count);
                    GC.Collect();
                    streamRead.Close();
                    streamRead = null;
                    binaryRead = null;
                }
            }
        }

        public void UnloadIndex()
        {
            bool test = true;
            try
            {
                test = _mutex.WaitOne(0, true);
            }
            catch
            {

            }
            if (test)
            {

                if (_loaded)
                {
                    foreach (string name in _index.Names)
                    {
                        for (int i = 0; i < _index.Paths[name].Count; i++)
                        {
                            IconManager.Instance.RemoveIcon(_index.Paths[name][i]);
                        }
                    }
                    if (_index != null)
                    {
                        if (_index == _old_index)
                        {
                            _index.Dispose();
                            _index = null;
                        }
                        else
                        {
                            _index.Dispose();
                            _index = null;
                            if (_old_index != null)
                            {
                                _old_index.Dispose();
                                _old_index = null;
                            }
                        }
                    }
                    _loaded = false;
                }
                _mutex.ReleaseMutex();
                GC.Collect();
            }
        }
        #endregion
    }
}
