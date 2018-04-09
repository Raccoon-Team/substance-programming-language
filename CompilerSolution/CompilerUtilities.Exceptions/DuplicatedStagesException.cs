using System;
using System.Runtime.Serialization;

namespace CompilerUtilities.Exceptions
{
    public class DuplicatedStagesException : Exception
    {
        public DuplicatedStagesException()
        {
        }

        public DuplicatedStagesException(string message) : base(message)
        {
        }

        public DuplicatedStagesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicatedStagesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}