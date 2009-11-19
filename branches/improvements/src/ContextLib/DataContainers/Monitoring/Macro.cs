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
using ContextLib.DataContainers.GUI;

namespace ContextLib.DataContainers.Monitoring
{
    public class Macro
    {
        #region Properties
        protected DateTime _start_time;
        protected DateTime _finish_time;
        protected List<UserAction> _user_actions;
        protected float _speed_multiplier;
        protected bool _use_compression;
        private UserAction _last_processed_action = null;
        private UserAction.Modifiers _modifiers = UserAction.Modifiers.None;
        #endregion

        #region Accessors
        public DateTime StartTime { get { return _start_time; } }
        public DateTime FinishTime { get { return _finish_time; } }
        public List<UserAction> UserActions { get { return _user_actions; } }
        public float SpeedMultiplier { get { return _speed_multiplier; } set { _speed_multiplier = value; } }
        public bool UseCompression { get { return _use_compression; } set { _use_compression = value; } }
        #endregion

        #region Constructors
        public Macro()
        {
            _user_actions = new List<UserAction>();
            _speed_multiplier = 1.0f;
            _use_compression = true;
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            _start_time = DateTime.Now;
        }

        public void Finish()
        {
            _finish_time = DateTime.Now;
        }

        public void AddAction(UserAction action)
        {
            if (action.Window == Window.InvalidWindow)
            {
                int index = _user_actions.Count - 1;
                if (index >= 0)
                    action.Window = _user_actions[index].Window;
            }

            if (UseCompression)
            {
                // if last actions was a MouseDown but the new one isn't a MouseUp, then replace the MouseDown by a MouseClick
                if ((GetLastAction() != null && GetLastAction().ActionType == UserAction.UserActionType.MouseDownAction))
                {
                    MouseDownAction temp_action = (MouseDownAction)GetLastAction();
                    if (action.ActionType == UserAction.UserActionType.MouseUpAction)
                    {
                        UserAction merged_action = action.Merge(temp_action);
                        if (merged_action == null) // if there is no possible merging, ignore the mouse up
                        {
                            MouseClickAction click = new MouseClickAction(temp_action.Window, temp_action.Button, temp_action.X, temp_action.Y, temp_action.Modifiers);
                            click.Time = temp_action.Time;
                            ReplaceLastAction(click);
                        }
                    }
                    else
                    {
                        MouseClickAction click = new MouseClickAction(temp_action.Window, temp_action.Button, temp_action.X, temp_action.Y, temp_action.Modifiers);
                        click.Time = temp_action.Time;
                        ReplaceLastAction(click);
                    }
                    //click.Id = temp_action.Id;
                    //_user_actions[_user_actions.Count - 1] = click;
                }

                // ignore KeyUp events
                if (action.ActionType == UserAction.UserActionType.KeyUpAction)
                {
                    UserAction last_action = _last_processed_action;
                    if (last_action != null)
                    {
                        KeyUpAction key_action = (KeyUpAction)action;
                        if (key_action.Key == System.Windows.Forms.Keys.LMenu ||
                            key_action.Key == System.Windows.Forms.Keys.RMenu)
                        {
                            if ((_modifiers & UserAction.Modifiers.Alt) != UserAction.Modifiers.Alt)
                            {
                                UserAction new_action = new KeyPressAction(last_action.Window, key_action.Key, key_action.Modifiers);
                                new_action.Time = key_action.Time;
                                BasicAddAction(new_action);
                                return;
                            }
                            else
                            {
                                _modifiers &= ~UserAction.Modifiers.Alt;
                                return;
                            }
                        }
                        else if (key_action.Key == System.Windows.Forms.Keys.LControlKey ||
                                 key_action.Key == System.Windows.Forms.Keys.RControlKey)
                        {
                            if ((_modifiers & UserAction.Modifiers.Ctrl) != UserAction.Modifiers.Ctrl)
                            {
                                UserAction new_action = new KeyPressAction(last_action.Window, key_action.Key, key_action.Modifiers);
                                new_action.Time = key_action.Time;
                                BasicAddAction(new_action);
                                return;
                            }
                            else
                            {
                                _modifiers &= ~UserAction.Modifiers.Ctrl;
                                return;
                            }
                        }
                        else if (key_action.Key == System.Windows.Forms.Keys.LShiftKey ||
                                 key_action.Key == System.Windows.Forms.Keys.RShiftKey)
                        {
                            if ((_modifiers & UserAction.Modifiers.Shift) != UserAction.Modifiers.Shift)
                            {
                                UserAction new_action = new KeyPressAction(last_action.Window, key_action.Key, key_action.Modifiers);
                                new_action.Time = key_action.Time;
                                BasicAddAction(new_action);
                                return;
                            }
                            else
                            {
                                _modifiers &= ~UserAction.Modifiers.Shift;
                                return;
                            }
                        }
                        else if (key_action.Key == System.Windows.Forms.Keys.LWin ||
                                 key_action.Key == System.Windows.Forms.Keys.RWin)
                        {
                            if ((_modifiers & UserAction.Modifiers.Win) != UserAction.Modifiers.Win)
                            {
                                UserAction new_action = new KeyPressAction(last_action.Window, key_action.Key, key_action.Modifiers);
                                new_action.Time = key_action.Time;
                                BasicAddAction(new_action);
                                return;
                            }
                            else
                            {
                                _modifiers &= ~UserAction.Modifiers.Win;
                                return;
                            }
                        }
                    }
                    else
                    {
                        _last_processed_action = (UserAction)action.Clone();
                        _modifiers = UserContext.Instance.ObserverObject.Modifiers;
                        return;
                    }
                }
                // ignore a MouseUp event if the last one isn't a MouseDown
                else if (action.ActionType == UserAction.UserActionType.MouseUpAction &&
                            (GetLastAction() == null || GetLastAction().ActionType != UserAction.UserActionType.MouseDownAction))
                {
                    _last_processed_action = action;
                    _modifiers = UserContext.Instance.ObserverObject.Modifiers;
                    return;
                }
                // if new action is a KeyDown, replace it by a KeyPress
                else if (action.ActionType == UserAction.UserActionType.KeyDownAction)
                {
                    KeyDownAction temp_action = (KeyDownAction)action;
                    UserAction new_action = new KeyPressAction(temp_action.Window, temp_action.Key, temp_action.Modifiers);
                    new_action.Time = temp_action.Time;
                    //new_action.Id = temp_action.Id;
                    BasicAddAction(new_action);
                }
                else if (action.ActionType == UserAction.UserActionType.KeyPressAction)
                {
                    KeyPressAction temp_action = (KeyPressAction)action;
                    BasicAddAction(temp_action);
                }
                // otherwise, just merge the last logged action with the new one
                else
                {
                    UserAction last_action = GetLastAction();
                    UserAction merged_action = action.Merge(last_action);
                    if (merged_action != null) // if merge was successful, replace the last action by the merged one
                    {
                        if (merged_action.IsType(UserAction.UserActionType.TerminalAction))
                        {
                            RemoveLastAction();
                        }
                        else
                        {
                            merged_action.Time = action.Time;
                            ReplaceLastAction(merged_action);
                        }
                    }
                    else // just add the new action
                    {
                        BasicAddAction(action);
                    }
                }
                // compress even more
                bool can_compress_more = true;
                while (can_compress_more)
                {
                    UserAction last_action = GetLastAction();
                    UserAction second_last_action = GetSecondLastAction();
                    if (last_action != null && second_last_action != null)
                    {
                        UserAction merged_action = last_action.Merge(second_last_action);
                        if (merged_action != null)
                        {
                            if (merged_action.IsType(UserAction.UserActionType.TerminalAction))
                            {
                                RemoveLastAction();
                                RemoveLastAction();
                            }
                            else
                            {
                                merged_action.Time = last_action.Time;
                                RemoveLastAction();
                                //merged_action.Id = second_last_action.Id;
                                //_user_actions[_user_actions.Count - 1] = merged_action;
                                ReplaceLastAction(merged_action);
                            }
                        }
                        else
                            can_compress_more = false;
                    }
                    else
                        can_compress_more = false;
                }
            }
            else
            {
                BasicAddAction(action);
            }
            _last_processed_action = (UserAction)action.Clone();
            _modifiers = UserContext.Instance.ObserverObject.Modifiers;
        }

