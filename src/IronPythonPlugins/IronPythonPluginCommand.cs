using SystemCore.CommonTypes;

namespace IronPythonPlugins
{
    class IronPythonPluginCommand : Command
    {
        public IronPythonPluginCommand(IIronPythonCommandPlugin plugin)
            : base(plugin.Name, "Python script " + plugin.Name)
        {
            SetIsOwnerDelegate(plugin.IsOwner);
            SetNameDelegate(plugin.GetName);
            SetDescriptionDelegate(plugin.GetDescription);
            SetIconDelegate(str => Resources.python_clear.ToBitmap());
            SetAutoCompleteDelegate(plugin.AutoComplete);
            SetUsageDelegate(str => new CommandUsage(plugin.Name));
            SetExecuteDelegate((parameters, modifiers) => plugin.Execute(parameters));
        }
    }
}