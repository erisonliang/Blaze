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
using System.IO;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction.FileHandling;
using Configurator;
using ContextLib;

namespace Prompt
{
    public partial class CommandPicker : Form
    {
        private Prompt _parent;
        private ToolTip _tooltip;

        public CommandPicker(Prompt parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void EnginePicker_Load(object sender, EventArgs e)
        {
            _tooltip = new ToolTip();
            _tooltip.IsBalloon = true;
            //_tooltip.ShowAlways = true;
            _tooltip.Active = true;
            _tooltip.ToolTipIcon = ToolTipIcon.Error;
            _tooltip.ToolTipTitle = "Error";
            //_tooltip.AutoPopDelay = 3000;
            _tooltip.InitialDelay = 0;
            //_tooltip.ReshowDelay = 500;
            ContextLib.DataContainers.Multimedia.MultiLevelData data = UserContext.Instance.GetSelectedContent();
            if (data.FileList != null && data.FileList.Length > 0)
            {
                string path = data.FileList[data.FileList.Length - 1];
                if (File.Exists(path))
                {
                    FileInfo info = new FileInfo(path);
                    nameTextBox.Text = FileNameManipulator.GetFileName(info.Name);
                    pathTextBox.Text = path;
                    data.Dispose();
                    return;
                }
                else if (Directory.Exists(path))
                {
                    DirectoryInfo info = new DirectoryInfo(path);
                    nameTextBox.Text = FileNameManipulator.GetFolderName(path);
                    pathTextBox.Text = path;
                    data.Dispose();
                    return;
                }
            }
            if (data.Text != null && data.Text.Trim() != string.Empty)
            {
                string path = data.Text.Trim();
                if (File.Exists(path))
                {
                    FileInfo info = new FileInfo(path);
                    nameTextBox.Text = FileNameManipulator.GetFileName(info.Name);
                    pathTextBox.Text = path;
                    data.Dispose();
                    return;
                }
                else if (Directory.Exists(path))
                {
                    DirectoryInfo info = new DirectoryInfo(path);
                    nameTextBox.Text = FileNameManipulator.GetFolderName(path);
                    pathTextBox.Text = path;
                    data.Dispose();
                    return;
                }
            }
            data.Dispose();
            
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text == string.Empty)
            {
                nameTextBox.Focus();
                _tooltip.SetToolTip(nameTextBox, "Error");
                _tooltip.Show("You must specify a name.", nameTextBox, 3000);
                //_tooltip.RemoveAll();
                return;
            }
            else if (_parent.PromptCommandNames.Contains(nameTextBox.Text))
            {
                nameTextBox.Focus();
                _tooltip.SetToolTip(nameTextBox, "Error");
                _tooltip.Show("A command with this name alreay exists. Please specify a different one.", nameTextBox, 3000);
                //_tooltip.RemoveAll();
                return;
            }
            else if (pathTextBox.Text == string.Empty)
            {
                pathTextBox.Focus();
                _tooltip.SetToolTip(pathTextBox, "Error");
                _tooltip.Show("You must specify a path.", pathTextBox, 3000);
                //_tooltip.RemoveAll();
                return;
            }
            else if (!File.Exists(pathTextBox.Text) && !Directory.Exists(pathTextBox.Text))
            {
                pathTextBox.Focus();
                _tooltip.SetToolTip(pathTextBox, "Error");
                _tooltip.Show("This is not a valid path.", pathTextBox, 3000);
                //_tooltip.RemoveAll();
                return;
            }
            //else
            //{
            //    FileInfo finfo = new FileInfo(pathTextBox.Text);
            //    if (finfo.Extension.ToLower() != ".exe")
            //    {
            //        pathTextBox.Focus();
            //        _tooltip.SetToolTip(pathTextBox, "Error");
            //        _tooltip.Show("This is not an .exe file.", pathTextBox, 3000);
            //        return;
            //    }
            //}
            //else if (argumentsTextBox.Text != string.Empty)
            //{
            //    if (!argumentsTextBox.Text.Contains(PromptCommand.ArgumentsToken))
            //    {
            //        argumentsTextBox.Focus();
            //        _tooltip.SetToolTip(argumentsTextBox, "Error");
            //        _tooltip.Show("Arguments must have a user input location specified ('" + PromptCommand.ArgumentsToken + "').", argumentsTextBox, 3000);
            //        //_tooltip.RemoveAll();
            //        return;
            //    }
            //}
            _parent.PromptCommandNames.Add(nameTextBox.Text);
            PromptCommand new_pcommand = new PromptCommand(nameTextBox.Text, pathTextBox.Text, argumentsTextBox.Text);
            _parent.PromptCommands.Add(new_pcommand);

            Command new_command = new Command(nameTextBox.Text);
            new_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return true;
            }));
            new_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return new_pcommand.Name;
            }));
            new_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                if (parameters == string.Empty)
                {
                    if (System.IO.Directory.Exists(new_pcommand.Path))
                    {
                        return "Open " + new_pcommand.Path;
                    }
                    else
                    {
                        return "Run " + new_pcommand.Name;
                    }
                }
                else
                {
                    return "Arguments: " + new_pcommand.GetArguments(parameters);
                }
            }));
            new_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                if (parameters == string.Empty)
                {
                    return new_pcommand.Name;
                }
                else
                {
                    return new_pcommand.Name + " " + parameters;
                }
            }));
            new_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.prompt.ToBitmap();
            }));
            new_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
                {
                    List<string> args = new List<string>();
                    Dictionary<string, bool> comp = new Dictionary<string, bool>();

                    if (new_pcommand.Arguments.Contains(PromptCommand.ArgumentsToken))
                    {
                        args.Add("arguments");
                        if (parameters != string.Empty)
                            comp.Add("arguments", true);
                        else
                            comp.Add("arguments", false);
                    }

                    return new CommandUsage(new_command.Name, args, comp);
                }));
            new_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {

                try
                {
                    ProcessStartInfo info;
                    if ((modifiers & Keys.Shift) == Keys.Shift)
                        info = new ProcessStartInfo(FileSearcher.GetItemFolder(new_pcommand.Path));
                    else
                        info = new ProcessStartInfo(new_pcommand.Path);
                    string wd = System.IO.Path.GetDirectoryName(new_pcommand.Path);
                    info.WorkingDirectory = (System.IO.Directory.Exists(wd) ? wd : string.Empty);
                    info.Arguments = new_pcommand.GetArguments(parameters);
                    info.UseShellExecute = true;
                    info.ErrorDialog = true;
                    Process.Start(info);
                    info = null;
                }
                catch
                {

                }
            }));
            _parent.Commands.Add(new_command);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (File.Exists(pathTextBox.Text))
            {
                FileInfo info = new FileInfo(pathTextBox.Text);
                fd.InitialDirectory = info.DirectoryName;
            }
            else
                fd.InitialDirectory = CommonInfo.UserHomeDrive;
            //fd.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            fd.Filter = "All files (*.*)|*.*";
            fd.Multiselect = false;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                nameTextBox.Text = FileNameManipulator.GetFileName(fd.SafeFileName);
                pathTextBox.Text = fd.FileName;
            }
        }
    }
}
