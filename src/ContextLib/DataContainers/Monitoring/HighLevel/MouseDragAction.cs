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
using System.Threading;
using System.Windows.Forms;
using SystemCore.SystemAbstraction;
using ContextLib.DataContainers.GUI;

namespace ContextLib.DataContainers.Monitoring
{
    public class MouseDragAction : MouseAction
    {
        #region Properties
        private MouseButtons _button;
        private uint _initial_x;
        private uint _initial_y;
        private uint _final_x;
        private uint _final_y;
        #endregion

        #region Accessors
        public MouseButtons Button { get { return _button; } set { _button = value; } }

        public uint InitialX { get { return _initial_x; } set { _initial_x = value; } }

        public uint InitialY { get { return _initial_y; } set { _initial_y = value; } }

        public uint FinalX { get { return _final_x; } set { _final_x = value; } }

        public uint FinalY { get { return _final_y; } set { _final_y = value; } }

        public override string IpySnippet
        {
            get
            {
                return
                    "window = ContextLib.DataContainers.GUI.Window(" + _window.Handle + ", \"" + _window.ClassName + "\", " + _window.ThreadID + ", " + _window.ProcessID + ", \"" + _window.ProcessName + "\", \"" + _window.Title + "\", " + _window.X + ", " + _window.Y + ", " + _window.Width + ", " + _window.Height + ")" + Environment.NewLine +
                    "action = ContextLib.DataContainers.Monitoring.MouseDragAction(window, System.Windows.Forms.MouseButtons." + _button + ", " + _initial_x + ", " + _initial_y + ", " + _final_x + ", " + _final_y + ", " + ModifiersIpySnippet + ")" + Environment.NewLine +
                    "action.Execute()" + Environment.NewLine;
            }
        }
        #endregion

        #region Constructors
        public MouseDragAction(MouseButtons button, uint initial_x, uint initial_y, uint final_x, uint final_y, Modifiers modifiers)
            : base(modifiers)
        {
            _button = button;
            _initial_x = initial_x;
            _initial_y = initial_y;
            _final_x = final_x;
            _final_y = final_y;
            _description = button.ToString() + " mouse drag (Modifiers: " + modifiers + ") from (" + _initial_x + "," + _initial_y + ") to (" + _final_x + "," + _final_y + ") on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "MouseDrag(" + button.ToString() + ", " + initial_x.ToString() + ", " + initial_y.ToString() + ", " + final_x.ToString() + ", " + final_y.ToString() + ", " + ((int)modifiers).ToString() + ")"; 
            _type = UserActionType.MouseDragAction;
        }

        public MouseDragAction(Window window, MouseButtons button, uint initial_x, uint initial_y, uint final_x, uint final_y, Modifiers modifiers)
            : base(window, modifiers)
        {
            _button = button;
            _initial_x = initial_x;
            _initial_y = initial_y;
            _final_x = final_x;
            _final_y = final_y;
            _description = button.ToString() + " mouse drag (Modifiers: " + modifiers + ") from (" + _initial_x + "," + _initial_y + ") to (" + _final_x + "," + _final_y + ") on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "MouseDrag(" + button.ToString() + ", " + initial_x.ToString() + ", " + initial_y.ToString() + ", " + final_x.ToString() + ", " + final_y.ToString() + ", " + ((int)modifiers).ToString() + ")"; 
            _type = UserActionType.MouseDragAction;
        }
        #endregion

