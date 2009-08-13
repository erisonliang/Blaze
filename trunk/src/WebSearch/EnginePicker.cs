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

namespace WebSearch
{
    public partial class EnginePicker : Form
    {
        private Web _parent;
        private ToolTip _tooltip;

        public EnginePicker(Web parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void EnginePicker_Load(object sender, EventArgs e)
        {
            _tooltip = new ToolTip();
            _tooltip.IsBalloon = true;
            //_tooltip.ShowAlways = true;
            _tooltip.Active = true;
            _tooltip.ToolTipIcon = ToolTipIcon.Error;
            _tooltip.ToolTipTitle = "Error";
            //_tooltip.AutoPopDelay = 3000;
            _tooltip.InitialDelay = 0;
            //_tooltip.ReshowDelay = 500;
            ContextLib.DataContainers.Multimedia.MultiLevelData data = UserContext.Instance.GetSelectedContent();
            if (data.Text != null && data.Text.Trim() != string.Empty && _parent.UrlRegex.IsMatch(data.Text.Trim()))
                siteUrlTextBox.Text = data.Text;
            else
            {
                siteNameTextBox.Text = UserContext.Instance.GetBrowserPageName();
                siteUrlTextBox.Text = UserContext.Instance.GetBrowserUrl();
            }
            data.Dispose();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (siteNameTextBox.Text == string.Empty)
            {
                siteNameTextBox.Focus();
                _tooltip.SetToolTip(siteNameTextBox, "Error");
                _tooltip.Show("You must specify a name.", siteNameTextBox, 3000);
                //_tooltip.RemoveAll();
                return;
            }
            else if (_parent.SearchEngineNames.Contains(siteNameTextBox.Text))
            {
                siteNameTextBox.Focus();
                _tooltip.SetToolTip(siteNameTextBox, "Error");
                _tooltip.Show("A website with this name alreay exists. Please specify a different one.", siteNameTextBox, 3000);
                //_tooltip.RemoveAll();
                return;
            }
            else if (siteUrlTextBox.Text == string.Empty)
            {
                siteUrlTextBox.Focus();
                _tooltip.SetToolTip(siteUrlTextBox, "Error");
                _tooltip.Show("You must specify a URL.", siteUrlTextBox, 3000);
                //_tooltip.RemoveAll();
                return;
            }
            else if (!_parent.UrlRegex.IsMatch(siteUrlTextBox.Text))
            {
                siteUrlTextBox.Focus();
                _tooltip.SetToolTip(siteUrlTextBox, "Error");
                _tooltip.Show("This is not a valid URL.", siteUrlTextBox, 3000);
                //_tooltip.RemoveAll();
                return;
            }
            else if (siteQueyTextBox.Text != string.Empty)
            {
                if (!_parent.UrlRegex.IsMatch(siteQueyTextBox.Text))
                {
                    siteQueyTextBox.Focus();
                    _tooltip.SetToolTip(siteQueyTextBox, "Error");
                    _tooltip.Show("This is not a valid search query.", siteQueyTextBox, 3000);
                    //_tooltip.RemoveAll();
                    return;
                }
                else if (!siteQueyTextBox.Text.Contains(SearchEngine.SearchTermToken))
                {
                    siteQueyTextBox.Focus();
                    _tooltip.SetToolTip(siteQueyTextBox, "Error");
                    _tooltip.Show("The search query must have a search term ('" + SearchEngine.SearchTermToken + "').", siteQueyTextBox, 3000);
                    //_tooltip.RemoveAll();
                    return;
                }
            }
            _parent.SearchEngineNames.Add(siteNameTextBox.Text);
            SearchEngine new_engine = new SearchEngine(siteNameTextBox.Text, siteUrlTextBox.Text, siteQueyTextBox.Text);
            _parent.SearchEngines.Add(new_engine);

            Command new_command = new Command(siteNameTextBox.Text);
            new_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return true;
            }));
            new_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                if (parameters.Trim() == string.Empty)
                    return new_engine.Name;
                else
                    return new_engine.Name + " " + parameters;
            }));
            new_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                if (parameters == string.Empty || new_engine.SearchQuery == string.Empty) // no search term or no search query was specified
                {
                    return "Go to " + new_engine.Name + " web page.";
                }
                else
                {
                    return "Search for " + parameters + " on " + new_engine.Name;
                }
            }));
            new_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                if (parameters == string.Empty)
                {
                    return new_engine.Name;
                }
                else
                {
                    return new_engine.Name + " " + parameters;
                }
            }));
            new_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.search.ToBitmap();
            }));
            new_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                args.Add("term");
                if (parameters != string.Empty)
                    comp.Add("term", true);
                else
                    comp.Add("term", false);

                return new CommandUsage(new_command.Name, args, comp);
            }));
            new_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
            {
                try
                {
                    if (parameters == string.Empty) // no search term
                    {
                        System.Diagnostics.Process.Start(new_engine.Url);
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(new_engine.GetSearchQuery(parameters));
                    }
                }
                catch
                {

                }
            }));

            _parent.Commands.Add(new_command);
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
