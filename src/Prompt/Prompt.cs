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
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction.FileHandling;
using Configurator;
using ContextLib;

namespace Prompt
{
    [AutomatorPlugin("Allows you run Command Prompt commands.")]
    public class Prompt : InterpreterPlugin
    {
        #region Properties
        private Icon _prompt_icon;
        private Icon _add_icon;
        private Command _run_command;
        private Command _runc_command;
        private Command _add_command;
        private List<string> _prompt_command_names;
        private List<PromptCommand> _pompt_commands;
        #endregion

        #region Accessors
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
        public List<string> PromptCommandNames { get { return _prompt_command_names; } set { _prompt_command_names = value; } }
        public List<PromptCommand> PromptCommands { get { return _pompt_commands; } set { _pompt_commands = value; } }
        #endregion

        #region Constructors
        public Prompt()
            : base("Allows you run Command Prompt commands and " + Environment.NewLine + "custom commands.")
        {
            _configurable = true;
            _description += Environment.NewLine + Environment.NewLine +
                            "E.g.: > ping www.google.pt";
            _prompt_icon = Properties.Resources.prompt;
            _add_icon = Properties.Resources.add;
        }
        #endregion

        #region Public Methods
        public void LoadSettings()
        {
            LoadDefaultCommands();
            List<PromptCommand> prompt_commands = new List<PromptCommand>();
            List<string> prompt_command_names = new List<string>();
            List<string> categories = INIManipulator.GetCategories(CommonInfo.UserConfigFile);
            if (categories.Count > 0)
            {
                string category = Name;
                if (categories.Contains(category))
                {
                    List<string> keys = INIManipulator.GetKeys(CommonInfo.UserConfigFile, category);
                    int key_len = keys.Count;
                    if (key_len > 1)
                    {
                        for (int i = 0; i < key_len; i += 3)
                        {
                            string name;
                            string path;
                            string arguments;
                            try
                            {
                                name = INIManipulator.GetValue(CommonInfo.UserConfigFile, category, keys[i], "");
                                path = INIManipulator.GetValue(CommonInfo.UserConfigFile, category, keys[i + 1], "");
                                arguments = INIManipulator.GetValue(CommonInfo.UserConfigFile, category, keys[i + 2], "");
                                prompt_command_names.Add(name);
                                prompt_commands.Add(new PromptCommand(name, path, arguments));
                            }
                            catch (Exception)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
            _prompt_command_names = prompt_command_names;
            _pompt_commands = prompt_commands;
        }

        public void SaveSettings()
        {
            string category = Name;
            int len = _prompt_command_names.Count;
            INIManipulator.DeleteCategory(CommonInfo.UserConfigFile, category);
            for (int i = 0; i < len; i++)
            {
                int pos = i + 1;
                INIManipulator.WriteValue(CommonInfo.UserConfigFile, category, pos.ToString() + "\\name", _pompt_commands[i].Name);
                INIManipulator.WriteValue(CommonInfo.UserConfigFile, category, pos.ToString() + "\\path", _pompt_commands[i].Path);
                INIManipulator.WriteValue(CommonInfo.UserConfigFile, category, pos.ToString() + "\\arguments", _pompt_commands[i].Arguments);
            }
        }
        //public override bool IsOwner(string cmd)
        //{
        //    string s = cmd.ToLower().Trim();
        //    if (s.Length > 0)
        //    {
        //        if (s[0] == '>')
        //            return true;
        //        else 
        //            return false;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public override void OnBuild()
        //{

        //}

        //public override string GetItemName(string cmd, string item)
        //{
        //    return "Run command";
        //}

        //public override string GetItemDescription(string cmd, string item)
        //{
        //    return BuildCommand(item);
        //}

        //public override string GetItemAutoComplete(string cmd, string item)
        //{
        //    return item;
        //}

        //public override Icon GetItemIcon(string cmd, string input)
        //{
        //    if (IsOwner(input))
        //        return _icon;
        //    else
        //        return null;
        //}

        //public override bool Execute(InterpreterItem item)
        //{
        //    ProcessStartInfo info = new ProcessStartInfo(Environment.GetEnvironmentVariable("COMSPEC"));
        //    if (UserContext.Instance.IsWindowsExplorerOnTop)
        //        info.WorkingDirectory = UserContext.Instance.GetExplorerPath(true);
        //    info.Arguments = "/K " + BuildCommand(item.AutoComplete);
        //    info.UseShellExecute = true;
        //    info.ErrorDialog = true;
        //    System.Diagnostics.Process.Start(info);
        //    info = null;
        //    return true;
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
        //    return null;
        //}
        #endregion

        #region Private Methods
        private string BuildCommand(string item)
        {
            string cmd = item.Substring(1).Trim();                                                                                              
            if (cmd.Length > 0 && cmd[cmd.Length - 1] == '&')
                return "/C " + cmd.Substring(0, cmd.Length - 1);
            else
                return "/K " + cmd;
        }

        private string BuildCommandDscpr(string item)
        {
            string cmd = item.Substring(1).Trim();
            if (cmd.Length > 0 && cmd[cmd.Length - 1] == '&')
                return cmd.Substring(0, cmd.Length - 1);
            else
                return cmd;
        }

        private bool RunAndClose(string item)
        {
            string cmd = item.Substring(1).Trim();
            if (cmd.Length > 0 && cmd[cmd.Length - 1] == '&')
                return true;
            else
                return false;
        }

        private void LoadDefaultCommands()
        {
            _prompt_command_names = new List<string>();
            _pompt_commands = new List<PromptCommand>();

            // add command line
            _prompt_command_names.Add("cmd");
            _pompt_commands.Add(new PromptCommand("cmd", Environment.GetEnvironmentVariable("COMSPEC"), "/K $$"));
            _prompt_command_names.Add("tskill");
            _pompt_commands.Add(new PromptCommand("tskill", Environment.GetEnvironmentVariable("COMSPEC"), "/C tskill $$"));
        }
        #endregion

        #region Overrided Methods
        protected override void SetupCommands()
        {
            LoadSettings();
            // Build the Run command
            _run_command = new Command("Prompt", Command.PriorityType.High);
            _run_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                string s = parameters.ToLower().Trim();
                if (s.Length > 0)
                {
                    if (s[0] == '>')
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }));
            _run_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return "Run command" + (RunAndClose(parameters) ? " and close" : string.Empty);
            }));
            _run_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return BuildCommandDscpr(parameters);
            }));
            _run_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return parameters;
            }));
            _run_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _prompt_icon.ToBitmap();
            }));
            _run_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>(new string[] { "command" });
                Dictionary<string, bool> comp = new Dictionary<string, bool>();
                foreach (string arg in args)
                    comp.Add(arg, false);

                if (BuildCommandDscpr(parameters) != string.Empty)
                    comp["command"] = true;

                return new CommandUsage(_run_command.Name, args, comp);
            }));
            _run_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                ProcessStartInfo info = new ProcessStartInfo(Environment.GetEnvironmentVariable("COMSPEC"));
                //if (UserContext.Instance.IsWindowsExplorerOnTop)
                info.WorkingDirectory = UserContext.Instance.GetExplorerPath(true);
                info.Arguments = BuildCommand(parameters);
                info.UseShellExecute = true;
                info.ErrorDialog = true;
                Process.Start(info);
                info = null;
            }));
            Commands.Add(_run_command);

            // Build the Add command
            _add_command = new Command("Add New Command", "Add a new command.", Command.PriorityType.Medium);
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
                CommandPicker ep = new CommandPicker(this);
                if (ep.ShowDialog() == DialogResult.OK)
                    SaveSettings();
                ep.Dispose();
            }));
            Commands.Add(_add_command);

            // Build user commands
            foreach (PromptCommand prompt_command in _pompt_commands)
            {
                PromptCommand pcommand = new PromptCommand(prompt_command);
                Command cmd = new Command(pcommand.Name);

                cmd.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
                {
                    return true;
                }));
                cmd.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return pcommand.Name;
                }));
                cmd.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    if (parameters == string.Empty)
                    {
                        return "Run " + pcommand.Name + ".";
                    }
                    else
                    {
                        return "Arguments: " + pcommand.GetArguments(parameters);
                    }
                }));
                cmd.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    if (parameters == string.Empty)
                    {
                        return pcommand.Name;
                    }
                    else
                    {
                        return pcommand.Name + " " + parameters;
                    }
                }));
                cmd.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
                {
                    return _prompt_icon.ToBitmap();
                }));
                cmd.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
                {
                    List<string> args = new List<string>();
                    Dictionary<string, bool> comp = new Dictionary<string, bool>();

                    if (pcommand.Arguments.Contains(PromptCommand.ArgumentsToken))
                    {
                        args.Add("arguments");
                        if (parameters != string.Empty)
                            comp.Add("arguments", true);
                        else
                            comp.Add("arguments", false);
                    }

                    return new CommandUsage(cmd.Name, args, comp);
                }));
                cmd.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
                {
                    
                    try
                    {
                        ProcessStartInfo info = new ProcessStartInfo(pcommand.Path);
                        //if (UserContext.Instance.IsWindowsExplorerOnTop)
                        //    info.WorkingDirectory = UserContext.Instance.GetExplorerPath(true);
                        string wd = System.IO.Path.GetDirectoryName(pcommand.Path);
                        info.WorkingDirectory = (System.IO.Directory.Exists(wd) ? wd : string.Empty);
                        info.Arguments = pcommand.GetArguments(parameters);
                        info.UseShellExecute = true;
                        info.ErrorDialog = true;
                        System.Diagnostics.Process.Start(info);
                        info = null;
                    }
                    catch
                    {

                    }
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
