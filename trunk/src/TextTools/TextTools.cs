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
using System.Windows.Forms;
using System.Xml.Serialization;
using SystemCore.CommonTypes;
using Configurator;
using ContextLib;

namespace TextTools
{
    [AutomatorPlugin("TextTools: modifies user selected text and inserts quick texts.")]
    public class TextTools : InterpreterPlugin
    {
        #region Properties
        private Icon _insert_icon;
        private Icon _add_icon;
        private Command _add_command;
        private Command _to_lower_command;
        private Command _to_upper_command;
        private Command _sort_command;
        private Command _insert_command;
        private List<string> _quick_text_names;
        private List<QuickText> _quick_texts;
        #endregion

        #region Accessors
        public List<string> QuickTextNames { get { return _quick_text_names; } set { _quick_text_names = value; } }
        public List<QuickText> QuickTexts { get { return _quick_texts; } set { _quick_texts = value; } }
        #endregion

        #region Constructors
        public TextTools()
            : base("Modifies selected text and inserts quick texts.")
        {
            _configurable = true;
            _add_icon = Properties.Resources.add;
            _insert_icon = Properties.Resources.insert;
        }
        #endregion

        #region Public Methods
        public void LoadSettings()
        {
            LoadDefaultQuickTexts();
            string file = CommonInfo.UserFolder + Name + ".xml";
            if (File.Exists(file))
            {
                List<string> qtextnames = new List<string>();
                List<QuickText> qtexts = new List<QuickText>();
                XmlSerializer serializer = null;
                TextReader reader = null;
                try
                {
                    serializer = new XmlSerializer(typeof(List<QuickText>));
                    reader = new StreamReader(file, Encoding.Default);
                    qtexts = (List<QuickText>)serializer.Deserialize(reader);
                }
                catch
                {
                    reader.Dispose();
                    return;
                }
                if (qtexts != null)
                {
                    foreach (QuickText qtext in qtexts)
                    {
                        qtextnames.Add(qtext.Name);
                        qtext.Text = qtext.Text.Replace("\n", Environment.NewLine);
                    }
                    _quick_text_names = qtextnames;
                    _quick_texts = qtexts;
                }
                reader.Dispose();
            }
        }

