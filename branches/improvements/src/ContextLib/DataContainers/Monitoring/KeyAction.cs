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
using System.Text;
using System.Windows.Forms;
using SystemCore.SystemAbstraction;
using ContextLib.DataContainers.GUI;

namespace ContextLib.DataContainers.Monitoring
{
    public abstract class KeyAction : UserAction
    {
        #region Properties
        protected Keys _key;
        protected ContextLib.DataContainers.Monitoring.UserAction.Modifiers _modifiers;
        #endregion

        #region Accessors
        public Keys Key { get { return _key; } set { _key = value; } }
        public ContextLib.DataContainers.Monitoring.UserAction.Modifiers Modifiers { get { return _modifiers; } set { _modifiers = value; } }
        protected string ModifiersIpySnippet { get { return "System.Enum.ToObject(ContextLib.DataContainers.Monitoring.UserAction.Modifiers, " + ((int)_modifiers).ToString() + ")"; } }
        #endregion

        #region Constructors
        public KeyAction(Keys key, ContextLib.DataContainers.Monitoring.UserAction.Modifiers modifiers)
            : base()
        {
            _key = key;
            _modifiers = modifiers;
        }

        public KeyAction(Window window, Keys key, ContextLib.DataContainers.Monitoring.UserAction.Modifiers modifiers)
            : base(window)
        {
            _key = key;
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

        //protected string GetChar()
        //{
        //    uint key_val = (uint)_key;
        //    int char_code = (int)Win32.MapVirtualKey(key_val, 2);
        //    if (char_code <= 0)
        //        return string.Empty;
        //    char mapped_char = Convert.ToChar(char_code);
        //    if (mapped_char == '\r' || mapped_char == '\b')
        //            return string.Empty;
        //    string ret_val = mapped_char.ToString();
        //    if (Char.IsLetter(mapped_char))
        //    {
        //        if (!IsModifierSet(Modifiers.Shift) && !IsModifierSet(Modifiers.CapsLock))
        //            ret_val = ret_val.ToLower();
        //    }
        //    return ret_val;
        //    //StringBuilder str_bld = new StringBuilder();
        //    //uint key_val = (uint)_key;
        //    //uint scan_code = Win32.MapVirtualKey(key_val, 0);
        //    //IntPtr HKL = Win32.GetKeyboardLayout(0);
        //    //byte[] key_state = new byte[256];
        //    ////Win32.ClearKeyboardBuffer(key_val, scan_code, HKL);
        //    //bool key_state_status = Win32.GetKeyboardState(key_state);
        //    //FixKeyboardState(key_state);
        //    //int lret = Win32.ToUnicodeEx(key_val, scan_code, key_state, str_bld, 32, 0, HKL);
        //    ////str_bld.Append("a");
        //    ///*
        //    // * If lret == -1, the specified virtual key os a dead-key.
        //    // * If lret == 0, the specified virtual key has no translation.
        //    // * If lret == 1, one character was written to the buffer.
        //    // * If lret == 2, two or more characters were writen to the buffer. Generaly caused by dead characters.
        //    // */
        //    //if (lret == 1)
        //    //{
        //    //    string str_c = str_bld.ToString();
        //    //    if (str_c == "\r" || str_c == "\b")
        //    //        return string.Empty;
        //    //    else
        //    //        return str_c;
        //    //}
        //    //else if (lret == -1)
        //    //{
        //    //    ClearKeyboardState();
        //    //    return string.Empty;
        //    //}
        //    //else
        //    //{
        //    //    return string.Empty;
        //    //}
        //}
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

        private void FixKeyboardState(byte[] key_state)
        {
            // fix modifiers
            if (IsModifierSet(Modifiers.Alt))
            {
                if ((key_state[18] & 128) == 0)
                    key_state[18] = 128;
            }
            if (IsModifierSet(Modifiers.Ctrl))
            {
                if ((key_state[17] & 128) == 0)
                    key_state[17] = 128;
            }
            if (IsModifierSet(Modifiers.Shift))
            {
                if ((key_state[16] & 128) == 0)
                    key_state[16] = 128;
            }
            //if (IsModifierSet(Modifiers.Win))
            //{
            //    if ((key_state[91] & 128) == 0)
            //        key_state[91] = 128;
            //}
        }

        private void ClearKeyboardState()
        {
            uint key_val = (uint)Keys.Space;
            uint scan_code = Win32.MapVirtualKey(key_val, 0);
            byte[] key_state = new byte[256];
            StringBuilder str_bld = new StringBuilder(2);
            Win32.ToUnicodeEx(key_val, scan_code, key_state, str_bld, 2, 0, IntPtr.Zero);
        }
        #endregion
    }
}
