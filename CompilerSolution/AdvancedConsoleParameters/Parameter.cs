using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedConsoleParameters
{
    public sealed class Parameter : IEquatable<Parameter>
    {
        private readonly List<string> _flagValue = new List<string> {"true"};
        private List<string> _values;

        private Parameter()
        {
            Values = new List<string>();
            PossibleValues = new string[0];
        }

        internal Parameter(string key) : this()
        {
            Key = key;
        }

        public string Key { get; }
        public bool IsFlag { get; private set; }
        public string[] PossibleValues { get; private set; }

        public List<string> Values
        {
            get => !IsFlag ? _values : _flagValue;
            set => _values = value;
        }

        internal void SetUp(ParameterAttribute attribute)
        {
            PossibleValues = attribute.PossibleValues.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            IsFlag = attribute.IsFlag;

            Validate();
        }

        private void Validate()
        {
            if (PossibleValues.Length > 0)
                if (!IsFlag)
                {
                    if (PossibleValues.Intersect(Values).Any())
                    {
                        //todo throw Exception. Недопустимые параметры
                    }
                }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Parameter);
        }

        public bool Equals(Parameter other)
        {
            return other != null &&
                   Key == other.Key &&
                   IsFlag == other.IsFlag &&
                   EqualityComparer<string[]>.Default.Equals(PossibleValues, other.PossibleValues) &&
                   EqualityComparer<List<string>>.Default.Equals(Values, other.Values);
        }

        public override int GetHashCode()
        {
            var hashCode = 538221694;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + IsFlag.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(PossibleValues);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(Values);
            return hashCode;
        }
    }
}