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

namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public class ConstantTextFunction : Function
    {
        #region Properties
        private string _constant;
        private int _occurrences;
        #endregion

        #region Accessors
        public string Constant { get { return _constant; } }
        public int NumberOfOccurrences { get { return _occurrences; } set { _occurrences = value; } }
        public override string Description
        {
            get { return Name + "(n) = " + _constant; }
        }
        protected override string Definition
        {
            get { return "CTF(n) = " + _constant; }
        }
        #endregion

        #region Constructors
        public ConstantTextFunction(string name, string constant, int occurrences)
            : base(name, FunctionType.ConstantTextFunction)
        {
            _constant = constant;
            _occurrences = occurrences;
        }
        #endregion

        #region Public Methods
        public string NextVal()
        {
            return _constant;
        }

        public static bool Generate(string str1, string str2, out string expression, out List<ConstantTextFunction> functions)
        {
            expression = string.Empty;
            functions = new List<ConstantTextFunction>();
            string f_name = string.Empty;
            if (str1 == str2)
            {
                f_name = "§1";
                expression = f_name;
                functions.Add(new ConstantTextFunction(f_name, str2, 2));
            }
            if (functions.Count > 0)
                return true;
            else
                return false;
        }

        public override object Clone()
        {
            return new ConstantTextFunction(this.Name, this.Constant, this.NumberOfOccurrences);
        }
        #endregion
    }
}
