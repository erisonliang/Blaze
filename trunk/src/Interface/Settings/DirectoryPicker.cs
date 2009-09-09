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
using System.IO;
using System.Windows.Forms;

namespace Blaze.Settings
{
    public partial class DirectoryPicker : Form
    {
        private ToolTip _tooltip;

        public DirectoryPicker()
        {
            InitializeComponent();
        }

        private void DirectoryPicker_Load(object sender, EventArgs e)
        {
            _tooltip = new ToolTip();
            _tooltip.IsBalloon = false;
            _tooltip.ShowAlways = true;
            _tooltip.Active = true;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (directoryInput.Text.Trim() == "" || !Directory.Exists(directoryInput.Text))
            {
                directoryInput.Focus();
                _tooltip.Show("Invalid directory.", directoryInput, 3000);
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}