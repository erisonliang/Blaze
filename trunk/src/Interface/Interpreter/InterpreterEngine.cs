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
using System.Threading;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.Settings;
using SystemCore.SystemAbstraction.FileHandling;
using SystemCore.SystemAbstraction.StringUtilities;
using Blaze.Indexer;
using Blaze.SystemBrowsing;
using Blaze.TextPrediction;
using Configurator;
using ContextLib;

namespace Blaze.Interpreter
{
    public class InterpreterEngine
    {
        #region Properties
        private MainForm _parent;
        private FileIndexer _fileIndexer;
        private SystemBrowser _systemBrowser;
        private MenuEngine _menuEngine;
        private List<InterpreterPlugin> _plugins;
        private PluginCommandCache _commandCache;
        private System.Timers.Timer _indexer_timer;
        private System.Timers.Timer _automation_timer;
        //private string[] _special_keywords;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public InterpreterEngine(MainForm parent)
        {
            _parent = parent;
            _fileIndexer = new FileIndexer();
            _systemBrowser = new SystemBrowser();
            _menuEngine = new MenuEngine(parent);
            _plugins = new List<InterpreterPlugin>();
            //_special_keywords = new string[] { @"!this", @"!clipboard", @"!actproc", @"!desktop", @"!explorer" };
            _indexer_timer = new System.Timers.Timer();
            _indexer_timer.Elapsed += new System.Timers.ElapsedEventHandler(_indexer_timer_Elapsed);
            _indexer_timer.AutoReset = true;
            if (SettingsManager.Instance.GetSystemOptionsInfo().UpdateTime > 0)
            {
                _indexer_timer.Interval = 1000 * 60 * SettingsManager.Instance.GetSystemOptionsInfo().UpdateTime;
                _indexer_timer.Start();
            }
            _automation_timer = new System.Timers.Timer();
            _automation_timer.Elapsed += new System.Timers.ElapsedEventHandler(_automation_timer_Elapsed);
            _automation_timer.Interval = 5000;
            _automation_timer.AutoReset = true;
            _automation_timer.Start();
        }
        #endregion

        void _indexer_timer_Elapsed(object sender, EventArgs e)
        {
            if (SettingsManager.Instance.GetSystemOptionsInfo().StopAutoUpdateOnBattery)
            {
                if (!UserContext.Instance.IsRunningOnBatteryPower())
                    BuildIndex();
            }
            else
                BuildIndex();
        }

        void _automation_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (SettingsManager.Instance.GetAutomationOptionsInfo().StopAutoUpdateOnBattery)
            {
                if (UserContext.Instance.IsRunningOnBatteryPower() && !UserContext.Instance.ObserverObject.IsRecording)
                    UserContext.Instance.ObserverObject.StopMonitoring();
                else if (SystemCore.Settings.SettingsManager.Instance.GetAutomationOptionsInfo().IsMonitoringEnabled)
                    UserContext.Instance.ObserverObject.StartMonitoring();
            }
        }

