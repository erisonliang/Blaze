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
                if (name.Length == 0)
                    name = "None";
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

        #region Public Methods
        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            HotKeyModifiers modifiers = (HotKeyModifiers)obj;
            if (modifiers == null) // check if it can be casted
                return false;

            if (modifiers.ModifiersName == this.ModifiersName)
                return true;
            else
                return false;
        }

        public bool Equals(HotKeyModifiers modifiers)
        {
            if (modifiers == null) // check if it can be casted
                return false;

            if (modifiers.ModifiersName == this.ModifiersName)
                return true;
            else
                return false;
        }

        public static bool operator ==(HotKeyModifiers a, HotKeyModifiers b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);

            if (object.ReferenceEquals(b, null))
                return object.ReferenceEquals(a, null);

            if (a.GetType() != b.GetType())
                return false;

            if (a.ModifiersName == b.ModifiersName)
                return true;
            else
                return false;
        }

        public static bool operator !=(HotKeyModifiers a, HotKeyModifiers b)
        {
            return !(a == b);
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

        #region Public Methods
        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            HotKey hotKey = (HotKey)obj;
            if (hotKey == null) // check if it can be casted
                return false;

            if (hotKey.Key == this.Key &&
                hotKey.Modifiers == this.Modifiers)
                return true;
            else
                return false;
        }

        public bool Equals(HotKey hotKey)
        {
            if (hotKey == null) // check if it can be casted
                return false;

            if (hotKey.Key == this.Key &&
                hotKey.Modifiers == this.Modifiers)
                return true;
            else
                return false;
        }

        public static bool operator ==(HotKey a, HotKey b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            if (object.ReferenceEquals(b, null))
                return object.ReferenceEquals(a, null);

            if (a.GetType() != b.GetType())
                return false;

            if (a.Key == b.Key &&
                a.Modifiers == b.Modifiers)
                return true;
            else
                return false;
        }

        public static bool operator !=(HotKey a, HotKey b)
        {
            return !(a == b);
        }
        #endregion
    }
}
