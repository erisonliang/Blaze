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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction.FileHandling;
using SystemCore.SystemAbstraction.ImageHandling;

namespace Blaze.SystemBrowsing
{
    public class SystemBrowser
    {
        #region Properties
        private Regex _regex;
        private Regex _web;
        private List<string> _index;
        #endregion

        #region Constructors
        public SystemBrowser()
        {
            _regex = new Regex(@"^.:");
            _web = new Regex(@"^(((h|H)(t|T)(t|T)(p|P)(s?|S?))\://)?(www.|[a-zA-Z0-9].)[a-zA-Z0-9\-\.]+\..*$");
            _index = new List<string>();
        }
        #endregion

        #region Public Methods
        public bool IsOwner(string input)
        {
            // Fix c:\\ and c:\. bugs
            Regex bugfix1 = new Regex(@"\\\\");
            Regex bugfix2 = new Regex(@"\\\.$");
            Regex bugfix3 = new Regex(@"\\\.\\");
            if (bugfix1.IsMatch(input) || bugfix2.IsMatch(input) || bugfix3.IsMatch(input) || _web.IsMatch(input))
                return false;
            else
                return _regex.IsMatch(input);
        }

        public List<string> RetrieveItems(string input)
        {
            _index = new List<string>();
            if (_regex.IsMatch(input))
            {
                // Get all directories
                string dir = input;
                // Ad a '\' do the drive letter if needed
                if (dir.Length == 2)
                {
                    dir += '\\';
                }
                // Solve incomplete paths
                while (!Directory.Exists(dir) || dir[dir.Length-1] != '\\')
                {
                    int i = dir.IndexOf('\\');
                    int f = dir.LastIndexOf('\\');
                    // If there is no '\' in the path or there is only one '\', which means an invalid drive letter, return empy index
                    if (f == -1 || (f == i && f == dir.Length-1))
                        return _index;
                    else if (f == dir.Length -1) // If it is and invalid path, trim the string (of '\') and validade it again
                        dir = dir.Substring(0, f);
                    else
                        dir = dir.Substring(0, f + 1);
                }
                // Add root to index
                _index.Add(dir);
                string[] directories;
                string[] files;
                try
                {
                    // Get all directories
                    directories = Directory.GetDirectories(FileNameManipulator.GetPath(dir));
                    // Get all files
                    files = FileSearcher.SearchFullNames(dir, false);
                }
                catch /*(Exception e)*/
                {
                    //MessageBox.Show(e.Message);
                    return _index;
                }
                //List<string> index_directories = new List<string>();
                //List<string> index_files = new List<string>();
                // For each directory
                for (int i = 0; i < directories.Length; i++)
                {
                    string directory = directories[i];
                    directory += "\\";
                    if (!_index.Contains(directory))
                    {
                        // Add directory path
                        _index.Add(directory);
                    }
                }
                // For each file
                for (int i = 0; i < files.Length; i++)
                {
                    string file = files[i];
                    if (!_index.Contains(file))
                    {
                        // Add file path
                        _index.Add(file);
                    }
                }
                // Sort directories and files
                //index_directories.Sort();
                //index_files.Sort();
                // Add them to the index
                //_index.AddRange(index_directories);
                //_index.AddRange(index_files);
                // Clean possible empty entries
                _index.RemoveAll(delegate(string s)
                {
                    return s == string.Empty;
                });
                
                directories = null;
                files = null;
            }
            return _index;
        }

        public Image GetItemIcon(string item)
        {
            if (_index.Contains(item))
                return IconManager.Instance.GetIcon(item);
            else
                return null;
        }

        public void Execute(InterpreterItem item)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(item.AutoComplete);
                info.UseShellExecute = true;
                info.ErrorDialog = true;
                System.Diagnostics.Process.Start(info);
                info = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
    }
}
