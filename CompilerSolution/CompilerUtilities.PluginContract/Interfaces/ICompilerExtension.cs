namespace CompilerUtilities.Plugins.Contract
{
    public interface ICompilerExtension
    {
        void Initialize(IFileBuffer fileBuffer);
    }
}