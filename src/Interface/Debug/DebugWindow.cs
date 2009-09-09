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

namespace Blaze.Debug
{
    public partial class DebugWindow : Form
    {
        private MainForm _parent;
        //private BindingList<ContextLib.DataContainers.Monitoring.UserAction> _dataSource;
        System.Windows.Forms.Timer _timer;

        public DebugWindow(MainForm parent)
        {
            _parent = parent;
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 250;
            _timer.Tick += new EventHandler(_timer_Tick);

            InitializeComponent();

            ActionsListBox.SelectedIndexChanged += new EventHandler(ActionsListBox_SelectedIndexChanged);
            GeneralizationsListBox.SelectedIndexChanged += new EventHandler(GeneralizationsListBox_SelectedIndexChanged);
            RepetitionsListBox.SelectedIndexChanged += new EventHandler(RepetitionsListBox_SelectedIndexChanged);
            //_dataSource = new BindingList<ContextLib.DataContainers.Monitoring.UserAction>(ContextLib.UserContext.Instance.ObserverObject.ActionWindow.UserActions);
        }

        // handle mouse clicking so item's info can be copied to the clipboard
        void ActionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string item = ActionsListBox.SelectedItem.ToString();
                Clipboard.SetText(item);
                ToClipboardIndicator1.Text = true.ToString();
            }
            catch
            {
                ToClipboardIndicator1.Text = false.ToString();
            }
        }

        void GeneralizationsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string item = GeneralizationsListBox.SelectedItem.ToString();
                Clipboard.SetText(item);
                ToClipboardIndicator2.Text = true.ToString();
            }
            catch
            {
                ToClipboardIndicator2.Text = false.ToString();
            }
        }

        void RepetitionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string item = RepetitionsListBox.SelectedItem.ToString();
                Clipboard.SetText(item);
                ToClipboardIndicator3.Text = true.ToString();
            }
            catch
            {
                ToClipboardIndicator3.Text = false.ToString();
            }
        }

        // update table
        void _timer_Tick(object sender, EventArgs e)
        {
            UpdateDataSource();
            MonitoringIndicator.Text = ContextLib.UserContext.Instance.ObserverObject.IsMonitoring.ToString();
            RecordingIndicator.Text = ContextLib.UserContext.Instance.ObserverObject.IsRecording.ToString();
            WorkingPathIndicator.Text = ContextLib.UserContext.Instance.ObserverObject.WorkingPath;
            NumActionsIndicator.Text = ActionsListBox.Items.Count.ToString();
            CompressionIndicator.Text = ContextLib.UserContext.Instance.ObserverObject.ActionWindow.UseCompression.ToString();
        }

        // on close, untick debug window option from context menus
        protected override void OnClosed(EventArgs e)
        {
            _parent.UncheckDebugWindow();
            base.OnClosed(e);
        }

        // on load, start timer and update data sources
        private void DebugWindow_Load(object sender, EventArgs e)
        {
            //ActionsListBox.DataSource = _dataSource;
            UpdateDataSource();
            _timer.Start();
        }

        private void UpdateDataSource()
        {
            //_dataSource = new BindingList<ContextLib.DataContainers.Monitoring.UserAction>(ContextLib.UserContext.Instance.ObserverObject.ActionWindow.UserActions);
            //ActionsListBox.DataSource = _dataSource;
            //ActionsListBox.DisplayMember = "Description";

            // update actions
            ActionsListBox.Items.Clear();
            try
            {
                foreach (ContextLib.DataContainers.Monitoring.UserAction action in ContextLib.UserContext.Instance.ObserverObject.ActionWindow.UserActions)
                {
                    string description = (action.Id > -1 ? "[ID: " + action.Id.ToString() + "] " + action.Description : action.Description);
                    ActionsListBox.Items.Add(description);
                }
            }
            catch
            {

            }

            // update generalizations
            GeneralizationsListBox.Items.Clear();
            try
            {
                foreach (KeyValuePair<short, List<ContextLib.DataContainers.Monitoring.Generalizations.Generalization>> pair in ContextLib.UserContext.Instance.ObserverObject.ActionWindow.Generalizations)
                {
                    string gens = "[ID: " + pair.Key.ToString() + "] ";
                    foreach (ContextLib.DataContainers.Monitoring.Generalizations.Generalization generalization in pair.Value)
                        gens += generalization.ToString() + "; ";
                    GeneralizationsListBox.Items.Add(gens);
                }
            }
            catch
            {

            }

            // update repetitions
            RepetitionsListBox.Items.Clear();
            try
            {
                foreach (ContextLib.DataContainers.Monitoring.UserActionList actions in ContextLib.UserContext.Instance.ApprenticeObject.GetLongestRepetitions())
                {
                    if (actions.Count > 0)
                    {
                        string repetition = "[";
                        foreach (ContextLib.DataContainers.Monitoring.UserAction action in actions)
                        {
                            repetition += action.Id + ", ";
                        }
                        repetition = repetition.Substring(0, repetition.Length - 2) + "]";
                        RepetitionsListBox.Items.Add(repetition);
                    }
                }
            }
            catch
            {

            }
        }

    }
}
