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
using System.IO;
using System.Windows.Forms;
using ContextLib.DataContainers.Monitoring;

namespace Blaze.Automation.Wizard
{
    public partial class WizardFilePicker : Form
    {
        private List<string> _items;
        private Dictionary<string, bool> _active;

        public List<string> Items { get { return _items; } }
        public Dictionary<string, bool> Active { get { return _active; } }

        public WizardFilePicker(UserAction action, List<string> items, Dictionary<string, bool> active)
        {
            InitializeComponent();
            switch (action.ActionType)
            {
                case UserAction.UserActionType.FileCreatedAction:
                    {
                        FileCreatedAction faction = (FileCreatedAction)action;
                        TextLabel1.Text = "Which items should be created in folder \"" + faction.Folder + "\"?";
                    }
                    break;
                case UserAction.UserActionType.FileDeletedAction:
                    {
                        FileDeletedAction faction = (FileDeletedAction)action;
                        TextLabel1.Text = "Which items should be deleted in folder \"" + faction.Folder + "\"?";
                    }
                    break;
                case UserAction.UserActionType.FileMovedAction:
                    {
                        FileMovedAction faction = (FileMovedAction)action;
                        TextLabel1.Text = "Which items should be moved from \"" + faction.Folder + "\" to \"" + Path.GetDirectoryName(faction.FileName) + "\"?";
                    }
                    break;
                case UserAction.UserActionType.FileRenamedAction:
                    {
                        FileRenamedAction faction = (FileRenamedAction)action;
                        TextLabel1.Text = "Which items should be renamed in folder \"" + faction.Folder + "\"?";
                    }
                    break;
            }

            _items = items;
            _active = active;

            foreach (string item in _items)
            {
                ItemsListBox.Items.Add(item, _active[item]);
            }

            ItemsListBox.ItemCheck += new ItemCheckEventHandler(ItemsListBox_ItemCheck);
        }

        void ItemsListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue != CheckState.Indeterminate)
            {
                string entry = _items[e.Index];
                _active[entry] = (e.NewValue == CheckState.Checked ? true : false);
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
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

        private void WizardFilePicker_Load(object sender, EventArgs e)
        {

        }

    }
}