        public void SaveSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<QuickText>));
            TextWriter writer = new StreamWriter(CommonInfo.UserFolder + Name + ".xml", false, Encoding.Default);
            serializer.Serialize(writer, _quick_texts);
            writer.Close();
        }

        public override void Configure()
        {
            ConfigDialog cd = new ConfigDialog(this);
            if (cd.ShowDialog() == DialogResult.OK)
            {
                SaveSettings();
            }
            cd.Dispose();
        }
        #endregion

        #region Private Methods
        private void LoadDefaultQuickTexts()
        {
            _quick_text_names = new List<string>();
            _quick_texts = new List<QuickText>();
        }

        private Insertion BuildInsertion(string parameters)
        {
            List<string> text = new List<string>(parameters.Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries));
            text.RemoveAll(delegate(string s)
            {
                if (s.Trim() == string.Empty)
                    return true;
                else
                    return false;
            });

            Insertion insertion;
            if (text.Count >= 2)
            {
                insertion = new Insertion(text[0], text[1]);
            }
            else if (text.Count == 1)
            {
                insertion = new Insertion(text[0]);
            }
            else
            {
                insertion = new Insertion();
            }
            return insertion;
        }
        #endregion

        #region Overrided Methods
        protected override void SetupCommands()
        {
            LoadSettings();

            // Build the Add command
            _add_command = new Command("Add New Quick Text", "Add a new quick text.");
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
                QuickTextPicker ep = new QuickTextPicker(this);
                if (ep.ShowDialog() == DialogResult.OK)
                    SaveSettings();
                ep.Dispose();
            }));
            Commands.Add(_add_command);

            // Build the to lower command
            _to_lower_command = new Command("To Lower", "Convert selected text to lower case.");
            _to_lower_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            _to_lower_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _to_lower_command.Name;
            }));
            _to_lower_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _to_lower_command.Description;
            }));
            _to_lower_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _to_lower_command.Name;
            }));
            _to_lower_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _insert_icon.ToBitmap();
            }));
            _to_lower_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(_to_lower_command.Name, args, comp);
            }));
            _to_lower_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
            {
                UserContext.Instance.SelectedTextToLower(true);
            }));
            Commands.Add(_to_lower_command);

            // Build the to upper command
            _to_upper_command = new Command("To Upper", "Convert selected text to upper case.");
            _to_upper_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            _to_upper_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _to_upper_command.Name;
            }));
            _to_upper_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _to_upper_command.Description;
            }));
            _to_upper_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _to_upper_command.Name;
            }));
            _to_upper_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _insert_icon.ToBitmap();
            }));
            _to_upper_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(_to_upper_command.Name, args, comp);
            }));
            _to_upper_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
            {
                UserContext.Instance.SelectedTextToUpper(true);
            }));
            Commands.Add(_to_upper_command);

            // Build the sort command
            _sort_command = new Command("Sort Text", "Sort selected text.");
            _sort_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            _sort_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _sort_command.Name;
            }));
            _sort_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _sort_command.Description;
            }));
            _sort_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _sort_command.Name;
            }));
            _sort_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _insert_icon.ToBitmap();
            }));
            _sort_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(_sort_command.Name, args, comp);
            }));
            _sort_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
            {
                UserContext.Instance.SelectedTextSortLines(true);
            }));
            Commands.Add(_sort_command);

            // Build the insert command
            _insert_command = new Command("Insert");
            _insert_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            _insert_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                Insertion insertion = BuildInsertion(parameters);
                if (insertion.Text == string.Empty)
                    return "Insert clipboard...";
                else
                    return "Insert " + insertion.Text;
            }));
            _insert_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                Insertion insertion = BuildInsertion(parameters);
                if (insertion.Target == string.Empty)
                {
                    return "... into top window.";
                }
                else
                {
                    return "... into " +insertion.Target;
                }
            }));
            _insert_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                string auto = _insert_command.Name;
                Insertion insertion = BuildInsertion(parameters);
                if (insertion.Text != string.Empty)
                    auto += " " + insertion.Text;
                if (insertion.Target != string.Empty)
                    auto += " " + insertion.Target;
                return auto;
            }));
            _insert_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _insert_icon.ToBitmap();
            }));
            _insert_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>(new string[] { "\"text\"", "\"file\""});
                Dictionary<string, bool> comp = new Dictionary<string, bool>();
                foreach (string arg in args)
                    comp.Add(arg, false);

                Insertion insertion = BuildInsertion(parameters);
                if (insertion.Text != string.Empty)
                    comp["\"text\""] = true;
                if (insertion.Target != string.Empty)
                    comp["\"file\""] = true;

                return new CommandUsage(_insert_command.Name, args, comp);
            }));
            _insert_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
            {
                Insertion insertion = BuildInsertion(parameters);
                string text = insertion.Text;
                if (text == string.Empty)
                {
                    ContextLib.DataContainers.Multimedia.MultiLevelData data = UserContext.Instance.GetClipboardContent();
                    if (data.Text != null)
                        text = data.Text;
                    data.Dispose();
                }
                if (insertion.Target == string.Empty)
                {
                    UserContext.Instance.InsertText(text, true);
                }
                else
                {
                    StreamWriter writer = new StreamWriter(insertion.Target, true);
                    writer.WriteLine(text);
                    writer.Close();
                }
            }));
            Commands.Add(_insert_command);

            // Build user quick text commands
            foreach (QuickText quick_text in _quick_texts)
            {
                QuickText qtext = new QuickText(quick_text);
                Command cmd = new Command("Insert " + qtext.Name);

                cmd.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
                {
                    return true;
                }));
                cmd.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return "Insert " + qtext.Name;
                }));
                cmd.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return "Insert quick text in active window.";
                }));
                cmd.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return "Insert " + qtext.Name;
                }));
                cmd.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
                {
                    return _insert_icon.ToBitmap();
                }));
                cmd.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
                {
                    List<string> args = new List<string>();
                    Dictionary<string, bool> comp = new Dictionary<string, bool>();

                    return new CommandUsage(cmd.Name, args, comp);
                }));
                cmd.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
                {
                    UserContext.Instance.InsertText(qtext.Text, true);
                }));
                _commands.Add(cmd);
            }
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
