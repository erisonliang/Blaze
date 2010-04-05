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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction.StringUtilities;
using Configurator;
using System.Threading;

namespace WebSearch
{
    [AutomatorPlugin("WebEngine: Opens URLs and searches the web.")]
    public class Web : InterpreterPlugin
    {
        #region Properties
        private Regex _regex;
        private Regex _wwwRegex;
        //private List<string> _search_engine_names;
        //private List<SearchEngine> _search_engines;
        //private int _favorite_engine;
        private Icon _browser_icon;
        private Icon _search_icon;
        private Icon _add_icon;
        private Command _add_command;
        private Command _process_url_command;
        private IconWebCache _icon_cache;
        //private int _cache_ttl;
        private string _settings_file_path;
        WebSearchConfig _config;
        #endregion

        #region Accessors
        public Regex UrlRegex
        {
            get { return _regex; }
            set { _regex = value; }
        }

        //public List<string> SearchEngineNames
        //{
        //    get { return _search_engine_names; }
        //    set { _search_engine_names = value; }
        //}

        public List<SearchEngine> SearchEngines
        {
            get { return _config.SearchEngines; }
            set { _config.SearchEngines = value; }
        }

        public int FavoriteEngine
        {
            get { return _config.FavoriteEngine; }
            set { _config.FavoriteEngine = value; }
        }

        public int CacheTTL
        {
            get { return _icon_cache.GetTTLhours(); }
            set { _config.CacheTTL = value; _icon_cache.SetTTLhours(value); }
        }

        public IconWebCache IconCache
        {
            get { return _icon_cache; }
        }
        #endregion

        #region Constructors
        public Web()
            : base("") 
        {
            _configurable = true;
            _description = "Opens website URLs and searches for terms in" + Environment.NewLine + "various web search engines.";
            _description += Environment.NewLine + Environment.NewLine +
                            "Press configure to check and/or change the list of"+Environment.NewLine+
                            "available web search engines.";
            _regex = new Regex(@"^((ftp|http|https|gopher|mailto|news|nntp|telnet|wais|file|prospero|aim|webcal)\://)?(www.|[a-zA-Z0-9].)[a-zA-Z0-9\-\.]+\..*$"); // old @"^(((h|H)(t|T)(t|T)(p|P)(s?|S?))\://)?(www.|[a-zA-Z0-9].)[a-zA-Z0-9\-\.]+\.[a-zA-Z.*]*$"
            _wwwRegex = new Regex(@"^((ftp|http|https|gopher|mailto|news|nntp|telnet|wais|file|prospero|aim|webcal):)");
            _browser_icon = Properties.Resources.browser;
            _search_icon = Properties.Resources.search;
            _add_icon = Properties.Resources.add;
            _icon_cache = new IconWebCache(_config.CacheTTL);
        }
        #endregion

        #region Public Methods

        
        public override void Configure()
        {
            ConfigDialog cd = new ConfigDialog(this);
            if (cd.ShowDialog() == DialogResult.OK)
            {
                SaveSettings();
            }
            cd.Dispose();
        }
       
        public void LoadSettings()
        {
            _settings_file_path = CommonInfo.UserFolder + Name + @"\" + Name + ".xml";
            _config = new WebSearchConfig();
            _config.Reset();
            if (File.Exists(_settings_file_path))
            {
                XmlSerializer serializer = null;
                TextReader reader = null;
                try
                {
                    serializer = new XmlSerializer(typeof(WebSearchConfig));
                    reader = new StreamReader(_settings_file_path, Encoding.Default);
                    _config = (WebSearchConfig)serializer.Deserialize(reader);
                    _icon_cache.SetTTLhours(_config.CacheTTL);
                }
                catch
                {
                    return;
                }
                finally
                {
                    reader.Dispose();
                }
            }
        }

