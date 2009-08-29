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
using System.Threading;
using System.Windows.Forms;
using ContextLib;
using ContextLib.DataContainers.Monitoring;

namespace Blaze.Automation
{
    public partial class AssistantWindow : Form
    {
        private MainForm _parent;
        private AssistantModifyWindow _edit_window = null;
        ScriptNamePicker _script_name_picker = null;

        private int _selected_suggestion = 0;
        private int _max_suggestions = 0;
        private Suggestion[] _suggestions;
        private int[] _iterations;
        private float[] _speeds;
        private bool[] _ignore_speed;
        private Thread _thread = null;

        public AssistantWindow(MainForm parent)
        {
            _parent = parent;
            InitializeComponent();
            Gma.UserActivityMonitor.HookManager.KeyDown += new KeyEventHandler(HookManager_KeyDown);
            SuggestionDisplay.KeyDown += new KeyEventHandler(AssistantWindow_KeyDown);
            this.GotFocus += new EventHandler(AssistantWindow_GotFocus);
            SaveFileDialog.InitialDirectory = SystemCore.CommonTypes.CommonInfo.ScriptsFolder;
            Iterations.KeyDown += new KeyEventHandler(Iterations_KeyDown);
            Iterations.LostFocus += new EventHandler(Iterations_LostFocus);
        }

        void Iterations_LostFocus(object sender, EventArgs e)
        {
            //Iterations.Text = GetCurrentIteration().ToString();
            int num;
            if (_suggestions.Length > 0)
            {
                if (Int32.TryParse(Iterations.Text, out num))
                {
                    SetCurrentIteration(num);
                    UpdateIterations();
                    LoadSelectedSuggestion();
                }
                else
                    Iterations.Text = GetCurrentIteration().ToString();
            }
            else
            {
                Iterations.Text = "0";
            }
        }

