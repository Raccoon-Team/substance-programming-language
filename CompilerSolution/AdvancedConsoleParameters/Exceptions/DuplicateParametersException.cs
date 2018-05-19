using System;

namespace AdvancedConsoleParameters.Exceptions
{
    [Serializable]
    public sealed class DuplicateParametersException : Exception
    {
        public string ParameterKey { get; }

        public DuplicateParametersException()
        {
        }

        public DuplicateParametersException(string message, string parameterKey) : base(message)
        {
            ParameterKey = parameterKey;
        }

        public DuplicateParametersException(string message, string parameterKey, Exception innerException) : base(message, innerException)
        {
            ParameterKey = parameterKey;
        }
    }
}