using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContextLib.Algorithms.Diff;
using ContextLib.DataContainers.Multimedia;
using System.Text.RegularExpressions;

namespace ContextLib.DataContainers.Monitoring.Generalization
{
    public class SequentialIntFunction : Function
    {
        #region Properties
        private int _last_value;
        private int _increment;
        private int _padding;
        private int _skip_times;
        private int _occurrences;
        private string _folder;
        private string[] _extensions;
        #endregion

        #region Accessors
        public int LastValue { get { return _last_value; } }
        public int Increment { get { return _increment; } }
        public int Padding { get { return _padding; } }
        public int Skip { get { return _skip_times; } set { _skip_times = value; } }
        public int NumberOfOccurrences { get { return _occurrences; } set { _occurrences = value; } }
        public string Folder { get { return _folder; } }
        public string[] Extensions { get { return _extensions; } set { _extensions = value; } }
        public override string Description
        {
            get { return Name + "(n+1) = " + Name + "(n) + " + _increment.ToString() + (_skip_times > 0 ? " (skip " + _skip_times + ")" : string.Empty) + ", " + Name + "(n)  = " + _last_value.ToString().PadLeft(_padding, '0'); }
        }
        protected override string Definition
        {
            get { return "SIF(n+1) = SIF(n) + " + _increment.ToString() + (_skip_times > 0 ? " (skip " + _skip_times + ")" : string.Empty) + ", SIF(n)  = " + _last_value.ToString().PadLeft(_padding, '0'); }
        }
        #endregion

        #region Constructors
        public SequentialIntFunction(string name, int last_val, int inc, int padding, int skip, int occurrences, string folder, string[] extensions)
            : base(name, FunctionType.SequentialIntFunction)
        {
            _last_value = last_val;
            _increment = inc;
            _padding = padding;
            _skip_times = skip;
            _occurrences = occurrences;
            _folder = folder;
            _extensions = extensions;
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
                    double test = (double)(last_occ / (_skip_times + 1));
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

        public static bool Generate(string str1, string str2, string folder, string extensions, out string expression, out List<SequentialIntFunction> functions)
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

                if (mcol1.Count != mcol2.Count)
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
                    for (int n = 0; n < mcol2.Count; n++)
                    {
                        Match match = mcol2[n];
                        padding = match.Length;
                        if (diff.Second.First >= match.Index && diff.Second.First <= (match.Index + match.Length - 1))
                        {
                            val1 = Int32.Parse(mcol1[n].Value);
                            pos1 = mcol1[n].Index;
                            val2 = Int32.Parse(match.Value);
                            pos2 = match.Index;
                            to_replace = match.Value;
                            break;
                        }
                    }
                    if (pos1 == pos2 && to_replace != string.Empty)
                    {
                        //expression = expression.Replace(to_replace, f_name);
                        SequentialIntFunction func = new SequentialIntFunction(f_name, val2, val2 - val1, padding, 0, 2, folder, extensions);
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
        #endregion
    }
}
