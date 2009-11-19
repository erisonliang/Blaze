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
using System.IO;

namespace ContextLib.DataContainers.Devices
{
    /// <summary>
    /// Represents a data container that provides information about a storage 
    /// device.
    /// </summary>
    /// <remarks>This class encapsulates some of the data provided by the win32 class <see href="http://msdn.microsoft.com/en-us/library/aa394173(VS.85).aspx">Win32_LogicalDisk</see>.</remarks>
    public class StorageDevice
    {
        #region Properties
        private string _caption;
        private bool _compressed;
        private string _descritpion;
        private string _device_id;
        private string _file_system;
        private UInt64 _free_space;
        private UInt64 _size;
        private UInt64 _block_size;
        private UInt64 _number_of_blocks;
        private string _name;
        private string _provider_name;
        private string _purpose;
        private string _system_name;
        private string _volume_name;
        private string _volume_serial_number;
        private DriveType _type;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets a short description of the object.
        /// </summary>
        public string Caption { get { return _caption; } }
        /// <summary>
        /// If True, the logical volume exists as a single compressed entity, such as a DoubleSpace volume. If file based compression is supported, such as on NTFS, this property is False.
        /// </summary>
        public bool Compressed { get { return _compressed; } }
        /// <summary>
        /// Gets a description of the object.
        /// </summary>
        public string Descritpion { get { return _descritpion; } }
        /// <summary>
        /// Gets the unique identifier of the logical disk from other devices on the system.
        /// </summary>
        public string DeviceId { get { return _device_id; } }
        /// <summary>
        /// Alias for DeviceId. Gets the device letter.
        /// </summary>
        public string Letter { get { return _device_id; } }
        /// <summary>
        /// Gets the file system on the logical disk.
        /// </summary>
        public string FileSystem { get { return _file_system; } }
        /// <summary>
        /// Gets the amout of space, in bytes, available on the logical disk.
        /// </summary>
        public UInt64 FreeSpace { get { return _free_space; } }
        /// <summary>
        /// Gets the size of the disk drive, in bytes.
        /// </summary>
        public UInt64 Size { get { return _size; } }
        /// <summary>
        /// Gets the size, in bytes, of the blocks that form this storage extent.
        /// </summary>
        public UInt64 BlockSize { get { return _block_size; } }
        /// <summary>
        /// Gets the total number of consecutive blocks, each block the size of the value contained in the BlockSize property, which form this storage extent.
        /// </summary>
        public UInt64 NumberOfBlocks { get { return _number_of_blocks; } }
        /// <summary>
        /// Gets the label by which the object is known.
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// Gets the network path to the logical device.
        /// </summary>
        public string ProviderName { get { return _provider_name; } }
        /// <summary>
        /// Gets the free-form string describing the media and its use.
        /// </summary>
        public string Purpose { get { return _purpose; } }
        /// <summary>
        /// Gets the name of the scoping system.
        /// </summary>
        public string SystemName { get { return _system_name; } }
        /// <summary>
        /// Gets the volume name of the logical disk.
        /// </summary>
        public string VolumeName { get { return _volume_name; } }
        /// <summary>
        /// Gets the volume serial number of the logical disk.
        /// </summary>
        public string VolumeSerialNumber { get { return _volume_serial_number; } }
        /// <summary>
        /// Gets the numeric value that corresponds to the type of disk drive this logical disk represents.
        /// </summary>
        public DriveType Type { get { return _type; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of ContextLib.DataContainers.Devices.StorageDevice class.
        /// </summary>
        /// <param name="caption">Short description of the object.</param>
        /// <param name="compressed">If True, the logical volume exists as a single compressed entity, such as a DoubleSpace volume. If file based compression is supported, such as on NTFS, this property is False.</param>
        /// <param name="description">Description of the object.</param>
        /// <param name="device_id">Unique identifier of the logical disk from other devices on the system.</param>
        /// <param name="file_system">File system on the logical disk.</param>
        /// <param name="free_space">Space, in bytes, available on the logical disk.</param>
        /// <param name="size">Size of the disk drive, in bytes.</param>
        /// <param name="block_size">Size, in bytes, of the blocks that form this storage extent.</param>
        /// <param name="number_of_blocks">Total number of consecutive blocks, each block the size of the value contained in the BlockSize property, which form this storage extent.</param>
        /// <param name="name">Label by which the object is known.</param>
        /// <param name="provider_name">Network path to the logical device.</param>
        /// <param name="purpose">Free-form string describing the media and its use.</param>
        /// <param name="system_name">Name of the scoping system.</param>
        /// <param name="volume_name">Volume name of the logical disk.</param>
        /// <param name="volume_serial_number">Volume serial number of the logical disk.</param>
        /// <param name="type">Numeric value that corresponds to the type of disk drive this logical disk represents.</param>
        public StorageDevice(string caption, bool compressed, string description, string device_id, string file_system,
                                UInt64 free_space, UInt64 size, UInt64 block_size, UInt64 number_of_blocks, string name, string provider_name,
                                string purpose, string system_name, string volume_name, string volume_serial_number, DriveType type)
        {
            _caption = caption;
            _compressed = compressed;
            _descritpion = description;
            _device_id = device_id;
            _file_system = file_system;
            _free_space = free_space;
            _size = size;
            _block_size = block_size;
            _number_of_blocks = number_of_blocks;
            _name = name;
            _provider_name = provider_name;
            _purpose = purpose;
            _system_name = system_name;
            _volume_name = volume_name;
            _volume_serial_number = volume_serial_number;
            _type = type;
        }
        #endregion
    }
}
