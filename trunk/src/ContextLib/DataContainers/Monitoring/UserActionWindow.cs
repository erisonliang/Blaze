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
using System.Threading;
using SystemCore.CommonTypes;
using ContextLib.DataContainers.Monitoring.Generalizations;

namespace ContextLib.DataContainers.Monitoring
{
    public class UserActionWindow
    {
        #region Properties
        private int _size;
        private TimeSpan _life_period;
        private TimeSpan _validation_period;
        private DateTime _last_validation;
        private List<UserAction> _user_actions;
        private bool _use_compression;
        private Mutex _mutex = new Mutex(false, CommonInfo.GUID + "-user-actions-lock");
        private short _counter; // action counter
        private short _counter_limit; // upper counter limit
        private Dictionary<short, int> _counter_reference; // <id . num of actions> the amout of actions referencia a certain id
        private Dictionary<short, List<Generalizations.Generalization>> _generalizations; // <id . generalizations> generalizations associated to a specifi id
        private UserAction _last_processed_action = null;
        private UserAction.Modifiers _modifiers = UserAction.Modifiers.None;
        #endregion

        #region Accessors
        public List<UserAction> UserActions { get { return _user_actions; } }
        public Dictionary<short, List<Generalizations.Generalization>> Generalizations { get { return _generalizations; } }
        public bool UseCompression { get { return _use_compression; } set { _use_compression = value; } }
        #endregion

        #region Constructors
        public UserActionWindow(int size, int life_period, int validation_period)
        {
            _size = size;
            _life_period = new TimeSpan(life_period * 10000);
            _validation_period = new TimeSpan(validation_period * 10000);
            _last_validation = DateTime.Now;
            _user_actions = new List<UserAction>(_size);
            _use_compression = true;

            _counter = 0;
            _counter_limit = (short)size;
            _counter_reference = new Dictionary<short, int>(_counter_limit);
            _generalizations = new Dictionary<short, List<Generalizations.Generalization>>(_counter_limit);
            for (short i = 0; i < _counter_limit; i++)
            {
                _counter_reference.Add(i, 0);
                _generalizations.Add(i, new List<ContextLib.DataContainers.Monitoring.Generalizations.Generalization>());
            }
        }
        #endregion

