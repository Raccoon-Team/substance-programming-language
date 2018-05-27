namespace CompilerUtilities.Plugins.Contract
{
    public interface IStage<in TIn, out TOut> : ICompilerExtension
    {
        TOut Process(TIn input);
    }
}