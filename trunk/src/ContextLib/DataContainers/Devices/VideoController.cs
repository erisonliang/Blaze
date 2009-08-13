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
    /// Represents a data container that provides information about a specific 
    /// video controller.
    /// </summary>
    /// <remarks>This class encapsulates some of the data provided by the win32 class <see href="http://msdn.microsoft.com/en-us/library/aa394512(VS.85).aspx">Win32_VideoController</see>.</remarks>
    public class VideoController
    {
        #region Properties
        private string _caption;
        private string _name;
        private string _description;
        private string _device_id;
        private string _system_name;
        private UInt32 _memory_size;
        private string _video_processor;
        private UInt32 _bits_per_pixel;
        private UInt32 _horizontal_resolution;
        private UInt32 _vertical_resolution;
        private UInt32 _refresh_rate;
        private string _video_mode;
        private string _driver_date;
        private string _driver_version;
        private string _driver_name;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets a short description (one-line string) of the controller.
        /// </summary>
        public string Caption { get { return _caption; } }
        /// <summary>
        /// Gets the label by which the controller is known.
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// Geths the description of the controller.
        /// </summary>
        public string Description { get { return _description; } }
        /// <summary>
        /// Gets the identifier (unique to the computer system) for this video controller.
        /// </summary>
        public string DeviceID { get { return _device_id; } }
        /// <summary>
        /// Gets the name of the scoping system.
        /// </summary>
        public string SystemName { get { return _system_name; } }
        /// <summary>
        /// Gets the memory size of the video adapter, in bytes.
        /// </summary>
        public UInt32 MemorySize { get { return _memory_size; } }
        /// <summary>
        /// Gets a free-form string describing the video processor.
        /// </summary>
        public string VideoProcessor { get { return _video_processor; } }
        /// <summary>
        /// Gets the number of bits used to display each pixel.
        /// </summary>
        public UInt32 BitsPerPixel { get { return _bits_per_pixel; } }
        /// <summary>
        /// Gets the current number of horizontal pixels.
        /// </summary>
        public UInt32 HorizontalResolution { get { return _horizontal_resolution; } }
        /// <summary>
        /// Gets the current number of vertical pixels.
        /// </summary>
        public UInt32 VerticalResolution { get { return _vertical_resolution; } }
        /// <summary>
        /// Gets the frequency at which the video controller refreshes the image for the monitor, in Hertz.
        /// </summary>
        public UInt32 RefreshRate { get { return _refresh_rate; } }
        /// <summary>
        /// Gets the current resolution, color, and scan mode settings of the video controller.
        /// </summary>
        public string VideoMode { get { return _video_mode; } }
        /// <summary>
        /// Gets the last modification date and time of the currently-installed video driver.
        /// </summary>
        public string DriverDate { get { return _driver_date; } }
        /// <summary>
        /// Gets the name of the installed display device driver.
        /// </summary>
        public string DriverName { get { return _driver_name; } }
        /// <summary>
        /// Gets the version number of the video driver.
        /// </summary>
        public string DriverVersion { get { return _driver_version; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of ContextLib.Controllers.VideoController class.
        /// </summary>
        /// <param name="caption">Short description (one-line string) of the controller.</param>
        /// <param name="name">Label by which the controller is known.</param>
        /// <param name="description">Description of the controller.</param>
        /// <param name="device_id">Identifier (unique to the computer system) for this video controller.</param>
        /// <param name="system_name">Name of the scoping system.</param>
        /// <param name="memory_size">Memory size of the video adapter, in bytes.</param>
        /// <param name="video_processor">Free-form string describing the video processor.</param>
        /// <param name="bits_per_pixel">Number of bits used to display each pixel.</param>
        /// <param name="horizontal_resolution">Current number of horizontal pixels.</param>
        /// <param name="vertical_resolution">Current number of vertical pixels.</param>
        /// <param name="refresh_rate">Frequency at which the video controller refreshes the image for the monitor, in Hertz.</param>
        /// <param name="video_mode">Current resolution, color, and scan mode settings of the video controller.</param>
        /// <param name="driver_date">Last modification date and time of the currently-installed video driver.</param>
        /// <param name="driver_name">Name of the installed display device driver.</param>
        /// <param name="driver_version">Version number of the video driver.</param>
        public VideoController(string caption, string name, string description, string device_id,
                                string system_name, UInt32 memory_size, string video_processor,
                                UInt32 bits_per_pixel, UInt32 horizontal_resolution, UInt32 vertical_resolution,
                                UInt32 refresh_rate, string video_mode, string driver_date,
                                string driver_name, string driver_version)
        {
            _caption = caption;
            _name = name;
            _description = description;
            _device_id = device_id;
            _system_name = system_name;
            _memory_size = memory_size;
            _video_processor = video_processor;
            _bits_per_pixel = bits_per_pixel;
            _horizontal_resolution = horizontal_resolution;
            _vertical_resolution = vertical_resolution;
            _refresh_rate = refresh_rate;
            _video_mode = video_mode;
            _driver_date = driver_date;
            _driver_name = driver_name;
            _driver_version = driver_version;
        }
        #endregion
    }
}
