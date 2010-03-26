using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemCore.CommonTypes
{
    public class IndexItemSearchResult : IComparable
    {
        #region Properties
        public IndexItem Result { get; set; }
        public short Error { get; set; }
        public bool IsLearned { get; set; }
        #endregion

        #region Constructors
        public IndexItemSearchResult(IndexItem result, short error)
        {
            Result = result;
            Error = error;
            IsLearned = false;
        }

        public IndexItemSearchResult(IndexItem result, short error, bool is_learned)
        {
            Result = result;
            Error = error;
            IsLearned = is_learned;
        }
        #endregion

        #region Public Methods
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            IndexItemSearchResult item = obj as IndexItemSearchResult;
            if ((Object)item == null)
                return false;

            return (this.Result == item.Result);
        }

        public override int GetHashCode()
        {
            return this.Result.GetHashCode();
        }

        int IComparable.CompareTo(object obj)
        {
            IndexItemSearchResult result = obj as IndexItemSearchResult;
            int eval = this.Error.CompareTo(result.Error);
            if (eval == 0)
            {
                eval = result.IsLearned.CompareTo(this.IsLearned);
                if (eval == 0)
                    eval = this.Result.NTokens.CompareTo(result.Result.NTokens);
            }
            return eval;
        }
        #endregion

        #region Operators
        public static bool operator ==(IndexItemSearchResult a, IndexItemSearchResult b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return (a.Result == b.Result);
        }

        public static bool operator !=(IndexItemSearchResult a, IndexItemSearchResult b)
        {
            return !(a == b);
        }
        #endregion
    }
}
