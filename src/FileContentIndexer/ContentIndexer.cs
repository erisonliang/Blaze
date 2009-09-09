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
using System.Reflection;
using System.Text;
using SystemCore.CommonTypes;

namespace FileContentIndexer
{
    [AutomatorPlugin("ContentIndexer: Indexes the contents of text files.")]
    public class ContentIndexer : IndexerPlugin
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
        public ContentIndexer()
            : base("Indexes the contents of text files.", "Index file contents", new string[] { ".txt" })
        {

        }
        #endregion

        #region Public Methods
        public override string[] GetFileKeywords(string file_path)
        {
            FileInfo info;
            try
            {
                info = new FileInfo(file_path);
            }
            catch
            {
                return new string[0];
            }
            if (info.Extension.ToLower() == ".txt")
            {
                StreamReader file = new StreamReader(file_path, Encoding.Default);
                string text = file.ReadToEnd();
                string[] tags = text.Split(new string[] { " ", "_", "(", ")", "[", "]", "{", "}", ".", "?", "!" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < tags.Length; i++)
                {
                    tags[i] = tags[i].ToLower();
                }
                return tags;
            }
            else
            {
                return new string[0];
            }
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
