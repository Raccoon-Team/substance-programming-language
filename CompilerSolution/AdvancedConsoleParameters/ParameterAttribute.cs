using System;
using AdvancedConsoleParameters.Exceptions;

namespace AdvancedConsoleParameters
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class ParameterAttribute : Attribute
    {
        private string _possibleValues;
        public bool IsFlag { get; }

        public string[] Keys { get; }
        
        public ParameterAttribute(string key, bool isFlag = false)
        {
            Keys = key.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            IsFlag = isFlag;
            _possibleValues = string.Empty;
        }

        public string PossibleValues
        {
            get => _possibleValues;
            set
            {
                if (IsFlag)
                    throw new PossibleValuesOfFlagParameterException("Cannot add possible values to flag parameter");
                _possibleValues = value;
            }
        }

        public string Description { get; set; }
    }
}