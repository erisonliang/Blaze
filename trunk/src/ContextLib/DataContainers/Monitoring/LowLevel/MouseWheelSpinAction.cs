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
    public class MouseWheelSpinAction : MouseAction
    {
        #region Properties
        private int _delta;
        private uint _x;
        private uint _y;
        #endregion

        #region Accessors
        public int Delta { get { return _delta; } set { _delta = value; } }

        public uint X { get { return _x; } set { _x = value; } }

        public uint Y { get { return _y; } set { _y = value; } }

        public override string IpySnippet
        {
            get {
                return
                    "window = ContextLib.DataContainers.GUI.Window(" + _window.Handle + ", \"" + _window.ClassName + "\", " + _window.ThreadID + ", " + _window.ProcessID + ", \"" + _window.ProcessName + "\", \"" + _window.Title + "\", " + _window.X + ", " + _window.Y + ", " + _window.Width + ", " + _window.Height + ")" + Environment.NewLine +
                    "action = ContextLib.DataContainers.Monitoring.MouseWheelSpinAction(window, " + _delta + ", " + _x + ", " + _y + ", " + ModifiersIpySnippet + ")" + Environment.NewLine +
                    "action.Execute()" + Environment.NewLine;
            }
        }
        #endregion

        #region Constructors
        public MouseWheelSpinAction(int delta, uint x, uint y, Modifiers modifiers)
            : base(modifiers)
        {
            _delta = delta;
            _x = x;
            _y = y;
            _description = "Spin mouse wheel by " + delta.ToString() + " (Modifiers: " + modifiers + ") at (" + _x + "," + _y + ") on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "MouseWheelSpin(" + delta.ToString() + ", " + x.ToString() + ", " + y.ToString() + ", " + ((int)modifiers).ToString() + ")";
            _type = UserActionType.MouseWheelSpinAction;
        }

        public MouseWheelSpinAction(Window window, int delta, uint x, uint y, Modifiers modifiers)
            : base(window, modifiers)
        {
            _delta = delta;
            _x = x;
            _y = y;
            _description = "Spin mouse wheel by " + delta.ToString() + " (Modifiers: " + modifiers + ") at (" + _x + "," + _y + ") on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "MouseWheelSpin(" + delta.ToString() + ", " + x.ToString() + ", " + y.ToString() + ", " + ((int)modifiers).ToString() + ")";
            _type = UserActionType.MouseWheelSpinAction;
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
            Win32.mouse_event((uint)MouseEventFlags.MOUSEEVENTF_WHEEL, 0, 0, (uint)_delta, 0);
            ReleaseModifierKeys();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            MouseWheelSpinAction action = (MouseWheelSpinAction)obj;
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
                if (action.ActionType == UserActionType.MouseWheelSpinAction)
                {
                    MouseWheelSpinAction m_action = (MouseWheelSpinAction)action;
                    if (this._delta == m_action._delta)
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
                if (action.ActionType == UserActionType.MouseWheelSpinAction)
                {
                    MouseWheelSpinAction m_action = (MouseWheelSpinAction)action;
                    if (this._delta == m_action._delta)
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
            MouseWheelSpinAction action = new MouseWheelSpinAction(new Window(_window), _delta, _x, _y, _modifiers);
            action.Id = this.Id;
            action.Time = this.Time;
            return action;
        }

        public override UserAction Merge(UserAction action)
        {
            UserAction ret = null;
            if (action != null && action.ActionType == UserActionType.MouseWheelSpinAction)
            {
                MouseWheelSpinAction m_action = (MouseWheelSpinAction)action;
                if ((this.Delta >= 0 && m_action.Delta >= 0) || (this.Delta <= 0 && m_action.Delta <= 0))
                {
                    if (this.Window.Handle == m_action.Window.Handle &&
                        this.Modifiers == m_action.Modifiers &&
                        this.X <= m_action.X + MAX_X_OFFSET * 2 &&
                        this.X >= m_action.X - MAX_X_OFFSET * 2 &&
                        this.Y <= m_action.Y + MAX_Y_OFFSET * 2 &&
                        this.Y >= m_action.Y - MAX_Y_OFFSET * 2)
                    {
                        ret = new MouseWheelSpinAction(m_action.Window, (this.Delta + m_action.Delta), m_action.X, m_action.Y, m_action.Modifiers);
                        ret.Time = this._time;
                    }
                }
            }
            return ret;
        }
        #endregion
    }
}
