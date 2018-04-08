using System;

namespace CompilerUtilities.Plugins
{
    public class CompileException:Exception
    {
        public override string Message { get; }

        public CompileException()
        {
        }
        
        public CompileException(string message)
        {
            Message = message;
        }
    }
}