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
using System.Drawing;

namespace SystemCore.CommonTypes
{
    public class InterpreterItem
    {
        #region Properties
        private string _name;
        private string _description;
        private string _command_name;
        private string[] _command_tokens;
        private CommandUsage _command_usage;
        private string _auto_complete;
        private OwnerType _owner_type;
        private string _plugin_id; // optional (only used if Type == Plugin)
        private Bitmap _icon;
        private string _text;
        #endregion

        #region Enums
        public enum OwnerType
        {
            Indexer, FileSystem, Plugin, Menu
        };
        #endregion

        #region Accessors
        public string Name { get { return _name; } set { _name = value; } }
        public string Desciption { get { return _description; } set { _description = value; } }
        public string CommandName { get { return _command_name; } set { _command_name = value; } }
        public string[] CommandTokens { get { return _command_tokens; } set { _command_tokens = value; } }
        public CommandUsage CommandUsage { get { return _command_usage; } set { _command_usage = value; } }
        public string AutoComplete { get { return _auto_complete; } set { _auto_complete = value; } }
        public Bitmap Icon { get { return _icon; } set { _icon = value; } }
        public OwnerType Type { get { return _owner_type; } set { _owner_type = value; } }
        public string PluginId { get { return _plugin_id; } set { _plugin_id = value; } }
        public string Text { get { return _text; } set { _text = value; } }
        #endregion

        #region Constructors
        public InterpreterItem(string name, string description, string auto_complete, Bitmap icon, OwnerType type, string text)
        {
            _name = name;
            _description = description;
            _auto_complete = auto_complete;
            _icon = icon;
            _command_name = string.Empty;
            _command_tokens = null;
            _command_usage = null;
            _owner_type = type;
            _plugin_id = string.Empty;
            _text = text;
        }
        #endregion
    }
}
