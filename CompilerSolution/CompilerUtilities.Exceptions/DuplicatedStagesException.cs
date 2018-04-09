using System;

namespace CompilerUtilities.Exceptions
{
    public class DuplicatedStagesException : Exception
    {
        public DuplicatedStagesException(string message) : base(message)
        {
        }
    }
}