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
    public class Operand : Element
    {
        #region Properties
        private double _val;
        #endregion

        #region Accessors
        public double Val { get { return _val; } }
        #endregion

        #region Constructors
        public Operand(double val) : base(ElementType.Operand)
        {
            _val = val;
        }
        #endregion

        #region Public Methods
        public double Eval()
        {
            return _val;
        }
        #endregion
    }
}
