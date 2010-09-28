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
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace ContextLib.DataContainers.Multimedia
{
    /// <summary>
    /// Representes a data container that has four levels of data. One level 
    /// for text, one for file list, another for containing an image and yet 
    /// another for audio.
    /// </summary>
    /// <remarks>This class is intended to store clipboard data.</remarks>
    public class MultiLevelData
    {
        #region Properties
        private string _text;
        private string[] _fileList;
        private Image _image;
        private Stream _audio;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets or sets the text data.
        /// </summary>
        public string Text { get { return _text; } set { _text = value; } }
        /// <summary>
        /// Gets or sets the file list data.
        /// </summary>
        public string[] FileList { get { return _fileList; } set { _fileList = value; } }
        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        public Image Image { get { return _image; } set { _image = value; } }
        /// <summary>
        /// Gets or sets the audio data.
        /// </summary>
        public Stream Audio { get { return _audio; } set { _audio = value; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of ContextLib.DataContainers.Multimedia.MultiLevelData class.
        /// </summary>
        /// <param name="text">Text data.</param>
        /// <param name="fileList">File list data.</param>
        /// <param name="image">Image data.</param>
        /// <param name="audio">Audio data.</param>
        public MultiLevelData(string text, string[] fileList, Image image, Stream audio)
        {
            _text = text;
            _fileList = fileList;
            _image = image;
            _audio = audio;
        }
        /// <summary>
        /// Creates a new instance of ContextLib.MultiLevelData class.
        /// </summary>
        /// <param name="text">Text data.</param>
        public MultiLevelData(string text)
        {
            _text = text;
            _fileList = null;
            _image = null;
            _audio = null;
        }
        /// <summary>
        /// Creates a new instance of ContextLib.MultiLevelData class.
        /// </summary>
        /// <param name="fileList">File list data.</param>
        public MultiLevelData(string[] fileList)
        {
            _text = null;
            _fileList = fileList;
            _image = null;
            _audio = null;
        }
        /// <summary>
        /// Creates a new instance of ContextLib.MultiLevelData class.
        /// </summary>
        /// <param name="image">Image data.</param>
        public MultiLevelData(Image image)
        {
            _text = null;
            _fileList = null;
            _image = image;
            _audio = null;
        }
        /// <summary>
        /// Creates a new instance of ContextLib.MultiLevelData class.
        /// </summary>
        /// <param name="stream">Audio data.</param>
        public MultiLevelData(Stream stream)
        {
            _text = null;
            _fileList = null;
            _image = null;
            _audio = stream;
        }
        /// <summary>
        /// Creates a new instance of ContextLib.MultiLevelData class.
        /// </summary>
        /// <param name="mld">Another instance of MultiLevelData from which data will be duplicated.</param>
        public MultiLevelData(MultiLevelData mld)
        {
            _text = (mld.Text == null ? null : (string)mld.Text.Clone());
            if (_fileList != null)
            {
                _fileList = new string[mld._fileList.Length];
                Array.Copy(mld._fileList, _fileList, _fileList.Length);
            }
            else
                _fileList = null;
            _image = (mld.Image == null ? null : (Image)mld.Image.Clone());
            _audio = mld._audio;
        }
        /// <summary>
        /// Creates a new instance of ContextLib.MultiLevelData class.
        /// </summary>
        public MultiLevelData()
        {
            _text = null;
            _fileList = null;
            _image = null;
            _audio = null;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Populates the MultiLevelData container with the clipboard's content.
        /// </summary>
        public void PopulateFromClipboard()
        {
            Thread staThread = new Thread(
                delegate()
                {
                    try
                    {
                        _text = Clipboard.GetText();
                        StringCollection sc = Clipboard.GetFileDropList();
                        _fileList = new string[sc.Count];
                        sc.CopyTo(_fileList, 0);
                        _image = Clipboard.GetImage();
                        _audio = Clipboard.GetAudioStream();
                    }
                    catch
                    {
                        _text = string.Empty;
                        _fileList = new string[0];
                        _image = null;
                        _audio = null;
                    }
                });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
        }

        /// <summary>
        /// Populates the MultiLevelData container with the specified object's content.
        /// </summary>
        /// <param name="obj">Object with the contents to populate.</param>
        public void Populate(IDataObject obj)
        {
            _text = (string)obj.GetData(DataFormats.Text);
            StringCollection sc = (StringCollection)obj.GetData(DataFormats.FileDrop);
            _fileList = new string[sc.Count];
            sc.CopyTo(_fileList, 0);
            _image = (Image)obj.GetData(DataFormats.Bitmap);
            _audio = (Stream)obj.GetData(DataFormats.WaveAudio);
        }

        /// <summary>
        /// Restores the contained data to the clipboard.
        /// </summary>
        public void RestoreToClipboard()
        {
            IDataObject data = null;
            try
            {
                data = GetDataObject();
                if (!string.IsNullOrEmpty(_text))
                    Clipboard.SetText(_text);
                if (_image != null)
                    Clipboard.SetImage(_image);
                if (_fileList != null && _fileList.Length > 0)
                {
                    StringCollection sc = new StringCollection();
                    foreach (string str in _fileList)
                        sc.Add(str);
                    Clipboard.SetFileDropList(sc);
                }
                if (_audio != null)
                    Clipboard.SetAudio(_audio);
                //Clipboard.SetDataObject(GetDataObject());
            }
            catch
            {
                try
                {
                    System.Threading.Thread.Sleep(25);
                    data = GetDataObject();
                    if (!string.IsNullOrEmpty(_text))
                        Clipboard.SetText(_text);
                    if (_image != null)
                        Clipboard.SetImage(_image);
                    if (_fileList != null && _fileList.Length > 0)
                    {
                        StringCollection sc = new StringCollection();
                        foreach (string str in _fileList)
                            sc.Add(str);
                        Clipboard.SetFileDropList(sc);
                    }
                    if (_audio != null)
                        Clipboard.SetAudio(_audio);
                    //Clipboard.SetDataObject(GetDataObject());
                }
                catch (Exception e)
                {
                    MessageBox.Show("Could not set clipboard data: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Gets the contained data in a IDataObject format.
        /// </summary>
        /// <returns></returns>
        public IDataObject GetDataObject()
        {
            IDataObject dataObj = new DataObject();
            dataObj.SetData(DataFormats.Text, true, _text);
            dataObj.SetData(DataFormats.Bitmap, true, _image);
            StringCollection sc = new StringCollection();
            if (_fileList != null)
                sc.AddRange(_fileList);
            dataObj.SetData(DataFormats.FileDrop, true, sc);
            dataObj.SetData(DataFormats.WaveAudio, true, _audio);
            return dataObj;
        }

        /// <summary>
        /// Clears the MultiLevelData container.
        /// </summary>
        public void Clear()
        {
            _text = null;
            _fileList = null;
            if (_image != null)
                _image.Dispose();
            _image = null;
            if (_audio != null)
                _audio.Dispose();
            _audio = null;
        }

        /// <summary>
        /// Releases all resources user by ContextLib.DataContainers.Multimedia.MultiLevelData.
        /// </summary>
        public void Dispose()
        {
            Clear();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Determines whether this data container and the specified object have the same value.
        /// </summary>
        /// <param name="obj">A system object.</param>
        /// <returns>True if the two objects have the same value, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;
            MultiLevelData mld = (MultiLevelData)obj;
            if (mld == null) // check if it can be casted
                return false;

            bool isTextEqual, isImgEqual, isStrEqual, isStreamEqual;
            isTextEqual = (this.Text == null || mld.Text == null ? (this.Text == mld.Text) : (this.Text.Equals(mld.Text)));
            isImgEqual = (this.Image == null || mld.Image == null ? (this.Image == mld.Image) : (this.Image.Equals(mld.Image)));
            isStrEqual = (this.FileList == null || mld.FileList == null ? (this.FileList == mld.FileList) : (this.FileList.Equals(mld.FileList)));
            isStreamEqual = (this.Audio == null || mld.Audio == null ? (this.Audio == mld.Audio) : (this.Audio.Equals(mld.Audio)));
            return isTextEqual && isImgEqual && isStrEqual && isStreamEqual;
        }

        /// <summary>
        /// Determines whether this data container and the specified one have the same value.
        /// </summary>
        /// <param name="mld">A MultiLevelData container.</param>
        /// <returns>True if the two objects have the same value, false otherwise.</returns>
        public bool Equals(MultiLevelData mld)
        {
            if ((object)mld == null)
                return false;
            else
            {
                bool isTextEqual, isImgEqual, isStrEqual, isStreamEqual;
                isTextEqual = (this.Text == null || mld.Text == null ? (this.Text == mld.Text) : (this.Text.Equals(mld.Text)));
                isImgEqual = (this.Image == null || mld.Image == null ? (this.Image == mld.Image) : (this.Image.Equals(mld.Image)));
                isStrEqual = (this.FileList == null || mld.FileList == null ? (this.FileList == mld.FileList) : (this.FileList.Equals(mld.FileList)));
                isStreamEqual = (this.Audio == null || mld.Audio == null ? (this.Audio == mld.Audio) : (this.Audio.Equals(mld.Audio)));
                return isTextEqual && isImgEqual && isStrEqual && isStreamEqual;
            }
        }

        /// <summary>
        /// Returns the hashcode for this data container.
        /// </summary>
        /// <returns>Integer containing the hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines if object a and b have the same value.
        /// </summary>
        /// <param name="a">A MultiLevelData container.</param>
        /// <param name="b">A MultiLevelData container.</param>
        /// <returns>True if both objects are have the same value, false otherwise.</returns>
        public static bool operator ==(MultiLevelData a, MultiLevelData b)
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
            bool isTextEqual, isImgEqual, isStrEqual, isStreamEqual;
            isTextEqual = (a.Text == null || b.Text == null ? (a.Text == b.Text) : (a.Text.Equals(b.Text)));
            isImgEqual = (a.Image == null || b.Image == null ? (a.Image == b.Image) : (a.Image.Equals(b.Image)));
            isStrEqual = (a.FileList == null || b.FileList == null ? (a.FileList == b.FileList) : (a.FileList.Equals(b.FileList)));
            isStreamEqual = (a.Audio == null || b.Audio == null ? (a.Audio == b.Audio) : (a.Audio.Equals(b.Audio)));
            return isTextEqual && isImgEqual && isStrEqual && isStreamEqual;

        }

        /// <summary>
        /// Determines if object a and b have different values.
        /// </summary>
        /// <param name="a">A MultiLevelData container.</param>
        /// <param name="b">A MultiLevelData container.</param>
        /// <returns>True if both objects have different values, false otherwise.</returns>
        public static bool operator !=(MultiLevelData a, MultiLevelData b)
        {
            return !(a == b);
        }
        #endregion
    }
}
