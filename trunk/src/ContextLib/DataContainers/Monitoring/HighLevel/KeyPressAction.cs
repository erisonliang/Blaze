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
using System.Windows.Forms;
using SystemCore.SystemAbstraction;
using ContextLib.DataContainers.GUI;

namespace ContextLib.DataContainers.Monitoring
{
    public class KeyPressAction : KeyAction
    {
        #region Properties
        private char _char;
        #endregion

        #region Accessors
        public override string IpySnippet
        {
            get
            {
                return
                    "window = ContextLib.DataContainers.GUI.Window(" + _window.Handle + ", \"" + _window.ClassName + "\", " + _window.ThreadID + ", " + _window.ProcessID + ", \"" + _window.ProcessName + "\", \"" + _window.Title + "\", " + _window.X + ", " + _window.Y + ", " + _window.Width + ", " + _window.Height + ")" + Environment.NewLine +
                    "action = ContextLib.DataContainers.Monitoring.KeyPressAction(window, System.Windows.Forms.Keys." + _key + ", " + ModifiersIpySnippet + ")" + Environment.NewLine +
                    "action.Execute()" + Environment.NewLine;
            }
        }

        public char KeyChar { get { return _char; } set { _char = value; } }
        #endregion
        
        #region Constructors
        public KeyPressAction(Keys key, Modifiers modifiers)
            : base(key, modifiers)
        {
            _description = "Press " + key.ToString() + " key (Modifiers: " + modifiers + ") on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "KeyPress(" + _key.ToString() + ", " + ((int)modifiers).ToString() + ")";
            _type = UserActionType.KeyPressAction;
            _char = char.MinValue;
        }

        public KeyPressAction(Window window, Keys key, Modifiers modifiers)
            : base(window, key, modifiers)
        {
            _description = "Press " + key.ToString() + " key (Modifiers: " + modifiers + ") on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "KeyPress(" + _key.ToString() + ", " + ((int)modifiers).ToString() + ")";
            _type = UserActionType.KeyPressAction;
            _char = char.MinValue;
        }
        #endregion

        #region Public Methods
        public override void Execute()
        {
            BringWindowToFront();
            PressModifierKeys();
            Win32.keybd_event((byte)_key, (byte)Win32.MapVirtualKey((uint)_key, 0), Win32.KEYEVENTF_EXTENDEDKEY | 0, 0);
            System.Threading.Thread.Sleep(15);
            Win32.keybd_event((byte)_key, (byte)Win32.MapVirtualKey((uint)_key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
            ReleaseModifierKeys();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            KeyPressAction action = (KeyPressAction)obj;
            if (action == null) // check if it can be casted
                return false;

            if (this.Id > -1 && action.Id > -1) // id already specified
            {
                if (this.Id == action.Id)
                    return true;
                else
                    return false;
            }
            else
            {
                if (action.ActionType == UserActionType.KeyPressAction)
                {
                    if (((KeyPressAction)action)._key == this._key)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public override bool Equals(UserAction action)
        {
            if ((object)action == null)
                return false;

            if (this.Id > -1 && action.Id > -1) // id already specified
            {
                if (this.Id == action.Id)
                    return true;
                else
                    return false;
            }
            else
            {
                if (action.ActionType == UserActionType.KeyPressAction)
                {
                    if (((KeyPressAction)action)._key == this._key)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public override object Clone()
        {
            KeyPressAction action = new KeyPressAction(new Window(_window), _key, _modifiers);
            action.Id = this.Id;
            action.Time = this.Time;
            return action;
        }

        public override UserAction Merge(UserAction action)
        {
            UserAction ret = null;
            //if (action != null)
            //{
            //    if (action.ActionType == UserActionType.KeyPressAction)
            //    {
            //        KeyPressAction k_action = (KeyPressAction)action;
            //        if (this.Window.Handle == k_action.Window.Handle &&
            //            ((this.Modifiers == Modifiers.None) || (this.Modifiers == Modifiers.Shift) || (this.Modifiers == Modifiers.CapsLock) || (this.Modifiers == (Modifiers.Alt | Modifiers.Ctrl))) &&
            //            ((k_action.Modifiers == Modifiers.None) || (k_action.Modifiers == Modifiers.Shift) || (k_action.Modifiers == Modifiers.CapsLock) || (k_action.Modifiers == (Modifiers.Alt | Modifiers.Ctrl))))
            //        {
            //            if (this.Key != Keys.Escape && k_action.Key != Keys.Escape)
            //            {
            //                char c_a = k_action.KeyChar;
            //                char c_b = this.KeyChar;
            //                if (c_a != char.MinValue && c_a != '\r' && c_a != '\b' &&
            //                    c_b != char.MinValue && c_b != '\r' && c_b != '\b')
            //                {
            //                    ret = new TypeTextAction(k_action.Window, new string( new char[] { c_a , c_b } ));
            //                    ret.Time = this._time;
            //                }
            //            }
            //        }
            //    }
            //    else if (action.ActionType == UserActionType.TypeTextAction)
            //    {
            //        TypeTextAction t_action = (TypeTextAction)action;
            //        if (this.Window.Handle == t_action.Window.Handle &&
            //            (this.Modifiers == Modifiers.None) || (this.Modifiers == Modifiers.Shift) || (this.Modifiers == Modifiers.CapsLock) || (this.Modifiers == (Modifiers.Alt | Modifiers.Ctrl)))
            //        {
            //            if (this.Key == Keys.Back)
            //            {
            //                string c_a = t_action.Text;
            //                if (c_a.Length > 1)
            //                {
            //                    ret = new TypeTextAction(t_action.Window, c_a.Substring(0, c_a.Length - 1));
            //                    ret.Time = this._time;
            //                }
            //                else
            //                    ret = this;
            //            }
            //            else if (this.Key != Keys.Escape)
            //            {
            //                string c_a = t_action.Text;
            //                char c_b = this.KeyChar;
            //                //string c_b = "A";
            //                if (c_b != char.MinValue && c_b != '\r' && c_b != '\b')
            //                {
            //                    ret = new TypeTextAction(t_action.Window, c_a + c_b);
            //                    ret.Time = this._time;
            //                }
            //            }
            //        }
            //    }
            //}
            if (action.ActionType == UserActionType.TypeTextAction)
            {
                TypeTextAction t_action = (TypeTextAction)action;
                if (this.Window.Handle == t_action.Window.Handle &&
                    ((_modifiers == ContextLib.DataContainers.Monitoring.UserAction.Modifiers.None) ||
                     (_modifiers == ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift) ||
                     (_modifiers == ContextLib.DataContainers.Monitoring.UserAction.Modifiers.CapsLock) ||
                     (_modifiers == (ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift | ContextLib.DataContainers.Monitoring.UserAction.Modifiers.CapsLock)) ||
                     (_modifiers == (ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt | ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl))))
                {
                    if (this.Key == Keys.Back)
                    {
                        string c_a = t_action.Text;
                        if (c_a.Length > 1)
                        {
                            ret = new TypeTextAction(t_action.Window, c_a.Substring(0, c_a.Length - 1));
                            ret.Time = this._time;
                        }
                        else
                            ret = this;
                    }
                }
            }
            return ret;
        }
        #endregion
    }
}
