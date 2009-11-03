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
        private bool _stop_auto_update_on_battery;
        private bool _auto_updates;
        #endregion

        #region Accessors
        public int UpdateTime
        {
            get { return _update_time; }
            set { _update_time = value; }
        }

        public bool StopAutoUpdateOnBattery
        {
            get { return _stop_auto_update_on_battery; }
            set { _stop_auto_update_on_battery = value; }
        }

        public bool AutoUpdates
        {
            get { return _auto_updates; }
            set { _auto_updates = value; }
        }
        #endregion

        #region Constructors
        public SystemOptionsInfo(int update_time, bool stop_auto_update_on_battery, bool auto_updates)
        {
            _update_time = update_time;
            _stop_auto_update_on_battery = stop_auto_update_on_battery;
            _auto_updates = auto_updates;
        }

        public SystemOptionsInfo(SystemOptionsInfo info)
        {
            _update_time = info.UpdateTime;
            _stop_auto_update_on_battery = info.StopAutoUpdateOnBattery;
            _auto_updates = info.AutoUpdates;
        }
        #endregion
    }
}
