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
using ContextLib.Algorithms.Diff;
using ContextLib.DataContainers.GUI;

namespace ContextLib.DataContainers.Monitoring
{
    public class FileRenamedAction : FileAction
    {
        #region Properties
        private string _old_path;
        private string _new_path;
        #endregion

        #region Accessors
        public string OldFile { get { return _old_path; } set { _old_path = value; } }

        public string NewFile { get { return _new_path; } set { _new_path = value; } }

        public override string IpySnippet
        {
            get
            {
                return
                    "window = ContextLib.DataContainers.GUI.Window(" + _window.Handle + ", \"" + _window.ClassName + "\", " + _window.ThreadID + ", " + _window.ProcessID + ", \"" + _window.ProcessName + "\", \"" + _window.Title + "\", " + _window.X + ", " + _window.Y + ", " + _window.Width + ", " + _window.Height + ")" + Environment.NewLine +
                    "action = ContextLib.DataContainers.Monitoring.FileRenamedAction(window, \"" + _old_path + "\", \"" + _new_path + "\", \"" + _folder + "\")" + Environment.NewLine +
                    "action.Execute()" + Environment.NewLine;
            }
        }
        #endregion
        
        #region Constructors
        public FileRenamedAction(string old_path, string new_path, string folder)
            : base(folder)
        {
            _old_path = old_path;
            _new_path = new_path;
            _description = "Rename file " + old_path + " to "+ _new_path + " in folder " + _folder + ".";
            _quick_description = "RenameFile(" + old_path + ", " + new_path + ", " + folder + ")";
            _type = UserActionType.FileRenamedAction;
        }

        public FileRenamedAction(Window window, string old_path, string new_path, string folder)
            : base(window, folder)
        {
            _old_path = old_path;
            _new_path = new_path;
            _description = "Rename file " + old_path + " to " + _new_path + " in folder " + _folder + ".";
            _quick_description = "RenameFile(" + old_path + ", " + new_path + ", " + folder + ")";
            _type = UserActionType.FileRenamedAction;
        }
        #endregion

        #region Public Methods
        public override void Execute()
        {
            if (_old_path != _new_path)
            {
                if (File.Exists(_old_path))
                {
                    File.Move(_old_path, _new_path);
                }
                else if (Directory.Exists(_old_path))
                {
                    Directory.Move(_old_path, _new_path);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            FileRenamedAction action = (FileRenamedAction)obj;
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
                if (action.ActionType == UserActionType.FileRenamedAction)
                {
                    if (Path.GetDirectoryName(this._old_path) == Path.GetDirectoryName(action._old_path) &&
                        ((Path.GetDirectoryName(this._old_path) != Path.GetDirectoryName(this._new_path) &&
                          Path.GetDirectoryName(action._old_path) != Path.GetDirectoryName(action._new_path)) ||
                         (Path.GetDirectoryName(this._old_path) == Path.GetDirectoryName(this._new_path) &&
                          Path.GetDirectoryName(action._old_path) == Path.GetDirectoryName(action._new_path))))
                    {
                        if (Diff.IsDiffIdentical(Diff.DiffString(Path.GetFileName(this._old_path), Path.GetFileName(this._new_path)),
                                                 Diff.DiffString(Path.GetFileName(action._old_path), Path.GetFileName(action._new_path))))
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
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
                if (action.ActionType == UserActionType.FileRenamedAction)
                {
                    FileRenamedAction f_action = (FileRenamedAction)action;
                    if (Path.GetDirectoryName(this._old_path) == Path.GetDirectoryName(f_action._old_path) &&
                        ((Path.GetDirectoryName(this._old_path) != Path.GetDirectoryName(this._new_path) &&
                          Path.GetDirectoryName(f_action._old_path) != Path.GetDirectoryName(f_action._new_path)) ||
                         (Path.GetDirectoryName(this._old_path) == Path.GetDirectoryName(this._new_path) &&
                          Path.GetDirectoryName(f_action._old_path) == Path.GetDirectoryName(f_action._new_path))))
                    {
                        if (Diff.IsDiffIdentical(Diff.DiffString(Path.GetFileName(this._old_path), Path.GetFileName(this._new_path)),
                                                 Diff.DiffString(Path.GetFileName(f_action._old_path), Path.GetFileName(f_action._new_path))))
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public override object Clone()
        {
            FileRenamedAction action = new FileRenamedAction(new Window(_window), _old_path, _new_path, _folder);
            action.Id = this.Id;
            action.Time = this.Time;
            return action;
        }

        public override UserAction Merge(UserAction action)
        {
            UserAction ret = null;
            if (action != null && action.ActionType == UserActionType.FileCreatedAction)
            {
                FileCreatedAction f_action = (FileCreatedAction)action;
                if (this._old_path == f_action.FilePath &&
                    (this.Time - f_action.Time) <= new TimeSpan(MAX_CREATE_DELAY * TimeSpan.TicksPerMillisecond))
                {
                    ret = new FileCreatedAction(this._new_path, this._folder);
                    ret.Time = this._time;
                }
            }
            return ret;
        }
        #endregion
    }
}
