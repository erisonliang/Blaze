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
using System.Runtime.Serialization;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using SystemCore.Settings;

namespace SystemCore.CommonTypes
{
    [Serializable]
    public class Index : ISerializable
    {
        #region Properties
        private Dictionary<string, List<IndexItem>> _dictionary; // < keyword . < items > 
        private Dictionary<string, Bitmap> _icons; // < iconHash . icon >
        private List<IndexItem> _items; // < path . item >
        private bool _is_disposed = false;
        private bool _is_building = false;
        private MD5CryptoServiceProvider _md5; // hasher
        private SystemCore.CommonTypes.BKTree<string> _tree;

        // benchmark data
        DateTime _info_build_started;
        DateTime _info_build_finished;
        TimeSpan _info_build_duration;
        int _info_item_count = 0;
        int _info_keyword_count = 0;
        int _info_icon_count = 0;
        #endregion

        #region Accessors
        public Dictionary<string, List<IndexItem>> Dictionary
        {
            get { return _dictionary; }
        }
        public Dictionary<string, Bitmap> Icons
        {
            get { return _icons; }
        }
        public List<IndexItem> Items
        {
            get { return _items; }
        }
        public bool IsDisposed
        {
            get { return _is_disposed; }
        }
        public BKTree<string> Tree {
            get { return _tree; }
        }

        public DateTime InfoBuildStarted { get { return _info_build_started; } }
        public DateTime InfoBuildFinished { get { return _info_build_finished; } }
        public TimeSpan InfoBuildDuration { get { return _info_build_duration; } }
        public int InfoItemCount { get { return _info_item_count; } }
        public int InfoKeywordCount { get { return _info_keyword_count; } }
        public int InfoIconCount { get { return _info_icon_count; } }
        #endregion

        #region Constructors
        public Index()
        {
            _dictionary = new Dictionary<string, List<IndexItem>>();
            _icons = new Dictionary<string, Bitmap>();
            _items = new List<IndexItem>();
            _md5 = new MD5CryptoServiceProvider();
            _tree = new BKTree<string>(delegate(string a, string b)
                {
                        return (ushort)SystemCore.SystemAbstraction.StringUtilities.EditDistanceMeasurer.DamerauLevenshteinDistance(a, b);
                });
        }

        public Index(SerializationInfo info, StreamingContext context)
        {
            _dictionary = (Dictionary<string, List<IndexItem>>)info.GetValue("Dictionary", typeof(Dictionary<string, List<IndexItem>>));
            _icons = (Dictionary<string, Bitmap>)info.GetValue("Icons", typeof(Dictionary<string, Bitmap>));
            _items = (List<IndexItem>)info.GetValue("Items", typeof(List<IndexItem>));
            _md5 = new MD5CryptoServiceProvider();
            _tree = (BKTree<string>)info.GetValue("Tree", typeof(BKTree<string>));
            _info_build_started = (DateTime)info.GetValue("InfoBuildStarted", typeof(DateTime));
            _info_build_finished = (DateTime)info.GetValue("InfoBuildFinished", typeof(DateTime));
            _info_build_duration = (TimeSpan)info.GetValue("InfoBuildDuration", typeof(TimeSpan));
            _info_item_count = (int)info.GetValue("InfoItemCount", typeof(int));
            _info_keyword_count = (int)info.GetValue("InfoKeywordCount", typeof(int));
            _info_icon_count = (int)info.GetValue("InfoIconCount", typeof(int));
        }

        ~Index()
        {
            if (_is_building)
                FinishBuilding();
            //Dispose();
        }
        #endregion