        #region Public Methods
        public void AddAction(UserAction action)
        {
            ValidadeActions();
            //_mutex.WaitOne();
            if (UseCompression)
            {
                // if last actions was a MouseDown but the new one isn't a MouseUp, then replace the MouseDown by a MouseClick
                if ((GetLastAction() != null && GetLastAction().ActionType == UserAction.UserActionType.MouseDownAction))
                {
                    MouseDownAction temp_action = (MouseDownAction)GetLastAction();
                    if (action.ActionType == UserAction.UserActionType.MouseUpAction)
                    {
                        UserAction merged_action = action.Merge(temp_action);
                        if (merged_action == null) // if there is no possible merging, ignore the mouse up
                        {
                            MouseClickAction click = new MouseClickAction(temp_action.Window, temp_action.Button, temp_action.X, temp_action.Y, temp_action.Modifiers);
                            click.Time = temp_action.Time;
                            ReplaceLastAction(click);
                        }
                    }
                    else
                    {
                        MouseClickAction click = new MouseClickAction(temp_action.Window, temp_action.Button, temp_action.X, temp_action.Y, temp_action.Modifiers);
                        click.Time = temp_action.Time;
                        ReplaceLastAction(click);
                    }
                    //click.Id = temp_action.Id;
                    //_user_actions[_user_actions.Count - 1] = click;
                }

                // ignore KeyUp events
                if (action.ActionType == UserAction.UserActionType.KeyUpAction)
                {
                    UserAction last_action = _last_processed_action;
                    if (last_action != null)
                    {
                        KeyUpAction key_action = (KeyUpAction)action;
                        if (key_action.Key == System.Windows.Forms.Keys.LMenu ||
                            key_action.Key == System.Windows.Forms.Keys.RMenu)
                        {
                            if ((_modifiers & UserAction.Modifiers.Alt) != UserAction.Modifiers.Alt)
                            {
                                UserAction new_action = new KeyPressAction(last_action.Window, key_action.Key, key_action.Modifiers);
                                new_action.Time = key_action.Time;
                                BasicAddAction(new_action);
                                return;
                            }
                            else
                            {
                                _modifiers &= ~UserAction.Modifiers.Alt;
                                return;
                            }
                        }
                        else if (key_action.Key == System.Windows.Forms.Keys.LControlKey ||
                                 key_action.Key == System.Windows.Forms.Keys.RControlKey)
                        {
                            if ((_modifiers & UserAction.Modifiers.Ctrl) != UserAction.Modifiers.Ctrl)
                            {
                                UserAction new_action = new KeyPressAction(last_action.Window, key_action.Key, key_action.Modifiers);
                                new_action.Time = key_action.Time;
                                BasicAddAction(new_action);
                                return;
                            }
                            else
                            {
                                _modifiers &= ~UserAction.Modifiers.Ctrl;
                                return;
                            }
                        }
                        else if (key_action.Key == System.Windows.Forms.Keys.LShiftKey ||
                                 key_action.Key == System.Windows.Forms.Keys.RShiftKey)
                        {
                            if ((_modifiers & UserAction.Modifiers.Shift) != UserAction.Modifiers.Shift)
                            {
                                UserAction new_action = new KeyPressAction(last_action.Window, key_action.Key, key_action.Modifiers);
                                new_action.Time = key_action.Time;
                                BasicAddAction(new_action);
                                return;
                            }
                            else
                            {
                                _modifiers &= ~UserAction.Modifiers.Shift;
                                return;
                            }
                        }
                        else if (key_action.Key == System.Windows.Forms.Keys.LWin ||
                                 key_action.Key == System.Windows.Forms.Keys.RWin)
                        {
                            if ((_modifiers & UserAction.Modifiers.Win) != UserAction.Modifiers.Win)
                            {
                                UserAction new_action = new KeyPressAction(last_action.Window, key_action.Key, key_action.Modifiers);
                                new_action.Time = key_action.Time;
                                BasicAddAction(new_action);
                                return;
                            }
                            else
                            {
                                _modifiers &= ~UserAction.Modifiers.Win;
                                return;
                            }
                        }
                    }
                    else
                    {
                        _last_processed_action = (UserAction)action.Clone();
                        _modifiers = UserContext.Instance.ObserverObject.Modifiers;
                        return;
                    }
                }
                // ignore a MouseUp event if the last one isn't a MouseDown
                else if (action.ActionType == UserAction.UserActionType.MouseUpAction &&
                            (GetLastAction() == null || GetLastAction().ActionType != UserAction.UserActionType.MouseDownAction))
                {
                    _last_processed_action = action;
                    _modifiers = UserContext.Instance.ObserverObject.Modifiers;
                    return;
                }
                // if new action is a KeyDown, replace it by a KeyPress
                else if (action.ActionType == UserAction.UserActionType.KeyDownAction)
                {
                    KeyDownAction temp_action = (KeyDownAction)action;
                    UserAction new_action = new KeyPressAction(temp_action.Window, temp_action.Key, temp_action.Modifiers);
                    new_action.Time = temp_action.Time;
                    //new_action.Id = temp_action.Id;
                    BasicAddAction(new_action);
                }
                else if (action.ActionType == UserAction.UserActionType.KeyPressAction)
                {
                    KeyPressAction temp_action = (KeyPressAction)action;
                    BasicAddAction(temp_action);
                }
                // otherwise, just merge the last logged action with the new one
                else
                {
                    UserAction last_action = GetLastAction();
                    UserAction merged_action = action.Merge(last_action);
                    if (merged_action != null) // if merge was successful, replace the last action by the merged one
                    {
                        if (merged_action.IsType(UserAction.UserActionType.TerminalAction))
                        {
                            RemoveLastAction();
                        }
                        else
                        {
                            merged_action.Time = action.Time;
                            ReplaceLastAction(merged_action);
                        }
                    }
                    else // just add the new action
                    {
                        BasicAddAction(action);
                    }
                }
                // compress even more
                bool can_compress_more = true;
                while (can_compress_more)
                {
                    UserAction last_action = GetLastAction();
                    UserAction second_last_action = GetSecondLastAction();
                    if (last_action != null && second_last_action != null)
                    {
                        UserAction merged_action = last_action.Merge(second_last_action);
                        if (merged_action != null)
                        {
                            if (merged_action.IsType(UserAction.UserActionType.TerminalAction))
                            {
                                RemoveLastAction();
                                RemoveLastAction();
                            }
                            else
                            {
                                merged_action.Time = last_action.Time;
                                RemoveLastAction();
                                //merged_action.Id = second_last_action.Id;
                                //_user_actions[_user_actions.Count - 1] = merged_action;
                                ReplaceLastAction(merged_action);
                            }
                        }
                        else
                            can_compress_more = false;
                    }
                    else
                        can_compress_more = false;
                }
            }
            else
            {
                BasicAddAction(action);
            }
            _last_processed_action = (UserAction)action.Clone();
            _modifiers = UserContext.Instance.ObserverObject.Modifiers;
        }

