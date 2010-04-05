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
using System.Xml.Serialization;

namespace WebSearch
{
    [XmlRoot("SearchEngine")]
    public class SearchEngine
    {
        #region Properties
        private string _name;
        private string _url;
        private string _search_query;
        private string _icon_source;
        private static string _search_term_token = "%s";
        #endregion

        #region Accessors
        [XmlAttribute("name")]
        public string Name { get { return _name; } set { _name = value; } }
        [XmlAttribute("url")]
        public string Url { get { return _url; } set { _url = value; } }
        [XmlAttribute("search_query")]
        public string SearchQuery { get { return _search_query; } set { _search_query = value; } }
        [XmlAttribute("icon_source")]
        public string IconSource { get { return _icon_source; } set { _icon_source = value; } }
        public static string SearchTermToken { get { return _search_term_token; } }
        #endregion

        #region Constructors
        public SearchEngine()
        {
            _name = string.Empty;
            _url = string.Empty;
            _search_query = string.Empty;
            _icon_source = string.Empty;
        }

        public SearchEngine(SearchEngine engine)
        {
            _name = engine.Name;
            _url = engine.Url;
            _search_query = engine.SearchQuery;
            _icon_source = engine.IconSource;
        }

        public SearchEngine(string name, string url, string search_query, string icon_source)
        {
            _name = name;
            _url = url;
            _search_query = search_query;
            _icon_source = icon_source;
        }
        #endregion

        #region Public Methods
        public string GetSearchQuery(string input)
        {
            if (_search_query == string.Empty)
                return _url;
            else
            {
                try
                {
                    return _search_query.Replace(_search_term_token, input);
                }
                catch
                {
                    return _url;
                }
            }
        }
        #endregion
    }
}
