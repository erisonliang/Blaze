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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.Settings;
using SystemCore.SystemAbstraction;
using Blaze.Settings;
using Configurator;
using ContextLib;

namespace Blaze
{
    public partial class SettingsForm : Form
    {
        #region Properties
        private MainForm _parent;
        private Dictionary<string, HotKeyModifiers> _modifiers;
        private Dictionary<string, Keys> _keys;
        private HotKey _hotkey;
        private Dictionary<string, Keys> _akeys;
        private HotKey _ahotkey;
        private List<string> _directories;
        private Dictionary<string, List<string>> _extensions;
        private Dictionary<string, bool> _indexSubdirectories;
        private TypePicker _typePicker;
        private FolderBrowserDialog _directoryPicker;
        private Dictionary<string, List<string>> _dirPlugins;
        private Dictionary<string, int> _plugins_map_options;
        private Dictionary<int, string> _options_map_plugins;
        private List<Plugin> _plugins;
        private List<IndexerPlugin> _indexerPlugins;
        private List<InterpreterPlugin> _interpreterPlugins;
        private PluginInfo _pluginInfo;
        private InterfaceInfo _interface_info;
        private SystemOptionsInfo _system_options_info;
        #endregion

        #region Assembly Attribute Accessors
        public string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // If there aren't any Description attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Description attribute, return its value
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // If there aren't any Product attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Product attribute, return its value
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // If there aren't any Company attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Company attribute, return its value
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        #region Constructors
        public SettingsForm(MainForm parent)
        {
            InitializeComponent();
            DirectoriesListBox.SelectedIndexChanged += new EventHandler(DirectoriesListBox_SelectedIndexChanged);
            OptionsListBox.ItemCheck += new ItemCheckEventHandler(OptionsListBox_ItemCheck);
            PluginsListBox.SelectedIndexChanged += new EventHandler(PluginsListBox_SelectedIndexChanged);
            PluginsListBox.ItemCheck += new ItemCheckEventHandler(PluginsListBox_ItemCheck);
            _parent = parent;
            Owner = parent;
            HomePageLink.LinkClicked += new LinkLabelLinkClickedEventHandler(HomePageLink_LinkClicked);
        }
        #endregion

