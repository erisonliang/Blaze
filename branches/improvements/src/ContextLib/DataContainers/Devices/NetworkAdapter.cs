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

namespace ContextLib.DataContainers.Devices
{
    /// <summary>
    /// Represents a data container that provides information about the network 
    /// interface for a specific networkd adapter.
    /// </summary>
    /// <remarks>This class encapsulates some of the data provided by the win32 classes <see href="http://msdn.microsoft.com/en-us/library/aa394216(VS.85).aspx">Win32_NetworkAdapter</see> and 
    /// <see href="http://msdn.microsoft.com/en-us/library/aa394217(VS.85).aspx">Win32_NetworkAdapterConfiguration</see>.</remarks>
    public class NetworkAdapter
    {
        #region Properties
        private string _id;
        private string _name;
        private string _description;
        private UInt64 _speed;
        private string _mac;
        private string[] _ips;
        private string[] _gateways;
        private bool _dhcp_enabled;
        private string _dhcp_lease_expires;
        private string _dhcp_lease_obtained;
        private string _dhcp_server;
        private string _dns_domain;
        private string _dns_hostname;
        private string[] _dns_server_search_order;
        private string _service_name;
        private string _system_name;
        private UInt32 _index;
        private UInt32 _interface_index;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the network adapter identifier.
        /// </summary>
        public string Id { get { return _id; } }
        /// <summary>
        /// Gets the network adapter name.
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// Gets the network adapter description.
        /// </summary>
        public string Description { get { return _description; } }
        /// <summary>
        /// Gets the current network adapter speed.
        /// </summary>
        public UInt64 Speed { get { return _speed; } }
        /// <summary>
        /// Gets the Media Access Control (MAC) address for this adapter.
        /// </summary>
        public string MacAddress { get { return _mac; } }
        /// <summary>
        /// Gets all IP addresses assigned to this interface.
        /// </summary>
        public string[] IpAddresses { get { return _ips; } }
        /// <summary>
        /// Gets an array of IP addresses of default gateways that the computer system uses.
        /// </summary>
        public string[] Gateways { get { return _gateways; } }
        /// <summary>
        /// If true, the dynamic host configuration protocol (DHCP) is enabled.
        /// </summary>
        public bool DhcpEnabled { get { return _dhcp_enabled; } }
        /// <summary>
        /// Gets the expiration date and time for a leased IP address that was assigned to the computer by the dynamic host configuration protocol (DHCP) server.
        /// </summary>
        public string DhcpLeaseExpires { get { return _dhcp_lease_expires; } }
        /// <summary>
        /// Gets the date and time the lease was obtained for the IP address assigned to the computer by the dynamic host configuration protocol (DHCP) server.
        /// </summary>
        public string DhcpLeaseObtained { get { return _dhcp_lease_obtained; } }
        /// <summary>
        /// Gets the IP address of the dynamic host configuration protocol (DHCP) server.
        /// </summary>
        public string DhcpServer { get { return _dhcp_server; } }
        /// <summary>
        /// Gets the organization name followed by a period and an extension that indicates the type of organization.
        /// </summary>
        public string DnsDomain { get { return _dns_domain; } }
        /// <summary>
        /// Gets the host name used to identify the local computer for authentication by some utilities.
        /// </summary>
        public string DnsHostName { get { return _dns_hostname; } }
        /// <summary>
        /// Gets an array of server IP addresses to be used in querying for DNS servers.
        /// </summary>
        public string[] DnsServerSearchOrder { get { return _dns_server_search_order; } }
        /// <summary>
        /// Gets the service name of the network adapter.
        /// </summary>
        public string ServiceName { get { return _service_name; } }
        /// <summary>
        /// Gets the name of the scoping system.
        /// </summary>
        public string SystemName { get { return _system_name; } }
        /// <summary>
        /// Gets the index number of the Windows network adapter configuration.
        /// </summary>
        public UInt32 Index { get { return _index; } }
        /// <summary>
        /// Gets the index value that uniquely identifies a local network interface.
        /// </summary>
        public UInt32 InterfaceIndex { get { return _interface_index; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of ContextLib.DataContainers.Devices.NetworkAdapter class.
        /// </summary>
        /// <param name="id">Network adapter identifier.</param>
        /// <param name="name">Network adapter name.</param>
        /// <param name="description">Network adapter description.</param>
        /// <param name="speed">Network adapter speed.</param>
        /// <param name="mac">Media Access Control (MAC) address for this adapter.</param>
        /// <param name="ips">IP addresses assigned to this interface.</param>
        /// <param name="gateways">Array of IP addresses of default gateways that the computer system uses.</param>
        /// <param name="dhcp_enabled">Expiration date and time for a leased IP address that was assigned to the computer by the dynamic host configuration protocol (DHCP) server.</param>
        /// <param name="dhcp_lease_expires">Date and time the lease was obtained for the IP address assigned to the computer by the dynamic host configuration protocol (DHCP) server.</param>
        /// <param name="dhcp_lease_obtained">Date and time the lease was obtained for the IP address assigned to the computer by the dynamic host configuration protocol (DHCP) server.</param>
        /// <param name="dhcp_server">IP address of the dynamic host configuration protocol (DHCP) server.</param>
        /// <param name="dns_domain">Organization name followed by a period and an extension that indicates the type of organization.</param>
        /// <param name="dns_hostname">Host name used to identify the local computer for authentication by some utilities.</param>
        /// <param name="dns_server_search_order">Array of server IP addresses to be used in querying for DNS servers.</param>
        /// <param name="service_name">Service name of the network adapter.</param>
        /// <param name="system_name">Name of the scoping system.</param>
        /// <param name="index">Index number of the Windows network adapter configuration.</param>
        /// <param name="interface_index">Index value that uniquely identifies a local network interface.</param>
        public NetworkAdapter(string id, string name, string description, UInt64 speed, string mac, string[] ips, string[] gateways, bool dhcp_enabled,
                                string dhcp_lease_expires, string dhcp_lease_obtained, string dhcp_server, string dns_domain,
                                string dns_hostname, string[] dns_server_search_order, string service_name, string system_name, UInt32 index, UInt32 interface_index)
        {
            _id = id;
            _name = name;
            _description = description;
            _speed = speed;
            _mac = mac;
            _ips = ips;
            _gateways = gateways;
            _dhcp_enabled = dhcp_enabled;
            _dhcp_lease_expires = dhcp_lease_expires;
            _dhcp_lease_obtained = dhcp_lease_obtained;
            _dhcp_server = dhcp_server;
            _dns_domain = dns_domain;
            _dns_hostname = dns_hostname;
            _dns_server_search_order = dns_server_search_order;
            _service_name = service_name;
            _system_name = system_name;
            _index = index;
            _interface_index = interface_index;
        }
        #endregion
    }
}
