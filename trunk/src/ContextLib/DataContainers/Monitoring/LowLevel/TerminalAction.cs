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

namespace ContextLib.DataContainers.Monitoring
{
    public class TerminalAction : UserAction
    {
        #region Properties
        #endregion

        #region Accessors
        public override string IpySnippet
        {
            get { return "# Terminal action"; }
        }
        #endregion

        #region Constructors
        public TerminalAction()
            : base(ContextLib.DataContainers.GUI.Window.InvalidWindow)
        {
            _type = UserActionType.TerminalAction;
            _description = "Terminal Action";
        }
        #endregion

        #region Public Methods
        public override void Execute()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            TerminalAction action = (TerminalAction)obj;
            if (action == null) // check if it can be casted
                return false;

            if (action.ActionType == UserActionType.TerminalAction)
            {
                return true;
            }
            else
                return false;
        }

        public override bool Equals(UserAction action)
        {
            if (action == null)
                return false;

            if (action.ActionType == UserActionType.TerminalAction)
            {
                return true;
            }
            else
                return false;
        }

        public override object Clone()
        {
            return new TerminalAction();
        }

        public override UserAction Merge(UserAction action)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