        #region Public Methods
        public override void Execute()
        {
            BringWindowToFront();
            Window window = new Window(Win32.GetForegroundWindow());
            uint initial_x = _initial_x, initial_y = _initial_y;
            uint final_x = _final_x, final_y = _final_y;
            if (window.ProcessName == _window.ProcessName && window.ClassName == _window.ClassName)
            {
                uint offx = (uint)((int)_initial_x - _window.X);
                uint offy = (uint)((int)_initial_y - _window.Y);
                initial_x = (uint)window.X + offx;
                initial_y = (uint)window.Y + offy;
                offx = (uint)((int)_final_x - _window.X);
                offy = (uint)((int)_final_y - _window.Y);
                final_x = (uint)window.X + offx;
                final_y = (uint)window.Y + offy;
            }
            uint initial_absx = (uint)(initial_x * 65535 / Screen.PrimaryScreen.Bounds.Width);
            uint initial_absy = (uint)(initial_y * 65535 / Screen.PrimaryScreen.Bounds.Height);
            uint final_absx = (uint)(final_x * 65535 / Screen.PrimaryScreen.Bounds.Width);
            uint final_absy = (uint)(final_y * 65535 / Screen.PrimaryScreen.Bounds.Height);

            if (initial_absx > 65535)
                initial_absx = 65535;
            else if (initial_absx < 0)
                initial_absx = 0;
            if (initial_absy > 65535)
                initial_absy = 65535;
            else if (initial_absy < 0)
                initial_absy = 0;

            if (final_absx > 65535)
                final_absx = 65535;
            else if (final_absx < 0)
                final_absx = 0;
            if (final_absy > 65535)
                final_absy = 65535;
            else if (final_absy < 0)
                final_absy = 0;

            PressModifierKeys();
            Win32.mouse_event((uint)(MouseEventFlags.MOVE | MouseEventFlags.ABSOLUTE), initial_absx, initial_absy, 0, 0);
            Thread.Sleep(60);
            switch (_button)
            {
                case MouseButtons.Left:
                    Win32.mouse_event((uint)(MouseEventFlags.LEFTDOWN | MouseEventFlags.ABSOLUTE), initial_absx, initial_absy, 0, 0);
                    Thread.Sleep(15);
                    Win32.mouse_event((uint)(MouseEventFlags.MOVE | MouseEventFlags.ABSOLUTE), final_absx, final_absy, 0, 0);
                    Thread.Sleep(60);
                    Win32.mouse_event((uint)(MouseEventFlags.LEFTUP | MouseEventFlags.ABSOLUTE), initial_absx, initial_absy, 0, 0);
                    break;
                case MouseButtons.Right:
                    Win32.mouse_event((uint)(MouseEventFlags.RIGHTDOWN | MouseEventFlags.ABSOLUTE), initial_absx, initial_absy, 0, 0);
                    Thread.Sleep(15);
                    Win32.mouse_event((uint)(MouseEventFlags.MOVE | MouseEventFlags.ABSOLUTE), final_absx, final_absy, 0, 0);
                    Thread.Sleep(60);
                    Win32.mouse_event((uint)(MouseEventFlags.RIGHTUP | MouseEventFlags.ABSOLUTE), final_absx, final_absy, 0, 0);
                    break;
                case MouseButtons.Middle:
                    Win32.mouse_event((uint)(MouseEventFlags.MIDDLEDOWN | MouseEventFlags.ABSOLUTE), initial_absx, initial_absy, 0, 0);
                    Thread.Sleep(15);
                    Win32.mouse_event((uint)(MouseEventFlags.MOVE | MouseEventFlags.ABSOLUTE), final_absx, final_absy, 0, 0);
                    Thread.Sleep(60);
                    Win32.mouse_event((uint)(MouseEventFlags.MIDDLEUP | MouseEventFlags.ABSOLUTE), final_absx, final_absy, 0, 0);
                    break;
                case MouseButtons.XButton1:
                    Win32.mouse_event((uint)(MouseEventFlags.MOUSEEVENTF_XDOWN | MouseEventFlags.ABSOLUTE), initial_absx, initial_absy, (uint)MouseEventFlags.XBUTTON1, 0);
                    Thread.Sleep(15);
                    Win32.mouse_event((uint)(MouseEventFlags.MOVE | MouseEventFlags.ABSOLUTE), final_absx, final_absy, 0, 0);
                    Thread.Sleep(60);
                    Win32.mouse_event((uint)(MouseEventFlags.MOUSEEVENTF_XUP | MouseEventFlags.ABSOLUTE), final_absx, final_absy, (uint)MouseEventFlags.XBUTTON1, 0);
                    break;
                case MouseButtons.XButton2:
                    Win32.mouse_event((uint)(MouseEventFlags.MOUSEEVENTF_XDOWN | MouseEventFlags.ABSOLUTE), initial_absx, initial_absy, (uint)MouseEventFlags.XBUTTON2, 0);
                    Thread.Sleep(15);
                    Win32.mouse_event((uint)(MouseEventFlags.MOVE | MouseEventFlags.ABSOLUTE), final_absx, final_absy, 0, 0);
                    Thread.Sleep(60);
                    Win32.mouse_event((uint)(MouseEventFlags.MOUSEEVENTF_XUP | MouseEventFlags.ABSOLUTE), final_absx, final_absy, (uint)MouseEventFlags.XBUTTON2, 0);
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

            MouseDragAction action = (MouseDragAction)obj;
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
                if (action.ActionType == UserActionType.MouseDragAction)
                {
                    MouseDragAction m_action = (MouseDragAction)action;
                    if (m_action._button == this._button)
                    {
                        if (this._initial_x <= (int)(m_action._initial_x + MAX_X_OFFSET) &&
                            this._initial_x >= (int)(m_action._initial_x - MAX_X_OFFSET) &&
                            this._initial_y <= (int)(m_action._initial_y + MAX_Y_OFFSET) &&
                            this._initial_y >= (int)(m_action._initial_y - MAX_Y_OFFSET) &&
                            this._final_x <= (int)(m_action._final_x + MAX_X_OFFSET) &&
                            this._final_x >= (int)(m_action._final_x - MAX_X_OFFSET) &&
                            this._final_y <= (int)(m_action._final_y + MAX_Y_OFFSET) &&
                            this._final_y >= (int)(m_action._final_y - MAX_Y_OFFSET))
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
                if (action.ActionType == UserActionType.MouseDragAction)
                {
                    MouseDragAction m_action = (MouseDragAction)action;
                    if (m_action._button == this._button)
                    {
                        if (this._initial_x <= (int)(m_action._initial_x + MAX_X_OFFSET) &&
                            this._initial_x >= (int)(m_action._initial_x - MAX_X_OFFSET) &&
                            this._initial_y <= (int)(m_action._initial_y + MAX_Y_OFFSET) &&
                            this._initial_y >= (int)(m_action._initial_y - MAX_Y_OFFSET) &&
                            this._final_x <= (int)(m_action._final_x + MAX_X_OFFSET) &&
                            this._final_x >= (int)(m_action._final_x - MAX_X_OFFSET) &&
                            this._final_y <= (int)(m_action._final_y + MAX_Y_OFFSET) &&
                            this._final_y >= (int)(m_action._final_y - MAX_Y_OFFSET))
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
            MouseDragAction action = new MouseDragAction(new Window(_window), _button, _initial_x, _initial_y, _final_x, _final_y, _modifiers);
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
