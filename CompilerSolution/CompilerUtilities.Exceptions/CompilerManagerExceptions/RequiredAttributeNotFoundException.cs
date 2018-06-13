using System;
using System.Runtime.Serialization;

namespace CompilerUtilities.Exceptions
{
    [Serializable]
    public sealed class RequiredAttributeNotFoundException : Exception
    {
        public RequiredAttributeNotFoundException()
        {
        }

        public RequiredAttributeNotFoundException(string message, object extension, Type requiredAttributeType) :
            base(message)
        {
            Extension = extension;
            RequiredAttributeType = requiredAttributeType;
        }

        public RequiredAttributeNotFoundException(string message, object extension, Type requiredAttributeType,
            Exception innerException) : base(message, innerException)
        {
            Extension = extension;
            RequiredAttributeType = requiredAttributeType;
        }

        public object Extension { get; }
        public Type RequiredAttributeType { get; }
    }
}