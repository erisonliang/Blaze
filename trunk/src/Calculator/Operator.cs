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
using System;

namespace Calculator
{
    public enum OperatorType
    {
        Plus, Minus, Multiply, Divide, Remainder, Powers, Percentage, None
    }

    public enum OperatorPriority
    {
        Low, Medium, High, None
    }

    public class Operator : Element
    {
        #region Properties
        private OperatorType _type;
        private OperatorPriority _priority;
        #endregion

        #region Accessors
        public OperatorType Type { get { return _type; } }
        public OperatorPriority Priority { get { return _priority; } }
        #endregion

        #region Constructors
        public Operator(OperatorType type) : base(ElementType.Operator)
        {
            _type = type;
            if (type == OperatorType.Powers || type == OperatorType.Percentage)
                _priority = OperatorPriority.High;
            else if (type == OperatorType.Multiply || type == OperatorType.Divide || type == OperatorType.Remainder)
                _priority = OperatorPriority.Medium;
            else
                _priority = OperatorPriority.Low;
        }
        #endregion

        #region Public Methods
        public double Eval(double a, double b)
        {
            switch (_type)
            {
                case OperatorType.Plus:
                    return a + b;
                case OperatorType.Minus:
                    return a - b;
                case OperatorType.Multiply:
                    return a * b;
                case OperatorType.Divide:
                    if (b == 0)
                        return double.PositiveInfinity;
                    else
                        return a / b;
                case OperatorType.Remainder:
                    if (b == 0)
                        return double.PositiveInfinity;
                    else
                        return a % b;
                case OperatorType.Powers:
                    return Math.Pow(a, b);
                case OperatorType.Percentage:
                    return (a/100)*b;
                default:
                    return 0;
            }
        }
        #endregion
    }
}
