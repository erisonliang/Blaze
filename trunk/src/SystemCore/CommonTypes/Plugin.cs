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
namespace SystemCore.CommonTypes
{
    public enum PluginType
    {
        Indexer, Interpreter
    }

    public abstract class Plugin
    {
        protected PluginType _type;
        protected bool _activated;
        protected string _name;
        protected string _description;
        protected string _version;
        protected string _website;
        protected bool _configurable;
        public PluginType Type { get { return _type; } }
        public bool Activated { get { return _activated; } set { _activated = value; } }
        public string Name { get { return _name; } }
        public string Description { get { return _description; } }
        public string Version { get { return _version; } }
        public string Website { get { return _website; } }
        public bool Configurable { get { return _configurable; } }
        public abstract void Configure();
    }
}
