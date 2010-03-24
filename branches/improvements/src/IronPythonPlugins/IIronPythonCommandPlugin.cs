namespace IronPythonPlugins
{
    public interface IIronPythonCommandPlugin
    {
        void Execute(string command);
        string GetName(string parameters);
        string GetDescription(string parameters);
        string AutoComplete(string parameters);
        bool IsOwner(string parameters);
        string Name { get; }
    }
}