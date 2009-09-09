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
using SystemCore.SystemAbstraction.StringUtilities;
using Configurator;

namespace SystemCore.CommonTypes
{
    /// <summary>
    /// Represents a interpreter command.
    /// </summary>
    public class Command
    {
        #region Properties
        private string _name;
        private string _protected_name;
        private string[] _keywords;
        private string _description;
        private PriorityType _priority;
        private OwnershipDelegate _is_owner_delegate;
        private EvaluationDelegate _name_delegate;
        private EvaluationDelegate _description_deletage;
        private EvaluationDelegate _auto_complete_delegate;
        private IconDelegate _icon_delegate;
        private UsageDelegate _usage_delegate;
        private ExecutionDelegate _execute_delegate;
        /// <summary>
        /// The command priority specifies when will the command be ran.
        /// </summary>
        [Flags]
        public enum PriorityType : int
        {
            Low = 1, Medium = 2, High = 4
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// Gets the protected command name. Protected command name consists of the command name with the applications GUID appended.
        /// </summary>
        public string ProtectedName { get { return _protected_name; } }
        /// <summary>
        /// Gets the keywords that add meaning to the command.
        /// </summary>
        public string[] Keywords { get { return _keywords; } }
        /// <summary>
        /// Gets the command description.
        /// </summary>
        public string Description { get { return _description; } }
        /// <summary>
        /// Gets the command priority.
        /// </summary>
        public PriorityType Priority { get { return _priority; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of SystemCore.CommonTypes.Command class.
        /// </summary>
        /// <param name="name">Command name.</param>
        public Command(string name)
        {
            _name = name;
            _protected_name = _name + " " + CommonInfo.GUID;
            _keywords = StringUtility.GenerateKeywords(_name);
            _description = string.Empty;
            _priority = PriorityType.Medium;
        }

        /// <summary>
        /// Creates a new instance of SystemCore.CommonTypes.Command class.
        /// </summary>
        /// <param name="name">Command name.</param>
        /// <param name="priority">Command priority.</param>
        public Command(string name, PriorityType priority)
        {
            _name = name;
            _protected_name = _name + " " + CommonInfo.GUID;
            _keywords = StringUtility.GenerateKeywords(_name);
            _description = string.Empty;
            _priority = priority;
        }

        /// <summary>
        /// Creates a new instance of SystemCore.CommonTypes.Command class.
        /// </summary>
        /// <param name="name">Command name.</param>
        /// <param name="discription">Command description.</param>
        public Command(string name, string discription)
        {
            _name = name;
            _protected_name = _name + " " + CommonInfo.GUID;
            _keywords = StringUtility.GenerateKeywords(_name);
            _description = discription;
            _priority = PriorityType.Medium;
        }

        /// <summary>
        /// Creates a new instance of SystemCore.CommonTypes.Command class.
        /// </summary>
        /// <param name="name">Command name.</param>
        /// <param name="discription">Command description.</param>
        /// <param name="priority">Command priority.</param>
        public Command(string name, string discription, PriorityType priority)
        {
            _name = name;
            _protected_name = _name + " " + CommonInfo.GUID;
            _keywords = StringUtility.GenerateKeywords(_name);
            _description = discription;
            _priority = priority;
        }

        ///// <summary>
        ///// Creates a new instance of SystemCore.CommonTypes.Command class.
        ///// </summary>
        ///// <param name="name">Command name.</param>
        ///// <param name="keywords">Keywords that add meaning to the command.</param>
        //public Command(string name, string[] keywords)
        //{
        //    _name = name;
        //    _protected_name = _name + " " + CommonInfo.GUID;
        //    _keywords = keywords;
        //    _description = string.Empty;
        //    _priority = PriorityType.Medium;
        //}

        ///// <summary>
        ///// Creates a new instance of SystemCore.CommonTypes.Command class.
        ///// </summary>
        ///// <param name="name">Command name.</param>
        ///// <param name="keywords">Keywords that add meaning to the command</param>
        ///// <param name="priority">Command priority.</param>
        //public Command(string name, string[] keywords, PriorityType priority)
        //{
        //    _name = name;
        //    _protected_name = _name + " " + CommonInfo.GUID;
        //    _keywords = keywords;
        //    _description = string.Empty;
        //    _priority = priority;
        //}

        ///// <summary>
        ///// Creates a new instance of SystemCore.CommonTypes.Command class.
        ///// </summary>
        ///// <param name="name">Command name.</param>
        ///// <param name="keywords">Keywords that add meaning to the command.</param>
        ///// <param name="description">Command description.</param>
        //public Command(string name, string[] keywords, string description)
        //{
        //    _name = name;
        //    _protected_name = _name + " " + CommonInfo.GUID;
        //    _keywords = keywords;
        //    _description = description;
        //    _priority = PriorityType.Medium;
        //}

        ///// <summary>
        ///// Creates a new instance of SystemCore.CommonTypes.Command class.
        ///// </summary>
        ///// <param name="name">Command name.</param>
        ///// <param name="keywords">Keywords that add meaning to the command.</param>
        ///// <param name="description">Command description.</param>
        ///// <param name="priority">Command priority.</param>
        //public Command(string name, string[] keywords, string description, PriorityType priority)
        //{
        //    _name = name;
        //    _protected_name = _name + " " + CommonInfo.GUID;
        //    _keywords = keywords;
        //    _description = description;
        //    _priority = priority;
        //}

        /// <summary>
        /// Creates a new instance of SystemCore.CommonTypes.Command class.
        /// </summary>
        /// <param name="command">Command to be duplicated.</param>
        public Command(Command command)
        {
            _name = command.Name;
            _protected_name = _name + " " + CommonInfo.GUID;
            _keywords = (string[])command.Keywords.Clone();
            _description = command.Description;
            _priority = command.Priority;
        }
        #endregion

        #region Delegates
        /// <summary>
        /// A delegate method that indicates if the current command can execute the specified paramenters.
        /// </summary>
        /// <param name="parameters">A string containing the parameters, provided by the user.</param>
        /// <returns>True if the command recognizes the parameters as valid, false otherwise.</returns>
        public delegate bool OwnershipDelegate(string parameters);
        /// <summary>
        /// A delegate method that returns the evaluation of the specified parameters.
        /// </summary>
        /// <param name="parameters">A string containing the parameters, provided by the user.</param>
        /// <returns>String containing the parameters' evaluation.</returns>
        public delegate string EvaluationDelegate(string parameters);
        /// <summary>
        /// A delegate method that returns the best icon according to the specified parameters.
        /// </summary>
        /// <param name="parameters">A string containing the parameters, provided by the user.</param>
        /// <returns>Most appropriated icon.</returns>
        public delegate System.Drawing.Image IconDelegate(string parameters);
        /// <summary>
        /// A delegate method that returns the command usage, according to the specified parameters.
        /// </summary>
        /// <param name="parameters">A string containing the parameters, provided by the user.</param>
        public delegate CommandUsage UsageDelegate(string parameters);
        /// <summary>
        /// A delegate method that executes the command with the specified parameters.
        /// </summary>
        /// <param name="parameters">A string containing the parameters, provided by the user.</param>
        public delegate void ExecutionDelegate(string parameters);
        #endregion

        #region Public Methods
        public void SetNewCommand(string name)
        {
            _name = name;
            _protected_name = _name + " " + CommonInfo.GUID;
            _keywords = StringUtility.GenerateKeywords(_name);
            _description = string.Empty;
            _priority = PriorityType.Medium;
        }

        public void SetNewCommand(string name, PriorityType priority)
        {
            _name = name;
            _protected_name = _name + " " + CommonInfo.GUID;
            _keywords = StringUtility.GenerateKeywords(_name);
            _description = string.Empty;
            _priority = priority;
        }

        public void SetNewCommand(string name, string discription)
        {
            _name = name;
            _protected_name = _name + " " + CommonInfo.GUID;
            _keywords = StringUtility.GenerateKeywords(_name);
            _description = discription;
            _priority = PriorityType.Medium;
        }

        public void SetNewCommand(string name, string discription, PriorityType priority)
        {
            _name = name;
            _protected_name = _name + " " + CommonInfo.GUID;
            _keywords = StringUtility.GenerateKeywords(_name);
            _description = discription;
            _priority = priority;
        }

        public void SetNewCommand(Command command)
        {
            _name = command.Name;
            _protected_name = _name + " " + CommonInfo.GUID;
            _keywords = (string[])command.Keywords.Clone();
            _description = command.Description;
            _priority = command.Priority;
        }

        public void SetIsOwnerDelegate(OwnershipDelegate method)
        {
            _is_owner_delegate = method;
        }

        public void SetNameDelegate(EvaluationDelegate method)
        {
            _name_delegate = method;
        }

        public void SetDescriptionDelegate(EvaluationDelegate method)
        {
            _description_deletage = method;
        }

        public void SetAutoCompleteDelegate(EvaluationDelegate method)
        {
            _auto_complete_delegate = method;
        }

        public void SetIconDelegate(IconDelegate method)
        {
            _icon_delegate = method;
        }

        public void SetUsageDelegate(UsageDelegate method)
        {
            _usage_delegate = method;
        }

        public void SetExecuteDelegate(ExecutionDelegate method)
        {
            _execute_delegate = method;
        }

        public bool IsOwner(string paramenters)
        {
            return _is_owner_delegate(paramenters);
        }

        public string GetName(string parameters)
        {
            return _name_delegate(parameters);
        }

        public string GetDescription(string parameters)
        {
            return _description_deletage(parameters);
        }

        public string GetAutoComplete(string parameters)
        {
            return _auto_complete_delegate(parameters);
        }

        public System.Drawing.Image GetIcon(string parameters)
        {
            return _icon_delegate(parameters);
        }

        public CommandUsage GetUsage(string parameters)
        {
            return _usage_delegate(parameters);
        }

        public void Execute(string parameters)
        {
            _execute_delegate(parameters);
        }

        public bool FitsPriority(PriorityType priority)
        {
            return (_priority & priority) == priority;
        }

        /// <summary>
        /// Appends the applications GUID to the specified command name.
        /// </summary>
        /// <param name="command_name">Unprotected command name.</param>
        /// <returns>Protected command name.</returns>
        public static string ProtectCommand(string command_name)
        {
            return command_name + " " + CommonInfo.GUID;
        }

        /// <summary>
        /// Removes the GUID from the specified command name.
        /// </summary>
        /// <param name="command_name">Protected command name.</param>
        /// <returns>Unprotected command name. Empty string if the specified command name was already unprotected.</returns>
        public static string UnprotectCommand(string command_name)
        {
            if (command_name.Contains(CommonInfo.GUID))
                return command_name.Replace(CommonInfo.GUID, string.Empty).Trim();
            else
                return string.Empty;
        }
        #endregion
    }
}
