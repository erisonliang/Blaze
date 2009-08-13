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
using SystemCore.SystemAbstraction.WindowManagement;

namespace ContextLib.DataContainers.GUI
{
    /// <summary>
    /// Represents an application window.
    /// </summary>
    public class Window
    {
        #region Properties
        private int _handle;
        private string _className;
        private int _threadId;
        private int _processId;
        private string _processName;
        private string _title;
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        /// <summary>
        /// A read-only field that represents an invalid window.
        /// </summary>
        public static readonly Window InvalidWindow = new Window(-1, "InvalidClass", -1, -1, "InvalidProcess", "InvalidWindow", -1, -1, 0, 0);
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the window's handler.
        /// </summary>
        public int Handle { get { return _handle; } }
        /// <summary>
        /// Gets or sets the window's class name.
        /// </summary>
        public string ClassName { get { return _className; } set { _className = value; } }
        /// <summary>
        /// Gets the identifier of the thread that created the window.
        /// </summary>
        public int ThreadID { get { return _threadId; } }
        /// <summary>
        /// Gets the identifier of the process that created the window.
        /// </summary>
        public int ProcessID { get { return _processId; } }
        /// <summary>
        /// Gets or sets the name of the process that created the window.
        /// </summary>
        public string ProcessName { get { return _processName; } set { _processName = value; } }
        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        public string Title { get { return _title; } set { _title = value; } }
        /// <summary>
        /// Gets the window x coordinate.
        /// </summary>
        public int X { get { return _x; } }
        /// <summary>
        /// Gets the window y coodinate.
        /// </summary>
        public int Y { get { return _y; } }
        /// <summary>
        /// Gets the window width.
        /// </summary>
        public int Width { get { return _width; } }
        /// <summary>
        /// Gets the window height.
        /// </summary>
        public int Height { get { return _height; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of ContextLib.DataContainers.GUI.Window class.
        /// </summary>
        /// <param name="handle">Window handle.</param>
        /// <param name="className">Window class name.</param>
        /// <param name="threadId">Identifier of the thread that created the window.</param>
        /// <param name="processId">Identifier of the process that created the window.</param>
        /// <param name="processName">Name of the process that created the window.</param>
        /// <param name="title">Window title.</param>
        /// <param name="x">Window x coordinate</param>
        /// <param name="y">Window y coodinate</param>
        /// <param name="width">Window width</param>
        /// <param name="height">Window height</param>
        public Window(int handle, string className, int threadId, int processId, string processName, string title, int x, int y, int width, int height)
        {
            _handle = handle;
            _className = className;
            _threadId = threadId;
            _processId = processId;
            _processName = processName;
            _title = title;
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Initializes a new instance of ContextLib.DataContainers.GUI.Window class.
        /// </summary>
        /// <param name="handle">Handler of a Win32 window from which to collect data.</param>
        public Window(IntPtr handle)
        {
            _handle = handle.ToInt32();
            _className = WindowUtility.Instance.GetClassName(handle);
            _threadId = (int)WindowUtility.Instance.GetThreadId(handle);
            _processId = (int)WindowUtility.Instance.GetProcessId(handle);
            _processName = WindowUtility.Instance.GetProcessName(handle);
            _title = WindowUtility.Instance.GetWindowTitle(handle);
            System.Drawing.Rectangle rect = WindowUtility.Instance.GetWindowRect(handle);
            _x = rect.X;
            _y = rect.Y;
            _width = rect.Width;
            _height = rect.Height;
        }

        /// <summary>
        /// Initializes a new instance of ContextLib.DataContainers.GUI.Window class.
        /// </summary>
        /// <param name="window">Window object to be copied.</param>
        public Window(Window window)
        {
            _handle = window._handle;
            _className = window._className;
            _threadId = window._threadId;
            _processId = window._processId;
            _processName = window._processName;
            _title = window._title;
            _x = window._x;
            _y = window._y;
            _width = window._width;
            _height = window._height;
        }
        #endregion
    }
}
