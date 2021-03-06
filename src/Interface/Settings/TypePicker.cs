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
using System.Windows.Forms;

namespace Blaze.Settings
{
    public partial class TypePicker : Form
    {
        private ToolTip _tooltip;

        public TypePicker()
        {
            InitializeComponent();
        }

        private void TypePicker_Load(object sender, EventArgs e)
        {
            _tooltip = new ToolTip();
            _tooltip.IsBalloon = false;
            _tooltip.ShowAlways = true;
            _tooltip.Active = true;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (typeInput.Text.Trim() == "")
            {
                typeInput.Focus();
                _tooltip.Show("File type can't be empty.", typeInput, 3000);
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}