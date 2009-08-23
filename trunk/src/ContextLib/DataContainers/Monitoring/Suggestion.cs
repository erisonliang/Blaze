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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ContextLib.DataContainers.GUI;
using ContextLib.DataContainers.Monitoring.Generalizations;

namespace ContextLib.DataContainers.Monitoring
{
    public class Suggestion
    {
        #region Properties
        private string _narrative;
        private UserActionList _basic_action_list;
        private UserActionList _action_list;
        private Dictionary<UserAction, List<Generalizations.Generalization>> _basic_generalizations;
        private Dictionary<UserAction, List<FileOpDescriptor>> _file_op_dscrps;
        private bool _is_file_system;
        private bool _supports_iterations;
        private int _iterations;
        private float _speed;
        private bool _ignore_speed;
        //private int _first_iterations;
        private bool _from_beginning = false;
        private int _last_iterations;
        private Dictionary<UserAction, int> _alternatives;
        private Dictionary<UserAction, int> _n_alternatives;
        private bool _valid;
        private DateTime _time;
        private bool _supports_max_speed = true;
        //System.Windows.Forms.RichTextBox _rtb = new System.Windows.Forms.RichTextBox();
        #endregion

        #region Accessors
        public string Narrative { get { return _narrative; } }
        public UserActionList BasicActionList { get { return _basic_action_list; } set { _basic_action_list = value; } }
        public UserActionList ActionList { get { return _action_list; } }
        public bool IsFileSystem { get { return _is_file_system; } }
        public bool SupportsIterations { get { return _supports_iterations; } }
        public int Iterations { get { return _iterations; } set { _iterations = value; } }
        public float Speed { get { return _speed; } set { _speed = value; } }
        public bool IngoreSpeed { get { return _ignore_speed; } set { _ignore_speed = value; } }
        public bool Valid { get { return _valid; } }
        public Dictionary<UserAction, int> Alternatives { get { return _alternatives; } set { _alternatives = value; } }
        public Dictionary<UserAction, int> NumberOfAlternatives { get { return _n_alternatives; } }
        public DateTime Time { get { return _time; } }
        public Dictionary<UserAction, List<Generalizations.Generalization>> BasicGeneralizations { get { return _basic_generalizations; } }
        public Dictionary<UserAction, List<FileOpDescriptor>> FileOpDescriptors { get { return _file_op_dscrps; } }
        public bool SupportsMaxSpeed { get { return _supports_max_speed; } }
        #endregion

        #region Constructors
        public Suggestion(UserActionList basic_action_list, Dictionary<UserAction, List<Generalizations.Generalization>> basic_generalizations)
        {
            _basic_action_list = basic_action_list;
            _basic_generalizations = basic_generalizations;
            _file_op_dscrps = null;
            _narrative = string.Empty;
            _action_list = new UserActionList();
            _is_file_system = false;
            _supports_iterations = true;
            _iterations = 1;
            _speed = 1.0f;
            _ignore_speed = false;
            _from_beginning = false;
            _supports_max_speed = true;

            // check if there is any file operation
            // if there is, iterations will not be supported
            foreach (UserAction action in _basic_action_list)
            {
                if (action.IsType(UserAction.UserActionType.FileAction))
                {
                    _is_file_system = true;
                    if (!action.IsType(UserAction.UserActionType.FileCreatedAction))
                        _supports_iterations = false;
                }
            }

            // if it's a file system repetition, remove other possible non fs actions, to prevent bugs
            if (_is_file_system)
            {
                _basic_action_list.RemoveAll(delegate(UserAction ua)
                {
                    return !ua.IsType(UserAction.UserActionType.FileAction);
                });
            }

            // check if this automation supports max speed
            if (!_is_file_system)
            {
                int handle = -1;
                foreach (UserAction action in _basic_action_list)
                {
                    if (handle == -1)
                        handle = action.Window.Handle;
                    else
                        if (action.Window.Handle != handle)
                        {
                            _supports_max_speed = false;
                            break;
                        }
                }
            }

            //_first_iterations = _basic_generalizations[_basic_action_list[_basic_action_list.Count - 1].Id][0].Occurrences;
            _last_iterations = 0;

            if (_basic_action_list.Count > 0)
                _time = _basic_action_list[_basic_action_list.Count - 1].Time;
            Validate(false);
        }

        public Suggestion(Suggestion suggestion)
        {
            _basic_action_list = new UserActionList(suggestion._basic_action_list);
            _basic_generalizations = suggestion._basic_generalizations;
            _file_op_dscrps = null;
            _narrative = string.Empty;
            _action_list = new UserActionList();
            _is_file_system = false;
            _supports_iterations = true;
            _iterations = 1;
            _speed = 1.0f;
            _ignore_speed = false;
            _from_beginning = false;
            _supports_max_speed = true;

            // check if there is any file operation
            // if there is, iterations will not be supported
            foreach (UserAction action in _basic_action_list)
            {
                if (action.IsType(UserAction.UserActionType.FileAction))
                {
                    _is_file_system = true;
                    if (!action.IsType(UserAction.UserActionType.FileCreatedAction))
                        _supports_iterations = false;
                }
            }

            // if it's a file system repetition, remove other possible non fs actions, to prevent bugs
            if (_is_file_system)
            {
                _basic_action_list.RemoveAll(delegate(UserAction ua)
                {
                    return !ua.IsType(UserAction.UserActionType.FileAction);
                });
            }

            // check if this automation supports max speed
            if (!_is_file_system)
            {
                int handle = -1;
                foreach (UserAction action in _basic_action_list)
                {
                    if (handle == -1)
                        handle = action.Window.Handle;
                    else
                        if (action.Window.Handle != handle)
                        {
                            _supports_max_speed = false;
                            break;
                        }
                }
            }

            //_first_iterations = _basic_generalizations[_basic_action_list[_basic_action_list.Count - 1].Id][0].Occurrences;
            _last_iterations = 0;

            if (_basic_action_list.Count > 0)
                _time = _basic_action_list[_basic_action_list.Count - 1].Time;
            Validate(false);
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            Update(_iterations, false);
        }

        public void Update(bool from_beginning)
        {
            Update(_iterations, from_beginning);
        }

        public void Update(int iterations)
        {
            Update(iterations, false);
        }

        public void Update(int iterations, bool from_beginnig)
        {
            if (from_beginnig)
                _from_beginning = true;
            int max = (iterations <= 0 ? 1 : iterations);
            max += (from_beginnig ? 0 : _last_iterations);
            string narrative = string.Empty;
            UserActionList list = new UserActionList();
            int it = (from_beginnig ? 1 : 1 + _last_iterations);
            if (_supports_iterations)
            {
                narrative = GenerateNewNarrative(iterations, _from_beginning);
                if (narrative.Trim() == string.Empty)
                {
                    narrative = "This automation is no longer valid. Maybe one of the involved applications was, meanwhile, closed.";
                    list.Clear();
                }
                else
                {
                    while (it <= max)
                    {
                        list.AddRange(GenerateNewActionList(it, _from_beginning));
                        it++;
                    }
                }
            }
            else
            {
                //UpdateFileOpDescriptions(from_beginnig);
                narrative = GenerateNewNarrative(1, _from_beginning);
                list.AddRange(GenerateNewActionList(1, _from_beginning));
                if (narrative.Trim() == string.Empty)   
                {
                    narrative = "This automation is no longer valid. Maybe some of the involved files or folders were, meanwhile, modified.";
                    list.Clear();
                }
            }
            _narrative = narrative;
            _action_list = list;
            _iterations = iterations;
            //_from_beginning = from_beginnig;
        }

        public void Execute()
        {
            Execute(_speed, _ignore_speed);
        }

        public void Execute(float speed, bool ignore_speed)
        {
            if (!_valid)
                return;
            DateTime last_action_time = DateTime.MinValue; // time in which the action was recorded
            DateTime last_action_performed = DateTime.MinValue; // time in which the action was performed
            TimeSpan elapsed_action_time = TimeSpan.Zero; // elapsed time between the last two actions, we they were recorded
            TimeSpan elapsed_performance_time = TimeSpan.Zero; // elapsed time between the last action performance

            _speed = speed;
            _ignore_speed = ignore_speed;
            //SystemCore.SystemAbstraction.FileHandling.Logger log = new SystemCore.SystemAbstraction.FileHandling.Logger("assistant.log");
            foreach (UserAction action in _action_list)
            {
                if (!ignore_speed)
                {
                    if (last_action_time != DateTime.MinValue)
                    {
                        elapsed_performance_time = DateTime.Now - last_action_performed;
                        elapsed_action_time = action.Time - last_action_time;
                        if (elapsed_action_time > elapsed_performance_time)
                        {
                            long speeded_ticks = (long)(((float)(elapsed_action_time - elapsed_performance_time).Ticks) / speed);
                            Thread.Sleep((int)(speeded_ticks / TimeSpan.TicksPerMillisecond));
                        }
                    }
                    last_action_time = action.Time;
                    last_action_performed = DateTime.Now;
                }
                action.Execute();
                //log.WriteLine(action.Description);
            }
            //log = null;
            if (_from_beginning)
                _last_iterations = _iterations;
            else
                _last_iterations += _iterations;
        }

        public void Validate(bool from_beginning)
        {
            List<UserAction> actions_to_be_removed = new List<UserAction>();
            Dictionary<UserAction, int> choice = new Dictionary<UserAction, int>();
            Dictionary<UserAction, int> n_choice = new Dictionary<UserAction, int>();
            Dictionary<UserAction, List<FileOpDescriptor>> ops = new Dictionary<UserAction, List<FileOpDescriptor>>(_basic_action_list.Count);
            for (int i = 0; i < _basic_action_list.Count; i++)
            {
                List<Generalization> gens_to_be_removed = new List<Generalization>();
                UserAction action = _basic_action_list[i];
                Generalization last_gen = null;
                try
                {
                    last_gen = _basic_generalizations[action][_basic_generalizations[action].Count - 1];
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("oh noes: " + e.ToString());
                }
                string ext = GetGenExt(last_gen);
                bool choice_added = false;
                int alt = 0;


                for (int n = 0; n < _basic_generalizations[action].Count; n++)
                {
                    Generalization gen = _basic_generalizations[action][n]; 
                    FileOpDescriptor fod = null;
                    bool gen_colapsed = false;

                    if (action.IsType(UserAction.UserActionType.FileCreatedAction) || !action.IsType(UserAction.UserActionType.FileAction))
                    {
                        if (!UserContext.Instance.IsWindowOpen(action.Window))
                            _basic_generalizations[action].Clear();
                    }
                    else
                    {
                        try
                        {
                            fod = GenerateFileOpDescriptor(action, gen, ext, from_beginning);
                            if (fod == null && ext != ".*" && ext != string.Empty)
                                fod = GenerateFileOpDescriptor(action, gen, ".*", from_beginning);
                        }
                        catch //(Exception e)
                        {
                            //System.Windows.Forms.MessageBox.Show(e.ToString());
                        }
                    }

                    if (fod != null)
                    {
                        if (!ops.ContainsKey(action))
                        {
                            List<FileOpDescriptor> fods = new List<FileOpDescriptor>();
                            fods.Add(fod);
                            ops.Add(action, fods);
                        }
                        else
                        {
                            if (!ops[action].Contains(fod))
                                ops[action].Add(fod);
                            else
                                gen_colapsed = true;
                        }
                    }

                    if ((fod == null && action.IsType(UserAction.UserActionType.FileAction)) && !action.IsType(UserAction.UserActionType.FileCreatedAction))
                    {
                        gens_to_be_removed.Add(gen);
                    }
                    else if (!choice_added)
                    {
                        if (!choice.ContainsKey(action))
                        {
                            choice.Add(action, alt++);
                            n_choice.Add(action, 1);
                            choice_added = true;
                        }
                        //else
                        //    System.Windows.Forms.MessageBox.Show("Debug: Dupplicated ID - " + action.Id + " : " + action.QuickDescription);
                    }
                    else if (!gen_colapsed)
                    {
                        n_choice[action]++;
                    }
                }
                foreach (Generalization g in gens_to_be_removed)
                    _basic_generalizations[action].Remove(g);
                if (_basic_generalizations[action].Count == 0)
                    actions_to_be_removed.Add(action);
            }
            foreach (UserAction id in actions_to_be_removed)
                _basic_action_list.RemoveAll(delegate(UserAction ac)
                {
                    return ac == id;
                });

            if (_basic_action_list.Count == 0)
                _valid = false;
            else
                _valid = true;

            _file_op_dscrps = ops;
            _alternatives = choice;
            _n_alternatives = n_choice;
        }

