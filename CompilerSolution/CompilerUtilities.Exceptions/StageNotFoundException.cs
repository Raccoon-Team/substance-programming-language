using System;

namespace CompilerUtilities.Exceptions
{
    public class StageNotFoundException : Exception
    {
        public StageNotFoundException(string message) : base(message)
        {
        }
    }
}