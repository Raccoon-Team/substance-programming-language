using System;
using System.Runtime.Serialization;

namespace CompilerUtilities.Exceptions
{
    [Serializable]
    public class ExtensionNotCompatibleException : Exception
    {
        public ExtensionNotCompatibleException()
        {
        }

        public ExtensionNotCompatibleException(string message, object extension, Version extensionVersion) :
            base(message)
        {
            Extension = extension;
            ExtensionVersion = extensionVersion;
        }

        public ExtensionNotCompatibleException(string message, object extension, Version extensionVersion,
            Exception innerException) : base(message, innerException)
        {
            Extension = extension;
            ExtensionVersion = extensionVersion;
        }

        protected ExtensionNotCompatibleException(SerializationInfo info, StreamingContext context) : base(info,
            context)
        {
        }

        public object Extension { get; set; }
        public Version ExtensionVersion { get; set; }
    }
}