        public void SaveSettings()
        {
            // save search engines
            XmlSerializer serializer = new XmlSerializer(typeof(WebSearchConfig));
            TextWriter writer = new StreamWriter(_settings_file_path, false, Encoding.Default);
            serializer.Serialize(writer, _config);
            writer.Close();
        }
        #endregion

        #region Private Methods
        private string FixUrl(string url)
        {
            if (!_wwwRegex.IsMatch(url))
            {
                return "http://" + url;
            }
            else
            {
                return url;
            }
        }

        private string RetrieveSearchQuery(string cmd, string item)
        {
            List<string> text = new List<string>(item.ToLower().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));

            SearchEngine engine = AssistingSearchEngine(cmd);
            string[] tokens = engine.Name.ToLower().Split(new string[] { " ", "_", "-", "(", ")", "[", "]", "{", "}" }, StringSplitOptions.RemoveEmptyEntries);

            // discover what tokens represent a search engine
            foreach (string token in tokens)
            {
                int min = Int32.MaxValue;
                int min_pos = -1;
                for (int i = 0; i < text.Count; i++)
                {
                    if (StringUtility.WordContainsWord(text[i], token))
                    {
                        int dist = EditDistanceMeasurer.LevenshteinDistance(text[i], token);
                        if (dist < min)
                        {
                            min = dist;
                            min_pos = i;
                        }
                    }
                }
                // delete the token
                if (min_pos != -1)
                    text.RemoveAt(min_pos);
            }

            // build the query
            string content = string.Empty;
            for (int i = 0; i < text.Count; i++)
            {
                if (i == text.Count - 1)
                {
                    content += text[i];
                }
                else
                {
                    content += text[i] + " ";
                }
            }

            return content;
        }

        private SearchEngine AssistingSearchEngine(string item)
        {
            foreach (SearchEngine engine in SearchEngines)
                if (item == engine.Name)
                    return engine;
            return null;
        }

        private bool CompIsOwner(string item)
        {
            if (item == _add_command.Name)
                return true;
            foreach (Command command in Commands)
                if (item.Contains(command.Name))
                    return true;
            return false;
        }

