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

namespace WebSearch
{
    [AutomatorPlugin("WebEngine: Opens URLs and searches the web.")]
    public class Web : InterpreterPlugin
    {
        #region Properties
        private Regex _regex;
        private Regex _wwwRegex;
        private List<string> _search_engine_names;
        private List<SearchEngine> _search_engines;
        private int _favorite_engine;
        private Icon _browser_icon;
        private Icon _search_icon;
        private Icon _add_icon;
        private Command _add_command;
        private Command _process_url_command;
        #endregion

        #region Accessors
        public Regex UrlRegex
        {
            get { return _regex; }
            set { _regex = value; }
        }

        public List<string> SearchEngineNames
        {
            get { return _search_engine_names; }
            set { _search_engine_names = value; }
        }

        public List<SearchEngine> SearchEngines
        {
            get { return _search_engines; }
            set { _search_engines = value; }
        }

        public int FavoriteEngine
        {
            get { return _favorite_engine; }
            set { _favorite_engine = value; }
        }

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
        }
        #endregion

        #region Public Methods

        //public override void OnBuild()
        //{
            
        //}

        //public override bool IsOwner(string cmd)
        //{
        //    return base.IsOwner(cmd) || CompIsOwner(cmd); //_search_engine_names.Contains(item)
        //}

        //public override string GetItemName(string cmd, string item)
        //{
        //    if (base.IsOwner(item))
        //    {
        //        return item;
        //    }
        //    else if (cmd == _add_command.Name)
        //    {
        //        return _add_command.Name;
        //    }
        //    else
        //    {
        //        SearchEngine engine = AssistingSearchEngine(cmd);
        //        if (engine != null)
        //        {
        //            string term = RetrieveSearchQuery(cmd, item);
        //            if (term == string.Empty) // no search term
        //            {
        //                return engine.Name;
        //            }
        //            else
        //            {
        //                return engine.Name + " " + term;
        //            }
        //        }
        //        else
        //        {
        //            return item;
        //        }
        //    }
        //}

        //public override string GetItemDescription(string cmd, string item)
        //{
        //    if (base.IsOwner(item))
        //    {
        //        return "Browse to " + item;
        //    }
        //    else if (cmd == _add_command.Name)
        //    {
        //        return _add_command.Description;
        //    }
        //    else 
        //    {
        //        SearchEngine engine = AssistingSearchEngine(cmd);
        //        if (engine != null)
        //        {
        //            string term = RetrieveSearchQuery(cmd, item);
        //            if (term == string.Empty || engine.SearchQuery == string.Empty) // no search term or no search query was specified
        //            {
        //                return "Go to " + engine.Name + " web page.";
        //            }
        //            else
        //            {
        //                return "Search for " + term + " on " + engine.Name;
        //            }
        //        }
        //        else
        //        {
        //            return "Search for " + item + " on " + _search_engines[_favorite_engine].Name;
        //        }
        //    }
        //}

        //public override string GetItemAutoComplete(string cmd, string item)
        //{
        //    if (base.IsOwner(item))
        //    {
        //        return item;
        //    }
        //    else if (cmd == _add_command.Name)
        //    {
        //        return _add_command.Name;
        //    }
        //    else
        //    {
        //        SearchEngine engine = AssistingSearchEngine(cmd);
        //        if (engine != null)
        //        {
        //            string term = RetrieveSearchQuery(cmd, item);
        //            if (term == string.Empty) // no search term
        //            {
        //                return engine.Name;
        //            }
        //            else
        //            {
        //                return engine.Name + " " + term;
        //            }
        //        }
        //        else
        //        {
        //            return item;
        //        }
        //    }
        //}

        //public override bool Execute(InterpreterItem item)
        //{
        //    if (base.IsOwner(item.AutoComplete))
        //    {
        //        System.Diagnostics.Process.Start(FixUrl(item.AutoComplete));
        //    }
        //    else if (item.CommandName == _add_command.Name)
        //    {
        //        EnginePicker ep = new EnginePicker(this);
        //        if (ep.ShowDialog() == DialogResult.OK)
        //            SaveSettings();
        //        ep.Dispose();
        //    }
        //    else
        //    {
        //        SearchEngine engine = AssistingSearchEngine(item.CommandName);
        //        try
        //        {
        //            if (engine != null)
        //            {
        //                string term = RetrieveSearchQuery(item.CommandName, item.AutoComplete);
        //                if (term == string.Empty) // no search term
        //                {
        //                    System.Diagnostics.Process.Start(engine.Url);
        //                }
        //                else
        //                {
        //                    System.Diagnostics.Process.Start(engine.GetSearchQuery(term));
        //                }
        //            }
        //            else
        //            {
        //                System.Diagnostics.Process.Start(_search_engines[_favorite_engine].GetSearchQuery(item.AutoComplete));
        //            }
        //        }
        //        catch
        //        {

