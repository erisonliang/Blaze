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
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.Settings;
using Configurator;
using ContextLib;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;

namespace Blaze.Interpreter
{
    public class MenuEngine : InterpreterPlugin
    {
        #region Properties
        private MainForm _parent;
        private ScriptEngine _script_engine;
        private FileSystemWatcher _file_watcher;
        private List<Thread> _thread_pool;
        #endregion

        #region Constructors
        public MenuEngine(MainForm parent)
            : base ("Provides blaze internal commands.")
        {
            _parent = parent;
            _script_engine = Python.CreateEngine();

            // initialize thread pool
            _thread_pool = new List<Thread>();

            // prepare file system watcher to load every new script
            if (!Directory.Exists(CommonInfo.ScriptsFolder))
                Directory.CreateDirectory(CommonInfo.ScriptsFolder);
            _file_watcher = new FileSystemWatcher(CommonInfo.ScriptsFolder);
            _file_watcher.Created += new FileSystemEventHandler(_file_watcher_Created);
            _file_watcher.Deleted += new FileSystemEventHandler(_file_watcher_Deleted);
            _file_watcher.Renamed += new RenamedEventHandler(_file_watcher_Renamed);

            // load System.dll
            _script_engine.Runtime.LoadAssembly(typeof(string).Assembly);

            // load mscorlib
            _script_engine.Runtime.LoadAssembly(typeof(System.Diagnostics.Debug).Assembly);

            List<string> search_paths = new List<string>();// = (List<string>)_script_engine.GetSearchPaths();
            //search_paths.Add(Path.GetFullPath(Environment.CurrentDirectory));
            search_paths.Add(Path.GetFullPath(CommonInfo.PythonFolder));
            search_paths.Add(Path.GetFullPath(CommonInfo.PythonTutorialFolder));
            search_paths.Add(Path.GetFullPath(CommonInfo.PythonLibFolder));
            search_paths.Add(Path.GetFullPath(CommonInfo.PythonSitePackagesFolder));
            //search_paths.Add(Path.GetFullPath(CommonInfo.ScriptsFolder));
            _script_engine.SetSearchPaths(search_paths);
            //Thread python = new Thread(new ThreadStart(delegate()
            //{
            //    ScriptSource source = _script_engine.CreateScriptSourceFromString("print \"start\"", SourceCodeKind.SingleStatement);
            //    ScriptScope scope = _script_engine.CreateScope();
            //    source.Execute(scope);
            //}));
            //python.Start();

            // hook Pause|Break key to abort script execution
            Gma.UserActivityMonitor.HookManager.KeyDown += new KeyEventHandler(HookManager_KeyDown);

            // enable new script detection
            _file_watcher.EnableRaisingEvents = true;
        }

        ~MenuEngine()
        {
            try
            {

                Gma.UserActivityMonitor.HookManager.KeyDown -= HookManager_KeyDown;
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region Event Handlers
        void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Pause)
            {
                List<Thread> threads = _thread_pool.FindAll(delegate(Thread t)
                {
                    return t.ThreadState != ThreadState.Stopped;
                });
                foreach (Thread t in threads)
                    t.Abort();
            }
        }

