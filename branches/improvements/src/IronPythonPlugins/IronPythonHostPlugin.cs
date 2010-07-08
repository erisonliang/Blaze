using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemCore.CommonTypes;
using Configurator;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;

namespace IronPythonPlugins
{
    [AutomatorPluginAttribute("IronPythonHostPlugin for plugins")]
    public class IronPythonHostPlugin : InterpreterPlugin
    {
        private ScriptEngine _engine;
        private Dictionary<string, IList<Command>> _pythonCommands;
        private string _ironpythonPluginsFolder;
        private FileSystemWatcher _pluginFolderWatcher;

        public IronPythonHostPlugin() : base("Host for IronPython plugins")
        {
            
        }

        protected override void SetupCommands()
        {
            _engine = Python.CreateEngine();
            _pythonCommands = new Dictionary<string, IList<Command>>();

            _ironpythonPluginsFolder = Path.Combine(CommonInfo.PluginsFolder, "IronPythonPlugins");
            SetupPythonCommands();
            _pluginFolderWatcher = new FileSystemWatcher(_ironpythonPluginsFolder, "*.ipy");
            _pluginFolderWatcher.IncludeSubdirectories = true;
            _pluginFolderWatcher.Changed += _pluginFolderWatcher_Changed;
            _pluginFolderWatcher.Created += _pluginFolderWatcher_Changed;
            _pluginFolderWatcher.Deleted += (sender, e) => RemoveCommandsForFile(e.FullPath);
            _pluginFolderWatcher.EnableRaisingEvents = true;

        }

        void _pluginFolderWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            var pythonFile = e.FullPath;
            RemoveCommandsForFile(pythonFile);
            LoadPythonCommandsForFile(pythonFile);
        }

        private void RemoveCommandsForFile(string pythonFile)
        {
            if(_pythonCommands.ContainsKey(pythonFile))
            {
                foreach (var command in _pythonCommands[pythonFile])
                {
                    _commands.Remove(command);
                }
                _pythonCommands.Remove(pythonFile);
            }
        }

        private void SetupPythonCommands()
        {
            var pythonFiles =
                Directory
                    .GetFiles(_ironpythonPluginsFolder)
                    .Where(f => Path.GetExtension(f).ToLowerInvariant() == ".ipy");

            foreach (var pythonFile in pythonFiles)
            {
                LoadPythonCommandsForFile(pythonFile);
            }
        }

        private void LoadPythonCommandsForFile(string pythonFile)
        {
            try
            {
                ScriptSource script = _engine.CreateScriptSourceFromFile(pythonFile);
                CompiledCode code = script.Compile();
                ScriptScope scope = _engine.CreateScope();
                    
                scope.SetVariable("IIronPythonCommand", ClrModule.GetPythonType(typeof(IIronPythonCommand)));
                scope.SetVariable("BaseIronPythonCommand", ClrModule.GetPythonType(typeof(BaseIronPythonCommand)));
                scope.SetVariable("clr", _engine.GetClrModule());
                code.Execute(scope);
                    
                _pythonCommands[pythonFile] = new List<Command>();


                var pluginClasses = scope.GetItems()
                    .Where(kvp => kvp.Value is IronPython.Runtime.Types.PythonType)
                    .Where(
                        kvp =>
                        typeof (IIronPythonCommand).IsAssignableFrom(((IronPython.Runtime.Types.PythonType) kvp.Value).__clrtype__()))
                    .Where(kvp => kvp.Key != "BaseIronPythonCommand" && kvp.Key != "IIronPythonCommand")

                    .Select(kvp => kvp.Value);
                    

                foreach (var p in pluginClasses)
                {
                    var plugin = (IIronPythonCommand)_engine.Operations.Invoke(p, new object[] { });

                    var command = new IronPythonPluginCommand(new FileInfo(pythonFile), plugin);

                    _commands.Add(command);
                    _pythonCommands[pythonFile].Add(command);
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.Write(string.Format("Error with file {1}: {0}", e, pythonFile));
            }
        }

        protected override string GetAssembyName()
        {
            return "IronPythonHostPlugin";
        }

        protected override string GetAssemblyVersion()
        {
            return "1.0.0.0";
        }
    }
}