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
    public class AutomationOptionsInfo
    {
        #region Properties
        private bool _is_monitoring_enabled;
        private bool _stop_auto_update_on_battery;
        #endregion

        #region Accessors
        public bool IsMonitoringEnabled
        {
            get { return _is_monitoring_enabled; }
            set { _is_monitoring_enabled = value; }
        }

        public bool StopAutoUpdateOnBattery
        {
            get { return _stop_auto_update_on_battery; }
            set { _stop_auto_update_on_battery = value; }
        }
        #endregion

        #region Constructors
        public AutomationOptionsInfo(bool is_monitoring_enabled, bool stop_auto_update_on_battery)
        {
            _is_monitoring_enabled = is_monitoring_enabled;
            _stop_auto_update_on_battery = stop_auto_update_on_battery;
        }

        public AutomationOptionsInfo(AutomationOptionsInfo info)
        {
            _is_monitoring_enabled = info.IsMonitoringEnabled;
            _stop_auto_update_on_battery = info.StopAutoUpdateOnBattery;
        }
        #endregion
    }
}
