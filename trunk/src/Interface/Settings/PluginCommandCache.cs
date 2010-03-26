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

namespace SystemCore.Settings
{
    public class PluginCommandCache
    {
        #region Properties
        private Index _index;
        private DateTime _last_modified;
        private TimeSpan _time_to_refresh;
        #endregion

        #region Accessors
        public Index Index { get { return _index; } }
        #endregion

        #region Constructors
        public PluginCommandCache(int time_to_refresh)
        {
            _index = new Index();
            _time_to_refresh = TimeSpan.FromSeconds((double)time_to_refresh);
        }
        #endregion

        #region Public Methods
        public void Update(List<InterpreterPlugin> plugins)
        {
            _index.Clear();
            foreach (InterpreterPlugin plugin in plugins)
            {
                foreach (Command command in plugin.Commands)
                {
                    if (command.FitsPriority(Command.PriorityType.Medium))
                        _index.AddItemByName(command.Name, plugin.Name, true);
                }
            }
            _last_modified = DateTime.Now;
        }

        public bool NeedsUpdate()
        {
            if (DateTime.Now - _last_modified >= _time_to_refresh)
                return true;
            else
                return false;
        }
        #endregion
    }
}
