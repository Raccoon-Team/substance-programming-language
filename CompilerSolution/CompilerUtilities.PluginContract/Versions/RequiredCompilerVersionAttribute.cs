using System;
using System.Text.RegularExpressions;

namespace CompilerUtilities.Plugins.Contract
{
    public sealed class RequiredCompilerVersionAttribute : Attribute
    {
        public RequiredCompilerVersionAttribute(int major, int minor, VersionPrefix prefix)
        {
            Major = major;
            Minor = minor;
            Prefix = prefix;
        }

        public RequiredCompilerVersionAttribute(string versionString)
        {
            Parse(versionString);
        }

        public string Name { get; set; }

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public VersionPrefix Prefix { get; private set; }

        public Version GetRequiredVersion => new Version(Major, Minor);

        private void Parse(string str)
        {
            var match = Regex.Match(str, @"^([ab])?(\d+).(\d+)$");

            if (!match.Success)
                throw new ArgumentException("Incorrect Version format");

            Prefix = GetPrefixFromString(match.Groups[1].Value);
            Major = int.Parse(match.Groups[2].Value);
            Minor = int.Parse(match.Groups[3].Value);
        }

        private static VersionPrefix GetPrefixFromString(string str)
        {
            switch (str)
            {
                case "a":
                    return VersionPrefix.Alpha;
                case "b":
                    return VersionPrefix.Beta;
                default:
                    return VersionPrefix.Release;
            }
        }

        public bool Equals(RequiredCompilerVersionAttribute other)
        {
            return Major == other.Major && Minor == other.Minor && Prefix == other.Prefix;
        }

        public override bool Equals(object obj)
        {
            return obj is RequiredCompilerVersionAttribute && Equals((RequiredCompilerVersionAttribute)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Major;
                hashCode = (hashCode * 397) ^ Minor;
                hashCode = (hashCode * 397) ^ (int)Prefix;
                return hashCode;
            }
        }
    }
}