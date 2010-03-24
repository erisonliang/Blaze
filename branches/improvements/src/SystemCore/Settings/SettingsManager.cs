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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction;
using SystemCore.SystemAbstraction.FileHandling;
using Configurator;

namespace SystemCore.Settings
{
    public class SettingsManager
    {
        #region Properties
        private string _path;
        private static readonly SettingsManager _instance = new SettingsManager();
        private DirInfo _directories = null;
        private HotKey _hotkey = null;
        private HotKey _ahotkey = null;
        private LearnedContent _learned_contents = null;
        private PluginInfo _plugin_info = null;
        private TimeSpan _indexing_time;
        private int _indexed_items = 0;
        private DateTime _last_index_build;
        private InterfaceInfo _interface_info = null;
        private SystemOptionsInfo _system_options_info = null;
        private AutomationOptionsInfo _automation_options_info = null;
        #endregion

        #region Accessors
        public static SettingsManager Instance { get { return _instance; } }
        #endregion

        #region Constructors
        private SettingsManager()
        {
            _path = Config.Instance.File;
            Config.Instance.Configure();
        }
        #endregion

        #region Public Methods
        public void PreLoadContents()
        {
            GetDirectories();
            GetMainHotKey();
            GetLearnedContents();
        }

        public DirInfo GetDirectories()
        {
            if (_directories == null)
            {
                List<string> directories = new List<string>();
                Dictionary<string, List<string>> extensions = new Dictionary<string, List<string>>();
                Dictionary<string, bool> indexSubdirectories = new Dictionary<string, bool>();
                Dictionary<string, bool> includeDirectories = new Dictionary<string, bool>();
                Dictionary<string, List<string>> plugins = new Dictionary<string, List<string>>();
                List<string> categories = INIManipulator.GetCategories(_path);
                if (categories.Count > 0)
                {
                    string category = "indexer";
                    if (categories.Contains(category))
                    {
                        List<string> keys = INIManipulator.GetKeys(_path, category);
                        int key_len = keys.Count;
                        if (key_len > 0)
                        {
                            for (int i = 0; i < key_len; i += 5)
                            {
                                string name;
                                string[] exts;
                                bool incdir = false;
                                bool subdir = false;
                                string[] plugs;
                                try
                                {
                                    name = INIManipulator.GetValue(_path, category, keys[i], "");
                                    exts = StrToArray(INIManipulator.GetValue(_path, category, keys[i + 1], ".lnk"));
                                    Boolean.TryParse(INIManipulator.GetValue(_path, category, keys[i + 2], "false"), out incdir);
                                    Boolean.TryParse(INIManipulator.GetValue(_path, category, keys[i + 3], "false"), out subdir);
                                    plugs = StrToArray(INIManipulator.GetValue(_path, category, keys[i + 4], ""));
                                }
                                catch (Exception)
                                {
                                    break;
                                }
                                directories.Add(name);
                                extensions.Add(name, new List<string>(exts));
                                includeDirectories.Add(name, incdir);
                                indexSubdirectories.Add(name, subdir);
                                plugins.Add(name, new List<string>(plugs));
                            }
                        }
                    }
                }
                _directories = new DirInfo(directories, extensions, includeDirectories,indexSubdirectories, plugins);
            }
            return _directories;
        }

