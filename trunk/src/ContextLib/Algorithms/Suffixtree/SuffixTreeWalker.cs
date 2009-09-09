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
using ContextLib.DataContainers.Monitoring;

namespace ContextLib.Algorithms.Suffixtree
{
    public class SuffixTreeWalker
    {
        #region Properties
        private SuffixTree _tree;
        private Dictionary<UserActionList, int> _deepested_repeated_prefixes;
        #endregion

        #region Accessors
        public Dictionary<UserActionList, int> DeepestRepeatedPrefixes { get { return _deepested_repeated_prefixes; } }
        #endregion

        #region Constructors
        public SuffixTreeWalker(SuffixTree tree)
        {
            _deepested_repeated_prefixes = new Dictionary<UserActionList, int>();
            _tree = tree;
        }
        #endregion

        #region Public Methods
        public void Walk()
        {
            _deepested_repeated_prefixes = new Dictionary<UserActionList, int>();
            //if (_tree.theString.Count == 9)
            //    System.Windows.Forms.MessageBox.Show("bbq");
            WalkTree(0, new UserActionList(), 0);
        }
        #endregion

        #region Private Methods
        private bool WalkTree(int start_node, UserActionList deepest_prefix, int depth)
        {
            List<UserAction> processed_actions = new List<UserAction>();
            int d = depth+1;
            bool is_leaf_node = true;
            for (int i = 0; i < _tree.theString.Count; i++)
            {
                UserAction action = _tree.theString[i];
                if (!processed_actions.Contains(action))
                {
                    Edge edge = Edge.Find(_tree.theString, _tree.Edges, start_node, action);
                    if (edge.startNode != -1)
                    {
                        is_leaf_node = false;
                        processed_actions.Add(action);
                        UserActionList acl = new UserActionList(_tree.theString.GetRange(edge.indexOfFirstCharacter, (edge.indexOfLastCharacter - edge.indexOfFirstCharacter)+1));
                        if (acl[acl.Count - 1].Id == -1)
                            acl.RemoveAt(acl.Count - 1);
                        bool deepest_non_leaf_leading_edge = WalkTree(edge.endNode, acl, d);
                        if (deepest_non_leaf_leading_edge && depth > 1)
                        {
                            if (!_deepested_repeated_prefixes.ContainsKey(deepest_prefix))
                                _deepested_repeated_prefixes.Add(deepest_prefix, depth);
                            else
                                if (depth > _deepested_repeated_prefixes[deepest_prefix])
                                    _deepested_repeated_prefixes[deepest_prefix] = depth;
                        }
                    }
                }
            }
            return is_leaf_node;
        }
        #endregion
    }
}
