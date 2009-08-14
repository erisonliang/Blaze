﻿// Blaze: Automated Desktop Experience
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
    public partial class WizardTextPicker : Form
    {
        private string _txt;

        public string TXT { get { return _txt; } }

        public WizardTextPicker()
        {
            InitializeComponent();
            NextButton.Enabled = false;

            _txt = string.Empty;
            TextBox.TextChanged += new EventHandler(TextBox_TextChanged);
        }

        public WizardTextPicker(string text, bool enabled)
        {
            InitializeComponent();
            NextButton.Enabled = false;

            TextBox.Enabled = enabled;

            _txt = text;
            TextBox.Text = _txt;
            TextBox.TextChanged += new EventHandler(TextBox_TextChanged);
        }

        void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (TextBox.Text == string.Empty)
            {
                NextButton.Enabled = false;
            }
            else
            {
                NextButton.Enabled = true;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            _txt = TextBox.Text;
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _txt = TextBox.Text;
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

        private void WizardTextPicker_Load(object sender, EventArgs e)
        {
            TextBox_TextChanged(null, null);
        }

    }
}
