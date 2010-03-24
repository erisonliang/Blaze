//#define MEMOIZATION
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCore.SystemAbstraction.StringUtilities
{
    public static class EditDistanceMeasurer
    {
        #region Public Methods
        public static int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (!string.IsNullOrEmpty(t))
                    return t.Length;
                else
                    return 0;
            }

            if (string.IsNullOrEmpty(t))
            {
                if (!string.IsNullOrEmpty(s))
                    return t.Length;
                else
                    return 0;
            }

            string ss = s.ToLower();
            string tt = t.ToLower();

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
                    matrix[i, j] = min(
                                        matrix[i - 1, j] + 1, // deletion
                                        min(
                                                matrix[i, j - 1] + 1, // insertion
                                                matrix[i - 1, j - 1] + cost // substitution
                                        )
                                );
                }
            }
            int dist = matrix[row_count - 1, column_count - 1];
            return dist;
        }

        public static int DamerauLevenshteinDistance(string s, string t)
        {
            //string str1 = s.ToLower();
            //string str2 = t.ToLower();

            int n = s.Length + 1;
            int m = t.Length + 1;

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            int[,] matrix = new int[n, m];
            int cost = 0;

            for (int i = 0; i < n; i++)
                matrix[i, 0] = i;
            for (int j = 0; j < m; j++)
                matrix[0, j] = j;

            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < m; j++)
                {
                    if (s[i - 1] == t[j - 1])
                        cost = 0;
                    else
                        cost = 1;

                    matrix[i, j] = min(
                                        matrix[i - 1, j] + 1, // deletion
                                        min(
                                                matrix[i, j - 1] + 1, // insertion
                                                matrix[i - 1, j - 1] + cost // substitution
                                        )
                                );

                    if (i > 1 && j > 1 && s[i - 1] == t[j - 2] && s[i - 2] == t[j - 1])
                    {
                        matrix[i,j] = min(
                                                matrix[i, j],
                                                matrix[i - 2, j - 2] + cost // transposition
                                      );
                    }
                }
            }
            return matrix[n - 1, m - 1];
        }
        #endregion

        #region Private Methods
        private static int min(int a, int b)
        {
            if (a <= b)
                return a;
            else
                return b;
        }
        #endregion
    }
}
