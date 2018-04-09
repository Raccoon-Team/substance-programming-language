using System;

namespace CompilerUtilities.Exceptions
{
    public class CompileException : Exception
    {
        public CompileException()
        {
        }

        public CompileException(string message)
        {
            Message = message;
        }

        public override string Message { get; }
    }
}