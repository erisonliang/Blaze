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
    public class PluginInfo
    {
        #region Properties
        private List<string> _names;
        private Dictionary<string, bool> _enabled;
        #endregion

        #region Accessors
        public List<string> Names
        {
            get { return _names; }
            set { _names = value; }
        }

        public Dictionary<string, bool> Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        #endregion

        #region Constructors
        public PluginInfo(List<string> names, Dictionary<string, bool> enabled)
        {
            _names = names;
            _enabled = enabled;
        }

        public PluginInfo()
        {
            _names = new List<string>();
            _enabled = new Dictionary<string, bool>();
        }

        public PluginInfo(PluginInfo info)
        {
            _names = new List<string>(info.Names);
            _enabled = new Dictionary<string, bool>(info.Enabled);
        }
        #endregion

        #region Public Methods
        #endregion
    }
}
