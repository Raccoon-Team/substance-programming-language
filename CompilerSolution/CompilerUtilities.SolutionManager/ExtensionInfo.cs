using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace CompilerUtilities.SolutionManager
{
    [DataContract]
    public class ExtensionInfo
    {
        [DataMember] public string Guid;

        [DataMember] public string Name;

        [DataMember] public Version Version;

        public ExtensionInfo(string name, string guid, Version version)
        {
            Name = name;
            Guid = guid;
            Version = version;
        }

        public ExtensionInfo(Assembly assembly)
        {
            Name = assembly.FullName;
            Guid = assembly.GetCustomAttribute<GuidAttribute>().Value;
            Version = assembly.GetName().Version;
        }
    }
}