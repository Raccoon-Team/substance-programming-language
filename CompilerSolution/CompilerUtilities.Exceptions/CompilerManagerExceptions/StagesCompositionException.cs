using System;
using System.Runtime.Serialization;

namespace CompilerUtilities.Exceptions
{
    [Serializable]
    public class StagesCompositionException : Exception
    {
        public StagesCompositionException()
        {
        }

        public StagesCompositionException(string message = "Could not create chain of stages") : base(message)
        {
        }

        public StagesCompositionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StagesCompositionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}