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

namespace SystemCore.Settings
{
    public class LearnedContent
    {
        #region Properties
        private static int _capacity = 30;
        private Queue<string> _keywords;
        private Dictionary<string, string> _contents;
        #endregion

        #region Accessors
        public Queue<string> Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }

        public Dictionary<string, string> Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }
        #endregion

        #region Constructors
        public LearnedContent()
        {
            _keywords = new Queue<string>(30);
            _contents = new Dictionary<string, string>(30);
        }

        public LearnedContent(LearnedContent content)
        {
            _keywords = new Queue<string>(content.Keywords);
            _contents = new Dictionary<string,string>(content.Contents);
        }
        #endregion

        #region Public Methods
        public void AddKeyword(string keyword, string content)
        {
            //string delete = string.Empty;
            //foreach (KeyValuePair<string, List<string>> pair in _contents)
            //{
            //    if (pair.Value.Contains(content))
            //    {
            //        if (pair.Key == keyword)
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            pair.Value.Remove(content);
            //            if (pair.Value.Count == 0)
            //                delete = pair.Key;
            //        }
            //    }
            //}
            //if (delete != string.Empty)
            //{
            //    _keywords.Remove(delete);
            //    _contents.Remove(delete);
            //}
            if (!_contents.ContainsKey(keyword))
            {
                if (!_contents.ContainsValue(content))
                if (_keywords.Count == _capacity)
                {
                    _contents.Remove(_keywords.Dequeue());
                }
                _keywords.Enqueue(keyword);
                _contents.Add(keyword, content);
            }
            else
            {
                List<string> temp = new List<string>(_keywords.ToArray());
                temp.Remove(keyword);
                _keywords = new Queue<string>(temp.ToArray());
                _keywords.Enqueue(keyword);
                _contents[keyword] = content;
                temp = null;
            }
            //delete = null;
        }

        public void RemoveKeyword(string keyword, string content)
        {
            if (_contents.ContainsKey(keyword))
            {
                List<string> temp = new List<string>(_keywords.ToArray());
                temp.Remove(keyword);
                _keywords = new Queue<string>(temp.ToArray());
                _contents.Remove(keyword);
                temp = null;
            }
        }
        #endregion
    }
}
