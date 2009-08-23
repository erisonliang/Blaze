using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Gma.UserActivityMonitor
{
    public class KeyEventExArgs : KeyEventArgs
    {
        private char _key_char;

        public char KeyChar { get { return _key_char; } set { _key_char = value; } }

        public KeyEventExArgs(Keys keyData, char keyChar)
            : base (keyData)
        {
            _key_char = keyChar;
        }
    }
}
