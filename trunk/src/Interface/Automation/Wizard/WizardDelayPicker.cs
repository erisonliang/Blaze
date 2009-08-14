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

namespace Blaze.Automation.Wizard
{
    public partial class WizardDelayPicker : Form
    {
        private int _delay;

        public int Delay { get { return _delay; } }

        public WizardDelayPicker()
        {
            InitializeComponent();
            //NextButton.Enabled = false;
            _delay = 250;
            UseDelay.Checked = true;
            DelayAmount.Enabled = true;
            UseDelay.CheckedChanged += new EventHandler(UseDelay_CheckedChanged);
        }

        public WizardDelayPicker(int delay)
        {
            InitializeComponent();
            //NextButton.Enabled = false;
            _delay = delay;
            if (_delay > 0)
            {
                UseDelay.Checked = true;
                DelayAmount.Enabled = true;
                DelayAmount.Value = _delay;
            }
            else
            {
                DontUseDelay.Checked = true;
                DelayAmount.Enabled = false;
            }
            UseDelay.CheckedChanged += new EventHandler(UseDelay_CheckedChanged);
        }

        void UseDelay_CheckedChanged(object sender, EventArgs e)
        {
            if (UseDelay.Checked)
                DelayAmount.Enabled = true;
            else
                DelayAmount.Enabled = false;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (UseDelay.Checked)
                _delay = (int)DelayAmount.Value;
            else
                _delay = 0;
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (UseDelay.Checked)
                _delay = (int)DelayAmount.Value;
            else
                _delay = 0;
            DialogResult = DialogResult.No;
            Close();
        }

        private void WizardDelayPicker_Load(object sender, EventArgs e)
        {
            UseDelay_CheckedChanged(null, null);
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
