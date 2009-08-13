using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;

namespace SystemCore.SystemAbstraction.WindowManagement
{
    /// <summary>
    /// Represents a profile of windows displayed to the user at a specified moment.
    /// </summary>
    public class VWndProfile
    {
        #region Properties
        private List<VWindow> _windows;
        private int _count;
        private bool _sorted;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the list of visible windows in this profile.
        /// </summary>
        public List<VWindow> Windows
        {
            get
            {
                if (!_sorted)
                    Sort();
                return _windows;
            }
        }

        /// <summary>
        /// Gets the number of windows in this profile.
        /// </summary>
        public int Count
        {
            get { return _count; }
        }
        #endregion

        #region Constructors
        public VWndProfile()
        {
            _windows = new List<VWindow>();
            _count = 0;
            _sorted = true;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the specified window to the profile.
        /// </summary>
        /// <param name="hWnd">Handle of the window to be added.</param>
        public void AddWindow(IntPtr hWnd)
        {
            //AutomationElement root = AutomationElement.FromHandle(hWnd);
            //string className = root.Current.ClassName;
            VWindow window = new VWindow(hWnd, _count, WindowUtility.Instance.GetProcessName(hWnd));
            _windows.Add(window);
            _count++;
            _sorted = false;
        }

        /// <summary>
        /// Removes all windows from this profile.
        /// </summary>
        public void Clear()
        {
            _windows.Clear();
            _count = 0;
            _sorted = true;
        }

        /// <summary>
        /// Sorts all windows according to their z-order.
        /// </summary>
        public void Sort()
        {
            RankWnd();
            _windows.Sort(WndComparison);
            _sorted = true;
        }
        #endregion

        #region PrivateMethods
        private void RankWnd()
        {
            int pos = 0;
            IntPtr forewindow = Win32.GetForegroundWindow();
            
            if (forewindow != IntPtr.Zero)
            {
                for (int i = 0; i < _windows.Count; i++)
                {
                    if (_windows[i].Handle == forewindow)
                    {
                        _windows[i] = new VWindow(forewindow, pos, _windows[i].ProcessName);
                        //MessageBox.Show("Title: "+WindowUtility.Instance.GetWindowTitle(forewindow) + " Class: " + WindowUtility.Instance.GetClassName(forewindow) + " Process: " + WindowUtility.Instance.GetProcessName(forewindow));
                        pos++;
                        break;
                    }
                }
            }
            IntPtr hWnd = Win32.GetWindow(Win32.GetForegroundWindow(), (uint)GetWindow_Cmd.GW_HWNDFIRST);
            while (hWnd != IntPtr.Zero)
            {
                //if (_windows.Contains(hWnd))
                //{
                //    _window_position[hWnd] = pos;
                //    pos++;
                //}
                if (hWnd != forewindow)
                {
                    for (int i = 0; i < _windows.Count; i++)
                    {
                        if (_windows[i].Handle == hWnd)
                        {
                            _windows[i] = new VWindow(hWnd, pos, _windows[i].ProcessName);
                            pos++;
                            break;
                        }
                    }
                }
                hWnd = Win32.GetWindow(hWnd, (uint)GetWindow_Cmd.GW_HWNDNEXT);
            }
        }

        private int WndComparison(VWindow x, VWindow y)
        {
            if (x.Handle == null)
            {
                if (y.Handle == null)
                {
                    return 0; // equal
                }
                else
                {
                    return -1; // y is greater
                }
            }
            else
            {
                if (y.Handle == null)
                {
                    return 1; // x is greater
                }
                else
                {
                    return x.Zorder.CompareTo(y.Zorder); // compare x value with y value
                }
            }
        }
        #endregion
    }
}