        void _file_watcher_Renamed(object sender, RenamedEventArgs e)
        {
            Command edit_script = Commands.Find(delegate(Command cmd)
            {
                return cmd.Name == Path.GetFileNameWithoutExtension(e.OldFullPath);
            });
            if (edit_script != null)
            {
                string file = Path.GetFileNameWithoutExtension(e.FullPath);
                edit_script.SetNewCommand(file, "Run " + file + ".py" + " script");
                edit_script.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
                {
                    return false;
                }));
                edit_script.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return edit_script.Name;
                }));
                edit_script.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return edit_script.Description;
                }));
                edit_script.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return edit_script.Name;
                }));
                edit_script.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
                {
                    return Properties.Resources.script.ToBitmap();
                }));
                edit_script.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
                {
                    List<string> args = new List<string>(new string[] { "arguments" });
                    Dictionary<string, bool> comp = new Dictionary<string, bool>();
                    foreach (string arg in args)
                        comp.Add(arg, false);

                    if (parameters != string.Empty)
                        comp["arguments"] = true;

                    return new CommandUsage(edit_script.Name, args, comp);
                }));
                edit_script.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
                {
                    string[] tokens = parameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    List args = new List();
                    foreach (string token in tokens)
                        args.Add(token);
                    ScriptSource source = _script_engine.CreateScriptSourceFromFile(CommonInfo.ScriptsFolder + edit_script.Name + ".py");
                    ScriptScope scope = _script_engine.CreateScope();
                    scope.SetVariable("_user_context", UserContext.Instance);
                    _script_engine.GetSysModule().SetVariable("argv", args);
                    Thread python = new Thread(new ThreadStart(delegate()
                    {
                        try
                        {
                            source.Execute(scope);
                        }
                        catch (ThreadAbortException)
                        {
                            MessageBox.Show(edit_script.Name + " execution was aborted by the user.", "Script execution aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        catch (Exception e2)
                        {
                            MessageBox.Show(edit_script.Name + " encountered an error while executing: " + Environment.NewLine + Environment.NewLine
                                + e2.ToString() + Environment.NewLine + Environment.NewLine + "Script execution aborted.", "Script execution error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }));
                    python.Start();
                    _thread_pool.Add(python);
                }));
                
            }
        }

        void _file_watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Commands.RemoveAll(delegate(Command cmd)
            {
                return cmd.Name == Path.GetFileNameWithoutExtension(e.FullPath);
            });
        }

        void _file_watcher_Created(object sender, FileSystemEventArgs e)
        {
            string file = Path.GetFileNameWithoutExtension(e.FullPath);
            Command new_script = new Command(file, "Run " + file + ".py" + " script");
            new_script.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            new_script.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return new_script.Name;
            }));
            new_script.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return new_script.Description;
            }));
            new_script.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return new_script.Name;
            }));
            new_script.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.script.ToBitmap();
            }));
            new_script.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>(new string[] { "arguments" });
                Dictionary<string, bool> comp = new Dictionary<string, bool>();
                foreach (string arg in args)
                    comp.Add(arg, false);

                if (parameters != string.Empty)
                    comp["arguments"] = true;

                return new CommandUsage(new_script.Name, args, comp);
            }));
            new_script.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                string[] tokens = parameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List args = new List();
                foreach (string token in tokens)
                    args.Add(token);
                ScriptSource source = _script_engine.CreateScriptSourceFromFile(CommonInfo.ScriptsFolder + new_script.Name + ".py");
                ScriptScope scope = _script_engine.CreateScope();
                scope.SetVariable("_user_context", UserContext.Instance);
                _script_engine.GetSysModule().SetVariable("argv", args);
                Thread python = new Thread(new ThreadStart(delegate()
                {
                    try
                    {
                        source.Execute(scope);
                    }
                    catch (ThreadAbortException)
                    {
                        MessageBox.Show(new_script.Name + " execution was aborted by the user.", "Script execution aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(new_script.Name + " encountered an error while executing: " + Environment.NewLine + Environment.NewLine
                            + e2.ToString() + Environment.NewLine + Environment.NewLine + "Script execution aborted.", "Script execution error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }));
                python.Start();
                _thread_pool.Add(python);
            }));
            Commands.Add(new_script);
        }
        #endregion

        #region Overrided Methods
        public override void OnBuild()
        {
            _thread_pool.RemoveAll(delegate(Thread t)
            {
                return t.ThreadState == ThreadState.Stopped || t.ThreadState == ThreadState.Aborted;
            });
            SetupCommands();
            base.OnBuild();
        }

        protected override void SetupCommands()
        {
            _commands = new List<Command>();

            // Build the Rebuild Index command
            Command rebuild_index = new Command("Rebuild Index", "Rescan files in your computer");
            rebuild_index.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            rebuild_index.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return rebuild_index.Name;
            }));
            rebuild_index.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return rebuild_index.Description;
            }));
            rebuild_index.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return rebuild_index.Name;
            }));
            rebuild_index.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.rebuild.ToBitmap();
            }));
            rebuild_index.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(rebuild_index.Name, args, comp);
            }));
            rebuild_index.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                _parent.RebuildIndex();
            }));
            Commands.Add(rebuild_index);

            // Build the Rebuild Index command
            Command configure_blaze = new Command("Settings", "Open settings dialog");
            configure_blaze.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            configure_blaze.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return configure_blaze.Name;
            }));
            configure_blaze.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return configure_blaze.Description;
            }));
            configure_blaze.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return configure_blaze.Name;
            }));
            configure_blaze.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.preferences.ToBitmap();
            }));
            configure_blaze.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(configure_blaze.Name, args, comp);
            }));
            configure_blaze.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                _parent.OpenSettings();
            }));
            Commands.Add(configure_blaze);

            // Build the Clear Learned Commands command
            //Command clear_commands = new Command("Clear Learned Commands", new string[] { "clear", "reset", "learned", "commands", "blaze" }, "Resets all learned commands");
            Command clear_command = new Command("Clear Commands", "Reset all learned commands");
            clear_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            clear_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return clear_command.Name;
            }));
            clear_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return clear_command.Description;
            }));
            clear_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return clear_command.Name;
            }));
            clear_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.preferences.ToBitmap();
            }));
            clear_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(clear_command.Name, args, comp);
            }));
            clear_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                _parent.ClearLearnedCommands();
            }));
            Commands.Add(clear_command);

            // Build the Open scripts folder command
            Command scripts_folder_command = new Command("Scripts Folder", "Open Blaze scripts folder");
            scripts_folder_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            scripts_folder_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return scripts_folder_command.Name;
            }));
            scripts_folder_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return scripts_folder_command.Description;
            }));
            scripts_folder_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return scripts_folder_command.Name;
            }));
            scripts_folder_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.folder.ToBitmap();
            }));
            scripts_folder_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(scripts_folder_command.Name, args, comp);
            }));
            scripts_folder_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                if (!Directory.Exists(CommonInfo.ScriptsFolder))
                    Directory.CreateDirectory(Path.GetFullPath(CommonInfo.ScriptsFolder));
                System.Diagnostics.Process.Start(Path.GetFullPath(CommonInfo.ScriptsFolder));
            }));
            Commands.Add(scripts_folder_command);

            // Build the Record Macro command
            Command record_macro_command = new Command("Record Macro", "Records a macro");
            record_macro_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            record_macro_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return record_macro_command.Name;
            }));
            record_macro_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return record_macro_command.Description;
            }));
            record_macro_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return record_macro_command.Name;
            }));
            record_macro_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.record.ToBitmap();
            }));
            record_macro_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(record_macro_command.Name, args, comp);
            }));
            record_macro_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                UserContext.Instance.StartMacroRecording();
            }));
            Commands.Add(record_macro_command);

            // Build the Exit command
            Command exit_command = new Command("Exit", "Exit Blaze");
            exit_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            exit_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return exit_command.Name;
            }));
            exit_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return exit_command.Description;
            }));
            exit_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return exit_command.Name;
            }));
            exit_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.exit.ToBitmap();
            }));
            exit_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(exit_command.Name, args, comp);
            }));
            exit_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                _parent.Exit();
            }));
            Commands.Add(exit_command);

            // Build the Assistant command
            Command assistant_command = new Command("Show Assistant", "Show Blaze Assistant");
            assistant_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                return false;
            }));
            assistant_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return assistant_command.Name;
            }));
            assistant_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return assistant_command.Description;
            }));
            assistant_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return assistant_command.Name;
            }));
            assistant_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.assistant.ToBitmap();
            }));
            assistant_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>();
                Dictionary<string, bool> comp = new Dictionary<string, bool>();

                return new CommandUsage(assistant_command.Name, args, comp);
            }));
            assistant_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                _parent.ShowAssistantWindow();
            }));
            Commands.Add(assistant_command);

            // Build the Redo command
            Command redo_command = new Command("redo", "Repeat last suggestion");
            redo_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                int iterations;
                return Int32.TryParse(parameters, out iterations) && iterations > 0 && iterations < 1001;
            }));
            redo_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return redo_command.Name;
            }));
            redo_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                int iterations;
                if (Int32.TryParse(parameters, out iterations) && iterations > 0 && iterations < 1001)
                    return "Repeat last suggestion for " + iterations.ToString() + (iterations > 1 ? " iterations" : " iteration");
                return redo_command.Description;
            }));
            redo_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                int iterations;
                if (Int32.TryParse(parameters, out iterations) && iterations > 0 && iterations < 1001)
                    return redo_command.Name + " " + iterations.ToString();
                else
                    return redo_command.Name;
            }));
            redo_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.redo.ToBitmap();
            }));
            redo_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>(new string[] { "number of iterations [0, 1000]" });
                Dictionary<string, bool> comp = new Dictionary<string, bool>();
                foreach (string arg in args)
                    comp.Add(arg, false);

                int iterations;
                if (Int32.TryParse(parameters, out iterations) && iterations > 0)
                    comp["number of iterations [0, 1000]"] = true;

                return new CommandUsage(redo_command.Name, args, comp);
            }));
            redo_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                if (_parent.LastAcceptedSuggestion != null)
                {
                    int iterations;
                    _parent.LastAcceptedSuggestion.Validate(true);
                    if (Int32.TryParse(parameters, out iterations))
                    {
                        if (iterations > 0)
                            _parent.LastAcceptedSuggestion.Update(iterations, true);
                    }
                    else
                        _parent.LastAcceptedSuggestion.Update(true);
                    Thread sug = new Thread(new ThreadStart(delegate()
                    {
                        _parent.LastAcceptedSuggestion.Execute();
                    }));
                    sug.Start();
                    _thread_pool.Add(sug);
                }
            }));
            Commands.Add(redo_command);

            // Build the Continue command
            Command continue_command = new Command("continue", "Continue last suggestion");
            continue_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
            {
                int iterations;
                return Int32.TryParse(parameters, out iterations) && iterations > 0 && iterations < 1001;
            }));
            continue_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                return continue_command.Name;
            }));
            continue_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                int iterations;
                if (Int32.TryParse(parameters, out iterations) && iterations > 0 && iterations < 1001)
                    return "Continue last suggestion for " + iterations.ToString() + (iterations > 1 ? " iterations" : " iteration");
                return continue_command.Description;
            }));
            continue_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
            {
                int iterations;
                if (Int32.TryParse(parameters, out iterations) && iterations > 0 && iterations < 1001)
                    return continue_command.Name + " " + iterations.ToString();
                else
                    return continue_command.Name;
            }));
            continue_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
            {
                return Properties.Resources.cont.ToBitmap();
            }));
            continue_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
            {
                List<string> args = new List<string>(new string[] { "number of iterations [1, 1000]" });
                Dictionary<string, bool> comp = new Dictionary<string, bool>();
                foreach (string arg in args)
                    comp.Add(arg, false);

                int iterations;
                if (Int32.TryParse(parameters, out iterations) && iterations > 0)
                    comp["number of iterations [1, 1000]"] = true;

                return new CommandUsage(continue_command.Name, args, comp);
            }));
            continue_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
            {
                if (_parent.LastAcceptedSuggestion != null)
                {
                    int iterations;
                    _parent.LastAcceptedSuggestion.Validate(false);
                    if (Int32.TryParse(parameters, out iterations))
                    {
                        if (iterations > 0)
                            _parent.LastAcceptedSuggestion.Update(iterations, false);
                    }
                    else
                        _parent.LastAcceptedSuggestion.Update(false);
                    Thread sug = new Thread(new ThreadStart(delegate()
                    {
                        _parent.LastAcceptedSuggestion.Execute();
                    }));
                    sug.Start();
                    _thread_pool.Add(sug);
                }
            }));
            Commands.Add(continue_command);

            // Build script commands
            string[] files = SettingsManager.Instance.GetScripts();
            foreach (string file in files)
            {
                string ifile = file;
                Command script_command = new Command(file, "Run " + file + ".py" + " script");
                script_command.SetIsOwnerDelegate(new Command.OwnershipDelegate(delegate(string parameters)
                {
                    return true;
                }));
                script_command.SetNameDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return script_command.Name;
                }));
                script_command.SetDescriptionDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return script_command.Description;
                }));
                script_command.SetAutoCompleteDelegate(new Command.EvaluationDelegate(delegate(string parameters)
                {
                    return script_command.Name;
                }));
                script_command.SetIconDelegate(new Command.IconDelegate(delegate(string parameters)
                {
                    return Properties.Resources.script.ToBitmap();
                }));
                script_command.SetUsageDelegate(new Command.UsageDelegate(delegate(string parameters)
                {
                    List<string> args = new List<string>(new string[] { "arguments"});
                    Dictionary<string, bool> comp = new Dictionary<string, bool>();
                    foreach (string arg in args)
                        comp.Add(arg, false);

                    if (parameters != string.Empty)
                        comp["arguments"] = true;

                    return new CommandUsage(script_command.Name, args, comp);
                }));
                script_command.SetExecuteDelegate(new Command.ExecutionDelegate(delegate(string parameters, Keys modifiers)
                {
                    string[] tokens = parameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    List args = new List();
                    foreach (string token in tokens)
                        args.Add(token);
                    ScriptSource source = _script_engine.CreateScriptSourceFromFile(CommonInfo.ScriptsFolder + script_command.Name + ".py");
                    ScriptScope scope = _script_engine.CreateScope();
                    scope.SetVariable("_user_context", UserContext.Instance);
                    _script_engine.GetSysModule().SetVariable("argv", args);
                    Thread python = new Thread(new ThreadStart(delegate()
                        {
                            try
                            {
                                source.Execute(scope);
                            }
                            catch (ThreadAbortException)
                            {
                                MessageBox.Show(script_command.Name + " execution was aborted by the user.", "Script execution aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            catch (Exception e2)
                            {
                                MessageBox.Show(script_command.Name + " encountered an error while executing: " + Environment.NewLine + Environment.NewLine
                                    + e2.ToString() + Environment.NewLine + Environment.NewLine + "Script execution aborted.", "Script execution error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            }
                        }));
                    python.Start();
                    _thread_pool.Add(python);
                }));
                Commands.Add(script_command);
            }
        }

        protected override string GetAssembyName()
        {
            return "MenuEngine";
        }

        protected override string GetAssemblyVersion()
        {
            return "1.0.0.0";
        }
        #endregion
    }
}
