using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemCore.CommonTypes
{
    [Serializable]
    public class BKTree<T>
    {
        private Node<T> root;
        public delegate ushort Distance(T x, T y); 
        private readonly Distance distance = null;

        public BKTree(Distance distance)
        {
            root = null;
            this.distance = distance;
        }

        public void Add(T term)
        {
            if (root != null)
            {
                root.Add(term);
            }
            else
            {
                root = new Node<T>(term, distance);
            }
        }

        public Dictionary<T, ushort> Query(T searchObject, ushort threshold)
        {
            Dictionary<T, ushort> matches = new Dictionary<T, ushort>();
            root.Query(searchObject, threshold, ref matches);
            return matches;
        }
    }

    [Serializable]
    public class Node<T>
    {
        private readonly BKTree<T>.Distance distance = null;
        private T term;
        private SortedDictionary<ushort, Node<T>> children;

        public T Term { get { return term; } }

        public Node(T term, BKTree<T>.Distance distance)
        {
            this.term = term;
            children = new SortedDictionary<ushort, Node<T>>();
            this.distance = distance;
        }

        public void Add(T term)
        {
            ushort score = distance(term, this.Term);
            if (children.ContainsKey(score))
            {
                Node<T> child = children[score];
                child.Add(term);
            }
            else
            {
                children.Add(score, new Node<T>(term, distance));
            }
        }

        public void Query(T term, ushort threshold, ref Dictionary<T, ushort> collected)
        {
            ushort distanceAtNode = distance(term, this.Term);
            if (distanceAtNode <= threshold && !collected.ContainsKey(this.term))
            {
                collected.Add(this.term, distanceAtNode);
            }
            for (short score = (short)(distanceAtNode - threshold); score <= distanceAtNode + threshold; score++)
            {
                if (score > 0)
                {
                    if (children.ContainsKey((ushort)score))
                    {
                        Node<T> child = children[(ushort)score];
                        child.Query(term, threshold, ref collected);
                    }
                }
            }
        }
    }
}
