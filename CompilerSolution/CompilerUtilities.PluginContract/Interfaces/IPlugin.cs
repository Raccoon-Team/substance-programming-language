namespace CompilerUtilities.Plugins.Contract
{
    public interface IPlugin<T> : ICompilerExtension
    {
        uint Priority { get; }
        T Activate(T input);
    }
}