// Blaze: Automated Desktop Experience
// Copyright (C) 2008-2010  Gabriel Barata
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
using SystemCore.SystemAbstraction.FileHandling;
using SystemCore.SystemAbstraction.ImageHandling;
using Configurator;

namespace Blaze.Indexer
{
    class FileIndexer
    {
        #region Properties
        private Index _index = null; // inverted index
        private Index _old_index = null; // backup inverted index
        private List<IndexerPlugin> _plugins;
        private Mutex _mutex;
        private bool _loaded = false;
        private bool _is_cache_merged = false;
        #endregion

        #region Accessors
        public bool IsCacheMerged { get { return _is_cache_merged; } set { _is_cache_merged = value; } }
        #endregion

        #region Constructors
        public FileIndexer()
        {
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

            // wait until its safe to enter an then lock
            try
            {
                _mutex.WaitOne();
            }
            catch (Exception)
            {
                //MessageBox.Show("file indexer error: "+e.Message);
            }

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = Path.GetFullPath(CommonInfo.BlazeIndexerPath); ;
            info.WorkingDirectory = Environment.CurrentDirectory;

            Process proc = new Process();
            proc.StartInfo = info;

            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(CommonInfo.BlazeIndexerPath));
            if (processes.Length > 0)
            {
                proc = processes[0];
            }
            else
            {
                proc.Start();
                proc.PriorityClass = ProcessPriorityClass.BelowNormal;
                proc.PriorityBoostEnabled = false;
            }
            
            proc.WaitForExit();
            proc.Dispose();
            info = null;
            _is_cache_merged = false;
            //LoadIndex();
            UpdateIndex();

            // release mutex
            _mutex.ReleaseMutex();
            //SettingsManager.Instance.SetIndexingTime(DateTime.Now - iTime);
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
                if (_index == null || _index.IsDisposed)
                    _index = new Index();
                if (_old_index != null && _old_index != _index && !_old_index.IsDisposed)
                {
                    _old_index.Dispose();
                    _old_index = _index;
                }
                return _index;
            }
            else
            {
                if (_old_index == null || _old_index.IsDisposed)
                    _old_index = new Index();
                return _old_index;
            }
        }

        public void Execute(InterpreterItem item, Keys modifiers)
        {
            string command = item.Desciption;
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
                
                if ((modifiers & Keys.Shift) == Keys.Shift)
                    command = FileSearcher.GetItemFolder(command);
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
                _mutex.ReleaseMutex();
            }
        }

        public Image GetItemIcon(IndexItem item)
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
                if (_index.ContainsItem(item))
                {
                    _mutex.ReleaseMutex();
                    return _index.GetItemIcon(item);
                }
                else
                {
                    _mutex.ReleaseMutex();
                    return null;
                }
            }
            else
            {
                if (_old_index.ContainsItem(item))
                {
                    return _old_index.GetItemIcon(item);
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
                        try
                        {
                            _index = (Index)binaryRead.Deserialize(streamRead);
                            if (_old_index != null)
                            {
                                if (_old_index != _index)
                                {
                                    _old_index.Dispose();
                                    _old_index = _index;
                                }
                            }
                            SettingsManager.Instance.SetNumberOfIndexedItems(_index.InfoItemCount);
                            SettingsManager.Instance.SetIndexingTime(_index.InfoBuildDuration);
                            SettingsManager.Instance.SetLastIndexBuild(_index.InfoBuildFinished);
                        }
                        catch
                        {

                        }
                        finally
                        {
                            streamRead.Close();
                            streamRead = null;
                            binaryRead = null;
                        }
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

                    try
                    {
                        _old_index = (Index)binaryRead.Deserialize(streamRead);
                        if (_index != null && /*_index != _old_index &&*/ !_index.IsDisposed)
                        {
                            _index.Dispose();
                            _index = null;
                        }
                        _index = _old_index;
                        SettingsManager.Instance.SetNumberOfIndexedItems(_index.InfoItemCount);
                        SettingsManager.Instance.SetIndexingTime(_index.InfoBuildDuration);
                        SettingsManager.Instance.SetLastIndexBuild(_index.InfoBuildFinished);
                    }
                    catch { }
                    finally
                    {
                        GC.Collect();
                        streamRead.Close();
                        streamRead = null;
                        binaryRead = null;
                    }
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
                    //foreach (string name in _index.Names)
                    //{
                    //    for (int i = 0; i < _index.Paths[name].Count; i++)
                    //    {
                    //        IconManager.Instance.RemoveIcon(_index.Paths[name][i]);
                    //    }
                    //}
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
