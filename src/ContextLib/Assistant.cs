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
using System.Collections.Generic;
using ContextLib.DataContainers.Monitoring;
using ContextLib.DataContainers.Monitoring.Generalizations;

namespace ContextLib
{
    public class Assistant
    {
        #region Properties
        private bool _has_suggestions;
        private Suggestion[] _suggestions;
        #endregion

        #region Accessors
        public bool HasSuggestions { get { return _has_suggestions; } }
        public Suggestion[] Suggestions { get { return _suggestions; } }
        #endregion

        #region Events
        public delegate void SuggestionEventHandler();
        public event SuggestionEventHandler NewSuggestion;
        public event SuggestionEventHandler NoNewSuggestion;
        #endregion

        #region Constructors
        public Assistant()
        {
            _has_suggestions = false;
            _suggestions = new Suggestion[0];
        }
        #endregion

        #region Public Methods
        public void GenerateSuggestions(UserActionList[] repetitions, Dictionary<short, List<Generalization>> generalizations)
        {
            List<Suggestion> suggestions = new List<Suggestion>();
            UserActionList basic_actions;
            Dictionary<UserAction, List<Generalization>> basic_generalizations;
            foreach (UserActionList repetition in repetitions)
            {
                basic_actions = new UserActionList(repetition);
                basic_generalizations = new Dictionary<UserAction, List<Generalization>>();
                foreach (UserAction action in repetition)
                {
                    foreach (KeyValuePair<short, List<Generalization>> pair in generalizations)
                    {
                        List<Generalization> new_gens = new List<Generalization>();
                        if (pair.Key == action.Id)
                        {
                            foreach (Generalization gen in pair.Value)
                                new_gens.Add((Generalization)gen.Clone());
                            //if (new_gens.Count == 0)
                            //    System.Windows.Forms.MessageBox.Show("huston, we got a problem :(");
                            basic_generalizations.Add(action, new_gens);
                        }
                    }
                }
                suggestions.Add(new Suggestion(basic_actions, basic_generalizations));
            }
            suggestions.RemoveAll(delegate(Suggestion suggestion)
            {
                return !suggestion.Valid;
            });
            suggestions.Sort(delegate(Suggestion a, Suggestion b)
            {
                if (a.BasicActionList.Count == b.BasicActionList.Count)
                    return a.Time.CompareTo(b.Time);
                else
                    return a.BasicActionList.Count.CompareTo(b.BasicActionList.Count);
            });
            suggestions.Reverse();
            _suggestions = suggestions.ToArray();
            if (_suggestions.Length > 0)
            {
                if (!_has_suggestions)
                {
                    _has_suggestions = true;
                    NewSuggestion();
                }
            }
            else
            {
                if (_has_suggestions)
                {
                    _has_suggestions = false;
                    NoNewSuggestion();
                }
            }
        }

        public void ValidateSuggestions()
        {
            ValidateSuggestions(false);
        }

        public void ValidateSuggestions(bool from_beginning)
        {
            List<Suggestion> suggestions = new List<Suggestion>(_suggestions);
            foreach (Suggestion suggestion in suggestions)
                suggestion.Validate(from_beginning);
            suggestions.RemoveAll(delegate(Suggestion suggestion)
            {
                return !suggestion.Valid;
            });
            _suggestions = suggestions.ToArray();
            if (_suggestions.Length > 0)
            {
                if (!_has_suggestions)
                {
                    _has_suggestions = true;
                    NewSuggestion();
                }
            }
            else
            {
                if (_has_suggestions)
                {
                    _has_suggestions = false;
                    NoNewSuggestion();
                }
            }
        }
        #endregion
    }
}
