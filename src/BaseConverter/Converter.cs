using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SystemCore.CommonTypes;

//
// Thanks to Eli Yukelzon
//
namespace Converter
{
    [AutomatorPlugin("Base Converter: Makes number base conversions.")]
    public class Converter : InterpreterPlugin
    {
        #region Properties
        private Icon _icon;
        private Command _calc_command;
        #endregion

        #region Constructors
        public Converter()
            : base("")
        {
            _description = "Makes number base conversions." + Environment.NewLine + Environment.NewLine +
                           "type \"=\" and then a value to be converted to" + Environment.NewLine +
                           "Decimal, Hexadecimal, Octal and Binary bases." + Environment.NewLine + Environment.NewLine +
                           "\"0x\" + value to specify Hex base" + Environment.NewLine +
                           "\"0\" + value to specify Octal base" + Environment.NewLine +
                           "value + \"b\" to specify Binary base" + Environment.NewLine +
                           "value to specify Dec base";

            _icon = BaseConverter.Properties.Resources.calc;
        }
        #endregion

        #region Public Methods

        public void LoadSettings()
        {

        }

        public void SaveSettings()
        {

        }
        #endregion

        #region Overrided Methods
        protected override void SetupCommands()
        {
            _calc_command = new Command("Convert", Command.PriorityType.High);
            _calc_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return parameters.StartsWith("=");
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

                return new CommandUsage(@"=0x2", args, comp);
            }));
            _calc_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
            {
                Clipboard.SetText(Eval(parameters));
            }));
            Commands.Add(_calc_command);
        }


        private string Eval(string parameters)
        {
            parameters = parameters.Substring(1);
            try
            {
                int ret = 0;
                if (parameters.StartsWith("0x"))
                {
                    ret = Convert.ToInt16(parameters.Substring(2), 16);
                }
                else if (parameters.EndsWith("b"))
                {
                    ret = Convert.ToInt16(parameters.Substring(0, parameters.Length - 1), 2);
                }
                else if (parameters.StartsWith("0")){
                    ret = Convert.ToInt16(parameters.Substring(0, parameters.Length), 8);
                }else{
                    ret = Convert.ToInt16(parameters, 10);
                }
                return 
                    "Dec: " + Convert.ToString(ret, 10) + 
                    " Hex: " + Convert.ToString(ret, 16) +
                    " Oct: " + Convert.ToString(ret, 8) +
                    " Bin: " + Convert.ToString(ret, 2)  
                     ;
            }catch {
                return "bad format";
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
