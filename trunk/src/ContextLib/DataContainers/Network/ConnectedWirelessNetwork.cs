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
    /// network with a established connection.
    /// </summary>
    public class ConnectedWirelessNetwork
    {
        #region Properties
        private string _ssid;
        private string _bssid;
        private int _signalQuality;
        private int _receptionRate;
        private int _transmissionRate;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the network Service Set Identifier (SSID).
        /// </summary>
        public string SSID { get { return _ssid; } }
        /// <summary>
        /// Gets the Basic Service Set Identifier (BSSID) of the associated access point.
        /// </summary>
        public string BSSID { get { return _bssid; } }
        /// <summary>
        /// Gets the network signal quality, in percentage.
        /// </summary>
        public int SignalQuality { get { return _signalQuality; } }
        /// <summary>
        /// Gets the connection reception rate.
        /// </summary>
        public int ReceptionRate { get { return _receptionRate; } }
        /// <summary>
        /// Gets the connection transmission rate.
        /// </summary>
        public int TransmissionRate { get { return _transmissionRate; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of ContextLib.DataContainers.Network.WirelessNetwork class.
        /// </summary>
        /// <param name="ssid">Network Service Set Identifier (SSID).</param>
        /// <param name="bssid">Network Basic Service Set Identifier (BSSID).</param>
        /// <param name="signalQuality">Network signal quality, in percentage.</param>
        /// <param name="receptionRate">Connection receiving rate.</param>
        /// <param name="transmissionRate">Connection transmission rate.</param>
        public ConnectedWirelessNetwork(string ssid, string bssid, int signalQuality, int receptionRate, int transmissionRate)
        {
            _ssid = ssid;
            _bssid = bssid;
            _signalQuality = signalQuality;
            _receptionRate = receptionRate;
            _transmissionRate = transmissionRate;
        }
        #endregion
    }
}
