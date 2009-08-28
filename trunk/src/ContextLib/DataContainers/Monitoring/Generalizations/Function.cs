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
namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public abstract class Function
    {
        #region Properties
        private FunctionType _type;
        private string _name;
        #endregion

        #region Public Enums
        public enum FunctionType : int
        {
            SequentialIntFunction = 0x0001,
            SequentialCharFunction = 0x0002,
            ConstantTextFunction = 0x0004,
            ConstantFileFunction = 0x0008,
            ConstantFileFunctionEx = 0x0010,
            ConstantFileExtFunction = 0x0020,
            ConstantFileDiffFunction = 0x0040
        }
        #endregion

        #region Accessors
        public string Name { get { return _name; } }
        public FunctionType Type { get { return _type; } }
        public abstract string Description { get; }
        protected abstract string Definition { get; }
        #endregion

        #region Constructors
        public Function(string name, FunctionType type)
        {
            _name = name;
            _type = type;
        }
        #endregion

        #region Public Methods
        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            Function function = (Function)obj;
            if (function == null) // check if it can be casted
                return false;

            if (function.Definition == this.Definition)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Definition.GetHashCode();
        }

        public abstract object Clone();

        public override string ToString()
        {
            return Description;
        }
        #endregion
    }
}
