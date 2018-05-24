namespace Substance.PluginManager.Backend
{
    public class DependenciesInfo
    {
        public string Title;
        public DependenciesCondition Condition;

        public DependenciesInfo(string title, DependenciesCondition condition)
        {
            Title = title;
            Condition = condition;
        }
    }
}