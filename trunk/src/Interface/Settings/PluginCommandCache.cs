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

namespace SystemCore.Settings
{
    public class PluginCommandCache
    {
        #region Properties
        private Index _index;
        #endregion

        #region Accessors
        public Index Index { get { return _index; } }
        #endregion

        #region Constructors
        public PluginCommandCache(List<InterpreterPlugin> plugins)
        {
            _index = new Index();
            foreach (InterpreterPlugin plugin in plugins)
            {
                foreach (Command command in plugin.Commands)
                {
                    string name = command.ProtectedName;
                    List<string> keywords = new List<string>(command.Keywords);
                    _index.Names.Add(name);
                    _index.Paths.Add(name, new List<string>());
                    _index.Keywords.Add(name, keywords);
                    _index.Icons.Add(name, new List<System.Drawing.Image>());
                }
            }
        }
        #endregion
    }
}
