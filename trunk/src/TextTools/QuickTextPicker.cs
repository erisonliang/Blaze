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
using System.Windows.Forms;
using SystemCore.CommonTypes;
using ContextLib;

namespace TextTools
{
    public partial class QuickTextPicker : Form
    {
        private TextTools _parent;
        private ToolTip _tooltip;
        private bool _edit_mode = false;
        private QuickText _quick_text;

        public QuickText UserQuickText { get { return _quick_text; } }

        public QuickTextPicker(TextTools parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        public QuickTextPicker(TextTools parent, string name, string text)
        {
            InitializeComponent();
            _parent = parent;
            Text = "Editing " + name;
            Icon = Properties.Resources.insert_small;
            nameTextBox.Text = name;
            textBox.Text = text;
            _edit_mode = true;
        }

        private void EnginePicker_Load(object sender, EventArgs e)
        {
            _tooltip = new ToolTip();
            _tooltip.IsBalloon = true;
            _tooltip.Active = true;
            _tooltip.ToolTipIcon = ToolTipIcon.Error;
            _tooltip.ToolTipTitle = "Error";
            _tooltip.InitialDelay = 0;
            if (!_edit_mode)
            {
                ContextLib.DataContainers.Multimedia.MultiLevelData data = UserContext.Instance.GetSelectedContent();
                if (data.Text != null && data.Text.Trim() != string.Empty)
                {
                    textBox.Text = data.Text;
                }
                data.Dispose();
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text == string.Empty)
            {
                nameTextBox.Focus();
                _tooltip.SetToolTip(nameTextBox, "Error");
                _tooltip.Show("You must specify a name.", nameTextBox, 3000);
                return;
            }
            else if (!_edit_mode && _parent.QuickTextNames.Contains(nameTextBox.Text))
            {
                nameTextBox.Focus();
                _tooltip.SetToolTip(nameTextBox, "Error");
                _tooltip.Show("A quick text with this name alreay exists. Please specify a different one.", nameTextBox, 3000);
                return;
            }
            else if (textBox.Text == string.Empty)
            {
                textBox.Focus();
                _tooltip.SetToolTip(textBox, "Error");
                _tooltip.Show("You must specify a path.", textBox, 3000);
                return;
            }

            _quick_text = new QuickText(nameTextBox.Text, textBox.Text);

            if (!_edit_mode)
            {
                _parent.QuickTextNames.Add(nameTextBox.Text);
                _parent.QuickTexts.Add(_quick_text);

                Command new_command = new Command("Insert " + nameTextBox.Text);
                new_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
                {
                    return true;
                }));
                new_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return "Insert " + _quick_text.Name;
                }));
                new_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return "Insert quick text in active window.";
                }));
                new_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return "Insert " + _quick_text.Name;
                }));
                new_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
                {
                    return Properties.Resources.insert_small.ToBitmap();
                }));
                new_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
                {
                    List<string> args = new List<string>();
                    Dictionary<string, bool> comp = new Dictionary<string, bool>();

                    return new CommandUsage(new_command.Name, args, comp);
                }));
                new_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
                {
                    UserContext.Instance.InsertText(_quick_text.Text, true);
                }));
                _parent.Commands.Add(new_command);
            }

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