        private void BasicAddAction(UserAction action)
        {
            if (_user_actions.Count == _size)
            {
                RemoveAction(0);
            }
            AssignValidId(action);
            _user_actions.Add(action);
        }

        public void RemoveAction(UserAction action)
        {
            //_mutex.WaitOne();
            //ValidadeActions();
            _counter_reference[action.Id]--;
            if (_counter_reference[action.Id] <= 1)
                _generalizations[action.Id].Clear();
            _user_actions.Remove(action);
            //_mutex.ReleaseMutex();
        }

        public void RemoveAction(int index)
        {
            //_mutex.WaitOne();
            //ValidadeActions();
            _counter_reference[_user_actions[index].Id]--;
            if (_counter_reference[_user_actions[index].Id] <= 1)
                _generalizations[_user_actions[index].Id].Clear();
            _user_actions.RemoveAt(index);
            //_mutex.ReleaseMutex();
        }

        public void Reset()
        {
            _last_processed_action = null;
        }

        public void ValidadeActions()
        {
            if (DateTime.Now - _last_validation > _validation_period)
            {
                //List<short> ids_to_be_removed = new List<short>();
                List<UserAction> actions_to_be_removed = new List<UserAction>();
                foreach (UserAction action in _user_actions)
                    if ((DateTime.Now - action.Time) > _life_period)
                    {
                        //ids_to_be_removed.Add(action.Id);
                        actions_to_be_removed.Add(action);
                    }
                //foreach (short id in ids_to_be_removed)
                //    _counter_reference[id]--;
                foreach (UserAction action in actions_to_be_removed)
                    RemoveAction(action);
                //_user_actions.RemoveAll(delegate(UserAction action)
                //{
                //    return (DateTime.Now - action.Time) > _life_period;
                //});
            }
        }
        #endregion

        #region Private Methods
        private UserAction GetLastAction()
        {
            if (_user_actions.Count > 0)
                return _user_actions[_user_actions.Count - 1];
            else
                return null;
        }

        private UserAction GetSecondLastAction()
        {
            if (_user_actions.Count > 1)
                return _user_actions[_user_actions.Count - 2];
            else
                return null;
        }

        private void RemoveLastAction()
        {
            if (_user_actions.Count > 0)
            {
                //_user_actions.RemoveAt(_user_actions.Count - 1);
                RemoveAction(_user_actions.Count - 1);
            }
        }

        private void ReplaceLastAction(UserAction action)
        {
            RemoveLastAction();
            BasicAddAction(action);
        }

        //private static bool _test = false;