        //private void LoadDefaultEngines()
        //{
        //    _search_engines = new List<SearchEngine>();
        //    _search_engine_names = new List<string>();
        //    //// add search engines
        //    // Google
        //    _search_engine_names.Add("Google");
        //    _search_engines.Add(new SearchEngine("Google",
        //                                        @"http://www.google.com/ncr",
        //                                        @"http://www.google.com/search?hl=en&q=%s&btnG=Google+Search&aq=f&oq=",
        //                                        @"http://www.iconarchive.com/icons/fasticon/web-2/32/Google-icon.png"));
        //    _favorite_engine = 0;
        //    // Youtube
        //    _search_engine_names.Add("YouTube");
        //    _search_engines.Add(new SearchEngine("YouTube",
        //                                        @"http://www.youtube.com/",
        //                                        @"http://www.youtube.com/results?search_query=%s",
        //                                        @"http://www.iconarchive.com/icons/fasticon/web-2/32/Youtube-icon.png"));
        //    // Wikipedia
        //    _search_engine_names.Add("Wikipedia");
        //    _search_engines.Add(new SearchEngine("Wikipedia",
        //                                        @"http://www.wikipedia.org/",
        //                                        @"http://en.wikipedia.org/wiki/Special:Search?search=%s&go=Go",
        //                                        @"http://www.iconarchive.com/icons/sykonist/popular-sites/32/Wikipedia-globe-icon.png"));
        //    // IMDB
        //    _search_engine_names.Add("IMDB");
        //    _search_engines.Add(new SearchEngine("IMDB",
        //                                        @"http://www.imdb.com/",
        //                                        @"http://www.imdb.com/find?s=all&q=%s",
        //                                        @"http://www.iconarchive.com/icons/iconshock/cinema/32/film-reel-icon.png"));
        //    // Yahoo
        //    _search_engine_names.Add("Yahoo");
        //    _search_engines.Add(new SearchEngine("Yahoo",
        //                                        @"http://search.yahoo.com/",
        //                                        @"http://search.yahoo.com/search?p=%s",
        //                                        @"http://www.iconarchive.com/icons/deleket/puck/32/Yahoo-Messenger-Alternate-icon.png"));
        //    // Weather
        //    _search_engine_names.Add("Weather");
        //    _search_engines.Add(new SearchEngine("Weather",
        //                                        @"http://www.weather.com/",
        //                                        @"http://www.weather.com/weather/today/%s",
        //                                        @"http://www.iconarchive.com/icons/icons-land/weather/32/Snow-Occasional-icon.png"));
        //    // Maps
        //    _search_engine_names.Add("Maps");
        //    _search_engines.Add(new SearchEngine("Maps",
        //                                        @"http://maps.google.com/",
        //                                        @"http://maps.google.com/maps?f=q&hl=en&geocode=&q=%s&ie=UTF8&z=12&iwloc=addr&om=1",
        //                                        @"http://www.iconarchive.com/icons/walrick/openphone/32/Maps-icon.png"));
        //    // Bing
        //    _search_engine_names.Add("Bing");
        //    _search_engines.Add(new SearchEngine("Bing",
        //                                        @"http://www.bing.com/",
        //                                        @"http://www.bing.com/search?q=%s",
        //                                        @"http://www.bing.com/"));
        //    // Dictionary
        //    _search_engine_names.Add("Dictionary");
        //    _search_engines.Add(new SearchEngine("Dictionary",
        //                                        @"http://www.dictionary.com/",
        //                                        @"http://www.dictionary.com/browse/%s",
        //                                        @"http://www.iconarchive.com/icons/dimension-of-deskmod/micro/32/Dictionary-icon.png"));
        //    // Thesaurus
        //    _search_engine_names.Add("Thesaurus");
        //    _search_engines.Add(new SearchEngine("Thesaurus",
        //                                        @"http://thesaurus.com/",
        //                                        @"http://thesaurus.reference.com/browse/%s",
        //                                        @"http://www.iconarchive.com/icons/dimension-of-deskmod/micro/32/Dictionary-icon.png"));
        //    // MSDN
        //    _search_engine_names.Add("MSDN");
        //    _search_engines.Add(new SearchEngine("MSDN",
        //                                        @"http://msdn.microsoft.com/",
        //                                        @"http://search.msdn.microsoft.com/search/default.aspx?siteId=0&tab=0&query=%s",
        //                                        @"http://msdn.microsoft.com/"));
        //    // Amazon
        //    _search_engine_names.Add("Amazon");
        //    _search_engines.Add(new SearchEngine("Amazon",
        //                                        @"http://www.amazon.com/",
        //                                        @"http://www.amazon.com/gp/search?keywords=%s&index=blended",
        //                                        @"http://www.amazon.com/"));
        //    // Facebook
        //    _search_engine_names.Add("Facebook");
        //    _search_engines.Add(new SearchEngine("Facebook",
        //                                        @"http://www.facebook.com/",
        //                                        @"http://www.facebook.com/search/?ref=search&q=%s",
        //                                        @"http://www.iconarchive.com/icons/fasticon/web-2/32/FaceBook-icon.png"));
        //    // Twitter
        //    _search_engine_names.Add("Twitter");
        //    _search_engines.Add(new SearchEngine("Twitter",
        //                                        @"http://twitter.com/",
        //                                        @"http://twitter.com/#search?q=%s",
        //                                        @"http://www.iconarchive.com/icons/fasticon/web-2/32/Twitter-icon.png"));
        //    // Flickr
        //    _search_engine_names.Add("Flickr");
        //    _search_engines.Add(new SearchEngine("Flickr",
        //                                        @"http://www.flickr.com/",
        //                                        @"http://www.flickr.com/search/?q=%s",
        //                                        @"http://www.iconarchive.com/icons/fasticon/web-2/32/Flickr-icon.png"));
        //}
        #endregion

