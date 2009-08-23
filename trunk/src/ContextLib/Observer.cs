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
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction;
using SystemCore.SystemAbstraction.WindowManagement;
using ContextLib.DataContainers.GUI;
using ContextLib.DataContainers.Monitoring;
using Gma.UserActivityMonitor;

namespace ContextLib
{
    public class Observer
    {
        #region Properties
        private UserActionWindow _actions;
        private Macro _macro;
        private FileSystemWatcher _file_system_watcher;
        //private SystemCore.SystemAbstraction.FileHandling.Logger _logger;
        private Window _working_window = null;
        private string _working_path = null;
        private bool _is_monitoring;
        private bool _is_recording;
        private Apprentice _apprentice = null;
        private Assistant _assistant = null;
        private ContextLib.DataContainers.Monitoring.UserAction.Modifiers _modifiers;
        private DateTime _last_user_action; // time of last user action occurence
        private TimeSpan _accept_user_action; // the amout of time needed for an action to be considerer to be made by the user and not the system
        private DateTime _last_dir_updade;
        private TimeSpan _accept_dir_update;
        private object _locker = new object();
        private bool _use_compression = true;
        private System.Timers.Timer _timer;
        #endregion

        #region Accessors
        public bool IsMonitoring { get { return _is_monitoring; } }
        public bool IsRecording { get { return _is_recording; } }
        public string WorkingPath { get { return _working_path; } }
        public int NumberOfActions { get { return _actions.UserActions.Count; } }
        public UserActionWindow ActionWindow { get { return _actions; } }
        public Apprentice Apprentice { get { return _apprentice; } set { _apprentice = value; } }
        public Assistant Assistant { get { return _assistant; } set { _assistant = value; } }
        public bool UseCompression { get { return _use_compression; } set { _use_compression = value; } }
        public ContextLib.DataContainers.Monitoring.UserAction.Modifiers Modifiers { get { return _modifiers; } }
        #endregion

        #region Constructors
        public Observer(int action_window_size, int action_life_period, int action_validation_period, int action_acceptance_period)
        {
            _actions = new UserActionWindow(action_window_size, action_life_period, action_validation_period);
            _actions.UseCompression = _use_compression;
            _file_system_watcher = new FileSystemWatcher();
            _file_system_watcher.IncludeSubdirectories = true;
            _accept_user_action = new TimeSpan(action_acceptance_period * TimeSpan.TicksPerMillisecond);
            _accept_dir_update = new TimeSpan(500 * TimeSpan.TicksPerMillisecond);
            _last_dir_updade = DateTime.Now;
            _last_user_action = DateTime.Now;
            _file_system_watcher.Created += new FileSystemEventHandler(_file_system_watcher_Created);
            _file_system_watcher.Deleted += new FileSystemEventHandler(_file_system_watcher_Deleted);
            _file_system_watcher.Renamed += new RenamedEventHandler(_file_system_watcher_Renamed);
            _use_compression = true;

            _timer = new System.Timers.Timer();
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Tick);
            _timer.AutoReset = false;
            _timer.Interval = action_validation_period / 2;

            // keyboard and mouse hooks
            HookManager.KeyDown += new KeyEventHandler(HookManager_KeyDown);
            HookManager.KeyUp += new KeyEventHandler(HookManager_KeyUp);
            HookManager.KeyPress += new Gma.UserActivityMonitor.HookManager.KeyPressExEventHandler(HookManager_KeyPress);
            HookManager.MouseDown += new MouseEventHandler(HookManager_MouseDown);
            HookManager.MouseUp += new MouseEventHandler(HookManager_MouseUp);
            HookManager.MouseWheel += new MouseEventHandler(HookManager_MouseWheel);
        }

