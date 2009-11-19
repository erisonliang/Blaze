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
using ContextLib.Algorithms.Diff;
using ContextLib.DataContainers.Multimedia;

namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public class ConstantFileFunction : Function
    {
        #region Properties
        private string _beginning;
        private string _ending;
        private string _folder;
        private string[] _extensions;
        private int _occurrences;
        #endregion

        #region Accessors
        public string Biginning { get { return _beginning; } set { _beginning = value; } }
        public string Ending { get { return _ending; } set { _ending = value; } }
        public string Folder { get { return _folder; } }
        public string[] Extensions { get { return _extensions; } set { _extensions = value; } }
        public int NumberOfOccurrences { get { return _occurrences; } set { _occurrences = value; } }
        public override string Description
        {
            get
            {
                return Name + "(n) = " + _folder + "\\" + _beginning + "*" + _ending + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty);
            }
        }
        protected override string Definition
        {
            get
            {
                return "CFF(n) = " + _folder + "\\" + _beginning + "*" + _ending + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty);
            }
        }
        #endregion

        #region Constructors
        public ConstantFileFunction(string name, string beginning, string ending, string folder, string[] extensions)
            : base(name, FunctionType.ConstantFileFunction)
        {
            _beginning = beginning;
            _ending = ending;
            _folder = folder;
            _extensions = extensions;
        }
        #endregion

        #region Public Methods
        public string NextVal()
        {
            return _folder + "\\" + _beginning + "*" + _ending + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty);
        }

        public static bool Generate(string file1, string file2, string folder, string[] extensions, out string expression, out List<ConstantFileFunction> functions)
        {
            expression = string.Empty;
            functions = new List<ConstantFileFunction>();
            List<Pair<Pair<int, string>, Pair<int, string>>> diffs = Diff.DiffString(file1, file2);
            if (diffs.Count > 0)
            {
                string file1_beginning, file1_ending, file2_beginning, file2_ending;
                string common_beginning, common_ending;
                int file1_beginning_fpos, file1_ending_ipos, file2_beginning_fpos, file2_ending_ipos;
                bool has_beginning, has_ending;
                file1_beginning = file1_ending = file2_beginning = file2_ending = common_beginning = common_ending = string.Empty;
                file1_beginning_fpos = file1_ending_ipos = file2_beginning_fpos = file2_ending_ipos = -1;
                has_beginning = has_ending = false;

                // get the ending positions of the beginning of the file name or the first position of file name's ending
                if (diffs[0].First.First > 0)
                    file1_beginning_fpos = diffs[0].First.First - 1;
                else
                    file1_ending_ipos = diffs[diffs.Count - 1].First.First + diffs[diffs.Count - 1].First.Second.Length;
                if (diffs[0].Second.First > 0)
                    file2_beginning_fpos = diffs[0].Second.First - 1;
                else
                    file2_ending_ipos = diffs[diffs.Count - 1].Second.First + diffs[diffs.Count - 1].Second.Second.Length;

                // if the final position of the beginning of the file name was found, then try getting te first position of the file name's ending
                if (file1_beginning_fpos != -1 && diffs.Count > 1)
                {
                    file1_ending_ipos = diffs[diffs.Count - 1].First.First + diffs[diffs.Count - 1].First.Second.Length;
                }
                if (file2_beginning_fpos != -1 && diffs.Count > 1)
                {
                    file2_ending_ipos = diffs[diffs.Count - 1].Second.First + diffs[diffs.Count - 1].Second.Second.Length;
                }

                // generate the beginning and ending tokens
                if (file1_beginning_fpos != -1)
                {
                    file1_beginning = file1.Substring(0, file1_beginning_fpos + 1).Trim().ToLower();
                }
                if (file1_ending_ipos != -1)
                {
                    file1_ending = file1.Substring(file1_ending_ipos, file1.Length - file1_ending_ipos).Trim().ToLower();
                }
                if (file2_beginning_fpos != -1)
                {
                    file2_beginning = file2.Substring(0, file2_beginning_fpos + 1).Trim().ToLower();
                }
                if (file2_ending_ipos != -1)
                {
                    file2_ending = file2.Substring(file2_ending_ipos, file2.Length - file2_ending_ipos).Trim().ToLower();
                }

                // check if there is a common file name beginning and ending
                if (file1_beginning != string.Empty && file1_beginning == file2_beginning)
                {
                    has_beginning = true;
                    common_beginning = file2_beginning;
                }
                if (file1_ending != string.Empty && file1_ending == file2_ending)
                {
                    has_ending = true;
                    common_ending = file2_ending;
                }

                // if there was a beginning or and ending in common, so there's a generalization
                if (has_beginning || has_ending)
                {
                    string f_name = "§1";
                    expression = f_name;
                    ConstantFileFunction cff = new ConstantFileFunction(f_name, common_beginning, common_ending, folder, extensions);
                    functions.Add(cff);
                }

            }
            if (functions.Count > 0)
                return true;
            else
                return false;
        }

        public override object Clone()
        {
            string[] exts = new string[this.Extensions.Length];
            Array.Copy(this.Extensions, exts, this.Extensions.Length);
            return new ConstantFileFunction(this.Name, this.Biginning, this.Ending, this.Folder, exts);
        }
        #endregion
    }
}
