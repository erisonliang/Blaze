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
using System.Text;
using ContextLib.DataContainers.Monitoring;

// http://marknelson.us/1996/08/01/suffix-trees/
// http://code.google.com/p/csharsuffixtree/source/browse/#svn/trunk/suffixtree

namespace ContextLib.Algorithms.Suffixtree
{
    public class SuffixTree
    {
        Edge edges;
        //public string theString;
        public UserActionList theString = null;
        public Dictionary<int, Edge> Edges = null;
        public Dictionary<int, Node> Nodes = null;
        private Dictionary<UserActionList, int> _deepested_repeated_prefixes;

        public SuffixTree(List<UserAction> theString)
        {
            this.theString = new UserActionList(theString, true);
            this.theString.Add(new TerminalAction());
            Nodes = new Dictionary<int, Node>();
            Edges = new Dictionary<int, Edge>();
            edges = new Edge(this.theString);
        }

        public void BuildTree()
        {
            Suffix active = new Suffix(this.theString, Edges, 0, 0, -1);
            for (int i = 0; i <= theString.Count - 1; i++)
            {
                AddPrefix(active, i);
            }
            SuffixTreeWalker walker = new SuffixTreeWalker(this);
            walker.Walk();
            _deepested_repeated_prefixes = walker.DeepestRepeatedPrefixes;
        }

        //public static void Save(BinaryWriter writer, SuffixTree tree)
        //{
        //    writer.Write(tree.Edges.Count);
        //    writer.Write(tree.theString.Length);
        //    writer.Write(tree.theString);
        //    foreach (KeyValuePair<int, Edge> edgePair in tree.Edges)
        //    {
        //        writer.Write(edgePair.Key);
        //        writer.Write(edgePair.Value.endNode);
        //        writer.Write(edgePair.Value.startNode);
        //        writer.Write(edgePair.Value.indexOfFirstCharacter);
        //        writer.Write(edgePair.Value.indexOfLastCharacter);
        //    }

        //}


        //public static void Save(Stream stream, SuffixTree tree)
        //{
        //    using (BinaryWriter writer = new BinaryWriter(stream))
        //    {
        //        Save(writer, tree);
        //    }
        //}

        //public static SuffixTree LoadFromFile(BinaryReader reader)
        //{
        //    SuffixTree tree;
        //    int count = reader.ReadInt32();
        //    int theStringLength = reader.ReadInt32();
        //    string theString = reader.ReadString();
        //    tree = new SuffixTree(theString);
        //    for (int i = 0; i < count; i++)
        //    {
        //        int key = reader.ReadInt32();
        //        Edge readEdge = new Edge(theString);
        //        readEdge.endNode = reader.ReadInt32();
        //        readEdge.startNode = reader.ReadInt32();
        //        readEdge.indexOfFirstCharacter = reader.ReadInt32();
        //        readEdge.indexOfLastCharacter = reader.ReadInt32();
        //        tree.Edges.Add(key, readEdge);
        //    }
        //    return tree;
        //}

        //public static SuffixTree LoadFromFile(Stream stream)
        //{
        //    SuffixTree tree;
        //    using (BinaryReader reader = new BinaryReader(stream))
        //    {
        //        tree = LoadFromFile(reader);
        //    }
        //    return tree;
        //}


        //public bool Search(string search)
        //{
        //    search = search.ToLower();
        //    //try
        //    //{
        //    if (search.Length == 0)
        //    {
        //        return false;
        //    }
        //    int index = 0;
        //    Edge edge;
        //    if (!this.Edges.TryGetValue((int)Edge.Hash(0, search[0]), out edge))
        //    {
        //        return false;
        //    }

        //    if (edge.startNode == -1)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        for (; ; )
        //        {
        //            for (int j = edge.indexOfFirstCharacter; j <= edge.indexOfLastCharacter; j++)
        //            {
        //                if (index >= search.Length)
        //                {
        //                    return true;
        //                }
        //                char test = theString[j];
        //                if (this.theString[j] != search[index++])
        //                {
        //                    return false;
        //                }
        //            }
        //            if (index < search.Length)
        //            {
        //                Edge value;
        //                if (this.Edges.TryGetValue(Edge.Hash(edge.endNode, search[index]), out value))
        //                {
        //                    edge = new Edge(value);
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    //}
        //    //catch (KeyNotFoundException)
        //    //{
        //    //    return false;
        //    //}
        //}

        public string[] DumpEdges()
        {
            List<string> edges = new List<string>();
            int count = this.theString.Count;
            for (int j = 0; j < Edge.HASH_TABLE_SIZE; j++)
            {
                if (this.Edges.ContainsKey(j))
                {
                    Edge edge = this.Edges[j];
                    if (edge.startNode == -1)
                    {
                        continue;
                    }
                    int top = 0;
                    if (count > edge.indexOfLastCharacter)
                    {
                        top = edge.indexOfLastCharacter;
                    }
                    else
                    {
                        top = count;
                    }
                    StringBuilder builder = new StringBuilder();
                    for (int i = edge.indexOfFirstCharacter; i <= top; i++)
                    {
                        builder.Append(this.theString[i]);
                    }
                    edges.Add(builder.ToString());
                }
            }
            return edges.ToArray();
        }


