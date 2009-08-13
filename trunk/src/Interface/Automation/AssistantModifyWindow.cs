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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Blaze.Automation.Wizard;
using ContextLib.DataContainers.Monitoring;
using ContextLib.DataContainers.Monitoring.Generalizations;

namespace Blaze.Automation
{
    public partial class AssistantModifyWindow : Form
    {
        private Suggestion _suggestion;
        private Suggestion _backup_suggestion;
        private int _n_actions;

        private enum ImageIndex : int
        {
            FileAction = 0,
            KeyboardAction = 1,
            MouseAction = 2,
            TextAction = 3
        }

        public Suggestion ModifiedSuggestion { get { return _suggestion; } }

        public AssistantModifyWindow(Suggestion suggestion)
        {
            InitializeComponent();
            ActionsView.TileSize = new Size(90, 35);

            _backup_suggestion = suggestion;
            RestoreSuggestion();
            ActionsView.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(ActionsView_ItemSelectionChanged);
            ActionsView.MouseClick += new MouseEventHandler(ActionsView_MouseClick);
            ActionsView.MouseDoubleClick += new MouseEventHandler(ActionsView_MouseDoubleClick);
        }

        void ActionsView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                EditActionWizard();
            }
        }

        void ActionsView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (GetSelectedAction() > -1)
                    ItemContextMenu.Show(ActionsView, e.Location);
            }
        }

        void ActionsView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                UpdateActionDescription();
        }

        private void AssistantModifyWindow_Load(object sender, EventArgs e)
        {
            UpdateActionsListView();
            UpdateAutomationPreview();
            SelectFirstAction();
        }

        //
        // Form Updating Methods
        //

        private void UpdateActionsListView()
        {
            ActionsView.Items.Clear();
            for (int i = 0; i < _suggestion.BasicActionList.Count; i++)
            {
                UserAction action = _suggestion.BasicActionList[i];
                ListViewItem item = new ListViewItem();
                item.Text = "Action " + i.ToString();
                if (action.IsType(UserAction.UserActionType.FileAction))
                {
                    item.ImageIndex = (int)ImageIndex.FileAction;
                    item.ToolTipText = "Filesystem Action";
                }
                else if (action.IsType(UserAction.UserActionType.TypeTextAction))
                {
                    item.ImageIndex = (int)ImageIndex.TextAction;
                    item.ToolTipText = "Text Action";
                }
                else if (action.IsType(UserAction.UserActionType.KeyAction))
                {
                    item.ImageIndex = (int)ImageIndex.KeyboardAction;
                    item.ToolTipText = "Key Press Action";
                }
                else if (action.IsType(UserAction.UserActionType.MouseAction))
                {
                    item.ImageIndex = (int)ImageIndex.MouseAction;
                    item.ToolTipText = "Mouse Action";
                }
                ActionsView.Items.Add(item);
            }
            _n_actions = _suggestion.BasicActionList.Count;
            if (_n_actions == 0)
                OkButton.Enabled = false;
            else
                OkButton.Enabled = true;
        }

        private void UpdateAutomationPreview()
        {
            if (_n_actions > 0)
            {
                _suggestion.Update();
                AutomationPreviewBox.Rtf = RTFUtility.GenerateRTF(_suggestion.Narrative);
            }
            else
            {
                AutomationPreviewBox.Text = "There are no actions available to compose an automation.";
            }
        }

        private void UpdateActionDescription()
        {
            int index = GetSelectedAction();
            if (index != -1)
            {
                UserAction action = _suggestion.BasicActionList[index];
                _suggestion.Update();
                ActionDescriptionBox.Rtf = RTFUtility.GenerateRTF(_suggestion.GetSingleActionNarrative(GetSelectedAction()));
                AlternativesDisplay.Text = (_suggestion.Alternatives[action] + 1).ToString() + "/" + _suggestion.NumberOfAlternatives[action].ToString();
            }
            else
            {
                ActionDescriptionBox.Text = "No action selected.";
                AlternativesDisplay.Text = "0/0";
            }
        }

        //
        //
        //

        //
        // Form Button Methods
        //

        private void Reset()
        {
            RestoreSuggestion();
            UpdateActionsListView();
            UpdateAutomationPreview();
            SelectFirstAction();
        }

        private void RestoreSuggestion()
        {
            _suggestion = new Suggestion(_backup_suggestion);
            _suggestion.IngoreSpeed = _backup_suggestion.IngoreSpeed;
            _suggestion.Iterations = _backup_suggestion.Iterations;
            _suggestion.Speed = _backup_suggestion.Speed;
            _suggestion.Validate(false);
        }

        //private bool ValidadeActions()
        //{
        //    bool has_file_actions = false;
        //    bool has_other_actions = false;
        //    foreach (UserAction action in _suggestion.BasicActionList)
        //    {
        //        if (action.IsType(UserAction.UserActionType.FileAction))
        //        {
        //            if (has_other_actions)
        //                return false;
        //            else
        //                has_file_actions = true;
        //        }
        //        else
        //        {
        //            if (has_file_actions)
        //                return false;
        //            else
        //                has_other_actions = true;
        //        }
        //    }
        //    return true;
        //}

        //
        //
        //

        //
        // Action List View Methods
        //

        private int GetSelectedAction()
        {
            if (_n_actions > 0)
            {
                int pos;
                try
                {
                    pos = ActionsView.SelectedIndices[0];
                }
                catch
                {
                    pos = 0;
                    SelectAtion(pos);
                }
                return pos;
            }
            else
            {
                return -1;
            }
        }

        private void SelectAtion(int index)
        {
            if (ActionsView.Items.Count > 0)
            {
                if (index < 0)
                    ActionsView.Items[_n_actions - 1].Selected = true;
                else if (index >= _n_actions)
                    ActionsView.Items[0].Selected = true;
                else
                    ActionsView.Items[index].Selected = true;
            }
            else
            {
                UpdateActionDescription();
            }
        }

        private void SelectFirstAction()
        {
            if (ActionsView.Items.Count > 0)
            {
                SelectAtion(0);
            }
        }

        private void SelectLastAction()
        {
            if (ActionsView.Items.Count > 0)
            {
                SelectAtion(ActionsView.Items.Count - 1);
            }
        }

        private void MoveSelectedActionUp()
        {
            int index = GetSelectedAction();
            if (index != -1)
            {
                if (index > 0)
                {
                    UserAction tmp = _suggestion.BasicActionList[index - 1];
                    _suggestion.BasicActionList[index - 1] = _suggestion.BasicActionList[index];
                    _suggestion.BasicActionList[index] = tmp;

                    DateTime tmp_time = _suggestion.BasicActionList[index - 1].Time;
                    _suggestion.BasicActionList[index - 1].Time = _suggestion.BasicActionList[index].Time;
                    _suggestion.BasicActionList[index].Time = tmp_time;
                }
                else
                {
                    UserAction tmp = _suggestion.BasicActionList[_n_actions - 1];
                    _suggestion.BasicActionList[_n_actions - 1] = _suggestion.BasicActionList[index];
                    _suggestion.BasicActionList[index] = tmp;

                    DateTime tmp_time = _suggestion.BasicActionList[_n_actions - 1].Time;
                    _suggestion.BasicActionList[_n_actions - 1].Time = _suggestion.BasicActionList[index].Time;
                    _suggestion.BasicActionList[index].Time = tmp_time;
                }
                UpdateActionsListView();
                UpdateAutomationPreview();
                SelectAtion(index - 1);
            }
        }

        private void MoveSelectedActionDown()
        {
            int index = GetSelectedAction();
            if (index != -1)
            {
                if (index < _n_actions - 1)
                {
                    UserAction tmp = _suggestion.BasicActionList[index + 1];
                    _suggestion.BasicActionList[index + 1] = _suggestion.BasicActionList[index];
                    _suggestion.BasicActionList[index] = tmp;

                    DateTime tmp_time = _suggestion.BasicActionList[index + 1].Time;
                    _suggestion.BasicActionList[index + 1].Time = _suggestion.BasicActionList[index].Time;
                    _suggestion.BasicActionList[index].Time = tmp_time;
                }
                else
                {
                    UserAction tmp = _suggestion.BasicActionList[0];
                    _suggestion.BasicActionList[0] = _suggestion.BasicActionList[index];
                    _suggestion.BasicActionList[index] = tmp;

                    DateTime tmp_time = _suggestion.BasicActionList[0].Time;
                    _suggestion.BasicActionList[0].Time = _suggestion.BasicActionList[index].Time;
                    _suggestion.BasicActionList[index].Time = tmp_time;
                }
                UpdateActionsListView();
                UpdateAutomationPreview();
                SelectAtion(index + 1);
            }
        }

        private void RemoveSelectedAction()
        {
            int index = GetSelectedAction();
            if (index != -1)
            {
                _suggestion.BasicActionList.RemoveAt(index);
                UpdateActionsListView();
                UpdateAutomationPreview();
                SelectAtion(index + 1);
            }
        }

        private void RemoveAllActions()
        {
            _suggestion.BasicActionList.Clear();
            UpdateActionsListView();
            UpdateAutomationPreview();
            UpdateActionDescription();
        }

        //
        //
        //

        //
        // Action Description Methods
        //

        private int GetCurrentAlternative(int index)
        {
            UserAction action = _suggestion.BasicActionList[index];
            int alternative = _suggestion.Alternatives[action];
            return alternative;
        }

        private void SetCurrentAlternative(int index, int val)
        {
            UserAction action = _suggestion.BasicActionList[index];
            if (val < 0)
                _suggestion.Alternatives[action] = _suggestion.NumberOfAlternatives[action] - 1;
            else if (val >= _suggestion.NumberOfAlternatives[action])
                _suggestion.Alternatives[action] = 0;
            else
                _suggestion.Alternatives[action] = val;
        }

        private void ShowPreviousAlternative()
        {
            int index = GetSelectedAction();
            if (index != -1)
            {
                int alternative = GetCurrentAlternative(index) - 1;
                SetCurrentAlternative(index, alternative);
                UpdateActionDescription();
                UpdateAutomationPreview();
            }
        }

        private void ShowNextAlternative()
        {
            int index = GetSelectedAction();
            if (index != -1)
            {
                int alternative = GetCurrentAlternative(index) + 1;
                SetCurrentAlternative(index, alternative);
                UpdateActionDescription();
                UpdateAutomationPreview();
            }
        }

        //
        //
        //

        //
        // Editing Methods
        //

        private void NewActionWizard()
        {
            List<Form> wizard = new List<Form>();
            DateTime base_date;
            if (_n_actions > 0)
                base_date = _suggestion.BasicActionList[_suggestion.BasicActionList.Count - 1].Time;
            else
                base_date = DateTime.Now;
            if (WizardLoop(out wizard, null, 0) == DialogResult.Yes)
            {
                UserAction.UserActionType type = ((WizardTypePicker)wizard[1]).ActionType;
                UserAction ac = null;
                Generalization[] generaliations = null;
                switch (type)
                {
                    case UserAction.UserActionType.KeyPressAction:
                        {
                            WizardKeyPicker key_picker = (WizardKeyPicker)wizard[2];
                            WizardWindowPicker window_picker = (WizardWindowPicker)wizard[3];
                            WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[4];
                            ac = new KeyPressAction(window_picker.Window, key_picker.Key, key_picker.Modifiers);
                            TimeSpan delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                            ac.Time = base_date + delta;
                            generaliations = KeyGeneralization.Generate(ac.QuickDescription, ac.QuickDescription, delta);
                        }
                        break;
                    case UserAction.UserActionType.MouseClickAction:
                        {
                            WizardClickPicker click_picker = (WizardClickPicker)wizard[2];
                            WizardWindowPicker window_picker = (WizardWindowPicker)wizard[3];
                            WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[4];
                            ac = new MouseClickAction(window_picker.Window, click_picker.Button, (uint)click_picker.X, (uint)click_picker.Y, click_picker.Modifiers);
                            TimeSpan delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                            ac.Time = base_date + delta;
                            generaliations = MouseGeneralization.Generate(click_picker.X, click_picker.Y, click_picker.X, click_picker.Y, delta);
                        }
                        break;
                    case UserAction.UserActionType.MouseDoubleClickAction:
                        {
                            WizardClickPicker click_picker = (WizardClickPicker)wizard[2];
                            WizardWindowPicker window_picker = (WizardWindowPicker)wizard[3];
                            WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[4];
                            ac = new MouseClickAction(window_picker.Window, click_picker.Button, (uint)click_picker.X, (uint)click_picker.Y, click_picker.Modifiers);
                            TimeSpan delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                            ac.Time = base_date + delta;
                            generaliations = MouseGeneralization.Generate(click_picker.X, click_picker.Y, click_picker.X, click_picker.Y, delta);
                        }
                        break;
                    case UserAction.UserActionType.MouseDragAction:
                        {
                            WizardClickPicker click_picker = (WizardClickPicker)wizard[2];
                            WizardPositionPicker position_picker = (WizardPositionPicker)wizard[2];
                            WizardWindowPicker window_picker = (WizardWindowPicker)wizard[3];
                            WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[4];
                            ac = new MouseDragAction(window_picker.Window, click_picker.Button, (uint)click_picker.X, (uint)click_picker.Y, (uint)position_picker.X, (uint)position_picker.Y, click_picker.Modifiers);
                            TimeSpan delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                            ac.Time = base_date + delta;
                            generaliations = MouseDragGeneralization.Generate(click_picker.X, click_picker.Y, click_picker.X, click_picker.Y,
                                                                             position_picker.X, position_picker.Y, position_picker.X, position_picker.Y, delta);
                        }
                        break;
                    case UserAction.UserActionType.MouseWheelSpinAction:
                        {
                            WizardWheelSpinPicker spin_picker = (WizardWheelSpinPicker)wizard[2];
                            WizardWindowPicker window_picker = (WizardWindowPicker)wizard[3];
                            WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[4];
                            ac = new MouseWheelSpinAction(window_picker.Window, spin_picker.Notches, (uint)spin_picker.X, (uint)spin_picker.Y, spin_picker.Modifiers);
                            TimeSpan delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                            ac.Time = base_date + delta;
                            generaliations = MouseGeneralization.Generate(spin_picker.X, spin_picker.Y, spin_picker.X, spin_picker.Y, delta);
                        }
                        break;
                    case UserAction.UserActionType.TypeTextAction:
                        {
                            WizardTextPicker text_picker = (WizardTextPicker)wizard[2];
                            WizardWindowPicker window_picker = (WizardWindowPicker)wizard[3];
                            WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[4];
                            ac = new TypeTextAction(window_picker.Window, text_picker.TXT);
                            TimeSpan delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                            ac.Time = base_date + delta;
                            generaliations = TextGeneralization.Generate(text_picker.TXT, text_picker.TXT, delta);
                        }
                        break;
                }
                _suggestion.BasicActionList.Add(ac);
                _suggestion.BasicGeneralizations.Add(ac, new List<Generalization>(generaliations));
                _suggestion.Validate(false);
                UpdateActionsListView();
                UpdateAutomationPreview();
                SelectLastAction();
            }

            for (int i = 0; i < wizard.Count; i++)
            {
                wizard[i].Dispose();
            }
        }

        private void EditActionWizard()
        {

            int ac_index = GetSelectedAction();
            if (ac_index > -1)
            {
                List<Form> wizard = new List<Form>();
                UserAction action = _suggestion.BasicActionList[ac_index];
                UserAction last_action = null;
                int time_span = 0;
                if (ac_index > 0)
                {
                    last_action = _suggestion.BasicActionList[ac_index - 1];
                    time_span = (int)((last_action.Time - action.Time).Ticks / TimeSpan.TicksPerMillisecond);
                }

                if (WizardLoop(out wizard, action, time_span) == DialogResult.Yes)
                {
                    UserAction.UserActionType type = action.ActionType;
                    TimeSpan delta = TimeSpan.Zero;
                    switch (type)
                    {
                        case UserAction.UserActionType.KeyPressAction:
                            {
                                WizardKeyPicker key_picker = (WizardKeyPicker)wizard[1];
                                WizardWindowPicker window_picker = (WizardWindowPicker)wizard[2];
                                WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[3];
                                DateTime base_date = (last_action == null ? DateTime.Now : last_action.Time);
                                KeyPressAction kaction = (KeyPressAction)action;
                                kaction.Window = window_picker.Window;
                                kaction.Key = key_picker.Key;
                                kaction.Modifiers = key_picker.Modifiers;
                                delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                                action.Time = base_date + delta;
                            }
                            break;
                        case UserAction.UserActionType.MouseClickAction:
                            {
                                WizardClickPicker click_picker = (WizardClickPicker)wizard[1];
                                WizardWindowPicker window_picker = (WizardWindowPicker)wizard[2];
                                WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[3];
                                DateTime base_date = (last_action == null ? DateTime.Now : last_action.Time);
                                MouseClickAction maction = (MouseClickAction)action;
                                maction.Window = window_picker.Window;
                                maction.Button = click_picker.Button;
                                maction.X = (uint)click_picker.X;
                                maction.Y = (uint)click_picker.Y;
                                maction.Modifiers = click_picker.Modifiers;
                                delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                                action.Time = base_date + delta;
                                foreach (Generalization gen in _suggestion.BasicGeneralizations[action])
                                {
                                    if (gen.Type == Generalization.GeneralizationType.MouseGeneralization)
                                    {
                                        MouseGeneralization g = (MouseGeneralization)gen;
                                        g.AverageX = click_picker.X;
                                        g.AverageY = click_picker.Y;
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.MouseDoubleClickAction:
                            {
                                WizardClickPicker click_picker = (WizardClickPicker)wizard[1];
                                WizardWindowPicker window_picker = (WizardWindowPicker)wizard[2];
                                WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[3];
                                DateTime base_date = (last_action == null ? DateTime.Now : last_action.Time);
                                MouseDoubleClickAction maction = (MouseDoubleClickAction)action;
                                maction.Window = window_picker.Window;
                                maction.Button = click_picker.Button;
                                maction.X = (uint)click_picker.X;
                                maction.Y = (uint)click_picker.Y;
                                maction.Modifiers = click_picker.Modifiers;
                                delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                                action.Time = base_date + delta;
                                foreach (Generalization gen in _suggestion.BasicGeneralizations[action])
                                {
                                    if (gen.Type == Generalization.GeneralizationType.MouseGeneralization)
                                    {
                                        MouseGeneralization g = (MouseGeneralization)gen;
                                        g.AverageX = click_picker.X;
                                        g.AverageY = click_picker.Y;
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.MouseDragAction:
                            {
                                WizardClickPicker click_picker = (WizardClickPicker)wizard[1];
                                WizardPositionPicker position_picker = (WizardPositionPicker)wizard[2];
                                WizardWindowPicker window_picker = (WizardWindowPicker)wizard[3];
                                WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[4];
                                DateTime base_date = (last_action == null ? DateTime.Now : last_action.Time);
                                MouseDragAction maction = (MouseDragAction)action;
                                maction.Window = window_picker.Window;
                                maction.Button = click_picker.Button;
                                maction.InitialX = (uint)click_picker.X;
                                maction.InitialY = (uint)click_picker.Y;
                                maction.FinalX = (uint)position_picker.X;
                                maction.FinalY = (uint)position_picker.Y;
                                maction.Modifiers = click_picker.Modifiers;
                                delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                                action.Time = base_date + delta;
                                foreach (Generalization gen in _suggestion.BasicGeneralizations[action])
                                {
                                    if (gen.Type == Generalization.GeneralizationType.MouseDragGeneralization)
                                    {
                                        MouseDragGeneralization g = (MouseDragGeneralization)gen;
                                        g.AverageXi = click_picker.X;
                                        g.AverageYi = click_picker.Y;
                                        g.AverageYi = position_picker.X;
                                        g.AverageYf = position_picker.Y;
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.MouseWheelSpinAction:
                            {
                                WizardWheelSpinPicker spin_picker = (WizardWheelSpinPicker)wizard[1];
                                WizardWindowPicker window_picker = (WizardWindowPicker)wizard[2];
                                WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[3];
                                DateTime base_date = (last_action == null ? DateTime.Now : last_action.Time);
                                MouseWheelSpinAction maction = (MouseWheelSpinAction)action;
                                maction.Window = window_picker.Window;
                                maction.Delta = spin_picker.Notches;
                                maction.X = (uint)spin_picker.X;
                                maction.Y = (uint)spin_picker.Y;
                                maction.Modifiers = spin_picker.Modifiers;
                                delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                                action.Time = base_date + delta;
                                foreach (Generalization gen in _suggestion.BasicGeneralizations[action])
                                {
                                    if (gen.Type == Generalization.GeneralizationType.MouseGeneralization)
                                    {
                                        MouseGeneralization g = (MouseGeneralization)gen;
                                        g.AverageX = spin_picker.X;
                                        g.AverageY = spin_picker.Y;
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.TypeTextAction:
                            {
                                WizardTextPicker text_picker = (WizardTextPicker)wizard[1];
                                WizardWindowPicker window_picker = (WizardWindowPicker)wizard[2];
                                WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[3];
                                DateTime base_date = (last_action == null ? DateTime.Now : last_action.Time);
                                TypeTextAction taction = (TypeTextAction)action;
                                taction.Window = window_picker.Window;
                                taction.Text = text_picker.TXT;
                                delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                                action.Time = base_date + delta;
                            }
                            break;
                        case UserAction.UserActionType.FileCreatedAction:
                            {
                                WizardFilePicker file_picker = (WizardFilePicker)wizard[1];
                                WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[2];
                                FileCreatedAction faction = (FileCreatedAction)action;
                                DateTime base_date = (last_action == null ? DateTime.Now : last_action.Time);
                                List<string> items = file_picker.Items;
                                Dictionary<string, bool> active = file_picker.Active;
                                string item;

                                _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Clear();
                                _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1.Clear();
                                for (int i = 0; i < items.Count; i++)
                                {
                                    item = items[i];
                                    if (active[item])
                                    {
                                        _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Add(_backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1[i]);
                                        _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1.Add(_backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1[i]);
                                    }
                                }
                                delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                                action.Time = base_date + delta;
                            }
                            break;
                        case UserAction.UserActionType.FileDeletedAction:
                            {
                                WizardFilePicker file_picker = (WizardFilePicker)wizard[1];
                                WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[2];
                                FileDeletedAction faction = (FileDeletedAction)action;
                                DateTime base_date = (last_action == null ? DateTime.Now : last_action.Time);
                                List<string> items = file_picker.Items;
                                Dictionary<string, bool> active = file_picker.Active;
                                string item;

                                _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Clear();
                                _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1.Clear();
                                for (int i = 0; i < items.Count; i++)
                                {
                                    item = items[i];
                                    if (active[item])
                                    {
                                        _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Add(_backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1[i]);
                                        _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1.Add(_backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1[i]);
                                    }
                                }
                                delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                                action.Time = base_date + delta;
                            }
                            break;
                        case UserAction.UserActionType.FileMovedAction:
                            {
                                WizardFilePicker file_picker = (WizardFilePicker)wizard[1];
                                WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[2];
                                FileMovedAction faction = (FileMovedAction)action;
                                DateTime base_date = (last_action == null ? DateTime.Now : last_action.Time);
                                List<string> items = file_picker.Items;
                                Dictionary<string, bool> active = file_picker.Active;
                                string item;

                                _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Clear();
                                _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1.Clear();
                                for (int i = 0; i < items.Count; i++)
                                {
                                    item = items[i];
                                    if (active[item])
                                    {
                                        _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Add(_backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1[i]);
                                        _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1.Add(_backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1[i]);
                                    }
                                }
                                delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                                action.Time = base_date + delta;
                            }
                            break;
                        case UserAction.UserActionType.FileRenamedAction:
                            {
                                WizardFilePicker file_picker = (WizardFilePicker)wizard[1];
                                WizardDelayPicker delay_picker = (WizardDelayPicker)wizard[2];
                                FileRenamedAction faction = (FileRenamedAction)action;
                                DateTime base_date = (last_action == null ? DateTime.Now : last_action.Time);
                                List<string> items = file_picker.Items;
                                Dictionary<string, bool> active = file_picker.Active;
                                string item;
                                _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Clear();
                                _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1.Clear();
                                _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList2.Clear();
                                _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList2.Clear();
                                for (int i = 0; i < items.Count; i++)
                                {
                                    item = items[i];
                                    if (active[item])
                                    {
                                        _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Add(_backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1[i]);
                                        _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1.Add(_backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList1[i]);
                                        _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList2.Add(_backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList2[i]);
                                        _suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList2.Add(_backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].RtfList2[i]);
                                    }
                                }
                                delta = new TimeSpan(delay_picker.Delay * TimeSpan.TicksPerMillisecond);
                                action.Time = base_date + delta;
                            }
                            break;
                    }
                    // TODO: mudar os timstamps de todas
                    for (int i = GetSelectedAction() + 1; i < _suggestion.BasicActionList.Count; i++)
                    {
                        _suggestion.BasicActionList[i].Time += delta;
                    }

                    //_suggestion.Validate(false);
                    UpdateActionsListView();
                    UpdateAutomationPreview();
                    UpdateActionDescription();
                    //SelectLastAction();
                }

                for (int i = 0; i < wizard.Count; i++)
                {
                    wizard[i].Dispose();
                }
            }
        }

        private List<Form> GenerateWizard(UserAction.UserActionType type, UserAction action, int time_span)
        {
            List<Form> wizard = new List<Form>();
            switch (type)
            {
                case UserAction.UserActionType.KeyPressAction:
                    if (action == null)
                    {
                        wizard.Add(new WizardKeyPicker());
                        wizard.Add(new WizardWindowPicker());
                        wizard.Add(new WizardDelayPicker());
                    }
                    else
                    {
                        KeyPressAction ac = (KeyPressAction)action;
                        wizard.Add(new WizardKeyPicker(ac.Key, ac.Modifiers));
                        wizard.Add(new WizardWindowPicker(ac.Window));
                        wizard.Add(new WizardDelayPicker(time_span));
                    }
                    break;
                case UserAction.UserActionType.MouseClickAction:
                    if (action == null)
                    {
                        wizard.Add(new WizardClickPicker(false));
                        wizard.Add(new WizardWindowPicker());
                        wizard.Add(new WizardDelayPicker());
                    }
                    else
                    {
                        MouseClickAction ac = (MouseClickAction)action;
                        wizard.Add(new WizardClickPicker(false, ac.Button, ac.Modifiers, (int)ac.X, (int)ac.Y));
                        wizard.Add(new WizardWindowPicker(ac.Window));
                        wizard.Add(new WizardDelayPicker(time_span));
                    }
                    break;
                case UserAction.UserActionType.MouseDoubleClickAction:
                    if (action == null)
                    {
                        wizard.Add(new WizardClickPicker(false));
                        wizard.Add(new WizardWindowPicker());
                        wizard.Add(new WizardDelayPicker());
                    }
                    else
                    {
                        MouseDoubleClickAction ac = (MouseDoubleClickAction)action;
                        wizard.Add(new WizardClickPicker(false, ac.Button, ac.Modifiers, (int)ac.X, (int)ac.Y));
                        wizard.Add(new WizardWindowPicker(ac.Window));
                        wizard.Add(new WizardDelayPicker(time_span));
                    }
                    break;
                case UserAction.UserActionType.MouseDragAction:
                    if (action == null)
                    {
                        wizard.Add(new WizardClickPicker(true));
                        wizard.Add(new WizardPositionPicker());
                        wizard.Add(new WizardWindowPicker());
                        wizard.Add(new WizardDelayPicker());
                    }
                    else
                    {
                        MouseDragAction ac = (MouseDragAction)action;
                        wizard.Add(new WizardClickPicker(false, ac.Button, ac.Modifiers, (int)ac.InitialX, (int)ac.InitialY));
                        wizard.Add(new WizardPositionPicker((int)ac.FinalX, (int)ac.FinalY));
                        wizard.Add(new WizardWindowPicker(ac.Window));
                        wizard.Add(new WizardDelayPicker(time_span));
                    }
                    break;
                case UserAction.UserActionType.MouseWheelSpinAction:
                    if (action == null)
                    {
                        wizard.Add(new WizardWheelSpinPicker());
                        wizard.Add(new WizardWindowPicker());
                        wizard.Add(new WizardDelayPicker());
                    }
                    else
                    {
                        MouseWheelSpinAction ac = (MouseWheelSpinAction)action;
                        wizard.Add(new WizardWheelSpinPicker(ac.Delta, ac.Modifiers, (int)ac.X, (int)ac.Y));
                        wizard.Add(new WizardWindowPicker(ac.Window));
                        wizard.Add(new WizardDelayPicker(time_span));
                    }
                    break;
                case UserAction.UserActionType.TypeTextAction:
                    if (action == null)
                    {
                        wizard.Add(new WizardTextPicker());
                        wizard.Add(new WizardWindowPicker());
                        wizard.Add(new WizardDelayPicker());
                    }
                    else
                    {
                        TypeTextAction ac = (TypeTextAction)action;
                        wizard.Add(new WizardTextPicker(ac.Text, false));
                        wizard.Add(new WizardWindowPicker(ac.Window));
                        wizard.Add(new WizardDelayPicker(time_span));
                    }
                    break;
                case UserAction.UserActionType.FileCreatedAction:
                    {
                        List<string> items;
                        Dictionary<string, bool> active;
                        GenerateWizardFileFormLists(action, out items, out active);
                        wizard.Add(new WizardFilePicker(action, items, active));
                        wizard.Add(new WizardDelayPicker(time_span));
                    }
                    break;
                case UserAction.UserActionType.FileDeletedAction:
                    {
                        List<string> items;
                        Dictionary<string, bool> active;
                        GenerateWizardFileFormLists(action, out items, out active);
                        wizard.Add(new WizardFilePicker(action, items, active));
                        wizard.Add(new WizardDelayPicker(time_span));
                    }
                    break;
                case UserAction.UserActionType.FileMovedAction:
                    {
                        List<string> items;
                        Dictionary<string, bool> active;
                        GenerateWizardFileFormLists(action, out items, out active);
                        wizard.Add(new WizardFilePicker(action, items, active));
                        wizard.Add(new WizardDelayPicker(time_span));
                    }
                    break;
                case UserAction.UserActionType.FileRenamedAction:
                    {
                        List<string> items;
                        Dictionary<string, bool> active;
                        GenerateWizardFileFormLists(action, out items, out active);
                        wizard.Add(new WizardFilePicker(action, items, active));
                        wizard.Add(new WizardDelayPicker(time_span));
                    }
                    break;
            }
            return wizard;
        }

        private void GenerateWizardFileFormLists(UserAction action, out List<string>items , out Dictionary<string, bool> active)
        {
            items = new List<string>();
            active = new Dictionary<string, bool>();
            switch (action.ActionType)
            {
                case UserAction.UserActionType.FileRenamedAction:
                    for (int i = 0; i < _backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Count; i++)
                    {
                        string old_name = _backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1[i];
                        string new_name = _backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList2[i];
                        string item = "\"" + Path.GetFileName(old_name) + "\" to \"" + Path.GetFileName(new_name) + "\"";
                        items.Add(item);
                        if (_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Contains(old_name))
                        {
                            active.Add(item, true);
                        }
                        else
                        {
                            active.Add(item, false);
                        }
                    }
                    break;
                default:
                    for (int i = 0; i < _backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Count; i++)
                    {
                        string name = _backup_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1[i];
                        string item = "\"" + Path.GetFileName(name) + "\"";
                        items.Add(item);
                        if (_suggestion.FileOpDescriptors[action][_suggestion.Alternatives[action]].FileList1.Contains(name))
                        {
                            active.Add(item, true);
                        }
                        else
                        {
                            active.Add(item, false);
                        }
                    }
                    break;
            }
        }

        private DialogResult WizardLoop(out List<Form> forms, UserAction action, int time_span)
        {
            int i = 0;
            UserAction.UserActionType type = (action == null ? UserAction.UserActionType.TerminalAction : action.ActionType);
            bool edit = (type == UserAction.UserActionType.TerminalAction ? false : true);
            forms = new List<Form>();
            if (!edit) // new
            {
                forms.Add(new WizardWelcome());
                forms.Add(new WizardTypePicker());
            }
            else // edit
            {
                forms.Add(new WizardWelcome(type));
            }
            while (i < forms.Count)
            {
                switch (forms[i].ShowDialog())
                {
                    case DialogResult.Yes:
                        if (edit) 
                        {
                            if (i == 0)
                            {
                                if (forms.Count == 1)
                                    forms.AddRange(GenerateWizard(type, action, time_span));
                            }
                        }
                        else // New
                        {
                            if (i == 1)
                            {
                                if (type == UserAction.UserActionType.TerminalAction ||
                                    type != GetWizardActionType(forms))
                                {
                                    type = GetWizardActionType(forms);
                                    if (forms.Count > 2)
                                        forms.RemoveRange(2, forms.Count - 2);
                                    forms.AddRange(GenerateWizard(type, action, time_span));
                                }
                            }
                        }
                        i++;
                        break;
                    case DialogResult.No:
                        i--;
                        break;
                    case DialogResult.Cancel:
                        return DialogResult.Cancel;
                }
            }
            return DialogResult.Yes;
        }

        private UserAction.UserActionType GetWizardActionType(List<Form> forms)
        {
            return ((WizardTypePicker)forms[1]).ActionType;
        }

        //
        //
        //

        //
        // Button Event Handlers
        //

        private void OkButton_Click(object sender, EventArgs e)
        {
            //bool valid = ValidadeActions();
            //if (valid)
            //{
            //    DialogResult = DialogResult.OK;
            //    Close();
            //}
            //else
            //{
            //    MessageBox.Show("Filesystem actions can't be mixed with other actions in an Automation. Please correct this issue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            MoveSelectedActionUp();
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            MoveSelectedActionDown();
        }

        private void RemoveActionButton_Click(object sender, EventArgs e)
        {
            RemoveSelectedAction();
        }

        private void PreviousAlternativeButton_Click(object sender, EventArgs e)
        {
            ShowPreviousAlternative();
        }

        private void NextAlternativeButton_Click(object sender, EventArgs e)
        {
            ShowNextAlternative();
        }

        private void EditAlternativeButton_Click(object sender, EventArgs e)
        {
            EditActionWizard();
        }

        private void AddActionButton_Click(object sender, EventArgs e)
        {
            NewActionWizard();
        }

        //
        //
        //

        //
        // Context Menu Event Handlers
        //

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditActionWizard();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSelectedAction();
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveSelectedActionUp();
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveSelectedActionDown();
        }

        private void clearAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RemoveAllActions();
        }

        //
        //
        //

    }
}
