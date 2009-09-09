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

namespace Blaze.Automation.Wizard
{
    public partial class WizardTypePicker : Form
    {
        private ContextLib.DataContainers.Monitoring.UserAction.UserActionType _type;
        private string[] _supported_types_names;
        private Dictionary<string, ContextLib.DataContainers.Monitoring.UserAction.UserActionType> _supported_types;

        public ContextLib.DataContainers.Monitoring.UserAction.UserActionType ActionType { get { return _type;  } set { _type = value; } }

        public WizardTypePicker(ContextLib.DataContainers.Monitoring.UserAction.UserActionType type)
        {
            InitializeComponent();
            _supported_types_names = new string[] { /*"File Create", "File Delete", "File Move", "File Rename",*/ "Key Press", "Mouse Click", "Mouse Double Click", "Mouse Drag", "Mouse Wheel Spin", "Text" };
            _supported_types = new Dictionary<string, ContextLib.DataContainers.Monitoring.UserAction.UserActionType>();
            //_supported_types.Add(_supported_types_names[0], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.FileCreatedAction);
            //_supported_types.Add(_supported_types_names[1], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.FileDeletedAction);
            //_supported_types.Add(_supported_types_names[2], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.FileMovedAction);
            //_supported_types.Add(_supported_types_names[3], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.FileRenamedAction);
            _supported_types.Add(_supported_types_names[0], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.KeyPressAction);
            _supported_types.Add(_supported_types_names[1], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.MouseClickAction);
            _supported_types.Add(_supported_types_names[2], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.MouseDoubleClickAction);
            _supported_types.Add(_supported_types_names[3], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.MouseDragAction);
            _supported_types.Add(_supported_types_names[4], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.MouseWheelSpinAction);
            _supported_types.Add(_supported_types_names[5], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.TypeTextAction);
            ActionTypeComboBox.SelectionChangeCommitted += new EventHandler(ActionTypeComboBox_SelectionChangeCommitted);

            foreach (string t in _supported_types_names)
                ActionTypeComboBox.Items.Add(t);

            foreach (KeyValuePair<string, ContextLib.DataContainers.Monitoring.UserAction.UserActionType> pair in _supported_types)
            {
                if (pair.Value == _type)
                    ActionTypeComboBox.SelectedItem = pair.Key;
            }

            _type = type;
        }

        public WizardTypePicker()
        {
            InitializeComponent();
            _supported_types_names = new string[] { /*"File Create", "File Delete", "File Move", "File Rename",*/ "Key Press", "Mouse Click", "Mouse Double Click", "Mouse Drag", "Mouse Wheel Spin", "Text" };
            _supported_types = new Dictionary<string, ContextLib.DataContainers.Monitoring.UserAction.UserActionType>();
            //_supported_types.Add(_supported_types_names[0], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.FileCreatedAction);
            //_supported_types.Add(_supported_types_names[1], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.FileDeletedAction);
            //_supported_types.Add(_supported_types_names[2], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.FileMovedAction);
            //_supported_types.Add(_supported_types_names[3], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.FileRenamedAction);
            _supported_types.Add(_supported_types_names[0], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.KeyPressAction);
            _supported_types.Add(_supported_types_names[1], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.MouseClickAction);
            _supported_types.Add(_supported_types_names[2], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.MouseDoubleClickAction);
            _supported_types.Add(_supported_types_names[3], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.MouseDragAction);
            _supported_types.Add(_supported_types_names[4], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.MouseWheelSpinAction);
            _supported_types.Add(_supported_types_names[5], ContextLib.DataContainers.Monitoring.UserAction.UserActionType.TypeTextAction);
            ActionTypeComboBox.SelectionChangeCommitted += new EventHandler(ActionTypeComboBox_SelectionChangeCommitted);

            foreach (string type in _supported_types_names)
                ActionTypeComboBox.Items.Add(type);
        }

        void ActionTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ActionTypeComboBox.SelectedItem != null && ActionTypeComboBox.SelectedItem.ToString() != string.Empty)
            {
                NextButton.Enabled = true;
            }
            else
            {
                NextButton.Enabled = false;
            }
        }

        private void WizardTypePicker_Load(object sender, EventArgs e)
        {
            //NextButton.Enabled = false;
            ActionTypeComboBox_SelectionChangeCommitted(null, null);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            string type = ActionTypeComboBox.SelectedItem.ToString();
            if (type != string.Empty)
            {
                _type = _supported_types[type];
                DialogResult = DialogResult.Yes;
                Close();
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (ActionTypeComboBox.SelectedItem != null)
            {
                string type = ActionTypeComboBox.SelectedItem.ToString();
                if (type != string.Empty)
                {
                    _type = _supported_types[type];
                }
            }
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
    }
}