        private void AddPrefix(Suffix active, int indexOfLastCharacter)
        {
            int parentNode;
            int lastParentNode = -1;

            for (; ; )
            {
                Edge edge = new Edge(theString);
                parentNode = active.originNode;


                if (active.IsExplicit)
                {
                    edge = new Edge(Edge.Find(this.theString, this.Edges, active.originNode, theString[indexOfLastCharacter]));
                    if (edge.startNode != -1)
                    {
                        break;
                    }
                }
                else
                {
                    edge = new Edge(Edge.Find(this.theString, this.Edges, active.originNode, theString[active.indexOfFirstCharacter]));
                    int span = active.indexOfLastCharacter - active.indexOfFirstCharacter;
                    //if (theString[edge.indexOfFirstCharacter + span + 1] == theString[indexOfLastCharacter])
                    //{
                    //    break;
                    //}
                    if (theString[edge.indexOfFirstCharacter + span + 1].Equals(theString[indexOfLastCharacter]))
                    {
                        break;
                    }
                    parentNode = Edge.SplitEdge(active, theString, Edges, Nodes, edge);
                }

                Edge newEdge = new Edge(this.theString, indexOfLastCharacter, this.theString.Count - 1 /*this.theString.Length - 1*/, parentNode);
                Edge.Insert(theString, Edges, newEdge);
                if (lastParentNode > 0)
                {
                    Nodes[lastParentNode].suffixNode = parentNode;
                }
                lastParentNode = parentNode;

                if (active.originNode == 0)
                {
                    active.indexOfFirstCharacter++;
                }
                else
                {
                    active.originNode = Nodes[active.originNode].suffixNode;
                }
                active.Canonize();
            }
            if (lastParentNode > 0)
            {
                Nodes[lastParentNode].suffixNode = parentNode;
            }
            active.indexOfLastCharacter++;
            active.Canonize();
        }

        public UserActionList[] GetLongestRepeatedSubstrings(int length)
        {
            List<UserActionList> list = new List<UserActionList>();
            foreach (KeyValuePair<UserActionList, int> pair in _deepested_repeated_prefixes)
            {
                if (pair.Key.Count >= length)
                {
                    list.Add(pair.Key);
                }
            }
            //list.Sort(delegate(UserActionList x, UserActionList y)
            //{
            //    if (x == null)
            //    {
            //        if (y == null)
            //        {
            //            // If x is null and y is null, they're
            //            // equal. 
            //            return 0;
            //        }
            //        else
            //        {
            //            // If x is null and y is not null, y
            //            // is greater. 
            //            return -1;
            //        }
            //    }
            //    else
            //    {
            //        // If x is not null...
            //        //
            //        if (y == null)
            //        // ...and y is null, x is greater.
            //        {
            //            return 1;
            //        }
            //        else
            //        {
            //            // ...and y is not null, compare the 
            //            // lengths of the two lists.
            //            //

            //            int retval = x.Count.CompareTo(y.Count);

            //            if (retval != 0)
            //            {
            //                // If the lists are not of equal length,
            //                // the longer list is greater.
            //                //
            //                return retval;
            //            }
            //            else
            //            {
            //                return _deepested_repeated_prefixes[x].CompareTo(_deepested_repeated_prefixes[y]);
            //            }
            //        }
            //    }
            //});
            //list.Reverse();
            return list.ToArray();
        }

        //private short[] GetLongestCommonPrefix(short[] a, short[] b, int max)
        //{
        //    short[] ret_array;
        //    int len_a = a.Length, len_b = a.Length;
        //    int min = (len_a < len_b ? len_a : len_b);
        //    min = (min < max ? min : max);
        //    for (int i = min; i < min; i++)
        //    {
        //        if (a[i] != b[i])
        //        {
        //            ret_array = new short[i];
        //            Array.Copy(a, ret_array, i);
        //        }
        //    }
        //    ret_array = new short[min];
        //    Array.Copy(a, ret_array, min);
        //}

        //private short[] GetLongestCommonPrefix(short[] a, short[] b)
        //{
        //    short[] ret_array;
        //    int len_a = a.Length, len_b = a.Length;
        //    int min = (len_a < len_b ? len_a : len_b);
        //    for (int i = min; i < min; i++)
        //    {
        //        if (a[i] != b[i])
        //        {
        //            ret_array = new short[i];
        //            Array.Copy(a, ret_array, i);
        //        }
        //    }
        //    ret_array = new short[min];
        //    Array.Copy(a, ret_array, min);
        //}

        //private static int CompareSubstring(short[] x, short[] y)
        //{
        //    if (x == null)
        //    {
        //        if (y == null)
        //        {
        //            // If x is null and y is null, they're
        //            // equal. 
        //            return 0;
        //        }
        //        else
        //        {
        //            // If x is null and y is not null, y
        //            // is greater. 
        //            return -1;
        //        }
        //    }
        //    else
        //    {
        //        // If x is not null...
        //        //
        //        if (y == null)
        //        // ...and y is null, x is greater.
        //        {
        //            return 1;
        //        }
        //        else
        //        {
        //            // ...and y is not null, compare the 
        //            // lengths of the two lists.
        //            //
        //            int retval = x.Count.CompareTo(y.Count);

        //            if (retval != 0)
        //            {
        //                // If the lists are not of equal length,
        //                // the longer list is greater.
        //                //
        //                return retval;
        //            }
        //            else
        //            {
        //                // If the lists are of equal length,
        //                // sort them with ordinary list comparison.
        //                //
        //                for (int i = 0; i < x.Length; i++)
        //                {
        //                    if (x[i] != y[i])
        //                        return x[i].CompareTo(y[i]);
        //                }

        //                return 0;
        //            }
        //        }
        //    }
        //}
    }
}
