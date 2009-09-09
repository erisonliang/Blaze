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
    /// Represents a data container that provides information about a processor.
    /// </summary>
    /// <remarks>This class encapsulates some of the data provided by the win32 class <see href="http://msdn.microsoft.com/en-us/library/aa394373.aspx">Win32_Processor</see>.</remarks>
    public class Processor
    {
        #region Properties
        private UInt16 _address_width;
        private UInt16 _data_width;
        private string _caption;
        private UInt32 _clock_speed;
        private UInt16 _voltage;
        private string _description;
        private string _device_id;
        private UInt32 _external_clock;
        private UInt32 _l2_cache_size;
        private UInt32 _l3_cache_size;
        private UInt16 _load_percentage;
        private UInt32 _max_clock_speed;
        private string _manufacturer;
        private string _name;
        private UInt32 _number_of_cores;
        private UInt32 _number_of_logical_processors;
        private string _processor_id;
        private string _role;
        private string _socket;
        private string _stepping;
        private string _system_name;
        private string _unique_id;
        private string _version;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the processor's address width. On a 32-bit operating system, the value is 32 and on a 64-bit operating system it is 64.
        /// </summary>
        public UInt16 AddressWidth { get { return _address_width; } }
        /// <summary>
        /// Gets the processor's data with. On a 32-bit processor, the value is 32 and on a 64-bit processor it is 64.
        /// </summary>
        public UInt16 DataWidth { get { return _data_width; } }
        /// <summary>
        /// Gets a short description of an object.
        /// </summary>
        public string Caption { get { return _caption; } }
        /// <summary>
        /// Gets the current speed of the processor, in MHz.
        /// </summary>
        public UInt32 ClockSpeed { get { return _clock_speed; } }
        /// <summary>
        /// Gets the voltage of the processor.
        /// </summary>
        public UInt16 Voltage { get { return _voltage; } }
        /// <summary>
        /// Gets a description of the object.
        /// </summary>
        public string Description { get { return _description; } }
        /// <summary>
        /// Gets the unique identifier of a processor on the system.
        /// </summary>
        public string DeviceId { get { return _device_id; } }
        /// <summary>
        /// Gets the external clock frequency, in MHz.
        /// </summary>
        public UInt32 ExternalClock { get { return _external_clock; } }
        /// <summary>
        /// Gets the size of the Level 2 processor cache.
        /// </summary>
        public UInt32 L2CacheSize { get { return _l2_cache_size; } }
        /// <summary>
        /// Gets the size of the Level 3 processor cache.
        /// </summary>
        public UInt32 L3CacheSize { get { return _l3_cache_size; } }
        /// <summary>
        /// Gets the load capacity of each processor, averaged to the last second.
        /// </summary>
        public UInt16 LoadPercentage { get { return _load_percentage; } }
        /// <summary>
        /// Gets the maximum speed of the processor, in MHz.
        /// </summary>
        public UInt32 MaxClockSpeed { get { return _max_clock_speed; } }
        /// <summary>
        /// Gets the name of the processor manufacturer.
        /// </summary>
        public string Manufacturer { get { return _manufacturer; } }
        /// <summary>
        /// Gets the label by which the object is known.
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// Gets the number of cores for the current instance of the processor.
        /// </summary>
        public UInt32 NumberOfCores { get { return _number_of_cores; } }
        /// <summary>
        /// Gets the number of logical processors for the current instance of the processor.
        /// </summary>
        public UInt32 NumberOfLogicalProcessors { get { return _number_of_logical_processors; } }
        /// <summary>
        /// Gets the processor information that describes the processor features.
        /// </summary>
        public string ProcessorId { get { return _processor_id; } }
        /// <summary>
        /// Gets the role of the processor.
        /// </summary>
        public string Role { get { return _role; } }
        /// <summary>
        /// Gets the type of chip socket used on the circuit.
        /// </summary>
        public string Socket { get { return _socket; } }
        /// <summary>
        /// Gets the revision level of the processor in the processor family.
        /// </summary>
        public string Stepping { get { return _stepping; } }
        /// <summary>
        /// Gets the name of the scoping system.
        /// </summary>
        public string SystemName { get { return _system_name; } }
        /// <summary>
        /// Gets the globally unique identifier for the processor.
        /// </summary>
        public string UniqueId { get { return _unique_id; } }
        /// <summary>
        /// Gets the processor revision number that depends on the architecture.
        /// </summary>
        public string Version { get { return _version; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of ContextLib.DataContainers.Devices.Processor class.
        /// </summary>
        /// <param name="address_width">On a 32-bit operating system, the value is 32 and on a 64-bit operating system it is 64.</param>
        /// <param name="data_width">On a 32-bit processor, the value is 32 and on a 64-bit processor it is 64.</param>
        /// <param name="caption">Short description of an object.</param>
        /// <param name="clock_speed">Current speed of the processor, in MHz.</param>
        /// <param name="voltage">Voltage of the processor.</param>
        /// <param name="description">Description of the object.</param>
        /// <param name="device_id">Unique identifier of a processor on the system.</param>
        /// <param name="external_clock">External clock frequency, in MHz.</param>
        /// <param name="l2_cache_size">Size of the Level 2 processor cache.</param>
        /// <param name="l3_cache_size">Size of the Level 3 processor cache.</param>
        /// <param name="load_percentage">Load capacity of each processor, averaged to the last second.</param>
        /// <param name="max_clock_speed">Maximum speed of the processor, in MHz.</param>
        /// <param name="manufacturer">Name of the processor manufacturer.</param>
        /// <param name="name">Label by which the object is known.</param>
        /// <param name="number_of_cores">Number of cores for the current instance of the processor.</param>
        /// <param name="number_of_logical_processors">Number of logical processors for the current instance of the processor.</param>
        /// <param name="processor_id">Processor information that describes the processor features.</param>
        /// <param name="role">Role of the processor.</param>
        /// <param name="socket">Type of chip socket used on the circuit.</param>
        /// <param name="stepping">Revision level of the processor in the processor family.</param>
        /// <param name="system_name">Name of the scoping system.</param>
        /// <param name="unique_id">Globally unique identifier for the processor.</param>
        /// <param name="version">Processor revision number that depends on the architecture.</param>
        public Processor(UInt16 address_width, UInt16 data_width, string caption, UInt32 clock_speed, UInt16 voltage, string description, string device_id,
                            UInt32 external_clock, UInt32 l2_cache_size, UInt32 l3_cache_size,
                            UInt16 load_percentage, UInt32 max_clock_speed, string manufacturer, string name, UInt32 number_of_cores,
                            UInt32 number_of_logical_processors, string processor_id, string role, string socket, string stepping,
                            string system_name, string unique_id, string version)
        {
            _address_width = address_width;
            _data_width = data_width;
            _caption = caption;
            _clock_speed = clock_speed;
            _voltage = voltage;
            _description = description;
            _device_id = device_id;
            _external_clock = external_clock;
            _l2_cache_size = l2_cache_size;
            _l3_cache_size = l3_cache_size;
            _load_percentage = load_percentage;
            _max_clock_speed = max_clock_speed;
            _manufacturer = manufacturer;
            _name = name;
            _number_of_cores = number_of_cores;
            _number_of_logical_processors = number_of_logical_processors;
            _processor_id = processor_id;
            _role = role;
            _socket = socket;
            _stepping = stepping;
            _system_name = system_name;
            _unique_id = unique_id;
            _version = version;
        }       
        #endregion
    }
}