        public void SaveDirectories(DirInfo dirs)
        {
            _directories = null;
            _directories = dirs;
            List<string> directories = dirs.Directories;
            Dictionary<string, List<string>> extensions = dirs.Extensions;
            Dictionary<string, bool> includeDirectories = dirs.IncludeDirectories;
            Dictionary<string, bool> indexSubdirectories = dirs.IndexSubdirectories;
            Dictionary<string, List<string>> dirPlugins = dirs.Plugins;
            string category = "indexer";
            int len = directories.Count;
            INIManipulator.DeleteCategory(_path, category);
            for (int i = 0; i < len; i++)
            {
                string dir = directories[i];
                int pos = i + 1;
                INIManipulator.WriteValue(_path, category, pos.ToString() + "\\name", dir);
                INIManipulator.WriteValue(_path, category, pos.ToString() + "\\extensions", ArrayToStr(extensions[dir].ToArray()));
                INIManipulator.WriteValue(_path, category, pos.ToString() + "\\includeDirectories", includeDirectories[dir].ToString());
                INIManipulator.WriteValue(_path, category, pos.ToString() + "\\indexSubdirectories", indexSubdirectories[dir].ToString());
                INIManipulator.WriteValue(_path, category, pos.ToString() + "\\plugins", ArrayToStr(dirPlugins[dir].ToArray()));
            }
        }

        public HotKey GetMainHotKey()
        {
            if (_hotkey == null)
            {
                int key = 32;
                bool alt = true;
                bool ctrl = true;
                bool shift = false;
                bool win = false;
                List<string> categories = INIManipulator.GetCategories(_path);
                if (categories.Count > 0)
                {
                    string category = "interaction";
                    string hotkeyMainkey = "hotkeyMainkey";
                    string hotkeyModifierAlt = "hotkeyModifierAlt";
                    string hotkeyModifierCtrl = "hotkeyModifierCtrl";
                    string hotkeyModifierShift = "hotkeyModifierShift";
                    string hotkeyModifierWin = "hotkeyModifierWin";
                    if (categories.Contains(category))
                    {
                        List<string> keys = INIManipulator.GetKeys(_path, category);
                        if (keys.Contains(hotkeyMainkey))
                        {
                            Int32.TryParse(INIManipulator.GetValue(_path, category, hotkeyMainkey, "32"), out key);
                        }
                        if (keys.Contains(hotkeyModifierAlt))
                        {
                            Boolean.TryParse(INIManipulator.GetValue(_path, category, hotkeyModifierAlt, "true"), out alt);
                        }
                        if (keys.Contains(hotkeyModifierCtrl))
                        {
                            Boolean.TryParse(INIManipulator.GetValue(_path, category, hotkeyModifierCtrl, "true"), out ctrl);
                        }
                        if (keys.Contains(hotkeyModifierShift))
                        {
                            Boolean.TryParse(INIManipulator.GetValue(_path, category, hotkeyModifierShift, "false"), out shift);
                        }
                        if (keys.Contains(hotkeyModifierWin))
                        {
                            Boolean.TryParse(INIManipulator.GetValue(_path, category, hotkeyModifierWin, "false"), out win);
                        }
                    }
                }
                _hotkey = new HotKey(alt, ctrl, shift, win, (Keys)key);
            }
            return _hotkey;
        }

        public HotKey GetAssistantHotKey()
        {
            if (_ahotkey == null)
            {
                int key = 20;
                List<string> categories = INIManipulator.GetCategories(_path);
                if (categories.Count > 0)
                {
                    string category = "interaction";
                    string hotkeyAssistantkey = "hotkeyAssistantkey";
                    if (categories.Contains(category))
                    {
                        List<string> keys = INIManipulator.GetKeys(_path, category);
                        if (keys.Contains(hotkeyAssistantkey))
                        {
                            Int32.TryParse(INIManipulator.GetValue(_path, category, hotkeyAssistantkey, "20"), out key);
                        }
                    }
                }
                _ahotkey = new HotKey(false, false, false, false, (Keys)key);
            }
            return _ahotkey;
        }

