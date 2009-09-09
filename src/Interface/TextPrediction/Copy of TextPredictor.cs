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
using SystemCore.Settings;
using SystemCore.SystemAbstraction.StringUtilities;
using Configurator;

namespace Blaze.TextPrediction
{
    public class TextPredictorBackUp
    {
        #region Properties
        private static readonly TextPredictorBackUp _instance = new TextPredictorBackUp();
        #endregion

        #region Accessors
        public static TextPredictorBackUp Instance { get { return _instance; } }
        #endregion

        #region Constructors
        private TextPredictorBackUp() { }
        #endregion

        #region Public Methods
        public void PredictNamesAndDistance(string user_text, List<string> user_tokens, List<string> names, Dictionary<string, List<string>> keywords, ref List<string> accepted_names, ref Dictionary<string, int> accepted_distances, ref Dictionary<string, string[]> accepted_tokens, bool complex_mode)
        {
            int names_size = names.Count;
            int user_tokens_size = user_tokens.Count;
            //LearnedContent learned_contents = SettingsManager.Instance.GetLearnedContents();
            List<string> contained_names = new List<string>();
            Dictionary<string, int> contained_distances = new Dictionary<string, int>();
            Dictionary<string, string[]> contained_tokens = new Dictionary<string, string[]>();

            List<string> effective_names = new List<string>();
            Dictionary<string, int> effective_distances = new Dictionary<string, int>();
            Dictionary<string, string[]> effective_tokens = new Dictionary<string, string[]>();

            List<string> relative_names = new List<string>();
            Dictionary<string, int> relative_distances = new Dictionary<string, int>();
            Dictionary<string, string[]> relative_tokens = new Dictionary<string, string[]>();

            List<string> alternative_names = new List<string>();
            Dictionary<string, int> alternative_distances = new Dictionary<string, int>();
            Dictionary<string, string[]> alternative_tokens = new Dictionary<string, string[]>();

            for (int i = 0; i < names_size; i++)
            {
                List<string> keywords_accepted = new List<string>();
                Dictionary<string, int> keywords_distance = new Dictionary<string, int>();
                string name = names[i];
                List<string> nkeywords = new List<string>(keywords[name]);
                //if (learned_contents.Contents.ContainsKey(name))
                //{
                //    List<string> temp_keywords = learned_contents.Contents[name];
                //    foreach (string key in temp_keywords)
                //        nkeywords.AddRange(key.Split(new char[] { ' ', '_', '-', '(', ')', '[', ']', '{', '}' }));
                //}
                for (int z = 0; z < nkeywords.Count; z++)
                    nkeywords[z] = nkeywords[z].ToLower();
                nkeywords.RemoveAll(delegate(string s)
                        {
                            return s.Trim() == string.Empty;
                        });
                //string[] nkeywords = temp_keys.ToArray();
                int nkeywords_size = nkeywords.Count;
                //int word_distance = 0;
                //nkeywords.Sort(delegate(string a, string b)
                //{
                //    if (a.Length == b.Length)
                //        return a.CompareTo(b);
                //    else
                //        return b.Length.CompareTo(a.Length);
                //});

                //new
                List<string> alternative_keywords_accepted = new List<string>();
                Dictionary<string, string> alternative_keyword_match = new Dictionary<string, string>();
                Dictionary<string, int> alternative_keywords_distance = new Dictionary<string, int>();
                List<string> token_match = new List<string>();
                Dictionary<string, string> token_match_keyword = new Dictionary<string, string>();
                Dictionary<string, int> keyword_ordering_distance = new Dictionary<string, int>();
                Dictionary<string, int> alternative_keyword_ordering_distance = new Dictionary<string, int>();

                //if ((name == @"What's new" || name == @"Add New Command " + CommonInfo.GUID) && (user_text == "add new"))
                //    MessageBox.Show("test2");

                bool must_continue = false;
                string magic_token = user_text.Replace(" ", string.Empty).Replace("_", string.Empty).Replace("-", string.Empty).Replace(".", string.Empty);

                int measure, magic_measure;
                string closest_token = string.Empty;
                int closest_measure = -1;
                int closest_offset = -1;
                int matched_toks = 0;
                bool is_command = (name.Contains(CommonInfo.GUID) ? true : false);

                for (int n = 0; n < nkeywords_size; n++)
                {
                    string tok = nkeywords[n];
                    if (tok == magic_token)
                    {
                        matched_toks++;
                        if (closest_measure == -1 || closest_measure > 0)
                        {
                            closest_token = tok;
                            closest_measure = 0;
                            //if (closest_offset == -1 || closest_offset > n)
                            closest_offset = n;
                        }
                        continue;
                    }
                    for (int m = 0; m < user_tokens_size; m++)
                    {
                        string utok = user_tokens[m];
                        measure = StringUtility.WordContainsStrMeasure(tok, utok);
                        if (measure > -1)
                        {
                            matched_toks++;
                            if (closest_measure == -1 || measure < closest_measure)
                            {
                                closest_token = utok;
                                closest_measure = measure;
                                //if (closest_offset == -1 || closest_offset > n)
                                closest_offset = n;
                                //break;
                            }
                        }
                    }
                }

                if (closest_measure != -1)
                {
                    must_continue = true;
                    contained_names.Add(name);
                    contained_distances.Add(name, closest_measure + closest_offset + (is_command ? (nkeywords_size - matched_toks) : (int)Math.Max((user_tokens_size - matched_toks), (nkeywords_size - matched_toks))));
                    contained_tokens.Add(name, new string[] { closest_token });
                }

                if (must_continue)
                {
                    continue;
                }

                string tmp_name = (is_command ? Command.UnprotectCommand(name) : name);
                measure = StringUtility.WordContainsStrMeasure(tmp_name, user_text);
                magic_measure = StringUtility.WordContainsStrMeasure(tmp_name, magic_token);
                if (measure > -1 || magic_measure > -1)
                {
                    contained_names.Add(name);
                    contained_tokens.Add(name, new string[] { user_text });
                    if (measure > -1 && magic_measure > -1)
                        contained_distances.Add(name, (int)Math.Min(measure, magic_measure));
                    else if (measure > -1)
                        contained_distances.Add(name, measure);
                    else
                        contained_distances.Add(name, magic_measure);
                    continue;
                }

                if (/*user_text.Length > name.Length || */user_tokens.Count > nkeywords_size && !name.Contains(CommonInfo.GUID))
                    continue;

                for (int j = 0; j < nkeywords_size; j++)
                {
                    string keyword = nkeywords[j];
                    for (int k = 0; k < user_tokens_size; k++)
                    {
                        string token = user_tokens[k];
                        bool token_fits_keyword = (complex_mode ? StringUtility.WordContainsStr(keyword, token) : keyword.Contains(token)); // token < keyword
                        bool keyword_fits_token = (complex_mode ? StringUtility.WordContainsStr(token, keyword) : false); // keyword < token
                        if (token_fits_keyword || keyword_fits_token) // token < keyword - best case
                        {
                            //word_distance += WordFLDistance(keyword, token);
                            if (token_fits_keyword)
                            {
                                if (!keywords_distance.ContainsKey(keyword))
                                {
                                    keywords_accepted.Add(keyword);
                                    keywords_distance.Add(keyword, CalcFittedWordDistance(keyword, token));
                                    keyword_ordering_distance.Add(keyword, StringUtility.WordStructuralDistance(keyword, token));
                                    if (!token_match.Contains(token))
                                    {
                                        token_match.Add(token);
                                        token_match_keyword.Add(token, keyword);
                                    }
                                    else
                                    {
                                        if (CalcFittedWordDistance(keyword, token) < keywords_distance[token_match_keyword[token]])
                                            token_match_keyword[token] = keyword;
                                    }
                                }
                                else
                                {
                                    int newdist = CalcFittedWordDistance(keyword, token);
                                    int olddist = keywords_distance[keyword];
                                    if (newdist < olddist)
                                    {
                                        keywords_distance[keyword] = newdist;
                                        keyword_ordering_distance[keyword] = StringUtility.WordStructuralDistance(keyword, token);
                                        if (!token_match.Contains(token))
                                        {
                                            token_match.Add(token);
                                            token_match_keyword.Add(token, keyword);
                                        }
                                        else
                                        {
                                            token_match_keyword[token] = keyword;
                                        }
                                    }
                                }
                            }
                            else // keyword < token - worse case
                            {
                                if (!keywords_distance.ContainsKey(keyword))
                                {
                                    int newdist = CalcFittedWordDistance(keyword, token);
                                    if (newdist <= 3)
                                    {
                                        keywords_accepted.Add(keyword);
                                        keywords_distance.Add(keyword, newdist);
                                        keyword_ordering_distance.Add(keyword, StringUtility.WordStructuralDistance(keyword, token));
                                        if (!token_match.Contains(token))
                                        {
                                            token_match.Add(token);
                                            token_match_keyword.Add(token, keyword);
                                        }
                                        else
                                        {
                                            if (newdist < keywords_distance[token_match_keyword[token]])
                                                token_match_keyword[token] = keyword;
                                        }
                                    }
                                }
                                else
                                {
                                    int newdist = CalcFittedWordDistance(keyword, token);
                                    int olddist = keywords_distance[keyword];
                                    if (newdist < olddist)
                                    {
                                        keywords_distance[keyword] = newdist;
                                        keyword_ordering_distance[keyword] = StringUtility.WordStructuralDistance(keyword, token);
                                        if (!token_match.Contains(token))
                                        {
                                            token_match.Add(token);
                                            token_match_keyword.Add(token, keyword);
                                        }
                                        else
                                        {
                                            token_match_keyword[token] = keyword;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //word_distance += (token.Length >= keyword.Length ? token.Length : keyword.Length);
                            //if (token_match.Contains(token))
                            //    continue;
                            if (!alternative_keywords_distance.ContainsKey(keyword))
                            {
                                int newdist = CalcNonFittedWordDistance(keyword, token);
                                alternative_keywords_accepted.Add(keyword);
                                alternative_keywords_distance.Add(keyword, newdist);
                                alternative_keyword_match.Add(keyword, token);
                                alternative_keyword_ordering_distance.Add(keyword, StringUtility.WordStructuralDistance(keyword, token));
                            }
                            else
                            {
                                int newdist = CalcNonFittedWordDistance(keyword, token); ;
                                int olddist = alternative_keywords_distance[keyword];
                                if (newdist < olddist)
                                {
                                    alternative_keywords_distance[keyword] = newdist;
                                    alternative_keyword_match[keyword] = token;
                                    alternative_keyword_ordering_distance[keyword] = StringUtility.WordStructuralDistance(keyword, token);
                                }
                            }
                        }
                    }
                }
                foreach (string k in keywords_accepted)
                {
                    if (alternative_keywords_distance.ContainsKey(k))
                    {
                        alternative_keywords_accepted.Remove(k);
                        alternative_keyword_match.Remove(k);
                        alternative_keywords_distance.Remove(k);
                    }
                }
                //if (name == "Microsoft Office Word 2007" && user_text == "word")
                //    MessageBox.Show("test1");
                //if (name == "Ranetki Girls - O Tebe Video Clip" && user_text == "word")
                //    MessageBox.Show("test2");
                //if (name == "world of goo" && user_text == "word")
                //    MessageBox.Show("test4");
                //if (i == 283 && user_text == "rametki girls")
                //    MessageBox.Show("test3");

                //if (name == "Patch scripts folder" && user_text == "scripts folder")
                //    MessageBox.Show("woohoo");
                ////if (name == "Google " + CommonInfo.GUID && user_text == "google broculos")
                ////    MessageBox.Show("woohoo");
                //if (name == "Scripts Folder " + CommonInfo.GUID && user_text == "scripts folder")
                //    MessageBox.Show("testing");

                //if (name == "d:\\erm\\irk.txt" && user_text == "d:\\erm\\i")
                //    MessageBox.Show("bikez0r");
                //if (name == "d:\\erm\\" && user_text == "d:\\erm\\i")
                //    MessageBox.Show("bikez0r");
                //if (name == @"Backup and Restore Center" && user_text == "email an")
                //    MessageBox.Show("test1");
                //if (name == "email " + CommonInfo.GUID && user_text == "email an")
                //    MessageBox.Show("test2");

                //if ((name == @"redo " + CommonInfo.GUID /*|| name == @"continue " + CommonInfo.GUID*/) && (user_text == "continue 10" || user_text == "redo 10"))
                //    MessageBox.Show("test2");

                if (token_match.Count == user_tokens_size) // all tokens were matched
                {
                    int distance = 0;
                    int penalty = 0;
                    //bool special = false;
                    //List<int> special_distance = new List<int>();
                    //if (learned_contents.Keywords.Contains(name))
                    //{
                    //    foreach (string keyword in keywords_accepted)
                    //    {
                    //        if (learned_contents.Contents[name].Contains(keyword))
                    //        {
                    //            special = true;
                    //            special_distance.Add(keywords_distance[keyword]);
                    //        }
                    //    }
                    //}

                    //if (special)
                    //{
                    //    special_distance.Sort();
                    //    distance = special_distance[0];
                    //}
                    //else
                    //{
                    foreach (string keyword in keywords_accepted)
                    {
                        if (token_match_keyword.ContainsValue(keyword))
                        {
                            penalty += 25 * keyword_ordering_distance[keyword];
                        }
                    }
                    //for (int m = 0; m < keywords_accepted.Count; m++)
                    //{
                    //    distance += keywords_distance[keywords_accepted[m]];
                    //    //distance += 12*keyword_ordering_distance[keywords_accepted[m]]; // weight disordering
                    //}

                    //for (int n = 0; n < alternative_keywords_accepted.Count; n++)
                    //{
                    //    distance += alternative_keywords_distance[alternative_keywords_accepted[n]];
                    //    //distance += 12*alternative_keyword_ordering_distance[alternative_keywords_accepted[n]];
                    //}
                    //// weight how far from the beginning of the name it is
                    //int min = -1;
                    //foreach (string t in token_match)
                    //{
                    //    if (min == -1)
                    //    {
                    //        min = Math.Max(StringUtility.WordBeginningDistance(user_text, t), StringUtility.WordBeginningDistance(name, token_match_keyword[t]));// StringUtility.WordBeginningDistance(name, token_match_keyword[t]); //(WordContainsStr(token_match_keyword[t], t) ? WordBeginningDistance(name, t) : WordBeginningDistance(name, token_match_keyword[t])); -> fica -1 :(
                    //    }
                    //    else
                    //    {
                    //        int newmin = Math.Max(StringUtility.WordBeginningDistance(user_text, t), StringUtility.WordBeginningDistance(name, token_match_keyword[t]));// StringUtility.WordBeginningDistance(name, token_match_keyword[t]); //(WordContainsStr(token_match_keyword[t], t) ? WordBeginningDistance(name, t) : WordBeginningDistance(name, token_match_keyword[t]));
                    //        if (newmin < min)
                    //            min = newmin;
                    //    }
                    //    //distance += nkeywords.IndexOf(token_match_keyword[t]) * keyword_ordering_distance[token_match_keyword[t]];
                    //}
                    //penalty += 3 * min;
                    //}
                    effective_names.Add(name);
                    effective_distances.Add(name, distance + penalty);
                    List<string> tokens = new List<string>();
                    foreach (string token in token_match)
                    {
                        tokens.Add(token);
                    }
                    effective_tokens.Add(name, tokens.ToArray());
                }
                else if (keywords_accepted.Count > 0)
                {
                    int distance = 0;
                    int error = 0;
                    int penalty = 60 * user_tokens_size; // penalty for not all user tokens being matched
                    bool reject = false;
                    foreach (string keyword in keywords_accepted)
                    {
                        int dist = keywords_distance[keyword];
                        if (token_match_keyword.ContainsValue(keyword))
                        {
                            penalty += 25 * keyword_ordering_distance[keyword];
                            error += dist;
                            //error += 2*keyword_ordering_distance[keyword];
                        }
                        else if (dist > 3)
                        {
                            reject = true;
                            break;
                        }
                        //distance += dist;
                    }
                    //for (int m = 0; m < keywords_accepted.Count; m++)
                    //{
                    //    distance += keywords_distance[keywords_accepted[m]];
                    //    //distance += 12 * keyword_ordering_distance[keywords_accepted[m]]; // weight disordering
                    //}
                    // weight how far from the beginning of the name it is
                    int min = -1;
                    foreach (string t in token_match)
                    {
                        if (min == -1)
                        {
                            min = Math.Max(StringUtility.WordBeginningDistance(user_text, t), StringUtility.WordBeginningDistance(name, token_match_keyword[t]));// StringUtility.WordBeginningDistance(name, token_match_keyword[t]); //(WordContainsStr(token_match_keyword[t], t) ? WordBeginningDistance(name, t) : WordBeginningDistance(name, token_match_keyword[t])); -> fica -1 :(
                        }
                        else
                        {
                            int newmin = Math.Max(StringUtility.WordBeginningDistance(user_text, t), StringUtility.WordBeginningDistance(name, token_match_keyword[t]));// StringUtility.WordBeginningDistance(name, token_match_keyword[t]); //(WordContainsStr(token_match_keyword[t], t) ? WordBeginningDistance(name, t) : WordBeginningDistance(name, token_match_keyword[t]));
                            if (newmin < min)
                                min = newmin;
                        }
                        ////distance += nkeywords.IndexOf(token_match_keyword[t]) * keyword_ordering_distance[token_match_keyword[t]];
                    }
                    penalty += 3 * min;
                    for (int n = 0; n < alternative_keywords_accepted.Count; n++)
                    {
                        string keyword = alternative_keywords_accepted[n];
                        //string token = alternative_keyword_match[keyword];
                        int dist = alternative_keywords_distance[keyword];
                        //if (token_match_keyword.ContainsKey(token))
                        //    continue;
                        //if (dist > 3)
                        //{
                        //    reject = true;
                        //    break;
                        //}
                        //distance += dist;
                    }
                    penalty += nkeywords_size - 1;
                    if (!reject && error <= (user_tokens_size < 6 ? 3 * user_tokens_size : 15) /*&& distance <= (user_tokens_size < 9 ? 4 * user_tokens_size : 32)*/)
                    {
                        relative_names.Add(name);
                        relative_distances.Add(name, distance + penalty);
                        List<string> tokens = new List<string>();
                        foreach (string token in token_match)
                        {
                            tokens.Add(token);
                        }
                        relative_tokens.Add(name, tokens.ToArray());
                    }
                }
                else if (alternative_keywords_accepted.Count > 0)
                {
                    int distance = 0;
                    int penalty = 1200 * user_tokens_size; // penalty for none of the user tokens being matched
                    bool reject = false;

                    List<string> disordered_keywords = new List<string>(alternative_keywords_accepted);
                    List<string> ordered_keywords = new List<string>(alternative_keywords_accepted.Count);
                    for (int q = 0; q < alternative_keywords_accepted.Count; q++)
                    {
                        int min = -1;
                        string min_key = string.Empty;
                        foreach (string keyword in disordered_keywords)
                        {
                            int val = alternative_keywords_distance[keyword];
                            if (min == -1)
                            {
                                min = val;
                                min_key = keyword;
                            }
                            else
                            {
                                if (val < min)
                                {
                                    min = val;
                                    min_key = keyword;
                                }
                            }
                        }
                        ordered_keywords.Add(min_key);
                        disordered_keywords.Remove(min_key);
                    }

                    //int[] alt_vals = new int[alternative_keywords_distance.Values.Count];
                    //alternative_keywords_distance.Values.CopyTo(alt_vals, 0);
                    //Array.Sort<int>(alt_vals);

                    // escolher os menors das alternativas de acordo com o numero de tokens
                    List<string> tokens = new List<string>();
                    for (int n = 0; n < ordered_keywords.Count && n < user_tokens_size; n++)
                    {
                        string keyword = ordered_keywords[n];
                        int dist = alternative_keywords_distance[keyword];
                        if (dist > 3)
                        {
                            reject = true;
                            break;
                        }
                        else
                        {
                            distance += dist;
                            tokens.Add(alternative_keyword_match[keyword]);
                        }
                    }

                    //for (int n = 0; n < alternative_keywords_accepted.Count && n < user_tokens_size; n++)
                    //{
                    //    int dist = alt_vals[n];
                    //    if (dist > 3)
                    //    {
                    //        reject = true;
                    //        break;
                    //    }
                    //    else
                    //        distance += alt_vals[n];
                    //}
                    if (!reject && distance <= (user_tokens_size < 6 ? 3 * user_tokens_size : 15)) // limitar o distance
                    {
                        alternative_names.Add(name);
                        alternative_distances.Add(name, distance + penalty);
                        //List<int> displacement = new List<int>();
                        //foreach (string keyword in alternative_keywords_accepted)
                        //{
                        //    displacement.Add(user_text.IndexOf(alternative_keyword_match[keyword]));
                        //}
                        alternative_tokens.Add(name, tokens.ToArray());
                    }
                }
            }
            if (contained_names.Count > 0)
            {
                accepted_names = contained_names;
                accepted_distances = contained_distances;
                accepted_tokens = contained_tokens;
                int missing = SettingsManager.Instance.GetNumberOfSuggestions() - accepted_names.Count;
                if (missing > 0)
                {
                    for (int i = 0; i < effective_distances.Count && i < missing; i++)
                    {
                        string name = effective_names[i];
                        if (!accepted_names.Contains(name))
                        {
                            accepted_names.Add(name);
                            accepted_distances.Add(name, effective_distances[name]);
                            accepted_tokens.Add(name, effective_tokens[name]);
                        }
                        else if (accepted_distances[name] > effective_distances[name])
                        {
                            accepted_distances[name] = effective_distances[name];
                            accepted_tokens[name] = effective_tokens[name];
                        }
                    }
                    missing = SettingsManager.Instance.GetNumberOfSuggestions() - accepted_names.Count;
                    for (int i = 0; i < relative_distances.Count && i < missing; i++)
                    {
                        string name = relative_names[i];
                        if (!accepted_names.Contains(name))
                        {
                            accepted_names.Add(name);
                            accepted_distances.Add(name, relative_distances[name]);
                            accepted_tokens.Add(name, relative_tokens[name]);
                        }
                        else if (accepted_distances[name] > relative_distances[name])
                        {
                            accepted_distances[name] = relative_distances[name];
                            accepted_tokens[name] = relative_tokens[name];
                        }
                    }
                    missing = SettingsManager.Instance.GetNumberOfSuggestions() - accepted_names.Count;
                    if (missing > 0)
                    {
                        for (int i = 0; i < alternative_names.Count && i < missing; i++)
                        {
                            string name = alternative_names[i];
                            if (!accepted_names.Contains(name))
                            {
                                accepted_names.Add(name);
                                accepted_distances.Add(name, alternative_distances[name]);
                                accepted_tokens.Add(name, alternative_tokens[name]);
                            }
                            else if (accepted_distances[name] > alternative_distances[name])
                            {
                                accepted_distances[name] = alternative_distances[name];
                                accepted_tokens[name] = alternative_tokens[name];
                            }
                        }
                    }
                }
            }
            else if (effective_names.Count > 0)
            {
                accepted_names = effective_names;
                accepted_distances = effective_distances;
                accepted_tokens = effective_tokens;
                int missing = SettingsManager.Instance.GetNumberOfSuggestions() - accepted_names.Count;
                if (missing > 0)
                {
                    for (int i = 0; i < relative_distances.Count && i < missing; i++)
                    {
                        string name = relative_names[i];
                        accepted_names.Add(name);
                        accepted_distances.Add(name, relative_distances[name]);
                        accepted_tokens.Add(name, relative_tokens[name]);
                    }
                    missing = SettingsManager.Instance.GetNumberOfSuggestions() - accepted_names.Count;
                    if (missing > 0)
                    {
                        for (int i = 0; i < alternative_names.Count && i < missing; i++)
                        {
                            string name = alternative_names[i];
                            accepted_names.Add(name);
                            accepted_distances.Add(name, alternative_distances[name]);
                            accepted_tokens.Add(name, alternative_tokens[name]);
                        }
                    }
                }
            }
            else if (relative_names.Count > 0)
            {
                accepted_names = relative_names;
                accepted_distances = relative_distances;
                accepted_tokens = relative_tokens;
                int missing = SettingsManager.Instance.GetNumberOfSuggestions() - accepted_names.Count;
                if (missing > 0)
                {
                    for (int i = 0; i < alternative_names.Count && i < missing; i++)
                    {
                        string name = alternative_names[i];
                        accepted_names.Add(name);
                        accepted_distances.Add(name, alternative_distances[name]);
                        accepted_tokens.Add(name, alternative_tokens[name]);
                    }
                }
            }
            else
            {
                accepted_names = alternative_names;
                accepted_distances = alternative_distances;
                accepted_tokens = alternative_tokens;
            }
        }
        #endregion

        #region Private Methods
        private int CalcFittedWordDistance(string keyword, string token)
        {
            return LevenshteinMeasurer.Instance.GetDistance(token, keyword);
            //return LevenshteinMeasurer.Instance.GetDistance(token, keyword) + WordFLDistance(keyword,token);
        }

        private int CalcNonFittedWordDistance(string keyword, string token)
        {
            return LevenshteinMeasurer.Instance.GetDistance(token, keyword);
            //return LevenshteinMeasurer.Instance.GetDistance(token, keyword) + WordFLDistance(keyword, token);
        }

        #endregion
    }
}
