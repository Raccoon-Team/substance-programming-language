using System;
using System.Text.RegularExpressions;

namespace CompilerUtilities.Plugins.Versions
{
    public struct VersionInfo : IEquatable<VersionInfo>
    {
        public readonly int Major, Minor;
        public readonly VersionPrefix Prefix;

        public VersionInfo(int major, int minor, VersionPrefix prefix)
        {
            Major = major;
            Minor = minor;
            Prefix = prefix;
        }

        public override string ToString()
        {
            return $"{PrefixToString(Prefix)}{Major}.{Minor}";
        }

        private static string PrefixToString(VersionPrefix prefix)
        {
            switch (prefix)
            {
                case VersionPrefix.Alpha:
                    return "a";
                case VersionPrefix.Beta:
                    return "b";
                default:
                    return "";
            }
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

        public static VersionInfo Parse(string str)
        {
            var match = Regex.Match(str, @"^([ab])?(\d+).(\d+)$");

            if (!match.Success)
                throw new ArgumentException("Incorrect VersionInfo format");

            var prefix = GetPrefixFromString(match.Groups[1].Value);
            var major = int.Parse(match.Groups[2].Value);
            var minor = int.Parse(match.Groups[3].Value);

            var newVersion = new VersionInfo(major, minor, prefix);

            return newVersion;
        }

        public bool Equals(VersionInfo other)
        {
            return Major == other.Major && Minor == other.Minor && Prefix == other.Prefix;
        }

        public override bool Equals(object obj)
        {
            return obj is VersionInfo && Equals((VersionInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Major;
                hashCode = (hashCode * 397) ^ Minor;
                hashCode = (hashCode * 397) ^ (int) Prefix;
                return hashCode;
            }
        }
    }
}