        public string GetSingleActionNarrative(int index)
        {
            return GetSingleActionNarrative(index, false);
        }

        public string GetSingleActionNarrative(int index, bool from_beginning)
        {
            UserActionList list = new UserActionList();
            list.Add(_basic_action_list[index]);
            string narrative;
            if (!_supports_iterations)
                narrative = GenerateNewNarrative(list, 1, from_beginning);
            else
                narrative = GenerateNewNarrative(list, _iterations, from_beginning);
            return narrative;
        }
        #endregion

        #region Private Methods
        private UserActionList GenerateNewActionList(int iteration, bool from_beginning)
        {
            UserActionList list = new UserActionList();
            int choice;
            for (int i = 0; i < _basic_action_list.Count; i++)
            {
                UserAction action = _basic_action_list[i];
                choice = _alternatives[action];
                Generalizations.Generalization gen = _basic_generalizations[action][choice];
                switch (gen.Type)
                {
                    case Generalizations.Generalization.GeneralizationType.TextGeneralization:
                        {
                            TextGeneralization tgen = (TextGeneralization)gen;
                            string text = (from_beginning ? tgen.SolveExpressionFromBeginning(iteration) : tgen.SolveExpression(iteration));
                            TypeTextAction new_action = (TypeTextAction)action.Clone();
                            new_action.Text = text;
                            new_action.Time += new TimeSpan(tgen.Time.Ticks * (iteration - 1));
                            list.Add(new_action);
                            break;
                        }
                    case Generalization.GeneralizationType.MouseGeneralization:
                        {
                            MouseGeneralization mgen = (MouseGeneralization)gen;
                            int x, y;
                            if (mgen.IsSequential)
                            {
                                if (from_beginning)
                                {
                                    x = (mgen.AverageX - mgen.AverageIncX * mgen.Occurrences) + mgen.AverageIncX * (iteration + 1);
                                    y = (mgen.AverageY - mgen.AverageIncY * mgen.Occurrences) + mgen.AverageIncY * (iteration + 1);
                                }
                                else
                                {
                                    x = mgen.AverageX + mgen.AverageIncX * (iteration + 1);
                                    y = mgen.AverageY + mgen.AverageIncY * (iteration + 1);
                                }
                            }
                            else
                            {
                                x = mgen.AverageX;
                                y = mgen.AverageY;
                            }
                            switch (action.ActionType)
                            {
                                case UserAction.UserActionType.MouseClickAction:
                                    {
                                        MouseClickAction maction = (MouseClickAction)action;
                                        MouseClickAction new_action = (MouseClickAction)maction.Clone();
                                        new_action.X = (uint)x;
                                        new_action.Y = (uint)y;
                                        new_action.Time += new TimeSpan(mgen.Time.Ticks * (iteration - 1));
                                        list.Add(new_action);
                                    }
                                    break;
                                case UserAction.UserActionType.MouseDoubleClickAction:
                                    {
                                        MouseDoubleClickAction maction = (MouseDoubleClickAction)action;
                                        MouseDoubleClickAction new_action = (MouseDoubleClickAction)maction.Clone();
                                        new_action.X = (uint)x;
                                        new_action.Y = (uint)y;
                                        new_action.Time += new TimeSpan(mgen.Time.Ticks * (iteration - 1));
                                        list.Add(new_action);
                                    }
                                    break;
                                case UserAction.UserActionType.MouseWheelSpinAction:
                                    {
                                        MouseWheelSpinAction maction = (MouseWheelSpinAction)action;
                                        MouseWheelSpinAction new_action = (MouseWheelSpinAction)maction.Clone();
                                        new_action.X = (uint)x;
                                        new_action.Y = (uint)y;
                                        new_action.Time += new TimeSpan(mgen.Time.Ticks * (iteration - 1));
                                        list.Add(new_action);
                                    }
                                    break;
                            }
                        }
                        break;
                    case Generalization.GeneralizationType.MouseDragGeneralization:
                        {
                            MouseDragGeneralization mgen = (MouseDragGeneralization)gen;
                            MouseDragAction maction = (MouseDragAction)action;
                            MouseDragAction new_action = (MouseDragAction)maction.Clone();
                            new_action.InitialX = (uint)mgen.AverageXi;
                            new_action.InitialY = (uint)mgen.AverageYi;
                            new_action.FinalX = (uint)mgen.AverageXf;
                            new_action.FinalY = (uint)mgen.AverageYf;
                            new_action.Time += new TimeSpan(mgen.Time.Ticks * (iteration - 1));
                            list.Add(new_action);
                        }
                        break;
                    case Generalization.GeneralizationType.KeyGeneralization:
                        {
                            KeyGeneralization kgen = (KeyGeneralization)gen;
                            KeyPressAction new_action = (KeyPressAction)action.Clone(); ;
                            new_action.Time += new TimeSpan(kgen.Time.Ticks * (iteration - 1));
                            list.Add(new_action);
                        }
                        break;
                    case Generalization.GeneralizationType.FileCreateGeneralization:
                        {
                            FileCreateGeneralization fgen = (FileCreateGeneralization)gen;
                            string text = (from_beginning ? fgen.SolveExpressionFromBeginning(iteration) : fgen.SolveExpression(iteration));
                            FileCreatedAction new_action = (FileCreatedAction)action.Clone();
                            new_action.FilePath = (Path.GetDirectoryName(text) == string.Empty ? Path.GetDirectoryName(new_action.FilePath) + @"\" + text : text);
                            new_action.Time += new TimeSpan(fgen.Time.Ticks * (iteration - 1));
                            list.Add(new_action);
                            //if (_file_op_dscrps.Count > 0)
                            //{
                                //for (int j = 0; j < _file_op_dscrps[action][choice].FileList1.Count; j++)
                                //{
                                //    string file = _file_op_dscrps[action][choice].FileList1[j];
                                //    FileCreatedAction new_action = (FileCreatedAction)action.Clone();
                                //    new_action.Time += new TimeSpan(fgen.Time.Ticks * (j + 1));
                                //    new_action.FilePath = file;
                                //    list.Add(new_action);
                                //}
                            //}
                        }
                        break;
                    case Generalization.GeneralizationType.FileDeleteGeneralization:
                        {
                            if (_file_op_dscrps.Count > 0)
                            {
                                FileDeleteGeneralization fgen = (FileDeleteGeneralization)gen;
                                for (int j = 0; j < _file_op_dscrps[action][choice].FileList1.Count; j++)
                                {
                                    string file = _file_op_dscrps[action][choice].FileList1[j];
                                    FileDeletedAction new_action = (FileDeletedAction)action.Clone();
                                    new_action.Time += new TimeSpan(fgen.Time.Ticks * (j + 1));
                                    new_action.FilePath = file;
                                    list.Add(new_action);
                                }
                            }
                        }
                        break;
                    case Generalization.GeneralizationType.FileMoveGeneralization:
                        {
                            if (_file_op_dscrps.Count > 0)
                            {
                                FileMoveGeneralization fgen = (FileMoveGeneralization)gen;
                                FileMovedAction faction = (FileMovedAction)action;
                                for (int j = 0; j < _file_op_dscrps[action][choice].FileList1.Count; j++)
                                {
                                    string folder = faction.Folder;
                                    string file = Path.GetDirectoryName(faction.FileName) + "\\" + Path.GetFileName(_file_op_dscrps[action][choice].FileList1[j]);
                                    FileMovedAction new_action = (FileMovedAction)action.Clone();
                                    new_action.Time += new TimeSpan(fgen.Time.Ticks * (j + 1));
                                    new_action.Folder = folder;
                                    new_action.FileName = file;
                                    list.Add(new_action);
                                }
                            }
                        }
                        break;
                    case Generalization.GeneralizationType.FileRenameGeneralization:
                        {
                            if (_file_op_dscrps.Count > 0)
                            {
                                FileRenameGeneralization fgen = (FileRenameGeneralization)gen;
                                for (int j = 0; j < _file_op_dscrps[action][choice].FileList1.Count; j++)
                                {
                                    string file1 = _file_op_dscrps[action][choice].FileList1[j];
                                    string file2 = _file_op_dscrps[action][choice].FileList2[j];
                                    FileRenamedAction new_action = (FileRenamedAction)action.Clone();
                                    new_action.Time += new TimeSpan(fgen.Time.Ticks * (j + 1));
                                    new_action.OldFile = file1;
                                    new_action.NewFile = file2;
                                    list.Add(new_action);
                                }
                            }
                        }
                        break;
                }
            }
            return list;
        }

        private string GenerateNewNarrative(int max_iterations, bool from_beginning)
        {
            return GenerateNewNarrative(_basic_action_list, max_iterations, from_beginning);
        }

        private string GenerateNewNarrative(UserActionList actions, int max_iterations, bool from_beginning)
        {
            string narrative = string.Empty;
            Window last_window = null;
            int choice;
            bool creating_folder_item = false;
            int it = 1;
            while (it <= max_iterations)
            {
                
                if (it == 5 && it < max_iterations)
                {
                    narrative += "..." + RTFPar();
                }
                else if (it <= 4 || it == max_iterations)
                {
                    for (int i = 0; i < actions.Count; i++)
                    {
                        UserAction action = actions[i];
                        choice = _alternatives[action];
                        Generalizations.Generalization gen = _basic_generalizations[action][choice];
                        if ((action.IsType(UserAction.UserActionType.FileCreatedAction) || !action.IsType(UserAction.UserActionType.FileAction)) && !UserContext.Instance.IsWindowOpen(action.Window))
                        {
                            return string.Empty;
                        }
                        if (last_window != null)
                        {
                            if (last_window.Title != action.Window.Title)
                            {
                                if (!action.IsType(UserAction.UserActionType.FileAction))
                                {
                                    narrative += RTFBoldText("On:") + " window \"" + RTFBoldText(last_window.Title) + "\" (" + last_window.ProcessName + ".exe):" + RTFPar(2);
                                }
                            }
                        }
                        else if (!action.IsType(UserAction.UserActionType.FileAction))
                        {
                            narrative += RTFBoldText("On:")+ " window \"" + RTFBoldText(action.Window.Title) + "\" (" + action.Window.ProcessName + ".exe):" + RTFPar(2);
                        }
                        switch (gen.Type)
                        {
                            case Generalizations.Generalization.GeneralizationType.TextGeneralization:
                                {
                                    TextGeneralization tgen = (TextGeneralization)gen;
                                    string text = (from_beginning ? tgen.SolveExpressionFromBeginning(it) : tgen.SolveExpression(it));
                                    narrative += "Type \"" + GenerateRTFSeqTextNarrativePart(text, tgen.Expression, tgen.Functions, it, from_beginning) + "\"";
                                    creating_folder_item = false;
                                    break;
                                }
                            case Generalization.GeneralizationType.MouseGeneralization:
                                {
                                    MouseGeneralization mgen = (MouseGeneralization)gen;
                                    int x, y;
                                    if (mgen.IsSequential)
                                    {
                                        if (from_beginning)
                                        {
                                            x = (mgen.AverageX - mgen.AverageIncX * mgen.Occurrences) + mgen.AverageIncX * (it + 1);
                                            y = (mgen.AverageY - mgen.AverageIncY * mgen.Occurrences) + mgen.AverageIncY * (it + 1);
                                        }
                                        else
                                        {
                                            x = mgen.AverageX + mgen.AverageIncX * (it + 1);
                                            y = mgen.AverageY + mgen.AverageIncY * (it + 1);
                                        }
                                    }
                                    else
                                    {
                                        x = mgen.AverageX;
                                        y = mgen.AverageY;
                                    }

                                    Window mwnd = null;
                                    switch (action.ActionType)
                                    {
                                        case UserAction.UserActionType.MouseClickAction:
                                            {
                                                MouseClickAction maction = (MouseClickAction)action;
                                                //narrative += maction.Button.ToString() + " mouse click at (x = \\b " + x.ToString() + "\\b0 , y = \\b " + y.ToString() + "\\b0 )";
                                                narrative += RTFBoldText(maction.Button.ToString()) + " mouse click at " + GenerateRTFMousePositionPart(x, y, maction.Window);
                                                mwnd = maction.Window;
                                            }
                                            break;
                                        case UserAction.UserActionType.MouseDoubleClickAction:
                                            {
                                                MouseDoubleClickAction maction = (MouseDoubleClickAction)action;
                                                narrative += RTFBoldText(maction.Button.ToString()) + " mouse double click at " + GenerateRTFMousePositionPart(x, y, maction.Window);
                                                mwnd = maction.Window;
                                            }
                                            break;
                                        case UserAction.UserActionType.MouseWheelSpinAction:
                                            {
                                                MouseWheelSpinAction maction = (MouseWheelSpinAction)action;
                                                narrative += "Spin the mouse wheel " + (maction.Delta >= 0 ? RTFBoldText("Up") : RTFBoldText("Down")) + ", " + Math.Abs(maction.Delta).ToString() + " notches, at " + GenerateRTFMousePositionPart(x, y, maction.Window);
                                                mwnd = maction.Window;
                                            }
                                            break;
                                    }
                                    creating_folder_item = false;
                                }
                                break;
                            case Generalization.GeneralizationType.MouseDragGeneralization:
                                {
                                    MouseDragGeneralization mgen = (MouseDragGeneralization)gen;
                                    MouseDragAction maction = (MouseDragAction)action;
                                    narrative += RTFBoldText(maction.Button.ToString()) + " mouse drag from " + GenerateRTFMousePositionPart((int)maction.InitialX, (int)maction.InitialY, maction.Window) + " to " + GenerateRTFMousePositionPart((int)maction.FinalX, (int)maction.FinalY, maction.Window);
                                    creating_folder_item = false;
                                }
                                break;
                            case Generalization.GeneralizationType.KeyGeneralization:
                                {
                                    KeyPressAction kaction = (KeyPressAction)action;
                                    narrative += "Press " + RTFBoldText((kaction.Modifiers != UserAction.Modifiers.None ? kaction.Modifiers + ", " : string.Empty) + kaction.Key.ToString()) + " key";
                                    creating_folder_item = false;
                                }
                                break;
                            case Generalization.GeneralizationType.FileCreateGeneralization:
                                {
                                    FileCreateGeneralization fgen = (FileCreateGeneralization)gen;
                                    FileCreatedAction faction = (FileCreatedAction)action;
                                    string text = (from_beginning ? fgen.SolveExpressionFromBeginning(it) : fgen.SolveExpression(it));
                                    bool is_constant = (fgen.Functions[0].Type == Function.FunctionType.ConstantTextFunction ? true : false);
                                    if (!creating_folder_item)
                                    {
                                        if (is_constant)
                                            narrative += RTFBoldText("Create:") + " \"" + text + "\"";
                                        else
                                            narrative += RTFBoldText("On:") + " folder " + GenerateRTFDirNarrativePart(faction.Folder) + RTFPar(1) + RTFBoldText("Create:") + RTFPar(2) + "\"" + GenerateRTFSeqTextNarrativePart(text, fgen.Expression, fgen.Functions, it, from_beginning) + "\"";
                                        //narrative += RTFBoldText("Create") + (is_constant ? " \"" + text + "\"" : " " + RTFBoldText("in") + " folder " + GenerateRTFDirNarrativePart(faction.Folder) + ":" + RTFPar(2) + "\"" + GenerateRTFSeqTextNarrativePart(text, fgen.Expression, fgen.Functions, it, from_beginning) + "\"");
                                    }
                                    else
                                    {
                                        narrative += "\"" + GenerateRTFSeqTextNarrativePart(text, fgen.Expression, fgen.Functions, it, from_beginning) + "\"";
                                    }
                                    creating_folder_item = true;
                                }
                                break;
                            case Generalization.GeneralizationType.FileDeleteGeneralization:
                                {
                                    if (_file_op_dscrps.Count > 0)
                                    {
                                        FileDeleteGeneralization fgen = (FileDeleteGeneralization)gen;
                                        FileDeletedAction faction = (FileDeletedAction)action;
                                        //string text = _file_op_dscrps[action].Token;
                                        //narrative += "Delete " + text + " in folder \"" + faction.Folder + "\"";
                                        narrative += GenerateFileOpNarrative(faction, fgen);
                                        creating_folder_item = false;
                                    }
                                }
                                break;
                            case Generalization.GeneralizationType.FileMoveGeneralization:
                                {
                                    if (_file_op_dscrps.Count > 0)
                                    {
                                        FileMoveGeneralization fgen = (FileMoveGeneralization)gen;
                                        FileMovedAction faction = (FileMovedAction)action;
                                        //string text = _file_op_dscrps[action].Token;
                                        //narrative += "Move " + text + " from \"" + faction.Folder + "\" to \"" + Path.GetDirectoryName(faction.FileName) + "\"";
                                        narrative += GenerateFileOpNarrative(faction, fgen);
                                        creating_folder_item = false;
                                    }
                                }
                                break;
                            case Generalization.GeneralizationType.FileRenameGeneralization:
                                {
                                    if (_file_op_dscrps.Count > 0)
                                    {
                                        FileRenameGeneralization fgen = (FileRenameGeneralization)gen;
                                        FileRenamedAction faction = (FileRenamedAction)action;
                                        //string text = _file_op_dscrps[action].Token;
                                        //narrative += "Rename " + text + " in folder \"" + faction.Folder + "\"";
                                        narrative += GenerateFileOpNarrative(faction, fgen);
                                        creating_folder_item = false;
                                    }
                                }
                                break;
                        }
                        last_window = action.Window;
                        if (it == max_iterations && i == actions.Count - 1)
                        {
                            if (!_is_file_system)
                                narrative += ".";
                        }
                        else
                        {
                            if (_is_file_system && !creating_folder_item)
                                narrative += RTFPar(2);
                            else
                                narrative += "," + RTFPar();
                        }
                    }
                }
                it++;
            }
            return narrative;
        }

        private void UpdateFileOpDescriptions(bool from_beginning)
        {
            Dictionary<UserAction, List<FileOpDescriptor>> ops = new Dictionary<UserAction, List<FileOpDescriptor>>(_basic_action_list.Count);
            int choice;
            for (int i = 0; i < _basic_action_list.Count; i++)
            {
                UserAction action = _basic_action_list[i];
                choice = _alternatives[action];
                Generalization last_gen = _basic_generalizations[action][_basic_generalizations[action].Count - 1];
                string ext = GetGenExt(last_gen);
                Generalization gen = _basic_generalizations[action][choice];
                FileOpDescriptor fod = GenerateFileOpDescriptor(action, gen, ext, from_beginning);
                if (fod == null && ext != ".*" && ext != string.Empty)
                    fod = GenerateFileOpDescriptor(action, gen, ".*", from_beginning);
                if (fod != null)
                {
                    List<FileOpDescriptor> fods = new List<FileOpDescriptor>();
                    fods.Add(fod);
                    ops.Add(action, fods);
                }
            }
            _file_op_dscrps = ops;
        }

        private FileOpDescriptor GenerateFileOpDescriptor(UserAction action, Generalization gen, string ext, bool from_beginning)
        {
            List<string> files1 = new List<string>();
            List<string> files2 = new List<string>();
            List<string> rtf1 = new List<string>();
            List<string> rtf2 = new List<string>();

            switch (gen.Type)
            {
                //case Generalization.GeneralizationType.FileCreateGeneralization:
                //    {
                //        FileCreateGeneralization fgen = (FileCreateGeneralization)gen;
                //        FileCreatedAction faction = (FileCreatedAction)action;
                //        if (Directory.Exists(faction.Folder))
                //        {
                //            switch (fgen.Functions[0].Type)
                //            {
                //                //case Function.FunctionType.SequentialIntFunction:
                //                //    {
                //                //        int it = 1;
                //                //        int index = -1;
                //                //        bool stop = false;
                //                //        while (!stop)
                //                //        {
                //                //            string fname = (from_beginning ? fgen.SolveExpressionFromBeginning(it) : fgen.SolveExpression(it));
                //                //            index = dir_noext_files.IndexOf(fname);
                //                //            if (index > -1)
                //                //            {
                //                //                files1.Add(dir_files[index]);
                //                //                rtf1.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(dir_files[index]), fgen.Expression, fgen.Functions, it, from_beginning));
                //                //            }
                //                //            else
                //                //            {
                //                //                stop = true;
                //                //            }
                //                //            it++;
                //                //        }
                //                //    }
                //                //    break;
                //                //case Function.FunctionType.SequentialCharFunction:
                //                //    {
                //                //        int it = 1;
                //                //        int index = -1;
                //                //        bool stop = false;
                //                //        while (!stop)
                //                //        {
                //                //            string fname = (from_beginning ? fgen.SolveExpressionFromBeginning(it) : fgen.SolveExpression(it));
                //                //            index = dir_noext_files.IndexOf(fname);
                //                //            if (index > -1)
                //                //            {
                //                //                files1.Add(dir_files[index]);
                //                //                rtf1.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(dir_files[index]), fgen.Expression, fgen.Functions));
                //                //            }
                //                //            else
                //                //            {
                //                //                stop = true;
                //                //            }
                //                //            it++;
                //                //        }
                //                //    }
                //                //    break;
                //                case Function.FunctionType.ConstantTextFunction:
                //                    {
                //                        string fname = fgen.SolveExpression(1);
                //                        files1.Add(fname);
                //                        rtf1.Add(GenerateRTFNormalizedText(fname));
                //                    }
                //                    break;
                //            }
                //        }
                //    }
                //    break;
                case Generalization.GeneralizationType.FileDeleteGeneralization:
                    {
                        FileDeleteGeneralization fgen = (FileDeleteGeneralization)gen;
                        FileDeletedAction faction = (FileDeletedAction)action;
                        string folder = faction.Folder;
                        folder = Path.GetDirectoryName(faction.FilePath);
                        if (!Directory.Exists(folder)) // isn't a file
                            folder = Directory.GetParent(Path.GetDirectoryName(faction.FilePath)).Name;
                        if (Directory.Exists(folder))
                        {
                            List<string> dir_files;
                            if (ext == string.Empty)
                                dir_files = new List<string>(Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly));
                            else
                                dir_files = new List<string>(Directory.GetFiles(folder, "*" + ext, SearchOption.TopDirectoryOnly));
                            List<string> dir_noext_files = new List<string>();
                            for (int j = 0; j < dir_files.Count; j++)
                            {
                                dir_noext_files.Add(Path.GetFileNameWithoutExtension(dir_files[j]));
                            }

                            switch (fgen.Functions[0].Type)
                            {
                                case Function.FunctionType.SequentialIntFunction:
                                    {
                                        int it = 1;
                                        int index = -1;
                                        bool stop = false;
                                        while (!stop)
                                        {
                                            string fname = (from_beginning ? fgen.SolveExpressionFromBeginning(it) : fgen.SolveExpression(it));
                                            index = dir_noext_files.IndexOf(fname);
                                            if (index > -1)
                                            {
                                                files1.Add(dir_files[index]);
                                                rtf1.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(dir_files[index]), fgen.Expression, fgen.Functions, it, from_beginning));
                                            }
                                            else
                                            {
                                                stop = true;
                                            }
                                            it++;
                                        }
                                    }
                                    break;
                                case Function.FunctionType.SequentialCharFunction:
                                    {
                                        int it = 1;
                                        int index = -1;
                                        bool stop = false;
                                        while (!stop)
                                        {
                                            string fname = (from_beginning ? fgen.SolveExpressionFromBeginning(it) : fgen.SolveExpression(it));
                                            index = dir_noext_files.IndexOf(fname);
                                            if (index > -1)
                                            {
                                                files1.Add(dir_files[index]);
                                                rtf1.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(dir_files[index]), fgen.Expression, fgen.Functions));
                                            }
                                            else
                                            {
                                                stop = true;
                                            }
                                            it++;
                                        }
                                    }
                                    break;
                                case Function.FunctionType.ConstantTextFunction:
                                    {
                                        string fname = fgen.SolveExpression(1);
                                        files1.Add(fname);
                                        rtf1.Add(RTFNormalizedText(fname));
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunction:
                                    {
                                        ConstantFileFunction func = (ConstantFileFunction)fgen.Functions[0];
                                        Regex regex = new Regex(func.Biginning + @".*" + func.Ending, RegexOptions.IgnoreCase);
                                        for (int j = 0; j < dir_noext_files.Count; j++)
                                        {
                                            string file = dir_noext_files[j];
                                            if (regex.IsMatch(file))
                                            {
                                                files1.Add(dir_files[j]);
                                                rtf1.Add(GenerateRTFConstFileNarrativePart(GetFolderItemName(dir_files[j]), func.Biginning.Length, func.Ending.Length));
                                            }
                                        }
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunctionEx:
                                    {
                                        ConstantFileFunctionEx func = (ConstantFileFunctionEx)fgen.Functions[0];
                                        for (int j = 0; j < dir_noext_files.Count; j++)
                                        {
                                            string file = dir_noext_files[j];
                                            bool to_add = true;
                                            foreach (string t in func.Contents)
                                            {
                                                if (!file.ToLower().Contains(t))
                                                {
                                                    to_add = false;
                                                    break;
                                                }
                                            }
                                            if (to_add)
                                            {
                                                files1.Add(dir_files[j]);
                                                rtf1.Add(GenerateRTFConstExFileNarrativePart(GetFolderItemName(dir_files[j]), func.Contents));
                                            }
                                        }
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileExtFunction:
                                    {
                                        ConstantFileExtFunction func = (ConstantFileExtFunction)fgen.Functions[0];
                                        files1.AddRange(dir_files);
                                        foreach (string file in dir_files)
                                            rtf1.Add(RTFNormalizedText(GetFolderItemName(file)));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case Generalization.GeneralizationType.FileMoveGeneralization:
                    {
                        FileMoveGeneralization fgen = (FileMoveGeneralization)gen;
                        FileMovedAction faction = (FileMovedAction)action;
                        string folder = faction.Folder;
                        if (Directory.Exists(folder))
                        {
                            List<string> dir_files;
                            if (ext == string.Empty)
                                dir_files = new List<string>(Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly));
                            else
                                dir_files = new List<string>(Directory.GetFiles(folder, "*" + ext, SearchOption.TopDirectoryOnly));
                            List<string> dir_noext_files = new List<string>();
                            string origin_dir = folder;//Path.GetDirectoryName(faction.FileName);
                            for (int j = 0; j < dir_files.Count; j++)
                            {
                                dir_noext_files.Add(Path.GetFileNameWithoutExtension(dir_files[j]));
                            }

                            switch (fgen.Functions[0].Type)
                            {
                                case Function.FunctionType.SequentialIntFunction:
                                    {
                                        int it = 1;
                                        int index = -1;
                                        bool stop = false;
                                        while (!stop)
                                        {
                                            string fname = (from_beginning ? fgen.SolveExpressionFromBeginning(it) : fgen.SolveExpression(it));
                                            index = dir_noext_files.IndexOf(fname);
                                            if (index > -1)
                                            {
                                                string move = origin_dir + @"\" + Path.GetFileName(dir_files[index]);
                                                files1.Add(move);
                                                rtf1.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(move), fgen.Expression, fgen.Functions, it, from_beginning));
                                            }
                                            else
                                            {
                                                stop = true;
                                            }
                                            it++;
                                        }
                                    }
                                    break;
                                case Function.FunctionType.SequentialCharFunction:
                                    {
                                        int it = 1;
                                        int index = -1;
                                        bool stop = false;
                                        while (!stop)
                                        {
                                            string fname = (from_beginning ? fgen.SolveExpressionFromBeginning(it) : fgen.SolveExpression(it));
                                            index = dir_noext_files.IndexOf(fname);
                                            if (index > -1)
                                            {
                                                string move = origin_dir + @"\" + Path.GetFileName(dir_files[index]);
                                                files1.Add(move);
                                                rtf1.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(move), fgen.Expression, fgen.Functions));
                                            }
                                            else
                                            {
                                                stop = true;
                                            }
                                            it++;
                                        }
                                    }
                                    break;
                                case Function.FunctionType.ConstantTextFunction:
                                    {
                                        string fname = (from_beginning ? fgen.SolveExpressionFromBeginning(1) : fgen.SolveExpression(1));
                                        string move = origin_dir + @"\" + Path.GetFileName(fname);
                                        files1.Add(move);
                                        rtf1.Add(RTFNormalizedText(move));
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunction:
                                    {
                                        ConstantFileFunction func = (ConstantFileFunction)fgen.Functions[0];
                                        Regex regex = new Regex(func.Biginning + @".*" + func.Ending, RegexOptions.IgnoreCase);
                                        for (int j = 0; j < dir_noext_files.Count; j++)
                                        {
                                            string file = dir_noext_files[j];
                                            if (regex.IsMatch(file))
                                            {
                                                string move = origin_dir + @"\" + Path.GetFileName(dir_files[j]);
                                                files1.Add(move);
                                                rtf1.Add(GenerateRTFConstFileNarrativePart(GetFolderItemName(move), func.Biginning.Length, func.Ending.Length));
                                            }
                                        }
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunctionEx:
                                    {
                                        ConstantFileFunctionEx func = (ConstantFileFunctionEx)fgen.Functions[0];
                                        for (int j = 0; j < dir_noext_files.Count; j++)
                                        {
                                            string file = dir_noext_files[j];
                                            bool to_add = true;
                                            foreach (string t in func.Contents)
                                            {
                                                if (!file.ToLower().Contains(t))
                                                {
                                                    to_add = false;
                                                    break;
                                                }
                                            }
                                            if (to_add)
                                            {
                                                string move = origin_dir + @"\" + Path.GetFileName(dir_files[j]);
                                                files1.Add(move);
                                                rtf1.Add(RTFNormalizedText(GetFolderItemName(move)));
                                            }
                                        }
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileExtFunction:
                                    {
                                        ConstantFileExtFunction func = (ConstantFileExtFunction)fgen.Functions[0];
                                        foreach (string file in dir_files)
                                        {
                                            string move = origin_dir + @"\" + Path.GetFileName(file);
                                            files1.Add(move);
                                            rtf1.Add(RTFNormalizedText(GetFolderItemName(move)));
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case Generalization.GeneralizationType.FileRenameGeneralization:
                    {
                        FileRenameGeneralization fgen = (FileRenameGeneralization)gen;
                        FileRenamedAction faction = (FileRenamedAction)action;
                        string folder = faction.Folder;
                        folder = Path.GetDirectoryName(faction.OldFile);
                        if (!Directory.Exists(folder)) // isn't a file
                            folder = Directory.GetParent(Path.GetDirectoryName(faction.OldFile)).Name;
                        if (Directory.Exists(folder))
                        {
                            List<string> dir_files;
                            if (ext == string.Empty)
                                dir_files = new List<string>(Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly));
                            else
                                dir_files = new List<string>(Directory.GetFiles(folder, "*" + ext, SearchOption.TopDirectoryOnly));
                            List<string> dir_noext_files = new List<string>();
                            string dir_file;
                            List<int> to_remove = new List<int>();
                            for (int j = 0; j < dir_files.Count; j++)
                            {
                                dir_file = Path.GetFileNameWithoutExtension(dir_files[j]);
                                if (!fgen.PastFiles.Contains(dir_file))
                                    dir_noext_files.Add(Path.GetFileNameWithoutExtension(dir_file));
                                else
                                    to_remove.Add(j);
                            }
                            for (int m = to_remove.Count - 1; m >= 0; m--)
                                dir_files.RemoveAt(to_remove[m]);

                            switch (fgen.OldNameFunctions[0].Type)
                            {
                                case Function.FunctionType.SequentialIntFunction:
                                    {
                                        int it = 1;
                                        int index = -1;
                                        bool stop = false;
                                        while (!stop)
                                        {
                                            string fname1 = (from_beginning ? fgen.SolveOldExpressionFromBeginning(it) : fgen.SolveOldExpression(it));
                                            index = dir_noext_files.IndexOf(fname1);
                                            if (index > -1)
                                            {
                                                string fname2 = string.Empty;
                                                switch (fgen.NewNameFunctions[0].Type)
                                                {
                                                    case Function.FunctionType.SequentialIntFunction:
                                                        fname2 = Path.GetDirectoryName(dir_files[index]) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it)) + Path.GetExtension(dir_files[index]);
                                                        rtf1.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(fname1), fgen.OldNameExpression, fgen.OldNameFunctions, it, from_beginning));
                                                        rtf2.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameExpression, fgen.NewNameFunctions, it, from_beginning));
                                                        break;
                                                    case Function.FunctionType.SequentialCharFunction:
                                                        fname2 = Path.GetDirectoryName(dir_files[index]) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it)) + Path.GetExtension(dir_files[index]);
                                                        rtf1.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(fname1), fgen.OldNameExpression, fgen.OldNameFunctions, it, from_beginning));
                                                        rtf2.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameExpression, fgen.NewNameFunctions));
                                                        break;
                                                    case Function.FunctionType.ConstantTextFunction:
                                                        fname2 = (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it));
                                                        rtf1.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(fname1), fgen.OldNameExpression, fgen.OldNameFunctions, it, from_beginning));
                                                        rtf2.Add(RTFNormalizedText(GetFolderItemName(fname2)));
                                                        break;
                                                    case Function.FunctionType.ConstantFileDiffFunction:
                                                        ConstantFileDiffFunction func = (ConstantFileDiffFunction)fgen.NewNameFunctions[0];
                                                        string tmp_name = fname1;
                                                        for (int n = func.ReplacementPositions.Length - 1; n >= 0; n--)
                                                        {
                                                            int ipos = func.OriginalPositions[n];
                                                            int fpos = func.OriginalPositions[n] + func.OriginalTokens[n].Length - 1;
                                                            string to_be_inserted = func.ReplacementTokens[n];
                                                            if (fpos - ipos == -1) // insertion instead of replacement
                                                                tmp_name = tmp_name.Insert(ipos, to_be_inserted);
                                                            else
                                                                tmp_name = SystemCore.SystemAbstraction.StringUtilities.StringUtility.ReplaceBetweenPositions(tmp_name, ipos, fpos, to_be_inserted);
                                                        }
                                                        fname2 = Path.GetDirectoryName(dir_files[index]) + @"\" + tmp_name + Path.GetExtension(dir_files[index]);
                                                        rtf1.Add(GenerateRTFDiffOriginalFileNarrativePart(GetFolderItemName(dir_files[index]), fgen.NewNameFunctions));
                                                        rtf2.Add(GenerateRTFDiffReplacementFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameFunctions));
                                                        break;
                                                }

                                                files1.Add(dir_files[index]);
                                                files2.Add(fname2);
                                            }
                                            else
                                            {
                                                stop = true;
                                            }
                                            it++;
                                        }
                                    }
                                    break;
                                case Function.FunctionType.SequentialCharFunction:
                                    {
                                        int it = 1;
                                        int index = -1;
                                        bool stop = false;
                                        while (!stop)
                                        {
                                            string fname1 = (from_beginning ? fgen.SolveOldExpressionFromBeginning(it) : fgen.SolveOldExpression(it));
                                            index = dir_noext_files.IndexOf(fname1);
                                            if (index > -1)
                                            {
                                                string fname2 = string.Empty;
                                                switch (fgen.NewNameFunctions[0].Type)
                                                {
                                                    case Function.FunctionType.SequentialIntFunction:
                                                        fname2 = Path.GetDirectoryName(dir_files[index]) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it)) + Path.GetExtension(dir_files[index]);
                                                        rtf1.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(fname1), fgen.OldNameExpression, fgen.OldNameFunctions));
                                                        rtf2.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameExpression, fgen.NewNameFunctions, it, from_beginning));
                                                        break;
                                                    case Function.FunctionType.SequentialCharFunction:
                                                        fname2 = Path.GetDirectoryName(dir_files[index]) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it)) + Path.GetExtension(dir_files[index]);
                                                        rtf1.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(fname1), fgen.OldNameExpression, fgen.OldNameFunctions));
                                                        rtf2.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameExpression, fgen.NewNameFunctions));
                                                        break;
                                                    case Function.FunctionType.ConstantTextFunction:
                                                        fname2 = (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it));
                                                        rtf1.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(fname1), fgen.OldNameExpression, fgen.OldNameFunctions));
                                                        rtf2.Add(RTFNormalizedText(GetFolderItemName(fname2)));
                                                        break;
                                                    case Function.FunctionType.ConstantFileDiffFunction:
                                                        ConstantFileDiffFunction func = (ConstantFileDiffFunction)fgen.NewNameFunctions[0];
                                                        string tmp_name = fname1;
                                                        for (int n = func.ReplacementPositions.Length - 1; n >= 0; n--)
                                                        {
                                                            int ipos = func.OriginalPositions[n];
                                                            int fpos = func.OriginalPositions[n] + func.OriginalTokens[n].Length - 1;
                                                            string to_be_inserted = func.ReplacementTokens[n];
                                                            if (fpos - ipos == -1) // insertion instead of replacement
                                                                tmp_name = tmp_name.Insert(ipos, to_be_inserted);
                                                            else
                                                                tmp_name = SystemCore.SystemAbstraction.StringUtilities.StringUtility.ReplaceBetweenPositions(tmp_name, ipos, fpos, to_be_inserted);
                                                        }
                                                        fname2 = Path.GetDirectoryName(dir_files[index]) + @"\" + tmp_name + Path.GetExtension(dir_files[index]);
                                                        rtf1.Add(GenerateRTFDiffOriginalFileNarrativePart(GetFolderItemName(dir_files[index]), fgen.NewNameFunctions));
                                                        rtf2.Add(GenerateRTFDiffReplacementFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameFunctions));
                                                        break;
                                                }

                                                files1.Add(dir_files[index]);
                                                files2.Add(fname2);
                                            }
                                            else
                                            {
                                                stop = true;
                                            }
                                            it++;
                                        }
                                    }
                                    break;
                                case Function.FunctionType.ConstantTextFunction:
                                    {
                                        string fname1 = (from_beginning ? fgen.SolveOldExpressionFromBeginning(1) : fgen.SolveOldExpression(1));
                                        string fname2 = string.Empty;
                                        switch (fgen.NewNameFunctions[0].Type)
                                        {
                                            case Function.FunctionType.SequentialIntFunction:
                                                fname2 = Path.GetDirectoryName(fname1) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(1) : fgen.SolveNewExpression(1)) + Path.GetExtension(fname1);
                                                rtf1.Add(RTFNormalizedText(GetFolderItemName(fname1)));
                                                rtf2.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(fname2), fgen.OldNameExpression, fgen.OldNameFunctions, 1, from_beginning));
                                                break;
                                            case Function.FunctionType.SequentialCharFunction:
                                                fname2 = Path.GetDirectoryName(fname1) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(1) : fgen.SolveNewExpression(1)) + Path.GetExtension(fname1);
                                                rtf1.Add(RTFNormalizedText(GetFolderItemName(fname1)));
                                                rtf2.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(fname2), fgen.OldNameExpression, fgen.OldNameFunctions));
                                                break;
                                            case Function.FunctionType.ConstantTextFunction:
                                                fname2 = (from_beginning ? fgen.SolveNewExpressionFromBeginning(1) : fgen.SolveNewExpression(1));
                                                rtf1.Add(RTFNormalizedText(GetFolderItemName(fname1)));
                                                rtf2.Add(RTFNormalizedText(GetFolderItemName(fname2)));
                                                break;
                                            case Function.FunctionType.ConstantFileDiffFunction:
                                                ConstantFileDiffFunction func = (ConstantFileDiffFunction)fgen.NewNameFunctions[0];
                                                string tmp_name = Path.GetFileNameWithoutExtension(fname1);
                                                for (int n = func.ReplacementPositions.Length - 1; n >= 0; n--)
                                                {
                                                    int ipos = func.OriginalPositions[n];
                                                    int fpos = func.OriginalPositions[n] + func.OriginalTokens[n].Length - 1;
                                                    string to_be_inserted = func.ReplacementTokens[n];
                                                    if (fpos - ipos == -1) // insertion instead of replacement
                                                        tmp_name = tmp_name.Insert(ipos, to_be_inserted);
                                                    else
                                                        tmp_name = SystemCore.SystemAbstraction.StringUtilities.StringUtility.ReplaceBetweenPositions(tmp_name, ipos, fpos, to_be_inserted);
                                                }
                                                fname2 = Path.GetDirectoryName(fname1) + @"\" + tmp_name + Path.GetExtension(fname1);
                                                rtf1.Add(GenerateRTFDiffOriginalFileNarrativePart(GetFolderItemName(fname1), fgen.NewNameFunctions));
                                                rtf2.Add(GenerateRTFDiffReplacementFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameFunctions));
                                                break;
                                        }
                                        files1.Add(fname1);
                                        files2.Add(fname2);
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunction:
                                    {
                                        ConstantFileFunction func = (ConstantFileFunction)fgen.OldNameFunctions[0];
                                        Regex regex = new Regex(func.Biginning + @".*" + func.Ending, RegexOptions.IgnoreCase);
                                        int it = 1;
                                        for (int j = 0; j < dir_noext_files.Count; j++)
                                        {
                                            string file = dir_noext_files[j];
                                            if (regex.IsMatch(file))
                                            {
                                                string fname1 = dir_files[j];
                                                string fname2 = string.Empty;
                                                switch (fgen.NewNameFunctions[0].Type)
                                                {
                                                    case Function.FunctionType.SequentialIntFunction:
                                                        fname2 = Path.GetDirectoryName(fname1) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it)) + Path.GetExtension(fname1);
                                                        rtf1.Add(GenerateRTFConstFileNarrativePart(GetFolderItemName(fname1), func.Biginning.Length, func.Ending.Length));
                                                        rtf2.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameExpression, fgen.NewNameFunctions, it, from_beginning));
                                                        break;
                                                    case Function.FunctionType.SequentialCharFunction:
                                                        fname2 = Path.GetDirectoryName(fname1) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it)) + Path.GetExtension(fname1);
                                                        rtf1.Add(GenerateRTFConstFileNarrativePart(GetFolderItemName(fname1), func.Biginning.Length, func.Ending.Length));
                                                        rtf2.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameExpression, fgen.NewNameFunctions));
                                                        break;
                                                    case Function.FunctionType.ConstantTextFunction:
                                                        fname2 = (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it));
                                                        rtf1.Add(GenerateRTFConstFileNarrativePart(GetFolderItemName(fname1), func.Biginning.Length, func.Ending.Length));
                                                        rtf2.Add(RTFNormalizedText(GetFolderItemName(fname2)));
                                                        break;
                                                    case Function.FunctionType.ConstantFileDiffFunction:
                                                        ConstantFileDiffFunction funcd = (ConstantFileDiffFunction)fgen.NewNameFunctions[0];
                                                        string tmp_name = file;
                                                        for (int n = funcd.ReplacementPositions.Length - 1; n >= 0; n--)
                                                        {
                                                            int ipos = funcd.OriginalPositions[n];
                                                            int fpos = funcd.OriginalPositions[n] + funcd.OriginalTokens[n].Length - 1;
                                                            string to_be_inserted = funcd.ReplacementTokens[n];
                                                            if (fpos - ipos == -1) // insertion instead of replacement
                                                                tmp_name = tmp_name.Insert(ipos, to_be_inserted);
                                                            else
                                                                tmp_name = SystemCore.SystemAbstraction.StringUtilities.StringUtility.ReplaceBetweenPositions(tmp_name, ipos, fpos, to_be_inserted);
                                                        }
                                                        fname2 = Path.GetDirectoryName(fname1) + @"\" + tmp_name + Path.GetExtension(fname1);
                                                        rtf1.Add(GenerateRTFDiffOriginalFileNarrativePart(GetFolderItemName(fname1), fgen.NewNameFunctions));
                                                        rtf2.Add(GenerateRTFDiffReplacementFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameFunctions));
                                                        break;
                                                }
                                                files1.Add(fname1);
                                                files2.Add(fname2);
                                                it++;
                                            }
                                        }
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunctionEx:
                                    {
                                        ConstantFileFunctionEx func = (ConstantFileFunctionEx)fgen.OldNameFunctions[0];
                                        int it = 1;
                                        for (int j = 0; j < dir_noext_files.Count; j++)
                                        {
                                            string file = dir_noext_files[j];
                                            bool to_add = true;
                                            foreach (string t in func.Contents)
                                            {
                                                if (!file.ToLower().Contains(t))
                                                {
                                                    to_add = false;
                                                    break;
                                                }
                                            }
                                            if (to_add)
                                            {
                                                string fname1 = dir_files[j];
                                                string fname2 = string.Empty;
                                                switch (fgen.NewNameFunctions[0].Type)
                                                {
                                                    case Function.FunctionType.SequentialIntFunction:
                                                        fname2 = Path.GetDirectoryName(fname1) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it)) + Path.GetExtension(fname1);
                                                        rtf1.Add(GenerateRTFConstExFileNarrativePart(GetFolderItemName(fname1), func.Contents));
                                                        rtf2.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameExpression, fgen.NewNameFunctions, it, from_beginning));
                                                        break;
                                                    case Function.FunctionType.SequentialCharFunction:
                                                        fname2 = Path.GetDirectoryName(fname1) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it)) + Path.GetExtension(fname1);
                                                        rtf1.Add(GenerateRTFConstExFileNarrativePart(GetFolderItemName(fname1), func.Contents));
                                                        rtf2.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameExpression, fgen.NewNameFunctions));
                                                        break;
                                                    case Function.FunctionType.ConstantTextFunction:
                                                        fname2 = (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it));
                                                        rtf1.Add(GenerateRTFConstExFileNarrativePart(GetFolderItemName(fname1), func.Contents));
                                                        rtf2.Add(RTFNormalizedText(GetFolderItemName(fname2)));
                                                        break;
                                                    case Function.FunctionType.ConstantFileDiffFunction:
                                                        ConstantFileDiffFunction funcd = (ConstantFileDiffFunction)fgen.NewNameFunctions[0];
                                                        string tmp_name = file;
                                                        for (int n = funcd.ReplacementPositions.Length - 1; n >= 0; n--)
                                                        {
                                                            int ipos = funcd.OriginalPositions[n];
                                                            int fpos = funcd.OriginalPositions[n] + funcd.OriginalTokens[n].Length - 1;
                                                            string to_be_inserted = funcd.ReplacementTokens[n];
                                                            if (fpos - ipos == -1) // insertion instead of replacement
                                                                tmp_name = tmp_name.Insert(ipos, to_be_inserted);
                                                            else
                                                                tmp_name = SystemCore.SystemAbstraction.StringUtilities.StringUtility.ReplaceBetweenPositions(tmp_name, ipos, fpos, to_be_inserted);
                                                        }
                                                        fname2 = Path.GetDirectoryName(fname1) + @"\" + tmp_name + Path.GetExtension(fname1);
                                                        rtf1.Add(GenerateRTFDiffOriginalFileNarrativePart(GetFolderItemName(fname1), fgen.NewNameFunctions));
                                                        rtf2.Add(GenerateRTFDiffReplacementFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameFunctions));
                                                        break;
                                                }
                                                files1.Add(fname1);
                                                files2.Add(fname2);
                                                it++;
                                            }
                                        }
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileExtFunction:
                                    {
                                        ConstantFileExtFunction func = (ConstantFileExtFunction)fgen.OldNameFunctions[0];
                                        int it = 1;
                                        for (int j = 0; j < dir_files.Count; j++)
                                        {
                                            string fname1 = dir_files[j];
                                            string fname2 = string.Empty;
                                            switch (fgen.NewNameFunctions[0].Type)
                                            {
                                                case Function.FunctionType.SequentialIntFunction:
                                                    fname2 = Path.GetDirectoryName(fname1) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it)) + Path.GetExtension(fname1);
                                                    rtf1.Add(RTFNormalizedText(GetFolderItemName(fname1)));
                                                    rtf2.Add(GenerateRTFSeqIntFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameExpression, fgen.NewNameFunctions, it, from_beginning));
                                                    break;
                                                case Function.FunctionType.SequentialCharFunction:
                                                    fname2 = Path.GetDirectoryName(fname1) + @"\" + (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it)) + Path.GetExtension(fname1);
                                                    rtf1.Add(RTFNormalizedText(GetFolderItemName(fname1)));
                                                    rtf2.Add(GenerateRTFSeqCharFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameExpression, fgen.NewNameFunctions));
                                                    break;
                                                case Function.FunctionType.ConstantTextFunction:
                                                    fname2 = (from_beginning ? fgen.SolveNewExpressionFromBeginning(it) : fgen.SolveNewExpression(it));
                                                    rtf1.Add(RTFNormalizedText(GetFolderItemName(fname1)));
                                                    rtf2.Add(RTFNormalizedText(GetFolderItemName(fname2)));
                                                    break;
                                                case Function.FunctionType.ConstantFileDiffFunction:
                                                    ConstantFileDiffFunction funcd = (ConstantFileDiffFunction)fgen.NewNameFunctions[0];
                                                    string tmp_name = Path.GetFileNameWithoutExtension(fname1);
                                                    for (int n = funcd.ReplacementPositions.Length - 1; n >= 0; n--)
                                                    {
                                                        int ipos = funcd.OriginalPositions[n];
                                                        int fpos = funcd.OriginalPositions[n] + funcd.OriginalTokens[n].Length - 1;
                                                        string to_be_inserted = funcd.ReplacementTokens[n];
                                                        if (fpos - ipos == -1) // insertion instead of replacement
                                                            tmp_name = tmp_name.Insert(ipos, to_be_inserted);
                                                        else
                                                            tmp_name = SystemCore.SystemAbstraction.StringUtilities.StringUtility.ReplaceBetweenPositions(tmp_name, ipos, fpos, to_be_inserted);
                                                    }
                                                    fname2 = Path.GetDirectoryName(fname1) + @"\" + tmp_name + Path.GetExtension(fname1);
                                                    rtf1.Add(GenerateRTFDiffOriginalFileNarrativePart(GetFolderItemName(fname1), fgen.NewNameFunctions));
                                                    rtf2.Add(GenerateRTFDiffReplacementFileNarrativePart(GetFolderItemName(fname2), fgen.NewNameFunctions));
                                                    break;
                                            }
                                            files1.Add(fname1);
                                            files2.Add(fname2);
                                            it++;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            if (files1.Count > 0)
            {
                FileOpDescriptor fod = new FileOpDescriptor();
                fod.FileList1 = files1;
                fod.RtfList1 = rtf1;
                fod.Extension = ext;
                if (files2.Count > 0)
                {
                    fod.FileList2 = files2;
                    fod.RtfList2 = rtf2;
                }
                else
                {
                    fod.FileList2 = null;
                    fod.RtfList2 = null;
                }
                return fod;
            }
            else
            {
                return null;
            }
        }

        private string GenerateFileOpNarrative(UserAction action, Generalization gen)
        {
            int choice = _alternatives[action];
            FileOpDescriptor fod = _file_op_dscrps[action][choice];
            Generalization last_gen = _basic_generalizations[action][_basic_generalizations[action].Count - 1];
            //string ext = GetGenExt(last_gen);
            string narrative = string.Empty;
            switch (gen.Type)
            {
                case Generalization.GeneralizationType.FileCreateGeneralization:
                    {
                        FileCreateGeneralization fgen = (FileCreateGeneralization)gen;
                        FileCreatedAction faction = (FileCreatedAction)action;
                        if (Directory.Exists(faction.Folder))
                        {
                            switch (fgen.Functions[0].Type)
                            {
                                case Function.FunctionType.ConstantTextFunction:
                                    {
                                        string temp_token = GenerateSeqFileNarrativePart(action);
                                        if (temp_token != string.Empty)
                                            narrative += "Create " + temp_token;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case Generalization.GeneralizationType.FileDeleteGeneralization:
                    {
                        FileDeleteGeneralization fgen = (FileDeleteGeneralization)gen;
                        FileDeletedAction faction = (FileDeletedAction)action;
                        string temp_token = string.Empty;
                        if (Directory.Exists(faction.Folder))
                        {
                            switch (fgen.Functions[0].Type)
                            {
                                case Function.FunctionType.SequentialIntFunction:
                                    {
                                        temp_token += GenerateSeqFileNarrativePart(action);
                                    }
                                    break;
                                case Function.FunctionType.SequentialCharFunction:
                                    {
                                        temp_token += GenerateSeqFileNarrativePart(action);
                                    }
                                    break;
                                case Function.FunctionType.ConstantTextFunction:
                                    {
                                        temp_token += GenerateSeqFileNarrativePart(action);
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunction:
                                    {
                                        ConstantFileFunction func = (ConstantFileFunction)fgen.Functions[0];
                                        temp_token += GenerateConstFileNarrativePart(action, func.Biginning, func.Ending);
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunctionEx:
                                    {
                                        ConstantFileFunctionEx func = (ConstantFileFunctionEx)fgen.Functions[0];
                                        temp_token += GenerateConstExFileNarrativePart(action, func.Contents);
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileExtFunction:
                                    {
                                        ConstantFileExtFunction func = (ConstantFileExtFunction)fgen.Functions[0];
                                        temp_token += GenerateConstFileExtNarrativePart(action);
                                    }
                                    break;
                            }
                        }
                        if (temp_token != string.Empty)
                            narrative += RTFBoldText("On:") + " folder " + GenerateRTFDirNarrativePart(faction.Folder) + RTFPar(1) + RTFBoldText("Delete:") + temp_token;
                        //narrative += "Delete, " + RTFBoldText("from") + " folder " + GenerateRTFDirNarrativePart(faction.Folder) + "" + temp_token;
                    }
                    break;
                case Generalization.GeneralizationType.FileMoveGeneralization:
                    {
                        FileMoveGeneralization fgen = (FileMoveGeneralization)gen;
                        FileMovedAction faction = (FileMovedAction)action;
                        string temp_token = string.Empty;
                        if (Directory.Exists(faction.Folder))
                        {
                            switch (fgen.Functions[0].Type)
                            {
                                case Function.FunctionType.SequentialIntFunction:
                                    {
                                        temp_token += GenerateSeqFileNarrativePart(action);
                                    }
                                    break;
                                case Function.FunctionType.SequentialCharFunction:
                                    {
                                        temp_token += GenerateSeqFileNarrativePart(action);
                                    }
                                    break;
                                case Function.FunctionType.ConstantTextFunction:
                                    {
                                        temp_token += GenerateSeqFileNarrativePart(action);
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunction:
                                    {
                                        ConstantFileFunction func = (ConstantFileFunction)fgen.Functions[0];
                                        temp_token += GenerateConstFileNarrativePart(action, func.Biginning, func.Ending);
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunctionEx:
                                    {
                                        ConstantFileFunctionEx func = (ConstantFileFunctionEx)fgen.Functions[0];
                                        temp_token += GenerateConstExFileNarrativePart(action, func.Contents);
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileExtFunction:
                                    {
                                        ConstantFileExtFunction func = (ConstantFileExtFunction)fgen.Functions[0];
                                        temp_token += GenerateConstFileExtNarrativePart(action);
                                    }
                                    break;
                            }
                        }
                        if (temp_token != string.Empty)
                            narrative += RTFBoldText("From:") + " folder " + GenerateRTFDirNarrativePart(faction.Folder) + RTFPar(1) + RTFBoldText("To:") + " folder " + GenerateRTFDirNarrativePart(Path.GetDirectoryName(faction.FileName)) + RTFPar(1) + RTFBoldText("Move:") + temp_token;
                        //narrative += "Move, " + RTFBoldText("from") + " " + GenerateRTFDirNarrativePart(faction.Folder) + " " + RTFBoldText("to") + " " + GenerateRTFDirNarrativePart(Path.GetDirectoryName(faction.FileName)) + "" + temp_token;
                    }
                    break;
                case Generalization.GeneralizationType.FileRenameGeneralization:
                    {
                        FileRenameGeneralization fgen = (FileRenameGeneralization)gen;
                        FileRenamedAction faction = (FileRenamedAction)action;
                        string temp_token = string.Empty;
                        if (Directory.Exists(faction.Folder))
                        {
                            switch (fgen.OldNameFunctions[0].Type)
                            {
                                case Function.FunctionType.SequentialIntFunction:
                                    {
                                        temp_token += GenerateSeqFileNarrativePart(action);
                                    }
                                    break;
                                case Function.FunctionType.SequentialCharFunction:
                                    {
                                        temp_token += GenerateSeqFileNarrativePart(action);
                                    }
                                    break;
                                case Function.FunctionType.ConstantTextFunction:
                                    {
                                        temp_token += GenerateSeqFileNarrativePart(action);
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunction:
                                    {
                                        ConstantFileFunction func = (ConstantFileFunction)fgen.OldNameFunctions[0];
                                        temp_token += GenerateConstFileNarrativePart(action, func.Biginning, func.Ending);
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunctionEx:
                                    {
                                        ConstantFileFunctionEx func = (ConstantFileFunctionEx)fgen.OldNameFunctions[0];
                                        temp_token += GenerateConstExFileNarrativePart(action, func.Contents);
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileExtFunction:
                                    {
                                        ConstantFileExtFunction func = (ConstantFileExtFunction)fgen.OldNameFunctions[0];
                                        temp_token += GenerateConstFileExtNarrativePart(action);
                                    }
                                    break;
                            }
                        }
                        if (temp_token != string.Empty)
                            narrative += RTFBoldText("On:") + " folder " + GenerateRTFDirNarrativePart(faction.Folder) + RTFPar(1) + RTFBoldText("Rename:") + temp_token;
                        //narrative += "Rename, " + RTFBoldText("in") + " folder " + GenerateRTFDirNarrativePart(faction.Folder) + "" + temp_token;
                    }
                    break;
            }
            return narrative;
        }

        //
        // Narrative Methods
        //
        private string GenerateSeqFileNarrativePart(UserAction action)
        {
            string narrative = string.Empty;
            int choice = _alternatives[action];
            List<string> files1 = _file_op_dscrps[action][choice].RtfList1;
            List<string> files2 = _file_op_dscrps[action][choice].RtfList2;
            if (files1.Count > 0)
            {
                narrative += RTFPar(2);
                narrative += GenerateFileEnumNarrativePart(files1, files2);
            }
            return narrative;
        }

        private string GenerateConstFileNarrativePart(UserAction action, string beginning, string ending)
        {
            string narrative = string.Empty;
            int choice = _alternatives[action];
            string ext = _file_op_dscrps[action][choice].Extension;
            List<string> files1 = _file_op_dscrps[action][choice].RtfList1;
            List<string> files2 = _file_op_dscrps[action][choice].RtfList2;
            if (files1.Count > 0)
            {
                bool is_file = File.Exists(_file_op_dscrps[action][choice].FileList1[0]);
                if (beginning != string.Empty && ending != string.Empty)
                    narrative += " all remaining " + (is_file ? "files" : "folders") + " beginning with \"" + RTFBoldText(beginning) + "\" and ending with \"" + RTFBoldText(ending) + "\"" + GenerateFileExtNarrativePart(ext);
                else if (beginning != string.Empty)
                    narrative += " all remaining " + (is_file ? "files" : "folders") + " beginning with \"" + RTFBoldText(beginning) + "\"" + GenerateFileExtNarrativePart(ext);
                else if (ending != string.Empty)
                    narrative += " all remaining " + (is_file ? "files" : "folders") + " ending with \"" + RTFBoldText(ending) + "\"" + GenerateFileExtNarrativePart(ext);
                narrative += ", such as:" + RTFPar(2) + GenerateFileEnumNarrativePart(files1, files2);
            }
            return narrative;
        }

        private string GenerateConstExFileNarrativePart(UserAction action, string[] contents)
        {
            string narrative = string.Empty;
            int choice = _alternatives[action];
            string ext = _file_op_dscrps[action][choice].Extension;
            List<string> files1 = _file_op_dscrps[action][choice].RtfList1;
            List<string> files2 = _file_op_dscrps[action][choice].RtfList2;
            if (files1.Count > 0)
            {
                bool is_file = File.Exists(_file_op_dscrps[action][choice].FileList1[0]);
                narrative += " all remaining " + (is_file ? "files" : "folders") + " containing \\{" + RTFArrayToStringBold(contents) + "\\}" + GenerateFileExtNarrativePart(ext);
                narrative += ", such as:" + RTFPar(2) + GenerateFileEnumNarrativePart(files1, files2);
            }
            return narrative;
        }

        private string GenerateConstFileExtNarrativePart(UserAction action)
        {
            string narrative = string.Empty;
            int choice = _alternatives[action];
            string ext = _file_op_dscrps[action][choice].Extension;
            List<string> files1 = _file_op_dscrps[action][choice].RtfList1;
            List<string> files2 = _file_op_dscrps[action][choice].RtfList2;
            if (files1.Count > 0)
            {
                bool is_file = File.Exists(_file_op_dscrps[action][choice].FileList1[0]);
                narrative += " all remaining " + (is_file ? "files" : "folders") + GenerateFileExtNarrativePart(ext);
                narrative += ", such as:" + RTFPar(2) + GenerateFileEnumNarrativePart(files1, files2); ;
            }
            return narrative;
        }

        private string GenerateFileExtNarrativePart(string ext)
        {
            string narrative = string.Empty;
            if (ext != ".*" && ext != string.Empty)
            {
                narrative += ", with extension " + ext;
            }
            return narrative;
        }

        private string GenerateFileEnumNarrativePart(List<string> files1, List<string> files2)
        {
            string narrative = string.Empty;
            if (files2 == null)
            {
                string file;
                for (int i = 0; i < files1.Count; i++)
                {
                    file = files1[i];
                    if (i == 0)
                        narrative += "\"" + file + "\"";
                    else
                        narrative += "," + RTFPar() + "\"" + file + "\"";
                    //if (i == 0)
                    //    narrative += "\"" + file + "\"";
                    //else if (i == files1.Count - 1 && i > 0)
                    //    narrative += RTFPar() + "\"" + file + "\""; // and
                    //else if (i < 4)
                    //    narrative += ","  + RTFPar() + "\"" + file + "\"";
                    //else if (i == 4 && i < files1.Count - 1)
                    //    narrative += ","  + RTFPar() + "...";
                }
            }
            else
            {
                for (int i = 0; i < files1.Count; i++)
                {
                    string file1 = files1[i];
                    string file2 = files2[i];
                    if (i == 0)
                        narrative += "\"" + file1 + "\" to \"" + file2 + "\"";
                    else
                        narrative += "," + RTFPar() + "\"" + file1 + "\" to \"" + file2 + "\"";
                    //if (i == 0)
                    //    narrative += "\"" + file1 + "\" to \"" + file2 + "\"";
                    //else if (i == files1.Count - 1 && i > 0)
                    //    narrative += RTFPar() + "\"" + file1 + "\" to \"" + file2 + "\""; // and
                    //else if (i < 4)
                    //    narrative += "," + RTFPar() + "\"" + file1 + "\" to \"" + file2 + "\"";
                    //else if (i == 4 && i < files1.Count - 1)
                    //    narrative += "," + RTFPar() + "...";
                }
            }
            return narrative;
        }

        //
        // RTF Methods
        //
        private string RTFNormalizedText(string text)
        {
            return text.Replace(@"\", @"\\").Replace(@"{", @"\{").Replace(@"}", @"\}");
        }

        private string RTFBoldText(string text)
        {
            return @"\b " + text + @"\b0 ";
        }

        private string RTFItalicText(string text)
        {
            return @"\i " + text + @"\i0 ";
        }

        private string RTFPar()
        {
            return RTFPar(1);
        }

        private string RTFPar(int count)
        {
            string par = string.Empty;
            for (int i = 0; i < count; i++)
                par += @"\par" + Environment.NewLine;
            return par;
        }

        private string GenerateRTFDirNarrativePart(string path)
        {
            return @"\i " + Path.GetDirectoryName((path[path.Length-1] != '\\' ? path + "\\" : path)) .Replace(@"\", @"\\") + @"\i0 ";
        }

        private string GenerateRTFSeqTextNarrativePart(string text, string expression, Function[] funcs, int iteration, bool from_beginning)
        {
            string narrative = string.Empty;
            if (funcs.Length > 0)
            {
                switch (funcs[0].Type)
                {
                    case Function.FunctionType.SequentialIntFunction:
                        narrative = GenerateRTFSeqIntFileNarrativePart(text, expression, funcs, iteration, from_beginning);
                        break;
                    case Function.FunctionType.SequentialCharFunction:
                        narrative = GenerateRTFSeqCharFileNarrativePart(text, expression, funcs);
                        break;
                    case Function.FunctionType.ConstantTextFunction:
                        narrative += text.Replace(@"\", @"\\");
                        break;
                }
            }
            return narrative;
        }

        private string GenerateRTFSeqIntFileNarrativePart(string file, string expression, Function[] funcs, int iteration, bool from_beginning)
        {
            string narrative = file;
            for (int i = funcs.Length - 1; i >= 0; i--)
            {
                SequentialIntFunction func = (SequentialIntFunction)funcs[i];
                int ipos = expression.IndexOf(func.Name);
                if (ipos > -1)
                {
                    int fpos = ipos + Math.Max(
                        (from_beginning ? func.AllVals(iteration).Last<int>().ToString().Length : func.NextVals(iteration).Last<int>().ToString().Length),
                        func.Padding);
                    narrative = narrative.Insert(fpos, @"\b0 ");
                    narrative = narrative.Insert(ipos, @"\b ");
                    narrative = narrative.Substring(0, ipos).Replace(@"\", @"\\") + narrative.Substring(ipos);
                }
            }
            return narrative;
        }

        private string GenerateRTFSeqCharFileNarrativePart(string file, string expression, Function[] funcs)
        {
            string narrative = file;
            for (int i = funcs.Length - 1; i >= 0; i--)
            {
                SequentialCharFunction func = (SequentialCharFunction)funcs[i];
                int ipos = expression.IndexOf(func.Name);
                if (ipos > -1)
                {
                    int fpos = ipos + 1;
                    narrative = narrative.Insert(fpos, @"\i0 ");
                    narrative = narrative.Insert(ipos, @"\i ");
                    narrative = narrative.Substring(0, ipos).Replace(@"\", @"\\") + narrative.Substring(ipos);
                }
            }
            return narrative;
        }

        private string GenerateRTFDiffOriginalFileNarrativePart(string file, Function[] funcs)
        {
            string narrative = file;
            for (int i = funcs.Length - 1; i >= 0; i--)
            {
                int ipos, fpos;
                ConstantFileDiffFunction func = (ConstantFileDiffFunction)funcs[i];
                for (int j = func.OriginalPositions.Length - 1; j >= 0; j--)
                {
                    ipos = func.OriginalPositions[j];
                    fpos = func.OriginalPositions[j] + func.OriginalTokens[j].Length;
                    if (fpos - ipos > 0)
                    {
                        narrative = narrative.Insert(fpos, @"\highlight0 ");
                        narrative = narrative.Insert(ipos, @"\highlight1 ");
                        narrative = narrative.Substring(0, ipos).Replace(@"\", @"\\") + narrative.Substring(ipos);
                    }
                }
            }
            return narrative;
        }

        private string GenerateRTFDiffReplacementFileNarrativePart(string file, Function[] funcs)
        {
            string narrative = file;
            for (int i = funcs.Length - 1; i >= 0; i--)
            {
                int ipos, fpos;
                ConstantFileDiffFunction func = (ConstantFileDiffFunction)funcs[i];
                for (int j = func.ReplacementPositions.Length - 1; j >= 0; j--)
                {
                    ipos = func.ReplacementPositions[j];
                    fpos = func.ReplacementPositions[j] + func.ReplacementTokens[j].Length;
                    if (fpos - ipos > -1)
                    {
                        narrative = narrative.Insert(fpos, @"\highlight0 ");
                        narrative = narrative.Insert(ipos, @"\highlight2 ");
                        narrative = narrative.Substring(0, ipos).Replace(@"\", @"\\") + narrative.Substring(ipos);
                    }
                }
            }
            return narrative;
        }

        private string GenerateRTFConstFileNarrativePart(string file, int i, int f)
        {
            string narrative = file.Replace(@"\", @"\\");
            narrative = narrative.Insert(file.Length, @"\b0 ");
            narrative = narrative.Insert(file.Length - f, @"\b ");
            narrative = narrative.Insert(0+i, @"\b0 ");
            narrative = narrative.Insert(0, @"\b ");
            return narrative;
        }

        private string GenerateRTFConstExFileNarrativePart(string file, string[] contents)
        {
            string narrative = file.Replace(@"\", @"\\");
            foreach (string token in contents)
                narrative = Regex.Replace(narrative, token, new MatchEvaluator(delegate(Match m)
                    {
                        return @"\b " + m.Value + @"\b0 ";
                    }), RegexOptions.IgnoreCase);
                //Regex.Replace(narative, token, @"\b " + token + @"\b0 ", RegexOptions.IgnoreCase);
                //narative = narative.Replace(token, @"\b " + token + @"\b0 ");
            return narrative;
        }

        // check:
        // http://www.codeproject.com/KB/edit/csexrichtextbox.aspx?print=true
        // http://www.developerfusion.com/code/4630/capture-a-screen-shot/
        // http://www.biblioscape.com/rtf15_spec.htm
        private string GenerateRTFMousePositionPart(int x, int y, Window wnd)
        {
            StringBuilder narrative = new StringBuilder();
            SystemCore.SystemAbstraction.ScreenCapture sc = new SystemCore.SystemAbstraction.ScreenCapture();
            System.Drawing.Image image = sc.CaptureWindow((IntPtr)wnd.Handle, (x - wnd.X) - 16, (y - wnd.Y) - 12, 32, 24);
            narrative.Append(GetImagePrefix(image));
            narrative.Append(GetRtfImage(image));
            narrative.Append(Environment.NewLine + @"}");
            image.Dispose();
            return narrative.ToString();
        }

        private string RTFArrayToStringBold(string[] arr)
        {
            string ret = string.Empty;
            if (arr != null)
            {
                int len = arr.Length;
                for (int i = 0; i < len; i++)
                {
                    ret += "\"" + @"\b " + arr[i].Replace(@"\", @"\\") + @"\b0 " + "\"";
                    if (i < len - 1)
                        ret += ", ";
                }
            }
            return ret;
        }

        private string GetImagePrefix(System.Drawing.Image image)
        {

            StringBuilder _rtf = new StringBuilder();

            //System.Drawing.Graphics graphics = _rtb.CreateGraphics();
            //float xDpi = graphics.DpiX;
            //float yDpi = graphics.DpiY;
            float xDpi = 96.0f;
            float yDpi = 96.0f;

            // Calculate the current width of the image in (0.01)mm
            int picw = (int)Math.Round((image.Width / xDpi) * 2540);

            // Calculate the current height of the image in (0.01)mm
            int pich = (int)Math.Round((image.Height / yDpi) * 2540);

            // Calculate the target width of the image in twips
            int picwgoal = (int)Math.Round((image.Width / xDpi) * 1440);

            // Calculate the target height of the image in twips
            int pichgoal = (int)Math.Round((image.Height / yDpi) * 1440);

            // Append values to RTF string
            _rtf.Append(@"{\pict\wmetafile8");
            _rtf.Append(@"\picw");
            _rtf.Append(picw);
            _rtf.Append(@"\pich");
            _rtf.Append(pich);
            _rtf.Append(@"\picwgoal");
            _rtf.Append(picwgoal);
            _rtf.Append(@"\pichgoal");
            _rtf.Append(pichgoal);
            _rtf.Append(" ");

            return _rtf.ToString();
        }

        private string GetRtfImage(System.Drawing.Image image)
        {

            StringBuilder rtf = null;

            // Used to store the enhanced metafile
            MemoryStream stream = null;

            // Used to create the metafile and draw the image
            System.Drawing.Graphics graphics = null;

            // The enhanced metafile
            System.Drawing.Imaging.Metafile metaFile = null;

            // Handle to the device context used to create the metafile
            IntPtr hdc;

            System.Windows.Forms.RichTextBox rtb = new System.Windows.Forms.RichTextBox();

            try
            {
                rtf = new StringBuilder();
                stream = new MemoryStream();

                // Get a graphics context from the RichTextBox
                using (graphics = rtb.CreateGraphics())
                {

                    // Get the device context from the graphics context
                    hdc = graphics.GetHdc();

                    // Create a new Enhanced Metafile from the device context
                    metaFile = new System.Drawing.Imaging.Metafile(stream, hdc);

                    // Release the device context
                    graphics.ReleaseHdc(hdc);
                }

                // Get a graphics context from the Enhanced Metafile
                using (graphics = System.Drawing.Graphics.FromImage(metaFile))
                {

                    // Draw the image on the Enhanced Metafile
                    graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, image.Width, image.Height));

                }

                // Get the handle of the Enhanced Metafile
                IntPtr hEmf = metaFile.GetHenhmetafile();

                // A call to EmfToWmfBits with a null buffer return the size of the
                // buffer need to store the WMF bits.  Use this to get the buffer
                // size.
                uint bufferSize = SystemCore.SystemAbstraction.Win32.GdipEmfToWmfBits(hEmf, 0, null, 8, // 8 = MM_ANISOTROPIC
                    SystemCore.SystemAbstraction.EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);

                // Create an array to hold the bits
                byte[] buffer = new byte[bufferSize];

                // A call to EmfToWmfBits with a valid buffer copies the bits into the
                // buffer an returns the number of bits in the WMF.  
                uint convertedSize = SystemCore.SystemAbstraction.Win32.GdipEmfToWmfBits(hEmf, bufferSize, buffer, 8, // 8 = MM_ANISOTROPIC
                    SystemCore.SystemAbstraction.EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);

                // Append the bits to the RTF string
                for (int i = 0; i < buffer.Length; ++i)
                {
                    rtf.Append(String.Format("{0:X2}", buffer[i]));
                }

                return rtf.ToString();
            }
            finally
            {
                if (graphics != null)
                    graphics.Dispose();
                if (metaFile != null)
                    metaFile.Dispose();
                if (stream != null)
                    stream.Close();
                if (rtb != null)
                    rtb.Dispose();
            }
        }

        //
        // Auxiliary Methods
        //
        private string GetFolderItemName(string path)
        {
            return Path.GetFileName(path);
        }

        private string GetFolderItemNameWithoutExtension(string path)
        {
            if (File.Exists(path))
                return Path.GetFileNameWithoutExtension(path);
            else if (Directory.Exists(path))
                return Path.GetDirectoryName(path);
            else
                return string.Empty;
        }

        private string GetFolderItemExtension(string path)
        {
            if (File.Exists(path))
                return Path.GetExtension(path);
            else
                return string.Empty;
        }

        private string GetGenExt(Generalization last_gen)
        {
            string ext = string.Empty;
            switch (last_gen.Type)
            {
                case Generalization.GeneralizationType.FileCreateGeneralization:
                    {
                        FileCreateGeneralization fgen = (FileCreateGeneralization)last_gen;
                        if (fgen.Functions[fgen.Functions.Length - 1].Type == Function.FunctionType.ConstantFileExtFunction)
                        {
                            ConstantFileExtFunction func = (ConstantFileExtFunction)fgen.Functions[fgen.Functions.Length - 1];
                            if (func.Extensions.Length > 0)
                                ext = func.Extensions[0];
                        }
                    }
                    break;
                case Generalization.GeneralizationType.FileDeleteGeneralization:
                    {
                        FileDeleteGeneralization fgen = (FileDeleteGeneralization)last_gen;
                        if (fgen.Functions[fgen.Functions.Length - 1].Type == Function.FunctionType.ConstantFileExtFunction)
                        {
                            ConstantFileExtFunction func = (ConstantFileExtFunction)fgen.Functions[fgen.Functions.Length - 1];
                            if (func.Extensions.Length > 0)
                                ext = func.Extensions[0];
                        }
                    }
                    break;
                case Generalization.GeneralizationType.FileMoveGeneralization:
                    {
                        FileMoveGeneralization fgen = (FileMoveGeneralization)last_gen;
                        if (fgen.Functions[fgen.Functions.Length - 1].Type == Function.FunctionType.ConstantFileExtFunction)
                        {
                            ConstantFileExtFunction func = (ConstantFileExtFunction)fgen.Functions[fgen.Functions.Length - 1];
                            if (func.Extensions.Length > 0)
                                ext = func.Extensions[0];
                        }
                    }
                    break;
                case Generalization.GeneralizationType.FileRenameGeneralization:
                    {
                        FileRenameGeneralization fgen = (FileRenameGeneralization)last_gen;
                        if (fgen.OldNameFunctions[fgen.OldNameFunctions.Length - 1].Type == Function.FunctionType.ConstantFileExtFunction)
                        {
                            ConstantFileExtFunction func1 = (ConstantFileExtFunction)fgen.OldNameFunctions[fgen.OldNameFunctions.Length - 1];
                            if (func1.Extensions.Length > 0)
                                ext = func1.Extensions[0];
                        }
                    }
                    break;
                default:
                    ext = ".*";
                    break;
            }
            return ext;
        }
        #endregion
    }
}
