namespace CompilerUtilities.PluginContract
{
    public interface ICompileOptions
    {
        string this[string key] { get; }
        bool Contains(string key);
    }
}