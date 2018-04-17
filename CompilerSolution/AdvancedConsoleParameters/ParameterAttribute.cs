using System;

namespace AdvancedConsoleParameters
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = false)]
    public class ParameterAttribute : Attribute
    {
        public ParameterAttribute(string key, bool isFlag = false)
        {
            Keys = key.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            IsFlag = isFlag;
            PossibleValues = string.Empty;
        }

        public string PossibleValues { get; set; }
        internal string[] Keys;
        public string Description { get; set; }
        internal bool IsFlag;
    }
}