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
using ContextLib.DataContainers.GUI;
using System.IO;

namespace ContextLib.DataContainers.Monitoring
{
    public abstract class FileAction : UserAction
    {
        #region Properties
        protected string _folder;
        //protected string[] _files;
        protected const int MAX_MOVE_DELAY = 1000;
        protected const int MAX_CREATE_DELAY = 30000;
        #endregion

        #region Accessors
        public string Folder { get { return _folder; } set { _folder = value; } }
        //public string[] Files { get { return _files; } set { _files = value; } }
        #endregion

        #region Constructors
        public FileAction(string folder)
            : base()
        {
            _folder = folder;
            //_files = Directory.GetFiles(_folder, "*.*", SearchOption.TopDirectoryOnly);
        }

        public FileAction(Window window, string folder)
            : base(window)
        {
            _folder = folder;
            //_files = Directory.GetFiles(_folder, "*.*", SearchOption.TopDirectoryOnly);
        }
        #endregion
    }
}
