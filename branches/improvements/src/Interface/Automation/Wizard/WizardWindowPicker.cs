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
using ContextLib.DataContainers.GUI;
using Gma.UserActivityMonitor;

namespace Blaze.Automation.Wizard
{
    public partial class WizardWindowPicker : Form
    {
        private Window _window = null;
        private bool _wait_for_click = false;

        public Window Window { get { return _window; } set { _window = value; } }

        public WizardWindowPicker()
        {
            InitializeComponent();
            PickWindowLabel.Text = "Tip: 1 After pressing this buttom, alt-tab to" + Environment.NewLine + " the desired window and click on it.";
            ProcessNameTextBox.TextChanged += new EventHandler(ProcessTextBox_TextChanged);
        }

        public WizardWindowPicker(Window window)
        {
            InitializeComponent();
            if (window != null)
            {
                _window = window;
                TitleTextBox.Text = _window.Title;
                ProcessNameTextBox.Text = _window.ProcessName;
            }
            PickWindowLabel.Text = "Tip 1: After pressing this buttom, alt-tab to" + Environment.NewLine + " the desired window and click on it.";
            ProcessNameTextBox.TextChanged += new EventHandler(ProcessTextBox_TextChanged);
        }

        void ProcessTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ProcessNameTextBox.Text.Trim() != string.Empty)
                NextButton.Enabled = true;
            else
                NextButton.Enabled = false;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            
            if (ProcessNameTextBox.Text.Trim() != string.Empty)
            {
                if (_window == null)
                {
                    _window = new Window(0, string.Empty, 0, 0, ProcessNameTextBox.Text, TitleTextBox.Text, 0, 0, 0, 0);
                }
                else
                {
                    _window.ProcessName = ProcessNameTextBox.Text;
                    _window.Title = TitleTextBox.Text;
                }
                DialogResult = DialogResult.Yes;
                Close();
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (_window != null)
            {
                _window.ProcessName = ProcessNameTextBox.Text;
                _window.Title = TitleTextBox.Text;
            }
            DialogResult = DialogResult.No;
            Close();
        }

        private void PickWindowButton_Click(object sender, EventArgs e)
        {
            PickWindowButton.Text = "Waiting for a mouse click...";
            _wait_for_click = true;
        }

        void HookManager_MouseClick(object sender, MouseEventArgs e)
        {
            if (_wait_for_click)
            {
                _wait_for_click = false;
                //Thread.Sleep(100);
                _window = new Window(SystemCore.SystemAbstraction.Win32.GetForegroundWindow());
                SystemCore.SystemAbstraction.WindowManagement.WindowUtility.Instance.BringWindowToFront(this.Handle);
                TitleTextBox.Text = _window.Title;
                ProcessNameTextBox.Text = _window.ProcessName;
                PickWindowButton.Text = "Pick a Window";
            }
        }

        void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (_wait_for_click)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    _wait_for_click = false;
                    PickWindowButton.Text = "Pick a Window";
                }
            }
        }

        private void WizardWindowPicker_Load(object sender, EventArgs e)
        {
            if (_window == null)
            {
                NextButton.Enabled = false;
            }
            else
            {
                ProcessTextBox_TextChanged(null, null);
                //TitleTextBox.Text = _window.Title;
                //ProcessNameTextBox.Text = _window.ProcessName;
            }
            HookManager.KeyDown += new KeyEventHandler(HookManager_KeyDown);
            HookManager.MouseUp += new MouseEventHandler(HookManager_MouseClick);
        }


        protected override void OnClosed(EventArgs e)
        {
            HookManager.KeyDown -= HookManager_KeyDown;
            HookManager.MouseUp -= HookManager_MouseClick;
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
