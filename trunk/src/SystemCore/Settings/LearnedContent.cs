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
using System.Linq;

namespace SystemCore.Settings
{
    [Serializable]
    public class LearnedContent
    {
        #region Properties
        private static int _capacity = 24;
        private List<LearnedItem> _items;
        private Dictionary<string, LearnedItem> _keyword_to_item;
        #endregion

        #region Accessors
        public List<LearnedItem> LearnedItems
        {
            get { return _items; }
        }

        public Dictionary<string, LearnedItem> KeywordToItem
        {
            get { return _keyword_to_item; }
        }
        #endregion

        #region Constructors
        public LearnedContent()
        {
            _items = new List<LearnedItem>(_capacity);
            _keyword_to_item = new Dictionary<string, LearnedItem>(_capacity);
        }

        public LearnedContent(LearnedContent content)
        {
            _items = new List<LearnedItem>(content.LearnedItems);
            _keyword_to_item = new Dictionary<string, LearnedItem>(content._keyword_to_item);
        }

        public LearnedContent(SerializationInfo info, StreamingContext context)
        {
            _items = (List<LearnedItem>)info.GetValue("LearnedItems", typeof(List<LearnedItem>));
            _keyword_to_item = (Dictionary<string, LearnedItem>)info.GetValue("KeywordToItem", typeof(Dictionary<string, LearnedItem>));
        }
        #endregion

        #region Public Methods
        public void AddKeyword(string keyword,  InterpreterItem.OwnerType type, string content, string[] tokens)
        {
            string[] f_tokens = (tokens == null ? new string[0] : tokens);
            if (!_keyword_to_item.ContainsKey(keyword))
            {
                if (_items.Count == _capacity)
                {
                    _items.RemoveAt(0);
                }
                LearnedItem li = new LearnedItem(keyword, type, content, f_tokens);
                _items.Add(li);
                _keyword_to_item.Add(keyword, li);
            }
            else
            {
                _items.RemoveAll(delegate(LearnedItem i)
                {
                    return i.Keyword == keyword;
                });
                LearnedItem li = new LearnedItem(keyword, type, content, f_tokens);
                _items.Add(li);
                _keyword_to_item[keyword] = li;
            }
        }

        public void RemoveKeyword(string keyword, string content)
        {
            if (_keyword_to_item.ContainsKey(keyword))
            {
                _items.RemoveAll(delegate(LearnedItem i)
                {
                    return i.Keyword == keyword && i.Distinguisher == content;
                });
                _keyword_to_item.Remove(keyword);
            }
        }

        public LearnedItem[] GetSortedItems(string text)
        {
            List<string> accepted_tokens = new List<string>();
            Dictionary<string, int> weights = new Dictionary<string, int>();
            foreach (string token in _keyword_to_item.Keys)
            {
                if (SystemCore.SystemAbstraction.StringUtilities.StringUtility.WordContainsStr(token, text))
                {
                    accepted_tokens.Add(token);
                    weights.Add(token, token.Length);
                }
            }
            accepted_tokens.Sort(delegate(string a, string b)
            {
                return weights[a].CompareTo(weights[b]);
            });
            List<LearnedItem> ret = new List<LearnedItem>();
            foreach (string token in accepted_tokens)
                ret.Add(_keyword_to_item[token]);
            return ret.ToArray();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LearnedItems", LearnedItems);
            info.AddValue("KeywordToItem", KeywordToItem);
        }
        #endregion
    }
}
