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
    public enum ElementType
    {
        Operator, Operand, Parenthesis
    }

    public abstract class Element
    {
        #region Properties
        ElementType _element_type;
        #endregion

        #region Accessors
        public ElementType ElementType { get { return _element_type; } }
        #endregion

        #region Constructors
        public Element(ElementType element_type)
        {
            _element_type = element_type;
        }
        #endregion
    }
}
