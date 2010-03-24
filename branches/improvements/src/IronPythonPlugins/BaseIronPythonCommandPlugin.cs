using System;

namespace IronPythonPlugins
{
    public abstract class BaseIronPythonCommandPlugin : IIronPythonCommandPlugin
    {
        public abstract string Name { get; }

        public virtual string GetName(string parameters)
        {
            return Name;
        }

        public virtual string GetDescription(string parameters)
        {
            return parameters;
        }

        public virtual string AutoComplete(string parameters)
        {
            return parameters;
        }

        public bool IsOwner(string str)
        {
            var split = str.Split(new[] {' '}, 1);
            return split.Length > 0 &&
                   split[0].Trim().ToLowerInvariant() == Name.ToLowerInvariant();
        }

        public abstract void Execute(string command);
    }
}
