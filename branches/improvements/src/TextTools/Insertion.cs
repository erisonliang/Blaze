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
using System.IO;
using ContextLib;

namespace TextTools
{
    public class Insertion
    {
        #region Properties
        private string _text;
        private string _target;
        #endregion

        #region Accessors
        public string Text { get { return _text; } set { _text = value; } }
        public string Target { get { return _target; } set { _target = value; } }
        #endregion

        #region Constructors
        public Insertion()
        {
            _text = string.Empty;
            _target = string.Empty;
        }

        public Insertion(string text)
        {
            _text = text;
            _target = string.Empty;
        }

        public Insertion(string text, string target)
        {
            _text = text;
            _target = target;
            if (_target != string.Empty)
            {
                string dir = string.Empty;
                string file = string.Empty;
                try
                {
                    if (Directory.Exists(_target))
                        dir = _target;
                    else
                        dir = Path.GetDirectoryName(_target);
                }
                catch
                {

                }
                try
                {
                    file = Path.GetFileName(_target);
                }
                catch
                {

                }
                if (file == string.Empty)
                {
                    file = "blaze_inserted_text.txt";
                }
                if (Directory.Exists(dir))
                {
                    _target = dir + (dir[dir.Length - 1] == '\\' ? "" : @"\") + file;
                }
                else
                {
                    _target = UserContext.Instance.GetExplorerPath(false) + "\\" + file;
                }
            }
        }

        public Insertion(Insertion insertion)
        {
            _text = insertion.Text;
            _target = insertion.Target;
        }
        #endregion

        #region Public Methods
        #endregion
    }
}
