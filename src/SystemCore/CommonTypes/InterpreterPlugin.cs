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
using System.Drawing;
using System.Windows.Forms;
using SystemCore.SystemAbstraction.StringUtilities;

namespace SystemCore.CommonTypes
{
    public abstract class InterpreterPlugin : Plugin
    {
        #region Properties
        protected List<Command> _commands;
        #endregion

        #region Accessors
        public List<Command> Commands { get { return _commands; } }
        #endregion

        #region Constructors
        public InterpreterPlugin(string name, string description, string version, string website)
        {
            _configurable = false;
            _type = PluginType.Interpreter;
            _name = name;
            _description = description;
            _version = version;
            _website = website;
            _commands = new List<Command>();
            SetupCommands();
        }

        public InterpreterPlugin(string description, string website)
        {
            _configurable = false;
            _type = PluginType.Interpreter;
            _name = GetAssembyName();
            _description = description;
            _version = GetAssemblyVersion();
            _website = website;
            _commands = new List<Command>();
            SetupCommands();
        }

        public InterpreterPlugin(string description)
        {
            _configurable = false;
            _type = PluginType.Interpreter;
            _name = GetAssembyName();
            _description = description;
            _version = GetAssemblyVersion();
            _website = string.Empty;
            _commands = new List<Command>();
            SetupCommands();
        }
        #endregion

        #region Public Methods
        //public virtual int IsOwner(Command.PriorityType priority, string text)
        //{
        //    foreach (Command cmd in Commands)
        //        if (cmd.Priority == priority && cmd.IsOwner(text))
        //            return 0;
        //    return -1;
        //}

        //public virtual int IsOwner(string cmd, string text, string[] tokens)
        //{
        //    Command command = GetCommandByName(cmd);
        //    if (command != null)
        //    {
        //        int min = -1;
        //        foreach (string token in tokens)
        //        {
        //            int pos = text.IndexOf(token);
        //            if (min == -1)
        //                min = pos;
        //            else if (pos < min)
        //                min = pos;
        //        }
        //        return min;
        //    }
        //    else
        //    {
        //        return -1;
        //    }
        //}

        public virtual bool IsOwner(string cmd)
        {
            Command command = GetCommandByName(cmd);
            if (command != null)
                return true;
            else
                return false;
        }

        public virtual void OnBuild()
        {

        }

        public override void Configure()
        {
            MessageBox.Show("There is nothing to configure.", "No configuration needed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public virtual Command GetAssistingCommand(Command.PriorityType priority, string text)
        {
            foreach (Command cmd in Commands)
                if ((cmd.Priority & priority) == priority && cmd.IsOwner(text))
                    return cmd;
            return null;
        }

        public virtual string GetAssistingCommandName(Command.PriorityType priority, string text)
        {
            return GetAssistingCommand(priority, text).Name;
        }

        public virtual string GetItemName(string cmd, string text, string[] tokens)
        {
            Command command = GetCommandByName(cmd);
            if (command != null)
            {
                string parameters = BuildParameters(text, tokens);
                return command.GetName(parameters);
            }
            return null;
        }

        public virtual string GetItemDescription(string cmd, string text, string[] tokens)
        {
            Command command = GetCommandByName(cmd);
            if (command != null)
            {
                string parameters = BuildParameters(text, tokens);
                return command.GetDescription(parameters);
            }
            return null;
        }

        public virtual string GetItemAutoComplete(string cmd, string text, string[] tokens)
        {
            Command command = GetCommandByName(cmd);
            if (command != null)
            {
                string parameters = BuildParameters(text, tokens);
                return command.GetAutoComplete(parameters);
            }
            return null;
        }

        public virtual Image GetItemIcon(string cmd, string text, string[] tokens)
        {
            Command command = GetCommandByName(cmd);
            if (command != null)
            {
                string parameters = BuildParameters(text, tokens);
                return command.GetIcon(parameters);
            }
            return null;
        }

        public virtual CommandUsage GetUsage(string cmd, string text, string[] tokens)
        {
            Command command = GetCommandByName(cmd);
            if (command != null)
            {
                string parameters = BuildParameters(text, tokens);
                return command.GetUsage(parameters);
            }
            return null;
        }

        public virtual bool Execute(InterpreterItem item)
        {
            Command command = GetCommandByName(item.CommandName);
            if (command != null)
            {
                string parameters = BuildParameters(item.Text, item.CommandTokens);
                command.Execute(parameters);
                return true;
            }
            return false;
        }

        public virtual Command GetCommand(InterpreterItem item)
        {
            return GetCommandByName(item.CommandName);
        }

        public virtual Command GetCommandByName(string cmd_name)
        {
            foreach (Command cmd in Commands)
                if (cmd.Name == cmd_name)
                    return cmd;
            return null;
        }
        #endregion

        #region Protected Methods
        protected abstract void SetupCommands();

        protected virtual string BuildParameters(string text, string[] tokens) // PROBLEMA!! pode haver mais tokens do que o texto para substituir. Exemplo: "email !des"
        {
            string parameters = text;
            if (tokens != null)
            {
                foreach (string token in tokens)
                    parameters = StringUtility.ReplaceFirstOccurrenceNoCaps(parameters, token, string.Empty);
                parameters = parameters.Replace("  ", " ");
            }   
            return parameters.Trim();
        }

        protected abstract string GetAssembyName();

        protected abstract string GetAssemblyVersion();
        #endregion
    }
}
