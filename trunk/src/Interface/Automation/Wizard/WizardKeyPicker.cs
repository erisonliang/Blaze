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
using System.ComponentModel;
using System.Windows.Forms;
using Gma.UserActivityMonitor;

namespace Blaze.Automation.Wizard
{
    public partial class WizardKeyPicker : Form
    {
        private Keys _key;
        private ContextLib.DataContainers.Monitoring.UserAction.Modifiers _modifiers;
        private bool _wait_for_keypress = false;

        private bool _is_alt = false;
        private bool _is_ctrl = false;
        private bool _is_shift = false;
        private bool _is_win = false;

        public Keys Key { get { return _key; } }
        public ContextLib.DataContainers.Monitoring.UserAction.Modifiers Modifiers { get { return _modifiers; } }

        public WizardKeyPicker()
        {
            InitializeComponent();
            NextButton.Enabled = false;

            foreach (string key in Enum.GetNames(typeof(Keys)))
                KeyComboBox.Items.Add(key);

            _key = Keys.None;
            _modifiers = ContextLib.DataContainers.Monitoring.UserAction.Modifiers.None;
            KeyComboBox.SelectionChangeCommitted += new EventHandler(KeyComboBox_SelectionChangeCommitted);
        }

        public WizardKeyPicker(Keys key, ContextLib.DataContainers.Monitoring.UserAction.Modifiers modifiers)
        {
            InitializeComponent();
            NextButton.Enabled = false;

            foreach (string k in Enum.GetNames(typeof(Keys)))
                KeyComboBox.Items.Add(k);

            _key = key;
            _modifiers = modifiers;

            KeyComboBox.SelectedItem = _key.ToString();
            if (IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt))
                AltModifier.Checked = true;
            if (IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl))
                CtrlModifier.Checked = true;
            if (IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift))
                ShiftModifier.Checked = true;
            if (IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Win))
                WinModifier.Checked = true;

            KeyComboBox.SelectionChangeCommitted += new EventHandler(KeyComboBox_SelectionChangeCommitted);
        }

        void KeyComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (KeyComboBox.SelectedItem != null &&
                KeyComboBox.SelectedItem.ToString() != string.Empty &&
                KeyComboBox.SelectedItem.ToString() != "None")
            {
                NextButton.Enabled = true;
            }
            else
            {
                NextButton.Enabled = false;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            string key = KeyComboBox.SelectedItem.ToString();
            if (key != string.Empty)
            {
                _key = (Keys)Enum.Parse(typeof(Keys), key);
                if (AltModifier.Checked)
                {
                    SetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt);
                }
                else
                {
                    UnsetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt);
                }
                if (CtrlModifier.Checked)
                {
                    SetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl);
                }
                else
                {
                    UnsetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl);
                }
                if (ShiftModifier.Checked)
                {
                    SetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift);
                }
                else
                {
                    UnsetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift);
                }
                if (WinModifier.Checked)
                {
                    SetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Win);
                }
                else
                {
                    UnsetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Win);
                }

                DialogResult = DialogResult.Yes;
                Close();
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (KeyComboBox.SelectedItem != null)
            {
                string key = KeyComboBox.SelectedItem.ToString();
                if (key != string.Empty)
                {
                    _key = (Keys)Enum.Parse(typeof(Keys), key);
                    if (AltModifier.Checked)
                    {
                        SetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt);
                    }
                    else
                    {
                        UnsetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt);
                    }
                    if (CtrlModifier.Checked)
                    {
                        SetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl);
                    }
                    else
                    {
                        UnsetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl);
                    }
                    if (ShiftModifier.Checked)
                    {
                        SetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift);
                    }
                    else
                    {
                        UnsetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift);
                    }
                    if (WinModifier.Checked)
                    {
                        SetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Win);
                    }
                    else
                    {
                        UnsetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Win);
                    }
                }
            }
            DialogResult = DialogResult.No;
            Close();
        }

        private void WizardKeyPicker_Load(object sender, EventArgs e)
        {
            KeyComboBox_SelectionChangeCommitted(null, null);
            HookManager.KeyDown += new KeyEventHandler(HookManager_KeyDown);
            HookManager.KeyUp += new KeyEventHandler(HookManager_KeyUp);
        }

        //
        // Modifier Methods
        //

        private bool IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers modifier)
        {
            return (_modifiers & modifier) == modifier;
        }

        private void SetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers modifier)
        {
            if (!IsModifierSet(modifier))
                _modifiers |= modifier;
        }

        private void UnsetModifier(ContextLib.DataContainers.Monitoring.UserAction.Modifiers modifier)
        {
            if (IsModifierSet(modifier))
                _modifiers &= ~modifier;
        }

        //
        //
        //

        private void CaptureKeyButton_Click(object sender, EventArgs e)
        {
            CaptureKeyButton.Text = "Waiting for a keypress...";
            _wait_for_keypress = true;
        }

        void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (_wait_for_keypress)
            {
                if (e.KeyCode == Keys.LMenu ||
                    e.KeyCode == Keys.RMenu)
                {
                    _is_alt = true;
                }
                else if (e.KeyCode == Keys.LControlKey ||
                    e.KeyCode == Keys.RControlKey)
                {
                    _is_ctrl = true;
                }
                else if (e.KeyCode == Keys.LShiftKey ||
                    e.KeyCode == Keys.RShiftKey)
                {
                    _is_shift = true;
                }
                else if (e.KeyCode == Keys.LWin ||
                    e.KeyCode == Keys.RWin)
                {
                    _is_win = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    _wait_for_keypress = false;
                    CaptureKeyButton.Text = "Pick a Key";
                }
            }
        }

        void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (_wait_for_keypress)
            {
                if (e.KeyCode == Keys.LMenu ||
                    e.KeyCode == Keys.RMenu)
                {
                    _is_alt = false;
                }
                else if (e.KeyCode == Keys.LControlKey ||
                    e.KeyCode == Keys.RControlKey)
                {
                    _is_ctrl = false;
                }
                else if (e.KeyCode == Keys.LShiftKey ||
                    e.KeyCode == Keys.RShiftKey)
                {
                    _is_shift = false;
                }
                else if (e.KeyCode == Keys.LWin ||
                    e.KeyCode == Keys.RWin)
                {
                    _is_win = false;
                }
                else
                {
                    _wait_for_keypress = false;
                    //Thread.Sleep(100);
                    KeyComboBox.SelectedItem = e.KeyCode.ToString();
                    if (_is_alt)
                        AltModifier.Checked = true;
                    else
                        AltModifier.Checked = false;
                    if (_is_ctrl)
                        CtrlModifier.Checked = true;
                    else
                        CtrlModifier.Checked = false;
                    if (_is_shift)
                        ShiftModifier.Checked = true;
                    else
                        ShiftModifier.Checked = false;
                    if (_is_win)
                        WinModifier.Checked = true;
                    else
                        WinModifier.Checked = false;
                    CaptureKeyButton.Text = "Pick a Key";
                    NextButton.Enabled = true;
                    SystemCore.SystemAbstraction.WindowManagement.WindowUtility.Instance.BringWindowToFront(this.Handle);
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            HookManager.KeyDown -= HookManager_KeyDown;
            HookManager.KeyUp -= HookManager_KeyUp;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult != DialogResult.Yes &&
                DialogResult != DialogResult.No)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to cancel the operation? If you do so, no changes will take effect.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DialogResult = DialogResult.Cancel;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

    }
}
