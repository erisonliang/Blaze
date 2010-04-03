using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemCore.CommonTypes;
using Configurator;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;

namespace IronPythonPlugins
{
    [AutomatorPluginAttribute("IronPythonHostPlugin for plugins")]
    public class IronPythonHostPlugin : InterpreterPlugin
    {
        private ScriptEngine _engine;
        private Dictionary<string, IList<Command>> _plugins;

        public IronPythonHostPlugin() : base("Host for IronPython plugins")
        {
            
        }

        protected override void SetupCommands()
        {
            _engine = Python.CreateEngine();
            _plugins = new Dictionary<string, IList<Command>>();

            var pythonFiles =
                Directory
                    .GetFiles(Path.Combine(CommonInfo.PluginsFolder, "IronPythonPlugins"))
                    .Where(f => f.ToLowerInvariant().EndsWith("ipy"));

            foreach (var pythonFile in pythonFiles)
            {
                try
                {
                    ScriptSource script = _engine.CreateScriptSourceFromFile(pythonFile);
                    CompiledCode code = script.Compile();
                    ScriptScope scope = _engine.CreateScope();
                    
                    scope.SetVariable("IIronPythonCommand", ClrModule.GetPythonType(typeof(IIronPythonCommand)));
                    scope.SetVariable("BaseIronPythonCommand", ClrModule.GetPythonType(typeof(BaseIronPythonCommand)));
                    code.Execute(scope);
                    
                    _plugins[pythonFile] = new List<Command>();
                    

                    var pluginClasses = scope.GetItems()
                        .Where(kvp => kvp.Value is IronPython.Runtime.Types.PythonType)
                        .Where(kvp => typeof (IIronPythonCommand).IsAssignableFrom(((IronPython.Runtime.Types.PythonType)kvp.Value).__clrtype__()))
                        .Select(kvp => kvp.Key)
                        .Where(c => c != "BaseIronPythonCommand" && c != "IIronPythonCommand");
                    

                    foreach (var p in pluginClasses)
                    {
                        var plugin = (IIronPythonCommand)_engine.Execute(string.Format("{0}()", p), scope);

                        var command = new IronPythonPluginCommand(plugin);

                        _commands.Add(command);
                        _plugins[pythonFile].Add(command);
                    }
                    
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.Write(string.Format("Error with file {1}: {0}", e, pythonFile));
                }
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