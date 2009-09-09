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
using System.Text.RegularExpressions;
using ContextLib.Algorithms.Diff;
using ContextLib.DataContainers.Multimedia;

namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public class SequentialIntFunction : Function
    {
        #region Properties
        private int _last_value;
        private int _increment;
        private int _padding;
        private int _skip_times;
        private int _occurrences;
        #endregion

        #region Accessors
        public int LastValue { get { return _last_value; } }
        public int Increment { get { return _increment; } }
        public int Padding { get { return _padding; } }
        public int Skip { get { return _skip_times; } set { _skip_times = value; } }
        public int NumberOfOccurrences { get { return _occurrences; } set { _occurrences = value; } }
        public override string Description
        {
            get { return Name + "(n+1) = " + Name + "(n) + " + _increment.ToString() + (_skip_times > 0 ? " (skip " + _skip_times + ")" : string.Empty) + ", " + Name + "(n) = " + _last_value.ToString().PadLeft(_padding, '0'); }
        }
        protected override string Definition
        {
            get { return "SIF(n+1) = SIF(n) + " + _increment.ToString() + (_skip_times > 0 ? " (skip " + _skip_times + ")" : string.Empty) + ", SIF(n) = " + _last_value.ToString().PadLeft(_padding, '0'); }
        }
        #endregion

        #region Constructors
        public SequentialIntFunction(string name, int last_val, int inc, int padding, int skip, int occurrences)
            : base(name, FunctionType.SequentialIntFunction)
        {
            _last_value = last_val;
            _increment = inc;
            _padding = padding;
            _skip_times = skip;
            _occurrences = occurrences;
        }
        #endregion

        #region Public Methods
        public int NextVal()
        {
            if (_skip_times > 0)
            {
                int dump;
                double test = ((double)_occurrences / (double)(_skip_times + 1));
                if ((test - Math.Floor(test)) == 0)
                    return _last_value + _increment;
                else
                    return _last_value;
            }
            else
            {
                return _last_value + _increment;
            }
        }

        public int[] NextVals(int n)
        {
            int last_val = _last_value;
            int last_occ = _occurrences;
            List<int> next_vals = new List<int>();
            for (int i = 0; i < n; i++, last_occ++)
            {
                if (_skip_times > 0)
                {
                    int dump;
                    double test = ((double)last_occ / (double)(_skip_times + 1));
                    if ((test - Math.Floor(test)) == 0)
                        last_val += _increment;
                }
                else
                {
                    last_val += _increment;
                }
                next_vals.Add(last_val);
            }
            return next_vals.ToArray();
        }

        public int PrevVal()
        {
            if (_skip_times > 0)
            {
                int dump;
                double test = (double)((_occurrences - 1) / (_skip_times + 1));
                if ((test - Math.Floor(test)) == 0)
                    return _last_value - _increment;
                else
                    return _last_value;
            }
            else
            {
                return _last_value - _increment;
            }
        }

        public int FirstVal()
        {
            if (_skip_times > 0)
            {
                return _last_value - (int)(_increment * Math.Floor(((double)_occurrences / (double)(_skip_times + 1))));
            }
            else
            {
                return _last_value - (_increment * _occurrences);
            }
        }

        public int[] AllVals(int n)
        {
            int last_val = FirstVal();
            int last_occ = 1;
            List<int> next_vals = new List<int>();
            for (int i = 0; i < n; i++, last_occ++)
            {
                if (_skip_times > 0)
                {
                    int dump;
                    double test = ((double)last_occ / (double)(_skip_times + 1));
                    if ((test - Math.Floor(test)) == 0)
                        last_val += _increment;
                }
                else
                {
                    last_val += _increment;
                }
                next_vals.Add(last_val);
            }
            return next_vals.ToArray();
        }

        public static bool Generate(string str1, string str2, out string expression, out List<SequentialIntFunction> functions)
        {
            expression = string.Empty;
            functions = new List<SequentialIntFunction>();
            List<Pair<Pair<int, string>, Pair<int, string>>> diffs = Diff.DiffString(str1, str2);
            if (diffs.Count > 0 /*&& Diff.IsDiffNumerical(diffs)*/)
            {
                //if (diffs[0].Second.Second == "2")
                //    System.Windows.Forms.MessageBox.Show("woot");
                Regex regex = new Regex(@"[0-9]+");
                MatchCollection mcol1 = regex.Matches(str1);
                MatchCollection mcol2 = regex.Matches(str2);

                if (mcol1.Count != mcol2.Count || mcol1.Count == 0)
                    return false;

                expression = str2;
                int val1, val2, padding, pos1, pos2;
                string f_name = string.Empty, to_replace = string.Empty;

                List<int> positions_to_replace = new List<int>();
                Dictionary<int, int> fpositions_to_replace = new Dictionary<int, int>();
                Dictionary<int, string> tokens_to_replace = new Dictionary<int, string>();
                Dictionary<int, SequentialIntFunction> funcs_to_add = new Dictionary<int, SequentialIntFunction>();

                for (int i = 1; i <= diffs.Count; i++)
                {
                    Pair<Pair<int, string>, Pair<int, string>> diff = diffs[i-1];
                    f_name = "§" + i.ToString();
                    //to_replace = diff.Second.Second;
                    //val1 = Int32.Parse(diff.First.Second);
                    //pos1 = diff.First.First;
                    //val2 = Int32.Parse(diff.Second.Second);
                    //pos2 = diff.Second.First;
                    //padding = 0;
                    val1 = val2 = pos1 = pos2 = padding = 0;
                    to_replace = string.Empty;
                    int offset = 0;
                    for (int n = 0; n < mcol2.Count; n++)
                    {
                        Match match1 = mcol1[n];
                        Match match2 = mcol2[n];
                        padding = match2.Length;
                        if (diff.Second.First >= match2.Index && diff.Second.First <= (match2.Index + match2.Length - 1) && diff.Second.First + diff.Second.Second.Length - 1 <= match2.Index + match2.Length - 1 &&
                            diff.First.First >= match1.Index && diff.First.First <= (match1.Index + match1.Length - 1) && diff.First.First + diff.First.Second.Length - 1 <= match1.Index + match1.Length - 1)
                        {
                            if (match1.Value.Length > 9)
                                offset = match1.Value.Length - 9;
                            else
                                offset = 0;
                            val1 = Int32.Parse(match1.Value.Substring(offset));
                            pos1 = match1.Index + offset;
                            if (match2.Value.Length > 9)
                                offset = match2.Value.Length - 9;
                            else
                                offset = 0;
                            val2 = Int32.Parse(match2.Value.Substring(offset));
                            pos2 = match2.Index + offset;
                            to_replace = match2.Value.Substring(offset);
                            break;
                        }
                        else
                        {
                            int result;
                            if (!Int32.TryParse(diff.First.Second, out result) && !Int32.TryParse(diff.Second.Second, out result))
                            {
                                return false;
                            }
                        }
                    }
                    if (pos1 == pos2 && to_replace != string.Empty)
                    {
                        //expression = expression.Replace(to_replace, f_name);
                        SequentialIntFunction func = new SequentialIntFunction(f_name, val2, val2 - val1, padding, 0, 2);
                        //if (!functions.Contains(func))
                        //    functions.Add(func);
                        if (!positions_to_replace.Contains(pos2))
                        {
                            positions_to_replace.Add(pos2);
                            fpositions_to_replace.Add(pos2, pos2 + to_replace.Length - 1);
                            tokens_to_replace.Add(pos2, f_name);
                            funcs_to_add.Add(pos2, func);
                        }
                    }
                }

                // add functions by its correct order
                positions_to_replace.Sort();
                foreach (int pos in positions_to_replace)
                {
                    functions.Add(funcs_to_add[pos]);
                }
                // replace text beginning from the end of the string
                positions_to_replace.Reverse();
                foreach (int pos in positions_to_replace)
                {
                    expression = SystemCore.SystemAbstraction.StringUtilities.StringUtility.ReplaceBetweenPositions(expression, pos, fpositions_to_replace[pos], tokens_to_replace[pos]);
                }
            }
            if (functions.Count > 0)
                return true;
            else
                return false;
        }

        public override object Clone()
        {
            return new SequentialIntFunction(this.Name, this.LastValue, this.Increment, this.Padding, this.Skip, this.NumberOfOccurrences);
        }
        #endregion
    }
}
