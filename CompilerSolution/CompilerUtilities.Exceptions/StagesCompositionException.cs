using System;

namespace CompilerUtilities.Exceptions
{
    public class StagesCompositionException : Exception
    {
        public StagesCompositionException(string message = "Could not create chain of stages") : base(message)
        {
        }
    }
}