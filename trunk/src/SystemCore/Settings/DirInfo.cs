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
using System.Collections.Generic;

namespace SystemCore.Settings
{
    public class DirInfo
    {
        #region Properties
        private List<string> _directories;
        private Dictionary<string, List<string>> _extensions;
        private Dictionary<string, bool> _indexSubdirectories;
        private Dictionary<string, List<string>> _plugins;
        #endregion

        #region Accessors
        public List<string> Directories
        {
            get { return _directories; }
            set { _directories = value; }
        }

        public Dictionary<string, List<string>> Extensions
        {
            get { return _extensions; }
            set { _extensions = value; }
        }

        public Dictionary<string, bool> IndexSubdirectories
        {
            get { return _indexSubdirectories; }
            set { _indexSubdirectories = value; }
        }

        public Dictionary<string, List<string>> Plugins
        {
            get { return _plugins; }
            set { _plugins = value; }
        }
        #endregion

        #region Constructors
        public DirInfo(List<string> directories, Dictionary<string, List<string>> extensions, Dictionary<string, bool> indexSubdirectories, Dictionary<string, List<string>> plugins)
        {
            _directories = directories;
            _extensions = extensions;
            _indexSubdirectories = indexSubdirectories;
            _plugins = plugins;
        }

        public DirInfo(DirInfo info)
        {
            _directories = new List<string>(info.Directories);
            _extensions = new Dictionary<string, List<string>>(info.Extensions);
            _indexSubdirectories = new Dictionary<string, bool>(info.IndexSubdirectories);
            _plugins = new Dictionary<string, List<string>>(info.Plugins);
        }

        public DirInfo()
        {
            _directories = new List<string>();
            _extensions = new Dictionary<string, List<string>>();
            _indexSubdirectories = new Dictionary<string, bool>();
            _plugins = new Dictionary<string, List<string>>();
        }
        #endregion

        #region Public Methods
        #endregion
    }
}
