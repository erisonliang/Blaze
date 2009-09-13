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
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SystemCore.CommonTypes;

namespace Calculator
{
    [AutomatorPlugin("Calculator: Makes simple calculations.")]
    public class Calc : InterpreterPlugin
    {
        #region Properties
        private Regex _regex_calc;
        private Regex _regex_forbidden_chars;
        private Regex _regex_comma_rule;
        private Icon _icon;
        private Command _calc_command;
        #endregion

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
        public Calc()
            : base("") 
        {
            _description = "Makes simple calculations." + Environment.NewLine + Environment.NewLine +
                            "Supports the following operators:" + Environment.NewLine +
                            "+ : addition" + Environment.NewLine +
                            "- : subtraction" + Environment.NewLine +
                            "* : multiplication" + Environment.NewLine +
                            "/ : division" + Environment.NewLine +
                            "\\ : remainder" + Environment.NewLine +
                            "^ : powers" + Environment.NewLine +
                            "% : percentage";
            _regex_calc = new Regex(@"\d*[\-\+\*\/\(\)\u0025\u005E\\]+[\d\s\-\+\*\/\(\)\u0025\u005E\\\s]+"); // \u0025 -> %
            _regex_forbidden_chars = new Regex(@"([^\d|\+|\-|\*|\/|\(|\)\u0025\u005E\\\,\.\s]|\|)+");
            _regex_comma_rule = new Regex(@"[\d]+[\,\.][\d]*[\,\.][\d]*");
            _icon = Properties.Resources.calc;
        }
        #endregion

        #region Public Methods
        //public override bool IsOwner(string cmd)
        //{
        //    if (RegEx.IsMatch(cmd))
        //    {
        //        bool test1 = !_regex_forbidden_chars.IsMatch(cmd);
        //        MatchCollection mc = _regex_comma_rule.Matches(cmd);
        //        bool test2 = !(mc.Count > 0);
        //        return test1 && test2;
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
        //    return Eval(item);
        //}

        //public override string GetItemDescription(string cmd, string item)
        //{
        //    return "Hit return key to copy to clipboard";
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
        //    Clipboard.SetText(item.Name);
        //    return false;
        //}

        //public override void Configure()
        //{
        //    MessageBox.Show("There is nothing to configure.");
        //}

        //public override Command GetCommand(InterpreterItem item)
        //{
        //    return null;
        //}

        public void LoadSettings()
        {

        }

        public void SaveSettings()
        {

        }
        #endregion

        #region Private Methods
        private string Eval(string input)
        {
            Expression expr = new Expression();
            string fixed_input = input.Replace(" ", string.Empty);
            //if (CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator == ",")
            //{
            //    fixed_input = fixed_input.Replace('.', ',');
            //}
            //else if (CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator == ".")
            //{
            //    fixed_input = fixed_input.Replace(',', '.');
            //}
            fixed_input = fixed_input.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
            fixed_input = fixed_input.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
            expr.Parse(fixed_input);
            return expr.Eval().ToString();
        }
        #endregion

        #region Overrided Methods
        protected override void SetupCommands()
        {
            _calc_command = new Command("Calculate", Command.PriorityType.High);
            _calc_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                if (_regex_calc.IsMatch(parameters))
                {
                    bool test1 = !_regex_forbidden_chars.IsMatch(parameters);
                    bool test2 = !_regex_comma_rule.IsMatch(parameters);
                    return test1 && test2;
                }
                else
                {
                    return false;
                }
            }));
            _calc_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return Eval(parameters);
            }));
            _calc_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return "Hit return key to copy to clipboard";
            }));
            _calc_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return parameters;
            }));
            _calc_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _icon.ToBitmap();
            }));
            _calc_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(@"Valid math expression. i.e.: 2+3*(3^2)", args, comp);
            }));
            _calc_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                Clipboard.SetText(Eval(parameters));
            }));
            Commands.Add(_calc_command);
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