        void Iterations_KeyDown(object sender, KeyEventArgs e)
        {
            int num;
            if (e.KeyCode == Keys.Return)
            {
                if (_suggestions.Length > 0)
                {
                    if (Int32.TryParse(Iterations.Text, out num))
                    {
                        SetCurrentIteration(num);
                        UpdateIterations();
                        LoadSelectedSuggestion();
                    }
                    else
                        Iterations.Text = GetCurrentIteration().ToString();
                }
                else
                {
                    Iterations.Text = "0";
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        void AssistantWindow_GotFocus(object sender, EventArgs e)
        {
            SuggestionDisplay.Focus();
        }

        void AssistantWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    DoitButton_Click(null, null);
                    break;
                case Keys.Left:
                    PreviousSuggestion_Click(null, null);
                    break;
                case Keys.Right:
                    NextSuggestion_Click(null, null);
                    break;
                case Keys.Down:
                    DecreaseRepetitionNumber_Click(null, null);
                    break;
                case Keys.Up:
                    IncreaseRepetitionNumber_Click(null, null);
                    break;
                case Keys.Subtract:
                    DecreaseSpeed_Click(null, null);
                    break;
                case Keys.Add:
                    IncreaseSpeed_Click(null, null);
                    break;
                case Keys.OemMinus:
                    DecreaseSpeed_Click(null, null);
                    break;
                case Keys.Oemplus:
                    IncreaseSpeed_Click(null, null);
                    break;
                case Keys.Escape:
                    Close();
                    break;
                case Keys.M:
                    if (e.Control)
                        ModifyButton_Click(null, null);
                    break;
                case Keys.N:
                    if (e.Control)
                        NewButton_Click(null, null);
                    break;
                case Keys.C:
                    if (e.Control)
                        CopyTextToClipboard();
                    break;
                case Keys.S:
                    if (e.Control && !e.Alt)
                        SaveMenuItem_Click(null, null);
                    else if (e.Control && e.Alt)
                        SaveAsMenuItem_Click(null, null);
                    break;
                case Keys.F1:
                    toolStripButton1_Click(null, null);
                    break;
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Pause)
            {
                if (_thread != null && _thread.ThreadState != ThreadState.Stopped)
                {
                    _thread.Abort();
                    //MessageBox.Show("Automation aborted by the user.", "Automation aborted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _parent.UncheckAssistantWindow();
            base.OnClosed(e);
        }

        private void AssistantWindow_Load(object sender, EventArgs e)
        {
            UserContext.Instance.ObserverObject.StopMonitoring();
            UserContext.Instance.ObserverObject.StopMonitoring();
            int iskip = (UserContext.Instance.ObserverObject.UseCompression ? 0 : 1), fskip = 1;
            UserContext.Instance.StopMacroRecording(SystemCore.CommonTypes.CommonInfo.ScriptsFolder, iskip, fskip);
            UserContext.Instance.AssistantObject.ValidateSuggestions();
            _suggestions = UserContext.Instance.AssistantObject.Suggestions;
            _max_suggestions = _suggestions.Length;
            _selected_suggestion = 1;

            _iterations = new int[_max_suggestions];
            _speeds = new float[_max_suggestions];
            _ignore_speed = new bool[_max_suggestions];

            for (int i = 0; i < _max_suggestions; i++)
            {
                _iterations[i] = 1;
                if (!_suggestions[i].SupportsMaxSpeed)
                {
                    _speeds[i] = 1.0f;
                    _ignore_speed[i] = false;
                }
                else
                {
                    _speeds[i] = 2.0f;
                    _ignore_speed[i] = true;
                }
            }

            UpdateCurrentSuggestion();
            UpdateIterations();
            UpdateSpeed();
            LoadSelectedSuggestion();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _suggestions = null;
            UserContext.Instance.ObserverObject.StartMonitoring();
            Gma.UserActivityMonitor.HookManager.KeyDown -= HookManager_KeyDown;
            base.OnClosing(e);
        }

        //

        private int GetCurrentIteration()
        {
            return _iterations[_selected_suggestion-1];
        }

        private float GetCurrentSpeed()
        {
            return _speeds[_selected_suggestion - 1];
        }

        private bool GetCurrentIgnoreSpeed()
        {
            return _ignore_speed[_selected_suggestion - 1];
        }

        private Suggestion GetCurrentSuggestion()
        {
            return _suggestions[_selected_suggestion - 1];
        }

        private void SetCurrentIteration(int val)
        {
            if (val > 1000)
                _iterations[_selected_suggestion - 1] = 1000;
            else if (val < 1)
                _iterations[_selected_suggestion - 1] = 1;
            else
                _iterations[_selected_suggestion - 1] = val;
        }

        private void SetCurrentSpeed(float val)
        {
            _speeds[_selected_suggestion - 1] = val;
        }

        private void SetCurrentIgnoreSpeed(bool val)
        {
            _ignore_speed[_selected_suggestion - 1] = val;
        }

        private void SetCurrentSuggestion(Suggestion suggestion)
        {
            _suggestions[_selected_suggestion - 1] = suggestion;
        }
        //

        private void UpdateCurrentSuggestion()
        {
            CurrentSuggestion.Text = (_suggestions.Length > 0 ? _selected_suggestion.ToString() : "0") + @"/" + _max_suggestions;
        }

        private void UpdateIterations()
        {
            Iterations.Text = (_suggestions.Length > 0 ? GetCurrentIteration().ToString() : "0");
        }

        private void UpdateSpeed()
        {
            if (_suggestions.Length > 0)
            {
                if (!GetCurrentIgnoreSpeed())
                    Speed.Text = GetCurrentSpeed().ToString() + "x";
                else
                    Speed.Text = "Max";
            }
            else
            {
                Speed.Text = "0x";
            }
        }

        private void LoadSelectedSuggestion()
        {
            if (_suggestions.Length > 0)
            {
                Suggestion suggestion = GetCurrentSuggestion();
                suggestion.Update(GetCurrentIteration());
                //SuggestionDisplay.Text = suggestion.Narrative;
                SuggestionDisplay.Rtf = RTFUtility.GenerateRTF(suggestion.Narrative);
                //MessageBox.Show(SuggestionDisplay.Rtf);
                if (suggestion.SupportsIterations)
                {
                    // show iterations
                    Separator2.Visible = true;
                    RepeatLabel.Visible = true;
                    DecreaseRepetitionNumber.Visible = true;
                    Iterations.Visible = true;
                    IncreaseRepetitionNumber.Visible = true;
                }
                else
                {
                    // hide iterations
                    Separator2.Visible = false;
                    RepeatLabel.Visible = false;
                    DecreaseRepetitionNumber.Visible = false;
                    Iterations.Visible = false;
                    IncreaseRepetitionNumber.Visible = false;
                }
            }
            else
            {
                SuggestionDisplay.Text = "Sorry but no repetitions were detected yet.";
            }
        }

        private void NextSuggestion_Click(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                _selected_suggestion++;
                if (_selected_suggestion > _max_suggestions)
                    _selected_suggestion = 1;
                UpdateCurrentSuggestion();
                UpdateIterations();
                UpdateSpeed();
                LoadSelectedSuggestion();
            }
        }

        private void PreviousSuggestion_Click(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                _selected_suggestion--;
                if (_selected_suggestion < 1)
                    _selected_suggestion = _max_suggestions;
                UpdateCurrentSuggestion();
                UpdateIterations();
                UpdateSpeed();
                LoadSelectedSuggestion();
            }
        }

        private void DoitButton_Click(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                Suggestion suggestion = GetCurrentSuggestion();
                WindowState = FormWindowState.Minimized;
                _parent.LastAcceptedSuggestion = suggestion;
                ThreadStart ts = new ThreadStart(delegate()
                    {
                        suggestion.Execute(GetCurrentSpeed(), GetCurrentIgnoreSpeed());
                    });
                _thread = new Thread(ts);
                _thread.SetApartmentState(ApartmentState.STA);
                _thread.Start();
                _thread.Join();
                Close();
            }
        }

