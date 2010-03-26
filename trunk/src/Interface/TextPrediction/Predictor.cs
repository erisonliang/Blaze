// Blaze: Automated Desktop Experience
// Copyright (C) 2009,2010  Gabriel Barata
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
using System.Text;
using SystemCore.CommonTypes;
using SystemCore.Settings;

namespace Blaze.TextPrediction
{
    public class Predictor
    {
        private static readonly Predictor _instance = new Predictor();
        private PredictorCache _cache;

        public static Predictor Instance { get { return _instance; } }

        private Predictor() { _cache = new PredictorCache(16); }

        public IndexItem[] GetBestItems(Index index, string user_text, List<string> user_tokens, ref Dictionary<IndexItem, List<string>> item_tokens, LearnedContent learned_commands)
        {
            Dictionary<string, IndexItemSearchResult[]> matches = new Dictionary<string, IndexItemSearchResult[]>();
            Dictionary<IndexItem, List<string>> tokens = new Dictionary<IndexItem, List<string>>();
            Dictionary<IndexItem, List<short>> tokens_values = new Dictionary<IndexItem, List<short>>();
            ushort n_results = (ushort)(SystemCore.Settings.SettingsManager.Instance.GetNumberOfSuggestions() * 10);
            
            LearnedItem[] cmds = learned_commands.GetSortedItems(user_text);
            Dictionary<IndexItem, List<string>> learned_tokens = new Dictionary<IndexItem, List<string>>();
            List<IndexItemSearchResult> learned_items = new List<IndexItemSearchResult>(index.GetLearnedMatches(cmds, n_results, ref learned_tokens));
            foreach (IndexItemSearchResult item in learned_items)
            {
                tokens.Add(item.Result, FixLearnedTokens(item.Result, user_tokens, learned_tokens[item.Result]));
                tokens_values.Add(item.Result, new List<short> (new short[] { item.Error }));
            }

            foreach (string token in user_tokens)
            {
                IndexItemSearchResult[] results;
                if (_cache.Contains(token))
                    results = _cache.Get(token);
                else
                    results = index.GetBestMatches(token, n_results, (ushort)5);
                foreach (IndexItemSearchResult result in results)
                {
                    if (!learned_items.Contains(result))
                    {
                        if (!tokens.ContainsKey(result.Result))
                        {
                            tokens.Add(result.Result, new List<string>(new string[] { token }));
                            tokens_values.Add(result.Result, new List<short>(new short[] { result.Error }));
                        }
                        else
                        {
                            if (result.Result.IsCommand &&
                                tokens[result.Result].Count == result.Result.NTokens)
                            {
                                short lsi = LeastSimilarIndex(tokens_values[result.Result], result.Error);
                                if (lsi != -1)
                                {
                                    tokens[result.Result][lsi] = token;
                                    tokens_values[result.Result][lsi] = result.Error;
                                }
                            }
                            else
                            {
                                tokens[result.Result].Add(token);
                                tokens_values[result.Result].Add(result.Error);
                            }
                        }
                    }
                }
                _cache.Add(token, results);
                matches.Add(token,
                            results            
                            );
            }
            Dictionary<IndexItemSearchResult, IndexItemSearchResult> command_items_dic = new Dictionary<IndexItemSearchResult,IndexItemSearchResult>();
            List<IndexItemSearchResult> non_command_items = null;
            foreach (string token in user_tokens)
            {
                List<IndexItemSearchResult> aux = new List<IndexItemSearchResult>();
                foreach (IndexItemSearchResult result in matches[token])
                {
                    if (result.Result.IsCommand)
                    {
                        if (!command_items_dic.ContainsKey(result))
                        {
                            command_items_dic.Add(result, result);
                        }
                        else
                        {
                            if (command_items_dic[result].Error > result.Error)
                            {
                                command_items_dic[result] = result;
                            }
                        }
                    }
                    else
                    {
                        aux.Add(result);
                    }
                }
                if (non_command_items != null)
                {
                    aux = aux.Intersect(non_command_items).ToList();
                }
                non_command_items = aux;
            }

            if (non_command_items == null)
                non_command_items = new List<IndexItemSearchResult>();
            List<IndexItemSearchResult> command_items = command_items_dic.Values.ToList();

            List<IndexItemSearchResult> items = Enumerable.Union(non_command_items, command_items).ToList();
            foreach (IndexItemSearchResult item in learned_items)
                items.Remove(item);
            learned_items.AddRange(items);
            items = learned_items;
            if (items != null)
            {
                item_tokens = tokens;
                items.Sort();
                return new List<IndexItem>(from item in items select item.Result).Take(SystemCore.Settings.SettingsManager.Instance.GetNumberOfSuggestions()).ToArray();
            }
            else
                return null;
        }

        public void InvalidateCache()
        {
            _cache.Clear();
        }

        private short LeastSimilarIndex(List<short> vals, short lim)
        {
            short max = -1;
            for (short i = 0; i < vals.Count; i++)
            {
                if (vals[i] > lim && vals[i] > max)
                    max = i;
            }
            return max;
        }

        private List<string> FixLearnedTokens(IndexItem item, List<string> user_tokens, List<string> learned_tokens)
        {
            if (!item.IsCommand)
                return new List<string>(user_tokens);
            else
            {
                List<string> ret = new List<string>();
                foreach (string ltoken in learned_tokens)
                {
                    int smallest_length = -1;
                    string token_to_add = string.Empty;
                    foreach (string utoken in user_tokens)
                    {
                        if (SystemCore.SystemAbstraction.StringUtilities.StringUtility.WordContainsStr(ltoken, utoken))
                        {
                            if (smallest_length == -1)
                            {
                                smallest_length = utoken.Length;
                                token_to_add = utoken;
                            }
                            else if (utoken.Length < smallest_length)
                            {
                                smallest_length = utoken.Length;
                                token_to_add = utoken;
                            }
                        }
                    }
                    if (smallest_length > -1)
                        ret.Add(token_to_add);
                }
                return ret;
            }
        }
    }
}
