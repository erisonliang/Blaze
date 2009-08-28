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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prompt
{
    public partial class ConfigDialog : Form
    {
        private Prompt _parent;
        private List<PromptCommand> _pcommands;
        private BindingList<PromptCommand> _dataSource;

        public ConfigDialog(Prompt parent)
        {
            InitializeComponent();
            _parent = parent;
            dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView1_RowPostPaint);
            _pcommands = new List<PromptCommand>();
            foreach (PromptCommand command in _parent.PromptCommands)
                _pcommands.Add(new PromptCommand(command));
            _dataSource = new BindingList<PromptCommand>(_pcommands);
            Text = _parent.Name + " settings";
            descriptionLabel.Text = _parent.Name + " allows you run Command Prompt commands and custom commands.";

            //dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (Environment.OSVersion.Version.Major == 5)
                dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            else
                dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
        }

        // draw line numbers
        void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex+1).ToString(System.Globalization.CultureInfo.CurrentUICulture),
                                    dataGridView1.RowHeadersDefaultCellStyle.Font, brush,
                                    e.RowBounds.X + 20, e.RowBounds.Y + 4);
        }

        // load data to table
        private void ConfigDialog_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _dataSource;
            //dataGridView1.Columns.RemoveAt(1);
            dataGridView1.Columns[0].HeaderText = "Name";
            dataGridView1.Columns[0].Width = 75;
            dataGridView1.Columns[1].HeaderText = "Path";
            dataGridView1.Columns[1].Width = 277;
            dataGridView1.Columns[2].HeaderText = "Arguments (optional)";
            dataGridView1.Columns[2].Width = 150;
            UpdateColSize();
        }

        // remove the selected command
        private void removeButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    dataGridView1.Rows.RemoveAt(row.Index);
            }
            UpdateColSize();
        }

        // add a new empty row to the table
        private void addButton_Click(object sender, EventArgs e)
        {
            _dataSource.Add(new PromptCommand("","",""));
            UpdateColSize();
        }

        // submit changes
        private void okButton_Click(object sender, EventArgs e)
        {
            //List<string> names = new List<string>();
            HashSet<string> names = new HashSet<string>();
            foreach (PromptCommand pcommand in _pcommands)
            {
                if (pcommand.Name == string.Empty)
                {
                    MessageBox.Show("There is a command with no name specified. Please, specify a name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (pcommand.Path == string.Empty)
                {
                    MessageBox.Show("The command " + pcommand.Name + " has no path specified. Please, specify a path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!File.Exists(pcommand.Path))
                {
                    MessageBox.Show(pcommand.Name + "'s path does not exist. Please, specify a valid path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //else
                //{
                    
                //    FileInfo finfo = new FileInfo(pcommand.Path);
                //    if (finfo.Extension.ToLower() != ".exe")
                //    {
                //        MessageBox.Show(pcommand.Name + " command doesn't have a valid .exe file associated. Please, specify one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //}
                if (!names.Add(pcommand.Name))
                {
                    MessageBox.Show(pcommand.Name + " command already exists. Please pick a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                //else if (pcommand.Arguments != string.Empty)
                //{
                //    if (!pcommand.Arguments.Contains(PromptCommand.ArgumentsToken))
                //    {
                //        MessageBox.Show(pcommand.Name + "'s arguments don't have a user input location specified ('" + PromptCommand.ArgumentsToken + "'). Please, specify a user input location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //}
                
            }

            _parent.PromptCommandNames = new List<string>(names);
            _parent.PromptCommands = _pcommands;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void UpdateColSize()
        {
            if (dataGridView1.Rows.Count <= 7)
                dataGridView1.Columns[2].Width = 150;
            else
                dataGridView1.Columns[2].Width = 133;
        }
    }
}
