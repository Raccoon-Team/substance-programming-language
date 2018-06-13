using System;
using System.Runtime.Serialization;

namespace CompilerUtilities.Exceptions
{
    [Serializable]
    public class CompileException : Exception
    {
        private CompileException(string message)
        {
            Message = message;
        }

        public CompileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CompileException(string message, int code, int line, string file) : this(message)
        {
            Code = code;
            Line = line;
            File = file;
        }

        protected CompileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message { get; }
        public int Code { get; set; }
        public int Line { get; set; }
        public string File { get; set; }
    }
}