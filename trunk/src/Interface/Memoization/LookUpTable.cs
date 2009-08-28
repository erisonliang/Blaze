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

namespace Blaze.Memoization
{
    public class LookUpTable
    {
        #region Properties
        Dictionary<string, int> _table;
        Queue<string> _queue;
        int _max_capacity;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public LookUpTable(int max_capacity)
        {
            _table = new Dictionary<string, int>(max_capacity);
            _queue = new Queue<string>(max_capacity);
            _max_capacity = max_capacity;
        }
        #endregion

        #region Public Methods
        public bool Contains(string a, string b)
        {
            return _table.ContainsKey(BuildEntry(a,b));
        }

        public int GetValue(string a, string b)
        {
            return _table[BuildEntry(a,b)];
        }

        public void Add(string a, string b, int val)
        {
            string new_entry = BuildEntry(a,b);
            if (_table.ContainsKey(new_entry))
            {
                _table[new_entry] = val;
            }
            else
            {
                if (_queue.Count == _max_capacity)
                {
                    _table.Remove(_queue.Dequeue());
                }
                _queue.Enqueue(new_entry);
                _table.Add(new_entry, val);
            }
        }
        #endregion

        #region Private Methods
        private string BuildEntry(string a, string b)
        {
            if (a.CompareTo(b) <= 0)
                return a + "," + b;
            else
                return b + "," + a;
        }
        #endregion
    }
}
