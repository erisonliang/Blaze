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

namespace SystemCore.CommonTypes
{
    [Serializable]
    public class Index : ISerializable
    {
        #region Properties
        private List<string> _names;
        private Dictionary<string, List<string>> _paths;
        private Dictionary<string, List<string>> _keywords;
        private Dictionary<string, List<System.Drawing.Image>> _icons;
        private bool _is_disposed = false;
        #endregion

        #region Accessors
        public List<string> Names
        {
            get { return _names; }
            set { _names = value; }
        }
        public Dictionary<string, List<string>> Paths
        {
            get { return _paths; }
            set { _paths = value; }
        }
        public Dictionary<string, List<string>> Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }
        public Dictionary<string, List<System.Drawing.Image>> Icons
        {
            get { return _icons; }
            set { _icons = value; }
        }
        public bool IsDisposed
        {
            get { return _is_disposed; }
        }
        #endregion

        #region Constructors
        public Index()
        {
            _names = new List<string>();
            _paths = new Dictionary<string, List<string>>();
            _keywords = new Dictionary<string, List<string>>();
            _icons = new Dictionary<string, List<System.Drawing.Image>>();
        }

        public Index(Index index)
        {
            _names = new List<string>(index.Names);
            _paths = new Dictionary<string, List<string>>(index.Paths);
            _keywords = new Dictionary<string, List<string>>(index.Keywords);
            _icons = index.Icons;
            //_icons = new Dictionary<string, List<System.Drawing.Icon>>();
            //foreach (KeyValuePair<string, List<System.Drawing.Icon>> pair in index.Icons)
            //{
            //    _icons.Add(pair.Key, new List<System.Drawing.Icon>());
            //    foreach (System.Drawing.Icon icon in pair.Value)
            //        _icons[pair.Key].Add((System.Drawing.Icon)icon.Clone());
            //}
        }

        public Index(SerializationInfo info, StreamingContext context)
        {
            _names = (List<string>)info.GetValue("Names", typeof(List<string>));
            _paths = (Dictionary<string, List<string>>)info.GetValue("Paths", typeof(Dictionary<string, List<string>>));
            _keywords = (Dictionary<string, List<string>>)info.GetValue("Keywords", typeof(Dictionary<string, List<string>>));
            _icons = (Dictionary<string, List<System.Drawing.Image>>)info.GetValue("Icons", typeof(Dictionary<string, List<System.Drawing.Image>>));
        }

        ~Index()
        {
            //Dispose();
        }
        #endregion

        #region Public Methods
        public void Dispose()
        {
            if (!IsDisposed)
            {
                foreach (string name in _names)
                {
                    foreach (System.Drawing.Image icon in _icons[name])
                    {
                        try
                        {
                            icon.Dispose();
                        }
                        catch
                        {

                        }
                    }
                }
                _names = null;
                _paths = null;
                _keywords = null;
                _icons = null;
                _is_disposed = true;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Names", Names);
            info.AddValue("Paths", Paths);
            info.AddValue("Keywords", Keywords);
            info.AddValue("Icons", Icons);
        }
        #endregion
    }
}
