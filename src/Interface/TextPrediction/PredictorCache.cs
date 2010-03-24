using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemCore.CommonTypes;

namespace Blaze.TextPrediction
{
    public class PredictorCache
    {
        #region Properties
        private List<string> _tokens;
        private Dictionary<string, IndexItemSearchResult[]> _results;
        private int _capacity;
        #endregion

        #region Accessors
        public List<string> Tokens { get { return _tokens; } }
        public Dictionary<string, IndexItemSearchResult[]> Results { get { return _results; } }
        private int Capacity { get { return _capacity; } }
        #endregion

        #region Constructors
        public PredictorCache(int capacity)
        {
            _tokens = new List<string>(capacity);
            _results = new Dictionary<string, IndexItemSearchResult[]>(capacity);
            _capacity = capacity;
        }
        #endregion

        #region Public Methods
        public void Add(string key, IndexItemSearchResult[] results)
        {
            if (_results.ContainsKey(key))
            {
                _results[key] = results;
                return;
            }
            // if its full, remove the older one
            if (_tokens.Count == _capacity)
            {
                _results.Remove(_tokens[0]);
                _tokens.RemoveAt(0);
            }
            _tokens.Add(key);
            _results.Add(key, results);
        }

        public IndexItemSearchResult[] Get(string key)
        {
            if (_results.ContainsKey(key))
            {
                // put this back to the end of the queue
                _tokens.Remove(key);
                _tokens.Add(key);
                return _results[key];
            }
            else
                return new IndexItemSearchResult[0];
        }

        public bool Contains(string key)
        {
            return _results.ContainsKey(key);
        }

        public void Clear()
        {
            _tokens.Clear();
            _results.Clear();
        }
        #endregion
    }
}
