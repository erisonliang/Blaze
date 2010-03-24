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

namespace Blaze.TextPrediction
{
    public class Predictor
    {
        private static readonly Predictor _instance = new Predictor();
        private PredictorCache _cache;

        public static Predictor Instance { get { return _instance; } }

        private Predictor() { _cache = new PredictorCache(15); }

        public IndexItem[] GetBestItems(Index index, List<string> user_tokens, ref Dictionary<IndexItem, List<string>> item_tokens)
        {
            Dictionary<string, IndexItemSearchResult[]> matches = new Dictionary<string, IndexItemSearchResult[]>();
            Dictionary<IndexItem, List<string>> tokens = new Dictionary<IndexItem, List<string>>();
            Dictionary<IndexItem, List<short>> tokens_values = new Dictionary<IndexItem, List<short>>();
            foreach (string token in user_tokens)
            {
                IndexItemSearchResult[] results;
                if (_cache.Contains(token))
                    results = _cache.Get(token);
                else
                    results = index.GetBestMatches(token, (ushort)(SystemCore.Settings.SettingsManager.Instance.GetNumberOfSuggestions() * 2), (ushort)5);
                foreach (IndexItemSearchResult result in results)
                {
                    if (!tokens.ContainsKey(result.Result))
                    {
                        tokens.Add(result.Result, new List<string>(new string[] { token }));
                        tokens_values.Add(result.Result, new List<short>( new short[] { result.Error }));
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(result.Result.Path) &&
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
                    if (string.IsNullOrEmpty(result.Result.Path))
                    {
                        //if (result.Result.Name == "redo")
                        //    System.Windows.Forms.MessageBox.Show("test");
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
            if (items != null)
            {
                item_tokens = tokens;
                items.Sort();
                //if (user_tokens.Contains("faiafo"))
                //    System.Windows.Forms.MessageBox.Show("woot");
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
    }
}