        #region Overrided Methods
        protected override void SetupCommands()
        {
            LoadSettings();
            // create search commands
            // problema com os delegates e ciclos: http://decav.com/blogs/andre/archive/2007/11/18/wtf-quot-problems-quot-with-anonymous-delegates-linq-lambdas-and-quot-foreach-quot-or-quot-for-quot-loops.aspx
            foreach (SearchEngine engine in SearchEngines)
            {
                SearchEngine eng = new SearchEngine(engine);
                Command cmd;
                if (eng.Name != SearchEngines[FavoriteEngine].Name)
                    cmd = new Command(eng.Name);
                else
                    cmd = new Command(eng.Name, Command.PriorityType.Medium | Command.PriorityType.Low);

                cmd.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
                {
                    return true;
                }));
                cmd.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    if (parameters.Trim() == string.Empty)
                        return eng.Name;
                    else
                        return eng.Name + " " + parameters;
                }));
                cmd.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    if (parameters == string.Empty || eng.SearchQuery == string.Empty) // no search term or no search query was specified
                    {
                        return "Go to " + eng.Name + " web page.";
                    }
                    else
                    {
                        return "Search for " + parameters + " on " + eng.Name;
                    }
                }));
                cmd.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    if (parameters == string.Empty)
                    {
                        return eng.Name;
                    }
                    else
                    {
                        return eng.Name + " " + parameters;
                    }
                }));
                cmd.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
                {
                    //return _search_icon.ToBitmap();
                    return _icon_cache.GetIcon(eng);
                }));
                cmd.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
                {
                    List<string> args = new List<string>();
                    Dictionary<string, bool> comp = new Dictionary<string, bool>();

                    args.Add("term");
                    if (parameters != string.Empty)
                        comp.Add("term", true);
                    else
                        comp.Add("term", false);

                    return new CommandUsage(cmd.Name, args, comp);
                }));
                cmd.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
                {
                    try
                    {
                        if (parameters == string.Empty) // no search term
                        {
                            System.Diagnostics.Process.Start(eng.Url);
                        }
                        else
                        {
                            System.Diagnostics.Process.Start(eng.GetSearchQuery(parameters));
                        }
                    }
                    catch
                    {

                    }
                }));
                _commands.Add(cmd);
            }

            //_add_command = new Command("Add New Website", new string[] { "add", "new", "website", "search", "engine" }, "Add a new search engine.", Command.PriorityType.Medium);
            _add_command = new Command("Add New Website", "Add a new search engine.", Command.PriorityType.Medium);
            _add_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            _add_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _add_command.Name;
            }));
            _add_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _add_command.Description;
            }));
            _add_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _add_command.Name;
            }));
            _add_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _add_icon.ToBitmap();
            }));
            _add_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(_add_command.Name, args, comp);
            }));
            _add_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                EnginePicker ep = new EnginePicker(this);
                if (ep.ShowDialog() == DialogResult.OK)
                    SaveSettings();
                ep.Dispose();
            }));

            _process_url_command = new Command("Process URL", Command.PriorityType.High);
            _process_url_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                //Regex regex = new Regex(@"^(((h|H)(t|T)(t|T)(p|P)(s?|S?))\://)?(www.|[a-zA-Z0-9].)[a-zA-Z0-9\-\.]+\..*$");
                return _regex.IsMatch(parameters);
            }));
            _process_url_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return parameters;
            }));
            _process_url_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return "Browse to " + parameters;
            }));
            _process_url_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return parameters;
            }));
            _process_url_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _browser_icon.ToBitmap();
            }));
            _process_url_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(@"Valid URL. i.e.: http://www.google.com", args, comp);
            }));
            _process_url_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                try
                {
                    System.Diagnostics.Process.Start(FixUrl(parameters));
                }
                catch
                {

                }
            }));

            _commands.Add(_add_command);
            _commands.Add(_process_url_command);
        }

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
