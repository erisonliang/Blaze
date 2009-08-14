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
namespace Calculator
{
    public enum ParenthesisType
    {
        Open, Close, None
    }

    public class Parenthesis : Element
    {
        #region Properties
        private ParenthesisType _type;
        #endregion

        #region Accessors
        public ParenthesisType Type { get { return _type; } }
        #endregion

        #region Constructors
        public Parenthesis(ParenthesisType type) : base(ElementType.Parenthesis)
        {
            _type = type;
        }
        #endregion
    }
}
