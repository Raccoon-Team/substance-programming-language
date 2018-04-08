using System;

namespace CompilerUtilities.Exceptions
{
    public class InvalidCompilerOptionsException : Exception
    {
        public InvalidCompilerOptionsException(string message) : base(message)
        {
        }
    }
}