        //        }
        //    }
        //    return true;
        //}

        //public override Icon GetItemIcon(string cmd, string item)
        //{
        //    if (base.IsOwner(item))
        //        return _browser_icon;
        //    else if (cmd == _add_command.Name)
        //        return _add_icon;
        //    else
        //        return _search_icon;
        //}

        public override void Configure()
        {
            ConfigDialog cd = new ConfigDialog(this);
            if (cd.ShowDialog() == DialogResult.OK)
            {
                SaveSettings();
            }
            cd.Dispose();
        }

        //public override Command GetCommand(InterpreterItem item)
        //{
        //    SearchEngine engine;
        //    if (item.CommandName == _add_command.Name)
        //        return _add_command;
        //    else if ((engine = AssistingSearchEngine(item.CommandName)) != null)
        //    {
        //        foreach (Command cmd in _commands)
        //            if (cmd.Name == engine.Name)
        //                return cmd;
        //        return null;
        //    }
        //    else
        //        return null;
        //}

        public void LoadSettings()
        {
            LoadDefaultEngines();
            string file = CommonInfo.UserFolder + Name + ".xml";
            if (File.Exists(file))
            {
                List<SearchEngine> search_engines = new List<SearchEngine>();
                List<string> search_engine_names = new List<string>();
                int favorite = 0;
                XmlSerializer serializer = null;
                TextReader reader = null;
                try
                {
                    serializer = new XmlSerializer(typeof(List<SearchEngine>));
                    reader = new StreamReader(file, Encoding.Default);
                    search_engines = (List<SearchEngine>)serializer.Deserialize(reader);
                }
                catch
                {
                    reader.Dispose();
                    return;
                }
                if (search_engines != null)
                {
                    for (int i = 0; i < search_engines.Count; i++)
                    {
                        search_engine_names.Add(search_engines[i].Name);
                        if (search_engines[i].IsFavorite)
                            favorite = i;
                    }

                    _search_engine_names = search_engine_names;
                    _search_engines = search_engines;
                    _favorite_engine = favorite;
                }
                reader.Dispose();
            }
            //List<string> categories = INIManipulator.GetCategories(CommonInfo.UserConfigFile);
            //if (categories.Count > 0)
            //{
            //    string category = Name;
            //    if (categories.Contains(category))
            //    {
            //        List<string> keys = INIManipulator.GetKeys(CommonInfo.UserConfigFile, category);
            //        int key_len = keys.Count;
            //        if (key_len > 1)
            //        {
            //            for (int i = 0; i < key_len; i += 3)
            //            {
            //                string name;
            //                string url;
            //                string term;
            //                try
            //                {
            //                    if (i == key_len - 1)
            //                    {
            //                        Int32.TryParse(INIManipulator.GetValue(CommonInfo.UserConfigFile, category, keys[i], "0"), out favorite);
            //                    }
            //                    else
            //                    {
            //                        name = INIManipulator.GetValue(CommonInfo.UserConfigFile, category, keys[i], "");
            //                        url = INIManipulator.GetValue(CommonInfo.UserConfigFile, category, keys[i + 1], "");
            //                        term = INIManipulator.GetValue(CommonInfo.UserConfigFile, category, keys[i + 2], "");
            //                        search_engine_names.Add(name);
            //                        search_engines.Add(new SearchEngine(name, url, term));
            //                    }
            //                }
            //                catch (Exception)
            //                {
            //                    return;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    return;
            //}
        }

