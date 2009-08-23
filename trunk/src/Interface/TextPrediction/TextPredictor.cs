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
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction.StringUtilities;
using System.Linq;

namespace Blaze.TextPrediction
{
    public class TextPredictor
    {
        #region Properties
        private static readonly TextPredictor _instance = new TextPredictor();
        #endregion

        #region Accessors
        public static TextPredictor Instance { get { return _instance; } }
        #endregion

        #region Constructors
        private TextPredictor() { }
        #endregion

        #region Public Methods
        public void PredictNamesAndDistance(string user_text, List<string> user_tokens, List<string> names, Dictionary<string, List<string>> keywords, ref List<string> accepted_names, ref Dictionary<string, double> accepted_distances, ref Dictionary<string, string[]> accepted_tokens, bool complex_mode)
        {
            int names_size = names.Count;
            int user_tokens_size = user_tokens.Count;

            for (int i = 0; i < names_size; i++)
            {
                List<string> keywords_accepted = new List<string>();
                Dictionary<string, double> keywords_distance = new Dictionary<string, double>();
                string name = names[i];
                List<string> nkeywords = new List<string>(keywords[name]);

                for (int z = 0; z < nkeywords.Count; z++)
                    nkeywords[z] = nkeywords[z].ToLower();
                nkeywords.RemoveAll(delegate(string s)
                        {
                            return s.Trim() == string.Empty;
                        });
                
                int nkeywords_size = nkeywords.Count;

                //new
                List<string> token_match = new List<string>();
                Dictionary<string, string> token_match_keyword = new Dictionary<string, string>();
                Dictionary<string, string> keyword_match_token = new Dictionary<string, string>();


                //if ((name == @"Google Chrome" || name == @"Google " + CommonInfo.GUID) && (user_text == "chrome google"))
                //    System.Windows.Forms.MessageBox.Show("test2");

                string magic_token = user_text.Replace(" ", string.Empty).Replace("_", string.Empty).Replace("-", string.Empty).Replace(".", string.Empty);
                bool is_command = (name.Contains(CommonInfo.GUID) ? true : false);
                double dist = 0, full_dist = -1;

                string tmp_name = (is_command ? Command.UnprotectCommand(name) : name);
                full_dist = StringUtility.WordContainsStrMeasure(tmp_name, user_text);
                if (full_dist == -1)
                    full_dist = StringUtility.WordContainsStrMeasure(tmp_name, magic_token);
                if (full_dist > -1)
                {
                    full_dist *= 2;
                }

                if (user_tokens.Count > nkeywords_size && !name.Contains(CommonInfo.GUID) && full_dist == -1)
                    continue;

                for (int j = 0; j < nkeywords_size; j++)
                {
                    string keyword = nkeywords[j];
                    for (int k = 0; k < user_tokens_size; k++)
                    {
                        string token = user_tokens[k];
                        bool token_fits_keyword = (complex_mode ? StringUtility.WordContainsStr(keyword, token) : keyword.Contains(token)); // token < keyword
                        bool keyword_fits_token = (complex_mode ? StringUtility.WordContainsStr(token, keyword) : false); // keyword < token
                        if (token_fits_keyword)
                            dist = CalcFittedWordDistance(keyword, token);
                        else if (keyword_fits_token)
                            dist = CalcFittedWordDistance(token, keyword);
                        else
                            dist = CalcNonFittedWordDistance(keyword, token);

                        if (dist < keyword.Length || is_command)
                        {
                            double offset = name.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase);
                            offset = (offset > -1 ? nkeywords.IndexOf(keyword) : keyword.Length);

                            if (token_fits_keyword)
                                dist += offset;
                            else if (keyword_fits_token)
                                dist = Math.Pow(Math.Exp(dist), 2) + offset;
                            else
                                dist = Math.Pow(Math.Exp(dist), 3) + offset;

                            if (!keywords_distance.ContainsKey(keyword))
                            {
                                if (!token_match.Contains(token))
                                {
                                    keywords_accepted.Add(keyword);
                                    keywords_distance.Add(keyword, dist);
                                    token_match.Add(token);
                                    token_match_keyword.Add(token, keyword);
                                    keyword_match_token.Add(keyword, token);
                                }
                                else
                                {
                                    string last_keyword = token_match_keyword[token];
                                    if (dist < keywords_distance[last_keyword])
                                    {
                                        keywords_accepted.Add(keyword);
                                        keywords_distance.Add(keyword, dist);
                                        token_match_keyword[token] = keyword;
                                        keyword_match_token[keyword] = token;
                                    }
                                }
                            }
                            else
                            {
                                double olddist = keywords_distance[keyword];
                                if (dist < olddist)
                                {
                                    if (!token_match.Contains(token))
                                    {
                                        keywords_distance[keyword] = dist;
                                        token_match.Add(token);
                                        foreach (var s in token_match_keyword.Where(o => o.Value == keyword).ToList())
                                        {
                                            token_match.Remove(s.Key);
                                            token_match_keyword.Remove(s.Key);
                                        }
                                        token_match_keyword.Add(token, keyword);
                                    }
                                    else
                                    {
                                        string last_keyword = token_match_keyword[token];
                                        if (dist < keywords_distance[last_keyword])
                                        {
                                            keywords_distance[keyword] = dist;
                                            token_match_keyword[token] = keyword;
                                        }
                                    }
                                    //if (!token_match_keyword.ContainsKey(token))
                                    //{
                                    //    keywords_distance[keyword] = dist;
                                    //    token_match.Add(token);
                                    //    token_match_keyword.Add(token, keyword);
                                    //    keyword_match_token[keyword] = token;
                                    //}
                                    //else
                                    //{
                                    //    keywords_distance[keyword] = dist;
                                    //    token_match_keyword[token] = keyword;
                                    //    keyword_match_token[keyword] = token;
                                    //}
                                }
                            }
                        }
                    }
                }
                List<string> keywords_to_be_removed = new List<string>();
                foreach (string keyword in keywords_accepted)
                {
                    if (!token_match_keyword.ContainsValue(keyword))
                        keywords_to_be_removed.Add(keyword);
                }
                foreach (string keyword in keywords_to_be_removed)
                {
                    keywords_accepted.Remove(keyword);
                    keywords_distance.Remove(keyword);
                }

                if (((is_command && user_tokens_size >= keywords_accepted.Count) ||
                    (!is_command && user_tokens_size == keywords_accepted.Count)) &&
                    keywords_accepted.Count > 0)
                {
                    double total_distance = 0;
                    foreach (string keyword in keywords_accepted)
                    {
                        total_distance += keywords_distance[keyword];
                    }

                    accepted_names.Add(name);
                    if (full_dist != -1)
                    {
                        if (full_dist < total_distance)
                        {
                            total_distance = full_dist;
                            accepted_tokens.Add(name, user_tokens.ToArray());
                        }
                        else
                        {
                            accepted_tokens.Add(name, token_match.ToArray());
                        }
                    }
                    else
                    {
                        accepted_tokens.Add(name, token_match.ToArray());
                    }
                    accepted_distances.Add(name, total_distance);
                }
                else if (full_dist > -1)
                {
                    accepted_names.Add(name);
                    accepted_distances.Add(name, full_dist);
                    accepted_tokens.Add(name, user_tokens.ToArray());
                }
            }
        }
        #endregion

        #region Private Methods
        private int CalcFittedWordDistance(string big_word, string small_word)
        {
            return StringUtility.WordContainsStrMeasure(big_word, small_word);
        }

        private int CalcNonFittedWordDistance(string word1, string word2)
        {
            return LevenshteinMeasurer.Instance.GetDistance(word2, word1);
        }

        #endregion
    }
}
