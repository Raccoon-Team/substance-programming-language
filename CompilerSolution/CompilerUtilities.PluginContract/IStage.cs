namespace CompilerUtilities.PluginContract
{
    public interface IStage<in TIn, out TOut>
    {
        void Initialize(ICompileOptions options);

        TOut DoStage(TIn input);
    }
}