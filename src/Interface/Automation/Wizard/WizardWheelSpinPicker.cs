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
    public partial class WizardWheelSpinPicker : Form
    {
        private int _notches;
        private ContextLib.DataContainers.Monitoring.UserAction.Modifiers _modifiers;
        private int _x;
        private int _y;

        public int Notches { get { return _notches; } }
        public ContextLib.DataContainers.Monitoring.UserAction.Modifiers Modifiers { get { return _modifiers; } }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }

        private bool _is_alt = false;
        private bool _is_ctrl = false;
        private bool _is_shift = false;
        private bool _is_win = false;

        private bool _wait_for_click = false;

        public WizardWheelSpinPicker()
        {
            InitializeComponent();

            _notches = -1;
            _modifiers = ContextLib.DataContainers.Monitoring.UserAction.Modifiers.None;
            _x = 0;
            _y = 0;
            TowardRadioButton.Checked = true;
        }

        public WizardWheelSpinPicker(int notches, ContextLib.DataContainers.Monitoring.UserAction.Modifiers modifiers, int x, int y)
        {
            InitializeComponent();

            _notches = notches;
            _modifiers = modifiers;
            _x = x;
            _y = y;

            if (_notches >= 0)
            {
                NotchesBox.Value = _notches;
                AwayRadioButton.Checked = true;
            }
            else
            {
                NotchesBox.Value = Math.Abs(_notches);
                TowardRadioButton.Checked = true;
            }
            if (IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Alt))
                AltModifier.Checked = true;
            if (IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Ctrl))
                CtrlModifier.Checked = true;
            if (IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Shift))
                ShiftModifier.Checked = true;
            if (IsModifierSet(ContextLib.DataContainers.Monitoring.UserAction.Modifiers.Win))
                WinModifier.Checked = true;
            XBox.Value = _x;
            YBox.Value = _y;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            _notches = (TowardRadioButton.Checked ? -1 : 1) * (int)NotchesBox.Value;
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
            _x = (int)XBox.Value;
            _y = (int)YBox.Value;

            DialogResult = DialogResult.Yes;
            Close();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _notches = (TowardRadioButton.Checked ? -1 : 1) * (int)NotchesBox.Value;
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
            _x = (int)XBox.Value;
            _y = (int)YBox.Value;

            DialogResult = DialogResult.No;
            Close();
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

        private void WizardWheelSpinPicker_Load(object sender, EventArgs e)
        {
            HookManager.KeyDown += new KeyEventHandler(HookManager_KeyDown);
            HookManager.KeyUp += new KeyEventHandler(HookManager_KeyUp);
            HookManager.MouseUp += new MouseEventHandler(HookManager_MouseUp);
        }

        protected override void OnClosed(EventArgs e)
        {
            HookManager.KeyDown -= HookManager_KeyDown;
            HookManager.KeyUp -= HookManager_KeyUp;
            HookManager.MouseUp -= HookManager_MouseUp;
        }

        private void CaptureClickButton_Click(object sender, EventArgs e)
        {
            CaptureClickButton.Text = "Waiting for a mouse click...";
            _wait_for_click = true;
        }

        void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
            if (_wait_for_click)
            {
                _wait_for_click = false;
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
                XBox.Value = e.X;
                YBox.Value = e.Y;
                CaptureClickButton.Text = "Pick a Click";
                NextButton.Enabled = true;
                SystemCore.SystemAbstraction.WindowManagement.WindowUtility.Instance.BringWindowToFront(this.Handle);
            }
        }

        void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (_wait_for_click)
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
                    _wait_for_click = false;
                    CaptureClickButton.Text = "Pick a Position";
                }
            }
        }

        void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (_wait_for_click)
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
            }
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

    }
}
