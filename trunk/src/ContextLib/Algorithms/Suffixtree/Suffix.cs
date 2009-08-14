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

// http://marknelson.us/1996/08/01/suffix-trees/
// http://code.google.com/p/csharsuffixtree/source/browse/#svn/trunk/suffixtree

namespace ContextLib.Algorithms.Suffixtree
{
    public class Suffix
    {
        public int originNode = 0;
        public int indexOfFirstCharacter;
        public int indexOfLastCharacter;
        //public string theString;
        public UserActionList theString;
        public Dictionary<int, Edge> edges;

        public Suffix(UserActionList theString, Dictionary<int, Edge> edges, int node, int start, int stop)
        {
            this.originNode = node;
            this.indexOfFirstCharacter = start;
            this.indexOfLastCharacter = stop;
            this.theString = theString;
            this.edges = edges;
        }

        public Suffix(Suffix suffix)
        {
            this.originNode = suffix.originNode;
            this.indexOfFirstCharacter = suffix.indexOfFirstCharacter;
            this.indexOfLastCharacter = suffix.indexOfLastCharacter;
            this.theString = suffix.theString;
            this.edges = suffix.edges;
        }

        public bool IsExplicit
        {
            get
            {
                return indexOfFirstCharacter > indexOfLastCharacter;
            }
        }

        public void Canonize()
        {
            if (!IsExplicit)
            {
                Edge edge = Edge.Find(theString, edges, originNode, theString[indexOfFirstCharacter]);
                int edgeSpan = edge.indexOfLastCharacter - edge.indexOfFirstCharacter;
                while (edgeSpan <= (this.indexOfLastCharacter - this.indexOfFirstCharacter))
                {
                    this.indexOfFirstCharacter = this.indexOfFirstCharacter + edgeSpan + 1;
                    this.originNode = edge.endNode;
                    if (this.indexOfFirstCharacter <= this.indexOfLastCharacter)
                    {
                        edge = Edge.Find(theString, edges, edge.endNode, theString[this.indexOfFirstCharacter]);
                        edgeSpan = edge.indexOfLastCharacter - edge.indexOfFirstCharacter;
                    }
                }
            }
        }
    }
}
