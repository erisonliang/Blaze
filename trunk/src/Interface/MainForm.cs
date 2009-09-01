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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.Settings;
using SystemCore.SystemAbstraction;
using SystemCore.SystemAbstraction.ImageHandling;
using SystemCore.SystemAbstraction.WindowManagement;
using Blaze.Interpreter;
using ContextLib;

namespace Blaze
{
    public partial class MainForm : Form
    {
        private InterpreterEngine _interpreter;
        private PluginLoader _loader;
        private bool _drag = false;
        private Point _start_point;
        private InterpreterItem[] _items;
        private int _id;
        private const Int32 WM_HOTKEY = 0x312;
        private SuperListBox _superListBox;
        private static Point _listBoxDisplacement = new Point(86, 85);
        private SettingsForm _settings = null;
        private List<Plugin> _plugins;
        private int _last_index;
        private bool _listbox_opening = false;
        private bool _assistant_button_showing = false;
        private bool _assistant_button_took_focus = false;
        private ToolTip _tooltip;
        private bool _tooltip_on = false;
        private CommandUsage _tooltip_content = null;
        private Automation.AssistantButton _assistant_button = null;
        private Automation.AssistantWindow _assistant = null;
        private DateTime _last_caps;
        private ContextLib.DataContainers.Monitoring.Suggestion _last_suggestion = null;
        private MainApplication _app;
        //private bool _is_indexing = false;
        //private bool _is_recording = false;
        //IntPtr _hook_id;

        //private bool _key_alt_down = false;
        //private bool _key_shift_down = false;
        //private bool _key_ctrl_down = false;
        //private bool _key_win_down = false;
        //private bool _key_space_down = false;

        private Debug.DebugWindow _debug_window = null;

        public TextBox TextBox
        {
            get { return TextInput; }
        }

        public InterpreterEngine Interpreter
        {
            get { return _interpreter; }
        }

        public List<Plugin> Plugins
        {
            get { return _plugins; }
        }

        public ContextLib.DataContainers.Monitoring.Suggestion LastAcceptedSuggestion
        {
            get { return _last_suggestion; }
            set { _last_suggestion = value; }
        }

        public MainForm(MainApplication app)
        {
            InitializeComponent();
            _app = app;

            _interpreter = new InterpreterEngine(this);
            _loader = new PluginLoader(CommonInfo.PluginsFolder);
            _superListBox = new SuperListBox(this);

            // bind hot key CTRL+ALT+SPACE
            _id = Win32.GlobalAddAtom(this.Name);
            _tooltip = new ToolTip();
            _tooltip.Active = _tooltip_on;
            _tooltip.OwnerDraw = true;
            _tooltip.BackColor = Color.Linen;
            _tooltip.Draw += new DrawToolTipEventHandler(_tooltip_Draw);
            _tooltip.Popup += new PopupEventHandler(_tooltip_Popup);
            RegisterHotKey();

            this.VisibleChanged += new EventHandler(MainForm_VisibleChanged);
            //this.Shown += new EventHandler(MainForm_Shown);
            TextInput.KeyDown += new KeyEventHandler(TextInput_KeyDown);
            TextInput.KeyPress += new KeyPressEventHandler(TextInput_KeyPress);
            TextInput.LostFocus += new EventHandler(TextInput_LostFocus);
            NotifyIcon.DoubleClick += new EventHandler(NotifyIcon_DoubleClick);
            NameDisplay.Text = string.Empty;
            CustomLabel.Text = string.Empty;
            CustomLabel.Paint += new PaintEventHandler(CustomLabel_Paint);

            LoadPlugins();

            // Load index
            _interpreter.LoadIndex();
            // Build forward index
            _interpreter.BuildIndex();

            // Do stuff when user types
            TextInput.TextChanged += new EventHandler(TextInput_TextChanged);

            // Event handlers to allow all controls to be dragged
            MouseUp += new MouseEventHandler(MainForm_MouseUp);
            MouseDown += new MouseEventHandler(MainForm_MouseDown);
            MouseMove += new MouseEventHandler(MainForm_MouseMove);
            IconBox.MouseUp += new MouseEventHandler(MainForm_MouseUp);
            IconBox.MouseDown += new MouseEventHandler(MainForm_MouseDown);
            IconBox.MouseMove += new MouseEventHandler(MainForm_MouseMove);
            NameDisplay.MouseUp += new MouseEventHandler(MainForm_MouseUp);
            NameDisplay.MouseDown += new MouseEventHandler(MainForm_MouseDown);
            NameDisplay.MouseMove += new MouseEventHandler(MainForm_MouseMove);
            CustomLabel.MouseUp += new MouseEventHandler(MainForm_MouseUp);
            CustomLabel.MouseDown += new MouseEventHandler(MainForm_MouseDown);
            CustomLabel.MouseMove += new MouseEventHandler(MainForm_MouseMove);

            Point new_location = SettingsManager.Instance.GetInterfaceInfo().WindowLocation;
            Rectangle bounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            if (new_location.X >= bounds.X && new_location.X <= bounds.Width && new_location.Y >= bounds.Y && new_location.Y <= bounds.Height)
                this.Location = new_location;

            _last_caps = DateTime.MinValue;
            Gma.UserActivityMonitor.HookManager.KeyDown += new KeyEventHandler(HookManager_KeyDown);
            UserContext.Instance.AssistantObject.NewSuggestion += new Assistant.SuggestionEventHandler(AssistantObject_NewSuggestion);
            UserContext.Instance.AssistantObject.NoNewSuggestion += new Assistant.SuggestionEventHandler(AssistantObject_NoNewSuggestion);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        void AssistantObject_NoNewSuggestion()
        {
            NotifyIcon.Icon = Properties.Resources.blaze_small;
        }

        void AssistantObject_NewSuggestion()
        {
            NotifyIcon.Icon = Properties.Resources.blaze_small_glow;
        }

        void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.CapsLock)
            {
                DateTime now = DateTime.Now;
                bool update = true;
                if (_last_caps != DateTime.MinValue)
                {
                    if (now - _last_caps <= new TimeSpan(500 * TimeSpan.TicksPerMillisecond))
                    {
                        ShowAssistantWindow();
                        update = false;
                    }
                }
                if (update)
                    _last_caps = now;
                else
                    _last_caps = DateTime.MinValue;
            }
            else
            {
                _last_caps = DateTime.MinValue;
            }
        }

