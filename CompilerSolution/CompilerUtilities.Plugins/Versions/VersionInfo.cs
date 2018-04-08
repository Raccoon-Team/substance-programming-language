using System;
using System.Text.RegularExpressions;

namespace CompilerUtilities.Plugins.Versions
{
    public struct VersionInfo
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly VersionPrefix Prefix;

        public VersionInfo(int major, int minor, VersionPrefix prefix)
        {
            Major = major;
            Minor = minor;
            Prefix = prefix;
        }

        public override string ToString()
        {
            return $"{PrefixToString()}{Major}.{Minor}";
        }

        private string PrefixToString()
        {
            switch (Prefix)
            {
                case VersionPrefix.Alpha:
                    return "a";
                case VersionPrefix.Beta:
                    return "b";
                default:
                    return "";
            }
        }

        private static VersionPrefix PrefixFromString(string str)
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
            Regex regex = new Regex(@"^([ab])?(\d+).(\d+)$");
            if (regex.IsMatch(str))
            {
                var match = regex.Match(str);

                var prefix = PrefixFromString(match.Groups[1].Value);
                var major = Int32.Parse(match.Groups[2].Value);
                var minor = Int32.Parse(match.Groups[3].Value);

                var newVersion = new VersionInfo(major, minor, prefix);

                return newVersion;
            }
            else
            {
                throw new ArgumentException("Incorrect str format");
            }
        }
    }
}