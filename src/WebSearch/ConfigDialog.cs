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
using System.Windows.Forms;
using System.IO;

namespace WebSearch
{
    public partial class ConfigDialog : Form
    {
        private Web _parent;
        private List<SearchEngine> _engines;
        private int _favorite;
        private BindingList<SearchEngine> _dataSource;

        public ConfigDialog(Web parent)
        {
            InitializeComponent();
            _parent = parent;
            dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView1_RowPostPaint);
            _engines = new List<SearchEngine>();
            foreach (SearchEngine engine in _parent.SearchEngines)
                _engines.Add(new SearchEngine(engine));
            _favorite = _parent.FavoriteEngine;
            _dataSource = new BindingList<SearchEngine>(_engines);
            Text = _parent.Name + " settings";
            descriptionLabel.Text = _parent.Name + " searches your favorite websites for whatever you want.";
            iconCacheTTL.Value = _parent.IconCache.GetTTLhours();

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

            DefaultLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(DefaultLinkLabel_LinkClicked);
        }

        // draw line numbers
        void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex+1).ToString(System.Globalization.CultureInfo.CurrentUICulture),
                                    dataGridView1.RowHeadersDefaultCellStyle.Font, brush,
                                    e.RowBounds.X + 20, e.RowBounds.Y + 4);
        }

        // process link clicking to open the search engine's website
        void DefaultLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(_engines[_favorite].Url);
        }

        // load data to table
        private void ConfigDialog_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _dataSource;
            dataGridView1.Columns[0].HeaderText = "Name";
            dataGridView1.Columns[0].Width = 75;
            dataGridView1.Columns[1].HeaderText = "URL";
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[2].HeaderText = "Search Query (optional)";
            dataGridView1.Columns[2].Width = 277;
            dataGridView1.Columns[3].HeaderText = "Icon URL (optional, .jpg and .png)";
            dataGridView1.Columns[3].Width = 250;
            //dataGridView1.Columns.RemoveAt(4);
            DefaultLinkLabel.Text = _engines[_favorite].Name;
            UpdateColSize();
        }

        // make the selected engine default
        private void defaultButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                _favorite = dataGridView1.SelectedCells[dataGridView1.SelectedCells.Count - 1].RowIndex;
                DefaultLinkLabel.Text = _engines[_favorite].Name;
            }
        }

        // remove the selected engine
        private void removeButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                //int rm = dataGridView1.SelectedCells[dataGridView1.SelectedCells.Count - 1].RowIndex;
                //int count = dataGridView1.SelectedCells.Count;
                //for (int i = rm - count - 1; i <= rm; i++)
                //    dataGridView1.Rows.RemoveAt(i);
                //dataGridView1.Rows.RemoveAt(rm);
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    dataGridView1.Rows.RemoveAt(row.Index);
            }
            UpdateColSize();
        }

        // add a new empty row to the table
        private void addButton_Click(object sender, EventArgs e)
        {
            _dataSource.Add(new SearchEngine("", "", "", ""));
            UpdateColSize();
        }

        // submit changes
        private void okButton_Click(object sender, EventArgs e)
        {
            //List<string> names = new List<string>();
            HashSet<string> names = new HashSet<string>();
            foreach (SearchEngine engine in _engines)
            {
                if (engine.Name == string.Empty)
                {
                    MessageBox.Show("There is a website with no name specified. Please, specify a name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (engine.Url == string.Empty)
                {
                    MessageBox.Show("The website " + engine.Name + " has no URL specified. Please, specify a URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!_parent.UrlRegex.IsMatch(engine.Url))
                {
                    MessageBox.Show("The website " + engine.Name + " doesn't have a valid URL. Please, specify a valid URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (engine.SearchQuery != string.Empty)
                {
                    if (!_parent.UrlRegex.IsMatch(engine.SearchQuery))
                    {
                        MessageBox.Show("The website " + engine.Name + " doesn't have a valid search query. Please, specify a valid search query.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (!engine.SearchQuery.Contains(SearchEngine.SearchTermToken))
                    {
                        MessageBox.Show(engine.Name + "'s search query doesn't have a search term ('" + SearchEngine.SearchTermToken + "'). Please, specify a search term.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (!names.Add(engine.Name))
                {
                    MessageBox.Show("The website " + engine.Name + "already exists. Please pick a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            _parent.SearchEngines = _engines;
            _parent.FavoriteEngine = _favorite;
            _parent.CacheTTL = (int)iconCacheTTL.Value;
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
                dataGridView1.Columns[2].Width = 277;
            else
                dataGridView1.Columns[2].Width = 260;
        }

        private void deleteIconCacheButton_Click(object sender, EventArgs e)
        {
            _parent.IconCache.Clear();
            if (Directory.Exists(_parent.IconCache.CachePath))
            {
                Array.ForEach(Directory.GetFiles(_parent.IconCache.CachePath),
                                delegate(string path) { try { File.Delete(path); } catch { } });
            }
        }
    }
}
