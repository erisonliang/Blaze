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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ContextLib
{
    public partial class MacroNamePicker : Form
    {
        private ToolTip _tooltip;
        private string _name;
        private string _folder;

        public string MacroName { get { return _name; } set { _name = value; }}
        public string Folder { get { return _folder; } set { _folder = value; } }

        public MacroNamePicker(string folder)
        {
            InitializeComponent();
            _tooltip = new ToolTip();
            _name = string.Empty;
            _folder = folder;
        }

        private void MacroNamePicker_Load(object sender, EventArgs e)
        {
            _tooltip = new ToolTip();
            _tooltip.IsBalloon = true;
            _tooltip.Active = true;
            _tooltip.ToolTipIcon = ToolTipIcon.Error;
            _tooltip.ToolTipTitle = "Error";
            _tooltip.InitialDelay = 0;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (macroNameTextBox.Text.Trim() == string.Empty)
            {
                macroNameTextBox.Focus();
                _tooltip.SetToolTip(macroNameTextBox, "Error");
                _tooltip.Show("You must specify a name.", macroNameTextBox, 3000);
                return;
            }
            
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (macroNameTextBox.Text.Contains(c))
                {
                    macroNameTextBox.Focus();
                    _tooltip.SetToolTip(macroNameTextBox, "Error");
                    _tooltip.Show(c.ToString() + " is an invalid character for the macro name.", macroNameTextBox, 3000);
                    return;
                }
            }

            List<string> files = new List<string>(Directory.GetFiles(_folder));
            foreach (string file in files)
            {
                if (Path.GetFileName(file).ToLower() == macroNameTextBox.Text + ".py")
                {
                    if (MessageBox.Show("A macro with this name already exists. Do you want to overwrite?", "Comfirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            _name = macroNameTextBox.Text + ".py";

            DialogResult = DialogResult.OK;
            Close();
        }

        private void discardButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