        #region Public Methods
        public IndexItemSearchResult[] GetLearnedMatches(LearnedItem[] learned_commands, ushort n_results, ref Dictionary<IndexItem, List<string>> tokens)
        {
            List<IndexItemSearchResult> ret = new List<IndexItemSearchResult>();

            foreach (LearnedItem cmd in learned_commands)
            {
                foreach (IndexItem item in _items)
                {
                    //if (item.Name == "Notepad")
                    //    System.Windows.Forms.MessageBox.Show("box");
                    if (item.IsCommand &&
                        (cmd.Type == InterpreterItem.OwnerType.Menu ||
                         cmd.Type == InterpreterItem.OwnerType.Plugin))
                    {
                        if (item.Name == cmd.Distinguisher)
                        {
                            ret.Add(new IndexItemSearchResult(item, 0, true));
                            tokens.Add(item, new List<string>(cmd.Tokens));
                            break;
                        }
                    }
                    else if (!item.IsCommand &&
                                (cmd.Type == InterpreterItem.OwnerType.Indexer))
                    {
                        if (item.Path == cmd.Distinguisher)
                        {
                            ret.Add(new IndexItemSearchResult(item, 0, true));
                            tokens.Add(item, new List<string>(cmd.Tokens));
                            break;
                        }
                    }
                }
                if (ret.Count > 0)
                    break;
            }
            return ret.ToArray();
        }

        public IndexItemSearchResult[] GetBestMatches(string key, ushort n_results, ushort max_error)
        {
            List<string> keys = new List<string>();
            Dictionary<string, ushort> errors = new Dictionary<string, ushort>();

            errors = _tree.Query(key, max_error);
            foreach (KeyValuePair<string, ushort> pair in errors)
                keys.Add(pair.Key);

            ushort error;
            foreach (string token in _dictionary.Keys)
            {
                if (token.Length >= key.Length &&
                    SystemCore.SystemAbstraction.StringUtilities.StringUtility.WordContainsStr(token, key))
                {
                    error = (ushort)Math.Round(((2-((token.Length+key.Length)/(double)token.Length))*2));
                    if (!errors.ContainsKey(token))
                    {
                        keys.Add(token);
                        errors.Add(token, error);
                    }
                    else if (errors[token] > error)
                        errors[token] = error;
                }
            }

            keys.Sort(delegate(string x, string y)
            {
                return errors[x].CompareTo(errors[y]);
            });

            List<IndexItemSearchResult> ret = GenerateSearchResult(keys, errors, n_results);
            return ret.ToArray();
        }

