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
using System.Collections.Generic;

namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public class ConstantFileExtFunction : Function
    {
        #region Properties
        private string _folder;
        private string[] _extensions;
        private int _occurrences;
        #endregion

        #region Accessors
        public string Folder { get { return _folder; } }
        public string[] Extensions { get { return _extensions; } set { _extensions = value; } }
        public int NumberOfOccurrences { get { return _occurrences; } set { _occurrences = value; } }
        public override string Description
        {
            get
            {
                return Name + "(n) = " + _folder + "\\" + "*" + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty);
            }
        }
        protected override string Definition
        {
            get
            {
                return "CFFex(n) = " + _folder + "\\" + "*" + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty);
            }
        }
        #endregion

        #region Constructors
        public ConstantFileExtFunction(string name, string folder, string[] extensions)
            : base(name, FunctionType.ConstantFileExtFunction)
        {
            _folder = folder;
            _extensions = extensions;
        }
        #endregion

        #region Public Methods
        public string NextVal()
        {
            return _folder + "\\" + "*" + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty);
        }

        public static bool Generate(string folder, string[] extensions,out string expression, out List<ConstantFileExtFunction> functions)
        {
            functions = new List<ConstantFileExtFunction>();
            string f_name = "§1";
            expression = f_name;
            ConstantFileExtFunction cfef = new ConstantFileExtFunction(f_name, folder, extensions);
            functions.Add(cfef);
            return true;
        }

        public override object Clone()
        {
            string[] exts = new string[this.Extensions.Length];
            Array.Copy(this.Extensions, exts, this.Extensions.Length);
            return new ConstantFileExtFunction(this.Name, this.Folder, exts);
        }
        #endregion
    }
}
