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
    public class SequentialCharFunction : Function
    {
        #region Properties
        private char _last_value;
        private int _increment;
        private int _skip_times;
        private int _occurrences;
        #endregion

        #region Accessors
        public char LastValue { get { return _last_value; } }
        public int Increment { get { return _increment; } }
        public int Skip { get { return _skip_times; } set { _skip_times = value; } }
        public int NumberOfOccurrences { get { return _occurrences; } set { _occurrences = value; } }
        public override string Description
        {
            get { return Name + "(n+1) = " + Name + "(n) + " + _increment.ToString() + (_skip_times > 0 ? " (skip " + _skip_times + ")" : string.Empty) + ", " + Name + "(n) = '" + _last_value.ToString() + "'"; }
        }
        protected override string Definition
        {
            get { return "SCF(n+1) = SCF(n) + " + _increment.ToString() + (_skip_times > 0 ? " (skip " + _skip_times + ")" : string.Empty) + ", SCF(n) = '" + _last_value.ToString() + "'"; }
        }
        #endregion

        #region Constructors
        public SequentialCharFunction(string name, char last_val, int inc, int skip, int occurrences)
            : base(name, FunctionType.SequentialCharFunction)
        {
            _last_value = last_val;
            _increment = inc;
            _skip_times = skip;
            _occurrences = occurrences;
        }
        #endregion

        #region Public Methods
        public char NextVal()
        {
            char ret_char;
            int int_char = (int)_last_value;
            if (_skip_times > 0)
            {
                int dump;
                double test = ((double)_occurrences / (double)(_skip_times + 1));
                if ((test - Math.Floor(test)) == 0)
                    int_char += _increment;
            }
            else
            {
                int_char += _increment;
            }
            try
            {
                ret_char = Convert.ToChar(int_char);
            }
            catch
            {
                ret_char = Char.MinValue;
            }
            return ret_char;
        }

        public char[] NextVals(int n)
        {
            int last_val = (int)_last_value;
            int last_occ = _occurrences;
            List<char> next_vals = new List<char>();
            for (int i = 0; i < n; i++, last_occ++)
            {
                char add_char;
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
                try
                {
                    add_char = Convert.ToChar(last_val);
                }
                catch
                {
                    add_char = Char.MinValue;
                }
                next_vals.Add(add_char);
            }
            return next_vals.ToArray();
        }

        public char PrevVal()
        {
            char ret_char;
            int int_char = (int)_last_value;
            if (_skip_times > 0)
            {
                int dump;
                double test = (double)((_occurrences - 1) / (_skip_times + 1));
                if ((test - Math.Floor(test)) == 0)
                    int_char -= _increment;
            }
            else
            {
                int_char -= _increment;
            }
            try
            {
                ret_char = Convert.ToChar(int_char);
            }
            catch
            {
                ret_char = Char.MinValue;
            }
            return ret_char;
        }

        public char FirstVal()
        {
            char ret_char;
            int int_char;
            if (_skip_times > 0)
            {
                int_char = _last_value - (int)(_increment * Math.Floor(((double)_occurrences / (double)(_skip_times + 1))));
                try
                {
                    ret_char = Convert.ToChar(int_char);
                }
                catch
                {
                    ret_char = Char.MinValue;
                }
                return ret_char;
            }
            else
            {
                int_char = _last_value - (_increment * _occurrences);
                try
                {
                    ret_char = Convert.ToChar(int_char);
                }
                catch
                {
                    ret_char = Char.MinValue;
                }
                return ret_char;
            }
        }

        public char[] AllVals(int n)
        {
            int last_val = (int)FirstVal();
            int last_occ = 1;
            List<char> next_vals = new List<char>();
            for (int i = 0; i < n; i++, last_occ++)
            {
                char add_char;
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
                try
                {
                    add_char = Convert.ToChar(last_val);
                }
                catch
                {
                    add_char = Char.MinValue;
                }
                next_vals.Add(add_char);
            }
            return next_vals.ToArray();
        }

        public static bool Generate(string str1, string str2, out string expression, out List<SequentialCharFunction> functions)
        {
            expression = string.Empty;
            functions = new List<SequentialCharFunction>();
            List<Pair<Pair<int, string>, Pair<int, string>>> diffs = Diff.DiffString(str1, str2);
            if (diffs.Count > 0 && Diff.IsDiffChar(diffs))
            {
                expression = str2;
                int val1, val2, pos1, pos2;
                string f_name = string.Empty;

                List<int> positions_to_replace = new List<int>();
                Dictionary<int, int> fpositions_to_replace = new Dictionary<int, int>();
                Dictionary<int, string> tokens_to_replace = new Dictionary<int, string>();
                Dictionary<int, SequentialCharFunction> funcs_to_add = new Dictionary<int, SequentialCharFunction>();

                for (int i = 1; i <= diffs.Count; i++)
                {
                    Pair<Pair<int, string>, Pair<int, string>> diff = diffs[i - 1];
                    f_name = diff.Second.Second.Substring(0, diff.Second.Second.Length - 1) + "§" + i.ToString();
                    val1 = (int)diff.First.Second[diff.First.Second.Length - 1];
                    pos1 = diff.First.First;
                    val2 = (int)diff.Second.Second[diff.Second.Second.Length - 1];
                    pos2 = diff.Second.First;
                    if (pos1 == pos2)
                    {
                        //expression = expression.Replace(diff.Second.Second, f_name);
                        SequentialCharFunction func = new SequentialCharFunction(f_name, (char)val2, val2 - val1, 0, 2);
                        //functions.Add(new SequentialCharFunction(f_name, (char)val2, val2 - val1, 0, 2));
                        if (!positions_to_replace.Contains(pos2))
                        {
                            positions_to_replace.Add(pos2);
                            fpositions_to_replace.Add(pos2, pos2);
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
            return new SequentialCharFunction(this.Name, this.LastValue, this.Increment, this.Skip, this.NumberOfOccurrences);
        }
        #endregion
    }
}