        public void AddEntry(string key, string name, int n_tokens, string path, Bitmap icon, bool is_command)
        {
            string hashed_icon = HashBmp(icon);
            IndexItem new_item = new IndexItem(this, name, path, hashed_icon, (short)n_tokens, is_command);
            if (!_items.Contains(new_item))
            {
                _items.Add(new_item);
                _info_item_count++;
                // add a new icon if it doesn't already exist
                if (!_icons.ContainsKey(hashed_icon))
                {
                    _icons.Add(hashed_icon, icon);
                    _info_icon_count++;
                }
            }
            // add a new keyword if needed
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, new List<IndexItem>());
                _info_keyword_count++;
                _tree.Add(key);
            }
            // bind the items to the keyword
            if (!_dictionary[key].Contains(new_item))
                _dictionary[key].Add(new_item);
        }

        public void AddItemByName(string name, string path, Bitmap icon, bool is_command)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string[] keywords = SystemCore.SystemAbstraction.StringUtilities.StringUtility.GenerateKeywords(name);
                foreach (string keyword in keywords)
                    AddEntry(keyword, name, keywords.Length, path, icon, is_command);
            }
        }

        public void AddItemByName(string name, string path, bool is_command)
        {
            AddItemByName(name, path, null, is_command);
        }

        public void AddItemByName(string name, bool is_command)
        {
            AddItemByName(name, string.Empty, null, is_command);
        }

        public Bitmap GetItemIcon(IndexItem item)
        {
            try
            {
                return _icons[item.IconId];
            }
            catch
            {
                return null;
            }
        }

        public bool ContainsItem(IndexItem item)
        {
            return _items.Contains(item);
        }

        public bool ContainsItem(string name, string path, bool is_command)
        {
            List<IndexItem> items = (from item in _items where (item.Name == name && item.Path == path && item.IsCommand == is_command) select item).ToList();
            return items.Count > 0;
        }

        public void Merge(Index index)
        {
            foreach (KeyValuePair<string, List<IndexItem>> pair in index.Dictionary)
            {
                string key = pair.Key;
                List<IndexItem> items = pair.Value;
                if (_dictionary.ContainsKey(key))
                {
                    _dictionary[key].AddRange(items);
                }
                else
                {
                    _dictionary.Add(key, items);
                }

                _tree.Add(key);
            }

            foreach (KeyValuePair<string, Bitmap> de in index.Icons)
            {
                if (!_icons.ContainsKey(de.Key))
                {
                    _icons.Add(de.Key, de.Value);
                }
            }

            foreach (IndexItem item in index.Items)
            {
                _items.Add(new IndexItem(item));
            }
        }

        public void Clear()
        {
            _dictionary.Clear();
            _icons.Clear();
            _items.Clear();
            _tree = new BKTree<string>(delegate(string a, string b)
                {
                        return (ushort)SystemCore.SystemAbstraction.StringUtilities.EditDistanceMeasurer.DamerauLevenshteinDistance(a, b);
                });
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                foreach (KeyValuePair<string,Bitmap> icon in _icons)
                {
                    try
                    {
                        icon.Value.Dispose();
                    }
                    catch
                    {

                    }
                }
                _dictionary = null;
                _icons = null;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Dictionary", Dictionary);
            info.AddValue("Icons", Icons);
            info.AddValue("Items", Items);
            info.AddValue("Tree", Tree);
            info.AddValue("InfoBuildStarted", InfoBuildStarted);
            info.AddValue("InfoBuildFinished", InfoBuildFinished);
            info.AddValue("InfoBuildDuration", InfoBuildDuration);
            info.AddValue("InfoIconCount", InfoIconCount);
            info.AddValue("InfoItemCount", InfoItemCount);
            info.AddValue("InfoKeywordCount", InfoKeywordCount);
        }

        public void StartBuilding()
        {
            if (!_is_building)
            {
                _is_building = true;
                _info_build_started = DateTime.Now;
            }
        }

        public void FinishBuilding()
        {
            if (_is_building)
            {
                _info_build_finished = DateTime.Now;
                _info_build_duration = _info_build_finished - _info_build_started;
                _is_building = false;
            }
        }
        #endregion

        #region Private Methods
        private List<IndexItemSearchResult> GenerateSearchResult(List<string> keys, Dictionary<string, ushort> errors, int max_items)
        {
            int count = 0;
            HashSet<IndexItemSearchResult> ret = new HashSet<IndexItemSearchResult>();
            foreach (string k in keys)
            {
                foreach (IndexItem item in _dictionary[k])
                {
                    IndexItemSearchResult result = new IndexItemSearchResult(item, (short)errors[k]);
                    if (count < max_items)
                    {
                        if (ret.Add(result))
                            count++;
                    }
                    else
                        return ret.ToList();
                }
            }
            return ret.ToList();
        }

        private string HashBmp(Bitmap bmp)
        {
            if (bmp == null)
                return string.Empty;
            byte[] bytes = BmpToBytes(bmp);
            byte[] hash = _md5.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private byte[] BmpToBytes(Bitmap bmp)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Could not hash icon.");
            }

            // read to end
            byte[] bmpBytes = ms.GetBuffer();
            //bmp.Dispose();
            ms.Close();

            return bmpBytes;
        }

        private Image BytesToImg(byte[] bmpBytes)
        {
            MemoryStream ms = new MemoryStream(bmpBytes);
            Image img = Image.FromStream(ms);
            // Do NOT close the stream!

            return img;
        }
        #endregion
    }
}
