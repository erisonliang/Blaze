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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SystemCore.CommonTypes;
using SystemCore.Settings;
using SystemCore.SystemAbstraction.FileHandling;
using SystemCore.SystemAbstraction.ImageHandling;
using SystemCore.SystemAbstraction.StringUtilities;
using Configurator;
using System.Drawing;

namespace BlazeIndexer
{
    public class Indexer
    {
        private Index _index;
        private List<IndexerPlugin> _plugins;
        
        public Indexer()
        {
            _index = new Index();
        }

        public void LoadPlugins()
        {
            PluginLoader loader = new PluginLoader(CommonInfo.PluginsFolder);
            List<Plugin> plugins = loader.Load();

            SettingsManager.Instance.RegisterPlugins(plugins);
            PluginInfo info = SettingsManager.Instance.GetLoadablePlugins();
            foreach (Plugin plugin in plugins)
            {
                plugin.Activated = info.Enabled[plugin.Name];
            }

            List<IndexerPlugin> indexerPlugins = new List<IndexerPlugin>();
            foreach (Plugin plugin in plugins)
            {
                if (plugin.Activated)
                {
                    if (plugin.Type == PluginType.Indexer)
                        indexerPlugins.Add((IndexerPlugin)plugin);
                }
            }
            _plugins = indexerPlugins;
        }

        public void SaveIndex()
        {
            try
            {
                Stream streamWrite = File.Create(CommonInfo.IndexCacheFile);
                BinaryFormatter binaryWrite = new BinaryFormatter();
                binaryWrite.Serialize(streamWrite, _index);
                streamWrite.Close();
                streamWrite.Dispose();
                binaryWrite = null;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("BlazeIndexer couldn't save the index:" + Environment.NewLine + Environment.NewLine +
                                                     e.Message, "BlazeIndexer encountered a problem", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public void Clean()
        {
            _index.Dispose();
        }

        public void BuildIndex()
        {
            DirInfo info = SettingsManager.Instance.GetDirectories();
            List<string> directories = info.Directories;
            Dictionary<string, List<string>> extensions = info.Extensions;
            Dictionary<string, bool> includeDirectories = info.IncludeDirectories;
            Dictionary<string, bool> indexSubdirectories = info.IndexSubdirectories;

            _index.StartBuilding();
            for (int i = 0; i < directories.Count; i++)
            {
                string directory = directories[i];
                List<string> ext = new List<string>(extensions[directory]);
                List<string> fullpathes;
                List<IndexerPlugin> assistingPlugins = new List<IndexerPlugin>();

                foreach (IndexerPlugin plugin in _plugins)
                {
                    if (SettingsManager.Instance.GetDirectories().Plugins[directory].Contains(plugin.Name))
                    {
                        assistingPlugins.Add(plugin);
                        ext.AddRange(plugin.Extensions);
                    }
                }

                try
                {
                    fullpathes = new List<string>(FileSearcher.SearchFullNames(Environment.ExpandEnvironmentVariables(directory), ext, indexSubdirectories[directory], includeDirectories[directory], (CommonInfo.IsPortable ? true : false)));
                }
                catch (Exception)
                {
                    fullpathes = new List<string>();
                }

                for (int j = 0; j < fullpathes.Count; j++)
                {
                    string path = fullpathes[j];
                    string name = FileNameManipulator.GetFileName(Directory.Exists(path)
                                       ? Path.GetDirectoryName(path)
                                       : path);
                    Bitmap icon = null;
                    try
                    {
                        icon = IconManager.RetrieveIcon(path).ToBitmap();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    if (icon == null)
                        continue;

                    List<string> list_keywords = new List<string>(StringUtility.GenerateKeywords(name, true));
                    foreach (string key in list_keywords)
                    {
                        _index.AddEntry(key, name, list_keywords.Count, path, icon);
                    }
                    list_keywords.Clear();
                    foreach (IndexerPlugin plugin in assistingPlugins)
                    {
                        string[] tmp_keywords = plugin.GetFileKeywords(path);
                        foreach (string key in tmp_keywords)
                            list_keywords.AddRange(StringUtility.GenerateKeywords(key, true));
                    }
                    foreach (string key in list_keywords)
                    {
                        _index.AddEntry(key, name, list_keywords.Count, path, icon);
                    }
                }
            }

            _index.FinishBuilding();

            // clean up
            directories = null;
            extensions = null;
            indexSubdirectories = null;
            GC.Collect();
        }
    }
}
