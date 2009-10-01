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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SystemCore.SystemAbstraction;
using SystemCore.SystemAbstraction.WindowManagement;
using Configurator;
using ContextLib.DataContainers.Devices;
using ContextLib.DataContainers.GUI;
using ContextLib.DataContainers.Multimedia;
using ContextLib.DataContainers.Network;
using NativeWifi;
using ThreadState=System.Diagnostics.ThreadState;
using IWshRuntimeLibrary;

namespace ContextLib
{
    /// <summary>
    /// Singleton object that provides methods to access information about user context.
    /// </summary>
    public sealed class UserContext
    {
        #region Properties
        private static volatile UserContext _instance;
        private static object _sync = new Object();
        private MultiLevelData _selected_content;
        private bool _is_windows_explorer_top_window;
        private Observer _observer;
        private Apprentice _apprentice;
        private Assistant _assistant;
        //private Thread _activity_monitor;
        private List<PostFocusDelegate> _post_focus_operations;
        #endregion

        #region Accessors
        /// <summary>
        /// Returns the instance of UserContext.
        /// </summary>
        public static UserContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_sync)
                    {
                        if (_instance == null)
                            _instance = new UserContext();
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// Gets a boolean indicating if the top windows is an instance of Windows Explorer or not.
        /// </summary>
        public bool IsWindowsExplorerOnTop
        {
            get { return _is_windows_explorer_top_window; }
        }

        public Observer ObserverObject { get { return _observer; } }

        public Apprentice ApprenticeObject { get { return _apprentice; } }

        public Assistant AssistantObject { get { return _assistant; } }
        #endregion

        #region Constructors
        private UserContext()
        {
            _selected_content = new MultiLevelData();
            _is_windows_explorer_top_window = false;
            _post_focus_operations = new List<PostFocusDelegate>();
            _observer = new Observer(20, 40000, 40000, 500);
            _apprentice = new Apprentice();
            _assistant = new Assistant();
            _observer.Apprentice = _apprentice;
            _observer.Assistant = _assistant;
            _observer.ResumeMonitoring();
        }

        ~UserContext()
        {
            _observer.PauseMonitoring();
        }
        #endregion

        #region Delegates
        private delegate void PostFocusDelegate();
        #endregion

        #region Public Methods

        #region Operation Methods
        /// <summary>
        /// Takes a snapshot of the user's selected contents. This method must be executed before the calling application gets user focus.
        /// </summary>
        /// <remarks>As this method users the clipboard as a means of data transfer, 
        /// if no data is selected by the user, the previous clipboard contente will 
        /// be returned.</remarks>
        public void TakeContextSnapshot()
        {
            WindowUtility.Instance.Refresh();
            VWindow window = WindowUtility.Instance.GetTopWindow();
            //Window swindow = GetTopWindow();
            //MessageBox.Show(swindow.Title);
            if (window != VWindow.InvalidWindow && WindowUtility.Instance.GetProcessName(window.Handle) == "explorer") // is it a Windows Explorer window? // window.ClassName == "CabinetWClass"
            {
                _is_windows_explorer_top_window = true;
            }
            else
            {
                _is_windows_explorer_top_window = false;
            }
            // backup clipboard
            MultiLevelData clipboard_backup = new MultiLevelData();
            clipboard_backup.PopulateFromClipboard();
            // copy selected content into the clipboard
            //Thread.Sleep(5);

            

            //SendKeys.SendWait("^{INSERT}");

            uint processId;
            uint activeThreadId = Win32.GetWindowThreadProcessId(Win32.GetForegroundWindow(), out processId);
            uint currentThreadId = Win32.GetCurrentThreadId();

            //Win32.AttachThreadInput(activeThreadId, currentThreadId, true);
            //IntPtr focusedHandle = Win32.GetFocus();
            //Win32.PostMessage(focusedHandle, Win32.WM_MOUSELEAVE, IntPtr.Zero, IntPtr.Zero);
            //Win32.AttachThreadInput(activeThreadId, currentThreadId, false);
            //SendKeys.SendWait("");
            SendKeys.SendWait("^{INSERT}");
            //byte[] keyState = new byte[256];
            //Win32.GetKeyboardState(keyState);
            //keyState[(int)Keys.LControlKey] |= 0x80;
            //keyState[(int)Keys.X] |= 0x80;
            //Win32.SetKeyboardState(keyState);

            //Win32.SendMessageA(focusedHandle, 0x100, (IntPtr)Keys.LControlKey, IntPtr.Zero);
            //Win32.SendMessageA(focusedHandle, 0x100, (IntPtr)Keys.X, IntPtr.Zero);
            ////Win32.SendMessageA(focusedHandle, 0x102, (IntPtr)0x03, (IntPtr)1);
            //Win32.SendMessageA(focusedHandle, 0x101, (IntPtr)Keys.X, (IntPtr)1);
            //Win32.SendMessageA(focusedHandle, 0x101, (IntPtr)Keys.LControlKey, (IntPtr)1);

            //keyState[(int)Keys.LControlKey] &= 0x00;
            //keyState[(int)Keys.X] &= 0x00;
            //Win32.SetKeyboardState(keyState);
            //Win32.AttachThreadInput(activeThreadId, currentThreadId, false);

            // store clipboard content
            //Thread.Sleep(5);
            _selected_content.PopulateFromClipboard();
            // restore clipboard
            //Thread.Sleep(5);
            clipboard_backup.RestoreToClipboard();
            //_selected_content = new MultiLevelData();
            //_selected_content.Text = WindowUtility.Instance.GetTextFromTopWindowEx();
            //Thread snapshot = new Thread(new ThreadStart(delegate()
            //    {
                    
            //    }));
            //snapshot.SetApartmentState(ApartmentState.STA);
            //snapshot.Start();
            //snapshot.Join();
        }

        /// <summary>
        /// Performs all scheduled Post Focus (PF) operations. This method should only be executed once the calling application lost focus.
        /// </summary>
        /// <remarks>A Post Focus operation consists of a method that will only be executed once <see cref="PerformPostFocusOperations"/> is also executed.</remarks>
        public void PerformPostFocusOperations()
        {
            foreach (PostFocusDelegate operation in _post_focus_operations)
            {
                Thread op = new Thread(new ThreadStart(delegate()
                    {
                        operation();
                    }));
                op.SetApartmentState(ApartmentState.STA);
                op.Start();
            }
            _post_focus_operations.Clear();
        }
        #endregion

        #region Application Methods
        /// <summary>
        /// Gets a Window object representing the window on top in the current system.
        /// </summary>
        /// <returns>Window object containing information about the top window. Null is returned 
        /// if there is no top window.</returns>
        public Window GetTopWindow()
        {
            VWindow window = WindowUtility.Instance.GetTopWindow();
            if (window != VWindow.InvalidWindow)
            {
                //IntPtr handle = window.Handle;
                //string className = WindowUtility.Instance.GetClassName(handle);
                //int threadId = (int)WindowUtility.Instance.GetThreadId(handle);
                //int processId = (int)WindowUtility.Instance.GetProcessId(handle);
                //string processName = WindowUtility.Instance.GetProcessName(handle);
                //string title = WindowUtility.Instance.GetWindowTitle(handle);
                //return new Window(handle.ToInt32(), className, threadId, processId, processName, title);
                return new Window(window.Handle);
            }
            else
                return null;
        }

        public Window GetTopWindow(string[] procs)
        {
            int order = -1;
            VWindow window = null;
            foreach (string proc in procs)
            {
                VWindow tmp_window = WindowUtility.Instance.GetTopWindow(proc);
                if (tmp_window != VWindow.InvalidWindow)
                {
                    if (order == -1)
                    {
                        order = tmp_window.Zorder;
                        window = tmp_window;
                    }
                    else if (tmp_window.Zorder < order)
                    {
                        order = tmp_window.Zorder;
                        window = tmp_window;
                    }
                }
            }
            return new Window(window.Handle);
        }

        /// <summary>
        /// Indicates if the specified application is already running on the system.
        /// </summary>
        /// <param name="name">Process name of the application.</param>
        /// <returns>True if the application is already open, false otherwise.</returns>
        /// <remarks>The process name must be specified without including the file extension (.exe).</remarks>
        public bool IsApplicationOpen(string name)
        {
            Process[] process = Process.GetProcessesByName(name);
            if (process.Length > 0)
                return true;
            else
                return false;
        }

        public bool IsWindowOpen(Window wnd)
        {
            return WindowUtility.Instance.IsWindowOpen((IntPtr)wnd.Handle);
        }
        #endregion

        #region Clipboard Methods
        /// <summary>
        /// Gets a MultiLevelData object containing the currently selected content by the user.
        /// </summary>
        /// <returns>MultiLevelData container with the selected content.</returns>
        /// <remarks>This method requires the user context to be frist captured. Therefore, this method 
        /// requires TakeContextSnapshot to be called first.</remarks>
        public MultiLevelData GetSelectedContent()
        {
            if (_is_windows_explorer_top_window)
            {
                VWindow top_window = WindowUtility.Instance.GetTopWindow();
                string[] fileList = WindowUtility.Instance.GetWExplorerSelectedContent(top_window.Handle);
                return new MultiLevelData(fileList);
            }
            else
            {
                return new MultiLevelData(_selected_content);
            }
        }

        /// <summary>
        /// Gets a MultiLevelData object containing the Clipboard data.
        /// </summary>
        /// <returns>MultiLevelData container with the clipboard data.</returns>
        public MultiLevelData GetClipboardContent()
        {
            MultiLevelData clipboard = new MultiLevelData();
            clipboard.PopulateFromClipboard();
            return clipboard;
        }

        public void SetClipboardText(string text)
        {
            ThreadStart ts = new ThreadStart(delegate ()
                                                 {
                                                     Clipboard.SetText(text);
                                                 });
            Thread t = new Thread(ts);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
        #endregion

        #region Windows Explorer Methods
        /// <summary>
        /// Gets the path from the top most Windows Explorer window. If the top most window isn't and explorer window and
        /// onTop is set to true, the path to desktop is returned.
        /// </summary>
        /// <param name="onTop">Boolean specifying if the Windows Explorer window must be the top most window in the system or not.</param>
        /// <returns>Path of the top most Windows Explorer window. </returns>
        public string GetExplorerPath(bool onTop)
        {
            if (onTop)
            {
                if (_is_windows_explorer_top_window)
                {
                    VWindow top_window = WindowUtility.Instance.GetTopWindow();
                    if (top_window == VWindow.InvalidWindow)
                        return string.Empty;
                    else
                        return WindowUtility.Instance.GetWExplorerUrl(top_window.Handle);
                }
                else
                {
                    //return string.Empty;
                    return CommonInfo.UserDesktop;
                }
            }
            else
            {
                VWindow top_window = WindowUtility.Instance.GetTopWindow("explorer");
                if (top_window == VWindow.InvalidWindow)
                    return CommonInfo.UserDesktop;
                else
                    return WindowUtility.Instance.GetWExplorerUrl(top_window.Handle);
            }
        }

        /// <summary>
        /// Gets the currently selected files or folders by the user, in the top most Windows Explorer window.
        /// </summary>
        /// <param name="onTop">Boolean specifying if the Windows Explorer window must be the top most window in the system or not.</param>
        /// <returns>Array containing the paths from selected files and folders.</returns>
        public string[] GetExplorerSelectedItems(bool onTop)
        {
            if (onTop)
            {
                if (_is_windows_explorer_top_window)
                {
                    VWindow top_window = WindowUtility.Instance.GetTopWindow();
                    if (top_window == VWindow.InvalidWindow)
                        return new string[0];
                    else
                        return WindowUtility.Instance.GetWExplorerSelectedContent(top_window.Handle);
                }
                else
                {
                    return new string[0];
                }
            }
            else
            {
                VWindow top_window = WindowUtility.Instance.GetTopWindow("explorer");
                if (top_window == VWindow.InvalidWindow)
                    return new string[0];
                else
                    return WindowUtility.Instance.GetWExplorerSelectedContent(top_window.Handle);
            }
        }

        /// <summary>
        /// Gets all files and folders contained in the folder opened in the top most Windows Explorer window.
        /// </summary>
        /// <param name="onTop">Boolean specifying if the Windows Explorer window must be the top most window in the system or not.</param>
        /// <returns>Array containing the paths.</returns>
        public string[] GetExplorerItems(bool onTop)
        {
            if (onTop)
            {
                if (_is_windows_explorer_top_window)
                {
                    VWindow top_window = WindowUtility.Instance.GetTopWindow();
                    if (top_window == VWindow.InvalidWindow)
                        return new string[0];
                    else
                        return WindowUtility.Instance.GetWExplorerContent(top_window.Handle);
                }
                else
                {
                    return new string[0];
                }
            }
            else
            {
                VWindow top_window = WindowUtility.Instance.GetTopWindow("explorer");
                if (top_window == VWindow.InvalidWindow)
                    return new string[0];
                else
                    return WindowUtility.Instance.GetWExplorerContent(top_window.Handle);
            }
        }
        #endregion

        #region Web Browser Methods
        /// <summary>
        /// Gets the URL of the top most browser window.
        /// </summary>
        /// <returns>Browser's window URL.</returns>
        /// <remarks>This method only works with Internet Explorer 6.x and 7.x, Mozilla Firefox 3.x, Opera 9.x and Google Chrome 1.x.</remarks>
        public string GetBrowserUrl()
        {
            //List<string> known_browsers = new List<string>(new string[] { "IEFrame", "MozillaUIWindowClass", "Chrome_VistaFrame", "OpWindow" }); //#32770
            List<string> known_browsers = new List<string>(new string[] { "iexplorer", "iexplore", "firefox", "chrome", "opera" }); //#32770
            Window top_window = GetTopWindow(known_browsers.ToArray());
            //string top_class = WindowUtility.Instance.GetClassName(top_window.Handle);
            if (top_window != null)
            {
                string process = top_window.ProcessName.ToLower();
                if (process == known_browsers[0] || process == known_browsers[1])
                    return WindowUtility.Instance.GetIExplorerUrl(new IntPtr(top_window.Handle));
                else if (process == known_browsers[2])
                    return WindowUtility.Instance.GetFirefoxUrl(new IntPtr(top_window.Handle));
                else if (process == known_browsers[3])
                    return WindowUtility.Instance.GetChromeUrl(new IntPtr(top_window.Handle));
                else if (process == known_browsers[4])
                    return WindowUtility.Instance.GetOperaUrl(new IntPtr(top_window.Handle));
                else
                    return string.Empty;
            }
            return string.Empty;
        }

        public string GetBrowserPageName()
        {
            List<string> known_browsers = new List<string>(new string[] { "iexplorer", "iexplore", "firefox", "chrome", "opera" }); //#32770
            Window top_window = GetTopWindow(known_browsers.ToArray());
            if (top_window != null)
            {
                string title = top_window.Title;
                int index = title.LastIndexOf(" - ");
                if (index > -1)
                    return title.Substring(0, index);
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the content of the top most browser window.
        /// </summary>
        /// <param name="onTop">Boolean specifying if the browser's window must be the top most window in the system.</param>
        /// <param name="html">Boolean specifying if the returned value should be formated as html code.</param>
        /// <returns>Browser's window content.</returns>
        /// <remarks>This method only works for Internet Explorer.</remarks>
        public string GetBrowserContent(bool onTop, bool html)
        {
            VWindow top_window = WindowUtility.Instance.GetTopWindow("IEFrame");
            bool is_browser_top_window = (top_window.Zorder == 0 ? true : false);
            if (onTop)
            {
                if (is_browser_top_window)
                {
                    if (top_window == VWindow.InvalidWindow)
                        return string.Empty;
                    else if (html)
                        return WindowUtility.Instance.GetIExplorerContent(top_window.Handle);
                    else
                        return WindowUtility.Instance.GetIExplorerText(top_window.Handle);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                if (top_window == VWindow.InvalidWindow)
                    return string.Empty;
                else if (html)
                    return WindowUtility.Instance.GetIExplorerContent(top_window.Handle);
                else
                    return WindowUtility.Instance.GetIExplorerText(top_window.Handle);
            }
        }

        /// <summary>
        /// Gets the selected content, by the user, of the top most browser window.
        /// </summary>
        /// <param name="onTop">Boolean specifying if the browser's window must be the top most window in the system.</param>
        /// <param name="html">Boolean specifying if the returned value should be formated as html code.</param>
        /// <returns>Browser's window selected content.</returns>
        /// <remarks>This method only works for Internet Explorer.</remarks>
        public string GetBrowserSelectedContent(bool onTop, bool html)
        {
            VWindow top_window = WindowUtility.Instance.GetTopWindow("IEFrame");
            bool is_browser_top_window = (top_window.Zorder == 0 ? true : false);
            if (onTop)
            {
                if (is_browser_top_window)
                {
                    if (top_window == VWindow.InvalidWindow)
                        return string.Empty;
                    else if (html)
                        return WindowUtility.Instance.GetIExplorerSelectedContent(top_window.Handle);
                    else
                        return WindowUtility.Instance.GetIExplorerSelectedText(top_window.Handle);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                if (top_window == VWindow.InvalidWindow)
                    return string.Empty;
                else if (html)
                    return WindowUtility.Instance.GetIExplorerSelectedContent(top_window.Handle);
                else
                    return WindowUtility.Instance.GetIExplorerSelectedText(top_window.Handle);
            }
        }
        #endregion

        #region Network Methods
        /// <summary>
        /// Gets an array containing all wireless networks with a connection established.
        /// </summary>
        /// <returns>Array of wireless networks with a established connection.</returns>
        public ConnectedWirelessNetwork[] GetConnectedWirelessNetworks()
        {
            string ssid, bssid;
            int signalQuality, receptionRate, transmissionRate;
            WlanClient client = new WlanClient();
            List<ConnectedWirelessNetwork> networks = new List<ConnectedWirelessNetwork>();
            foreach (WlanClient.WlanInterface i in client.Interfaces)
            {
                Wlan.WlanConnectionAttributes connection = i.CurrentConnection;
                ssid = GetStringForSSID(connection.wlanAssociationAttributes.dot11Ssid);
                bssid = connection.wlanAssociationAttributes.Dot11Bssid.ToString();
                signalQuality = (int)connection.wlanAssociationAttributes.wlanSignalQuality;
                receptionRate = (int)connection.wlanAssociationAttributes.rxRate;
                transmissionRate = (int)connection.wlanAssociationAttributes.txRate;
                networks.Add(new ConnectedWirelessNetwork(ssid, bssid, signalQuality, receptionRate, transmissionRate));
            }
            return networks.ToArray();
        }

        /// <summary>
        /// Gets an array containing the Service Set Identifiers (SSIDs) of all wireless networks with a 
        /// connection established.
        /// </summary>
        /// <returns>Array of strings containing the SSIDs.</returns>
        public string[] GetConnectedWirelessSSIDs()
        {
            ConnectedWirelessNetwork[] networks = GetConnectedWirelessNetworks();
            List<string> ssids = new List<string>();
            foreach (ConnectedWirelessNetwork network in networks)
            {
                ssids.Add(network.SSID);
            }
            return ssids.ToArray();
        }

        /// <summary>
        /// Gets an array containing all available wireless networks.
        /// </summary>
        /// <returns>Array of wireless networks currently available.</returns>
        public WirelessNetwork[] GetAvailableWirelessNetworks()
        {
            string ssid;
            int signalQuality;
            WlanClient client = new WlanClient();
            List<WirelessNetwork> networks = new List<WirelessNetwork>();
            foreach (WlanClient.WlanInterface i in client.Interfaces)
            {
                Wlan.WlanAvailableNetwork[] available_networks = i.GetAvailableNetworkList(0);
                foreach (Wlan.WlanAvailableNetwork network in available_networks)
                {
                    ssid = GetStringForSSID(network.dot11Ssid);
                    signalQuality = (int)network.wlanSignalQuality;
                    networks.Add(new WirelessNetwork(ssid, signalQuality));
                }
            }
            return networks.ToArray();
        }

        /// <summary>
        /// Gets an array containing the Service Set Identifiers (SSIDs) of all available wireless networks.
        /// </summary>
        /// <returns>Array of strings containing the SSIDs.</returns>
        public string[] GetAvailableWirelessSSIDs()
        {
            WirelessNetwork[] networks = GetAvailableWirelessNetworks();
            List<string> ssids = new List<string>();
            foreach (WirelessNetwork network in networks)
            {
                ssids.Add(network.SSID);
            }
            return ssids.ToArray();
        }

        /// <summary>
        /// Gets a boolean indicating if there is or not a wireless connection
        /// established.
        /// </summary>
        /// <returns>True if there is a wireles connection established, false otherwise.</returns>
        public bool IsWirelessConnected()
        {
            if (GetConnectedWirelessNetworks().Length > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets a boolean indicating if there is or not, at least, a wireless connection
        /// available.
        /// </summary>
        /// <returns>True if there is a wireles connection available, false otherwise.</returns>
        public bool IsWirelessConnectionAvailable()
        {
            if (GetAvailableWirelessNetworks().Length > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the external Internet Protocol (IP) address of the current machine. The external IP address is 
        /// assigned by the user's Internet Service Provider.
        /// </summary>
        /// <returns>External IP address.</returns>
        /// <remarks>This method queries an external web side (<see href="http://www.whatismyip.com/automation/n09230945.asp">What Is My Ip Address?</see>) 
        /// for the ip. Therefore, the return of this method might be slow, according to the user's internet service quality or congestion.</remarks>
        public string GetExternalIpAddress()
        {
            string whatIsMyIp = "http://www.whatismyip.com/automation/n09230945.asp";
            WebClient wc = new WebClient();
            UTF8Encoding utf8 = new UTF8Encoding();
            string requestHtml = "";
            try
            {
                requestHtml = utf8.GetString(wc.DownloadData(whatIsMyIp));
            }
            catch (WebException)
            {

            }

            //IPAddress externalIp = IPAddress.Parse(requestHtml);
            //return externalIp;
            return requestHtml;
        }

        /// <summary>
        /// Gets the Internet Protocol (IP) address associated to the network adapter that currently 
        /// provides internet access.
        /// </summary>
        /// <returns></returns>
        public string GetInternalIpAddress()
        {
            NetworkAdapter adapter = GetInternetNetworkAdapter();
            return adapter.IpAddresses[0];
        }

        /// <summary>
        /// Gets the Internet Protocol (IP) addresses for the local computer.
        /// </summary>
        /// <returns></returns>
        public string[] GetInternalIpAdresses()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] addrs = Dns.GetHostAddresses(hostName);
            List<string> final_addrs = new List<string>();
            foreach (IPAddress addr in addrs)
                final_addrs.Add(addrs.ToString());
            return final_addrs.ToArray();
            //List<IPAddress> ips = new List<IPAddress>();
            //NetworkAdapter[] adapters = GetNetworkAdapters();
            //foreach (NetworkAdapter adapter in adapters)
            //{
            //    if (adapter.IpAddresses.Length > 0)
            //    {
            //        foreach (IPAddress ip in adapter.IpAddresses)
            //            ips.Add(ip);
            //    }
            //}
            //return ips.ToArray();
        }

        /// <summary>
        /// Gets an array containing all network adapters on the local computer.
        /// </summary>
        /// <returns>Array containing all network adapters on the local computer.</returns>
        /// <remarks>This method returns only network adapters that support, at least, the IPv4 protocol.</remarks>
        public NetworkAdapter[] GetNetworkAdapters()
        {
            List<NetworkAdapter> sni = new List<NetworkAdapter>();
            ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapter");
            ManagementClass mgmt2 = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objCol = mgmt.GetInstances();
            ManagementObjectCollection objCol2 = mgmt2.GetInstances();
            foreach (ManagementObject obj in objCol)
            {
                string mac; GetManagementObjectSafe<string>(obj, "MACAddress", out mac, string.Empty);
                string id; GetManagementObjectSafe<string>(obj, "DeviceID", out id, string.Empty);
                string name; GetManagementObjectSafe<string>(obj, "Name", out name, string.Empty);
                string description; GetManagementObjectSafe<string>(obj, "Description", out description, string.Empty);
                string system_name; GetManagementObjectSafe<string>(obj, "SystemName", out system_name, string.Empty);
                UInt32 index; GetManagementObjectSafe<UInt32>(obj, "Index", out index, 0);
                UInt32 interface_index; GetManagementObjectSafe<UInt32>(obj, "InterfaceIndex", out interface_index, 0);
                UInt64 speed; GetManagementObjectSafe<UInt64>(obj, "Speed", out speed, 0);

                foreach (ManagementObject obj2 in objCol2)
                {
                    if ((UInt32)obj2["Index"] == index)
                    {
                        string[] ips; GetManagementObjectSafe<string[]>(obj2, "IPAddress", out ips, new string[0]);
                        string[] gateways; GetManagementObjectSafe<string[]>(obj2, "DefaultIPGateway", out gateways, new string[0]);
                        bool dhcp_enabled; GetManagementObjectSafe<bool>(obj2, "DHCPEnabled", out dhcp_enabled, false);
                        string dhcp_lease_expires; GetManagementObjectSafe<string>(obj2, "DHCPLeaseExpires", out dhcp_lease_expires, string.Empty);
                        string dhcp_lease_obtained; GetManagementObjectSafe<string>(obj2, "DHCPLeaseObtained", out dhcp_lease_obtained, string.Empty);
                        string dhcp_server; GetManagementObjectSafe<string>(obj2, "DHCPServer", out dhcp_server, string.Empty);
                        string dns_domain; GetManagementObjectSafe<string>(obj2, "DNSDomain", out dns_domain, string.Empty);
                        string dns_hostname; GetManagementObjectSafe<string>(obj2, "DNSHostName", out dns_hostname, string.Empty);
                        string[] dns_server_search_order; GetManagementObjectSafe<string[]>(obj2, "DNSServerSearchOrder", out dns_server_search_order, new string[0]);
                        string service_name; GetManagementObjectSafe<string>(obj2, "ServiceName", out service_name, string.Empty);
                        NetworkAdapter adapter = new NetworkAdapter(id, name, description, speed, mac, ips, gateways, dhcp_enabled, dhcp_lease_expires, dhcp_lease_obtained,
                                                    dhcp_server, dns_domain, dns_hostname, dns_server_search_order, service_name, system_name, index, interface_index);
                        sni.Add(adapter);
                        break;
                    }
                }
            }
            //NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //foreach (NetworkInterface nic in nics)
            //{
            //    if (nic.Supports(NetworkInterfaceComponent.IPv4))
            //    {
            //        IPInterfaceProperties properties = nic.GetIPProperties();
            //        string mac = nic.GetPhysicalAddress().ToString();
            //        string id = nic.Id;
            //        string name = nic.Name;
            //        string description = nic.Description;
            //        long speed = nic.Speed;
            //        List<string> ips = new List<string>();
            //        foreach (IPAddressInformation ipinfo in properties.AnycastAddresses)
            //        {
            //            ips.Add(ipinfo.Address.ToString());
            //        }
            //        sni.Add(new NetworkAdapter(id, name, description, speed, mac, ips.ToArray()));
            //    }
            //}
            return sni.ToArray();
        }

        /// <summary>
        /// Gets the interface adapter that currently provides internet access.
        /// </summary>
        /// <returns>Network adapter that provides internet access. If no adapter was found, null is returned.</returns>
        /// <remarks>This method queries an external web side (<see href="http://www.whatismyip.com/">What Is My Ip Address?</see>) 
        /// to find the best suited interface. Therefore, the return of this method might be slow, according to the user's internet 
        /// service quality or congestion. Furthermore, as this method uses the iphlpapi.dll's <see href="http://msdn.microsoft.com/en-us/library/aa365920(VS.85).aspx">GetBestInterface()</see> function, 
        /// so wrong interfaces may be returned, in rare cases, if there is more than one interface that can reach the destination.</remarks>
        public NetworkAdapter GetInternetNetworkAdapter()
        {
            // Get the best interface index to reach whatismyip.com
            IPAddress[] addresses = Dns.GetHostAddresses("www.whatismyip.com");
            byte[] byteArray = addresses[0].GetAddressBytes();
            Array.Reverse(byteArray, 0, byteArray.Length);
            UInt32 ipaddr = BitConverter.ToUInt32(byteArray, 0);
            UInt32 interfaceIndex = 0;
            Win32.GetBestInterface(ipaddr, out interfaceIndex);
            // User the index to find the network interface
            ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapter");
            ManagementClass mgmt2 = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objCol = mgmt.GetInstances();
            ManagementObjectCollection objCol2 = mgmt2.GetInstances();
            foreach (ManagementObject obj in objCol)
            {

                UInt32 index; GetManagementObjectSafe<UInt32>(obj, "Index", out index, 0);
                UInt32 interface_index; GetManagementObjectSafe<UInt32>(obj, "InterfaceIndex", out interface_index, 0);
                if (interface_index == interfaceIndex)
                {
                    string mac; GetManagementObjectSafe<string>(obj, "MACAddress", out mac, string.Empty);
                    string id; GetManagementObjectSafe<string>(obj, "DeviceID", out id, string.Empty);
                    string name; GetManagementObjectSafe<string>(obj, "Name", out name, string.Empty);
                    string description; GetManagementObjectSafe<string>(obj, "Description", out description, string.Empty);
                    string system_name; GetManagementObjectSafe<string>(obj, "SystemName", out system_name, string.Empty);
                    UInt64 speed; GetManagementObjectSafe<UInt64>(obj, "Speed", out speed, 0);

                    foreach (ManagementObject obj2 in objCol2)
                    {
                        if ((UInt32)obj2["InterfaceIndex"] == interface_index)
                        {
                            string[] ips; GetManagementObjectSafe<string[]>(obj2, "IPAddress", out ips, new string[0]);
                            string[] gateways; GetManagementObjectSafe<string[]>(obj2, "DefaultIPGateway", out gateways, new string[0]);
                            bool dhcp_enabled; GetManagementObjectSafe<bool>(obj2, "DHCPEnabled", out dhcp_enabled, false);
                            string dhcp_lease_expires; GetManagementObjectSafe<string>(obj2, "DHCPLeaseExpires", out dhcp_lease_expires, string.Empty);
                            string dhcp_lease_obtained; GetManagementObjectSafe<string>(obj2, "DHCPLeaseObtained", out dhcp_lease_obtained, string.Empty);
                            string dhcp_server; GetManagementObjectSafe<string>(obj2, "DHCPServer", out dhcp_server, string.Empty);
                            string dns_domain; GetManagementObjectSafe<string>(obj2, "DNSDomain", out dns_domain, string.Empty);
                            string dns_hostname; GetManagementObjectSafe<string>(obj2, "DNSHostName", out dns_hostname, string.Empty);
                            string[] dns_server_search_order; GetManagementObjectSafe<string[]>(obj2, "DNSServerSearchOrder", out dns_server_search_order, new string[0]);
                            string service_name; GetManagementObjectSafe<string>(obj2, "ServiceName", out service_name, string.Empty);
                            return new NetworkAdapter(id, name, description, speed, mac, ips, gateways, dhcp_enabled, dhcp_lease_expires, dhcp_lease_obtained,
                                                        dhcp_server, dns_domain, dns_hostname, dns_server_search_order, service_name, system_name, index, interface_index);
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region Video Methods
        /// <summary>
        /// Gets all available video controllers in the system.
        /// </summary>
        /// <returns>Array containing all available video controllers.</returns>
        public VideoController[] GetVideoControllers()
        {
            List<VideoController> video_controllers = new List<VideoController>();
            ManagementClass mgmt = new ManagementClass("Win32_VideoController");
            ManagementObjectCollection objCol = mgmt.GetInstances();
            foreach (ManagementObject obj in objCol)
            {
                string caption; GetManagementObjectSafe<string>(obj, "Caption", out caption, string.Empty);
                string name; GetManagementObjectSafe<string>(obj, "Name", out name, string.Empty);
                string description; GetManagementObjectSafe<string>(obj, "Description", out description, string.Empty);
                string device_id; GetManagementObjectSafe<string>(obj, "DeviceID", out device_id, string.Empty);
                string system_name; GetManagementObjectSafe<string>(obj, "SystemName", out system_name, string.Empty);
                UInt32 memory_size; GetManagementObjectSafe<UInt32>(obj, "AdapterRAM", out memory_size, 0);
                string video_processor; GetManagementObjectSafe<string>(obj, "VideoProcessor", out video_processor, string.Empty);
                UInt32 bits_per_pixel; GetManagementObjectSafe<UInt32>(obj, "CurrentBitsPerPixel", out bits_per_pixel, 0);
                UInt32 horizontal_resolution; GetManagementObjectSafe<UInt32>(obj, "CurrentHorizontalResolution", out horizontal_resolution, 0);
                UInt32 vertical_resolution; GetManagementObjectSafe<UInt32>(obj, "CurrentVerticalResolution", out vertical_resolution, 0);
                UInt32 refresh_rate; GetManagementObjectSafe<UInt32>(obj, "CurrentRefreshRate", out refresh_rate, 0);
                string video_mode; GetManagementObjectSafe<string>(obj, "VideoMode", out video_mode, string.Empty);
                string driver_date; GetManagementObjectSafe<string>(obj, "DriverDate", out driver_date, string.Empty);
                string driver_name; GetManagementObjectSafe<string>(obj, "InstalledDisplayDrivers", out driver_name, string.Empty);
                string driver_version; GetManagementObjectSafe<string>(obj, "DriverVersion", out driver_version, string.Empty);
                VideoController vc = new VideoController(caption, name, description, device_id, system_name,
                                                            memory_size, video_processor, bits_per_pixel, horizontal_resolution,
                                                            vertical_resolution, refresh_rate, video_mode, driver_date, driver_name,
                                                            driver_version);
                video_controllers.Add(vc);
            }
            return video_controllers.ToArray();
        }

        /// <summary>
        /// Gets the current resolution of the primary display.
        /// </summary>
        /// <returns>Size specifying the resolution.</returns>
        public Size GetPrimaryDisplayResolution()
        {
            //VideoController[] vcs = GetVideoControllers();
            //List<string> modes = new List<string>();
            //foreach (VideoController vc in vcs)
            //    modes.Add(vc.VideoMode);
            //return modes.ToArray();
            return SystemInformation.PrimaryMonitorSize;
        }
        #endregion

        #region Sound Methods
        /// <summary>
        /// Gets the master volume level, in percentage (between 0.0f and 1.0f).
        /// </summary>
        /// <returns>Master volume level, in percentage (between 0.0f and 1.0f).</returns>
        public float GetMasterVolume()
        {
            if (Environment.OSVersion.Version.Major == 5)
            {
                VolumeControl.MixerInfo mi = VolumeControl.GetMixerControls();
                VolumeControl.VOLUME volume = VolumeControl.GetVolume(mi);
                return (float)((volume.left + volume.right) / 2) / (float)mi.maxVolume;
            }
            else
            {
                VistaVolumeControl.EndpointVolume ev = new VistaVolumeControl.EndpointVolume();
                float ret = ev.MasterVolume;
                ev.Dispose();
                return ev.MasterVolume;
            }

        }

        /// <summary>
        /// Sets the master volume level, in percentage (between 0.0f and 1.0f).
        /// </summary>
        /// <param name="vol">Master volume level, in percentage (between 0.0f and 1.0f).</param>
        public void SetMasterVolume(float vol)
        {

            if (Environment.OSVersion.Version.Major == 5)
            {
                VolumeControl.MixerInfo mi = VolumeControl.GetMixerControls();
                VolumeControl.VOLUME volume = new VolumeControl.VOLUME();
                volume.left = (int)(vol * mi.maxVolume);
                volume.right = (int)(vol * mi.maxVolume);
                VolumeControl.SetVolume(mi, volume);
            }
            else
            {
                VistaVolumeControl.EndpointVolume ev = new VistaVolumeControl.EndpointVolume();
                ev.MasterVolume = vol;
                ev.Dispose();
            }
        }

        /// <summary>
        /// Indicates if the master volue is muted.
        /// </summary>
        /// <returns>True if the master volume is muted, false otherwise.</returns>
        public bool IsMasterVolumeMuted()
        {
            if (Environment.OSVersion.Version.Major == 5)
            {
                VolumeControl.MixerInfo mi = VolumeControl.GetMixerControls();
                return VolumeControl.IsMuted(mi);
            }
            else
            {
                VistaVolumeControl.EndpointVolume ev = new VistaVolumeControl.EndpointVolume();
                bool ret = ev.Mute;
                ev.Dispose();
                return ev.Mute;
            }
        }

        /// <summary>
        /// Sets the master volume mute.
        /// </summary>
        /// <param name="mute">True to mute the master volume. False to unmute.</param>
        public void SetMute(bool mute)
        {
            if (Environment.OSVersion.Version.Major == 5)
            {
                VolumeControl.MixerInfo mi = VolumeControl.GetMixerControls();
                VolumeControl.SetMute(mi, mute);
            }
            else
            {
                VistaVolumeControl.EndpointVolume ev = new VistaVolumeControl.EndpointVolume();
                ev.Mute = mute;
                ev.Dispose();
            }
        }
        #endregion

        #region Disk Methods
        /// <summary>
        /// Gets all storage devices in the local computer.
        /// </summary>
        /// <returns>Array containing all storage devices.</returns>
        public StorageDevice[] GetStorageDevices()
        {
            List<StorageDevice> storage_devices = new List<StorageDevice>();
            ManagementClass mgmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection objCol = mgmt.GetInstances();
            foreach (ManagementObject obj in objCol)
            {
                string caption; GetManagementObjectSafe<string>(obj, "Caption", out caption, string.Empty);
                bool compressed; GetManagementObjectSafe<bool>(obj, "Compressed", out compressed, false);
                string descritpion; GetManagementObjectSafe<string>(obj, "Description", out descritpion, string.Empty);
                string device_id; GetManagementObjectSafe<string>(obj, "DeviceID", out device_id, string.Empty);
                string file_system; GetManagementObjectSafe<string>(obj, "FileSystem", out file_system, string.Empty);
                UInt64 free_space; GetManagementObjectSafe<UInt64>(obj, "FreeSpace", out free_space, 0);
                UInt64 size; GetManagementObjectSafe<UInt64>(obj, "Size", out size, 0);
                UInt64 block_size; GetManagementObjectSafe<UInt64>(obj, "BlockSize", out block_size, 0);
                UInt64 number_of_blocks; GetManagementObjectSafe<UInt64>(obj, "NumberOfBlocks", out number_of_blocks, 0);
                string name; GetManagementObjectSafe<string>(obj, "Name", out name, string.Empty);
                string provider_name; GetManagementObjectSafe<string>(obj, "ProviderName", out provider_name, string.Empty);
                string purpose; GetManagementObjectSafe<string>(obj, "Purpose", out purpose, string.Empty);
                string system_name; GetManagementObjectSafe<string>(obj, "SystemName", out system_name, string.Empty);
                string volume_name; GetManagementObjectSafe<string>(obj, "VolumeName", out volume_name, string.Empty);
                string volume_serial_number; GetManagementObjectSafe<string>(obj, "VolumeSerialNumber", out volume_serial_number, string.Empty);
                int type_raw; GetManagementObjectSafe<int>(obj, "DriveType", out type_raw, 0);
                DriveType type = (DriveType)type_raw;
                StorageDevice sd = new StorageDevice(caption, compressed, descritpion, device_id, file_system,
                                                        free_space, size, block_size, number_of_blocks, name,
                                                        provider_name, purpose, system_name, volume_name, volume_serial_number,
                                                        type);
                storage_devices.Add(sd);
            }
            return storage_devices.ToArray();
        }

        /// <summary>
        /// Gets the specified storage device.
        /// </summary>
        /// <param name="letter">Device ID specifying the storage device.</param>
        /// <returns>The specified storage device. Null if the specified device doesn't exist.</returns>
        public StorageDevice GetStorageDevice(string letter)
        {
            foreach (StorageDevice device in GetStorageDevices())
                if (device.Letter == letter)
                    return device;
            return null;
        }

        /// <summary>
        /// Indicates if the specified storage device exists in the local computer.
        /// </summary>
        /// <param name="letter">Device ID specifying the storage device.</param>
        /// <returns>True if the specified device exists, false otherwise.</returns>
        public bool ExistStorageDevice(string letter)
        {
            if (GetStorageDevice(letter) == null)
                return false;
            else
                return true;
        }
        #endregion

        #region Processor Methods
        /// <summary>
        /// Gets all processors installed in the system.
        /// </summary>
        /// <returns>Array containing all installed processors.</returns>
        public Processor[] GetProcessors()
        {
            List<Processor> processors = new List<Processor>();
            ManagementClass mgmt = new ManagementClass("Win32_Processor");
            ManagementObjectCollection objCol = mgmt.GetInstances();
            foreach (ManagementObject obj in objCol)
            {
                UInt16 address_width; GetManagementObjectSafe<UInt16>(obj, "AddressWidth", out address_width, 0);
                UInt16 data_width; GetManagementObjectSafe<UInt16>(obj, "DataWidth", out data_width, 0);
                string caption; GetManagementObjectSafe<string>(obj, "Caption", out caption, string.Empty);
                UInt32 clock_speed; GetManagementObjectSafe<UInt32>(obj, "CurrentClockSpeed", out clock_speed, 0);
                UInt16 voltage; GetManagementObjectSafe<UInt16>(obj, "CurrentVoltage", out voltage, 0);
                string description; GetManagementObjectSafe<string>(obj, "Description", out description, string.Empty);
                string device_id; GetManagementObjectSafe<string>(obj, "DeviceID", out device_id, string.Empty);
                UInt32 external_clock; GetManagementObjectSafe<UInt32>(obj, "ExtClock", out external_clock, 0);
                UInt32 l2_cache_size; GetManagementObjectSafe<UInt32>(obj, "L2CacheSize", out l2_cache_size, 0);
                UInt32 l3_cache_size; GetManagementObjectSafe<UInt32>(obj, "L3CacheSize", out l3_cache_size, 0);
                UInt16 load_percentage; GetManagementObjectSafe<UInt16>(obj, "LoadPercentage", out load_percentage, 0);
                UInt32 max_clock_speed; GetManagementObjectSafe<UInt32>(obj, "MaxClockSpeed", out max_clock_speed, 0);
                string manufacturer; GetManagementObjectSafe<string>(obj, "Manufacturer", out manufacturer, string.Empty);
                string name; GetManagementObjectSafe<string>(obj, "Name", out name, string.Empty);
                UInt32 number_of_cores; GetManagementObjectSafe<UInt32>(obj, "NumberOfCores", out number_of_cores, 0);
                UInt32 number_of_logical_processors; GetManagementObjectSafe<UInt32>(obj, "NumberOfLogicalProcessors", out number_of_logical_processors, 0);
                string processor_id; GetManagementObjectSafe<string>(obj, "ProcessorId", out processor_id, string.Empty);
                string role; GetManagementObjectSafe<string>(obj, "Role", out role, string.Empty);
                string socket; GetManagementObjectSafe<string>(obj, "SocketDesignation", out socket, string.Empty);
                string stepping; GetManagementObjectSafe<string>(obj, "Stepping", out stepping, string.Empty);
                string system_name; GetManagementObjectSafe<string>(obj, "SystemName", out system_name, string.Empty);
                string unique_id; GetManagementObjectSafe<string>(obj, "UniqueId", out unique_id, string.Empty);
                string version; GetManagementObjectSafe<string>(obj, "Version", out version, string.Empty);
                Processor processor = new Processor(address_width, data_width, caption, clock_speed, voltage, description, device_id,
                                                    external_clock, l2_cache_size, l3_cache_size,
                                                    load_percentage, max_clock_speed, manufacturer, name, number_of_cores, number_of_logical_processors,
                                                    processor_id, role, socket, stepping, system_name, unique_id, version);
                processors.Add(processor);
            }
            return processors.ToArray();
        }

        /// <summary>
        /// Gets the current CPU usage, in percentage.
        /// </summary>
        /// <returns>Percentage of CPU used.</returns>
        public float GetCpuUsage()
        {
            PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            return cpu.NextValue();
        }

        /// <summary>
        /// Indicates if the installed processor(s) is 64-bit.
        /// </summary>
        /// <returns>True if the processor(s) is 64-bit, false otherwise.</returns>
        public bool Is64BitProcessor()
        {
            Processor[] processors = GetProcessors();
            if (processors.Length > 0 && processors[0].DataWidth == 64)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Indicates if the installed operative system is 64-bit.
        /// </summary>
        /// <returns>True if the operative system is 64-bits, false otherwise.</returns>
        public bool Is64BitOperativeSystem()
        {
            Processor[] processors = GetProcessors();
            if (processors.Length > 0 && processors[0].AddressWidth == 64)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the current processor speed, in Mhz.
        /// </summary>
        /// <returns>Current processor speed, in Mhz</returns>
        public int GetProcessorSpeed()
        {
            Processor[] processors = GetProcessors();
            if (processors.Length > 0)
                return (int)processors[0].ClockSpeed;
            else
                return -1;
        }

        /// <summary>
        /// Gets the maximum processor speed, in Mhz.
        /// </summary>
        /// <returns>Maximum processor speed, in Mhz</returns>
        public int GetProcessorMaxSpeed()
        {
            Processor[] processors = GetProcessors();
            if (processors.Length > 0)
                return (int)processors[0].MaxClockSpeed;
            else
                return -1;
        }
        #endregion

        #region RAM Methods
        /// <summary>
        /// Gets the current amout of available memory, in kilobytes.
        /// </summary>
        /// <returns>Amout of available memory, in kilobytes.</returns>
        public double GetAvailableMemory()
        {
            PerformanceCounter ram = new PerformanceCounter("Memory", "Available KBytes", true);
            return (double)ram.NextValue();
        }

        /// <summary>
        /// Gets the current amout of used memory, in kilobytes.
        /// </summary>
        /// <returns>Amout of used memory, in kilobytes.</returns>
        public double GetUsedMemory()
        {
            return GetMemorySize() - GetAvailableMemory();
        }

        /// <summary>
        /// Gets the current percentage of used memory.
        /// </summary>
        /// <returns>Percentage of used memory.</returns>
        public double GetMemoryUsage()
        {
            return GetUsedMemory() / GetMemorySize() * 100.0;
        }

        /// <summary>
        /// Gets the total amout of RAM, in kilobytes.
        /// </summary>
        /// <returns>Total amout of RAM, in kilobytes.</returns>
        public double GetMemorySize()
        {
            double mem = GetMemoryEndingAddress();
            return mem;
        }

        private double GetMemoryEndingAddress()
        {
            ManagementClass mgmt = new ManagementClass("Win32_MemoryArray");
            ManagementObjectCollection objCol = mgmt.GetInstances();
            double max_end_addr = -1;
            foreach (ManagementObject obj in objCol)
            {
                MessageBox.Show(((Double)obj["EndingAddress"]).ToString());
                max_end_addr = Double.Parse(((UInt64)obj["EndingAddress"]).ToString());
            }
            return max_end_addr;
        }
        #endregion

        #region Power Methods
        /// <summary>
        /// Get current system power status.
        /// </summary>
        /// <returns>Current system power status.</returns>
        public PowerStatus GetPowerStatus()
        {
            return SystemInformation.PowerStatus;
        }

        /// <summary>
        /// Gets the battery full life time, in seconds.
        /// </summary>
        /// <returns>Battery full life time.</returns>
        public int GetBatteryFullLifeTime()
        {
            return SystemInformation.PowerStatus.BatteryFullLifetime;
        }

        /// <summary>
        /// Gets an approximate percentage of the battery time remaining.
        /// </summary>
        /// <returns>Percentage of battery time remaining.</returns>
        public float GetBatteryLifePercent()
        {
            return SystemInformation.PowerStatus.BatteryLifePercent;
        }

        /// <summary>
        /// Gets the number of seconds of battery time remaining.
        /// </summary>
        /// <returns>Number of seconds of battery time remaining.</returns>
        public int GetBatteryLifeRemaining()
        {
            return SystemInformation.PowerStatus.BatteryLifeRemaining;
        }

        /// <summary>
        /// Indicates if the current system has a battery installed.
        /// </summary>
        /// <returns>True if the a battery is present, false otherwise.</returns>
        public bool IsBatteryPresent()
        {
            return SystemInformation.PowerStatus.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery;
        }

        /// <summary>
        /// Indicates if the current system is running on battery power.
        /// </summary>
        /// <returns>True if running on battery power, false if system is plugged into an 
        /// AC socket.</returns>
        public bool IsRunningOnBatteryPower()
        {
            return SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline;
        }
        #endregion

        #region Identification Methods
        /// <summary>
        /// Gets the user name of the person that is currently logged in.
        /// </summary>
        /// <returns>User name of the currently logged in person.</returns>
        public string GetCurrentUser()
        {
            return Environment.UserName;
        }

        /// <summary>
        /// Gets the NetBIOS computer name of the local computer.
        /// </summary>
        /// <returns>Computer name.</returns>
        public string GetComputerName()
        {
            return SystemInformation.ComputerName;
        }

        /// <summary>
        /// Gets the network domain name associated with the current user.
        /// </summary>
        /// <returns>Network domain name associated with the current user.</returns>
        public string GetNetWorkgroupName()
        {
            return Environment.UserDomainName;
        }
        #endregion

        #region Time Methods
        /// <summary>
        /// Gets the current date and time on this computer, expressed as the local time.
        /// </summary>
        /// <returns>Current date and time.</returns>
        /// <remarks>This method is just an alias for the DateTime.Now property.</remarks>
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Gets the current date and time on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <returns>Current date and time, in UTC.</returns>
        /// <remarks>This method is just an alias for the DateTime.UtcNow property.</remarks>
        public DateTime GetCurrentTimeUtc()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the amount of time elapsed since the system started.
        /// </summary>
        /// <returns>Amount of time elapsed since the system started.</returns>
        public DateTime GetUpTime()
        {
            return new DateTime(Environment.TickCount);
        }
        #endregion

        #region String Tools
        /// <summary>
        /// Inserts text in the current selected window, by the user.
        /// </summary>
        /// <param name="text">Text string to be inserted.</param>
        /// <param name="pf">Boolean indicating if this method should only be executed once <see cref="PerformPostFocusOperations"/> is also executed.</param>
        public void InsertText(string text, bool pf)
        {
            string new_text = FixBraces(text);
            if (pf)
            {
                _post_focus_operations.Add(new PostFocusDelegate(delegate()
                    {
                        //SendKeys.SendWait(new_text);
                        WshShellClass wsh = new WshShellClass();
                        object wait = false;
                        wsh.SendKeys(new_text, ref wait);
                    }));

                //_post_focus_operations.Add(new PostFocusDelegate(delegate()
                //{
                //    // backup clipboard
                //    MultiLevelData clipboard_backup = new MultiLevelData();
                //    clipboard_backup.PopulateFromClipboard();
                //    // copy new content to clipboard
                //    MultiLevelData new_data = new MultiLevelData();
                //    new_data.Text = text;
                //    // sent it to the window
                //    new_data.RestoreToClipboard();
                //    SendKeys.SendWait("^v");
                //    // restore clipboard
                //    clipboard_backup.RestoreToClipboard();
                //}));
            }
            else
            {
                SendKeys.SendWait(new_text);
            }
        }

        public void PasteText(string text, bool pf)
        {
            if (pf)
            {
                _post_focus_operations.Add(new PostFocusDelegate(delegate()
                {
                    MultiLevelData clipboard_backup = new MultiLevelData();
                    clipboard_backup.PopulateFromClipboard();
                    Clipboard.SetText(text);
                    SendKeys.SendWait("+{INSERT}");
                    clipboard_backup.RestoreToClipboard();
                }));
            }
            else
            {
                MultiLevelData clipboard_backup = new MultiLevelData();
                clipboard_backup.PopulateFromClipboard();
                Clipboard.SetText(text);
                SendKeys.SendWait("+{INSERT}");
                clipboard_backup.RestoreToClipboard();
            }
        }

        // fix brace characters
        private string FixBraces(string txt)
        {
            string new_txt = string.Empty;
            foreach (char c in txt)
            {
                if (c == '{')
                    new_txt += "{{}";
                else if (c == '}')
                    new_txt += "{}}";
                else
                    new_txt += c;
            }
            return new_txt.Replace("+", "{+}")
                            .Replace("^", "{^}")
                            .Replace("%", "{%}")
                            .Replace("~", "{~}")
                            .Replace("(", "{(}")
                            .Replace(")", "{)}")
                            .Replace("[", "{[}")
                            .Replace("]", "{]}")
                            .Replace(Environment.NewLine, "{ENTER}");
        }

        /// <summary>
        /// Transformes the selected text by the user to lowercase.
        /// </summary>
        /// <param name="pf">Boolean indicating if this method should only be executed once <see cref="PerformPostFocusOperations"/> is also executed.</param>
        public void SelectedTextToLower(bool pf)
        {
            MultiLevelData data = GetSelectedContent();
            if (data.Text != null)
                InsertText(data.Text.ToLower(), pf);
        }

        /// <summary>
        /// Transformes the selected text by the user to uppercase.
        /// </summary>
        /// <param name="pf">Boolean indicating if this method should only be executed once <see cref="PerformPostFocusOperations"/> is also executed.</param>
        public void SelectedTextToUpper(bool pf)
        {
            MultiLevelData data = GetSelectedContent();
            if (data.Text != null)
                InsertText(data.Text.ToUpper(), pf);
        }

        /// <summary>
        /// Sorts the selected text by the user. The sorting is done alphabetically, regarding each text line.
        /// </summary>
        /// <param name="pf">Boolean indicating if this method should only be executed once <see cref="PerformPostFocusOperations"/> is also executed.</param>
        public void SelectedTextSortLines(bool pf)
        {
            MultiLevelData data = GetSelectedContent();
            if (data.Text != null)
            {
                string[] tokens = data.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                Array.Sort<string>(tokens);
                string new_text = string.Empty;
                foreach (string token in tokens)
                {
                    new_text += token + Environment.NewLine;
                }
                //if (data.Text[data.Text.Length - 1] != Environment.NewLine)
                //    new_text.Remove(new_text.Length - 1);
                InsertText(new_text, pf);
            }
        }
        #endregion

        #region Macro Recording
        public void StartMacroRecording()
        {
            _observer.StartMacroRecording();
        }

        public void StopMacroRecording(string folder)
        {
            StopMacroRecording(folder, 0, 0);
        }

        public void StopMacroRecording(string folder, int iskip, int fskip)
        {
            if (_observer.IsRecording)
            {
                _observer.StopMacroRecording();
                _observer.SaveScript(folder, iskip, fskip);
            }
        }
        #endregion

        #region Automation

        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// Converts a 802.11 SSID to a string.
        /// </summary>
        private string GetStringForSSID(byte[] ssid)
        {
            return new string(Encoding.ASCII.GetChars(ssid));
        }

        /// <summary>
        /// Converts a 802.11 SSID to a string.
        /// </summary>
        private string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }

        private void GetManagementObjectSafe<T>(ManagementObject obj, string property, out T dest, T defaults)
        {
            dest = defaults;
            try
            {
                dest = (T)obj[property];
            }
            catch
            {

            }
        }
        #endregion
    }
}
