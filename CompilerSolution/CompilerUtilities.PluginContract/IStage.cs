namespace CompilerUtilities.Plugins.Contract
{
    public interface IStage<in TIn, out TOut> : ICompilerExtension
    {
        void Initialize(ICompileOptions options);

        TOut Process(TIn input);
    }
}