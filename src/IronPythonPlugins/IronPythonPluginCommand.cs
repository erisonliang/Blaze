using System;
using System.IO;
using SystemCore.CommonTypes;

namespace IronPythonPlugins
{
    class IronPythonPluginCommand : Command
    {
        public IronPythonPluginCommand(FileInfo pythonFile, IIronPythonCommand plugin)
            : base(plugin.Name, "Python script " + plugin.Name)
        {
            SetIsOwnerDelegate(plugin.IsOwner);
            SetNameDelegate(plugin.GetName);
            SetDescriptionDelegate(parameters =>
                                       {
                                           var description = plugin.GetDescription(parameters);
                                           return string.IsNullOrEmpty(description)
                                                      ? pythonFile.FullName + ":" + plugin.Name
                                                      : description;
                                       });
            SetIconDelegate(str => Resources.python_clear.ToBitmap());
            SetAutoCompleteDelegate(plugin.AutoComplete);
            SetUsageDelegate(plugin.Usage);
            SetExecuteDelegate((parameters, modifiers) =>
                                   {
                                       try
                                       {
                                           plugin.Execute(parameters);
                                       }
                                       catch (Exception e)
                                       {
                                           SetDescriptionDelegate(s => string.Format("Error in past execution: {0}", e.Message));
                                       }
                                   });
        }
    }
}