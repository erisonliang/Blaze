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
using System;
using System.Collections.Generic;

namespace ContextLib.DataContainers.Monitoring
{
    public class UserActionList : List<UserAction>, IEquatable<UserActionList>
    {
        #region Properties
        #endregion

        #region Constructors
        public UserActionList()
            : base()
        {
            
        }

        public UserActionList(int capacity)
            : base(capacity)
        {

        }

        public UserActionList(IEnumerable<UserAction> collection)
        {
            this.AddRange(collection);
        }

        public UserActionList(IEnumerable<UserAction> collection, bool clone)
        {
            if (clone)
            {
                foreach (UserAction action in collection)
                    this.Add((UserAction)action.Clone());
            }
            else
            {
                this.AddRange(collection);
            }
        }
        #endregion

        #region Public Methods
        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            UserActionList list = (UserActionList)obj;
            if (list == null) // check if it can be casted
                return false;

            if (this.Count != list.Count)
            {
                return false;
            }
            else
            {
                bool same = true;
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Id != list[i].Id)
                    {
                        same = false;
                        break;
                    }
                }
                return same;
            }
        }

        public bool Equals(UserActionList list)
        {
            if (list == null)
                return false;

            if (this.Count != list.Count)
            {
                return false;
            }
            else
            {
                bool same = true;
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Id != list[i].Id)
                    {
                        same = false;
                        break;
                    }
                }
                return same;
            }
        }

        public override int GetHashCode()
        {
            string str = string.Empty;
            foreach (UserAction action in this)
                str += action.Id.ToString() + ",";
            return str.GetHashCode();
        }
        #endregion
    }
}
