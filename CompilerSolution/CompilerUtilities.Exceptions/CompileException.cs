using System;
using System.Runtime.Serialization;

namespace CompilerUtilities.Exceptions
{
    [Serializable]
    public class CompileException : Exception
    {
        public CompileException()
        {
        }

        public CompileException(string message)
        {
            Message = message;
        }

        public CompileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CompileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message { get; }
    }
}