namespace CompilerUtilities.Plugins.Contract
{
    public interface IPlugin<T> : ICompilerExtension
    {
        T Activate(T input);
    }
}