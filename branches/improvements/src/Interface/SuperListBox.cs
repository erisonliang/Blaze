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
using System.Drawing;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction;
using SystemCore.SystemAbstraction.ImageHandling;

namespace Blaze
{
    public partial class SuperListBox : Form
    {
        #region Properties
        private InterpreterItem[] _items;
        private MainForm _parent;
        private ToolTip _tooltip;
        private InterpreterItem _lastItem;
        #endregion

        #region Accessors
        public InterpreterItem[] Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public ListBox ListBox
        {
            get { return listBox; }
        }
        #endregion

        #region Constructors
        public SuperListBox(MainForm parent)
        {
            InitializeComponent();
            _parent = parent;
            Owner = parent;
            listBox.DrawItem += new DrawItemEventHandler(listBox_DrawItem);
            listBox.MeasureItem += new MeasureItemEventHandler(listBox_MeasureItem);
            listBox.MouseMove += new MouseEventHandler(listBox_MouseMove);
            listBox.MouseEnter += new EventHandler(listBox_MouseEnter);
            listBox.MouseLeave += new EventHandler(listBox_MouseLeave);
            listBox.MouseDown += new MouseEventHandler(listBox_MouseDown);
            _lastItem = null;
        }
        #endregion

        #region Event Handling
        void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font font_name = new Font("verdana", 10);
            SolidBrush brush_name = new SolidBrush(Color.Black);
            Font font_description = new Font("verdana", 8);
            SolidBrush brush_description = new SolidBrush(Color.Gray);

            Rectangle icon_rectangle = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 32, 32);
            Rectangle name_rectangle = new Rectangle(e.Bounds.X + 34, e.Bounds.Y + 1,
                                            e.Bounds.Width - 34 - 5, e.Bounds.Height - 12 - 3);
            Rectangle description_rectangle = new Rectangle(e.Bounds.X + 34, e.Bounds.Y + 18,
                                            e.Bounds.Width - 34 - 5, e.Bounds.Height - 16 - 3);
            Rectangle item_rectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            
            e.DrawBackground();
            //if (_items[e.Index].Type == ItemType.Learned)
            //    e.Graphics.FillRectangle(new SolidBrush(Color.Wheat), item_rectangle);
            e.DrawFocusRectangle();
            StringFormat sfname = new StringFormat();
            StringFormat sfdescription = new StringFormat();
            sfname.Alignment = StringAlignment.Near;
            sfname.Trimming = StringTrimming.EllipsisPath;
            sfdescription.Alignment = StringAlignment.Near;
            sfdescription.Trimming = StringTrimming.EllipsisPath;
            Image icon;
            try
            {
                icon = _items[e.Index].Icon;
            }
            catch (Exception)
            {
                icon = IconManager.Instance.GetIcon(_items[e.Index].Desciption);
            }
            try
            {
                e.Graphics.DrawImage(icon, icon_rectangle);
            }
            catch (ArgumentException)
            {
                //icon.Dispose();
                //icon = null;
                //RecoverIcon(icon_rectangle, e);
                while(!RecoverIcon(icon_rectangle, e));
            }
            e.Graphics.DrawString(_items[e.Index].Name, font_name, brush_name, name_rectangle, sfname);
            e.Graphics.DrawString(_items[e.Index].Desciption, font_description, brush_description, description_rectangle, sfdescription);

            // clean up
            font_name.Dispose();
            brush_name.Dispose();
            font_description.Dispose();
            brush_description.Dispose();
            e.Graphics.Dispose();
            sfdescription.Dispose();
            sfname.Dispose();
            icon_rectangle = Rectangle.Empty;
            name_rectangle = Rectangle.Empty;
            description_rectangle = Rectangle.Empty;
            item_rectangle = Rectangle.Empty;
            //if (icon != null)
            //    icon.Dispose();
            //GC.Collect();
        }

        bool RecoverIcon(Rectangle icon_rectangle, DrawItemEventArgs e)
        { 
            try
            {
                Image icon = IconManager.Instance.GetIcon(_items[e.Index].Desciption);
                e.Graphics.DrawImage(icon, icon_rectangle);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
                //RecoverIcon(icon_rectangle, e);
            }
        }

        void listBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 34;
        }

        void listBox_MouseDown(object sender, MouseEventArgs e)
        {
            _parent.TextBox.Focus();
            _parent.Interpreter.Execute(_parent.TextBox.Text, _items[listBox.SelectedIndex], Keys.None);
            _parent.HideAutomator();
        }

        void listBox_MouseEnter(object sender, EventArgs e)
        {
            _tooltip.Active = true;
        }

        void listBox_MouseMove(object sender, MouseEventArgs e)
        {
            int index = listBox.IndexFromPoint(listBox.PointToClient(MousePosition));
            if (index != ListBox.NoMatches)
            {
                InterpreterItem item = _items[index];
                if (item != _lastItem)
                {
                    _lastItem = item;
                    _tooltip.SetToolTip(listBox, item.Desciption);
                }
            }
        }

        void listBox_MouseLeave(object sender, EventArgs e)
        {
            _tooltip.Active = false;
            _lastItem = null;
        }
        #endregion

        private void SuperListBox_Load(object sender, EventArgs e)
        {
            _tooltip = new ToolTip();
            _tooltip.AutoPopDelay = 5000;
            _tooltip.InitialDelay = 1000;
            _tooltip.ReshowDelay = 500;
            _tooltip.ShowAlways = true;
            _tooltip.Active = false;
        }

        // avoid superlistbox to show in alt tab menu
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= Win32.WS_EX_TOOLWINDOW;
                return cp;
            }
        }
    }
}