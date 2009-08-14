using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SystemCore.SystemAbstraction
{
    public class HotKeyModifiers
    {
        #region Properties
        protected bool _alt;
        protected bool _ctrl;
        protected bool _shift;
        protected bool _win;
        #endregion

        #region Accessors
        public int Modifiers
        {
            get
            {
                int mod = 0;
                if (_alt)
                    mod += (int)HotkeyModifiersFlags.ALT;
                if (_ctrl)
                    mod += (int)HotkeyModifiersFlags.CONTROL;
                if (_shift)
                    mod += (int)HotkeyModifiersFlags.SHIFT;
                if (_win)
                    mod += (int)HotkeyModifiersFlags.WIN;
                return mod;
            }
        }

        public string ModifiersName
        {
            get
            {
                string name = "";
                if (_alt)
                    name += "Alt";
                if (_ctrl)
                    name += (name.Length > 0 ? "+Ctrl" : "Ctrl");
                if (_shift)
                    name += (name.Length > 0 ? "+Shift" : "Shift");
                if (_win)
                    name += (name.Length > 0 ? "+Win" : "Win");
                return name;
            }
        }

        public bool IsAlt { get { return _alt; } }
        public bool IsCtrl { get { return _ctrl; } }
        public bool IsShift { get { return _shift; } }
        public bool IsWin { get { return _win; } }
        #endregion

        #region Constructors
        public HotKeyModifiers(bool alt, bool ctrl, bool shift, bool win)
        {
            _alt = alt;
            _ctrl = ctrl;
            _shift = shift;
            _win = win;
        }
        #endregion
    }

    public class HotKey
    {
        #region Properties
        protected HotKeyModifiers _modifiers;
        protected Keys _key;
        #endregion

        #region Accessors
        public int Modifiers
        {
            get { return _modifiers.Modifiers; }
        }

        public string ModifiersName
        {
            get { return _modifiers.ModifiersName; }
        }

        public int Key
        {
            get { return (int)_key; }
        }

        public string KeyName
        {
            get { return _key.ToString(); }
        }

        public bool IsAlt { get { return _modifiers.IsAlt; } }
        public bool IsCtrl { get { return _modifiers.IsCtrl; } }
        public bool IsShift { get { return _modifiers.IsShift; } }
        public bool IsWin { get { return _modifiers.IsWin; } }
        #endregion

        #region Constructors
        public HotKey(bool alt, bool ctrl, bool shift, bool win, Keys key)
        {
            _modifiers = new HotKeyModifiers(alt, ctrl, shift, win);
            _key = key;
        }

        public HotKey(HotKeyModifiers modifiers, Keys key)
        {
            _modifiers = modifiers;
            _key = key;
        }
        #endregion
    }
}
