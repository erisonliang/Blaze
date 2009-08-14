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
            Dictionary<string, bool> indexSubdirectories = info.IndexSubdirectories;

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

                if (ext.Count == 0)
                    ext.Add(".*");
                try
                {
                    fullpathes = new List<string>(FileSearcher.SearchFullNames(directory, ext, indexSubdirectories[directory]));
                }
                catch (Exception)
                {
                    //_logger.WriteLine("Could not index file: {0}", e.Message);
                    fullpathes = new List<string>();
                }
                List<string> filenames = new List<string>();

                for (int j = 0; j < fullpathes.Count; j++)
                {
                    string path = fullpathes[j];
                    string name = FileNameManipulator.GetFileName(path);
                    filenames.Add(name);

                    //FileInfo finfo = new FileInfo(path);
                    //if (finfo.Extension.ToLower() == ".lnk")
                    //{
                    //    string new_path = MsiShortcutParser.ParseShortcut(path);
                    //    if (new_path == null)
                    //    {
                    //        try
                    //        {
                    //            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                    //            IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(path);
                    //            if (Directory.Exists(link.TargetPath))
                    //            {
                    //                new_path = link.TargetPath;
                    //            }
                    //            else if (File.Exists(link.TargetPath))
                    //            {
                    //                new_path = link.TargetPath;
                    //            }
                    //            else
                    //            {
                    //                new_path = path;
                    //            }
                    //            if (File.Exists(link.IconLocation))
                    //            {
                    //                icon_path = link.IconLocation;
                    //            }
                    //        }
                    //        catch
                    //        {
                    //            new_path = path;
                    //            icon_path = path;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        icon_path = new_path;
                    //    }
                    //    path = new_path;
                    //}

                    if (_index.Paths.ContainsKey(name))
                    {
                        _index.Paths[name].Add(path);
                        _index.Icons[name].Add(IconManager.RetrieveIcon(path).ToBitmap());

                        List<string> list_keywords = new List<string>();
                        foreach (IndexerPlugin plugin in assistingPlugins)
                        {
                            string[] tmp_keywords = plugin.GetFileKeywords(path);
                            foreach (string key in tmp_keywords)
                                list_keywords.AddRange(StringUtility.GenerateKeywords(key, true));
                        }
                        _index.Keywords[name].AddRange(list_keywords.Distinct());
                    }
                    else
                    {
                        List<string> newpath = new List<string>();
                        newpath.Add(path);
                        _index.Names.Add(name);
                        _index.Paths.Add(name, newpath);
                        _index.Icons.Add(name, new List<System.Drawing.Image>(new System.Drawing.Image[] { IconManager.RetrieveIcon(path).ToBitmap() }));

                        List<string> list_keywords = new List<string>(StringUtility.GenerateKeywords(name, true));
                            //name.Split(new char[] { ' ', '_', '-', '(', ')', '[', ']', '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
                        //int len = keywords.Length;
                        //for (int k = 0; k < len; k++)
                        //    keywords[k] = keywords[k].ToLower();

                        //List<string> list_keywords = new List<string>();

                        //string small_name = string.Empty;
                        //foreach (string token in list_tokens)
                        //{
                        //    small_name += token[0];
                        //}
                        //list_keywords.Add(small_name);

                        foreach (IndexerPlugin plugin in assistingPlugins)
                        {
                            string[] tmp_keywords = plugin.GetFileKeywords(path);
                            foreach (string key in tmp_keywords)
                                list_keywords.AddRange(StringUtility.GenerateKeywords(key, true));
                                    //key.Split(new char[] { ' ', '_', '-', '(', ')', '[', ']', '{', '}' }, StringSplitOptions.RemoveEmptyEntries));
                        }

                        //bool can_remove_possible_indefinite_article = false;
                        ////bool can_remove_possible_preposition = false;
                        ////bool can_remove_possible_definite_article = false;
                        //if (list_keywords.FindAll(delegate(string s) { return s.Length > 1; }).Count > 0)
                        //    can_remove_possible_indefinite_article = true;
                        ////if (list_keywords.FindAll(delegate(string s) { return s.Length > 2; }).Count > 2)
                        ////    can_remove_possible_preposition = true;
                        ////if (list_keywords.FindAll(delegate(string s) { return s.Length > 3; }).Count > 6)
                        ////    can_remove_possible_preposition = true;
                        //list_keywords.RemoveAll(delegate(string s)
                        //{
                        //    if (s.Trim() == string.Empty)
                        //        return true;
                        //    else if (s.Length == 1 && can_remove_possible_indefinite_article && Char.IsLetter(s[0]))
                        //        return true;
                        //    //else if (s.Length == 2 && can_remove_possible_preposition)
                        //    //    return true;
                        //    //else if (s.Length == 3 && can_remove_possible_definite_article)
                        //    //    return true;
                        //    else
                        //        return false;
                        //});
                        ////list_keywords.Find(delegate(string s)
                        ////{
                        ////    return s.Length > 1;
                        ////});
                        //for (int k = 0; k < list_keywords.Count; k++)
                        //{
                        //    list_keywords[k] = list_keywords[k].ToLower();
                        //}
                        //_index.Tokens.Add(name, list_tokens);
                        _index.Keywords.Add(name, list_keywords.Distinct().ToList());
                    }
                    //_logger.WriteLine("Added to inverted index <{0} . {1}>", name, path);
                }
            }
            //_logger.WriteLine("Indexed {0} files.", _index.Paths.Count);

            _index.Names.Sort();

            // clean up
            directories = null;
            extensions = null;
            indexSubdirectories = null;
            GC.Collect();
        }
    }
}
