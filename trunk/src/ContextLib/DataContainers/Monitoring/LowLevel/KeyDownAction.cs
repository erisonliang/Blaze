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
    public class KeyDownAction : KeyAction
    {
        #region Properties
        #endregion

        #region Accessors
        public override string IpySnippet
        {
            get
            {
                return
                    "window = ContextLib.DataContainers.GUI.Window(" + _window.Handle + ", \"" + _window.ClassName + "\", " + _window.ThreadID + ", " + _window.ProcessID + ", \"" + _window.ProcessName + "\", \"" + _window.Title + "\", " + _window.X + ", " + _window.Y + ", " + _window.Width + ", " + _window.Height + ")" + Environment.NewLine +
                    "action = ContextLib.DataContainers.Monitoring.KeyDownAction(window, System.Windows.Forms.Keys." + _key + ", " + ModifiersIpySnippet + ")" + Environment.NewLine +
                    "action.Execute()" + Environment.NewLine;
            }
        }
        #endregion
        
        #region Constructors
        public KeyDownAction(Keys key, Modifiers modifiers)
            : base(key, modifiers)
        {
            _description = "Press " + key.ToString() + " key (Modifiers: " + modifiers + ") on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "KeyDown(" + _key.ToString() + ", " + ((int)modifiers).ToString() + ")";
            _type = UserActionType.KeyDownAction;
        }

        public KeyDownAction(Window window, Keys key, Modifiers modifiers)
            : base(window, key, modifiers)
        {
            _description = "Press " + key.ToString() + " key (Modifiers: " + modifiers + ") on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "KeyDown(" + _key.ToString() + ", " + ((int)modifiers).ToString() + ")";
            _type = UserActionType.KeyDownAction;
        }
        #endregion

        #region Public Methods
        public override void Execute()
        {
            BringWindowToFront();
            PressModifierKeys();
            Win32.keybd_event((byte)_key, (byte)Win32.MapVirtualKey((uint)_key, 0), Win32.KEYEVENTF_EXTENDEDKEY | 0, 0);
            ReleaseModifierKeys();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            KeyDownAction action = (KeyDownAction)obj;
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
                if (action.ActionType == UserActionType.KeyDownAction)
                {
                    if (((KeyDownAction)action)._key == this._key)
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
                if (action.ActionType == UserActionType.KeyDownAction)
                {
                    if (((KeyDownAction)action)._key == this._key)
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
            KeyDownAction action = new KeyDownAction(new Window(_window), _key, _modifiers);
            action.Id = this.Id;
            action.Time = this.Time;
            return action;
        }

        public override UserAction Merge(UserAction action)
        {
            return null;
        }
        #endregion
    }
}
