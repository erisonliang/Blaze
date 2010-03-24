using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SystemCore.CommonTypes
{
    [Serializable]
    public class IndexItem
    {
        #region Properties
        private Index _parent;
        private string _name;
        private string _path; // optional (only used if Type == Indexer)
        private string _icon_id;
        private short _n_tokens;
        #endregion

        #region Accessors
        public string Name { get { return _name; } }
        public string Path { get { return _path; } }
        public string IconId { get { return _icon_id; } }
        public short NTokens { get { return _n_tokens; } }
        #endregion

        #region Constructors
        public IndexItem(Index parent)
        {
            _parent = parent;
            _name = string.Empty;
            _path = string.Empty;
            _icon_id = string.Empty;
            _n_tokens = 0;
        }

        public IndexItem(Index parent, string name, string path, string icon_id, short n_tokens)
        {
            _parent = parent;
            _name = name;
            _path = path;
            _icon_id = icon_id;
            _n_tokens = n_tokens;
        }

        public IndexItem(IndexItem item)
        {
            _parent = item._parent;
            _name = item._name;
            _path = item._path;
            _icon_id = item._icon_id;
            _n_tokens = item._n_tokens;
        }
        #endregion

        #region Public Methods
        public Bitmap GetIcon()
        {
            return _parent.GetItemIcon(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            IndexItem item = obj as IndexItem;
            if ((Object)item == null)
                return false;

            return (this.Name == item.Name) &&
                    (this.Path == item.Path) &&
                    (this.IconId == item.IconId) &&
                    (this.NTokens == item.NTokens);
        }

        public override int GetHashCode()
        {
            return (this._name + this._path + this._icon_id + this._n_tokens).GetHashCode();
        }
        #endregion

        #region Operators
        public static bool operator ==(IndexItem a, IndexItem b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return (a.Name == b.Name) &&
                    (a.Path == b.Path) &&
                    (a.IconId == b.IconId) &&
                    (a.NTokens == b.NTokens);
        }

        public static bool operator !=(IndexItem a, IndexItem b)
        {
            return !(a == b);
        }
        #endregion
    }
}
