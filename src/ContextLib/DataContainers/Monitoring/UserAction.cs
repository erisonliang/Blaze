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
using SystemCore.SystemAbstraction;
using ContextLib.DataContainers.GUI;

namespace ContextLib.DataContainers.Monitoring
{
    public abstract class UserAction : ICloneable
    {
        #region Properties
        protected string _description;
        protected string _quick_description;
        protected DateTime _time;
        protected Window _window;
        protected UserActionType _type;
        protected short _id = -1;
        #endregion

        #region Public Enums
        [Flags]
        public enum UserActionType : int
        {
            LowLevelAction = 0x1000,
            HighLevelAction = 0x2000,
            KeyAction = 0x0002,
            MouseAction = 0x0040,
            FileAction = 0x0400,
            TerminalAction = 0x1001,
            KeyDownAction = 0x1003,
            KeyUpAction = 0x1006,
            MouseDownAction = 0x1050,
            MouseUpAction = 0x1060,
            MouseWheelSpinAction = 0x10C0,
            FileCreatedAction = 0x2500,
            FileDeletedAction = 0x2600,
            FileRenamedAction = 0x2C00,
            FileMovedAction = 0x3C00,
            KeyPressAction = 0x2003,
            TypeTextAction = 0x2004,
            MouseClickAction = 0x2050,
            MouseDoubleClickAction = 0x2060,
            MouseDragAction = 0x20C0
        }

        [Flags]
        public enum Modifiers : int
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            Win = 8,
            CapsLock = 16
        }
        #endregion

        #region Accessors
        public string Description { get { return _description; } set { _description = value; } }
        public string QuickDescription { get { return _quick_description; } set { _quick_description = value; } }
        public DateTime Time { get { return _time; } set { _time = value; } }
        public Window Window { get { return _window; } set { _window = value; } }
        public UserActionType ActionType { get { return _type; } }
        public short Id { get { return _id; } set { _id = value; } }
        public abstract string IpySnippet { get; }
        #endregion

        #region Constructors
        public UserAction()
        {
            _time = DateTime.Now;
            try
            {
                _window = new Window(Win32.GetForegroundWindow());
            }
            catch
            {
                _window = Window.InvalidWindow;
            }
        }

        public UserAction(Window window)
        {
            _time = DateTime.Now;
            _window = window;
        }
        #endregion

        #region Public Methods
        public abstract void Execute();

        public abstract override bool Equals(object obj);

        public abstract bool Equals(UserAction action);

        public abstract object Clone();

        public abstract UserAction Merge(UserAction action);

        public bool IsType(UserActionType type)
        {
            return (_type & type) == type;
        }
        #endregion

        #region Protected Methods
        protected void BringWindowToFront()
        {
            if (!SystemCore.SystemAbstraction.WindowManagement.WindowUtility.Instance.BringWindowToFrontIfNeeded((IntPtr)_window.Handle))
            {
                if (!SystemCore.SystemAbstraction.WindowManagement.WindowUtility.Instance.BringWindowToFrontIfNeeded(_window.ClassName, _window.Title))
                {
                    IntPtr ptr = SystemCore.SystemAbstraction.WindowManagement.WindowUtility.Instance.GetTopWindow(_window.ProcessName).Handle;
                    SystemCore.SystemAbstraction.WindowManagement.WindowUtility.Instance.BringWindowToFrontIfNeeded(ptr);
                }
            }
            System.Threading.Thread.Sleep(10);
        }
        #endregion
    }
}
