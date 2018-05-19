using System;

namespace AdvancedConsoleParameters.Exceptions
{
    public sealed class PossibleValuesOfFlagParameterException : Exception
    {
        public PossibleValuesOfFlagParameterException()
        {
        }

        public PossibleValuesOfFlagParameterException(string message) : base(message)
        {
        }

        public PossibleValuesOfFlagParameterException(string message, Exception innerException) : base(message,
            innerException)
        {
        }
    }
}