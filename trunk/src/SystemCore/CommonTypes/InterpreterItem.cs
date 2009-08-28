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
    public enum OwnerType
    {
        Unspecified, Indexer, FileSystem, Plugin, Menu
    };

    public enum ItemType
    {
        Normal, Learned
    };

    public class InterpreterItem
    {
        #region Properties
        private string _name;
        private string _description;
        private OwnerType _owner_type;
        private string _owner_id;
        private string _command_name;
        private string[] _command_tokens;
        private CommandUsage _command_usage;
        private string _autoComplete;
        private int _pos;
        private Image _icon;
        private ItemType _type;
        private string _text;
        #endregion

        #region Accessors
        public string Name { get { return _name; } set { _name = value; } }
        public string Desciption { get { return _description; } set { _description = value; } }
        public OwnerType OwnerType { get { return _owner_type; } set { _owner_type = value; } }
        public string OwnerId { get { return _owner_id; } set { _owner_id = value; } }
        public string CommandName { get { return _command_name; } set { _command_name = value; } }
        public string[] CommandTokens { get { return _command_tokens; } set { _command_tokens = value; } }
        public CommandUsage CommandUsage { get { return _command_usage; } set { _command_usage = value; } }
        public string AutoComplete { get { return _autoComplete; } set { _autoComplete = value; } }
        public int Pos { get { return _pos; } set { _pos = value; } }
        public Image Icon { get { return _icon; } set { _icon = value; } }
        public ItemType Type { get { return _type; } set { _type = value; } }
        public string Text { get { return _text; } set { _text = value; } }
        #endregion

        #region Constructors
        public InterpreterItem(string name, string description, OwnerType owner_type, string autoComplete, int pos, Image icon, ItemType type)
        {
            _name = name;
            _description = description;
            _owner_type = owner_type;
            _owner_id = string.Empty;
            _command_name = string.Empty;
            _autoComplete = autoComplete;
            _pos = pos;
            _icon = icon;
            _type = type;
            _command_tokens = null;
            _text = autoComplete;
        }

        public InterpreterItem(string name, string description, OwnerType owner_type, string autoComplete, int pos, Image icon)
        {
            _name = name;
            _description = description;
            _owner_type = owner_type;
            _owner_id = string.Empty;
            _command_name = string.Empty;
            _autoComplete = autoComplete;
            _pos = pos;
            _icon = icon;
            _type = ItemType.Normal;
            _command_tokens = null;
            _text = autoComplete;
        }

        public InterpreterItem(string name, string description, OwnerType owner_type, string autoComplete, Image icon)
        {
            _name = name;
            _description = description;
            _owner_type = owner_type;
            _owner_id = string.Empty;
            _command_name = string.Empty;
            _autoComplete = autoComplete;
            _pos = 0;
            _icon = icon;
            _type = ItemType.Normal;
            _command_tokens = null;
            _text = autoComplete;
        }

        public InterpreterItem(string name, string description, string autoComplete, Image icon)
        {
            _name = name;
            _description = description;
            _owner_type = OwnerType.Unspecified;
            _owner_id = string.Empty;
            _command_name = string.Empty;
            _autoComplete = autoComplete;
            _pos = 0;
            _icon = icon;
            _type = ItemType.Normal;
            _command_tokens = null;
            _text = autoComplete;
        }
        #endregion

        #region Public Methods
        public void Dispose()
        {
            _name = string.Empty;;
            _description = string.Empty;
            _owner_id = string.Empty;
            _command_name = string.Empty;
            _autoComplete = string.Empty;
            _icon.Dispose();
            _icon = null;
            _command_tokens = null;
            _text = string.Empty;
        }
        #endregion
    }
}