        public void SaveMainHotKey(HotKey key)
        {
            _hotkey = null;
            _hotkey = key;
            string category = "interaction";
            string hotkeyMainkey = "hotkeyMainkey";
            string hotkeyModifierAlt = "hotkeyModifierAlt";
            string hotkeyModifierCtrl = "hotkeyModifierCtrl";
            string hotkeyModifierShift = "hotkeyModifierShift";
            string hotkeyModifierWin = "hotkeyModifierWin";
            INIManipulator.WriteValue(_path, category, hotkeyMainkey, key.Key.ToString());
            INIManipulator.WriteValue(_path, category, hotkeyModifierAlt, key.IsAlt.ToString());
            INIManipulator.WriteValue(_path, category, hotkeyModifierCtrl, key.IsCtrl.ToString());
            INIManipulator.WriteValue(_path, category, hotkeyModifierShift, key.IsShift.ToString());
            INIManipulator.WriteValue(_path, category, hotkeyModifierWin, key.IsWin.ToString());
            category = null;
            hotkeyMainkey = null;
            hotkeyModifierAlt = null;
            hotkeyModifierCtrl = null;
            hotkeyModifierShift = null;
            hotkeyModifierWin = null;
        }

        public void SaveAssistantHotKey(HotKey key)
        {
            _ahotkey = null;
            _ahotkey = key;
            string category = "interaction";
            string hotkeyAssistantkey = "hotkeyAssistantkey";
            INIManipulator.WriteValue(_path, category, hotkeyAssistantkey, key.Key.ToString());
            category = null;
            hotkeyAssistantkey = null;
        }

        public LearnedContent GetLearnedContents()
        {
            if (_learned_contents == null)
            {
                LearnedContent learned_content = new LearnedContent();
                List<string> categories = INIManipulator.GetCategories(_path);
                if (categories.Count > 0)
                {
                    string category = "learned";
                    if (categories.Contains(category))
                    {
                        List<string> keys = INIManipulator.GetKeys(_path, category);
                        int key_len = keys.Count;
                        for (int i = 0; i < key_len; i++)
                        {
                            string name = keys[i];
                            string val = INIManipulator.GetValue(_path, category, name, string.Empty);
                            //string[] vals = StrToArray(INIManipulator.GetValue(_path, category, name, string.Empty));
                            learned_content.AddKeyword(name, val);
                        }
                        keys = null;
                    }
                }
                categories = null;
                _learned_contents = learned_content;
            }
            return _learned_contents;
        }

        public void SaveLearned(LearnedContent learned)
        {
            _learned_contents = null;
            _learned_contents = learned;
            string category = "learned";
            INIManipulator.DeleteCategory(_path, category);
            foreach (string keyword in learned.Keywords)
            {
                INIManipulator.WriteValue(_path, category, keyword, learned.Contents[keyword]); //ArrayToStr(learned.Contents[keyword].ToArray())
            }
        }

        public void AddLearned(string cmd, string content)
        {
            if (_learned_contents == null)
                _learned_contents = new LearnedContent();
            _learned_contents.AddKeyword(cmd, content);
            SaveLearned(_learned_contents);
        }

        public void RemoveLearned(string cmd, string content)
        {
            if (_learned_contents != null)
            {
                _learned_contents.RemoveKeyword(cmd, content);
                SaveLearned(_learned_contents);
            }
        }

        public void ClearLearned()
        {
            _learned_contents = new LearnedContent();
            SaveLearned(_learned_contents);
        }

        public PluginInfo GetLoadablePlugins()
        {
            if (_plugin_info == null)
            {
                PluginInfo plugin_info = new PluginInfo();
                List<string> categories = INIManipulator.GetCategories(_path);
                if (categories.Count > 0)
                {
                    string category = "plugins";
                    if (categories.Contains(category))
                    {
                        List<string> keys = INIManipulator.GetKeys(_path, category);
                        int key_len = keys.Count;
                        for (int i = 0; i < key_len; i++)
                        {
                            string name = keys[i];
                            bool val = false;
                            Boolean.TryParse(INIManipulator.GetValue(_path, category, name, "false"), out val);
                            plugin_info.Names.Add(name);
                            plugin_info.Enabled.Add(name, val);
                        }
                        keys = null;
                    }
                }
                categories = null;
                _plugin_info = plugin_info;
            }
            return _plugin_info;
        }

