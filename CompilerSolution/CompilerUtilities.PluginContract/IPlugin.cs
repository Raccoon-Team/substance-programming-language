namespace CompilerUtilities.PluginContract
{
    public interface IPlugin<T>
    {
        T Process(T input);
    }
}