        public void BasicAddAction(UserAction action)
        {
            _user_actions.Add(action);
        }

        public void RemoveAction(UserAction action)
        {
            _user_actions.Remove(action);
        }

        public void RemoveAction(int index)
        {
            _user_actions.RemoveAt(index);
        }

        public void GenerateScriptFile(string path, int iskip, int fskip)
        {
            StreamWriter writer = new StreamWriter(path, false);
            string code = GenerateIpyCode(iskip, fskip);
            writer.Write(code);
            writer.Close();
            writer.Dispose();
        }

        public string GenerateIpyCode(int iskip, int fskip)
        {
            string code = string.Empty;
            DateTime temp_time = _start_time;
            int action_count = 0;
            int script_action_count = _user_actions.Count;
            if (iskip != 0 || fskip != 0)
            {
                script_action_count = (iskip + fskip <= script_action_count ? script_action_count - (iskip + fskip) : 0);
            }

            // generate sumary
            code += "# Balze macro sumary:" + Environment.NewLine;
            code += "# Recoding began at " + _start_time.ToString() + " and ended at " + _finish_time.ToString() + "." + Environment.NewLine;
            code += "# A total of " + script_action_count + " user actions were recorded." + Environment.NewLine;
            code += Environment.NewLine;

            // generate imports
            code += "# Import required modules" + Environment.NewLine;
            code += "import sys" + Environment.NewLine;
            code += "import clr" + Environment.NewLine;
            code += "import System" + Environment.NewLine;
            code += "from System import Threading" + Environment.NewLine;
            code += "clr.AddReference(\"System.Windows.Forms\")" + Environment.NewLine;
            code += "clr.AddReference(\"ContextLib\")" + Environment.NewLine;
            code += "import ContextLib" + Environment.NewLine;
            code += "from ContextLib import *" + Environment.NewLine;
            code += Environment.NewLine;

            // generate speed multiplier
            code += "# Change this value to modify macro execution speed" + Environment.NewLine;
            code += "speed_multiplier = 1" + Environment.NewLine;
            code += Environment.NewLine;

            // generate amount of time to wait between repetitions
            code += "# Change this value to modify the amount of time to wait between repetitions, in milliseconds" + Environment.NewLine;
            code += "wait_time = 0" + Environment.NewLine;
            code += Environment.NewLine;

            // determine amount of iterations
            code += "repetitions = 1" + Environment.NewLine;
            code += "len = len(sys.argv)" + Environment.NewLine;
            code += "if len > 0 and sys.argv[0].isdigit():" + Environment.NewLine;
            code += "   repetitions = int(sys.argv[0])" + Environment.NewLine;
            code += Environment.NewLine;

            // generate the for loop
            code += "for index in range(repetitions):" + Environment.NewLine;

            // generate actions
            for (int i = 0; i < _user_actions.Count; i++)
            {
                if (i <= iskip - 1 || i >= _user_actions.Count - fskip)
                    continue;
                action_count++;
                UserAction action = _user_actions[i];
                code += "   # Action " + action_count.ToString() + " (at " + action.Time.ToString() + "):" + Environment.NewLine;
                code += "   Threading.Thread.Sleep(" + (int)((action.Time - temp_time).TotalMilliseconds / _speed_multiplier) + " / speed_multiplier)" + Environment.NewLine;
                //code += "   " + action.IpySnippet;
                string[] code_lines = action.IpySnippet.Split(new string[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < code_lines.Length; j++)
                    code += "   " + code_lines[j] + Environment.NewLine;
                temp_time = action.Time;
            }

            code += "   #Wait before executing the next repetition" + Environment.NewLine;
            code += "   Threading.Thread.Sleep(wait_time / speed_multiplier)" + Environment.NewLine;

            return code;
        }
        #endregion

        #region Private Methods
        private UserAction GetLastAction()
        {
            if (_user_actions.Count > 0)
                return _user_actions[_user_actions.Count - 1];
            else
                return null;
        }

        private UserAction GetSecondLastAction()
        {
            if (_user_actions.Count > 1)
                return _user_actions[_user_actions.Count - 2];
            else
                return null;
        }

        private void RemoveLastAction()
        {
            if (_user_actions.Count > 0)
            {
                RemoveAction(_user_actions.Count - 1);
            }
        }

        private void ReplaceLastAction(UserAction action)
        {
            RemoveLastAction();
            BasicAddAction(action);
        }
        #endregion
    }
}
