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
        private bool _favorite;
        private static string _search_term_token = "%s";
        #endregion

        #region Accessors
        [XmlAttribute("name")]
        public string Name { get { return _name; } set { _name = value; } }
        [XmlAttribute("url")]
        public string Url { get { return _url; } set { _url = value; } }
        [XmlAttribute("search query")]
        public string SearchQuery { get { return _search_query; } set { _search_query = value; } }
        [XmlAttribute("favorite")]
        public bool IsFavorite { get { return _favorite; } set { _favorite = value; } }
        public static string SearchTermToken { get { return _search_term_token; } }
        #endregion

        #region Constructors
        public SearchEngine()
        {
            _name = string.Empty;
            _favorite = false;
            _url = string.Empty;
            _search_query = string.Empty;
        }

        public SearchEngine(SearchEngine engine)
        {
            _name = engine.Name;
            _favorite = false;
            _url = engine.Url;
            _search_query = engine.SearchQuery;
        }

        public SearchEngine(string name, string url, string search_query)
        {
            _name = name;
            _favorite = false;
            _url = url;
            _search_query = search_query;
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
