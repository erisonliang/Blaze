// Blaze: Automated Desktop Experience
// Copyright (C) 2008-2010  Gabriel Barata
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
using System.Linq;
using System.Text;
using SystemCore.CommonTypes;

namespace SystemCore.Settings
{
    [Serializable]
    public class LearnedItem
    {
        #region Properties
        private string _keyword;
        private InterpreterItem.OwnerType _type;
        private string _distinguisher;
        private string[] _tokens;
        #endregion

        #region Accessors
        public string Keyword { get { return _keyword; } }
        public InterpreterItem.OwnerType Type { get { return _type; } }
        public string Distinguisher { get { return _distinguisher; } }
        public string[] Tokens { get { return _tokens; } }
        #endregion

        #region Constructor
        public LearnedItem(string keyword, InterpreterItem.OwnerType type, string distinguisher, string[] tokens)
        {
            _keyword = keyword;
            _type = type;
            _distinguisher = distinguisher;
            _tokens = tokens;
        }

        public LearnedItem(LearnedItem item)
        {
            _keyword = item._keyword;
            _type = item._type;
            _distinguisher = item._distinguisher;
            _tokens = item._tokens;
        }
        #endregion
    }
}
