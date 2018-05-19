namespace CompilerUtilities.Plugins.Contract.Interfaces
{
    public interface IStage<in TIn, out TOut> : ICompilerExtension
    {
        void Initialize();
        uint Priority { get; }
        TOut Process(TIn input);
    }
}