        private void IncreaseRepetitionNumber_Click(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                SetCurrentIteration(GetCurrentIteration() + 1);
                UpdateIterations();
                LoadSelectedSuggestion();
            }
        }

        private void DecreaseRepetitionNumber_Click(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                SetCurrentIteration(GetCurrentIteration() - 1);
                UpdateIterations();
                LoadSelectedSuggestion();
            }
        }

        private void DecreaseSpeed_Click(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                if (GetCurrentIgnoreSpeed())
                    SetCurrentIgnoreSpeed(false);
                else if (GetCurrentSpeed() > 0.25f)
                    SetCurrentSpeed(GetCurrentSpeed() - 0.25f);
                UpdateSpeed();
                LoadSelectedSuggestion();
            }
        }

        private void IncreaseSpeed_Click(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                if (GetCurrentSpeed() == 2.0f)
                    SetCurrentIgnoreSpeed(true);
                if (GetCurrentSpeed() < 2.0f)
                    SetCurrentSpeed(GetCurrentSpeed() + 0.25f);
                UpdateSpeed();
                LoadSelectedSuggestion();
            }
        }

        private void ModifyButton_Click(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                _edit_window = new AssistantModifyWindow(GetCurrentSuggestion());
                if (_edit_window.ShowDialog() == DialogResult.OK)
                {
                    SetCurrentSuggestion(_edit_window.ModifiedSuggestion);
                    UpdateIterations();
                    UpdateSpeed();
                    LoadSelectedSuggestion();
                }
                _edit_window.Dispose();
                _edit_window = null;
            }
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            _edit_window = new AssistantModifyWindow(new Suggestion(new UserActionList(), new Dictionary<UserAction,List<ContextLib.DataContainers.Monitoring.Generalizations.Generalization>>()));
            if (_edit_window.ShowDialog() == DialogResult.OK)
            {
                List<Suggestion> sugs = new List<Suggestion>(_suggestions);
                sugs.Reverse();
                sugs.Add(_edit_window.ModifiedSuggestion);
                sugs.Reverse();
                _suggestions = sugs.ToArray();
                _max_suggestions = _suggestions.Length;

                List<int> it = new List<int>(_iterations);
                it.Reverse();
                it.Add(1);
                it.Reverse();
                _iterations = it.ToArray();

                List<float> sp = new List<float>(_speeds);
                sp.Reverse();
                sp.Add(1.0f);
                sp.Reverse();
                _speeds = sp.ToArray();
                
                List<bool> ig = new List<bool>(_ignore_speed);
                ig.Reverse();
                ig.Add(false);
                ig.Reverse();
                _ignore_speed = ig.ToArray();

                UpdateCurrentSuggestion();
                UpdateIterations();
                UpdateSpeed();
                LoadSelectedSuggestion();
            }
            _edit_window.Dispose();
            _edit_window = null;
        }

        private void CopyTextToClipboard()
        {
            if (SuggestionDisplay.SelectedText == string.Empty)
                CopyEverythingToClipboard();
            else
                CopySelectedTextToClipboard();
        }

        private void CopySelectedTextToClipboard()
        {
            DataObject data = new DataObject();
            data.SetData(DataFormats.Text, SuggestionDisplay.SelectedText);
            data.SetData(DataFormats.Rtf, SuggestionDisplay.SelectedRtf);
            Clipboard.SetDataObject(data);
        }

        private void CopyEverythingToClipboard()
        {
            DataObject data = new DataObject();
            data.SetData(DataFormats.Text, SuggestionDisplay.Text);
            data.SetData(DataFormats.Rtf, SuggestionDisplay.Rtf);
            Clipboard.SetDataObject(data);
        }

        private void SaveButton_ButtonClick(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                _script_name_picker = new ScriptNamePicker(SystemCore.CommonTypes.CommonInfo.ScriptsFolder);
                if (_script_name_picker.ShowDialog() == DialogResult.OK)
                {

                }
                _script_name_picker.Dispose();
                _script_name_picker = null;
            }
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                _script_name_picker = new ScriptNamePicker(SystemCore.CommonTypes.CommonInfo.ScriptsFolder);
                if (_script_name_picker.ShowDialog() == DialogResult.OK)
                {

                }
                _script_name_picker.Dispose();
                _script_name_picker = null;
            }
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            if (_suggestions.Length > 0)
            {
                SaveFileDialog.ShowDialog();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://blaze-wins.sourceforge.net/index.php?page=overview#assistant");
        }
    }
}
