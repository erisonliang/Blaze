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
namespace ContextLib.DataContainers.Network
{
    /// <summary>
    /// Represents a data container that provides information about a wireless 
    /// network in the current system.
    /// </summary>
    public class WirelessNetwork
    {
        #region Properties
        private string _ssid;
        private int _signalQuality;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the network Service Set Identifier (SSID).
        /// </summary>
        public string SSID { get { return _ssid; } }
        /// <summary>
        /// Gets the network signal quality, in percentage.
        /// </summary>
        public int SignalQuality { get { return _signalQuality; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of ContextLib.DataContainers.Network.WirelessNetwork class.
        /// </summary>
        /// <param name="ssid">Network Service Set Identifier (SSID).</param>
        /// <param name="signalQuality">Network signal quality, in percentage.</param>
        public WirelessNetwork(string ssid, int signalQuality)
        {
            _ssid = ssid;
            _signalQuality = signalQuality;
        }
        #endregion
    }
}
