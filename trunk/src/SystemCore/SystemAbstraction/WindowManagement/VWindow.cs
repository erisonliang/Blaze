using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCore.SystemAbstraction.WindowManagement
{
    /// <summary>
    /// Represents a visible window to the user.
    /// </summary>
    public class VWindow
    {
        #region Properties
        protected IntPtr _handle;
        protected int _zorder;
        protected string _process_name;
        /// <summary>
        /// A read-only field that represents an invalid visible window.
        /// </summary>
        public static readonly VWindow InvalidWindow = new VWindow(IntPtr.Zero, 0, string.Empty);
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the window handle.
        /// </summary>
        public IntPtr Handle { get { return _handle; } }
        /// <summary>
        /// Gets the order from which the window is displayed to the user (starting in 0).
        /// </summary>
        public int Zorder { get { return _zorder; } }
        /// <summary>
        /// Gets the window process name.
        /// </summary>
        public string ProcessName { get { return _process_name; } }
        #endregion

        #region Constructors
        public VWindow(IntPtr handle, int zorder, string process_name)
        {
            _handle = handle;
            _zorder = zorder;
            _process_name = process_name;
        }
        #endregion

        #region Public Methods
        ///// <summary>
        ///// Gets the window title.
        ///// </summary>
        ///// <returns>String containing the window title.</returns>
        //public string GetTitle()
        //{
        //    return WindowUtility.Instance.GetWindowTitle(this);
        //}

        ///// <summary>
        ///// Sets a new window title
        ///// </summary>
        ///// <param name="text">String containing the new window title.</param>
        //public void SetTitle(string text)
        //{
        //    WindowUtility.Instance.SetWindowTitle(this, text);
        //}

        ///// <summary>
        ///// Gets the URL or PATH of the address bar of this window.
        ///// </summary>
        ///// <returns>String containing the URL or PATH.</returns>
        //public string GetUrl()
        //{
        //    return WindowUtility.Instance.GetUrl(this);
        //}

        ///// <summary>
        ///// Gets the text contained in this window.
        ///// </summary>
        ///// <returns>String containing the text.</returns>
        //public string GetText()
        //{
        //    return WindowUtility.Instance.GetText(this);
        //}

        ///// <summary>
        ///// Gets the currently selected text by the user in this window.
        ///// </summary>
        ///// <returns>String containing the selected text by the user.</returns>
        //public string GetSelectedText()
        //{
        //    return WindowUtility.Instance.GetSelectedText(this);
        //}

        ///// <summary>
        ///// Gets the content of this window.
        ///// </summary>
        ///// <returns>String array containing the window content.</returns>
        //public string[] GetContent()
        //{
        //    return WindowUtility.Instance.GetContent(this);
        //}

        ///// <summary>
        ///// Gets the currently selected content by the user in this window.
        ///// </summary>
        ///// <returns>String array containing the selected content.</returns>
        //public string[] GetSelectedContet()
        //{
        //    return WindowUtility.Instance.GetSelectedContent(this);
        //}
        #endregion

        #region Operators
        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;
            VWindow wnd = (VWindow)obj;
            if (wnd == null) // check if it can be casted
                return false;
            return Handle == wnd.Handle;
        }

        public bool Equals(VWindow wnd)
        {
            if ((object)wnd == null)
                return false;
            else
                return Handle == wnd.Handle;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(VWindow a, VWindow b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Handle == b.Handle;

        }

        public static bool operator !=(VWindow a, VWindow b)
        {
            return !(a == b);
        }
        #endregion
    }
}
