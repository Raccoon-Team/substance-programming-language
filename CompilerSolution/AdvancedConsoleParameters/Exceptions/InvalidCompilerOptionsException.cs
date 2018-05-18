using System;
using System.Runtime.Serialization;

namespace CompilerUtilities.Exceptions
{
    public class InvalidCompilerOptionsException : Exception
    {
        public InvalidCompilerOptionsException()
        {
        }

        public InvalidCompilerOptionsException(string message) : base(message)
        {
        }

        public InvalidCompilerOptionsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCompilerOptionsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}