        public void SaveLoadablePlugins(PluginInfo info)
        {
            _plugin_info = info;
            string category = "plugins";
            INIManipulator.DeleteCategory(_path, category);
            foreach (string plugin in info.Names)
            {
                INIManipulator.WriteValue(_path, category, plugin, info.Enabled[plugin].ToString());
            }
        }

        public void RegisterPlugins(List<Plugin> plugins)
        {
            bool save = false;
            if (_plugin_info == null)
                _plugin_info = GetLoadablePlugins();
            foreach (Plugin plugin in plugins)
            {
                if (!_plugin_info.Names.Contains(plugin.Name))
                {
                    _plugin_info.Names.Add(plugin.Name);
                    _plugin_info.Enabled.Add(plugin.Name, true);
                    if (!save)
                        save = true;
                }
            }
            if (save)
                SaveLoadablePlugins(_plugin_info);
        }

        public InterfaceInfo GetInterfaceInfo()
        {
            if (_interface_info == null)
            {
                int number_of_suggestions = 10;
                Point location = new Point(-1, -1);
                List<string> categories = INIManipulator.GetCategories(_path);
                if (categories.Count > 0)
                {
                    string category = "interface";
                    if (categories.Contains(category))
                    {
                        List<string> keys = INIManipulator.GetKeys(_path, category);
                        if (keys.Count > 0)
                        {
                            string number_of_suggestions_name = "suggestions";
                            string location_x_name = "location_x";
                            string location_y_name = "location_y";
                            if (keys.Contains(number_of_suggestions_name))
                            {
                                Int32.TryParse(INIManipulator.GetValue(_path, category, number_of_suggestions_name, "10"), out number_of_suggestions);
                            }
                            if (keys.Contains(location_x_name) && keys.Contains(location_y_name))
                            {
                                int x, y;
                                Int32.TryParse(INIManipulator.GetValue(_path, category, location_x_name, "-1"), out x);
                                Int32.TryParse(INIManipulator.GetValue(_path, category, location_y_name, "-1"), out y);
                                location.X = x;
                                location.Y = y;
                            }
                        }
                    }
                }
                _interface_info = new InterfaceInfo(number_of_suggestions, location);
            }
            return _interface_info;
        }

        public void SaveInterfaceInfo(InterfaceInfo info)
        {
            _interface_info = info;
            string category = "interface";
            INIManipulator.WriteValue(_path, category, "suggestions", info.NumberOfSuggestions.ToString());
            INIManipulator.WriteValue(_path, category, "location_x", info.WindowLocation.X.ToString());
            INIManipulator.WriteValue(_path, category, "location_y", info.WindowLocation.Y.ToString());
        }

        public void SaveInterfaceInfo()
        {
            SaveInterfaceInfo(_interface_info);
        }

        public int GetNumberOfSuggestions()
        {
            if (_interface_info == null)
            {
                return GetInterfaceInfo().NumberOfSuggestions;
            }
            return _interface_info.NumberOfSuggestions;
        }

        public SystemOptionsInfo GetSystemOptionsInfo()
        {
            if (_system_options_info == null)
            {
                int update_time = 20;
                bool stop_auto_update = true;
                bool auto_update = true;
                List<string> categories = INIManipulator.GetCategories(_path);
                if (categories.Count > 0)
                {
                    string category = "system";
                    if (categories.Contains(category))
                    {
                        List<string> keys = INIManipulator.GetKeys(_path, category);
                        if (keys.Count > 0)
                        {
                            string update_time_name = "updateTime";
                            string stop_auto_update_name = "stopAutoUpdateOnBattery";
                            string auto_update_name = "autoUpdate";
                            if (keys.Contains(update_time_name))
                            {
                                Int32.TryParse(INIManipulator.GetValue(_path, category, update_time_name, "20"), out update_time);
                            }
                            if (keys.Contains(stop_auto_update_name))
                            {
                                Boolean.TryParse(INIManipulator.GetValue(_path, category, stop_auto_update_name, "true"), out stop_auto_update);
                            }
                            if (keys.Contains(auto_update_name))
                            {
                                Boolean.TryParse(INIManipulator.GetValue(_path, category, auto_update_name, "true"), out auto_update);
                            }
                        }
                    }
                }
                _system_options_info = new SystemOptionsInfo(update_time, stop_auto_update, auto_update);
            }
            return _system_options_info;
        }

