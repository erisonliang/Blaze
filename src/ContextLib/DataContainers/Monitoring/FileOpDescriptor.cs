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
    public class FileOpDescriptor : IEquatable<FileOpDescriptor>
    {
        #region Properties
        //private string _token;
        private List<string> _file_list1;
        private List<string> _file_list2;
        private List<string> _rtf_list1;
        private List<string> _rtf_list2;
        private string _extension;
        #endregion

        #region Accessors
        //public string Token { get { return _token; } set { _token = value; } }
        public List<string> FileList1 { get { return _file_list1; } set { _file_list1 = value; } }
        public List<string> FileList2 { get { return _file_list2; } set { _file_list2 = value; } }
        public List<string> RtfList1 { get { return _rtf_list1; } set { _rtf_list1 = value; } }
        public List<string> RtfList2 { get { return _rtf_list2; } set { _rtf_list2 = value; } }
        public string Extension { get { return _extension; } set { _extension = value; } }
        #endregion

        #region Constructors
        public FileOpDescriptor()
        {
            //_token = string.Empty;
            _file_list1 = null;
            _file_list2 = null;
            _rtf_list1 = null;
            _rtf_list2 = null;
            _extension = string.Empty;
        }
        #endregion

        #region Overridden Methods
        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            FileOpDescriptor fod = (FileOpDescriptor)obj;
            if (fod == null) // check if it can be casted
                return false;

            if (this.FileList1.Count != fod.FileList1.Count)
            {
                return false;
            }
            else
            {
                bool same = true;
                for (int i = 0; i < this.FileList1.Count; i++)
                {
                    if (this.FileList1[i] != fod.FileList1[i])
                    {
                        same = false;
                        break;
                    }
                }
                if (this.FileList2 != null)
                {
                    if (fod.FileList2 != null)
                    {
                        for (int i = 0; i < this.FileList2.Count; i++)
                        {
                            if (this.FileList2[i] != fod.FileList2[i])
                            {
                                same = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        same = false;
                    }
                }
                else if (fod.FileList2 != null)
                {
                    same = false;
                }
                return same;
            }
        }

        public bool Equals(FileOpDescriptor fod)
        {
            if (fod == null)
                return false;

            if (this.FileList1.Count != fod.FileList1.Count)
            {
                return false;
            }
            else
            {
                bool same = true;
                for (int i = 0; i < this.FileList1.Count; i++)
                {
                    if (this.FileList1[i] != fod.FileList1[i])
                    {
                        same = false;
                        break;
                    }
                }
                if (this.FileList2 != null)
                {
                    if (fod.FileList2 != null)
                    {
                        for (int i = 0; i < this.FileList2.Count; i++)
                        {
                            if (this.FileList2[i] != fod.FileList2[i])
                            {
                                same = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        same = false;
                    }
                }
                else if (fod.FileList2 != null)
                {
                    same = false;
                }
                return same;
            }
        }

        public override int GetHashCode()
        {
            string str = string.Empty;
            foreach (string file in _file_list1)
                str += file + Environment.NewLine;
            if (_file_list2 != null)
            {
                str += @"<>" + Environment.NewLine;
                foreach (string file in _file_list2)
                    str += file + Environment.NewLine;
            }
            return str.GetHashCode();
        }
        #endregion
    }
}
