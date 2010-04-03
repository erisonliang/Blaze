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
using System.Linq;

namespace Blaze.SystemBrowsing
{
    public class SystemBrowser
    {
        #region Properties
        private Regex _regex;
        private Regex _web;
        private List<string> best_paths;
        #endregion

        #region Constructors
        public SystemBrowser()
        {
            _regex = new Regex(@"^.:");
            _web = new Regex(@"^(((h|H)(t|T)(t|T)(p|P)(s?|S?))\://)?(www.|[a-zA-Z0-9].)[a-zA-Z0-9\-\.]+\..*$");
            best_paths = new List<string>();
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
            best_paths = new List<string>();
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
                        return best_paths;
                    else if (f == dir.Length -1) // If it is and invalid path, trim the string (of '\') and validade it again
                        dir = dir.Substring(0, f);
                    else
                        dir = dir.Substring(0, f + 1);
                }
                // Add root to index
                if (input.Length <= dir.Length)
                    best_paths.Add(dir);

                string[] available_directories;
                string[] available_files;
                HashSet<string> accepted_directories = new HashSet<string>();
                HashSet<string> accepted_files = new HashSet<string>();
                try
                {
                    // Get all directories
                    available_directories = Directory.GetDirectories(FileNameManipulator.GetPath(dir));
                    // Get all files
                    available_files = FileSearcher.SearchFullNames(dir, false, false, false);
                }
                catch /*(Exception e)*/
                {
                    //MessageBox.Show(e.Message);
                    return best_paths;
                }

                // For each directory
                for (int i = 0; i < available_directories.Length; i++)
                {
                    string directory = available_directories[i];
                    directory += "\\";
                    if (!string.IsNullOrEmpty(directory) &&
                        !accepted_directories.Contains(directory) &&
                        SystemCore.SystemAbstraction.StringUtilities.StringUtility.WordContainsStr(directory, input))
                    {
                        // Add directory path
                        accepted_directories.Add(directory);
                    }
                }
                // For each file
                for (int i = 0; i < available_files.Length; i++)
                {
                    string file = available_files[i];
                    if (!string.IsNullOrEmpty(file) &&
                        !accepted_files.Contains(file) &&
                        SystemCore.SystemAbstraction.StringUtilities.StringUtility.WordContainsStr(file, input))
                    {
                        // Add file path
                        accepted_files.Add(file);
                    }
                }
                List<string> acc_dir_lst = accepted_directories.ToList();
                List<string> acc_fil_lst = accepted_files.ToList();

                acc_dir_lst.Sort();
                acc_fil_lst.Sort();

                best_paths.AddRange(acc_dir_lst.Union(acc_fil_lst));
                
                available_directories = null;
                available_files = null;
                acc_dir_lst = null;
                acc_fil_lst = null;
            }
            return best_paths;
        }

        public Bitmap GetItemIcon(string item)
        {
            if (best_paths.Contains(item))
                return (Bitmap)IconManager.Instance.GetIcon(item);
            else
                return null;
        }

        public void Execute(InterpreterItem item, Keys modifiers)
        {
            try
            {
                string command = item.AutoComplete;
                if ((modifiers & Keys.Shift) == Keys.Shift)
                    command = FileSearcher.GetItemFolder(command);
                ProcessStartInfo info = new ProcessStartInfo(command);
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

        #region Private Methods
        #endregion
    }
}
