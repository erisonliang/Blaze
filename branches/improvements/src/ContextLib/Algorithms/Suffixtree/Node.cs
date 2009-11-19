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
// http://marknelson.us/1996/08/01/suffix-trees/
// http://code.google.com/p/csharsuffixtree/source/browse/#svn/trunk/suffixtree

namespace ContextLib.Algorithms.Suffixtree
{
    public class Node
    {
        public int suffixNode;

        public Node()
        {
            suffixNode = -1;
        }
        public Node(Node node)
        {
            this.suffixNode = node.suffixNode;
        }
        public static int Count = 1;
    }
}
