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
using System.Collections.Generic;
using System.Reflection;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction.StringUtilities;

namespace MusicFileTagIndexer
{
    [AutomatorPlugin("TagIndexer: Indexes mp3 and ogg files by their ID3 tags.")]
    public class TagIndexer : IndexerPlugin
    {
        //#region Accessors
        //public override string Name
        //{
        //    get
        //    {
        //        // Get all Title attributes on this assembly
        //        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
        //        // If there is at least one Title attribute
        //        if (attributes.Length > 0)
        //        {
        //            // Select the first one
        //            AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
        //            // If it is not an empty string, return it
        //            if (titleAttribute.Title != "")
        //                return titleAttribute.Title;
        //        }
        //        // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
        //        return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        //    }
        //}

        //public override string Version
        //{
        //    get
        //    {
        //        return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //    }
        //}
        //#endregion

        #region Constructors
        public TagIndexer()
            : base("Indexes mp3 and ogg files by their ID3 tags.", "Index music file tags", new string[] { ".mp3", ".ogg" })
        {

        }
        #endregion

        #region Public Methods
        public override string[] GetFileKeywords(string file_path)
        {
            TagLib.File file;
            try
            {
                file = TagLib.File.Create(file_path);
            }
            catch
            {
                return new string[0];
            }
            List<string> tags = new List<string>();
            try
            {
                if (file.Tag.Album != null)
                    tags.Add(file.Tag.Album);
            }
            catch
            {

            }
            try
            {
                if (file.Tag.Comment != null)
                    tags.Add(file.Tag.Comment);
            }
            catch
            {

            }
            try
            {
                if (file.Tag.Title != null)
                    tags.Add(file.Tag.Title);
            }
            catch
            {

            }
            try
            {
                if (file.Tag.Year.ToString() != null)
                    tags.Add(file.Tag.Year.ToString());
            }
            catch
            {

            }
            foreach (string artist in file.Tag.AlbumArtists)
            {
                if (artist != null)
                    tags.Add(artist);
            }
            foreach (string genre in file.Tag.Genres)
            {
                if (genre != null)
                    tags.Add(genre);
            }
            foreach (string composer in file.Tag.Composers)
            {
                if (composer != null)
                    tags.Add(composer);
            }
            foreach (string performer in file.Tag.Performers)
            {
                if (performer != null)
                    tags.Add(performer);    
            }
            List<string> ret_tags = new List<string>();
            foreach (string tag in tags)
            {
                ret_tags.AddRange(StringUtility.GenerateKeywords(tag));
            }
            return ret_tags.ToArray();
        }
        #endregion

        #region Protected Methods
        protected override string GetAssembyName()
        {
            // Get all Title attributes on this assembly
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            // If there is at least one Title attribute
            if (attributes.Length > 0)
            {
                // Select the first one
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                // If it is not an empty string, return it
                if (titleAttribute.Title != "")
                    return titleAttribute.Title;
            }
            // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
            return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        }

        protected override string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion
    }
}
