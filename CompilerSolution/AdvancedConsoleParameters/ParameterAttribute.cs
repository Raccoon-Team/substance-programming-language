using System;

namespace AdvancedConsoleParameters
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = false)]
    public class ParameterAttribute : Attribute
    {
        public ParameterAttribute(string key, bool isSingle = false)
        {
            Key = key;
            IsSingle = isSingle;
        }

        public string PossibleValues { get; set; }
        internal string Key { get; set; }
        public string Description { get; set; }
        internal bool IsSingle { get; set; }
    }
}