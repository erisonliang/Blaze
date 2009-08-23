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
using System.Linq;
using ContextLib.Algorithms.Diff;
using ContextLib.DataContainers.Multimedia;

namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public class ConstantFileFunctionEx : Function
    {
        #region Properties
        private string[] _contents;
        private string _folder;
        private string[] _extensions;
        private int _occurrences;
        #endregion

        #region Accessors
        public string[] Contents { get { return _contents; } set { _contents = value; } }
        public string Folder { get { return _folder; } }
        public string[] Extensions { get { return _extensions; } set { _extensions = value; } }
        public int NumberOfOccurrences { get { return _occurrences; } set { _occurrences = value; } }
        public override string Description
        {
            get
            {
                return Name + "(n) = " + _folder + "\\*" + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty) +" containg \"" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_contents) + "\"";
            }
        }
        protected override string Definition
        {
            get
            {
                return "CFFex(n) = " + _folder + "\\*" + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty) +" containg \"" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_contents) + "\"";
            }
        }
        #endregion

        #region Constructors
        public ConstantFileFunctionEx(string name, string[] contents, string folder, string[] extensions)
            : base(name, FunctionType.ConstantFileFunctionEx)
        {
            _contents = contents;
            _folder = folder;
            _extensions = extensions;
        }
        #endregion

        #region Public Methods
        public string NextVal()
        {
            return _folder + "\\*" + (_extensions.Length > 0 ? ".(" + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_extensions) + ")" : string.Empty) +" containg " + SystemCore.SystemAbstraction.StringUtilities.StringUtility.ArrayToStr(_contents);
        }

        public static bool Generate(string file1, string file2, string folder, string[] extensions,out string expression, out List<ConstantFileFunctionEx> functions)
        {
            expression = string.Empty;
            functions = new List<ConstantFileFunctionEx>();
            List<Pair<Pair<int, string>, Pair<int, string>>> diffs = Diff.DiffString(file1, file2);
            if (diffs.Count > 0)
            {
                //int file1_last_token_beginning = 0, file2_last_token_beginning = 0;
                string token = string.Empty;
                HashSet<string> file1_tokens = new HashSet<string>();
                HashSet<string> file2_tokens = new HashSet<string>();
                HashSet<string> common_tokens = new HashSet<string>();
                char[] splitter = new char[] { ' ', '_', '-', '.', '\'', ',' };

                //// grab the constant tokens
                //for (int i = 0; i < diffs.Count; i++)
                //{
                //    Pair<Pair<int, string>, Pair<int, string>> diff = diffs[i];

                //    if (diff.First.First > file1_last_token_beginning)
                //    {
                //        token = file1.Substring(file1_last_token_beginning, diff.First.First - file1_last_token_beginning);
                //        file1_tokens.Add(token);
                //    }
                //    file1_last_token_beginning = diff.First.First + diff.First.Second.Length;

                //    if (diff.Second.First > file2_last_token_beginning)
                //    {
                //        token = file2.Substring(file2_last_token_beginning, diff.Second.First - file2_last_token_beginning);
                //        file2_tokens.Add(token);
                //    }
                //    file2_last_token_beginning = diff.Second.First + diff.Second.Second.Length;
                //}
                //if (file1_last_token_beginning < file1.Length)
                //{
                //    token = file1.Substring(file1_last_token_beginning, file1.Length - file1_last_token_beginning);
                //    file1_tokens.Add(token);
                //}
                //if (file2_last_token_beginning < file2.Length)
                //{
                //    token = file2.Substring(file2_last_token_beginning, file2.Length - file2_last_token_beginning);
                //    file2_tokens.Add(token);
                //}

                //// refine file1 tokens
                //HashSet<string> refined_tokens = new HashSet<string>();
                //foreach (string t in file1_tokens)
                //{
                //    string[] ref_ts = t.Trim().Split(new char[] { ' ' });
                //    foreach (string ref_t in ref_ts)
                //        if (ref_t.Trim() != string.Empty)
                //            refined_tokens.Add(ref_t.ToLower());
                //}
                //file1_tokens = refined_tokens;

                //// refine file2 tokens
                //refined_tokens.Clear();
                //foreach (string t in file2_tokens)
                //{
                //    string[] ref_ts = t.Trim().Split(new char[] { ' ' });
                //    foreach (string ref_t in ref_ts)
                //        if (ref_t.Trim() != string.Empty)
                //            refined_tokens.Add(ref_t.ToLower());
                //}
                //file2_tokens = refined_tokens;

                file1_tokens = new HashSet<string>(file1.Split(splitter, StringSplitOptions.RemoveEmptyEntries));
                file2_tokens = new HashSet<string>(file2.Split(splitter, StringSplitOptions.RemoveEmptyEntries));

                foreach (string token1 in file1_tokens)
                {
                    foreach (string token2 in file2_tokens)
                    {
                        if (token1.ToLower() == token2.ToLower())
                        {
                            if (token1.Length > 1)
                            {
                                common_tokens.Add(token1.ToLower());
                                break;
                            }
                        }
                    }
                }

                // if there is more than one common token, there's a generalization
                if (common_tokens.Count > 0)
                {
                    string f_name = "§1";
                    expression = f_name;
                    ConstantFileFunctionEx cffe = new ConstantFileFunctionEx(f_name, common_tokens.ToArray<string>(), folder, extensions);
                    functions.Add(cffe);
                }
            }
            if (functions.Count > 0)
                return true;
            else
                return false;
        }

        public override object Clone()
        {
            string[] contents = new string[this.Contents.Length];
            string[] exts = new string[this.Extensions.Length];
            Array.Copy(this.Contents, contents, this.Contents.Length);
            Array.Copy(this.Extensions, exts, this.Extensions.Length);
            return new ConstantFileFunctionEx(this.Name, contents, this.Folder, exts);
        }
        #endregion
    }
}
