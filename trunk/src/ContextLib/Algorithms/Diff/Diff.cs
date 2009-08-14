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
using ContextLib.DataContainers.Multimedia;

namespace ContextLib.Algorithms.Diff
{
    public static class Diff
    {
        #region Constructors
        #endregion

        #region Public Methods
        public static List<Pair<Pair<int, string>, Pair<int, string>>> DiffString(string a, string b)
        {
            List<Pair<Pair<int, string>, Pair<int, string>>> ret_diffs = new List<Pair<Pair<int, string>, Pair<int, string>>>();
            int[] a_codes = DiffCharCodes(a, false);
            int[] b_codes = DiffCharCodes(b, false);
            SystemCore.SystemAbstraction.StringUtilities.Diff.Item[] diffs = SystemCore.SystemAbstraction.StringUtilities.Diff.DiffInt(a_codes, b_codes);

            int pos = 0;
            for (int n = 0; n < diffs.Length; n++)
            {
                SystemCore.SystemAbstraction.StringUtilities.Diff.Item item = diffs[n];
                Pair<Pair<int, string>, Pair<int, string>> new_diff = new Pair<Pair<int, string>, Pair<int, string>>();
                int pos_a = item.StartA, pos_b = item.StartB;
                string str_a = string.Empty, str_b = string.Empty;

                // get deleted portion of a
                if (item.deletedA > 0)
                {
                    for (int m = 0; m < item.deletedA; m++)
                    {
                        str_a += a[item.StartA + m];
                    }
                }

                // get inserted portion of b
                if (item.insertedB > 0)
                {
                    for (int m = 0; m < item.insertedB; m++)
                    {
                        str_b += b[item.StartB + m];
                    }
                }
                //if (pos < item.StartB + item.insertedB)
                //{
                //    while (pos < item.StartB + item.insertedB)
                //    {
                //        str_b += b[pos];
                //        pos++;
                //    }
                //}


                new_diff.First = new Pair<int, string>();
                new_diff.First.First = pos_a;
                new_diff.First.Second = str_a;
                new_diff.Second = new Pair<int, string>();
                new_diff.Second.First = pos_b;
                new_diff.Second.Second = str_b;
                ret_diffs.Add(new_diff);
            }

            return ret_diffs;
        }

        public static bool IsDiffNumerical(List<Pair<Pair<int, string>, Pair<int, string>>> diffs)
        {
            //Regex regex = new Regex(@"[0-9]+");
            int val;
            foreach (Pair<Pair<int, string>, Pair<int, string>> diff in diffs)
            {
                //if (diff.First.First != diff.Second.First)
                //    return false;
                //else
                bool is_first_int = Int32.TryParse(diff.First.Second, out val);
                bool is_second_int = Int32.TryParse(diff.Second.Second, out val);
                if (!is_first_int || !is_second_int)
                    return false;
            }
            return true;
        }

        public static bool IsDiffChar(List<Pair<Pair<int, string>, Pair<int, string>>> diffs)
        {
            foreach (Pair<Pair<int, string>, Pair<int, string>> diff in diffs)
            {
                //if (diff.First.First != diff.Second.First)
                //    return false;
                //else
                if (diff.First.Second.Length == 0 || diff.Second.Second.Length == 0 || (diff.First.Second.Length != diff.Second.Second.Length))
                    return false;
                else if (Math.Abs((int)diff.First.Second[diff.First.Second.Length - 1] - (int)diff.Second.Second[diff.Second.Second.Length - 1]) != 1 ||
                         diff.First.Second.Substring(0, diff.First.Second.Length - 1) != diff.Second.Second.Substring(0, diff.First.Second.Length - 1))
                    return false;
            }
            return true;
        }

        public static bool IsDiffIdentical(List<Pair<Pair<int, string>, Pair<int, string>>> diff_a, List<Pair<Pair<int, string>, Pair<int, string>>> diff_b)
        {
            if (diff_a.Count != diff_b.Count)
                return false;
            else
            {
                for (int i = 0; i < diff_a.Count; i++)
                {
                    if (/*diff_a[i].First.First != diff_b[i].First.First ||*/
                        diff_a[i].Second.First != diff_b[i].Second.First ||
                        /*diff_a[i].First.Second != diff_b[i].First.Second ||*/
                        diff_a[i].Second.Second != diff_b[i].Second.Second)
                        return false;
                }
            }
            return true;
        }
        #endregion

        #region Private Methods
        private static int[] DiffCharCodes(string aText, bool ignoreCase)
        {
            int[] Codes;

            if (ignoreCase)
                aText = aText.ToUpperInvariant();

            Codes = new int[aText.Length];

            for (int n = 0; n < aText.Length; n++)
                Codes[n] = (int)aText[n];

            return (Codes);
        }
        #endregion
    }
}
