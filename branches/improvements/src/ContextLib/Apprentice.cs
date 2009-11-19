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
using ContextLib.Algorithms.Suffixtree;
using ContextLib.DataContainers.Monitoring;

namespace ContextLib
{
    public class Apprentice
    {
        #region Properties
        //private SystemCore.SystemAbstraction.FileHandling.Logger _logger;
        UserActionList[] _longest_repetitions = null;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public Apprentice()
        {
            //_logger = new SystemCore.SystemAbstraction.FileHandling.Logger("apprendice.txt");
        }
        #endregion

        #region Public Methods
        public void Rebuild(List<UserAction> actions)
        {
            //if (actions.Count == 4)
            //    System.Windows.Forms.MessageBox.Show("bbq");
            //Mutex mutex = new Mutex(false, CommonInfo.GUID + "-user-actions-lock");
            //mutex.WaitOne();
            SuffixTree suffix_tree = new SuffixTree(actions);
            //mutex.ReleaseMutex();

            suffix_tree.BuildTree();
            _longest_repetitions = suffix_tree.GetLongestRepeatedSubstrings(1);
            //UserActionList list = suffix_tree.GetLongestRepeatedSubstring(1, 3);
            //if (list != null)
            //{
            //    //_logger.WriteLine("Repetition detected at " + DateTime.Now.ToString());
            //    foreach (UserAction action in list)
            //        _logger.WriteLine(action.Description);
            //}
            //_logger.WriteLine("Tree build on " + DateTime.Now);
            //try
            //{
            //    foreach (string str in _suffix_tree.DumpEdges())
            //    {
            //        _logger.WriteLine(str);
            //    }
            //}
            //catch (Exception e)
            //{
            //    //System.Windows.Forms.MessageBox.Show(e.Message);
            //}
        }

        public UserActionList[] GetLongestRepetitions()
        {
            return _longest_repetitions;
        }
        #endregion
    }
}
