using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Gma.UserActivityMonitor
{
    public class KeyPressExEventArgs : KeyPressEventArgs
    {
        private Keys _key;

        public Keys KeyCode { get { return _key; } }

        public KeyPressExEventArgs(char KeyChar, Keys KeyCode)
            : base (KeyChar)
        {
            _key = KeyCode;
        }
    }
}
