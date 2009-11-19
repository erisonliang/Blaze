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
using System.IO;
using ContextLib.DataContainers.GUI;

namespace ContextLib.DataContainers.Monitoring
{
    public class FileDeletedAction : FileAction
    {
        #region Properties
        private string _file;
        #endregion

        #region Accessors
        public string FilePath { get { return _file; } set { _file = value; } }

        public override string IpySnippet
        {
            get
            {
                return
                    "window = ContextLib.DataContainers.GUI.Window(" + _window.Handle + ", \"" + _window.ClassName + "\", " + _window.ThreadID + ", " + _window.ProcessID + ", \"" + _window.ProcessName + "\", \"" + _window.Title + "\", " + _window.X + ", " + _window.Y + ", " + _window.Width + ", " + _window.Height + ")" + Environment.NewLine +
                    "action = ContextLib.DataContainers.Monitoring.FileDeletedAction(window, \"" + _file + "\", \"" + _folder + "\")" + Environment.NewLine +
                    "action.Execute()" + Environment.NewLine;
            }
        }
        #endregion
        
        #region Constructors
        public FileDeletedAction(string file, string folder)
            : base(folder)
        {
            _file = file;
            _description = "Delete file " + file + " in folder " + _folder + ".";
            _quick_description = "DeleteFile(" + file + ", " + folder + ")";
            _type = UserActionType.FileDeletedAction;
        }

        public FileDeletedAction(Window window, string file, string folder)
            : base(window, folder)
        {
            _file = file;
            _description = "Delete file " + file + " in folder " + _folder + ".";
            _quick_description = "DeleteFile(" + file + ", " + folder + ")";
            _type = UserActionType.FileDeletedAction;
        }
        #endregion

        #region Public Methods
        public override void Execute()
        {
            try
            {
                if (File.Exists(_file))
                {
                    File.Delete(_file);
                }
                else if (Directory.Exists(_file))
                {
                    Directory.Delete(_file);
                }
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            UserAction action = (UserAction)obj;
            if (action == null) // check if it can be casted
                return false;

            //UserAction action;
            //try
            //{
            //    action = (UserAction)obj;
            //    if (action == null) // check if it can be casted
            //        return false;
            //}
            //catch
            //{
            //    return false;
            //}

            if (this.Id > -1 && action.Id > -1) // id already specified
            {
                if (this.Id == action.Id)
                    return true;
                else
                    return false;
            }
            else
            {
                if (action.ActionType == UserActionType.FileDeletedAction)
                {
                    FileDeletedAction f_action = (FileDeletedAction)action;
                    if (Path.GetFileNameWithoutExtension(this._file) == Path.GetFileNameWithoutExtension(f_action._file) ||
                        Path.GetExtension(this._file) == Path.GetExtension(f_action._file))
                        return true;
                    else
                        return false;
                }
                //else if (action.ActionType == UserActionType.FileCreatedAction)
                //{
                //    FileCreatedAction f_action = (FileCreatedAction)action;
                //    if (Path.GetDirectoryName(this._file).Length < Path.GetDirectoryName(f_action.FileName).Length &&
                //        Path.GetFileName(this._file) == Path.GetFileName(f_action.FileName))
                //        return true;
                //    else
                //        return false;
                //}
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
                if (action.ActionType == UserActionType.FileDeletedAction)
                {
                    FileDeletedAction f_action = (FileDeletedAction)action;
                    if (Path.GetFileNameWithoutExtension(this._file) == Path.GetFileNameWithoutExtension(f_action._file) ||
                        Path.GetExtension(this._file) == Path.GetExtension(f_action._file))
                        return true;
                    else
                        return false;
                }
                //else if (action.ActionType == UserActionType.FileCreatedAction)
                //{
                //    FileCreatedAction f_action = (FileCreatedAction)action;
                //    if (Path.GetDirectoryName(this._file).Length < Path.GetDirectoryName(f_action.FileName).Length &&
                //        Path.GetFileName(this._file) == Path.GetFileName(f_action.FileName))
                //        return true;
                //    else
                //        return false;
                //}
                else
                    return false;
            }
        }

        public override object Clone()
        {
            FileDeletedAction action = new FileDeletedAction(new Window(_window), _file, _folder);
            action.Id = this.Id;
            action.Time = this.Time;
            return action;
        }

        public override UserAction Merge(UserAction action)
        {
            return null;
        }
        #endregion
    }
}