        //void HookManager_KeyUp(object sender, KeyEventArgs e)
        //{
        //    // alt
        //    if (e.KeyCode == Keys.LMenu)
        //    {
        //        _key_alt_down = false;
        //        //e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    if (e.KeyCode == Keys.RMenu)
        //    {
        //        _key_alt_down = false;
        //        //e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    // ctrl
        //    if (e.KeyCode == Keys.LControlKey)
        //    {
        //        _key_ctrl_down = false;
        //        //e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    if (e.KeyCode == Keys.RControlKey)
        //    {
        //        _key_ctrl_down = false;
        //        //e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    // shift
        //    if (e.KeyCode == Keys.LShiftKey)
        //    {
        //        _key_alt_down = false;
        //        //e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    if (e.KeyCode == Keys.RShiftKey)
        //    {
        //        _key_alt_down = false;
        //        //e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    // win
        //    if (e.KeyCode == Keys.LWin)
        //    {
        //        _key_win_down = false;
        //        //e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    if (e.KeyCode == Keys.RWin)
        //    {
        //        _key_win_down = false;
        //        //e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    // space
        //    if (e.KeyCode == Keys.Space)
        //    {
        //        _key_space_down = false;
        //        //e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //}

        //void HookManager_KeyDown(object sender, KeyEventArgs e)
        //{
        //    //e.SuppressKeyPress = true;
        //    // alt
        //    if (e.KeyCode == Keys.LMenu)
        //    {
        //        _key_alt_down = true;
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    if (e.KeyCode == Keys.RMenu)
        //    {
        //        _key_alt_down = true;
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    // ctrl
        //    if (e.KeyCode == Keys.LControlKey)
        //    {
        //        _key_ctrl_down = true;
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    if (e.KeyCode == Keys.RControlKey)
        //    {
        //        _key_ctrl_down = true;
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    // shift
        //    if (e.KeyCode == Keys.LShiftKey)
        //    {
        //        _key_alt_down = true;
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    if (e.KeyCode == Keys.RShiftKey)
        //    {
        //        _key_alt_down = true;
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    // win
        //    if (e.KeyCode == Keys.LWin)
        //    {
        //        _key_win_down = true;
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    if (e.KeyCode == Keys.RWin)
        //    {
        //        _key_win_down = true;
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    // space
        //    if (e.KeyCode == Keys.Space)
        //    {
        //        _key_space_down = true;
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //    }
        //    HotKey key = SettingsManager.Instance.GetHotKey();
        //    if (_key_alt_down == key.IsAlt &&
        //        _key_ctrl_down == key.IsCtrl &&
        //        _key_shift_down == key.IsShift &&
        //        _key_win_down == key.IsWin &&
        //        _key_space_down)
        //    {
        //        if (Visible)
        //        {
        //            if (_settings == null)
        //            {
        //                HideAutomator();
        //            }
        //            else
        //            {
        //                ShowAutomator();
        //                if (_settings != null)
        //                    _settings.Activate();
        //            }
        //        }
        //        else
        //        {
        //            ShowAutomator();
        //        }
        //    }
        //    //MessageBox.Show(e.KeyCode.ToString());
        //}

        //private static IntPtr HookCallback(
        //    int nCode, IntPtr wParam, IntPtr lParam)
        //{
        //    MessageBox.Show("T_T");
        //    if (nCode >= 0 && wParam == (IntPtr)GlobalKeyboardHook.WM_KEYDOWN || wParam == (IntPtr)GlobalKeyboardHook.WM_SYSKEYDOWN)
        //    {
        //        int vkCode = Marshal.ReadInt32(lParam);
        //        if ((Keys)vkCode == Keys.Space)
        //        {
        //            MessageBox.Show("Q_Q");
        //            return IntPtr.Zero;
        //        }
        //    }
        //    return GlobalKeyboardHook.CallNextHook(nCode, wParam, lParam);
        //}

