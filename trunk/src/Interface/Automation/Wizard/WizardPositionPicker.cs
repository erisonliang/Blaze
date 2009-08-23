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
    public partial class WizardPositionPicker : Form
    {
        private int _x;
        private int _y;

        public int X { get { return _x; } }
        public int Y { get { return _y; } }

        private bool _wait_for_click = false;

        public WizardPositionPicker()
        {
            InitializeComponent();

            _x = 0;
            _y = 0;
        }

        public WizardPositionPicker(int x, int y)
        {
            InitializeComponent();

            _x = x;
            _y = y;

            XBox.Value = _x;
            YBox.Value = _y;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            _x = (int)XBox.Value;
            _y = (int)YBox.Value;

            DialogResult = DialogResult.Yes;
            Close();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _x = (int)XBox.Value;
            _y = (int)YBox.Value;
            DialogResult = DialogResult.No;
            Close();
        }

        private void WizardDragPicker_Load(object sender, EventArgs e)
        {
            HookManager.KeyDown += new KeyEventHandler(HookManager_KeyDown);
            HookManager.MouseUp += new MouseEventHandler(HookManager_MouseUp);
        }

        protected override void OnClosed(EventArgs e)
        {
            HookManager.KeyDown -= HookManager_KeyDown;
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
                XBox.Value = e.X;
                YBox.Value = e.Y;
                CaptureClickButton.Text = "Pick a Position";
                SystemCore.SystemAbstraction.WindowManagement.WindowUtility.Instance.BringWindowToFront(this.Handle);
            }
        }

        void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (_wait_for_click)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    _wait_for_click = false;
                    CaptureClickButton.Text = "Pick a Position";
                }
            }
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
