using System;
using System.Runtime.Serialization;

namespace AdvancedConsoleParameters.Exceptions
{
    public class MismatchOfArgumentCount : Exception
    {
        public MismatchOfArgumentCount(string message, string key, int expectedArgumentCount, int currentArgumentCount) :
            base(message)
        {
            ExpectedArgumentCount = expectedArgumentCount;
            CurrentArgumentCount = currentArgumentCount;
            Key = key;
        }
        public MismatchOfArgumentCount(string key, int expectedArgumentCount, int currentArgumentCount) :
            base($"Количество передаваемых параметров в ключе \"-{key}\" не совпадает. Ожидалось {expectedArgumentCount} параметров.")
        {
            ExpectedArgumentCount = expectedArgumentCount;
            CurrentArgumentCount = currentArgumentCount;
            Key = key;
        }

        public MismatchOfArgumentCount(string message, string key, int expectedArgumentCount, int currentArgumentCount,
            Exception innerException) : base(message, innerException)
        {
            ExpectedArgumentCount = expectedArgumentCount;
            CurrentArgumentCount = currentArgumentCount;
            Key = key;
        }

        protected MismatchOfArgumentCount(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public int ExpectedArgumentCount { get; set; }
        public int CurrentArgumentCount { get; set; }
        public string Key { get; set; }
    }
}