        ~Observer()
        {
            // keyboard and mouse hooks
            HookManager.KeyDown -= HookManager_KeyDown;
            HookManager.KeyUp -= HookManager_KeyUp;
            HookManager.KeyPress -= HookManager_KeyPress;
            HookManager.MouseDown -= HookManager_MouseDown;
            HookManager.MouseUp -= HookManager_MouseUp;
            HookManager.MouseWheel -= HookManager_MouseWheel;
            _file_system_watcher.Created -= _file_system_watcher_Created;
            _file_system_watcher.Deleted -= _file_system_watcher_Deleted;
            _file_system_watcher.Renamed -= _file_system_watcher_Renamed;
        }
        #endregion

        #region Timer Event Handler
        void _timer_Tick(object sender, EventArgs e)
        {
            _actions.ValidadeActions();
            _apprentice.Rebuild(_actions.UserActions);
            _assistant.GenerateSuggestions(_apprentice.GetLongestRepetitions(), _actions.Generalizations);
            if (_actions.UserActions.Count > 0)
            {
                _timer.Stop();
                _timer.Start();
            }
        }
        #endregion

        #region Public Methods
        public void StartMonitoring()
        {
            if (!_is_monitoring)
            {
                // file system watcher
                if (_working_path != null)
                    _file_system_watcher.EnableRaisingEvents = true;
                //_logger.WriteLine("Observer started at {0}.", DateTime.Now);
                _is_monitoring = true;
            }
        }

        public void StopMonitoring()
        {
            if (_is_monitoring)
            {
                // file system watcher
                _file_system_watcher.EnableRaisingEvents = false;
                //_logger.WriteLine("Observer stopped at {0}.", DateTime.Now);
                _is_monitoring = false;
                _modifiers = UserAction.Modifiers.None;
                _actions.Reset();
            }
        }

        public void StartMacroRecording()
        {
            if (!_is_recording)
            {
                _macro = new Macro();
                _macro.UseCompression = _use_compression;
                _macro.Start();
                _is_recording = true;
                //_logger.WriteLine("Macro recording started at {0}.", DateTime.Now);
            }
        }

        public void StopMacroRecording()
        {
            if (_is_recording)
            {
                _is_recording = false;
                _macro.Finish();
                //_logger.WriteLine("Macro recording stopped at {0}.", DateTime.Now);
            }
        }

        public void SaveScript(string folder)
        {
            SaveScript(folder, 0, 0);
        }

        public void SaveScript(string folder, int iskip, int fskip)
        {
            MacroNamePicker picker = new MacroNamePicker(folder);
            DialogResult result = picker.ShowDialog();
            if (result == DialogResult.OK)
                _macro.GenerateScriptFile(folder + picker.MacroName, iskip, fskip);
            picker.Dispose();
            //_logger.WriteLine("Macro saved: {0}", folder + picker.MacroName + ".py");
        }

