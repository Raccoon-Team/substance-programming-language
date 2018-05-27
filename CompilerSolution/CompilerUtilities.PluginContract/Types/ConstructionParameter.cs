namespace CompilerUtilities.Plugins.Contract
{
    public sealed class ConstructionParameter
    {
        public ConstructionParameter(ConstructionType constructionType, string returnType, string name)
        {
            ConstructionType = constructionType;
            ReturnType = returnType;
            Name = name;
        }

        public ConstructionType ConstructionType { get; }
        public string ReturnType { get; }
        public string Name { get; }
    }
}