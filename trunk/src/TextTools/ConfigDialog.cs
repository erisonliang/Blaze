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
using System.Windows.Forms;

namespace TextTools
{
    public partial class ConfigDialog : Form
    {
        private TextTools _parent;
        private List<QuickText> _quick_texts;
        private BindingList<QuickText> _dataSource;

        public ConfigDialog(TextTools parent)
        {
            InitializeComponent();
            _parent = parent;
            _quick_texts = new List<QuickText>();
            foreach (QuickText qtext in _parent.QuickTexts)
                _quick_texts.Add(new QuickText(qtext));
            _dataSource = new BindingList<QuickText>(_quick_texts);
            Text = _parent.Name + " settings";
            descriptionLabel.Text = _parent.Name + " allows you create quick texts and insert these in any window.";
        }

        // load data to table
        private void ConfigDialog_Load(object sender, EventArgs e)
        {
            listBox.DataSource = _dataSource;
            listBox.DisplayMember = "Name";
        }

        // remove the selected quick texts
        private void removeButton_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndices.Count > 0)
            {
                foreach (int index in listBox.SelectedIndices)
                    _dataSource.RemoveAt(index);
            }
        }

        // add a new quick text
        private void addButton_Click(object sender, EventArgs e)
        {
            QuickTextPicker picker = new QuickTextPicker(_parent, "", "");
            if (picker.ShowDialog() == DialogResult.OK)
            {
                _dataSource.Add(picker.UserQuickText);
            }
            picker.Dispose();
        }

        // edit selected quick text
        private void editButton_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndices.Count > 0)
            {
                int index = listBox.SelectedIndices[0];
                QuickTextPicker picker = new QuickTextPicker(_parent, _quick_texts[index].Name, _quick_texts[index].Text);
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    _dataSource[index] = picker.UserQuickText;
                }
                picker.Dispose();
                listBox.Update();
            }
        }

        // submit changes
        private void okButton_Click(object sender, EventArgs e)
        {
            //List<string> names = new List<string>();
            HashSet<string> names = new HashSet<string>();
            foreach (QuickText qtext in _quick_texts)
            {
                if (!names.Add(qtext.Name))
                {
                    MessageBox.Show(qtext.Name + " already exists. Please pick a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            _parent.QuickTextNames = new List<string>(names);
            _parent.QuickTexts = _quick_texts;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}