        #region Event Handlers
        #region Keyboard Event Handlers
        void HookManager_KeyPress(object sender, KeyPressExEventArgs e)
        {
            try
            {
                UpdateWorkingDir(true);
                lock (_locker)
                {
                    if (_is_monitoring)
                    {
                        UserAction action;
                        if (e.KeyChar != char.MinValue && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyCode != Keys.Escape &&
                            ((_modifiers == ContextLib.DataContainers.Monitoring.UserAction.Modifiers.None) ||
                             (_modifiers == ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift) ||
                             (_modifiers == ContextLib.DataContainers.Monitoring.UserAction.Modifiers.CapsLock) ||
                             (_modifiers == (ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift | ContextLib.DataContainers.Monitoring.UserAction.Modifiers.CapsLock)) ||
                             (_modifiers == (ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt | ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl))))
                        {
                            action = new TypeTextAction(new string(e.KeyChar, 1));
                        }
                        else
                        {
                            action = new KeyPressAction(e.KeyCode, _modifiers);
                            ((KeyPressAction)action).KeyChar = e.KeyChar;
                        }
                        if (_is_recording)
                            _macro.AddAction(action);
                        if (!IsBrowsingExplorerWindow(action.Window))
                            _actions.AddAction(action);
                        else
                            _actions.ValidadeActions();
                        NotifyOtherAgents();
                        _last_user_action = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                UpdateWorkingDir(true);
                lock (_locker)
                {
                    HandleModifierKeyDown(e.KeyCode);// if it is a modifier key, don't log it
                    if (_is_monitoring)
                    {
                        NotifyOtherAgents();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                UpdateWorkingDir(true);
                lock (_locker)
                {
                    if (_is_monitoring)
                    {
                        if (HandleModifierKeyUp(e.KeyCode))
                        {
                            UserAction action = new KeyUpAction(e.KeyCode, _modifiers);

                            //if (e.KeyCode == Keys.RMenu)
                            //    MessageBox.Show("woot");

                            //if (e.KeyChar != char.MinValue && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyCode != Keys.Escape &&
                            //    ((_modifiers == ContextLib.DataContainers.Monitoring.UserAction.Modifiers.None) ||
                            //     (_modifiers == ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift) ||
                            //     (_modifiers == ContextLib.DataContainers.Monitoring.UserAction.Modifiers.CapsLock) ||
                            //     (_modifiers == (ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift | ContextLib.DataContainers.Monitoring.UserAction.Modifiers.CapsLock)) ||
                            //     (_modifiers == (ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt | ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl))))
                            //{
                            //    action = new TypeTextAction(new string(e.KeyChar, 1));
                            //}
                            //else
                            //{
                            //    if (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu ||
                            //        e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey ||
                            //        e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey ||
                            //        e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin ||
                            //        e.KeyCode == Keys.CapsLock)
                            //    {
                            //        ContextLib.DataContainers.Monitoring.UserAction.Modifiers mod = UserAction.Modifiers.None;
                            //        switch (e.KeyCode)
                            //        {
                            //            case Keys.LMenu:
                            //                mod &= ~UserAction.Modifiers.Alt;
                            //                break;
                            //            case Keys.RMenu:
                            //                mod &= ~UserAction.Modifiers.Alt;
                            //                break;
                            //            case Keys.LControlKey:
                            //                mod &= ~UserAction.Modifiers.Ctrl;
                            //                break;
                            //            case Keys.RControlKey:
                            //                mod &= ~UserAction.Modifiers.Ctrl;
                            //                break;
                            //            case Keys.LShiftKey:
                            //                mod &= ~UserAction.Modifiers.Shift;
                            //                break;
                            //            case Keys.RShiftKey:
                            //                mod &= ~UserAction.Modifiers.Shift;
                            //                break;
                            //            case Keys.LWin:
                            //                mod &= ~UserAction.Modifiers.Win;
                            //                break;
                            //            case Keys.RWin:
                            //                mod &= ~UserAction.Modifiers.Win;
                            //                break;
                            //        }
                            //        action = new KeyUpAction(e.KeyCode, mod);
                            //    }
                            //    else
                            //    {
                            //        action = new KeyPressAction(e.KeyCode, _modifiers);
                            //        ((KeyPressAction)action).KeyChar = e.KeyChar;
                            //    }
                            //}
                            //HandleModifierKeyUp(e.KeyCode);
                            if (_is_recording)
                                _macro.AddAction(action);
                            if (!IsBrowsingExplorerWindow(action.Window))
                                _actions.AddAction(action);
                            else
                                _actions.ValidadeActions();
                            NotifyOtherAgents();
                            _last_user_action = DateTime.Now;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region Mouse Event Handlers
        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                UpdateWorkingDir(true);
                lock (_locker)
                {

                    if (_is_monitoring)
                    {
                        UserAction action = new MouseDownAction(e.Button, (uint)e.X, (uint)e.Y, _modifiers);
                        if (_is_recording)
                            _macro.AddAction(action);
                        if (!IsBrowsingExplorerWindow(action.Window))
                            _actions.AddAction(action);
                        else
                            _actions.ValidadeActions();
                        NotifyOtherAgents();
                        _last_user_action = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                UpdateWorkingDir(e, true);
                lock (_locker)
                {

                    if (_is_monitoring)
                    {
                        UserAction action;

                        Window window = null;
                        if (_is_recording)
                        {
                            if (_macro.UserActions.Count > 0)
                            {
                                if (_macro.UserActions[_macro.UserActions.Count - 1].IsType(UserAction.UserActionType.MouseDownAction))
                                    window = _macro.UserActions[_macro.UserActions.Count - 1].Window;
                            }
                        }
                        else
                        {
                            if (_actions.UserActions.Count > 0)
                            {
                                if (_actions.UserActions[_actions.UserActions.Count - 1].IsType(UserAction.UserActionType.MouseDownAction))
                                    window = _actions.UserActions[_actions.UserActions.Count - 1].Window;
                            }
                        }
                        if (window != null)
                            action = new MouseUpAction(window, e.Button, (uint)e.X, (uint)e.Y, _modifiers);
                        else
                            action = new MouseUpAction(e.Button, (uint)e.X, (uint)e.Y, _modifiers);

                        if (_is_recording)
                            _macro.AddAction(action);
                        if (!IsBrowsingExplorerWindow(action.Window))
                            _actions.AddAction(action);
                        else
                            _actions.ValidadeActions();
                        NotifyOtherAgents();
                        _last_user_action = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void HookManager_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                UpdateWorkingDir(true);
                lock (_locker)
                {

                    if (_is_monitoring)
                    {
                        UserAction action = new MouseWheelSpinAction(e.Delta, (uint)e.X, (uint)e.Y, _modifiers);
                        if (_is_recording)
                            _macro.AddAction(action);
                        if (!IsBrowsingExplorerWindow(action.Window))
                            _actions.AddAction(action);
                        else
                            _actions.ValidadeActions();
                        NotifyOtherAgents();
                        _last_user_action = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region File System Event Handlers
        void _file_system_watcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                if ((DateTime.Now - _last_user_action) <= _accept_user_action)
                {
                    lock (_locker)
                    {
                        UpdateWorkingDir(false);

                        if (_is_monitoring)
                        {
                            UserAction action = new FileCreatedAction(e.FullPath, _working_path);
                            _actions.AddAction(action);
                            NotifyOtherAgents();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void _file_system_watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                if ((DateTime.Now - _last_user_action) <= _accept_user_action)
                {
                    lock (_locker)
                    {
                        UpdateWorkingDir(false);

                        if (_is_monitoring)
                        {
                            UserAction action = new FileDeletedAction(e.FullPath, _working_path);
                            _actions.AddAction(action);
                            NotifyOtherAgents();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void _file_system_watcher_Renamed(object sender, RenamedEventArgs e)
        {
            try
            {
                if ((DateTime.Now - _last_user_action) <= _accept_user_action)
                {
                    lock (_locker)
                    {

                        if (_is_monitoring)
                        {
                            UpdateWorkingDir(false);
                            UserAction action = new FileRenamedAction(e.OldFullPath, e.FullPath, _working_path);
                            _actions.AddAction(action);
                            NotifyOtherAgents();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        #endregion
        #endregion

        #region Private Methods
        private void UpdateWorkingDir(bool wait_for_acceptance)
        {
            UpdateWorkingDir(null, wait_for_acceptance);
        }

        private void UpdateWorkingDir(MouseEventArgs e, bool wait_for_acceptance)
        {
            if (DateTime.Now - _last_dir_updade > _accept_dir_update)
            {
                _last_dir_updade = DateTime.Now;
                //System.Media.SoundPlayer enter = new System.Media.SoundPlayer(@"D:\OLD\Saved Games\GaimPortable\App\Gaim\sounds\gaim\receive.wav");
                //enter.Play();
                Window window = new Window(Win32.GetForegroundWindow());
                if (IsBrowsingExplorerWindow(window))
                {

                    if (e != null)
                    {
                        if (e.X <= (window.X + window.Width) &&
                            e.X >= (window.X + window.Width - 100) &&
                            e.Y <= (window.Y + 20) &&
                            e.Y >= (window.Y))
                        {
                            return;
                        }
                    }

                    string working_path = CommonInfo.UserDesktop;
                    Thread get_path = new Thread(new ThreadStart(delegate() // evil hack T_T
                        {
                            working_path = WindowUtility.Instance.GetWExplorerUrl(new IntPtr(window.Handle));
                            if ((_working_path == null || working_path != _working_path) && Directory.Exists(working_path))
                            {
                                _working_path = working_path;
                                _file_system_watcher.Path = _working_path;
                                if (!_file_system_watcher.EnableRaisingEvents)
                                    _file_system_watcher.EnableRaisingEvents = true;
                                _working_window = window;
                                _last_dir_updade = DateTime.Now;
                            }
                        }));
                    get_path.SetApartmentState(ApartmentState.STA);
                    get_path.Start();
                }
                //System.Media.SoundPlayer exit = new System.Media.SoundPlayer(@"D:\OLD\Saved Games\GaimPortable\App\Gaim\sounds\gaim\send.wav");
                //exit.Play();
            }
        }

        private bool IsBrowsingExplorerWindow(Window wnd)
        {
            if (wnd.ProcessName == "explorer" && (wnd.ClassName == "CabinetWClass" || wnd.ClassName == "ExploreWClass" || wnd.ClassName == "Progman"))
                return true;
            else
                return false;
        }

        private void NotifyOtherAgents()
        {
            _apprentice.Rebuild(_actions.UserActions);
            _assistant.GenerateSuggestions(_apprentice.GetLongestRepetitions(), _actions.Generalizations);
            _timer.Stop();
            _timer.Start();
        }

        private void HandleModifierKeyDown(Keys key)
        {
            if (key == Keys.LMenu || key == Keys.RMenu) // alt
            {
                SetModifier(UserAction.Modifiers.Alt);
            }
            else if (key == Keys.LControlKey || key == Keys.RControlKey) // ctrl
            {
                SetModifier(UserAction.Modifiers.Ctrl);
            }
            else if (key == Keys.LShiftKey || key == Keys.RShiftKey) // shift
            {
                SetModifier(UserAction.Modifiers.Shift);
            }
            else if (key == Keys.LWin || key == Keys.RWin) // win
            {
                SetModifier(UserAction.Modifiers.Win);
            }
            //else if (key == Keys.CapsLock)
            //{
            //    ToggleModifier(UserAction.Modifiers.CapsLock);
            //}
        }

        private bool HandleModifierKeyUp(Keys key)
        {
            if (key == Keys.LMenu || key == Keys.RMenu) // alt
            {
                return UnsetModifier(UserAction.Modifiers.Alt);
            }
            else if (key == Keys.LControlKey || key == Keys.RControlKey) // ctrl
            {
                return UnsetModifier(UserAction.Modifiers.Ctrl);
            }
            else if (key == Keys.LShiftKey || key == Keys.RShiftKey) // shift
            {
                return UnsetModifier(UserAction.Modifiers.Shift);
            }
            else if (key == Keys.LWin || key == Keys.RWin) // win
            {
                return UnsetModifier(UserAction.Modifiers.Win);
            }
            return true;
        }

        public bool IsModifierSet(UserAction.Modifiers modifier)
        {
            return (_modifiers & modifier) == modifier;
        }

        private void ToggleModifier(UserAction.Modifiers modifier)
        {
            if (!IsModifierSet(modifier))
                _modifiers |= modifier;
            else
                _modifiers &= ~modifier;
        }

        private void SetModifier(UserAction.Modifiers modifier)
        {
            if (!IsModifierSet(modifier))
                _modifiers |= modifier;
        }

        private bool UnsetModifier(UserAction.Modifiers modifier)
        {
            if (IsModifierSet(modifier))
            {
                _modifiers &= ~modifier;
                return true;
            }
            else return false;
        }
        #endregion
    }
}
