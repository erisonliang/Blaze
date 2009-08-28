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
using ContextLib.DataContainers.Monitoring;

namespace Blaze.Automation.Wizard
{
    public partial class WizardWelcome : Form
    {
        public WizardWelcome()
        {
            InitializeComponent();
        }

        public WizardWelcome(UserAction.UserActionType type)
        {
            InitializeComponent();
            TextLabel1.Text = "This wizard will guide you through the steps required to modify a ";
            switch (type)
            {
                case UserAction.UserActionType.KeyPressAction:
                    TextLabel1.Text += "key press";
                    break;
                case UserAction.UserActionType.MouseClickAction:
                    TextLabel1.Text += "mouse click";
                    break;
                case UserAction.UserActionType.MouseDoubleClickAction:
                    TextLabel1.Text += "mouse double click";
                    break;
                case UserAction.UserActionType.MouseDragAction:
                    TextLabel1.Text += "mouse drag";
                    break;
                case UserAction.UserActionType.MouseWheelSpinAction:
                    TextLabel1.Text += "mouse wheel spin";
                    break;
                case UserAction.UserActionType.TypeTextAction:
                    TextLabel1.Text += "text";
                    break;
                case UserAction.UserActionType.FileCreatedAction:
                    TextLabel1.Text += "file create";
                    break;
                case UserAction.UserActionType.FileDeletedAction:
                    TextLabel1.Text += "file delete";
                    break;
                case UserAction.UserActionType.FileMovedAction:
                    TextLabel1.Text += "file move";
                    break;
                case UserAction.UserActionType.FileRenamedAction:
                    TextLabel1.Text += "file rename";
                    break;
            }
            TextLabel1.Text += " action.";
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

        private void WizardWelcome_Load(object sender, EventArgs e)
        {

        }
    }
}
