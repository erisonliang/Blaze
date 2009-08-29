using System;
using System.Collections.Generic;
using System.Text;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction.WindowManagement;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using System.Windows.Automation.Text;
using System.IO;
using mshtml;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SystemCore.SystemAbstraction.WindowManagement
{
    public sealed class WindowUtility
    {
        #region Properties
        private static volatile WindowUtility _instance;
        private static object _sync = new Object();
        private VWndProfile _wndProfile;
        private static List<string> _known_apps = new List<string>(new string[] { "Notepad", "WordPadClass", "CabinetWClass", "IEFrame" });
        private static Regex _web_regex = new Regex(@"^((ftp|http|https|gopher|mailto|news|nntp|telnet|wais|file|prospero|aim|webcal)\://)?(www.|[a-zA-Z0-9].)[a-zA-Z0-9\-\.]+\..*$");
        #endregion

        #region Accessors
        public static WindowUtility Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_sync)
                    {
                        if (_instance == null)
                            _instance = new WindowUtility();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Constructors
        private WindowUtility()
        {
            _wndProfile = new VWndProfile();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Retrieves the URL or PATH of the address bar from the specified window.
        /// </summary>
        /// <param name="wnd">Window fom which to retrieve the url or path.</param>
        /// <returns>String containing the URL or PATH.</returns>
        //public string GetUrl(VWindow wnd)
        //{
        //    AutomationElement root = AutomationElement.FromHandle(wnd.Handle);
        //    string className = root.Current.ClassName;
        //    if (_known_apps.Contains(className))
        //    {
        //        if (className == _known_apps[0]) // Notepad
        //            return string.Empty;
        //        else if (className == _known_apps[1]) // Wordpad
        //            return string.Empty;
        //        else if (className == _known_apps[2]) // Windows Explorer
        //            return GetWExplorerUrl(wnd.Handle);
        //        else if (className == _known_apps[3]) // Internet Explorer
        //            return GetIExplorerUrl(wnd.Handle);
        //        else
        //            return string.Empty;
        //    }
        //    return string.Empty;
        //}

        /// <summary>
        /// Retrieves the selected text by the user from the specified window.
        /// </summary>
        /// <param name="wnd">Window from which to retrieve the selected text.</param>
        /// <returns>String containing user selected text.</returns>
        //public string GetSelectedText(VWindow wnd)
        //{
        //    AutomationElement root = AutomationElement.FromHandle(wnd.Handle);
        //    string className = root.Current.ClassName;
        //    if (_known_apps.Contains(className))
        //    {
        //        if (className == _known_apps[0]) // Notepad
        //            return GetNotepadSelectedText(root);
        //        else if (className == _known_apps[1]) // Wordpad
        //            return GetWordpadSelectedText(root);
        //        else if (className == _known_apps[2]) // Windows Explorer
        //            return GetWExplorerSelectedText(wnd.Handle);
        //        else if (className == _known_apps[3]) // Internet Explorer
        //            return GetIExplorerSelectedText(wnd.Handle);
        //        else
        //            return string.Empty;
        //    }
        //    return string.Empty;
        //}

        /// <summary>
        /// Retrieves the text contained in the specified window.
        /// </summary>
        /// <param name="wnd">Window from which to retrieve the text.</param>
        /// <returns>String containing the text.</returns>
        //public string GetText(VWindow wnd)
        //{
        //    AutomationElement root = AutomationElement.FromHandle(wnd.Handle);
        //    string className = root.Current.ClassName;
        //    if (_known_apps.Contains(className))
        //    {
        //        if (className == _known_apps[0]) // Notepad
        //            return GetNotepadText(root);
        //        else if (className == _known_apps[1]) // Wordpad
        //            return GetWordpadText(root);
        //        else if (className == _known_apps[2]) // Windows Explorer
        //            return GetWExplorerText(wnd.Handle);
        //        else if (className == _known_apps[3]) // Internet Explorer
        //            return GetIExplorerText(wnd.Handle);
        //        else
        //            return string.Empty;
        //    }
        //    return string.Empty;
        //}

        /// <summary>
        /// Retrieves the selected content by the user from the specified window.
        /// </summary>
        /// <param name="wnd">Window from which to retrieve the selected content.</param>
        /// <returns>String array containing user selected content.</returns>
        //public string[] GetSelectedContent(VWindow wnd)
        //{
        //    AutomationElement root = AutomationElement.FromHandle(wnd.Handle);
        //    string className = root.Current.ClassName;
        //    if (_known_apps.Contains(className))
        //    {
        //        if (className == _known_apps[0]) // Notepad
        //            return new string[0];
        //        else if (className == _known_apps[1]) // Wordpad
        //            return new string[0];
        //        else if (className == _known_apps[2]) // Windows Explorer
        //            return GetWExplorerSelectedContent(wnd.Handle);
        //        else if (className == _known_apps[3]) // Internet Explorer
        //            return GetIExplorerSelectedContent(wnd.Handle);
        //        else
        //            return new string[0];
        //    }
        //    return new string[0];
        //}

        /// <summary>
        /// Retrieves the content from the specified window.
        /// </summary>
        /// <param name="wnd">Window from which to retrieve the content.</param>
        /// <returns>String array containing the content.</returns>
        //public string[] GetContent(VWindow wnd)
        //{
        //    AutomationElement root = AutomationElement.FromHandle(wnd.Handle);
        //    string className = root.Current.ClassName;
        //    if (_known_apps.Contains(className))
        //    {
        //        if (className == _known_apps[0]) // Notepad
        //            return new string[0];
        //        else if (className == _known_apps[1]) // Wordpad
        //            return new string[0];
        //        else if (className == _known_apps[2]) // Windows Explorer
        //            return GetWExplorerContent(wnd.Handle);
        //        else if (className == _known_apps[3]) // Internet Explorer
        //            return GetIExplorerContent(wnd.Handle);
        //        else
        //            return new string[0];
        //    }
        //    return new string[0];
        //}

        /// <summary>
        /// Retrieve the text of the specified window's title bar (if it has one).
        /// </summary>
        /// <param name="hWnd">Handle to the window or control containing the text.</param>
        /// <returns>String containing window title.</returns>
        //public string GetWindowTitle(VWindow wnd)
        //{
        //    IntPtr hWnd = wnd.Handle;
        //    int length = Win32.GetWindowTextLength(hWnd);
        //    StringBuilder sb = new StringBuilder(length + 1);
        //    Win32.GetWindowText(hWnd, sb, sb.Capacity);
        //    return sb.ToString();
        //}

        /// <summary>
        /// Changes the text of the specified window's title bar (if it has one) or control.
        /// </summary>
        /// <param name="hWnd">Handle to the window or control whose text is to be changed.</param>
        /// <param name="text">String to be used as the new title or control text.</param>
        //public void SetWindowTitle(VWindow wnd, string text)
        //{
        //    IntPtr hWnd = wnd.Handle;
        //    Win32.SetWindowText(hWnd, text);
        //}

        public string GetTextFromTopWindow()
        {
            //// code needed to get selected text - returns empty string if nothing selected
            //IntPtr hWnd = Win32.GetForegroundWindow();

            //uint processId;
            //uint activeThreadId = Win32.GetWindowThreadProcessId(hWnd, out processId);
            //uint currentThreadId = Win32.GetCurrentThreadId();

            //Win32.AttachThreadInput(activeThreadId, currentThreadId, true);
            //IntPtr focusedHandle = Win32.GetFocus();
            //Win32.AttachThreadInput(activeThreadId, currentThreadId, false);

            //int len = Win32.SendMessage(focusedHandle, Win32.WM_GETTEXTLENGTH, 0, null);
            //StringBuilder sb = new StringBuilder(len);
            //int numChars = Win32.SendMessage(focusedHandle, Win32.WM_GETTEXT, len + 1, sb);
            //int start, next;
            //Win32.SendMessage(focusedHandle, Win32.EM_GETSEL, out start, out next);

            //string selectedText = sb.ToString().Substring(start, next - start);

            //SendCtrlC();
            //SendKeys.SendWait("^(C)");
            //SendCtrlChar('c');
            //IntPtr ptr = Win32.GetForegroundWindow();
            //IntPtr ptr = GetEditControl(GetTopWindow().Handle);
            //IntPtr ptr = GetTopWindow().Handle;
            //Win32.PostMessage(ptr, Win32.WM_KEYDOWN, new IntPtr(Win32.VkKeyScan('c')), IntPtr.Zero);
            //Win32.PostMessage(ptr, Win32.WM_KEYUP, new IntPtr(Win32.VkKeyScan('c')), IntPtr.Zero);
            //Win32.keybd_event((byte)VirtualKey.C, (byte)Win32.MapVirtualKey((uint)VirtualKey.C, 0), Win32.KEYEVENTF_EXTENDEDKEY, 0);
            // backup clipboard
            IDataObject clipboard_backup = new DataObject();
            clipboard_backup.SetData(DataFormats.Text, Clipboard.GetText());
            clipboard_backup.SetData(DataFormats.Bitmap, Clipboard.GetImage());
            clipboard_backup.SetData(DataFormats.FileDrop, Clipboard.GetFileDropList());
            clipboard_backup.SetData(DataFormats.WaveAudio, Clipboard.GetAudioStream());
            // copy selected content into the clipboard
            SendKeys.SendWait("^c");
            IDataObject clipboard = new DataObject();
            clipboard.SetData(DataFormats.Text, Clipboard.GetText());
            clipboard.SetData(DataFormats.Bitmap, Clipboard.GetImage());
            clipboard.SetData(DataFormats.FileDrop, Clipboard.GetFileDropList());
            clipboard.SetData(DataFormats.WaveAudio, Clipboard.GetAudioStream());
            // restore clipboard
            Clipboard.SetDataObject(clipboard_backup, true);
            // retrieve data
            string selectedText = (string)clipboard.GetData(DataFormats.Text);

            return selectedText;
        }

        public string GetTextFromTopWindowEx()
        {
            IntPtr hWnd = Win32.GetForegroundWindow();

            uint processId;

            uint activeThreadId = Win32.GetWindowThreadProcessId(hWnd, out processId);

            uint currentThreadId = Win32.GetCurrentThreadId();

            Win32.AttachThreadInput(activeThreadId, currentThreadId, true);

            IntPtr focusedHandle = Win32.GetFocus();

            Win32.AttachThreadInput(activeThreadId, currentThreadId, false);

            int len = Win32.SendMessage(focusedHandle, Win32.WM_GETTEXTLENGTH, 0, null);

            StringBuilder sb = new StringBuilder(len);

            int numChars = Win32.SendMessage(focusedHandle, Win32.WM_GETTEXT, len + 1, sb);

            int start, next;

            Win32.SendMessage(focusedHandle, Win32.EM_GETSEL, out start, out next);

            string selectedText = sb.ToString().Substring(start, next - start);

            return selectedText;
        }

        //public string Wtf()
        //{
        //    IntPtr ptr = GetEditControl(GetTopWindow().Handle);
        //    int len = Win32.SendMessage(ptr, Win32.WM_GETTEXTLENGTH, 0, null);
        //    StringBuilder sb = new StringBuilder(len);
        //    int numChars = Win32.SendMessage(ptr, Win32.WM_GETTEXT, len + 1, sb);
        //    return sb.ToString();

        //}

        //public IntPtr GetEditControl(IntPtr hWnd)
        //{
        //    //string lpszClass = "Edit";
        //    //IntPtr edit = Win32.FindWindowEx(hWnd, IntPtr.Zero, lpszClass, IntPtr.Zero);
        //    //return edit;
        //    MessageBox.Show(GetWindowTitle(hWnd));
        //    Condition condition = new AndCondition(
        //                            new PropertyCondition(AutomationElement.ClassNameProperty, "Edit"),
        //                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
        //    AutomationElement root = AutomationElement.FromHandle(hWnd);
        //    AutomationElement edit = root.FindFirst(TreeScope.Subtree, condition);

        //    return new IntPtr(edit.Current.NativeWindowHandle);
        //}

        //public void SendCtrlCharToWindow(IntPtr hWnd, char c)
        //{
        //    Win32.keybd_event((byte)VirtualKey.CONTROL, (byte)Win32.MapVirtualKey((uint)VirtualKey.CONTROL, 0), Win32.KEYEVENTF_EXTENDEDKEY, 0);
        //    Win32.PostMessage(hWnd, Win32.WM_KEYDOWN, new IntPtr(Win32.VkKeyScan(c)), IntPtr.Zero);
        //    Win32.PostMessage(hWnd, Win32.WM_KEYUP, new IntPtr(Win32.VkKeyScan(c)), IntPtr.Zero);
        //    Win32.keybd_event((byte)VirtualKey.CONTROL, (byte)Win32.MapVirtualKey((uint)VirtualKey.CONTROL, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
        //}

        //public void SendCtrlC()
        //{
        //    Win32.keybd_event(0x11, 0x9d, Win32.KEYEVENTF_EXTENDEDKEY, 0);// press ctrl
        //    Win32.keybd_event(0x43, 0xae, Win32.KEYEVENTF_EXTENDEDKEY, 0); // press c        
        //    Win32.keybd_event(0x43, 0xae, Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0); //release c
        //    Win32.keybd_event(0x11, 0x9d, Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0); //release ctrl
        //}

        //public IntPtr GetFocus(IntPtr hWnd)
        //{
        //    //uint processId;
        //    //uint activeThreadId = Win32.GetWindowThreadProcessId(hWnd, out processId);
        //    //uint currentThreadId = Win32.GetCurrentThreadId();

        //    //GUITHREADINFO gInfo = new GUITHREADINFO();
        //    //gInfo.cbSize = (uint)Marshal.SizeOf(gInfo);
        //    //bool test = Win32.GetGUIThreadInfo(activeThreadId, out gInfo);

        //    //if (test == false)
        //    //    MessageBox.Show("NOES");

        //    //string lpszClass = "Edit";
        //    //IntPtr edit = Win32.FindWindowEx(hWnd, IntPtr.Zero, lpszClass, IntPtr.Zero);

        //    //MessageBox.Show(GetWindowText(hWnd)+ " " + edit.ToString() + " " + processId);

        //    //IntPtr focusedHandle = gInfo.hwndFocus;
        //    //Win32.AttachThreadInput(activeThreadId, currentThreadId, true);
        //    //IntPtr focusedHandle = Win32.GetFocus();
        //    //Win32.AttachThreadInput(activeThreadId, currentThreadId, false);
        //    IntPtr currWindow = Win32.GetForegroundWindow();
        //    Win32.SetForegroundWindow(hWnd);
        //    IntPtr focusedHandle = GetFocus();
        //    Win32.SetForegroundWindow(currWindow);
            
        //    return focusedHandle;
        //}

        public uint GetThreadId(IntPtr hWnd)
        {
            uint pid;
            return Win32.GetWindowThreadProcessId(hWnd, out pid);
        }

        public uint GetProcessId(IntPtr hWnd)
        {
            uint pid;
            Win32.GetWindowThreadProcessId(hWnd, out pid);
            return pid;
        }

        public string GetProcessName(IntPtr hWnd)
        {
            uint pid = GetProcessId(hWnd);
            Process proc = Process.GetProcessById((int)pid);
            return proc.ProcessName;
        }

        public bool IsWindowOpen(IntPtr hWnd)
        {
            if (GetProcessId(hWnd) == 0)
                return false;
            else
                return true;
        }

        public string GetClassName(IntPtr hWnd)
        {
            //AutomationElement root = AutomationElement.FromHandle(hWnd);
            //string className = root.Current.ClassName;
            //return className;
            int output;
            StringBuilder class_name = new StringBuilder(64);
            //Get the window class name
            output = Win32.GetClassName(hWnd, class_name, class_name.Capacity);
            if (output != 0)
            {
                return class_name.ToString();
            }
            else
            {
                return "No class found";
            }
        }

        public bool IsWindowOnFront(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return false;
            IntPtr top_hWnd = Win32.GetForegroundWindow();
            if (hWnd == top_hWnd)
                return true;
            else
                return false;
        }

        public bool BringWindowToFront(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return false;
            else
                return Win32.SetForegroundWindow(hWnd);
        }

        public bool BringWindowToFrontIfNeeded(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return false;
            IntPtr top_hWnd = Win32.GetForegroundWindow();
            if (hWnd == top_hWnd)
                return true;
            return Win32.SetForegroundWindow(hWnd);
        }

        public bool BringWindowToFrontIfNeeded(string className, string title)
        {
            IntPtr top_hWnd = Win32.GetForegroundWindow();
            IntPtr hWnd = Win32.FindWindow(className, title);
                
            if (hWnd == IntPtr.Zero)
                return false;
            else if (hWnd == top_hWnd)
                return true;
            else
                return Win32.SetForegroundWindow(hWnd);
        }

        public bool VistaSetForegroundWindow(IntPtr hWnd)
        {
            Win32.SetWindowPos(hWnd,(IntPtr)Win32.HWND_TOPMOST,0,0,0,0,Win32.SWP_NOMOVE | Win32.SWP_NOSIZE);
            System.Threading.Thread.Sleep(10);
            Win32.SetWindowPos(hWnd, (IntPtr)Win32.HWND_NOTOPMOST, 0, 0, 0, 0, Win32.SWP_SHOWWINDOW | Win32.SWP_NOMOVE | Win32.SWP_NOSIZE);
            System.Threading.Thread.Sleep(10);
            return Win32.GetForegroundWindow() == hWnd;
        }

        public string GetWindowTitle(IntPtr hWnd)
        {
            int length = Win32.GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            Win32.GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        public void SetWindowTitle(IntPtr hWnd, string text)
        {
            Win32.SetWindowText(hWnd, text);
        }

        public System.Drawing.Rectangle GetWindowRect(IntPtr hWnd)
        {
            RECT rect = new RECT();
            if (Win32.GetWindowRect(hWnd, out rect))
            {
                return new System.Drawing.Rectangle((int)rect.Left, (int)rect.Top, (int)(rect.Right - rect.Left), (int)(rect.Bottom - rect.Top));
            }
            else
            {
                return System.Drawing.Rectangle.Empty;
            }
        }

        public bool IsWindowsExplorerBrowsingWindow(IntPtr hWnd)
        {
            string process_name = GetProcessName(hWnd);
            string class_name = GetClassName(hWnd);
            if (process_name == "explorer" && (class_name == "CabinetWClass" || class_name == "ExploreWClass" || class_name == "Progman"))
                return true;
            else
                return false;
        }

        public void SendString(IntPtr hWnd, string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Win32.PostMessage(hWnd, Win32.WM_KEYDOWN, new IntPtr(Win32.VkKeyScan(s[i])), IntPtr.Zero);
                Win32.PostMessage(hWnd, Win32.WM_KEYUP, new IntPtr(Win32.VkKeyScan(s[i])), IntPtr.Zero);
            }
        }

        public void PressKey(Keys key)
        {
            Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | 0, 0);
        }

        public void ReleaseKey(Keys key)
        {
            Win32.keybd_event((byte)key, (byte)Win32.MapVirtualKey((uint)key, 0), Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// Gets the top visible window to the user.
        /// </summary>
        /// <returns>Top most visible window to the user.</returns>
        public VWindow GetTopWindow()
        {
            //IntPtr hWnd = Win32.GetForegroundWindow();
            //string windowText = GetWindowText(hWnd);
            //if (windowText == CommonInfo.AppName)
            //{
            //    hWnd = Win32.GetWindow(hWnd, (uint)GetWindow_Cmd.GW_HWNDFIRST);
            //    //hWnd = Win32.GetDesktopWindow();
            //    hWnd = GetNextWindow(hWnd, 0);
            //}
            //return hWnd;
            if (_wndProfile.Windows.Count > 0)
                return _wndProfile.Windows[0];
            else
                return VWindow.InvalidWindow;
        }

        /// <summary>
        /// Gets the top visible window to the user, of the according to a specified class.
        /// </summary>
        /// <param name="className">Name of te window's class.</param>
        /// <returns>Top most visible window to the user, of the specified class.</returns>
        public VWindow GetTopWindow(string processName)
        {
            if (_wndProfile.Windows.Count > 0)
            {
                for (int i = 0; i < _wndProfile.Count; i++)
                {
                    if (_wndProfile.Windows[i].ProcessName.ToLower() == processName.ToLower())
                        return _wndProfile.Windows[i];
                }
                return VWindow.InvalidWindow;
            }
            else
                return VWindow.InvalidWindow;
        }

        public VWindow GetFocusWindow()
        {
            if (_wndProfile.Windows.Count > 0)
            {
                IntPtr top = _wndProfile.Windows[0].Handle;
                if (top == Win32.GetFocus())
                    return _wndProfile.Windows[0];
                else
                    return _wndProfile.Windows[_wndProfile.Count - 1];
            }
            else
                return VWindow.InvalidWindow;
        }

        /// <summary>
        /// Gets the next window with bigger z-order priority, when compared to the specified window.
        /// </summary>
        /// <param name="wnd">Visible window to be compared.</param>
        /// <returns>Next visible window with bigger z-order priority.</returns>
        public VWindow GetPreviousWindow(VWindow wnd)
        {
            //return GetPreviousWindow(hWnd, 1);
            if (_wndProfile.Windows.Contains(wnd))
            {
                if (wnd.Zorder > 0)
                {
                    return _wndProfile.Windows[wnd.Zorder - 1];
                }
                else
                    return _wndProfile.Windows[0];
            }
            else
                return VWindow.InvalidWindow;
        }

        /// <summary>
        /// Gets the next window with smaller z-order priority, when compared to the specified window.
        /// </summary>
        /// <param name="wnd">Visible window to be compared.</param>
        /// <returns>Next visible window with smaller z-order priority.</returns>
        public VWindow GetNextWindow(VWindow wnd)
        {
            //return GetNextWindow(hWnd, 1);
            if (_wndProfile.Windows.Contains(wnd))
            {
                if (wnd.Zorder < _wndProfile.Count - 1)
                {
                    return _wndProfile.Windows[wnd.Zorder + 1];
                }
                else
                    return _wndProfile.Windows[_wndProfile.Count-1];
            }
            else
                return VWindow.InvalidWindow;
        }

        /// <summary>
        /// Updates the window profile according to the currently open visible windows.
        /// </summary>
        public void Refresh()
        {
            _wndProfile.Clear();
            Win32.EnumDelegate func = new Win32.EnumDelegate(EnumWindowsProc);
            IntPtr desktop = IntPtr.Zero;
            Win32.EnumDesktopWindows(desktop, func, IntPtr.Zero);
            _wndProfile.Sort();
        }
        #endregion

        #region Private Methods
        #region Window Listing
        private bool EnumWindowsProc(IntPtr hWnd, int lParam)
        {
            if (Win32.IsWindowVisible(hWnd))
            {
                if (Win32.GetParent(hWnd) == IntPtr.Zero)
                {
                    //if (Win32.GetWindowLongPtr(hWnd, (int)GWL.GWL_HWNDPARENT) == IntPtr.Zero)
                    //{
                        string title = GetWindowTitle(hWnd).Trim();
                        if (title != string.Empty)
                            _wndProfile.AddWindow(hWnd);
                    //}
                }
            }
            return true;
        }
        #endregion

        #region Notepad Methods
        private AutomationElement GetNotepadEdit(AutomationElement node)
        {
            Condition condition = new AndCondition(
                                    new PropertyCondition(AutomationElement.ClassNameProperty, "Edit"),
                                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            AutomationElement edit = node.FindFirst(TreeScope.Descendants, condition);
            return edit;
        }

        private string GetNotepadText(AutomationElement node)
        {
            AutomationElement edit = GetNotepadEdit(node);

            TextPattern pattern = edit.GetCurrentPattern(TextPattern.Pattern) as TextPattern;
            string ret = string.Empty;
            if (pattern != null)
            {
                TextPatternRange[] ranges = pattern.GetVisibleRanges();
                foreach (TextPatternRange range in ranges)
                {
                    ret += range.GetText(-1);
                }
            }
            return ret;
        }

        private string GetNotepadSelectedText(AutomationElement node)
        {
            AutomationElement edit = GetNotepadEdit(node);
            TextPattern pattern = edit.GetCurrentPattern(TextPattern.Pattern) as TextPattern;
            string ret = string.Empty;
            if (pattern != null)
            {
                TextPatternRange[] ranges = pattern.GetSelection();
                foreach (TextPatternRange range in ranges)
                {
                    ret += range.GetText(-1);
                }
            }
            return ret;
        }
        #endregion

        #region Wordpad Methods
        private AutomationElement GetWordpadEdit(AutomationElement node)
        {
            Condition condition = new AndCondition(
                                    new PropertyCondition(AutomationElement.ClassNameProperty, "RICHEDIT50W"),
                                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            AutomationElement edit = node.FindFirst(TreeScope.Descendants, condition);
            return edit;
        }

        private string GetWordpadText(AutomationElement node)
        {
            AutomationElement edit = GetWordpadEdit(node);

            TextPattern pattern = edit.GetCurrentPattern(TextPattern.Pattern) as TextPattern;
            string ret = string.Empty;
            if (pattern != null)
            {
                TextPatternRange[] ranges = pattern.GetVisibleRanges();
                foreach (TextPatternRange range in ranges)
                {
                    ret += range.GetText(-1);
                }
            }
            return ret;
        }

        private string GetWordpadSelectedText(AutomationElement node)
        {
            AutomationElement edit = GetWordpadEdit(node);
            TextPattern pattern = edit.GetCurrentPattern(TextPattern.Pattern) as TextPattern;
            string ret = string.Empty;
            if (pattern != null)
            {
                TextPatternRange[] ranges = pattern.GetSelection();
                foreach (TextPatternRange range in ranges)
                {
                    ret += range.GetText(-1);
                }
            }
            return ret;
        }
        #endregion

        #region Windows Explorer Methods
        private SHDocVw.InternetExplorer GetWindowsExplorer(IntPtr hWnd)
        {
            SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindowsClass();

            string filename;

            foreach (SHDocVw.InternetExplorer ie in shellWindows)
            {
                try
                {
                    if (File.Exists(ie.FullName))
                    {
                        filename = Path.GetFileNameWithoutExtension(ie.FullName).ToLower();
                        if (filename.Equals("explorer"))
                        {
                            try
                            {
                                if (ie.HWND == hWnd.ToInt32())
                                {
                                    return ie;
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                catch
                {
                    // POSSIBLE PROBLEM HERE
                }
            }
            return null;
        }

        public string GetWExplorerUrl(IntPtr hWnd)
        {
            //SHDocVw.InternetExplorer ie = GetWindowsExplorer(hWnd);
            //return ie.LocationURL;
            string path = string.Empty;
            string class_name = GetClassName(hWnd);
            SHDocVw.InternetExplorer ie = GetWindowsExplorer(hWnd);
            if (ie != null)
            {
                if (class_name == "CabinetWClass" || class_name == "ExploreWClass")
                {
                    try
                    {
                        Shell32.IShellFolderViewDual2 folderView = ie.Document as Shell32.IShellFolderViewDual2;
                        path = ((Shell32.Folder3)folderView.Folder).Self.Path;
                    }
                    catch
                    {

                    }
                }
            }
            else if (class_name == "Progman")
                path = CommonInfo.UserDesktop;
            //string name = folderView.Folder.Title;
            //Shell32.Folder parent = folderView.Folder.ParentFolder;
            //Shell32.FolderItem item = parent.ParseName(name);
            //MessageBox.Show(item.Path);
            return path;
        }

        private string GetWExplorerText(IntPtr hWnd)
        {
            return string.Empty;
        }

        private string GetWExplorerSelectedText(IntPtr hWnd)
        {
            return string.Empty;
        }

        public string[] GetWExplorerContent(IntPtr hWnd)
        {
            List<string> files = new List<string>();
            string class_name = GetClassName(hWnd);
            SHDocVw.InternetExplorer ie = GetWindowsExplorer(hWnd);
            if (ie != null)
            {
                if (class_name == "CabinetWClass" || class_name == "ExploreWClass")
                {
                    Shell32.IShellFolderViewDual2 folderView = ie.Document as Shell32.IShellFolderViewDual2;
                    Shell32.FolderItems items = folderView.Folder.Items();
                    foreach (Shell32.FolderItem item in items)
                    {
                        files.Add(item.Path);
                    }
                }
            }
            else if (class_name == "Progman")
            {
                Directory.GetFiles(CommonInfo.UserDesktop, "*.*", SearchOption.AllDirectories);
            }
            return files.ToArray();
        }

        public string[] GetWExplorerSelectedContent(IntPtr hWnd)
        {
            List<string> files = new List<string>();
            SHDocVw.InternetExplorer ie = GetWindowsExplorer(hWnd);
            if (ie != null)
            {
                string class_name = GetClassName(hWnd);
                if (class_name == "CabinetWClass" || class_name == "ExploreWClass")
                {
                    Shell32.IShellFolderViewDual2 folderView = ie.Document as Shell32.IShellFolderViewDual2;
                    Shell32.FolderItems items = folderView.SelectedItems();
                    foreach (Shell32.FolderItem item in items)
                    {
                        files.Add(item.Path);
                    }
                }
            }
            return files.ToArray();
        }
        #endregion

        #region Internet Explorer Methods
        private SHDocVw.InternetExplorer GetInternetExplorer(IntPtr hWnd)
        {
            SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindowsClass();

            string filename;

            foreach (SHDocVw.InternetExplorer ie in shellWindows)
            {
                if (File.Exists(ie.FullName))
                {
                    filename = Path.GetFileNameWithoutExtension(ie.FullName).ToLower();
                    if (filename.Equals("iexplore"))
                    {
                        if (ie.HWND == hWnd.ToInt32())
                        {
                            return ie;
                        }
                    }
                }
            }
            return null;
        }

        public string GetIExplorerUrl(IntPtr hWnd)
        {
            SHDocVw.InternetExplorer ie = GetInternetExplorer(hWnd);
            return ie.LocationURL;
        }

        public string GetIExplorerText(IntPtr hWnd)
        {
            SHDocVw.InternetExplorer ie = GetInternetExplorer(hWnd);
            mshtml.IHTMLDocument2 htmlDoc = ie.Document as mshtml.IHTMLDocument2;
            return GetHtmlDocText(htmlDoc, false);
        }

        public string GetIExplorerSelectedText(IntPtr hWnd)
        {
            SHDocVw.InternetExplorer ie = GetInternetExplorer(hWnd);
            mshtml.IHTMLDocument2 htmlDoc = ie.Document as mshtml.IHTMLDocument2;
            return GetHtmlDocSelectedText(htmlDoc, false);
        }

        public string GetIExplorerContent(IntPtr hWnd)
        {
            SHDocVw.InternetExplorer ie = GetInternetExplorer(hWnd);
            mshtml.IHTMLDocument2 htmlDoc = ie.Document as mshtml.IHTMLDocument2;
            return GetHtmlDocText(htmlDoc, true);
        }

        public string GetIExplorerSelectedContent(IntPtr hWnd)
        {
            SHDocVw.InternetExplorer ie = GetInternetExplorer(hWnd);
            mshtml.IHTMLDocument2 htmlDoc = ie.Document as mshtml.IHTMLDocument2;
            return GetHtmlDocSelectedText(htmlDoc, true);
        }
        #endregion

        #region Mozilla Firefox Methods
        public string GetFirefoxUrl(IntPtr hWnd)
        {
            string ret = string.Empty;
            AutomationElement root = AutomationElement.FromHandle(hWnd);
            Condition condition = new AndCondition(
                                   new PropertyCondition(AutomationElement.ClassNameProperty, "MozillaContentWindowClass"),
                                   new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            AutomationElement content = root.FindFirst(TreeScope.Descendants, condition);
            if (content == null)
            {
                condition = new PropertyCondition(AutomationElement.NameProperty, "Location");
                content = root.FindFirst(TreeScope.Descendants, condition);
            }

            if (content != null)
            {
                ValuePattern pattern = content.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                if (pattern != null)
                {
                    ret = pattern.Current.Value;
                }
            }
            return ret;
        }
        #endregion

        #region Opera Methods
        public string GetOperaUrl(IntPtr hWnd)
        {
            string ret = string.Empty;
            AutomationElement root = AutomationElement.FromHandle(hWnd);
            // find correct subwindow
            Condition condition = new AndCondition(
                                    new NotCondition(new PropertyCondition(AutomationElement.NameProperty, string.Empty)),
                                   new PropertyCondition(AutomationElement.ClassNameProperty, "OpWindow"),
                                   new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane));
            AutomationElement subwindow = root.FindFirst(TreeScope.Descendants, condition);
            if (subwindow != null)
            {
                // find toolbar
                condition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ToolBar);
                AutomationElement toolbar = subwindow.FindFirst(TreeScope.Descendants, condition);
                if (toolbar != null)
                {
                    // find all edit boxes
                    condition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit);

                    AutomationElementCollection edits = toolbar.FindAll(TreeScope.Descendants, condition);
                    // pick one with a valid url
                    foreach (AutomationElement edit in edits)
                    {
                        ValuePattern pattern = edit.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                        if (pattern != null)
                        {
                            string text = pattern.Current.Value;
                            if (_web_regex.IsMatch(text))
                            {
                                ret = pattern.Current.Value;
                                break;
                            }
                        }
                    }

                    
                }
            }
            return ret;
        }
        #endregion

        #region Google Chrome Methods
        public string GetChromeUrl(IntPtr hWnd)
        {
            string ret = string.Empty;
            AutomationElement root = AutomationElement.FromHandle(hWnd);
            Condition condition = new AndCondition(
                                   new PropertyCondition(AutomationElement.ClassNameProperty, "Chrome_AutocompleteEditView"),
                                   new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            AutomationElement location = root.FindFirst(TreeScope.Descendants, condition);

            if (location != null)
            {
                ValuePattern pattern = location.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                if (pattern != null)
                {
                    ret = pattern.Current.Value;
                }
            }
            return ret;
        }
        #endregion

        #region HTML Docs Methods
        private string GetHtmlDocText(mshtml.IHTMLDocument2 htmlDoc, bool html)
        {
            if (html)
                return htmlDoc.body.parentElement.outerHTML;
            else
                return htmlDoc.body.innerText;
        }

        private string GetHtmlDocSelectedText(mshtml.IHTMLDocument2 htmlDoc, bool html)
        {
            IHTMLSelectionObject selobj = null;
            IHTMLTxtRange range = null;

            if ((htmlDoc == null) || (htmlDoc.selection == null))
                return string.Empty;

            selobj = htmlDoc.selection as IHTMLSelectionObject;
            if (selobj == null)
                return string.Empty;

            range = selobj.createRange() as IHTMLTxtRange;
            if (range == null)
                return string.Empty;

            if (html)
                return range.htmlText;
            else
                return range.text;
        }

        private string GetHtmlDocContent(mshtml.IHTMLDocument2 htmlDoc)
        {
            return htmlDoc.body.innerHTML;
        }

        private string GetHtmlDocSelectedContent(mshtml.IHTMLDocument2 htmlDoc)
        {
            IHTMLSelectionObject selobj = null;
            IHTMLTxtRange range = null;

            if ((htmlDoc == null) || (htmlDoc.selection == null))
                return string.Empty;

            selobj = htmlDoc.selection as IHTMLSelectionObject;
            if (selobj == null)
                return string.Empty;

            range = selobj.createRange() as IHTMLTxtRange;
            if (range == null)
                return string.Empty;

            return range.htmlText;
        }
        #endregion
        #endregion
    }
}
