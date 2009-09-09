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
using System.Windows.Forms;

namespace SystemCore.CommonTypes
{
    public abstract class IndexerPlugin : Plugin
    {
        #region Properties
        protected string _quickDescription;
        protected string[] _extensions;
        #endregion

        #region Accessors
        public string QuickDescription
        {
            get { return _quickDescription; }
        }

        public string[] Extensions
        {
            get { return _extensions; }
        }
        #endregion

        #region Constructors
        public IndexerPlugin(string name, string description, string quick_description, string[] extensions, string version, string website)
        {
            _configurable = false;
            _type = PluginType.Indexer;
            _name = name;
            _description = description;
            _quickDescription = quick_description;
            _extensions = extensions;
            _version = version;
            _website = website;
        }

        public IndexerPlugin(string description, string quick_description, string[] extensions, string website)
        {
            _configurable = false;
            _type = PluginType.Indexer;
            _name = GetAssembyName();
            _description = description;
            _quickDescription = quick_description;
            _extensions = extensions;
            _version = GetAssemblyVersion();
            _website = website;
        }

        public IndexerPlugin(string description, string quick_description, string[] extensions)
        {
            _configurable = false;
            _type = PluginType.Indexer;
            _name = GetAssembyName();
            _description = description;
            _quickDescription = quick_description;
            _extensions = extensions;
            _version = GetAssemblyVersion();
            _website = string.Empty;
        }
        #endregion

        #region Public Methods
        public abstract string[] GetFileKeywords(string file_path);

        public override void Configure()
        {
            MessageBox.Show("There is nothing to configure.", "No configuration needed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Protected Methods
        protected abstract string GetAssembyName();

        protected abstract string GetAssemblyVersion();
        #endregion
    }
}
