using System.Collections.Generic;
using CompilerUtilities.PluginContract;

namespace CompilerUtilities.PluginImporter
{
    internal class OptionsDict : ICompileOptions
    {
        private readonly Dictionary<string, string> _dictionary;

        public OptionsDict(IReadOnlyList<string> args)
        {
            _dictionary = new Dictionary<string, string>();
            for (var i = 0; i < args.Count; i++)
            {
                var key = args[i];
                var value = !IsSingleKey(key) ? args[++i] : "";
                _dictionary.Add(key, value);
            }
        }

        public string this[string key] => _dictionary[key];

        public bool Contains(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        private static bool IsSingleKey(string key)
        {
            return false;
        }
    }
}