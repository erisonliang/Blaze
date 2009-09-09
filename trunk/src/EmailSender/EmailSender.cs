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
using SystemCore.CommonTypes;

namespace EmailSender
{
    [AutomatorPlugin("EmailSender: Calls you default windows email client to send email.")]
    public class EmailSender : InterpreterPlugin
    {
        #region Properties
        private Icon _icon;
        private Command _email_command;
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
        public EmailSender()
            : base("Sends email through you're default windows email "+ Environment.NewLine + " client.")
        {
            _description += Environment.NewLine + Environment.NewLine +
                            "E.g.: email john.doe@email.provider.com Hi man, " + Environment.NewLine + "what's up?";
            _icon = Properties.Resources.email;
        }
        #endregion

        #region Public Methods
        //public override bool IsOwner(string cmd)
        //{
        //    foreach (Command command in Commands)
        //        if (cmd.Contains(command.Name))
        //            return true;
        //    return false;
        //    //string[] s = input.ToLower().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        //    //if (s.Length > 0)
        //    //{
        //    //    int val = LevenshteinMeasurer.Instance.GetDistance("email", s[0]);
        //    //    if (val <= 1)
        //    //        return true;
        //    //    else
        //    //        return false;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //}

        //public override void OnBuild()
        //{

        //}

        //public override string GetItemName(string cmd, string item)
        //{
        //    Email email = BuildEmail(item);
        //    if (email.Contact == string.Empty)
        //    {
        //        return "E-mail";
        //    }
        //    else
        //    {
        //        return "Send e-mail to " + email.Contact;
        //    }
        //}

        //public override string GetItemDescription(string cmd, string item)
        //{
        //    Email email = BuildEmail(item);
        //    if (email.Contact == string.Empty)
        //    {
        //        return "Open e-mail client";
        //    }
        //    else
        //    {
        //        return "Subject: " + email.Subject;
        //    }
        //}

        //public override string GetItemAutoComplete(string cmd, string item)
        //{
        //    Email email = BuildEmail(item);
        //    if (email.Contact == string.Empty)
        //    {
        //        return "email";
        //    }
        //    else
        //    {
        //        return "email " + email.Contact + " " + email.Subject;
        //    }
        //}

        //public override Icon GetItemIcon(string cmd, string input)
        //{
        //    return _icon;
        //}

        //public override bool Execute(InterpreterItem item)
        //{
        //    Email email = BuildEmail(item.AutoComplete);
        //    ProcessStartInfo info = new ProcessStartInfo("mailto:"+email.Contact+"?body="+email.Body+"&subject="+email.Subject);
        //    info.UseShellExecute = true;
        //    info.ErrorDialog = true;
        //    System.Diagnostics.Process.Start(info);
        //    info = null;
        //    return true;
        //}

        //public override void Configure()
        //{
        //    MessageBox.Show("There is nothing to configure.");
        //}

        //public override Command GetCommand(InterpreterItem item)
        //{
        //    return _email_command;
        //}
        #endregion

        #region Private Methods 
        private Email BuildEmail(string parameters)
        {
            //List<string> text = new List<string>(StringUtility.GenerateKeywords(parameters, false));
            List<string> text = new List<string>(parameters.Split(new string[] {"\""}, StringSplitOptions.RemoveEmptyEntries));
            text.RemoveAll(delegate(string s)
            {
                if (s.Trim() == string.Empty)
                    return true;
                else
                    return false;
            });

            //// discover what token represent 'e-mail'
            //int min = Int32.MaxValue;
            //int min_pos = -1;
            //for (int i = 0; i < text.Count; i++)
            //{
            //    int dist = LevenshteinMeasurer.Instance.GetDistance(text[i], _email_command.Name);
            //    if (dist < min)
            //    {
            //        min = dist;
            //        min_pos = i;
            //    }
            //}

            //// delete that token
            //text.RemoveAt(min_pos);

            Email email;
            if (text.Count >= 3)
            {
                //string content = string.Empty;
                //for (int i = 1; i < text.Count; i++)
                //{
                //    if (i == text.Count - 1)
                //    {
                //        content += text[i];
                //    }
                //    else
                //    {
                //        content += text[i] + " ";
                //    }
                //}
                //email = new Email(text[0], @content);
                email = new Email(text[0], text[1], text[2]);
            }
            else if (text.Count == 2)
            {
                //email = new Email(text[0], string.Empty);
                email = new Email(text[0], text[1]);
            }
            else if (text.Count == 1)
            {
                email = new Email(text[0]);
            }
            else
            {
                email = new Email();
            }
            return email;
        }
        #endregion

        #region Overrided Methods
        protected override void SetupCommands()
        {
            _email_command = new Command("email");
            _email_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return true;
            }));
            _email_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                Email email = BuildEmail(parameters);
                if (email.Contact == string.Empty)
                {
                    return "E-mail";
                }
                else
                {
                    return "Send e-mail to " + email.Contact;
                }
            }));
            _email_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                Email email = BuildEmail(parameters);
                if (email.Contact == string.Empty)
                {
                    return "Open e-mail client";
                }
                else
                {
                    return "Subject: " + email.Subject;
                }
            }));
            _email_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                Email email = BuildEmail(parameters);
                if (email.Contact == string.Empty)
                {
                    return "email";
                }
                else
                {
                    return "email " + email.Contact + " " + email.Subject;
                }
            }));
            _email_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return _icon.ToBitmap();
            }));
            _email_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>(new string[] { "\"contact1, contact2, ...\"", "\"subject\"", "\"body\"" });
                Dictionary<string, bool> comp = new Dictionary<string, bool>();
                foreach (string arg in args)
                    comp.Add(arg, false);

                Email email = BuildEmail(parameters);
                if (email.Contact != string.Empty)
                    comp["\"contact1, contact2, ...\""] = true;
                if (email.Subject != string.Empty)
                    comp["\"subject\""] = true;
                if (email.Body != string.Empty)
                    comp["\"body\""] = true;

                return new CommandUsage(_email_command.Name, args, comp);
            }));
            _email_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters)
            {
                Email email = BuildEmail(parameters);
                ProcessStartInfo info = new ProcessStartInfo("mailto:" + email.Contact + "?body=" + email.Body + "&subject=" + email.Subject);
                info.UseShellExecute = true;
                info.ErrorDialog = true;
                System.Diagnostics.Process.Start(info);
                info = null;
            }));
            Commands.Add(_email_command);
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
