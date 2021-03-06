﻿// Blaze: Automated Desktop Experience
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
using System.Threading;
using System.Windows.Forms;
using SystemCore.SystemAbstraction;
using ContextLib.DataContainers.GUI;

namespace ContextLib.DataContainers.Monitoring
{
    public class MouseDownAction : MouseAction
    {
        #region Properties
        private MouseButtons _button;
        private uint _x;
        private uint _y;
        #endregion

        #region Accessors
        public MouseButtons Button { get { return _button; } }

        public uint X { get { return _x; } }

        public uint Y { get { return _y; } }

        public override string IpySnippet
        {
            get
            {
                return
                    "window = ContextLib.DataContainers.GUI.Window(" + _window.Handle + ", \"" + _window.ClassName + "\", " + _window.ThreadID + ", " + _window.ProcessID + ", \"" + _window.ProcessName + "\", \"" + _window.Title + "\", " + _window.X + ", " + _window.Y + ", " + _window.Width + ", " + _window.Height + ")" + Environment.NewLine +
                    "action = ContextLib.DataContainers.Monitoring.MouseDownAction(window, System.Windows.Forms.MouseButtons." + _button + ", " + _x + ", " + _y + ", " + ModifiersIpySnippet + ")" + Environment.NewLine +
                    "action.Execute()" + Environment.NewLine;
            }
        }
        #endregion

        #region Constructors
        public MouseDownAction(MouseButtons button, uint x, uint y, Modifiers modifiers)
            : base(modifiers)
        {
            _button = button;
            _x = x;
            _y = y;
            _description = "Press " + button.ToString() + " mouse button (Modifiers: " + modifiers + ") at (" + _x + "," + _y + ") on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "MouseDown(" + button.ToString() + ", " + x.ToString() + ", " + y.ToString() + ", " + ((int)modifiers).ToString() + ")"; 
            _type = UserActionType.MouseDownAction;
        }

        public MouseDownAction(Window window, MouseButtons button, uint x, uint y, Modifiers modifiers)
            : base(window, modifiers)
        {
            _button = button;
            _x = x;
            _y = y;
            _description = "Press " + button.ToString() + " mouse button (Modifiers: " + modifiers + ") at (" + _x + "," + _y + ") on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "MouseDown(" + button.ToString() + ", " + x.ToString() + ", " + y.ToString() + ", " + ((int)modifiers).ToString() + ")";
            _type = UserActionType.MouseDownAction;
        }
        #endregion

        #region Public Methods
        public override void Execute()
        {
            BringWindowToFront();
            Window window = new Window(Win32.GetForegroundWindow());
            uint x = _x, y = _y;
            if (window.ProcessName == _window.ProcessName && window.ClassName == _window.ClassName)
            {
                uint offx = (uint)((int)_x - _window.X);
                uint offy = (uint)((int)_y - _window.Y);
                x = (uint)window.X + offx;
                y = (uint)window.Y + offy;
            }
            uint absx = (uint)(x * 65535 / Screen.PrimaryScreen.Bounds.Width);
            uint absy = (uint)(y * 65535 / Screen.PrimaryScreen.Bounds.Height);
            if (absx > 65535)
                absx = 65535;
            else if (absx < 0)
                absx = 0;
            if (absy > 65535)
                absy = 65535;
            else if (absy < 0)
                absy = 0;

            PressModifierKeys();
            Win32.mouse_event((uint)(MouseEventFlags.MOVE | MouseEventFlags.ABSOLUTE), absx, absy, 0, 0);
            Thread.Sleep(60);
            switch (_button)
            {
                case MouseButtons.Left:
                    Win32.mouse_event((uint)(MouseEventFlags.LEFTDOWN | MouseEventFlags.ABSOLUTE), absx, absy, 0, 0);
                    break;
                case MouseButtons.Right:
                    Win32.mouse_event((uint)(MouseEventFlags.RIGHTDOWN | MouseEventFlags.ABSOLUTE), absx, absy, 0, 0);
                    break;
                case MouseButtons.Middle:
                    Win32.mouse_event((uint)(MouseEventFlags.MIDDLEDOWN | MouseEventFlags.ABSOLUTE), absx, absy, 0, 0);
                    break;
                case MouseButtons.XButton1:
                    Win32.mouse_event((uint)(MouseEventFlags.MOUSEEVENTF_XDOWN | MouseEventFlags.ABSOLUTE), absx, absy, (uint)MouseEventFlags.XBUTTON1, 0);
                    break;
                case MouseButtons.XButton2:
                    Win32.mouse_event((uint)(MouseEventFlags.MOUSEEVENTF_XDOWN | MouseEventFlags.ABSOLUTE), absx, absy, (uint)MouseEventFlags.XBUTTON2, 0);
                    break;
            }
            ReleaseModifierKeys();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            MouseDownAction action = (MouseDownAction)obj;
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
                if (action.ActionType == UserActionType.MouseDownAction)
                {
                    MouseDownAction m_action = (MouseDownAction)action;
                    if (m_action._button == this._button)
                    {
                        if (this._x <= (int)(m_action._x + MAX_X_OFFSET) &&
                            this._x >= (int)(m_action._x - MAX_X_OFFSET) &&
                            this._y <= (int)(m_action._y + MAX_Y_OFFSET) &&
                            this._y >= (int)(m_action._y - MAX_Y_OFFSET))
                            return true;
                        else
                            return false;
                    }
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
                if (action.ActionType == UserActionType.MouseDownAction)
                {
                    MouseDownAction m_action = (MouseDownAction)action;
                    if (m_action._button == this._button)
                    {
                        if (this._x <= (int)(m_action._x + MAX_X_OFFSET) &&
                            this._x >= (int)(m_action._x - MAX_X_OFFSET) &&
                            this._y <= (int)(m_action._y + MAX_Y_OFFSET) &&
                            this._y >= (int)(m_action._y - MAX_Y_OFFSET))
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public override object Clone()
        {
            MouseDownAction action = new MouseDownAction(new Window(_window), _button, _x, _y, _modifiers);
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