        private void AssignValidId(UserAction action)
        {
            // find the last logged action that is of the same kind of the new one
            UserAction same_kind = null;
            UserAction regeneralize = null;
            Generalizations.Generalization[] generalizations = new Generalizations.Generalization[0];
            bool break_loop = false;
            for (int i = UserActions.Count - 1; i >= 0; i--)
            {
                //if (UserActions[i].Equals(action))
                //{
                //    same_kind = UserActions[i];
                //}
                if (action.GetType() == UserActions[i].GetType())
                {
                    switch (action.ActionType)
                    {
                        case UserAction.UserActionType.KeyPressAction:
                            {
                                KeyPressAction old_action = (KeyPressAction)UserActions[i];
                                KeyPressAction new_action = (KeyPressAction)action;
                                generalizations = KeyGeneralization.Generate(old_action.QuickDescription, new_action.QuickDescription, new_action.Time - old_action.Time);
                                if (generalizations.Length > 0)
                                {
                                    if (_generalizations[UserActions[i].Id].Count > 0)
                                    {
                                        Generalizations.Generalization[] merged_gens = KeyGeneralization.Merge(_generalizations[UserActions[i].Id].ToArray(), generalizations);
                                        if (merged_gens.Length > 0)
                                        {
                                            same_kind = UserActions[i];
                                            break_loop = true;
                                            generalizations = merged_gens;
                                        }
                                    }
                                    else
                                    {
                                        same_kind = UserActions[i];
                                        break_loop = true;
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.TypeTextAction:
                            {
                                TypeTextAction old_action = (TypeTextAction)UserActions[i];
                                TypeTextAction new_action = (TypeTextAction)action;
                                //if (new_action.Text == "ola 3 ")
                                //    _test = true;
                                //if (_test && new_action.Text == "ola 3")
                                //    System.Windows.Forms.MessageBox.Show("test");
                                generalizations = TextGeneralization.Generate(old_action.Text, new_action.Text, new_action.Time - old_action.Time);
                                if (generalizations.Length > 0)
                                {
                                    if (_generalizations[UserActions[i].Id].Count > 0)
                                    {
                                        Generalizations.Generalization[] merged_gens = TextGeneralization.Merge(_generalizations[UserActions[i].Id].ToArray(), generalizations);
                                        if (merged_gens.Length > 0)
                                        {
                                            same_kind = UserActions[i];
                                            break_loop = true;
                                            generalizations = merged_gens;
                                        }
                                    }
                                    else
                                    {
                                        same_kind = UserActions[i];
                                        break_loop = true;
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.MouseClickAction:
                            {
                                MouseClickAction old_action = (MouseClickAction)UserActions[i];
                                MouseClickAction new_action = (MouseClickAction)action;
                                if (old_action.Window.Handle == new_action.Window.Handle &&
                                    old_action.Button == new_action.Button &&
                                    old_action.Modifiers == new_action.Modifiers)
                                {
                                    generalizations = MouseGeneralization.Generate((int)old_action.X, (int)old_action.Y, (int)new_action.X, (int)new_action.Y, new_action.Time - old_action.Time);
                                    if (generalizations.Length > 0)
                                    {
                                        if (_generalizations[UserActions[i].Id].Count > 0)
                                        {
                                            Generalizations.Generalization[] merged_gens = MouseGeneralization.Merge(_generalizations[UserActions[i].Id].ToArray(), generalizations);
                                            if (merged_gens.Length > 0)
                                            {
                                                same_kind = UserActions[i];
                                                break_loop = true;
                                                generalizations = merged_gens;
                                            }
                                            else if (_generalizations[UserActions[i].Id][0].Occurrences == 2)
                                            {
                                                regeneralize = UserActions[i];
                                                break_loop = true;
                                            }
                                        }
                                        else
                                        {
                                            same_kind = UserActions[i];
                                            break_loop = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.MouseDoubleClickAction:
                            {
                                MouseDoubleClickAction old_action = (MouseDoubleClickAction)UserActions[i];
                                MouseDoubleClickAction new_action = (MouseDoubleClickAction)action;
                                if (old_action.Window.Handle == new_action.Window.Handle &&
                                    old_action.Button == new_action.Button &&
                                    old_action.Modifiers == new_action.Modifiers)
                                {
                                    generalizations = MouseGeneralization.Generate((int)old_action.X, (int)old_action.Y, (int)new_action.X, (int)new_action.Y, new_action.Time - old_action.Time);
                                    if (generalizations.Length > 0)
                                    {
                                        if (_generalizations[UserActions[i].Id].Count > 0)
                                        {
                                            Generalizations.Generalization[] merged_gens = MouseGeneralization.Merge(_generalizations[UserActions[i].Id].ToArray(), generalizations);
                                            if (merged_gens.Length > 0)
                                            {
                                                same_kind = UserActions[i];
                                                break_loop = true;
                                                generalizations = merged_gens;
                                            }
                                            else if (_generalizations[UserActions[i].Id][0].Occurrences == 2)
                                            {
                                                regeneralize = UserActions[i];
                                                break_loop = true;
                                            }
                                        }
                                        else
                                        {
                                            same_kind = UserActions[i];
                                            break_loop = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.MouseDragAction:
                            {
                                MouseDragAction old_action = (MouseDragAction)UserActions[i];
                                MouseDragAction new_action = (MouseDragAction)action;
                                if (old_action.Button == new_action.Button)
                                {
                                    generalizations = MouseDragGeneralization.Generate((int)old_action.InitialX, (int)old_action.InitialY, (int)new_action.InitialX, (int)new_action.InitialY, (int)old_action.FinalX, (int)old_action.FinalY, (int)new_action.FinalX, (int)new_action.FinalY, new_action.Time - old_action.Time);
                                    if (generalizations.Length > 0)
                                    {
                                        if (_generalizations[UserActions[i].Id].Count > 0)
                                        {
                                            Generalizations.Generalization[] merged_gens = MouseDragGeneralization.Merge(_generalizations[UserActions[i].Id].ToArray(), generalizations);
                                            if (merged_gens.Length > 0)
                                            {
                                                same_kind = UserActions[i];
                                                break_loop = true;
                                                generalizations = merged_gens;
                                            }
                                            else if (_generalizations[UserActions[i].Id][0].Occurrences == 2)
                                            {
                                                regeneralize = UserActions[i];
                                                break_loop = true;
                                            }
                                        }
                                        else
                                        {
                                            same_kind = UserActions[i];
                                            break_loop = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.MouseWheelSpinAction:
                            {
                                MouseWheelSpinAction old_action = (MouseWheelSpinAction)UserActions[i];
                                MouseWheelSpinAction new_action = (MouseWheelSpinAction)action;
                                if (((old_action.Delta >= 0 && new_action.Delta >= 0) || (old_action.Delta <= 0 && new_action.Delta <= 0)) &&
                                    old_action.Window.Handle == new_action.Window.Handle &&
                                    old_action.Modifiers == new_action.Modifiers)
                                {
                                    generalizations = MouseGeneralization.Generate((int)old_action.X, (int)old_action.Y, (int)new_action.X, (int)new_action.Y, new_action.Time - old_action.Time);
                                    if (generalizations.Length > 0)
                                    {
                                        if (_generalizations[UserActions[i].Id].Count > 0)
                                        {
                                            Generalizations.Generalization[] merged_gens = MouseGeneralization.Merge(_generalizations[UserActions[i].Id].ToArray(), generalizations);
                                            if (merged_gens.Length > 0)
                                            {
                                                same_kind = UserActions[i];
                                                break_loop = true;
                                                generalizations = merged_gens;
                                            }
                                            else if (_generalizations[UserActions[i].Id][0].Occurrences == 2)
                                            {
                                                regeneralize = UserActions[i];
                                                break_loop = true;
                                            }
                                        }
                                        else
                                        {
                                            same_kind = UserActions[i];
                                            break_loop = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.FileCreatedAction:
                            {
                                FileCreatedAction old_action = (FileCreatedAction)UserActions[i];
                                FileCreatedAction new_action = (FileCreatedAction)action;
                                generalizations = FileCreateGeneralization.Generate(old_action.FilePath, new_action.FilePath, new_action.Time - old_action.Time);
                                if (generalizations.Length > 0)
                                {
                                    if (_generalizations[UserActions[i].Id].Count > 0)
                                    {
                                        Generalizations.Generalization[] merged_gens = FileCreateGeneralization.Merge(_generalizations[UserActions[i].Id].ToArray(), generalizations);
                                        if (merged_gens.Length > 0)
                                        {
                                            same_kind = UserActions[i];
                                            break_loop = true;
                                            generalizations = merged_gens;
                                        }
                                    }
                                    else
                                    {
                                        same_kind = UserActions[i];
                                        break_loop = true;
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.FileDeletedAction:
                            {
                                FileDeletedAction old_action = (FileDeletedAction)UserActions[i];
                                FileDeletedAction new_action = (FileDeletedAction)action;
                                generalizations = FileDeleteGeneralization.Generate(old_action.FilePath, new_action.FilePath, new_action.Time - old_action.Time);
                                if (generalizations.Length > 0)
                                {
                                    if (_generalizations[UserActions[i].Id].Count > 0)
                                    {
                                        Generalizations.Generalization[] merged_gens = FileDeleteGeneralization.Merge(_generalizations[UserActions[i].Id].ToArray(), generalizations);
                                        if (merged_gens.Length > 0)
                                        {
                                            same_kind = UserActions[i];
                                            break_loop = true;
                                            generalizations = merged_gens;
                                        }
                                    }
                                    else
                                    {
                                        same_kind = UserActions[i];
                                        break_loop = true;
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.FileMovedAction:
                            {
                                FileMovedAction old_action = (FileMovedAction)UserActions[i];
                                FileMovedAction new_action = (FileMovedAction)action;
                                if (old_action.Folder == new_action.Folder)
                                {
                                    generalizations = FileMoveGeneralization.Generate(old_action.Folder, new_action.Folder, old_action.FileName, new_action.FileName, new_action.Time - old_action.Time);
                                    if (generalizations.Length > 0)
                                    {
                                        if (_generalizations[UserActions[i].Id].Count > 0)
                                        {
                                            Generalizations.Generalization[] merged_gens = FileMoveGeneralization.Merge(_generalizations[UserActions[i].Id].ToArray(), generalizations);
                                            if (merged_gens.Length > 0)
                                            {
                                                same_kind = UserActions[i];
                                                break_loop = true;
                                                generalizations = merged_gens;
                                            }
                                        }
                                        else
                                        {
                                            same_kind = UserActions[i];
                                            break_loop = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case UserAction.UserActionType.FileRenamedAction:
                            {
                                FileRenamedAction old_action = (FileRenamedAction)UserActions[i];
                                FileRenamedAction new_action = (FileRenamedAction)action;
                                //if (new_action.NewFile == @"D:\Test\other 2\3_hundred_men_attack #1.txt")
                                //    System.Windows.Forms.MessageBox.Show("tf?");
                                generalizations = FileRenameGeneralization.Generate(old_action.OldFile, old_action.NewFile, new_action.OldFile, new_action.NewFile, new_action.Time - old_action.Time);
                                if (generalizations.Length > 0)
                                {
                                    if (_generalizations[UserActions[i].Id].Count > 0)
                                    {
                                        Generalizations.Generalization[] merged_gens = FileRenameGeneralization.Merge(_generalizations[UserActions[i].Id].ToArray(), generalizations);
                                        if (merged_gens.Length > 0)
                                        {
                                            same_kind = UserActions[i];
                                            break_loop = true;
                                            generalizations = merged_gens;
                                        }
                                    }
                                    else
                                    {
                                        same_kind = UserActions[i];
                                        break_loop = true;
                                    }
                                }
                            }
                            break;
                        default:
                            if (UserActions[i].Equals(action))
                            {
                                //generalizations = TextGeneralization.Generate(((TypeTextAction)UserActions[i]).Text, ((TypeTextAction)action).Text);
                                same_kind = UserActions[i];
                                break_loop = true;
                            }
                            break;
                    }
                }
                if (break_loop)
                    break;
            }

            if (same_kind != null) // if an action of the same kind exists, get its ID
            {
                action.Id = same_kind.Id;
                if (generalizations.Length == 0)
                    System.Windows.Forms.MessageBox.Show("A zero length generalization was accepted. This should NEVER happen...");
                _generalizations[action.Id] = new List<ContextLib.DataContainers.Monitoring.Generalizations.Generalization>(generalizations);
            }
            else if (regeneralize != null) // there an action to regeneralize
            {
                action.Id = GenerateValidId();
                _counter_reference[regeneralize.Id]--;
                regeneralize.Id = action.Id;
                _counter_reference[action.Id]++;
                _generalizations[action.Id] = new List<ContextLib.DataContainers.Monitoring.Generalizations.Generalization>(generalizations);
            }
            else // else, get a new one
            {
                action.Id = GenerateValidId();
            }
            _counter_reference[action.Id]++;
        }

        private short GenerateValidId()
        {
            while (_counter_reference[_counter] != 0)
            {
                _counter++;
                if (_counter >= _counter_limit)
                {
                    _counter = 0;
                }
            }
            return _counter;
        }
        #endregion
    }
}
