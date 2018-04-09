using System;
using System.Runtime.Serialization;

namespace CompilerUtilities.Exceptions
{
    public class StageNotFoundException : Exception
    {
        public StageNotFoundException()
        {
        }

        public StageNotFoundException(string message) : base(message)
        {
        }

        public StageNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StageNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}