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
using SystemCore.CommonTypes;
using System;
using System.Runtime.Serialization;

namespace SystemCore.Settings
{
    [Serializable]
    public class LearnedContent
    {
        #region Properties
        private static int _capacity = 24;
        private Queue<string> _keywords;
        private Dictionary<string, InterpreterItem.OwnerType> _types;
        private Dictionary<string, string> _distinguisher;
        #endregion

        #region Accessors
        public Queue<string> Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }

        public Dictionary<string, InterpreterItem.OwnerType> Types
        {
            get { return _types; }
            set { _types = value; }
        }

        public Dictionary<string, string> Distinguishers
        {
            get { return _distinguisher; }
            set { _distinguisher = value; }
        }
        #endregion

        #region Constructors
        public LearnedContent()
        {
            _keywords = new Queue<string>(_capacity);
            _types = new Dictionary<string, InterpreterItem.OwnerType>(_capacity);
            _distinguisher = new Dictionary<string, string>(_capacity);
        }

        public LearnedContent(LearnedContent content)
        {
            _keywords = new Queue<string>(content.Keywords);
            _types = new Dictionary<string, InterpreterItem.OwnerType>(content.Types);
            _distinguisher = new Dictionary<string,string>(content.Distinguishers);
        }

        public LearnedContent(SerializationInfo info, StreamingContext context)
        {
            _keywords = (Queue<string>)info.GetValue("Keywords", typeof(Queue<string>));
            _types = (Dictionary<string, InterpreterItem.OwnerType>)info.GetValue("Types", typeof(Dictionary<string, InterpreterItem.OwnerType>));
            _distinguisher = (Dictionary<string, string>)info.GetValue("Distinguishers", typeof(Dictionary<string, string>));
        }
        #endregion

        #region Public Methods
        public void AddKeyword(string keyword,  InterpreterItem.OwnerType type, string content)
        {
            if (!_distinguisher.ContainsKey(keyword))
            {
                if (!_distinguisher.ContainsValue(content))
                {
                    if (_keywords.Count == _capacity)
                    {
                        string to_remove = _keywords.Dequeue();
                        _distinguisher.Remove(to_remove);
                        _types.Remove(to_remove);
                    }
                    _keywords.Enqueue(keyword);
                    _distinguisher.Add(keyword, content);
                    _types.Add(keyword, type);
                }
            }
            else
            {
                List<string> temp = new List<string>(_keywords.ToArray());
                temp.Remove(keyword);
                _keywords = new Queue<string>(temp.ToArray());
                _keywords.Enqueue(keyword);
                _distinguisher[keyword] = content;
                _types[keyword] = type;
                temp = null;
            }
        }

        public void RemoveKeyword(string keyword, string content)
        {
            if (_distinguisher.ContainsKey(keyword))
            {
                List<string> temp = new List<string>(_keywords.ToArray());
                temp.Remove(keyword);
                _keywords = new Queue<string>(temp.ToArray());
                _distinguisher.Remove(keyword);
                _types.Remove(keyword);
                temp = null;
            }
        }

        public string[] GetSortedKeywords(string text)
        {
            List<string> accepted_tokens = new List<string>();
            Dictionary<string, int> weights = new Dictionary<string, int>();
            foreach (string token in _keywords)
            {
                if (SystemCore.SystemAbstraction.StringUtilities.StringUtility.WordContainsStr(token, text))
                {
                    accepted_tokens.Add(token);
                    weights.Add(token, token.Length);
                }
            }
            accepted_tokens.Sort(delegate(string a, string b)
            {
                return weights[b].CompareTo(weights[a]);
            });
            return accepted_tokens.ToArray();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Keywords", Keywords);
            info.AddValue("Types", Types);
            info.AddValue("Distinguishers", Distinguishers);
        }
        #endregion
    }
}
