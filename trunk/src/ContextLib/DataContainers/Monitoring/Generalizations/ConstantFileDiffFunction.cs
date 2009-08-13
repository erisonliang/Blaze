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
    public class ConstantFileDiffFunction : Function
    {
        #region Properties
        private string _folder;
        private int[] _original_positions;
        private int[] _replacement_positions;
        private string[] _original_tokens;
        private string[] _replacement_tokens;
        private string[] _extensions;
        private int _occurrences;
        #endregion

        #region Accessors
        public string Folder { get { return _folder; } }
        public int[] OriginalPositions { get { return _original_positions; } set { _original_positions = value; } }
        public int[] ReplacementPositions { get { return _replacement_positions; } set { _replacement_positions = value; } }
        public string[] OriginalTokens { get { return _original_tokens; } set { _original_tokens = value; } }
        public string[] ReplacementTokens { get { return _replacement_tokens; } set { _replacement_tokens = value; } }
        public string[] Extensions { get { return _extensions; } set { _extensions = value; } }
        public int NumberOfOccurrences { get { return _occurrences; } set { _occurrences = value; } }
        public override string Description
        {
            get
            {
                return Name + "(n) = " + _folder + "\\" + "*" + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty) + " with " + GenOriginalString() + " replace " + GenReplacementString();
            }
        }
        protected override string Definition
        {
            get
            {
                return "CFFex(n) = " + _folder + "\\" + "*" + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty) + " with " + GenOriginalString() + " replace " + GenReplacementString();
            }
        }
        #endregion

        #region Constructors
        public ConstantFileDiffFunction(string name, int[] original_positions, int[] replacement_positions, string[] original_tokens, string[] replacement_tokens, string folder, string[] extensions)
            : base(name, FunctionType.ConstantFileDiffFunction)
        {
            _folder = folder;
            _original_positions = original_positions;
            _replacement_positions = replacement_positions;
            _original_tokens = original_tokens;
            _replacement_tokens = replacement_tokens;
            _extensions = extensions;
        }
        #endregion

        #region Public Methods
        public string NextVal()
        {
            return _folder + "\\" + "*" + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty) + " with " + GenOriginalString() + " replace " + GenReplacementString();
        }

        public static bool Generate(string old_file_name1, string new_file_name1, string old_file_name2, string new_file_name2, string folder, string[] extensions, out string expression, out List<ConstantFileDiffFunction> functions)
        {
            expression = string.Empty;
            functions = new List<ConstantFileDiffFunction>();

            List<Pair<Pair<int, string>, Pair<int, string>>> diffs1 = Diff.DiffString(old_file_name1, new_file_name1);
            List<Pair<Pair<int, string>, Pair<int, string>>> diffs2 = Diff.DiffString(old_file_name2, new_file_name2);

            // if all diffs are identical, between the old and new names...
            if (Diff.IsDiffIdentical(diffs1, diffs2) && diffs1.Count > 0)
            {
                List<int> original_positions = new List<int>();
                List<int> replacement_positions = new List<int>();
                List<string> original_tokens = new List<string>();
                List<string> replacement_tokens = new List<string>();

                // store them as they can be reproduced and there is a generalization
                for (int i = 0; i < diffs2.Count; i++)
                {
                    Pair<Pair<int, string>, Pair<int, string>> diff = diffs2[i];
                    original_positions.Add(diff.First.First);
                    replacement_positions.Add(diff.Second.First);
                    original_tokens.Add(diff.First.Second);
                    replacement_tokens.Add(diff.Second.Second);
                }

                if (original_positions.Count > 0) // doh, of course it is... xD
                {
                    string f_name = "§1";
                    expression = f_name;
                    ConstantFileDiffFunction cfdf = new ConstantFileDiffFunction(f_name, original_positions.ToArray(), replacement_positions.ToArray(), original_tokens.ToArray(), replacement_tokens.ToArray(), folder, extensions);
                    functions.Add(cfdf);
                }
            }

            if (functions.Count > 0)
                return true;
            else
                return false;
        }

        public override object Clone()
        {
            int[] ipos = new int[this.OriginalPositions.Length];
            int[] fpos = new int[this.ReplacementPositions.Length];
            string[] itokens = new string[this.OriginalTokens.Length];
            string[] ftokens = new string[this.ReplacementTokens.Length];
            string[] exts = new string[this.Extensions.Length];
            Array.Copy(this.OriginalPositions, ipos, this.OriginalPositions.Length);
            Array.Copy(this.ReplacementPositions, fpos, this.ReplacementPositions.Length);
            Array.Copy(this.OriginalTokens, itokens, this.OriginalTokens.Length);
            Array.Copy(this.ReplacementTokens, ftokens, this.ReplacementTokens.Length);
            Array.Copy(this.Extensions, exts, this.Extensions.Length);
            return new ConstantFileDiffFunction(this.Name, ipos, fpos, itokens, ftokens, this.Folder, exts);
        }
        #endregion

        #region Private Methods
        private string GenOriginalString()
        {
            string str = string.Empty;
            for (int i = 0; i < _replacement_positions.Length; i++)
            {
                str += "(" + _original_positions[i] + ",\"" + _original_tokens[i] + "\")";
            }
            return str;
        }

        private string GenReplacementString()
        {
            string str = string.Empty;
            for (int i = 0; i < _replacement_positions.Length; i++)
            {
                str += "(" + _replacement_positions[i] + ",\"" + _replacement_tokens[i] + "\")";
            }
            return str;
        }
        #endregion
    }
}
