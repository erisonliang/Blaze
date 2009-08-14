using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SystemCore.SystemAbstraction.StringUtilities
{
    public static class StringUtility
    {
        public static int MaxLen(string str1, string str2)
        {
            int len1 = str1.Length, len2 = str2.Length;
            if (len1 >= len2)
                return len1;
            else
                return len2;
        }

        public static bool WordContainsStr(string word, string str)
        {
            string s = word.ToLower();
            string t = str.ToLower();
            string temp = s;
            int len = t.Length;
            for (int i = 0; i < len; i++)
            {
                int index = temp.IndexOf(t[i]);
                if (index > -1)
                {
                    temp = temp.Substring(index + 1);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static int WordContainsStrMeasure(string word, string str)
        {
            string s = word.ToLower();
            string t = str.ToLower();
            string temp = s;
            int len = t.Length;
            int first = 0;
            int measure = 0;
            for (int i = 0; i < len; i++)
            {
                int index = temp.IndexOf(t[i]);
                if (index > -1)
                {
                    if (i == 0)
                    {
                        first = index;
                    }
                    else
                    {
                        measure += index * 3 ;
                    }
                    temp = temp.Substring(index + 1);
                }
                else
                {
                    return -1;
                }
            }
            measure += first;
            measure += temp.Length;//word.Length - (str.Length + first);
            return measure;
        }

        public static bool WordContainsWord(string word1, string word2)
        {
            string s = word1.ToLower();
            string t = word2.ToLower();
            int slen = s.Length;
            int tlen = t.Length;
            string temp;

            if (slen >= tlen)
            {
                temp = s;
                for (int i = 0; i < tlen; i++)
                {
                    int index = temp.IndexOf(t[i]);
                    if (index > -1)
                    {
                        temp = temp.Substring(index + 1);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                temp = t;
                for (int i = 0; i < slen; i++)
                {
                    int index = temp.IndexOf(s[i]);
                    if (index > -1)
                    {
                        temp = temp.Substring(index + 1);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static int WordStructuralDistance(string word1, string word2)
        {
            string s1 = word1.ToLower();
            string s2 = word2.ToLower();
            int s1len = s1.Length;
            int s2len = s2.Length;
            int dist = 0;
            int last_pos = 0;
            if (s1len >= s2len)
            {
                for (int i = 0; i < s2len; i++)
                {
                    last_pos = i;
                    int index = s1.IndexOf(s2[i]);
                    if (index > -1)
                    {
                        s1 = s1.Substring(index + 1);
                        dist += index;
                    }
                    else
                    {
                        return s1len;
                    }
                }
                return dist * 10 + (s1len - (last_pos+1));
            }
            else
            {
                for (int i = 0; i < s1len; i++)
                {
                    last_pos = i;
                    int index = s2.IndexOf(s1[i]);
                    if (index > -1)
                    {
                        s2 = s2.Substring(index + 1);
                        dist += index;
                    }
                    else
                    {
                        return s2len;
                    }
                }
                return dist * 10 +(s2len - (last_pos + 1));
            }
        }

        public static int WordBeginningDistance(string phrase, string word)
        {
            string s1 = phrase.ToLower();
            string s2 = word.ToLower();
            int index = s1.IndexOf(s2);
            if (index == -1)
                return phrase.Length;
            else
                return index;
        }

        public static string[] StrToArray(string str)
        {
            List<string> ret = new List<string>(str.Split(new char[] { ' ', ',' }));
            ret.RemoveAll(delegate(string s)
            {
                return s.Trim() == string.Empty;
            });
            return ret.ToArray();
        }

        public static string ArrayToStr(string[] arr)
        {
            string ret = string.Empty;
            if (arr != null)
            {
                int len = arr.Length;
                for (int i = 0; i < len; i++)
                {
                    if (i == len - 1)
                        ret += arr[i];
                    else
                        ret += arr[i] + ", ";
                }
            }
            return ret;
        }

        public static string[] GenerateKeywords(string str)
        {
            return GenerateKeywords(str, true);
        }

        public static string[] GenerateKeywords(string str, bool lower)
        {
            List<string> tokens;
            if (lower)
                tokens = new List<string>(str.ToLower().Split(new char[] { ' ', '_', '-', '(', ')', '[', ']', '{', '}' }, StringSplitOptions.RemoveEmptyEntries));
            else
                tokens = new List<string>(str.Split(new char[] { ' ', '_', '-', '(', ')', '[', ']', '{', '}' }, StringSplitOptions.RemoveEmptyEntries));
            bool can_remove_possible_indefinite_article = false;
            //bool can_remove_possible_preposition = false;
            //bool can_remove_possible_definite_article = false;
            if (tokens.FindAll(delegate(string s) { return s.Length > 1; }).Count > 0)
                can_remove_possible_indefinite_article = true;
            //if (list_keywords.FindAll(delegate(string s) { return s.Length > 2; }).Count > 2)
            //    can_remove_possible_preposition = true;
            //if (list_keywords.FindAll(delegate(string s) { return s.Length > 3; }).Count > 6)
            //    can_remove_possible_preposition = true;
            tokens.RemoveAll(delegate(string s)
            {
                if (s == string.Empty)
                    return true;
                else if (s.Length == 1 && can_remove_possible_indefinite_article && Char.IsLetter(s[0]))
                    return true;
                //else if (s.Length == 2 && can_remove_possible_preposition)
                //    return true;
                //else if (s.Length == 3 && can_remove_possible_definite_article)
                //    return true;
                else
                    return false;
            });
            return tokens.Distinct().ToArray();
        }

        /// <summary>
        /// Replaces the first occurrence of a string found within 
        /// another string.
        /// </summary>
        /// <param name="original">The original System.String to have the replacement.</param>
        /// <param name="oldValue">A System.String to be replaced.</param>
        /// <param name="newValue">A System.String to replace the 
        /// first occurrence of oldValue.</param>
        public static string ReplaceFirstOccurrence(string original,
            string oldValue, string newValue)
        {
            int loc = original.IndexOf(oldValue);
            if (loc > -1)
                return original.Remove(loc, oldValue.Length).Insert(loc, newValue);
            else return original;
        } // http://fortycal.blogspot.com/2007/07/replace-first-occurrence-of-string-in-c.html

        /// <summary>
        /// Replaces the last occurrence of a string found within 
        /// another string.
        /// </summary>
        /// <param name="original">The original System.String to have the replacement.</param>
        /// <param name="oldValue">A System.String to be replaced.</param>
        /// <param name="newValue">A System.String to replace the 
        /// first occurrence of oldValue.</param>
        public static string ReplaceLastOccurrence(string original,
            string oldValue, string newValue)
        {
            int loc = original.LastIndexOf(oldValue);
            if (loc > -1)
                return original.Remove(loc, oldValue.Length).Insert(loc, newValue);
            else return original;
        }

        /// <summary>
        /// Replaces the first occurrence of a string found within 
        /// another string, without regarding the capitalization.
        /// </summary>
        /// <param name="original">The original System.String to have the replacement.</param>
        /// <param name="oldValue">A System.String to be replaced.</param>
        /// <param name="newValue">A System.String to replace the 
        /// first occurrence of oldValue.</param>
        public static string ReplaceFirstOccurrenceNoCaps(string original,
            string oldValue, string newValue)
        {
            int loc = original.ToLower().IndexOf(oldValue.ToLower());
            if (loc > -1)
                return original.Remove(loc, oldValue.Length).Insert(loc, newValue);
            else
                return original;
        } // http://fortycal.blogspot.com/2007/07/replace-first-occurrence-of-string-in-c.html

        /// <summary>
        /// Replaces the last occurrence of a string found within 
        /// another string, without regarding the capitalization.
        /// </summary>
        /// <param name="original">The original System.String to have the replacement.</param>
        /// <param name="oldValue">A System.String to be replaced.</param>
        /// <param name="newValue">A System.String to replace the 
        /// first occurrence of oldValue.</param>
        public static string ReplaceLastOccurrenceNoCaps(string original,
            string oldValue, string newValue)
        {
            int loc = original.ToLower().LastIndexOf(oldValue.ToLower());
            if (loc > -1)
                return original.Remove(loc, oldValue.Length).Insert(loc, newValue);
            else
                return original;
        }

        public static string ReplaceBetweenPositions(string source, int ipos, int fpos, string new_value)
        {
            if (ipos < 0 || ipos >= source.Length || fpos < ipos || fpos >= source.Length)
            {
                return source;
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < source.Length; i++)
                {
                    if (i == ipos)
                    {
                        for (int j = 0; j < new_value.Length; j++)
                        {
                            builder.Append(new_value[j]);
                        }
                        i = fpos;
                    }
                    else
                    {
                        builder.Append(source[i]);
                    }
                }
                return builder.ToString();
            }
        }
    }
}
