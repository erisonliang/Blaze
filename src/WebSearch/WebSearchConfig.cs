using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WebSearch
{
    [XmlRoot("WebSearchConfig")]
    public class WebSearchConfig
    {
        #region Private Methods
        private List<SearchEngine> _search_engines;
        private int _favorite_engine;
        private int _cache_ttl;
        #endregion

        #region Accessors
        [XmlElement("engines")]
        public List<SearchEngine> SearchEngines { get { return _search_engines; } set { _search_engines = value; } }
        [XmlAttribute("favorite_engine")]
        public int FavoriteEngine { get { return _favorite_engine; } set { _favorite_engine = value; } }
        [XmlAttribute("cache_ttl")]
        public int CacheTTL { get { return _cache_ttl; } set { _cache_ttl = value; } }
        #endregion

        #region Constructors
        public WebSearchConfig()
        {
            _search_engines = new List<SearchEngine>();
            _favorite_engine = 0;
            _cache_ttl = 168;
        }
        #endregion

        #region Public Methods
        public void Reset()
        {
            //// add search engines
            // Google
            _search_engines.Add(new SearchEngine("Google",
                                                @"http://www.google.com/ncr",
                                                @"http://www.google.com/search?hl=en&q=%s&btnG=Google+Search&aq=f&oq=",
                                                @"http://www.iconarchive.com/icons/fasticon/web-2/32/Google-icon.png"));
            _favorite_engine = 0;
            // Youtube
            _search_engines.Add(new SearchEngine("YouTube",
                                                @"http://www.youtube.com/",
                                                @"http://www.youtube.com/results?search_query=%s",
                                                @"http://www.iconarchive.com/icons/fasticon/web-2/32/Youtube-icon.png"));
            // Wikipedia
            _search_engines.Add(new SearchEngine("Wikipedia",
                                                @"http://www.wikipedia.org/",
                                                @"http://en.wikipedia.org/wiki/Special:Search?search=%s&go=Go",
                                                @"http://www.iconarchive.com/icons/sykonist/popular-sites/32/Wikipedia-globe-icon.png"));
            // IMDB
            _search_engines.Add(new SearchEngine("IMDB",
                                                @"http://www.imdb.com/",
                                                @"http://www.imdb.com/find?s=all&q=%s",
                                                @"http://www.iconarchive.com/icons/iconshock/cinema/32/film-reel-icon.png"));
            // Yahoo
            _search_engines.Add(new SearchEngine("Yahoo",
                                                @"http://search.yahoo.com/",
                                                @"http://search.yahoo.com/search?p=%s",
                                                @"http://www.iconarchive.com/icons/deleket/puck/32/Yahoo-Messenger-Alternate-icon.png"));
            // Weather
            _search_engines.Add(new SearchEngine("Weather",
                                                @"http://www.weather.com/",
                                                @"http://www.weather.com/weather/today/%s",
                                                @"http://www.iconarchive.com/icons/icons-land/weather/32/Snow-Occasional-icon.png"));
            // Maps
            _search_engines.Add(new SearchEngine("Maps",
                                                @"http://maps.google.com/",
                                                @"http://maps.google.com/maps?f=q&hl=en&geocode=&q=%s&ie=UTF8&z=12&iwloc=addr&om=1",
                                                @"http://www.iconarchive.com/icons/walrick/openphone/32/Maps-icon.png"));
            // Bing
            _search_engines.Add(new SearchEngine("Bing",
                                                @"http://www.bing.com/",
                                                @"http://www.bing.com/search?q=%s",
                                                @"http://www.bing.com/"));
            // Dictionary
            _search_engines.Add(new SearchEngine("Dictionary",
                                                @"http://www.dictionary.com/",
                                                @"http://www.dictionary.com/browse/%s",
                                                @"http://www.iconarchive.com/icons/dimension-of-deskmod/micro/32/Dictionary-icon.png"));
            // Thesaurus
            _search_engines.Add(new SearchEngine("Thesaurus",
                                                @"http://thesaurus.com/",
                                                @"http://thesaurus.reference.com/browse/%s",
                                                @"http://www.iconarchive.com/icons/dimension-of-deskmod/micro/32/Dictionary-icon.png"));
            // MSDN
            _search_engines.Add(new SearchEngine("MSDN",
                                                @"http://msdn.microsoft.com/",
                                                @"http://search.msdn.microsoft.com/search/default.aspx?siteId=0&tab=0&query=%s",
                                                @"http://msdn.microsoft.com/"));
            // Amazon
            _search_engines.Add(new SearchEngine("Amazon",
                                                @"http://www.amazon.com/",
                                                @"http://www.amazon.com/gp/search?keywords=%s&index=blended",
                                                @"http://www.amazon.com/"));
            // Facebook
            _search_engines.Add(new SearchEngine("Facebook",
                                                @"http://www.facebook.com/",
                                                @"http://www.facebook.com/search/?ref=search&q=%s",
                                                @"http://www.iconarchive.com/icons/fasticon/web-2/32/FaceBook-icon.png"));
            // Twitter
            _search_engines.Add(new SearchEngine("Twitter",
                                                @"http://twitter.com/",
                                                @"http://twitter.com/#search?q=%s",
                                                @"http://www.iconarchive.com/icons/fasticon/web-2/32/Twitter-icon.png"));
            // Flickr
            _search_engines.Add(new SearchEngine("Flickr",
                                                @"http://www.flickr.com/",
                                                @"http://www.flickr.com/search/?q=%s",
                                                @"http://www.iconarchive.com/icons/fasticon/web-2/32/Flickr-icon.png"));
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
