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
    public class Edge
    {
        public int indexOfFirstCharacter;
        public int indexOfLastCharacter;
        public int startNode;
        public int endNode;
        //string theString;
        UserActionList theString;

        public const int HASH_TABLE_SIZE = 306785407;

        public Edge(UserActionList theString)
        {
            this.theString = theString;
            this.startNode = -1;
        }

        public Edge(UserActionList theString, int indexOfFirstCharacter, int indexOfLastCharacter, int parentNode)
        {
            this.theString = theString;
            this.indexOfFirstCharacter = indexOfFirstCharacter;
            this.indexOfLastCharacter = indexOfLastCharacter;
            this.startNode = parentNode;
            this.endNode = Node.Count++;
        }

        public Edge(Edge edge)
        {
            this.startNode = edge.startNode;
            this.endNode = edge.endNode;
            this.indexOfFirstCharacter = edge.indexOfFirstCharacter;
            this.indexOfLastCharacter = edge.indexOfLastCharacter;
            this.theString = edge.theString;
        }

        static public void Insert(UserActionList theString, Dictionary<int, Edge> edges, Edge edge)
        {
            int i = Hash(edge.startNode, theString[edge.indexOfFirstCharacter]);
            if (!edges.ContainsKey(i))
            {
                edges.Add(i, new Edge(theString));
            }
            while (edges[i].startNode != -1)
            {
                i = ++i % HASH_TABLE_SIZE;
                if (!edges.ContainsKey(i))
                {
                    edges.Add(i, new Edge(theString));
                }

            }
            edges[i] = new Edge(edge);
        }

        static public void Remove(UserActionList theString, Dictionary<int, Edge> edges, Edge edge)
        {
            edge = new Edge(edge);
            int i = Hash(edge.startNode, theString[edge.indexOfFirstCharacter]);
            while (edges[i].startNode != edge.startNode || edges[i].indexOfFirstCharacter != edge.indexOfFirstCharacter)
            {
                i = ++i % HASH_TABLE_SIZE;
            }

            for (; ; )
            {
                edges[i].startNode = -1;
                int j = i;
                for (; ; )
                {
                    i = ++i % HASH_TABLE_SIZE;
                    if (!edges.ContainsKey(i))
                    {
                        edges.Add(i, new Edge(theString));
                    }
                    if (edges[i].startNode == -1)
                    {
                        return;
                    }

                    int r = Hash(edges[i].startNode, theString[edges[i].indexOfFirstCharacter]);
                    if (i >= r && r > j)
                    {
                        continue;
                    }
                    if (r > j && j > i)
                    {
                        continue;
                    }
                    if (j > i && i >= r)
                    {
                        continue;
                    }
                    break;
                }
                edges[j] = new Edge(edges[i]);
            }
        }

        static public int SplitEdge(Suffix s, UserActionList theString, Dictionary<int, Edge> edges, Dictionary<int, Node> nodes, Edge edge)
        {
            Remove(theString, edges, edge);
            Edge newEdge = new Edge(theString, edge.indexOfFirstCharacter,
                edge.indexOfFirstCharacter + s.indexOfLastCharacter
                - s.indexOfFirstCharacter, s.originNode);
            Edge.Insert(theString, edges, newEdge);
            //nodes[newEdge.endNode].suffixNode = s.originNode;
            //newEdge.Insert();
            if (nodes.ContainsKey(newEdge.endNode))
            {
                nodes[newEdge.endNode].suffixNode = s.originNode;
            }
            else
            {
                Node newNode = new Node();
                newNode.suffixNode = s.originNode;
                nodes.Add(newEdge.endNode, newNode);
            }

            edge.indexOfFirstCharacter += s.indexOfLastCharacter - s.indexOfFirstCharacter + 1;
            edge.startNode = newEdge.endNode;
            Edge.Insert(theString, edges, edge);
            //Insert();
            return newEdge.endNode;

        }

        static public Edge Find(UserActionList theString, Dictionary<int, Edge> edges, int node, UserAction c)
        {
            int i = Hash(node, c);
            for (; ; )
            {
                if (!edges.ContainsKey(i))
                {
                    edges.Add(i, new Edge(theString));
                }
                if (edges[i].startNode == node)
                {
                    //if (c == theString[edges[i].indexOfFirstCharacter])
                    //{
                    //    return edges[i];
                    //}
                    if (c.Equals(theString[edges[i].indexOfFirstCharacter]))
                    {
                        return edges[i];
                    }
                }
                if (edges[i].startNode == -1)
                {
                    return edges[i];
                }
                i = ++i % HASH_TABLE_SIZE;
            }
            //return null;
        }

        public static int Hash(int node, UserAction c)
        {
            int rtnValue = ((node << 16) + c.Id) % (int)HASH_TABLE_SIZE;
            if (rtnValue == 1585)
            {
                rtnValue = 1585;
            }
            return rtnValue;
        }
    }
}
