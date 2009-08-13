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
using System.Windows.Forms;

namespace Blaze.Automation
{
    public partial class AssistantButton : Form
    {
        private MainForm _parent;

        public AssistantButton(MainForm parent)
        {
            _parent = parent;
            Owner = parent;
            InitializeComponent();

            this.MouseDown += new MouseEventHandler(AssistantButton_MouseDown);
        }

        void AssistantButton_MouseDown(object sender, MouseEventArgs e)
        {
            _parent.SetAssistantButtonFocus(true);
            _parent.TextBox.Focus();
            _parent.ShowAssistantWindow();
        }
    }
}
