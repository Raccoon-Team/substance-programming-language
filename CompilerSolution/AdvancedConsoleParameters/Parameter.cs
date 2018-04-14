using System.Collections.Generic;

namespace AdvancedConsoleParameters
{
    public sealed class Parameter
    {
        private Parameter()
        {
            Values = new List<string>();
        }

        internal Parameter(string name) : this()
        {
            Name = name;
        }

        internal Parameter(string name, bool isSingle, string[] possibleValues) : this()
        {
            Name = name;
            IsSingle = isSingle;
            PossibleValues = possibleValues;
        }

        public string Name { get; }
        public bool IsSingle { get; }
        public string[] PossibleValues { get; }

        public List<string> Values { get; set; }
    }
}