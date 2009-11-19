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
using ContextLib.DataContainers.GUI;

namespace ContextLib.DataContainers.Monitoring
{
    public class TypeTextAction : UserAction
    {
        #region Properties
        private string _text;
        #endregion

        #region Accessors
        public string Text { get { return _text; } set { _text = value; } }

        public override string IpySnippet
        {
            get
            {
                return
                    "window = ContextLib.DataContainers.GUI.Window(" + _window.Handle + ", \"" + _window.ClassName + "\", " + _window.ThreadID + ", " + _window.ProcessID + ", \"" + _window.ProcessName + "\", \"" + _window.Title + "\", " + _window.X + ", " + _window.Y + ", " + _window.Width + ", " + _window.Height + ")" + Environment.NewLine +
                    "action = ContextLib.DataContainers.Monitoring.TypeTextAction(window, \"" + _text + "\")" + Environment.NewLine +
                    "action.Execute()" + Environment.NewLine;
            }
        }
        #endregion

        #region Constructors
        public TypeTextAction(string text) :
            base()
        {
            _text = text;
            _description = "Type \"" + _text + "\" on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "TypeText(\"" + _text + "\")";
            _type = UserActionType.TypeTextAction;
        }

        public TypeTextAction(Window window, string text) :
            base(window)
        {
            _text = text;
            _description = "Type \"" + _text + "\" on window \"" + _window.Title + "\" (" + _window.ProcessName + ".exe)";
            _quick_description = "TypeText(\"" + _text + "\")";
            _type = UserActionType.TypeTextAction;
        }
        #endregion

        #region Public Methods
        public override void Execute()
        {
            BringWindowToFront();
            //System.Windows.Forms.SendKeys.SendWait(_text);
            UserContext.Instance.InsertText(_text, false);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            TypeTextAction action = (TypeTextAction)obj;
            if (action == null) // check if it can be casted
                return false;

            if (this.Id > -1 && action.Id > -1) // id already specified
            {
                if (this.Id == action.Id)
                    return true;
                else
                    return false;
            }
            else
            {
                if (this.Text == action.Text)
                    return true;
                else
                    return false;
            }
        }

        public override bool Equals(UserAction action)
        {
            if ((object)action == null)
                return false;

            if (this.Id > -1 && action.Id > -1) // id already specified
            {
                if (this.Id == action.Id)
                    return true;
                else
                    return false;
            }
            else
            {
                if (action.ActionType == UserActionType.TypeTextAction)
                {
                    TypeTextAction t_action = (TypeTextAction)action;
                    if (this.Text == t_action.Text)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public override object Clone()
        {
            TypeTextAction action = new TypeTextAction(new Window(_window), Text);
            action.Id = this.Id;
            action.Time = this.Time;
            return action;
        }

        public override UserAction Merge(UserAction action)
        {
            UserAction ret = null;
            if (action != null)
            {
                if (action.ActionType == UserActionType.TypeTextAction)
                {
                    TypeTextAction t_action = (TypeTextAction)action;
                    if (this.Window.Handle == t_action.Window.Handle)
                    {
                        string c_a = t_action.Text;
                        string c_b = this.Text;
                        ret = new TypeTextAction(t_action.Window, c_a + c_b);
                        ret.Time = this._time;
                    }
                }
            }
            return ret;
        }
        #endregion
    }
}
