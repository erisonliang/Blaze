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

namespace SystemCore.CommonTypes
{
    public class CommandUsage
    {
        #region Properties
        private string _name;
        private List<string> _arguments;
        private Dictionary<string, bool> _completeness;
        #endregion

        #region Accessors
        public string Name { get { return _name; } }
        public List<string> Arguments { get { return _arguments; } }
        public Dictionary<string, bool> Completeness { get { return _completeness; } }
        #endregion

        #region Constructors
        public CommandUsage(CommandUsage usage)
        {
            _name = usage.Name;
            _arguments = new List<string>(usage.Arguments);
            _completeness = new Dictionary<string,bool>(usage.Completeness);
        }

        public CommandUsage(string name)
        {
            _name = name;
            _arguments = new List<string>();
            _completeness = new Dictionary<string, bool>();
        }

        public CommandUsage(string name, List<string> arguments)
        {
            _name = name;
            _arguments = arguments;
            _completeness = new Dictionary<string, bool>(_arguments.Count);
            foreach (string arg in _arguments)
                _completeness.Add(arg, false);
        }

        public CommandUsage(string name, List<string> arguments, Dictionary<string, bool> completeness)
        {
            _name = name;
            _arguments = arguments;
            _completeness = completeness;
        }
        #endregion
    }
}
