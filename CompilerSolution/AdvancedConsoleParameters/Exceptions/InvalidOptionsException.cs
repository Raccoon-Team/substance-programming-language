using System;
using System.Runtime.Serialization;

namespace AdvancedConsoleParameters.Exceptions
{
    [Serializable]
    public class InvalidOptionsException : Exception
    {
        public InvalidOptionsException()
        {
        }

        public InvalidOptionsException(string message) : base(message)
        {
        }

        public InvalidOptionsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidOptionsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}