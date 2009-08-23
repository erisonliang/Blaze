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
using System.Xml.Serialization;

namespace TextTools
{
    [XmlRoot("QuickText")]
    public class QuickText
    {
        #region Properties
        private string _name;
        private string _text;
        #endregion

        #region Accessors
        [XmlAttribute("name")]
        public string Name { get { return _name; } set { _name = value; } }
        [XmlText()]
        public string Text { get { return _text; } set { _text = value; } }
        #endregion

        #region Constructors
        public QuickText(string name, string text)
        {
            _name = name;
            _text = text;
        }

        public QuickText(QuickText quick_text)
        {
            _name = quick_text.Name;
            _text = quick_text.Text;
        }

        public QuickText()
        {
            _name = string.Empty;
            _text = string.Empty;
        }
        #endregion
    }
}