        void TextInput_LostFocus(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (!this.TopMost)
                {
                    if (!_listbox_opening && !_assistant_button_showing)
                    {
                        if (!_superListBox.Visible && (_assistant_button != null ? !_assistant_button.Focused && !_assistant_button_took_focus : true))
                        {
                            //if (_superListBox.Focused)
                            //    MessageBox.Show(UserContext.Instance.GetTopWindow().Title);
                            HideAutomator();
                        }
                        _assistant_button_took_focus = false;
                        //if (_assistant_button.Focused)
                        //    TextBox.Focus();
                    }
                }
                else
                {
                    HideAutomator();
                }
            }
        }

        //void MainForm_Shown(object sender, EventArgs e)
        //{
        //    HideAutomator();
        //}

        //
        // Draw description label
        //
        void CustomLabel_Paint(object sender, PaintEventArgs e)
        {
            Rectangle area = e.ClipRectangle;
            SolidBrush label_brush = new SolidBrush(CustomLabel.ForeColor);
            StringFormat sf = new StringFormat();
            sf.Trimming = StringTrimming.EllipsisPath;
            e.Graphics.DrawString(CustomLabel.Text, CustomLabel.Font, label_brush, area, sf);

            //clean up
            area = Rectangle.Empty;
            label_brush.Dispose();
            sf.Dispose();
            e.Graphics.Dispose();
        }

        //
        // Draw dashboard
        //
        //void IndexingLabel_Paint(object sender, PaintEventArgs e)
        //{
        //    Rectangle frame = e.ClipRectangle;
        //    Rectangle text_area = new Rectangle(frame.X + 1, frame.Y + 0, frame.Width - 1, frame.Height - 1);
        //    SolidBrush label_brush; 
        //    if (_is_indexing)
        //        label_brush = new SolidBrush(Color.FromArgb(170, Color.Black));
        //    else
        //        label_brush = new SolidBrush(Color.FromArgb(70, Color.Gray));
        //    StringFormat sf = new StringFormat();
        //    sf.Trimming = StringTrimming.None;
        //    e.Graphics.DrawString(IndexingLabel.Text, IndexingLabel.Font, label_brush, text_area, sf);
        //    e.Graphics.DrawRectangle(new Pen(label_brush), frame.X, frame.Y, frame.Width-1, frame.Height-1);

        //    //clean up
        //    text_area = Rectangle.Empty;
        //    label_brush.Dispose();
        //    sf.Dispose();
        //    e.Graphics.Dispose();
        //}

        //void RecordingLabel_Paint(object sender, PaintEventArgs e)
        //{
        //    Rectangle frame = e.ClipRectangle;
        //    Rectangle text_area = new Rectangle(frame.X + 1, frame.Y + 0, frame.Width - 1, frame.Height - 1);
        //    SolidBrush label_brush;
        //    if (_is_recording)
        //        label_brush = new SolidBrush(Color.FromArgb(170, Color.Black));
        //    else
        //        label_brush = new SolidBrush(Color.FromArgb(70, Color.Gray));
        //    StringFormat sf = new StringFormat();
        //    sf.Trimming = StringTrimming.None;
        //    e.Graphics.DrawString(RecordingLabel.Text, RecordingLabel.Font, label_brush, text_area, sf);
        //    e.Graphics.DrawRectangle(new Pen(label_brush), frame.X, frame.Y, frame.Width - 1, frame.Height - 1);

        //    //clean up
        //    text_area = Rectangle.Empty;
        //    label_brush.Dispose();
        //    sf.Dispose();
        //    e.Graphics.Dispose();
        //}

        //private delegate void UpdateLabelDelegate(bool indexing);

        //public void SetIndexing(bool indexing)
        //{
        //    _is_indexing = indexing;
        //    if (IndexingLabel.InvokeRequired)
        //    {
        //        // This is a worker thread so delegate the task.
        //        IndexingLabel.Invoke(new UpdateLabelDelegate(SetIndexing), indexing);
        //    }
        //    else
        //    {
        //        // This is the UI thread so perform the task.
        //        IndexingLabel.Refresh();
        //    }
        //}

        //public void SetRecording(bool recording)
        //{
        //    _is_recording = recording;
        //    if (RecordingLabel.InvokeRequired)
        //    {
        //        // This is a worker thread so delegate the task.
        //        RecordingLabel.Invoke(new UpdateLabelDelegate(SetRecording), recording);
        //    }
        //    else
        //    {
        //        // This is the UI thread so perform the task.
        //        RecordingLabel.Refresh();
        //    }
        //}
        //

        //
        // Control
        //
        public void LoadPlugins()
        {
            // Load plugins
            //PluginPackage pp = _loader.Load();
            //_indexer.LoadPlugins(pp.InterpreterPlugins);
            _plugins = _loader.Load();
            //Calc calc = new Calc();
            //WebEngine web = new WebEngine();
            //MusicSearcher mp3 = new MusicSearcher();
            //ContentIndexer cont = new ContentIndexer();
            //_plugins = new List<Plugin>();
            //_plugins.Add(calc);
            //_plugins.Add(web);
            //_plugins.Add(mp3);
            //_plugins.Add(cont);
            SettingsManager.Instance.RegisterPlugins(_plugins);
            PluginInfo info = SettingsManager.Instance.GetLoadablePlugins();
            foreach (Plugin plugin in _plugins)
            {
                plugin.Activated = info.Enabled[plugin.Name];
            }
            _interpreter.LoadPlugins(_plugins);
        }

        public void ShowAutomator()
        {
            ShowAutomator(true);
        }

        public void ShowAutomator(bool release_keys)
        {
            UserContext.Instance.ObserverObject.StopMonitoring();
            int iskip = (UserContext.Instance.ObserverObject.UseCompression ? 0 : 1), fskip = 1;

            UserContext.Instance.StopMacroRecording(CommonInfo.ScriptsFolder, iskip, fskip);
            _interpreter.LoadIndex();
            UserContext.Instance.TakeContextSnapshot();

            Show();
            Activate();
            UpdateUponInput();
            if (TextInput.Text.Length > 0)
            {
                TextInput.SelectionStart = 0;
                TextInput.SelectionLength = TextInput.Text.Length;
            }
            ValidateAssistantButton();

            if (release_keys)
            {
                Thread.Sleep(100);
                HotKey hotkey = SettingsManager.Instance.GetHotKey();
                Keys key;
                if (hotkey.IsAlt && Win32.GetAsyncKeyState((int)VirtualKey.VK_MENU) == 0 /*!UserContext.Instance.ObserverObject.IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt)*/)
                {
                    key = Keys.LMenu;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                    key = Keys.RMenu;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                }
                if (hotkey.IsCtrl && Win32.GetAsyncKeyState((int)VirtualKey.VK_CONTROL) == 0 /*!UserContext.Instance.ObserverObject.IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl)*/)
                {
                    key = Keys.LControlKey;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                    key = Keys.RControlKey;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                }
                if (hotkey.IsShift && Win32.GetAsyncKeyState((int)VirtualKey.VK_SHIFT) == 0 /*!UserContext.Instance.ObserverObject.IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift)*/)
                {
                    key = Keys.LShiftKey;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                    key = Keys.RShiftKey;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                }
                if (hotkey.IsWin && (Win32.GetAsyncKeyState((int)VirtualKey.VK_LWIN) == 0 || Win32.GetAsyncKeyState((int)VirtualKey.VK_RWIN) == 0)/*!UserContext.Instance.ObserverObject.IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Win)*/)
                {
                    key = Keys.LWin;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                    key = Keys.RWin;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                }
            }
        }

        public void HideAutomator()
        {
            HideAutomator(true);
        }

        public void HideAutomator(bool release_keys)
        {
            HideAssistantButton();
            if (_superListBox.Visible)
            {
                _superListBox.Hide();//_superListBox.Dispose();//
            }
            GC.Collect();
            Hide();
            _interpreter.UnloadIndex();

            UserContext.Instance.PerformPostFocusOperations();

            if (release_keys)
            {
                Thread.Sleep(100);
                HotKey hotkey = SettingsManager.Instance.GetHotKey();
                Keys key;
                if (hotkey.IsAlt && Win32.GetAsyncKeyState((int)VirtualKey.VK_MENU) == 0 /*!UserContext.Instance.ObserverObject.IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt)*/)
                {
                    key = Keys.LMenu;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                    key = Keys.RMenu;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                }
                if (hotkey.IsCtrl && Win32.GetAsyncKeyState((int)VirtualKey.VK_CONTROL) == 0 /*!UserContext.Instance.ObserverObject.IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl)*/)
                {
                    key = Keys.LControlKey;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                    key = Keys.RControlKey;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                }
                if (hotkey.IsShift && Win32.GetAsyncKeyState((int)VirtualKey.VK_SHIFT) == 0 /*!UserContext.Instance.ObserverObject.IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift)*/)
                {
                    key = Keys.LShiftKey;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                    key = Keys.RShiftKey;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                }
                if (hotkey.IsWin && (Win32.GetAsyncKeyState((int)VirtualKey.VK_LWIN) == 0 || Win32.GetAsyncKeyState((int)VirtualKey.VK_RWIN) == 0)/*!UserContext.Instance.ObserverObject.IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Win)*/)
                {
                    key = Keys.LWin;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                    key = Keys.RWin;
                    Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
                }
            }

            UserContext.Instance.ObserverObject.StartMonitoring();
        }

        public void UpdateListBoxPosition()
        {
            _superListBox.Location = new Point(Location.X + _listBoxDisplacement.X, Location.Y + _listBoxDisplacement.Y);
        }

        public void UpdateUponInput()
        {
            // If the user types something, hide the alternative listbox
            if (_superListBox.Visible)
            {
                _superListBox.Hide();
            }
            _superListBox.ListBox.Items.Clear();
            // Clear all displays
            IconBox.Image = null;
            NameDisplay.Text = string.Empty;
            CustomLabel.Text = string.Empty;
            // If there is no selected text, do nothing
            if (TextInput.Text.Trim() == string.Empty)
            {
                NameDisplay.Text = "Blaze";
                CustomLabel.Text = "Please type in your commands...";
                CustomLabel.Refresh();
                UpdateToolTip();
                IconBox.Image = Properties.Resources.blaze_med.ToBitmap();
                Refresh();
                return;
            }
            else
            {
                // Rebuild the alternative entries for user input
                //if (_items != null)
                //    foreach (InterpreterItem item in _items)
                //        item.Dispose();
                //if (_superListBox.Items != null)
                //    foreach (InterpreterItem item in _superListBox.Items)
                //        item.Dispose();
                _items = null;
                _superListBox.Items = null;
                _items = _interpreter.RetrieveTopItems(TextInput.Text).ToArray(); // mem leak
                _superListBox.Items = _items;
                for (int i = 0; i < _items.Length; i++)
                {
                    _superListBox.ListBox.Items.Add(_items[i].Name);
                    // Update displays with topmost's information
                    if (i == 0)
                    {
                        //Bitmap bitmap = (IconManager.Instance.GetIcon(_items[i].IconPtr)).ToBitmap();
                        //IconBox.Image = bitmap;
                        //NameDisplay.Text = _items[i].Name;
                        //CustomLabel.Text = _items[i].Desciption;
                        _superListBox.ListBox.SelectedIndex = 0;
                    }
                }
                UpdatePanel();
            }
            // Force CustomLabel to redraw
            //CustomLabel.Refresh();
            //GC.Collect();
        }

        public bool RegisterHotKey()
        {
            HotKey hotkey = SettingsManager.Instance.GetHotKey();
            //MessageBox.Show("Key: " + hotkey.Key.ToString() + " Modifiers: " + hotkey.Modifiers.ToString());
            return Win32.RegisterHotKey(this.Handle, _id, hotkey.Modifiers, hotkey.Key);
        }

        public bool RegisterHotKey(HotKey hotkey)
        {
            return Win32.RegisterHotKey(this.Handle, _id, hotkey.Modifiers, hotkey.Key);
        }

        public void UnregisterHotKey()
        {
            Win32.UnregisterHotKey(this.Handle, _id);
        }

        public void OpenSettings()
        {
            if (_settings == null)
            {
                _settings = new SettingsForm(this);
                _settings.ShowDialog(this);
                _settings.Dispose();
                _settings = null;
            }
            else
            {
                _settings.Focus();
            }
        }

        public void RebuildIndex()
        {
            _interpreter.BuildIndex();
            UpdateUponInput();
        }

        public void ClearLearnedCommands()
        {
            SettingsManager.Instance.ClearLearned();
        }

        public void UpdatePanel()
        {
            if (_superListBox.ListBox.Items.Count > 0)
            {
                int index = _superListBox.ListBox.SelectedIndex;
                Image bit;
                try
                {
                    bit = (Bitmap)_items[index].Icon;
                }
                catch (Exception)
                {
                    //MessageBox.Show(e.Message);
                    bit = IconManager.Instance.GetIcon(_items[index].Desciption);
                }
                try
                {
                    if (bit == null)
                        throw new ArgumentException();
                    IconBox.Image = bit;
                    Refresh();
                }
                catch (ArgumentException)
                {
                    RecoverIcon(_items[index].Desciption, IconBox);
                }
                NameDisplay.Text = _items[index].Name;
                CustomLabel.Text = _items[index].Desciption;
                CustomLabel.Refresh();
            }
            else
            {
                CustomLabel.Refresh();
            }
            UpdateToolTip();
            //else
            //{
            //    IconBox.Image = null;
            //    NameDisplay.Text = string.Empty;
            //    CustomLabel.Text = string.Empty;
            //}
        }

        void RecoverIcon(string path, PictureBox box)
        {
            try
            {
                box.Image = IconManager.Instance.GetIcon(path);
                box.Refresh();
            }
            catch (ArgumentException)
            {
                RecoverIcon(path, box);
            }
        }

        public void UpdateToolTip()
        {
            if (_superListBox.ListBox.Items.Count > 0)
            {
                int index = _superListBox.ListBox.SelectedIndex;
                if (_items[index].CommandUsage != null)
                {
                    string usage = "Usage: ";
                    usage += _items[index].CommandUsage.Name;
                    if (_items[index].CommandUsage.Arguments.Count > 0)
                    {
                        usage += " ";
                        foreach (string arg in _items[index].CommandUsage.Arguments)
                            usage += arg + " ";
                        usage = usage.Trim();
                        //                        usage = @"{\rtf1\ansi{\fonttbl\f0\fswiss Helvetica;}\f0\pard
                        //                                    This is some {\b bold} text.\par
                        //                                    }";
                    }
                    _tooltip_content = _items[index].CommandUsage;
                    _tooltip.Show(usage, TextBox, new Point(0, 21));
                }
                else
                {
                    _tooltip.Hide(TextBox);
                }
            }
            else
            {
                _tooltip.Hide(TextBox);
            }
        }

        void _tooltip_Popup(object sender, PopupEventArgs e)
        {
            string test = _tooltip.GetToolTip(e.AssociatedControl);
            Size size = TextRenderer.MeasureText(test, new Font("Verdana", 9, FontStyle.Bold));
            e.ToolTipSize = size + new Size((int)(0.05 * size.Width), 4);
        }

        void _tooltip_Draw(object sender, DrawToolTipEventArgs e)
        {
            if (_tooltip_content != null)
            {

                e.DrawBackground();

                e.Graphics.DrawLines(SystemPens.ControlLightLight, new Point[] {
                        new Point (0, e.Bounds.Height - 1), 
                        new Point (0, 0), 
                        new Point (e.Bounds.Width - 1, 0)
                    });
                e.Graphics.DrawLines(SystemPens.ControlDarkDark, new Point[] {
                        new Point (0, e.Bounds.Height - 1), 
                        new Point (e.Bounds.Width - 1, e.Bounds.Height - 1), 
                        new Point (e.Bounds.Width - 1, 0)
                    });

                //e.DrawBorder();
                //StringFormat sf = new StringFormat();
                SolidBrush brush = new SolidBrush(Color.Black);
                Font normal = new Font("Verdana", 9, FontStyle.Regular);
                Font bold = new Font("Verdana", 9, FontStyle.Bold);

                PointF pos = new PointF(e.Bounds.X + 2, e.Bounds.Y + 1);
                e.Graphics.DrawString("Usage:", normal, brush, pos);
                SizeF displacement = new SizeF(e.Graphics.MeasureString("Usage:", normal).Width, 0);
                e.Graphics.DrawString(_tooltip_content.Name, bold, brush, pos + displacement);
                displacement = new SizeF(e.Graphics.MeasureString(_tooltip_content.Name, bold).Width + displacement.Width, displacement.Height);
                foreach (string arg in _tooltip_content.Arguments)
                {
                    if (_tooltip_content.Completeness[arg])
                    {
                        e.Graphics.DrawString(arg, bold, brush, pos + displacement);
                        displacement = new SizeF(e.Graphics.MeasureString(arg, bold).Width + displacement.Width, displacement.Height);
                    }
                    else
                    {
                        e.Graphics.DrawString(arg, normal, brush, pos + displacement);
                        displacement = new SizeF(e.Graphics.MeasureString(arg, normal).Width + displacement.Width, displacement.Height);
                    }
                }
            }
        }
        //

        //
        // Drag form
        //
        void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_drag)
            {
                Point point = PointToScreen(new Point(e.X, e.Y));
                Location = new Point(point.X - _start_point.X, point.Y - _start_point.Y);
                UpdateListBoxPosition();
                UpdateAssistantButtonPosition();
                _tooltip.Active = false;
            }
            else
            {
                bool old_active = _tooltip.Active;
                _tooltip.Active = _tooltip_on;
                if (old_active != _tooltip_on)
                    UpdateToolTip();
            }
        }

        void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            SizeF size = this.BackgroundImage.PhysicalDimension;
            if (e.X < size.Width && e.Y < size.Height)
            {
                _drag = true;
                _start_point = new Point(e.X, e.Y);
            }
        }

        void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            _drag = false;
        }
        //

        //
        // Handling user input text
        //
        void TextInput_TextChanged(object sender, EventArgs e)
        {
            UpdateUponInput();
        }
        //

        //
        // Key Handling
        //
        void TextInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Return) // avoid *dling*
                e.Handled = true;
        }

        void TextInput_KeyDown(object sender, KeyEventArgs e)
        {
            string cmd = TextInput.Text.ToString();

            if (e.KeyCode == Keys.Return)
            {
                if (_superListBox.Visible)
                {
                    // select a entry from the alternatives listbox and hide it
                    _interpreter.Execute(TextInput.Text, _items[_superListBox.ListBox.SelectedIndex], e.Modifiers);
                    //HideAutomator();
                }
                else
                {
                    // execute the command and hide automator
                    if (_superListBox.ListBox.Items.Count > 0)
                    {
                        _interpreter.Execute(TextInput.Text, _items[_superListBox.ListBox.SelectedIndex], e.Modifiers);
                        //HideAutomator();
                    }
                }
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideAutomator();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (!_superListBox.Visible)
                {
                    // show the alternatives listbox if it has, at least, one item
                    if (_superListBox.ListBox.Items.Count > 1)
                    {
                        _last_index = _superListBox.ListBox.SelectedIndex;
                        _listbox_opening = true;
                        _superListBox.Show();
                        UpdateListBoxPosition();
                        //_superListBox.BringToFront();
                        Focus();
                        _listbox_opening = false;
                        //_superListBox.Owner = this;
                        _superListBox.ListBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    // if the listbox is already open, then select the next item
                    int newpos = _superListBox.ListBox.SelectedIndex + 1;
                    if (newpos < 0)
                        newpos = _superListBox.ListBox.Items.Count - 1;
                    else if (newpos >= _superListBox.ListBox.Items.Count)
                        newpos = 0;
                    _superListBox.ListBox.SelectedIndex = newpos;
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (_superListBox.Visible)
                {
                    // if the listbox is open, then select the previous item
                    int newpos = _superListBox.ListBox.SelectedIndex - 1;
                    if (newpos < 0)
                        newpos = _superListBox.ListBox.Items.Count - 1;
                    else if (newpos >= _superListBox.ListBox.Items.Count)
                        newpos = 0;
                    _superListBox.ListBox.SelectedIndex = newpos;
                }
                else
                {
                    // show the alternatives listbox if it has, at least, one item
                    if (_superListBox.ListBox.Items.Count > 1)
                    {
                        _last_index = _superListBox.ListBox.SelectedIndex;
                        _listbox_opening = true;
                        _superListBox.Show();
                        UpdateListBoxPosition();
                        //_superListBox.BringToFront();
                        Focus();
                        _listbox_opening = false;
                        //_superListBox.Owner = this;
                        _superListBox.ListBox.SelectedIndex = 0;
                    }
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                // hide the alternatives listbox
                if (_superListBox.Visible)
                {
                    _superListBox.ListBox.SelectedIndex = _last_index;
                    _superListBox.Hide();
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Back && e.Control) // handle ctrl+backspace
            {
                e.SuppressKeyPress = true;
                int lastIndex = TextInput.Text.Trim(new char[] { ' ', '\\' }).LastIndexOfAny(new char[] { ' ', '\\' });
                if (lastIndex != -1)
                {
                    TextInput.Text = TextInput.Text.Substring(0, lastIndex + 1);
                    TextInput.SelectionStart = TextInput.Text.Length;
                }
                else
                {
                    TextInput.Text = string.Empty;
                }
            }
            else if (e.KeyCode == Keys.A && e.Control) // handle ctrl+A
            {
                e.SuppressKeyPress = true;
                if (TextInput.Text.Length > 0)
                {
                    TextInput.SelectionStart = 0;
                    TextInput.SelectionLength = TextInput.Text.Length;
                }
            }
            else if (e.KeyCode == Keys.F1)
            {
                System.Diagnostics.Process.Start("http://blaze-wins.sourceforge.net/index.php?page=overview#blaze");
            }
            else if (e.Control && e.Shift)
            {
                e.SuppressKeyPress = true;
                _tooltip_on = !_tooltip_on;
                _tooltip.Active = _tooltip_on;
                UpdateToolTip();
            }
        }

        protected override bool ProcessTabKey(bool forward)
        {
            // If there is, at least, a entry, auto complete with it
            if (_superListBox.ListBox.Items.Count > 0)
            {
                if (_superListBox.Visible)
                {
                    //TextInput.Text = _items[_superListBox.ListBox.SelectedIndex].AutoComplete;//_superListBox.ListBox.Sele;
                    TextInput.Text = ProcessAutoComplete(_items[_superListBox.ListBox.SelectedIndex]);
                }
                else
                {
                    InterpreterItem item = _items[_superListBox.ListBox.SelectedIndex];
                    string compare;
                    OwnerType type;
                    switch (item.OwnerType)
                    {
                        case OwnerType.FileSystem:
                            compare = item.Desciption;
                            type = OwnerType.FileSystem;
                            break;
                        default:
                            //compare = item.Name;
                            compare = ProcessAutoComplete(_items[_superListBox.ListBox.SelectedIndex]);
                            type = OwnerType.Unspecified;
                            break;
                    }

                    if (forward) // shift was not used
                    {
                        if (compare == TextInput.Text) // if text doesn't need to be auto-completed
                        {
                            if (type == OwnerType.FileSystem) // if its filesystem path, auto-complete with the next path
                            {
                                int newpos = _superListBox.ListBox.SelectedIndex + 1;
                                if (newpos >= _superListBox.ListBox.Items.Count)
                                    newpos = 0;
                                _superListBox.ListBox.SelectedIndex = newpos;
                                TextInput.Text = _items[newpos].AutoComplete;
                            }
                            else // if not, cycle through items
                            {
                                int newpos = _superListBox.ListBox.SelectedIndex + 1;
                                if (newpos >= _superListBox.ListBox.Items.Count)
                                    newpos = 0;
                                _superListBox.ListBox.SelectedIndex = newpos;
                            }
                        }
                        else
                        {
                            // Auto-complete
                            //TextInput.Text = _items[_superListBox.ListBox.SelectedIndex].AutoComplete;
                            TextInput.Text = compare;
                        }
                    }
                    else // shift was used
                    {
                        int newpos = _superListBox.ListBox.SelectedIndex + 1;
                        if (newpos >= _superListBox.ListBox.Items.Count)
                            newpos = 0;
                        _superListBox.ListBox.SelectedIndex = newpos;
                    }

                    UpdatePanel();
                    item = null;
                    compare = null;
                }
            }
            // Place the cursor ate the end of the string typed by the user
            _superListBox.Hide();
            TextInput.SelectionStart = TextInput.Text.Length;
            return true;
        }

        private string ProcessAutoComplete(InterpreterItem item)
        {
            if (item.OwnerType == OwnerType.Indexer)
            {
                //IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                //IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(item.Desciption);
                //MessageBox.Show(link.TargetPath);
                //if (Directory.Exists(item.Desciption))
                //{
                //    if (item.Desciption[item.Desciption.Length - 1] != '\\')
                //        return item.Desciption + "\\";
                //    else
                //        return item.Desciption;
                //}
                FileInfo info = new FileInfo(item.Desciption);
                if (info.Extension.ToLower() == ".lnk")
                {
                    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                    IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(item.Desciption);
                    //MessageBox.Show(link.TargetPath);
                    //Clipboard.SetText(link.TargetPath);
                    if (Directory.Exists(link.TargetPath))
                    {
                        if (link.TargetPath[link.TargetPath.Length - 1] != '\\')
                            return link.TargetPath + "\\";
                        else
                            return link.TargetPath;
                    }
                }
            }
            return item.AutoComplete;
        }
        //

        //
        // Free memory
        //
        void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            //if (Visible)
            //{
            //    TextInput_TextChanged(sender, e);
            //}
            //else
            //{
            //    _items = null;
            //    _superListBox.ListBox.Items.Clear();
            //}
        }
        //

        void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowAutomator();
        }

        private void NotifyIconContextMenu_Exit_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Normal;
            //Show();
            //Activate();
            ShowAutomator();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        protected override void DestroyHandle()
        {
            UnregisterHotKey();

            //GlobalKeyboardHook.UnHook(_hook_id);
            //IconManager.Instance.Clear();
            NotifyIcon.Dispose();
            SettingsManager.Instance.GetInterfaceInfo().WindowLocation = this.Location;
            SettingsManager.Instance.SaveInterfaceInfo();
            base.DestroyHandle();
        }

        // React to HOTKEY
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                if (Visible)
                {
                    if (_settings == null)
                    {
                        HideAutomator();
                    }
                    else
                    {
                        ShowAutomator();
                        if (_settings != null)
                            _settings.Activate();
                    }
                }
                else
                {
                    ShowAutomator();
                }
            }
            else if (m.Msg == Win32.WM_SHOWME)
            {
                ShowAutomator();
            }
            else if (m.Msg == Win32.WM_KILLME)
            {
                Dispose();
            }
            base.WndProc(ref m);
        }

        private void NotifyIconContextMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenSettings();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSettings();
        }

        private void rebuildIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RebuildIndex();
        }

        private void rebuildIndexToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RebuildIndex();
        }

        private void hideToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HideAutomator();
        }

        private void showDebugWindowMenuItem1_Click(object sender, EventArgs e)
        {
            if (showDebugWindowMenuItem1.Checked)
            {
                _debug_window = new Blaze.Debug.DebugWindow(this);
                _debug_window.Show();
                showDebugWindowMenuItem2.Checked = true;
            }
            else
            {
                _debug_window.Close();
                _debug_window.Dispose();
                _debug_window = null;
            }
        }

        private void showDebugWindowMenuItem2_Click(object sender, EventArgs e)
        {
            if (showDebugWindowMenuItem2.Checked)
            {
                _debug_window = new Blaze.Debug.DebugWindow(this);
                _debug_window.Show();
                showDebugWindowMenuItem1.Checked = true;
            }
            else
            {
                _debug_window.Close();
                _debug_window.Dispose();
                _debug_window = null;
            }
        }

        public void UncheckDebugWindow()
        {
            showDebugWindowMenuItem1.Checked = false;
            showDebugWindowMenuItem2.Checked = false;
        }

        private void ShowAssistantButton()
        {
            if (_assistant_button == null)
            {
                _assistant_button_showing = true;
                _assistant_button = new Blaze.Automation.AssistantButton(this);
                _assistant_button.Show();
                //_assistant_button.Size = new Size(48, 48);
                UpdateAssistantButtonPosition();
                Focus();
                _assistant_button_showing = false;
            }
        }

        private void HideAssistantButton()
        {
            if (_assistant_button != null)
            {
                _assistant_button.Close();
                _assistant_button.Dispose();
                _assistant_button = null;
            }
        }

        private void UpdateAssistantButtonPosition()
        {
            if (_assistant_button != null)
            {
                _assistant_button.Location = new Point(this.Location.X + this.Size.Width - 28, this.Location.Y - 20);
            }
        }

        private void ValidateAssistantButton()
        {
            if (ContextLib.UserContext.Instance.AssistantObject.HasSuggestions)
            {
                ShowAssistantButton();
            }
        }

        public void SetAssistantButtonFocus(bool focus)
        {
            _assistant_button_took_focus = focus;
        }

        private void showAssistantWindowMenuItem1_Click(object sender, EventArgs e)
        {
            if (showAssistantWindowMenuItem1.Checked)
            {
                ShowAssistantWindow();
                showAssistantWindowMenuItem2.Checked = true;
            }
            else
            {
                HideAssistantWindow();
            }
        }

        private void showAssistantWindowMenuItem2_Click(object sender, EventArgs e)
        {
            if (showAssistantWindowMenuItem2.Checked)
            {
                ShowAssistantWindow();
                showAssistantWindowMenuItem1.Checked = true;
            }
            else
            {
                HideAssistantWindow();
            }
        }

        public void ShowAssistantWindow()
        {
            if (_assistant == null || _assistant.IsDisposed)
            {
                _assistant = new Blaze.Automation.AssistantWindow(this);
                _assistant.Show();
                WindowUtility.Instance.BringWindowToFrontIfNeeded(_assistant.Handle);
            }
            else
            {
                if (WindowUtility.Instance.IsWindowOnFront(_assistant.Handle))
                {
                    _assistant.Close();
                    _assistant.Dispose();
                    _assistant = null;
                }
                else
                {

                    WindowUtility.Instance.BringWindowToFront(_assistant.Handle);
                }
            }
        }

        public void HideAssistantWindow()
        {
            if (_assistant != null && !_assistant.IsDisposed)
            {
                _assistant.Close();
                _assistant.Dispose();
                _assistant = null;
            }
        }

        public void UncheckAssistantWindow()
        {
            showAssistantWindowMenuItem1.Checked = false;
            showAssistantWindowMenuItem2.Checked = false;
        }

        public void Exit()
        {
            Dispose();
        }

        private void blazeWebpageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://blaze-wins.sourceforge.net/");
        }

        private void blazeWebpageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://blaze-wins.sourceforge.net/");
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://blaze-wins.sourceforge.net/index.php?page=documentation");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://blaze-wins.sourceforge.net/index.php?page=documentation");
        }
    }
}