        #region Public Methods
        public void BuildIndex()
        {
            //while (true)
            //{
                // initialize
                Thread build_index = new Thread(new ThreadStart(delegate()
                {
                    Mutex mutex = new Mutex(false, CommonInfo.GUID + "-rebuild-index");
                    try
                    {
                        if (mutex.WaitOne(TimeSpan.Zero, true))
                        {
                            //_fileIndexer.LoadIndex();
                            //_parent.SetIndexing(true);
                            _fileIndexer.BuildIndex();
                            //_parent.SetIndexing(false);
                            //_fileIndexer.SaveIndex();
                            mutex.ReleaseMutex();
                            mutex.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        mutex.Close();
                        MessageBox.Show("build index error: "+e.Message);
                    }
                }));
                //build_index.SetApartmentState(ApartmentState.STA);
                //build_index.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                build_index.Start();
                //_fileIndexer.BuildIndex();

                // initialize menu
                Thread init_menu = new Thread(new ThreadStart(delegate() { _menuEngine.OnBuild(); }));
                init_menu.Start();

                // initialize plugins
                foreach (Plugin plugin in _plugins)
                {
                    if (plugin.Type == PluginType.Interpreter)
                    {
                        Thread init_plugin = new Thread(new ThreadStart(delegate() { ((InterpreterPlugin)plugin).OnBuild(); }));
                        init_plugin.Start();
                    }
                }
            //}
        }

        public void LoadIndex()
        {
            //Thread build_index = new Thread(new ThreadStart(delegate()
            //{
                
            //}));
            //build_index.Start();

            Mutex mutex = new Mutex(false, CommonInfo.GUID + "-rebuild-index");
            try
            {
                if (mutex.WaitOne(TimeSpan.Zero, true))
                {
                    _fileIndexer.LoadIndex();
                    mutex.ReleaseMutex();
                    mutex.Close();
                }
            }
            catch (Exception e)
            {
                mutex.Close();
                MessageBox.Show("load index error: " + e.Message);
            }
        }

        public void UnloadIndex()
        {
            //Thread build_index = new Thread(new ThreadStart(delegate()
            //{
                
            //}));
            //build_index.Start();

            Mutex mutex = new Mutex(false, CommonInfo.GUID + "-rebuild-index");
            try
            {
                if (mutex.WaitOne(TimeSpan.Zero, true))
                {
                    _fileIndexer.UnloadIndex();
                    mutex.ReleaseMutex();
                    mutex.Close();
                }
            }
            catch (Exception e)
            {
                mutex.Close();
                MessageBox.Show("unload index error: " + e.Message);
            }
        }

        public List<InterpreterItem> RetrieveTopItems(string text)
        {
            List<InterpreterItem> ret = new List<InterpreterItem>();
            string user_text = text.Trim().ToLower();
            List<string> assisting_commands = new List<string>();
            List<string> assisting_plugins = new List<string>();
            List<InterpreterPlugin> plugins = new List<InterpreterPlugin>(_plugins);
            plugins.Add(_menuEngine);
            _commandCache = new PluginCommandCache(plugins);
            plugins = null;
            bool filesystem_assisted = false;
            
            // take care of high priority commands
            foreach (InterpreterPlugin plugin in _plugins)
            {
                Command command = plugin.GetAssistingCommand(Command.PriorityType.High, text);
                if (command != null)
                {
                    InterpreterItem item = new InterpreterItem(plugin.GetItemName(command.Name, text, null),
                                                                plugin.GetItemDescription(command.Name, text, null),
                                                                OwnerType.Plugin,
                                                                plugin.GetItemAutoComplete(command.Name, text, null),
                                                                plugin.GetItemIcon(command.Name, text, null));
                    item.OwnerId = plugin.Name;
                    item.CommandName = command.Name;
                    item.CommandTokens = null;
                    item.CommandUsage = plugin.GetUsage(command.Name, text, null);
                    item.Text = text;
                    assisting_commands.Add(plugin.Name);
                    ret.Add(item);
                    item = null;
                }
            }

            if (_systemBrowser.IsOwner(user_text))
            {
                List<string> user_tokens = new List<string>();
                user_tokens.Add(user_text.ToLower());

                Dictionary<string, double> distances = new Dictionary<string, double>();
                Dictionary<string, string[]> displacements = new Dictionary<string, string[]>();
                List<string> suited_names = new List<string>();
                List<string> min_names = new List<string>();

                List<string> paths = _systemBrowser.RetrieveItems(text);
                paths.Sort();

                // take care of learned contents
                LearnedContent lc = new LearnedContent(SettingsManager.Instance.GetLearnedContents());
                List<string> lc_temp = new List<string>(lc.Keywords);
                lc_temp.Reverse();
                foreach (string keyword in lc_temp)
                {
                    if (StringUtility.WordContainsStr(keyword, user_text))
                    {
                        string content = lc.Contents[keyword];
                        // file system
                        if (_systemBrowser.IsOwner(content) && paths.Contains(content))
                        {
                            InterpreterItem item = new InterpreterItem(FileNameManipulator.GetFolderName(content), content, OwnerType.FileSystem, content, _systemBrowser.GetItemIcon(content));
                            item.Type = ItemType.Learned;
                            ret.Add(item);
                            paths.Remove(content);
                        }
                    }
                }
                lc = null;
                lc_temp = null;

                Dictionary<string, List<string>> keywords = new Dictionary<string, List<string>>();
                foreach (string s in paths)
                    keywords.Add(s, new List<string>(new string[] { s.ToLower() }));

                TextPredictor.Instance.PredictNamesAndDistance(user_text, user_tokens, paths, keywords, ref suited_names, ref distances, ref displacements, false);

                while (suited_names.Count > 0)
                {
                    string min_name = string.Empty;
                    for (int i = 0; i < suited_names.Count; i++)
                    {
                        if (min_name == string.Empty)
                        {
                            min_name = suited_names[i];
                        }
                        else if (distances[min_name] > distances[suited_names[i]])
                        {
                            min_name = suited_names[i];
                        }
                    }
                    if (min_name != string.Empty)
                    {
                        min_names.Add(min_name);
                        suited_names.Remove(min_name);
                    }
                }

                foreach (string s in min_names)
                {
                    ret.Add(new InterpreterItem(FileNameManipulator.GetFolderName(s), s, OwnerType.FileSystem, s, _systemBrowser.GetItemIcon(s)));
                }

                // clean up
                user_tokens = null;
                distances = null;
                suited_names = null;
                min_names = null;
                paths = null;
                keywords = null;

                filesystem_assisted = true;
            }
            if (!filesystem_assisted)
            {
                List<string> user_tokens = new List<string>(StringUtility.GenerateKeywords(user_text));
                int user_tokens_size = user_tokens.Count;

                Index index = new Index(_fileIndexer.RetrieveItems());
                //if (index.Names.Count == 0)
                //    return ret;
                List<string> names = new List<string>(index.Names);
                Dictionary<string, List<string>> paths = new Dictionary<string, List<string>>(index.Paths);
                Dictionary<string, List<string>> keywords = new Dictionary<string, List<string>>(index.Keywords);

                //// add blaze commands to index
                //List<string> menu_options = new List<string>();
                //Dictionary<string, List<string>> menu_keywords = new Dictionary<string, List<string>>();
                //foreach (string option in _menuEngine.Names)
                //{
                //    menu_options.Add(option + " " + CommonInfo.GUID);
                //    menu_keywords.Add(option + " " + CommonInfo.GUID, _menuEngine.Keywords[option]);
                //}
                //foreach (string option in menu_options)
                //{
                //    names.Add(option);
                //    paths.Add(option, new List<string>());
                //    keywords.Add(option, menu_keywords[option]);
                //}

                // add plugin commands to index
                names.AddRange(_commandCache.Index.Names);
                foreach (KeyValuePair<string, List<string>> pair in _commandCache.Index.Paths)
                    paths.Add(pair.Key, pair.Value);
                foreach (KeyValuePair<string, List<string>> pair in _commandCache.Index.Keywords)
                    keywords.Add(pair.Key, pair.Value);

                // take care of learned contents
                LearnedContent lc = new LearnedContent(SettingsManager.Instance.GetLearnedContents());
                List<string> lc_temp = new List<string>(lc.Keywords);
                lc_temp.Reverse();
                InterpreterPlugin ap;
                foreach (string keyword in lc_temp)
                {
                    //if (StringUtility.WordContainsStr(keyword, user_text))
                    //{
                        string content = lc.Contents[keyword];
                        // menu
                        if (_menuEngine.IsOwner(UnprotectCommand(content)))
                        {
                            //if (StringUtility.WordContainsStr(keyword, user_text))
                            //{
                            //    string ucontent = UnprotectCommand(content);
                            //    InterpreterItem item = new InterpreterItem(_menuEngine.GetItemName(ucontent, text, null),
                            //                                                _menuEngine.GetItemDescription(ucontent, text, null),
                            //                                                OwnerType.Menu,
                            //                                                _menuEngine.GetItemAutoComplete(ucontent, text, null),
                            //                                                _menuEngine.GetItemIcon(ucontent, text, null));
                            //    item.Type = ItemType.Learned;
                            //    ret.Add(item);
                            //    names.Remove(content);
                            //    paths.Remove(content);
                            //    keywords.Remove(content);
                            //}
                            if (!keywords[content].Contains(keyword))
                                keywords[content].Add(keyword);
                        }
                        else if ((ap = GetAssistingPlugin(UnprotectCommand(content), keyword, Command.PriorityType.Medium)) != null /*&& !assisting_plugins.Contains(ap.Name)*/)
                        {
                            string command_name = UnprotectCommand(content);
                            Command command = ap.GetCommandByName(command_name);
                            if (command != null && command.FitsPriority(Command.PriorityType.Medium) && !assisting_commands.Contains(command_name))
                            {

                                //InterpreterItem item = new InterpreterItem(ap.GetItemName(command_name, keyword, null),
                                //                                            ap.GetItemDescription(command_name, keyword, null),
                                //                                            OwnerType.Plugin,
                                //                                            ap.GetItemAutoComplete(command_name, keyword, null),
                                //                                            ap.GetItemIcon(command_name, keyword, null));
                                //item.OwnerId = ap.Name;
                                //item.CommandName = command_name;
                                //item.CommandTokens = null;
                                //item.Text = text;
                                //assisting_commands.Add(command_name);
                                //assisting_plugins.Add(ap.Name);
                                //ret.Add(item);
                                //item = null;
                                if (!keywords[content].Contains(keyword))
                                    keywords[content].Add(keyword);
                            }
                        }
                        else if (File.Exists(content)) // indexer
                        {
                            if (StringUtility.WordContainsStr(keyword, user_text))
                            {
                                string name = FileNameManipulator.GetFileName(content);
                                List<string> list;
                                try
                                {
                                    list = paths[name];
                                }
                                catch (Exception)
                                {
                                    //SettingsManager.Instance.RemoveLearned(keyword, content);
                                    continue;
                                }
                                for (int i = 0; i < list.Count; i++)
                                {
                                    InterpreterItem item = new InterpreterItem(name, list[i], OwnerType.Indexer, name, i, _fileIndexer.GetItemIcon(name, i));
                                    item.Type = ItemType.Learned;
                                    ret.Add(item);
                                }
                                names.Remove(name);
                                paths.Remove(name);
                                keywords.Remove(name);
                            }
                        }
                        //else
                        //{
                        //    if (StringUtility.WordContainsStr(keyword, user_text))
                        //    {
                        //        SettingsManager.Instance.RemoveLearned(keyword, content);
                        //    }
                        //}
                    //}
                }
                lc = null;
                lc_temp = null;
                ap = null;

                int names_size = names.Count;
                int paths_size = paths.Count;
                int keywords_size = keywords.Count;

                Dictionary<string, double> distances = new Dictionary<string, double>();
                Dictionary<string, string[]> tokens = new Dictionary<string, string[]>();
                List<string> suited_names = new List<string>();
                List<string> min_names = new List<string>();
                List<string> visited = new List<string>();

                TextPredictor.Instance.PredictNamesAndDistance(user_text, user_tokens, names, keywords, ref suited_names, ref distances, ref tokens, true);

                int limit = SettingsManager.Instance.GetNumberOfSuggestions();
                for (int num = 0; num < limit; num++)
                {
                    string min_name = string.Empty;
                    for (int i = 0; i < suited_names.Count; i++)
                    {
                        if (visited.Contains(suited_names[i]))
                        {
                            continue;
                        }
                        else if (min_name == string.Empty)
                        {
                            min_name = suited_names[i];
                        }
                        else if (distances[min_name] > distances[suited_names[i]])
                        {
                            min_name = suited_names[i];
                        }
                        else if (distances[min_name] == distances[suited_names[i]])
                        {
                            if (min_name.Length > suited_names[i].Length)
                                min_name = suited_names[i];
                        }
                    }
                    if (min_name != string.Empty)
                    {
                        min_names.Add(min_name);
                        visited.Add(min_name);
                    }
                }

                foreach (string s in min_names)
                {
                    // take care of medium priority commands
                    foreach (InterpreterPlugin plugin in _plugins)
                    {
                        string command_name = UnprotectCommand(s);
                        if (command_name == string.Empty)
                             continue;
                        Command command = plugin.GetCommandByName(command_name);
                        if (command != null && command.FitsPriority(Command.PriorityType.Medium) /*&& !assisting_plugins.Contains(plugin.Name)*/ && !assisting_commands.Contains(command_name))
                        {
                            InterpreterItem item = new InterpreterItem(plugin.GetItemName(command_name, text, tokens[s]),
                                                                        plugin.GetItemDescription(command_name, text, tokens[s]),
                                                                        OwnerType.Plugin,
                                                                        plugin.GetItemAutoComplete(command_name, text, tokens[s]),
                                                                        plugin.GetItemIcon(command_name, text, tokens[s]));
                            item.OwnerId = plugin.Name;
                            item.CommandName = command_name;
                            item.CommandTokens = tokens[s];
                            item.CommandUsage = plugin.GetUsage(command.Name, text, tokens[s]);
                            item.Text = text;
                            assisting_commands.Add(command_name);
                            //assisting_plugins.Add(plugin.Name);
                            ret.Add(item);
                            item = null;
                        }
                    }
                    if (_menuEngine.IsOwner(UnprotectCommand(s)))
                    {
                        string ucontent = UnprotectCommand(s);
                        InterpreterItem item = new InterpreterItem(_menuEngine.GetItemName(ucontent, text, tokens[s]),
                                                    _menuEngine.GetItemDescription(ucontent, text, tokens[s]),
                                                    OwnerType.Menu,
                                                    _menuEngine.GetItemAutoComplete(ucontent, text, tokens[s]),
                                                    _menuEngine.GetItemIcon(ucontent, text, tokens[s]));
                        item.CommandName = ucontent;
                        item.CommandTokens = tokens[s];
                        item.CommandUsage = _menuEngine.GetUsage(ucontent, text, tokens[s]);
                        item.Text = text;
                        ret.Add(item);
                        item = null;
                    }
                    else
                    {
                        List<string> list = paths[s];
                        for (int i = 0; i < list.Count; i++)
                            ret.Add(new InterpreterItem(s, list[i], OwnerType.Indexer, s, i, _fileIndexer.GetItemIcon(s, i)));
                    }
                }

                //clean up
                user_tokens = null;
                paths = null;
                keywords = null;
                names = null;
                distances = null;
                suited_names = null;
                min_names = null;
                visited = null;
            }

            // take care of low priority commands
            foreach (InterpreterPlugin plugin in _plugins)
            {
                Command command = plugin.GetAssistingCommand(Command.PriorityType.Low, text);
                if (command != null /*&& !assisting_plugins.Contains(plugin.Name)*/ && !assisting_commands.Contains(command.Name))
                {
                    InterpreterItem item = new InterpreterItem(plugin.GetItemName(command.Name, text, null),
                                                                plugin.GetItemDescription(command.Name, text, null),
                                                                OwnerType.Plugin,
                                                                plugin.GetItemAutoComplete(command.Name, text, null),
                                                                plugin.GetItemIcon(command.Name, text, null));
                    item.OwnerId = plugin.Name;
                    item.CommandName = command.Name;
                    item.CommandTokens = null;
                    item.CommandUsage = plugin.GetUsage(command.Name, text, null);
                    item.Text = text;
                    assisting_commands.Add(command.Name);
                    //assisting_plugins.Add(plugin.Name);
                    ret.Add(item);
                    item = null;
                }
            }
            //ret.Add(new InterpreterItem(_webEngine.GetItemName(user_text), _webEngine.GetItemDescription(user_text), OwnerType.Web, _webEngine.GetItemAutoComplete(user_text), _webEngine.GetItemIconPtr(user_text)));

            user_text = null;
            assisting_commands = null;
            return ret;
        }

        public void Execute(string cmd, InterpreterItem item, Keys modifiers)
        {
            StoreKeywords(cmd, item);
            //string[] tiles = EnumDesktopWindowsDemo.GetDesktopWindowsCaptions();
            //WindowUtility.Instance.Refresh();
            //List<VWindow> windows = WindowUtility.Instance.Windows();
            //StreamWriter writer = new StreamWriter("log.txt");
            //for (int i = 1; i <= windows.Count; i++)
            //{
            //    writer.WriteLine("{0}) {1}", i,windows[i-1].GetTitle());
            //    writer.WriteLine("Selected text: {0}", windows[i - 1].GetSelectedText());
            //}
            //writer.Close();
            //WindowUtility.Instance.SendStringToWindow(WindowUtility.Instance.GetTopWindow().Handle, "foca");
            //MessageBox.Show("edit: " + WindowUtility.Instance.GetText(WindowUtility.Instance.GetTopWindow()));
            //WindowUtility.Instance.SendCtrlCharToWindow(WindowUtility.Instance.GetTopWindow().Handle, 'c');
            //MessageBox.Show(Clipboard.GetText());
            //MessageBox.Show("Selected text: " + UserContext.Instance.RetrieveSelectedText());
            item.Text = ReplaceSpecialKeywords(item.Text, item.CommandTokens);
            switch (item.OwnerType)
            {
                case OwnerType.Indexer:
                    //StoreKeywords(cmd, item);
                    _fileIndexer.Execute(item, modifiers);
                    _parent.HideAutomator();
                    break;
                case OwnerType.FileSystem:
                    //StoreKeywords(cmd, item);
                    _systemBrowser.Execute(item, modifiers);
                    _parent.HideAutomator();
                    break;
                case OwnerType.Menu:
                    //StoreKeywords(cmd, item);
                    if (_menuEngine.Execute(item, modifiers))
                        _parent.HideAutomator();
                    break;
                case OwnerType.Plugin:
                    StoreKeywords(cmd, item);
                    foreach (InterpreterPlugin plugin in _plugins)
                    {
                        if (plugin.Name == item.OwnerId)
                        {
                            if (plugin.Execute(item, modifiers))
                                _parent.HideAutomator();
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
            //Clipboard.SetData(DataFormats.Html, HtmlFragment.));
            //HtmlFragment.CopyToClipboard(WindowUtility.Instance.GetContent(WindowUtility.Instance.GetTopWindow()));
            //Clipboard.SetText(WindowUtility.Instance.GetContent(WindowUtility.Instance.GetTopWindow()), TextDataFormat.Html);
            //string files = string.Empty;
            //string[] list = WindowUtility.Instance.GetTopWindow().GetSelectedContet();
            //foreach (string file in list)
            //    files += file + ";";
            //MessageBox.Show(WindowUtility.Instance.GetTopWindow().GetUrl());
            //MessageBox.Show(files);
            //Clipboard.SetData(DataFormats.Html, Clipboard.GetText());
        }

        public void LoadPlugins(List<Plugin> plugins)
        {
            List<IndexerPlugin> indexerPlugins = new List<IndexerPlugin>();
            List<InterpreterPlugin> interpreterPlugins = new List<InterpreterPlugin>();
            foreach (Plugin plugin in plugins)
            {
                if (plugin.Activated)
                {
                    if (plugin.Type == PluginType.Indexer)
                        indexerPlugins.Add((IndexerPlugin)plugin);
                    else
                        interpreterPlugins.Add((InterpreterPlugin)plugin);
                }
            }
            _fileIndexer.LoadPlugins(indexerPlugins);
            _plugins = interpreterPlugins;
            //_commandCache = new PluginCommandCache(_plugins);
        }

        public void SetUpdateTimerInterval(int minutes)
        {
            if (minutes > 0)
            {
                _indexer_timer.Interval = minutes * 60 * 1000;
                if (!_indexer_timer.Enabled)
                    _indexer_timer.Start();
            }
            else if (_indexer_timer.Enabled)
            {
                _indexer_timer.Stop();
            }
        }
        #endregion

        #region Private Methods
        private void StoreKeywords(string cmd, InterpreterItem item)
        {
            string str = cmd.ToLower();
            //if (str != item.AutoComplete.ToLower())
            //{
            if (item.OwnerType == OwnerType.Indexer)
            {
                if (str != item.Desciption.ToLower())
                    SettingsManager.Instance.AddLearned(str, item.Desciption);
            }
            else if (item.OwnerType == OwnerType.Menu)
            {
                //SettingsManager.Instance.AddLearned(str, ProtectCommand(item.Name));
                //SettingsManager.Instance.AddLearned(item.Text, Command.ProtectCommand(item.CommandName)); // StringUtility.ArrayToStr(item.CommandTokens), item.CommandName
            }
            else if (item.OwnerType == OwnerType.Plugin)
            {
                //InterpreterPlugin p = _plugins.Find(delegate(InterpreterPlugin plugin) { return plugin.Name == item.OwnerId; });
                //Command command = p.GetCommand(item);
                //if (command == null)
                //    SettingsManager.Instance.AddLearned(str, str);
                //else if (command.FitsPriority(Command.PriorityType.Medium))
                //    SettingsManager.Instance.AddLearned(item.Text, command.ProtectedName);//SettingsManager.Instance.AddLearned(StringUtility.ArrayToStr(item.CommandTokens), command.ProtectedName);
                //else if (command.FitsPriority(Command.PriorityType.Low))
                //    SettingsManager.Instance.AddLearned(item.Text, command.ProtectedName);
                //else if (command.FitsPriority(Command.PriorityType.Medium))
                //    SettingsManager.Instance.AddLearned(BuildCommandParameters(item.Text, item.CommandTokens), command.ProtectedName);
            }
            //else
            //{
            //    //if (str != item.AutoComplete.ToLower())
            //    SettingsManager.Instance.AddLearned(str, item.AutoComplete);
            //}
            //}
        }

        private InterpreterPlugin GetAssistingPlugin(string cmd, string parameters, Command.PriorityType priority)
        {
            if (parameters.Trim() == string.Empty)
            {
                return null;
            }
            else
            {
                foreach (InterpreterPlugin plugin in _plugins)
                {
                    Command command = plugin.GetCommandByName(cmd);
                    if (command != null && command.FitsPriority(priority) && command.IsOwner(parameters))
                    {
                        return plugin;
                    }
                }
                return null;
            }
        }

        private string UnprotectCommand(string item)
        {
            if (item.Contains(CommonInfo.GUID))
                return item.Replace(CommonInfo.GUID, string.Empty).Trim();
            else
                return string.Empty;
        }

        private string ProtectCommand(string item)
        {
            return item + " " + CommonInfo.GUID;
        }

        private string BuildCommandParameters(string item, string[] tokens)
        {
            string parameters = item;
            if (tokens != null)
            {
                foreach (string token in tokens)
                    parameters = StringUtility.ReplaceFirstOccurrenceNoCaps(parameters, token, string.Empty);
                parameters = parameters.Replace("  ", " ");
            }
            return parameters.Trim();
        }

        private string ReplaceSpecialKeywords(string text, string[] tokens)
        {
            string ret = text;
            string this_keyword = @"!this";
            string clipboard_keyword = @"!clipboard";
            string process_keyword = @"!topapp";
            string desktop_keyword = @"!desktop";
            string explorer_keyword = @"!here";
            string item_keyword = @"!selected";
            string url_keyword = @"!url";
            bool look_for_this = true;
            bool look_for_clipboard = true;
            bool look_for_process = true;
            bool look_for_desktop = true;
            bool look_for_explorer = true;
            bool look_for_item = true;
            bool look_for_url = true;
            if (tokens != null)
            {
                if (Array.IndexOf<string>(tokens, this_keyword) != -1)
                    look_for_this = false;
                if (Array.IndexOf<string>(tokens, clipboard_keyword) != -1)
                    look_for_clipboard = false;
                if (Array.IndexOf<string>(tokens, process_keyword) != -1)
                    look_for_process = false;
                if (Array.IndexOf<string>(tokens, desktop_keyword) != -1)
                    look_for_desktop = false;
                if (Array.IndexOf<string>(tokens, explorer_keyword) != -1)
                    look_for_explorer = false;
                if (Array.IndexOf<string>(tokens, item_keyword) != -1)
                    look_for_item = false;
                if (Array.IndexOf<string>(tokens, url_keyword) != -1)
                    look_for_url = false;
            }
            if (!look_for_this && !look_for_clipboard && !look_for_process &&
                !look_for_desktop && !look_for_explorer && !look_for_item &&
                !look_for_url)
                return text;
            if (look_for_this)
            {
                if (text.Contains(this_keyword))
                {
                    ContextLib.DataContainers.Multimedia.MultiLevelData data = UserContext.Instance.GetSelectedContent();
                    if (data.Text != null)
                        ret = ret.Replace(this_keyword, data.Text);
                    data.Dispose();
                }
            }
            if (look_for_clipboard)
            {
                if (text.Contains(clipboard_keyword))
                {
                    ContextLib.DataContainers.Multimedia.MultiLevelData data = UserContext.Instance.GetClipboardContent();
                    if (data.Text != null)
                        ret = ret.Replace(clipboard_keyword, data.Text);
                    data.Dispose();
                }
            }
            if (look_for_process)
            {
                if (text.Contains(process_keyword))
                {
                    ContextLib.DataContainers.GUI.Window window = UserContext.Instance.GetTopWindow();
                    if (window != null)
                        ret = ret.Replace(process_keyword, window.ProcessName);
                }
            }
            if (look_for_desktop)
            {
                if (text.Contains(desktop_keyword))
                {
                    ret = ret.Replace(desktop_keyword, CommonInfo.UserDesktop);
                }
            }
            if (look_for_explorer)
            {
                if (text.Contains(explorer_keyword))
                {
                    ret = ret.Replace(explorer_keyword, UserContext.Instance.GetExplorerPath(false));
                }
            }
            if (look_for_item)
            {
                if (text.Contains(item_keyword))
                {
                    ret = ret.Replace(item_keyword, StringUtility.ArrayToStr(UserContext.Instance.GetExplorerSelectedItems(false)));
                }
            }
            if (look_for_url)
            {
                if (text.Contains(url_keyword))
                {
                    ret = ret.Replace(url_keyword, UserContext.Instance.GetBrowserUrl());
                }
            }
            //string[] args = StringUtility.GenerateKeywords(text, false);
            //for (int i = 0; i < args.Length; i++)
            //{
            //    string arg = args[i].Trim(new char[] { '\"', '\'', ',', ';' });
            //    if (arg == this_keyword)
            //    {
            //        if (look_for_this)
            //        {
            //            ContextLib.DataContainers.Multimedia.MultiLevelData data = UserContext.Instance.GetSelectedContent();
            //            if (data.Text != null)
            //                args[i] = data.Text;
            //            else
            //                args[i] = string.Empty;
            //            data.Dispose();
            //        }
            //    }
            //    else if (arg == clipboard_keyword)
            //    {
            //        if (look_for_clipboard)
            //        {
            //            ContextLib.DataContainers.Multimedia.MultiLevelData data = UserContext.Instance.GetClipboardContent();
            //            if (data.Text != null)
            //                args[i] = data.Text;
            //            else
            //                args[i] = string.Empty;
            //            data.Dispose();
            //        }
            //    }
            //    else if (arg == process_keyword)
            //    {
            //        if (look_for_process)
            //        {
            //            ContextLib.DataContainers.GUI.Window window = UserContext.Instance.GetTopWindow();
            //            if (window != null)
            //                args[i] = window.ProcessName;
            //        }
            //    }
            //    else if (arg == desktop_keyword)
            //    {
            //        if (look_for_desktop)
            //        {
            //            return CommonInfo.UserDesktop;
            //        }
            //    }
            //    else if (arg == explorer_keyword)
            //    {
            //        if (look_for_explorer)
            //        {
            //            args[i] = UserContext.Instance.GetExplorerPath(false);
            //        }
            //    }
            //    else if (arg == item_keyword)
            //    {
            //        if (look_for_item)
            //        {
            //            string items = StringUtility.ArrayToStr(UserContext.Instance.GetExplorerSelectedFiles(false));
            //            args[i] = items;
            //        }
            //    }
            //}
            //foreach (string arg in args)
            //    ret += arg + " ";
            //return ret.TrimEnd();
            return ret;
        }
        #endregion
    }
}
