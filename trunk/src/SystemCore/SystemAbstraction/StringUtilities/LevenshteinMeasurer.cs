//#define MEMOIZATION
using System;
using System.Collections.Generic;
using System.Text;
#if MEMOIZATION
using Automator.Memoization;
#endif

namespace SystemCore.SystemAbstraction.StringUtilities
{
    public class LevenshteinMeasurer
    {
        #region Properties
        private static readonly LevenshteinMeasurer _instance = new LevenshteinMeasurer();
        #if MEMOIZATION
        private LookUpTable _look_up_table; // memoization
        #endif
        #endregion

        #region Accessors
        public static LevenshteinMeasurer Instance { get { return _instance; } }
        #endregion

        #region Constructors
        private LevenshteinMeasurer()
        {
            #if MEMOIZATION
            _look_up_table = new LookUpTable(1000);
            #endif
        }
        #endregion

        #region Public Methods
        public int GetDistance(string s, string t)
        {
            string ss = s.ToLower();
            string tt = t.ToLower();

            #if MEMOIZATION
            if (_look_up_table.Contains(ss, tt))
                return _look_up_table.GetValue(ss, tt);
            #endif

            int row_count = s.Length + 1;
            int column_count = t.Length + 1;
            int[,] matrix = new int[row_count, column_count];
            int cost = 0;

            for (int i = 0; i < row_count; i++)
                matrix[i, 0] = i;
            for (int j = 0; j < column_count; j++)
                matrix[0, j] = j;

            for (int i = 1; i < row_count; i++)
            {
                for (int j = 1; j < column_count; j++)
                {
                    if (ss[i - 1] == tt[j - 1])
                        cost = 0;
                    else
                        cost = 1;
                    matrix[i, j] = Math.Min(
                                        matrix[i - 1, j] + 1, // deletion
                                        Math.Min(
                                                matrix[i, j - 1] + 1, // insertion
                                                matrix[i - 1, j - 1] + cost // substitution
                                        )
                                );
                }
            }
            int dist = matrix[row_count - 1, column_count - 1];
            #if MEMOIZATION
            _look_up_table.Add(ss, tt, dist);
            #endif
            return dist;
        }
        #endregion
    }
}
