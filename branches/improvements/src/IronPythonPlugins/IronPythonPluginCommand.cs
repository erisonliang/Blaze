using SystemCore.CommonTypes;

namespace IronPythonPlugins
{
    class IronPythonPluginCommand : Command
    {
        public IronPythonPluginCommand(IIronPythonCommand plugin)
            : base(plugin.Name, "Python script " + plugin.Name)
        {
            SetIsOwnerDelegate(plugin.IsOwner);
            SetNameDelegate(plugin.GetName);
            SetDescriptionDelegate(plugin.GetDescription);
            SetIconDelegate(str => Resources.python_clear.ToBitmap());
            SetAutoCompleteDelegate(plugin.AutoComplete);
            SetUsageDelegate(plugin.Usage);
            SetExecuteDelegate((parameters, modifiers) => plugin.Execute(parameters));
        }
    }
}