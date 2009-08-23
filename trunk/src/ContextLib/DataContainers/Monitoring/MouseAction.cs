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
using System.Windows.Forms;
using SystemCore.SystemAbstraction;
using ContextLib.DataContainers.GUI;

namespace ContextLib.DataContainers.Monitoring
{
    public abstract class MouseAction : UserAction
    {
        #region Properties
        protected ContextLib.DataContainers.Monitoring.UserAction.Modifiers _modifiers;
        public static int MAX_X_OFFSET = 8;
        public static int MAX_Y_OFFSET = 8;
        public static int MAX_CLICK_DELAY = 500;
        public static int MAX_DOUBLE_CLICK_DELAY = 500;
        #endregion

        #region Accessors
        public ContextLib.DataContainers.Monitoring.UserAction.Modifiers Modifiers { get { return _modifiers; } set { _modifiers = value; } }
        protected string ModifiersIpySnippet { get { return "System.Enum.ToObject(ContextLib.DataContainers.Monitoring.UserAction.Modifiers, " + ((int)_modifiers).ToString() + ")"; } }
        #endregion

        #region Constructors
        public MouseAction(ContextLib.DataContainers.Monitoring.UserAction.Modifiers modifiers)
            : base()
        {
            _modifiers = modifiers;
        }

        public MouseAction(Window window, ContextLib.DataContainers.Monitoring.UserAction.Modifiers modifiers)
            : base(window)
        {
            _modifiers = modifiers;
        }
        #endregion

        #region Public Methods
        public bool IsModifierSet(UserAction.Modifiers modifier)
        {
            return (_modifiers & modifier) == modifier;
        }

        public void ToggleModifier(UserAction.Modifiers modifier)
        {
            if (!IsModifierSet(modifier))
                _modifiers |= modifier;
            else
                _modifiers &= ~modifier;
        }

        public void SetModifier(UserAction.Modifiers modifier)
        {
            if (!IsModifierSet(modifier))
                _modifiers |= modifier;
        }

        public void UnsetModifier(UserAction.Modifiers modifier)
        {
            if (IsModifierSet(modifier))
                _modifiers &= ~modifier;
        }
        #endregion

        #region Protected Methods
        protected void PressModifierKeys()
        {
            if (IsModifierSet(Modifiers.Alt))
            {
                PressKey(Keys.LMenu);
                System.Threading.Thread.Sleep(15);
            }
            if (IsModifierSet(Modifiers.Ctrl))
            {
                PressKey(Keys.LControlKey);
                System.Threading.Thread.Sleep(15);
            }
            if (IsModifierSet(Modifiers.Shift))
            {
                PressKey(Keys.LShiftKey);
                System.Threading.Thread.Sleep(15);
            }
            if (IsModifierSet(Modifiers.Win))
            {
                PressKey(Keys.LWin);
                System.Threading.Thread.Sleep(15);
            }
        }

        protected void ReleaseModifierKeys()
        {
            if (IsModifierSet(Modifiers.Alt))
            {
                ReleaseKey(Keys.LMenu);
                System.Threading.Thread.Sleep(15);
            }
            if (IsModifierSet(Modifiers.Ctrl))
            {
                ReleaseKey(Keys.LControlKey);
                System.Threading.Thread.Sleep(15);
            }
            if (IsModifierSet(Modifiers.Shift))
            {
                ReleaseKey(Keys.LShiftKey);
                System.Threading.Thread.Sleep(15);
            }
            if (IsModifierSet(Modifiers.Win))
            {
                ReleaseKey(Keys.LWin);
                System.Threading.Thread.Sleep(15);
            }
        }
        #endregion

        #region Private Methods
        private void PressKey(Keys key)
        {
            Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | 0, 0);
        }

        private void ReleaseKey(Keys key)
        {
            Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
        }
        #endregion
    }
}
