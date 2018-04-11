using System;
using System.Linq;
using System.Xml;

namespace Substance.PluginManager.Backend.Configs
{
    public class ConfigParameter
    {
        public ConfigParameter(string name, ConfigType configType, string value)
        {
            PossibleValues = new string[0];
            Name = name;
            ConfigType = configType;
            Value = value;
        }

        public ConfigParameter(string name, string value, string[] possibleValues)
        {
            PossibleValues = possibleValues;
            Name = name;
            ConfigType = ConfigType.Values;
            Value = value;
        }

        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                if(!PossibleValues.Contains(value) && ConfigType == ConfigType.Values)
                    throw new InvalidOperationException("Try set invalid value");
                _value = value;
            }
        }
        public string Name { get; private set; }
        public ConfigType ConfigType { get; private set; }
        public string[] PossibleValues { get; private set; }


    }
}