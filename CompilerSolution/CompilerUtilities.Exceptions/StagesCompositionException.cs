using System;

namespace CompilerUtilities.Exceptions
{
    public class StagesCompositionException : Exception
    {
        public StagesCompositionException(string message = "Could not create chain of stages") : base(message)
        {
        }
    }

    public class StageNotFoundException:Exception
    {
        public StageNotFoundException(string message) : base(message)
        {
            
        }
    }

    public class DuplicatedStagesException:Exception
    {
        public DuplicatedStagesException(string message) : base(message)
        {
            
        }
    }
}