        public void SaveSystemOptionsInfo(SystemOptionsInfo info)
        {
            _system_options_info = info;
            string category = "system";
            INIManipulator.WriteValue(_path, category, "updateTime", info.UpdateTime.ToString());
            INIManipulator.WriteValue(_path, category, "stopAutoUpdateOnBattery", info.StopAutoUpdateOnBattery.ToString());
            INIManipulator.WriteValue(_path, category, "autoUpdate", info.AutoUpdates.ToString());
        }

        public AutomationOptionsInfo GetAutomationOptionsInfo()
        {
            if (_automation_options_info == null)
            {
                bool is_monitoring = true;
                bool stop_monitoring = true;
                List<string> categories = INIManipulator.GetCategories(_path);
                if (categories.Count > 0)
                {
                    string category = "automation";
                    if (categories.Contains(category))
                    {
                        List<string> keys = INIManipulator.GetKeys(_path, category);
                        if (keys.Count > 0)
                        {
                            string is_monitoring_name = "monitoringEnabled";
                            string stop_monitoring_name = "stopMonitoringOnBattery";
                            if (keys.Contains(is_monitoring_name))
                            {
                                Boolean.TryParse(INIManipulator.GetValue(_path, category, is_monitoring_name, "true"), out is_monitoring);
                            }
                            if (keys.Contains(stop_monitoring_name))
                            {
                                Boolean.TryParse(INIManipulator.GetValue(_path, category, stop_monitoring_name, "true"), out stop_monitoring);
                            }
                        }
                    }
                }
                _automation_options_info = new AutomationOptionsInfo(is_monitoring, stop_monitoring);
            }
            return _automation_options_info;
        }

        public void SaveAutomationOptionsInfo(AutomationOptionsInfo info)
        {
            _automation_options_info = info;
            string category = "automation";
            INIManipulator.WriteValue(_path, category, "monitoringEnabled", info.IsMonitoringEnabled.ToString());
            INIManipulator.WriteValue(_path, category, "stopMonitoringOnBattery", info.StopAutoUpdateOnBattery.ToString());
        }

        public TimeSpan GetIndexingTime()
        {
            return _indexing_time;
        }

        public void SetIndexingTime(TimeSpan indexing)
        {
            _indexing_time = indexing;
        }

        public DateTime GetLastIndexBuild()
        {
            return _last_index_build;
        }

        public void SetLastIndexBuild(DateTime last_time)
        {
            _last_index_build = last_time;
        }

        public int GetNumberOfIndexedItems()
        {
            return _indexed_items;
        }

        public void SetNumberOfIndexedItems(int items)
        {
            _indexed_items = items;
        }

        public string[] GetScripts()
        {
            if (Directory.Exists(CommonInfo.ScriptsFolder))
            {
                string[] files = Directory.GetFiles(CommonInfo.ScriptsFolder, "*.py", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Path.GetFileNameWithoutExtension(files[i]);
                }
                return files;
            }
            else
                return new string[0];
        }
        #endregion

        #region Private Methods
        private string[] StrToArray(string str)
        {
            List<string> ret = new List<string>(str.Split(new char[] { ' ', ',' }));
            ret.RemoveAll(delegate(string s)
            {
                return s.Trim() == string.Empty;
            });
            return ret.ToArray();
        }

        private string ArrayToStr(string[] arr)
        {
            string ret = string.Empty;
            int len = arr.Length;
            for (int i = 0; i < len; i++)
            {
                if (i == len - 1)
                    ret += arr[i];
                else
                    ret += arr[i] + ", ";
            }
            return ret;
        }
        #endregion
    }
}
