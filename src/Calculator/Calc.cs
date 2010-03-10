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
using ContextLib;
using ContextLib.DataContainers.Multimedia;
using SystemCore.SystemAbstraction.StringUtilities;

namespace Calculator
{
    [AutomatorPlugin("Calculator: Makes simple calculations.")]
    public class Calc : InterpreterPlugin
    {
        #region Properties
        private Regex _regex_calc;
        private Regex _regex_forbidden_chars;
        private Regex _regex_comma_rule;
        private Regex _regex_convert_base;
        private Regex _regex_base;
        private Regex _regex_base_value;
        private Icon _icon;
        private Command _calc_command;
        private Command _solve_command;
        private Command _convert_base_command;
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
                            "% : percentage"+ Environment.NewLine +
                            "Parenthesis () are also suported.";
            _regex_calc = new Regex(@"(\d[(\.|,)\d]*)*[\-\+\*\/\(\)\u0025\u005E\\]+[\d(\.|,)\d*\s\-\+\*\/\(\)\u0025\u005E\\\s]+"); // \u0025 -> %
            _regex_forbidden_chars = new Regex(@"([^\d|\+|\-|\*|\/|\(|\)\u0025\u005E\\\,\.\s]|\|)+");
            _regex_comma_rule = new Regex(@"[\d]*[\,\.][\d]*[\,\.]");
            _icon = Properties.Resources.calc;
            _regex_convert_base = new Regex(@"(^\d+\s*(dec|oct|bin)|^[0-9a-fA-F]+\s*hex)(\s*$|\s*(to|>))(\s*$|\s*(dec|hex|oct|bin)$)");
            _regex_base = new Regex(@"(dec|hex|oct|bin)");
            _regex_base_value = new Regex(@"[0-9a-fA-F]+");
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
            return string.Format("{0:0.##}", expr.Eval());
        }

         private string ConvertBase(string input)
        {
            int index = input.LastIndexOf("to");
            if (index == -1)
                index = input.LastIndexOf(">");
            if (index == -1)
            {
                string val = _regex_base_value.Match(input).Value;
                string base_str = _regex_base.Match(input).Value;
                int origin = 0;
                string final = string.Empty;
                if (base_str == "dec")
                {
                    origin = Convert.ToInt32(val, 10);
                    final = Convert.ToString(origin, 16) + " hex";
                }
                else
                {
                    if (base_str == "hex")
                        origin = Convert.ToInt32(val, 16);
                    else if (base_str == "oct")
                        origin = Convert.ToInt32(val, 8);
                    else if (base_str == "bin")
                        origin = Convert.ToInt32(val, 2);
                    final = Convert.ToString(origin, 10) + " dec";
                }
                return final;
            }
            else
            {
                string temp_input = input.Replace("to", string.Empty).Replace(">", string.Empty);
                string left_input = temp_input.Substring(0, index);
                string right_input = temp_input.Substring(index);

                string val = _regex_base_value.Match(left_input).Value;
                string origin_base = _regex_base.Match(left_input).Value;
                string destination_base;
                int origin = 0;
                string final = string.Empty;

                if (origin_base == "dec")
                {
                    destination_base = (right_input.Trim().Length > 0 ? _regex_base.Match(right_input).Value : "hex");
                    origin = Convert.ToInt32(val, 10);
                }
                else
                {
                    destination_base = (right_input.Trim().Length > 0 ? _regex_base.Match(right_input).Value : "dec");
                    if (origin_base == "hex")
                        origin = Convert.ToInt32(val, 16);
                    else if (origin_base == "oct")
                        origin = Convert.ToInt32(val, 8);
                    else if (origin_base == "bin")
                        origin = Convert.ToInt32(val, 2);
                }
                if (destination_base == "dec")
                    final = Convert.ToString(origin, 10) + " dec";
                else if (destination_base == "hex")
                    final = Convert.ToString(origin, 16) + " hex";
                else if (destination_base == "oct")
                    final = Convert.ToString(origin, 8) + " oct";
                else if (destination_base == "bin")
                    final = Convert.ToString(origin, 2) + " bin";
                return final;
            }
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
                if ((modifiers & Keys.Shift) == Keys.Shift)
                    Clipboard.SetText(parameters + " = " + Eval(parameters));
                else
                    Clipboard.SetText(Eval(parameters));
            }));
            Commands.Add(_calc_command);

            _solve_command = new Command("Solve", Command.PriorityType.Medium);
            _solve_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            _solve_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                MultiLevelData data = UserContext.Instance.GetSelectedContent();
                string calc = "Invalid expression";
                bool valid_calc = false;
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data.Text) && _regex_calc.IsMatch(data.Text))
                    {
                        bool test1 = !_regex_forbidden_chars.IsMatch(data.Text);
                        bool test2 = !_regex_comma_rule.IsMatch(data.Text);
                        valid_calc = test1 && test2;
                        if (valid_calc)
                            calc = data.Text;
                    }
                    data.Dispose();
                }
                if (valid_calc)
                {
                    string result = Eval(calc);
                    return "Solve: " + StringUtility.ApplyEllipsis(calc, 14 - result.Length) + " = " + result;
                }
                else
                {
                    return "Solve: " + calc;
                }
            }));
            _solve_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return "Hit return key to paste solution";
            }));
            _solve_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return parameters;
            }));
            _solve_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _icon.ToBitmap();
            }));
            _solve_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(@"Solves the selected calculation on top window", args, comp);
            }));
            _solve_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                MultiLevelData data = UserContext.Instance.GetSelectedContent();
                bool valid_calc = false;
                string calc = string.Empty;
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data.Text) && _regex_calc.IsMatch(data.Text))
                    {
                        bool test1 = !_regex_forbidden_chars.IsMatch(data.Text);
                        bool test2 = !_regex_comma_rule.IsMatch(data.Text);
                        valid_calc = test1 && test2;
                        calc = data.Text;
                    }
                    data.Dispose();
                }
                if (valid_calc)
                {
                    string solution;
                    if ((modifiers & Keys.Shift) == Keys.Shift)
                        solution = calc + " = " + Eval(calc);
                    else
                        solution = Eval(calc);
                    UserContext.Instance.PasteText(solution, true);
                }
            }));
            Commands.Add(_solve_command);

            _convert_base_command = new Command("Convert to Base", Command.PriorityType.High);
            _convert_base_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                if (_regex_convert_base.IsMatch(parameters))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }));
            _convert_base_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return ConvertBase(parameters);
            }));
            _convert_base_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return "Hit return key to copy to clipboard";
            }));
            _convert_base_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return _convert_base_command.Name;
            }));
            _convert_base_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _icon.ToBitmap();
            }));
            _convert_base_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(@"Converts a value from a base to another. i.e.: 16 dec to hex", args, comp);
            }));
            _convert_base_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                if ((modifiers & Keys.Shift) == Keys.Shift)
                    Clipboard.SetText(parameters + " = " + ConvertBase(parameters));
                else
                    Clipboard.SetText(ConvertBase(parameters));
            }));
            Commands.Add(_convert_base_command);
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