        #region EventHandling
        void DirectoriesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DirectoriesListBox.SelectedIndex != ListBox.NoMatches)
            {
                FileTypesListBox.Items.Clear();
                FileTypesListBox.Items.AddRange(_extensions[DirectoriesListBox.SelectedItem.ToString()].ToArray());
                //IncludeSubDirCheckBox.Checked = _indexSubdirectories[DirectoriesListBox.SelectedItem.ToString()];
                OptionsListBox.SetItemChecked(0, _indexSubdirectories[DirectoriesListBox.SelectedItem.ToString()]);
                string item = DirectoriesListBox.SelectedItem.ToString();
                for (int i = 0; i < _indexerPlugins.Count; i++)
                {
                    string plugin = _indexerPlugins[i].Name;
                    if (_dirPlugins[item].Contains(plugin))
                    {
                        OptionsListBox.SetItemChecked(_plugins_map_options[plugin], true);
                    }
                    else
                    {
                        OptionsListBox.SetItemChecked(_plugins_map_options[plugin], false);
                    }
                }
            }
        }

        void OptionsListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (DirectoriesListBox.SelectedIndex != ListBox.NoMatches)
            {
                string item = DirectoriesListBox.SelectedItem.ToString();
                if (e.Index == 0)
                {
                    _indexSubdirectories[item] = (e.NewValue == CheckState.Checked ? true : false);
                }
                else
                {
                    string plugin = _options_map_plugins[e.Index];
                    if (e.NewValue == CheckState.Checked)
                    {
                        if (!_dirPlugins[item].Contains(plugin))
                            _dirPlugins[item].Add(plugin);
                    }
                    else
                    {
                        if (_dirPlugins[item].Contains(plugin))
                            _dirPlugins[item].Remove(plugin);
                    }
                }
            }
        }

        void PluginsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PluginsListBox.Items.Count > 0 && PluginsListBox.SelectedIndex != ListBox.NoMatches)
            {
                int index = PluginsListBox.SelectedIndex;
                PluginNameEditableLabel.Text = _parent.Plugins[index].Name;
                PluginDescriptionEditableLayer.Text = _parent.Plugins[index].Description;
                PluginVersionEditableLabel.Text = _parent.Plugins[index].Version;
                PluginWebsiteEditableLabel.Text = _parent.Plugins[index].Website;
                if (_plugins[index].Configurable)
                    ConfigurePluginButton.Enabled = true;
                else
                    ConfigurePluginButton.Enabled = false;
            }
        }

        void PluginsListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string item = PluginsListBox.Items[e.Index].ToString();
            _pluginInfo.Enabled[item] = (e.NewValue == CheckState.Checked ? true : false);
        }


        void HomePageLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://blaze-wins.sourceforge.net/");
        }
        #endregion

        #region Private Methods
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            _modifiers = new Dictionary<string, HotKeyModifiers>();
            _keys = new Dictionary<string, Keys>();
            _akeys = new Dictionary<string, Keys>();
            OptionsListBox.CheckOnClick = false;

            //
            // Blaze HotKey
            //
            _hotkey = SettingsManager.Instance.GetMainHotKey();
            // modifiers
            _modifiers.Add("None", new HotKeyModifiers(false, false, false, false));
            _modifiers.Add("Alt", new HotKeyModifiers(true, false, false, false));
            _modifiers.Add("Ctrl", new HotKeyModifiers(false, true, false, false));
            _modifiers.Add("Shift", new HotKeyModifiers(false, false, true, false));
            _modifiers.Add("Win", new HotKeyModifiers(false, false, false, true));

            _modifiers.Add("Alt+Ctrl", new HotKeyModifiers(true, true, false, false));
            _modifiers.Add("Alt+Shift", new HotKeyModifiers(true, false, true, false));
            _modifiers.Add("Alt+Win", new HotKeyModifiers(true, false, false, true));

            _modifiers.Add("Ctrl+Shift", new HotKeyModifiers(false, true, true, false));
            _modifiers.Add("Ctrl+Win", new HotKeyModifiers(false, true, false, true));

            _modifiers.Add("Shift+Win", new HotKeyModifiers(false, false, true, true));

            // keys
            _keys.Add("Space", Keys.Space);
            _keys.Add("A", Keys.A);
            _keys.Add("B", Keys.B);
            _keys.Add("C", Keys.C);
            _keys.Add("D", Keys.D);
            _keys.Add("E", Keys.E);
            _keys.Add("F", Keys.F);
            _keys.Add("G", Keys.G);
            _keys.Add("H", Keys.H);
            _keys.Add("I", Keys.I);
            _keys.Add("J", Keys.J);
            _keys.Add("K", Keys.K);
            _keys.Add("L", Keys.L);
            _keys.Add("M", Keys.M);
            _keys.Add("N", Keys.N);
            _keys.Add("O", Keys.O);
            _keys.Add("P", Keys.P);
            _keys.Add("Q", Keys.Q);
            _keys.Add("R", Keys.R);
            _keys.Add("S", Keys.S);
            _keys.Add("T", Keys.T);
            _keys.Add("U", Keys.U);
            _keys.Add("V", Keys.V);
            _keys.Add("W", Keys.W);
            _keys.Add("X", Keys.X);
            _keys.Add("Y", Keys.Y);
            _keys.Add("Z", Keys.Z);
            _keys.Add("0", Keys.D0);
            _keys.Add("1", Keys.D1);
            _keys.Add("2", Keys.D2);
            _keys.Add("4", Keys.D4);
            _keys.Add("5", Keys.D5);
            _keys.Add("6", Keys.D6);
            _keys.Add("8", Keys.D8);
            _keys.Add("9", Keys.D9);
            _keys.Add("Pause", Keys.Pause);
            _keys.Add("Scroll Lock", Keys.Scroll);
            _keys.Add("Caps Lock", Keys.CapsLock);
            _keys.Add("Num Lock", Keys.NumLock);

            string[] modifiers = new string[_modifiers.Keys.Count];
            string[] keys = new string[_keys.Keys.Count];
            _modifiers.Keys.CopyTo(modifiers, 0);
            _keys.Keys.CopyTo(keys, 0);

            Keys[] keys_codes = new Keys[_keys.Values.Count];
            _keys.Values.CopyTo(keys_codes, 0);

            ModifierComboBox.Items.AddRange(modifiers);
            ModifierComboBox.SelectedIndex = 0;
            MainKeyComboBox.Items.AddRange(keys);
            MainKeyComboBox.SelectedIndex = 0;
            // select user key
            ModifierComboBox.SelectedIndex = Array.FindIndex<string>(modifiers, delegate(string s)
                {
                    return s == _hotkey.ModifiersName;
                });
            MainKeyComboBox.SelectedIndex = Array.FindIndex<Keys>(keys_codes, delegate(Keys s)
                {
                    return s == (Keys)_hotkey.Key;
                });
            //

            //
            // Assistant HotKey
            //
            _ahotkey = SettingsManager.Instance.GetAssistantHotKey();

            // keys
            _akeys.Add("None", Keys.None);
            _akeys.Add("Caps Lock", Keys.CapsLock);
            _akeys.Add("Pause", Keys.Pause);
            _akeys.Add("Scroll Lock", Keys.Scroll);
            _akeys.Add("Num Lock", Keys.NumLock);
            _akeys.Add("Left Alt", Keys.LMenu);
            _akeys.Add("Left Ctrl", Keys.LControlKey);
            _akeys.Add("Right Ctrl", Keys.RControlKey);
            _akeys.Add("Left Shift", Keys.LShiftKey);
            _akeys.Add("Right Shift", Keys.RShiftKey);
            _akeys.Add("Left Win", Keys.LWin);
            _akeys.Add("Right Win", Keys.RWin);

            string[] akeys_names = new string[_akeys.Keys.Count];
            _akeys.Keys.CopyTo(akeys_names, 0);
            
            Keys[] akeys_codes = new Keys[_akeys.Values.Count];
            _akeys.Values.CopyTo(akeys_codes, 0);

            AssistantKeyComboBox.Items.AddRange(akeys_names);
            AssistantKeyComboBox.SelectedIndex = 0;
            // select user key
            AssistantKeyComboBox.SelectedIndex = Array.FindIndex<Keys>(akeys_codes, delegate(Keys s)
            {
                return s == (Keys)_ahotkey.Key;
            });
            //

            //
            // Interface
            //
            _interface_info = new InterfaceInfo(SettingsManager.Instance.GetInterfaceInfo());
            suggestionsNumericUpDown.Value = _interface_info.NumberOfSuggestions;
            //

            //
            // System Options
            //
            _system_options_info = new SystemOptionsInfo(SettingsManager.Instance.GetSystemOptionsInfo());
            updateTimeNumericUpDown.Value = _system_options_info.UpdateTime;
            //

            //
            // Plugins
            //
            OptionsListBox.Items.Add("Index subdirectories");
            _plugins_map_options = new Dictionary<string, int>();
            _options_map_plugins = new Dictionary<int, string>();
            _indexerPlugins = new List<IndexerPlugin>();
            _interpreterPlugins = new List<InterpreterPlugin>();
            _plugins = _parent.Plugins;
            _pluginInfo = new PluginInfo(SettingsManager.Instance.GetLoadablePlugins());
            for (int i = 0; i < _plugins.Count; i++)
            {
                PluginsListBox.Items.Add(_plugins[i].Name, _pluginInfo.Enabled[_plugins[i].Name]);
                if (_plugins[i].Type == PluginType.Indexer)
                {
                    IndexerPlugin p = (IndexerPlugin)_plugins[i];
                    _indexerPlugins.Add(p);
                    OptionsListBox.Items.Add(p.QuickDescription);
                    _plugins_map_options.Add(p.Name, OptionsListBox.Items.Count-1);
                    _options_map_plugins.Add(OptionsListBox.Items.Count - 1, p.Name);
                }
                else
                {
                    InterpreterPlugin p = (InterpreterPlugin)_plugins[i];
                    _interpreterPlugins.Add(p);
                }
            }
            if (PluginsListBox.Items.Count > 0)
            {
                PluginsListBox.SelectedIndex = 0;
            }
            //

            //
            // Index
            //
            DirInfo info = SettingsManager.Instance.GetDirectories();
            _directories = new List<string>(info.Directories.ToArray());
            _extensions = new Dictionary<string, List<string>>(info.Extensions);
            _indexSubdirectories = new Dictionary<string, bool>(info.IndexSubdirectories);
            _dirPlugins = new Dictionary<string, List<string>>(info.Plugins);
            DirectoriesListBox.Items.AddRange(_directories.ToArray());
            if (_directories.Count > 0)
                DirectoriesListBox.SelectedIndex = 0;
            //

            AutomatorLabel.Text = AssemblyTitle + (CommonInfo.IsPortable ? " Portable " : " ") + AssemblyVersion + " beta";
            AutomatorDescriptionLabel.Text = "Blaze is an application that is being developed in the scope of a college project." + Environment.NewLine +
                                        "The main goal is to develop an application launcher that is able to automate the" + Environment.NewLine +
                                        "recurrent tasks that arise from everyday usage.";
            Process proc = Process.GetCurrentProcess();
            MemoryEditableLabel.Text = (proc.WorkingSet64 / 1024).ToString() + " kilobytes";
            StartTimeEditableLabel.Text = proc.StartTime.ToString();
            IndexingTimeEditableLabel.Text = SettingsManager.Instance.GetIndexingTime().ToString();
            IndexedItemsEditableLabel.Text = SettingsManager.Instance.GetNumberOfIndexedItems().ToString() + " items";
            proc.Dispose();
        }

        private void RemoveDirectoryButton_Click(object sender, EventArgs e)
        {
            if (DirectoriesListBox.SelectedItem != null)
            {
                string item = DirectoriesListBox.SelectedItem.ToString();
                if (item != string.Empty)
                {
                    _directories.Remove(item);
                    _extensions.Remove(item);
                    _indexSubdirectories.Remove(item);
                    _dirPlugins.Remove(item);
                    DirectoriesListBox.Items.Clear();
                    DirectoriesListBox.Items.AddRange(_directories.ToArray());
                    //if (_directories.Count == 0)
                    //{
                        FileTypesListBox.Items.Clear();
                        //IncludeSubDirCheckBox.Checked = false;
                        OptionsListBox.SetItemChecked(0, false);
                    //}
                    //else
                    //{
                    //    DirectoriesListBox.SelectedIndex = 0;
                    //}
                }
            }
        }

        private void RemoveTypeButton_Click(object sender, EventArgs e)
        {
            if (DirectoriesListBox.SelectedItem != null && _extensions[DirectoriesListBox.SelectedItem.ToString()].Count > 0 && FileTypesListBox.SelectedItem != null)
            {
                string item = FileTypesListBox.SelectedItem.ToString();
                if (item != string.Empty)
                {
                    string dir = DirectoriesListBox.SelectedItem.ToString();
                    _extensions[dir].Remove(FileTypesListBox.SelectedItem.ToString());
                    FileTypesListBox.Items.Clear();
                    FileTypesListBox.Items.AddRange(_extensions[dir].ToArray());
                    //if (_extensions[dir].Count > 0)
                    //    FileTypesListBox.SelectedIndex = 0;
                }
            }
        }

        private void IncludeSubDirCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DirectoriesListBox.SelectedItem != null)
            {
                string item = DirectoriesListBox.SelectedItem.ToString();
                if (item != string.Empty)
                {
                    //_indexSubdirectories[item] = IncludeSubDirCheckBox.Checked;
                    _indexSubdirectories[item] = OptionsListBox.GetItemChecked(0);
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Keys[] keys_codes = new Keys[_keys.Values.Count];
            _keys.Values.CopyTo(keys_codes, 0);
            Keys[] akeys_codes = new Keys[_akeys.Values.Count];
            _akeys.Values.CopyTo(akeys_codes, 0);

            HotKey newMainHotkey = new HotKey(_modifiers[ModifierComboBox.SelectedItem.ToString()],
                                            keys_codes[MainKeyComboBox.SelectedIndex]);
            HotKey newAssistantHotkey = new HotKey(false, false, false, false, akeys_codes[AssistantKeyComboBox.SelectedIndex]);
            if (newMainHotkey != _hotkey ||
                newAssistantHotkey != _ahotkey)
            {
                _parent.UnregisterHotKey();
                if (!_parent.RegisterHotKey(newMainHotkey) || newMainHotkey == newAssistantHotkey)
                {
                    MessageBox.Show("The hotkey you have chosen is already in use. Please pick another one",
                                    "Hotkey in use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _parent.RegisterHotKey();
                    return;
                }
                _parent.RegisterAssistantHotKey(newAssistantHotkey);
                SettingsManager.Instance.SaveMainHotKey(newMainHotkey);
                SettingsManager.Instance.SaveAssistantHotKey(newAssistantHotkey);
            }
            _interface_info.NumberOfSuggestions = (int)suggestionsNumericUpDown.Value;
            _system_options_info.UpdateTime = (int)updateTimeNumericUpDown.Value;
            List<List<string>> ext = new List<List<string>>();
            List<bool> subdir = new List<bool>();
            foreach (string dir in _directories)
            {
                ext.Add(_extensions[dir]);
                subdir.Add(_indexSubdirectories[dir]);
            }
            SettingsManager.Instance.SaveDirectories(new DirInfo(_directories, _extensions, _indexSubdirectories, _dirPlugins));
            SettingsManager.Instance.SaveLoadablePlugins(_pluginInfo);
            SettingsManager.Instance.SaveInterfaceInfo(_interface_info);
            SettingsManager.Instance.SaveSystemOptionsInfo(_system_options_info);
            _parent.Interpreter.SetUpdateTimerInterval(_system_options_info.UpdateTime);
            _parent.LoadPlugins();
            _parent.UpdateUponInput();
            //_parent.RebuildIndex();
            Close();
        }

        private void AddDirectoryButton_Click(object sender, EventArgs e)
        {
            //_directoryPicker = new DirectoryPicker();
            _directoryPicker = new FolderBrowserDialog();
            _directoryPicker.ShowNewFolderButton = false;
            _directoryPicker.Description = "Please select a directory:";

            ContextLib.DataContainers.Multimedia.MultiLevelData data = UserContext.Instance.GetSelectedContent();
            string actual_path = UserContext.Instance.GetExplorerPath(true);
            if (data.FileList != null && data.FileList.Length > 0 && Directory.Exists(data.FileList[0]))
            {
                _directoryPicker.SelectedPath = data.FileList[0];
            }
            else if (actual_path != CommonInfo.UserDesktop)
            {
                _directoryPicker.SelectedPath = actual_path;
            }
            else if (data.Text != null && Directory.Exists(data.Text))
            {
                _directoryPicker.SelectedPath = data.Text;
            }

            if (_directoryPicker.ShowDialog() == DialogResult.OK)
            {
                //string dir = _directoryPicker.directoryInput.Text;
                string dir = _directoryPicker.SelectedPath;
                if (dir[dir.Length - 1] != '\\')
                    dir += "\\";
                if (_directories.Contains(dir))
                {
                    DialogResult res = MessageBox.Show("This directory has already been added. Do you want to pick a different one?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.Yes)
                        AddDirectoryButton_Click(sender, e);
                    return;
                }
                _directories.Add(dir);
                _extensions.Add(dir, new List<string>());
                _indexSubdirectories.Add(dir, false);
                _dirPlugins.Add(dir, new List<string>());
                DirectoriesListBox.Items.Clear();
                DirectoriesListBox.Items.AddRange(_directories.ToArray());
                FileTypesListBox.Items.Clear();
                FileTypesListBox.Items.AddRange(_extensions[dir].ToArray());
                //IncludeSubDirCheckBox.Checked = _indexSubdirectories[dir];
                OptionsListBox.SetItemChecked(0, _indexSubdirectories[dir]);
            }
            _directoryPicker.Dispose();
            _directoryPicker = null;

        }

        private void AddTypeButton_Click(object sender, EventArgs e)
        {
            _typePicker = new TypePicker();
            if (_typePicker.ShowDialog() == DialogResult.OK)
            {
                if (DirectoriesListBox.SelectedItem != null)
                {
                    string dir = DirectoriesListBox.SelectedItem.ToString();
                    _extensions[dir].Add("." + _typePicker.typeInput.Text);
                    FileTypesListBox.Items.Clear();
                    FileTypesListBox.Items.AddRange(_extensions[dir].ToArray());
                    //if (_extensions[dir].Count > 0)
                    //    FileTypesListBox.SelectedIndex = 0;
                }
            }
            _typePicker.Dispose();
            _typePicker = null;
        }
        #endregion

        private void ConfigurePluginButton_Click(object sender, EventArgs e)
        {
            if (PluginsListBox.Items.Count > 0 && PluginsListBox.SelectedIndex != ListBox.NoMatches)
            {
                int index = PluginsListBox.SelectedIndex;
                _plugins[index].Configure();
            }
        }
    }
}