        public void SaveSettings(/*List<string> names, List<SearchEngine> engines, int fav*/)
        {
            //_search_engine_names = names;
            //_search_engines = engines;
            //_favorite_engine = fav;
            //string category = Name;
            //int len = _search_engine_names.Count+1;
            //INIManipulator.DeleteCategory(CommonInfo.UserConfigFile, category);
            //for (int i = 0; i < len; i++)
            //{
            //    int pos = i + 1;
            //    if (i == len-1)
            //    {
            //        INIManipulator.WriteValue(CommonInfo.UserConfigFile, category, "default", _favorite_engine.ToString());
            //    }
            //    else
            //    {
            //        INIManipulator.WriteValue(CommonInfo.UserConfigFile, category, pos.ToString() + "\\name", _search_engines[i].Name);
            //        INIManipulator.WriteValue(CommonInfo.UserConfigFile, category, pos.ToString() + "\\url", _search_engines[i].Url);
            //        INIManipulator.WriteValue(CommonInfo.UserConfigFile, category, pos.ToString() + "\\query", _search_engines[i].SearchQuery);
            //    }
            //}
            _search_engines[_favorite_engine].IsFavorite = true;
            XmlSerializer serializer = new XmlSerializer(typeof(List<SearchEngine>));
            TextWriter writer = new StreamWriter(CommonInfo.UserFolder + Name + ".xml", false, Encoding.Default);
            serializer.Serialize(writer, _search_engines);
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
                        int dist = LevenshteinMeasurer.Instance.GetDistance(text[i], token);
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
            //List<string> text = new List<string>(item.ToLower().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            //int min = Int32.MaxValue;
            //SearchEngine min_search_engine = null;

            //if (text.Count > 0)
            //{
            //    for (int i = 0; i < text.Count; i++)
            //    {
            //        foreach (SearchEngine engine in _search_engines)
            //        {
            //            if (StringUtility.WordContainsWord(text[i], engine.Name))
            //            {
            //                int dist = LevenshteinMeasurer.Instance.GetDistance(text[i], engine.Name);
            //                if (dist < min)
            //                {
            //                    min = dist;
            //                    min_search_engine = engine;
            //                }
            //            }
            //        }
            //    }
            //}
            //if (min_search_engine != null)
            //    return min_search_engine;
            //else
            //    return null;
            foreach (SearchEngine engine in _search_engines)
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

        private void LoadDefaultEngines()
        {
            _search_engines = new List<SearchEngine>();
            _search_engine_names = new List<string>();
            //// add search engines
            // Google
            _search_engine_names.Add("Google");
            _search_engines.Add(new SearchEngine("Google", @"http://www.google.com/ncr", @"http://www.google.com/search?hl=en&q=%s&btnG=Google+Search&aq=f&oq="));
            _favorite_engine = 0;
            // Youtube
            _search_engine_names.Add("YouTube");
            _search_engines.Add(new SearchEngine("YouTube", @"http://www.youtube.com/", @"http://www.youtube.com/results?search_query=%s"));
            // Wikipedia
            _search_engine_names.Add("Wikipedia");
            _search_engines.Add(new SearchEngine("Wikipedia", @"http://www.wikipedia.org/", @"http://en.wikipedia.org/wiki/Special:Search?search=%s&go=Go"));
            //old: http://en.wikipedia.org/wiki/Special:Search?search=%s&fulltext=Search
            // IMDB
            _search_engine_names.Add("IMDB");
            _search_engines.Add(new SearchEngine("IMDB", @"http://www.imdb.com/", @"http://www.imdb.com/find?s=all&q=%s"));
            // Yahoo
            _search_engine_names.Add("Yahoo");
            _search_engines.Add(new SearchEngine("Yahoo", @"http://search.yahoo.com/", @"http://search.yahoo.com/search?p=%s"));
            // Weather
            _search_engine_names.Add("Weather");
            _search_engines.Add(new SearchEngine("Weather", @"http://www.weather.com/", @"http://www.weather.com/weather/local/%s"));
            // Maps
            _search_engine_names.Add("Maps");
            _search_engines.Add(new SearchEngine("Maps", @"http://maps.google.com/", @"http://maps.google.com/maps?f=q&hl=en&geocode=&q=%s&ie=UTF8&z=12&iwloc=addr&om=1"));
            // MSN
            _search_engine_names.Add("MSN");
            _search_engines.Add(new SearchEngine("MSN", @"http://search.msn.com/", @"http://search.msn.com/results.aspx?q=%s"));
            // Live Search
            _search_engine_names.Add("Live Search");
            _search_engines.Add(new SearchEngine("Live Search", @"http://search.live.com/", @"http://search.live.com/results.aspx?q=%s"));
            //Dictionary
            _search_engine_names.Add("Dictionary");
            _search_engines.Add(new SearchEngine("Dictionary", @"http://www.dictionary.com/", @"http://www.dictionary.com/browse/%s"));
        }
        #endregion

        #region Overrided Methods
        protected override void SetupCommands()
        {
            LoadSettings();
            // create search commands
            // problema com os delegates e ciclos: http://decav.com/blogs/andre/archive/2007/11/18/wtf-quot-problems-quot-with-anonymous-delegates-linq-lambdas-and-quot-foreach-quot-or-quot-for-quot-loops.aspx
            foreach (SearchEngine engine in _search_engines)
            {
                SearchEngine eng = new SearchEngine(engine);
                Command cmd;
                if (eng.Name != _search_engine_names[_favorite_engine])
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
                    return _search_icon.ToBitmap();
                    //string iconURL = "http://www.google.com/s2/favicons?domain_url=" + eng.Url;
                    //System.Net.HttpWebRequest wr = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(iconURL);
                    //System.Net.HttpWebResponse ws = (System.Net.HttpWebResponse)wr.GetResponse();
                    //System.IO.Stream stream = ws.GetResponseStream();
                    //Image urlImage = System.Drawing.Image.FromStream(stream);
                    //return Icon.FromHandle(((Bitmap)urlImage).GetHicon());
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
                cmd.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
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
            _add_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
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
            _process_url_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
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
