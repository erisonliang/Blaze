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
namespace SystemCore.Settings
{
    public class SystemOptionsInfo
    {
        #region Properties
        private int _update_time;
        #endregion

        #region Accessors
        public int UpdateTime
        {
            get { return _update_time; }
            set { _update_time = value; }
        }
        #endregion

        #region Constructors
        public SystemOptionsInfo(int update_time)
        {
            _update_time = update_time; 
        }

        public SystemOptionsInfo(SystemOptionsInfo info)
        {
            _update_time = info.UpdateTime;
        }
        #endregion
    }
}
