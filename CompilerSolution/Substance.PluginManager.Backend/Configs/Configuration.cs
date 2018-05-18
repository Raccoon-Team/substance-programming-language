using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Substance.PluginManager.Backend.Configs
{
    public class Configuration
    {
        private const string GlobalConfigName = "global.xml";
        private readonly Dictionary<string, ConfigParameter> _parameters = new Dictionary<string, ConfigParameter>();
        private readonly string _source;

        public Configuration() : this(GlobalConfigName)
        {
        }

        public Configuration(string path)
        {
            ParseSettings(path);
            _source = path;
        }

        public ConfigParameter this[string name] => _parameters[name];

        private void ParseSettings(string path)
        {
            if (File.Exists(path))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                foreach (XmlNode node in xmlDocument.SelectNodes("//*/*"))
                    try
                    {
                        var parameter = ConfigParse(node);
                        _parameters.Add(parameter.Name, parameter);
                    }
                    catch (XmlException e)
                    {
                        throw new InvalidDataException("Invalid node format");
                    }
            }
        }

        private static ConfigParameter ConfigParse(XmlNode node)
        {
            var attribute = node.SelectSingleNode("@Type");

            var name = node.Name;
            var type = ConfigType.String;
            var value = node.InnerText.Trim();

            if (attribute != null)
                Enum.TryParse(attribute.InnerText, out type);
            if (type == ConfigType.Values)
            {
                attribute = node.SelectSingleNode("@Values");
                var values = attribute.InnerText.Split(',').Select(x => x.Trim()).ToArray();
                return new ConfigParameter(name, value, values);
            }

            return new ConfigParameter(name, type, value);
        }

        public void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = _source;
            var xmlDocument = new XmlDocument();

            var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);
            if (xmlDocument.DocumentElement == null)
                xmlDocument.InsertBefore(xmlDocument.CreateElement("Configures"), null);
            var root = xmlDocument.DocumentElement;


            foreach (var parameter in _parameters.Values)
            {
                var element = xmlDocument.CreateElement(parameter.Name);
                element.InnerText = parameter.Value;
                var configType = parameter.ConfigType.ToString();
                element.SetAttribute("Type", configType);

                if (parameter.ConfigType == ConfigType.Values)
                    element.SetAttribute("Values", string.Join(",", parameter.PossibleValues));

                root.AppendChild(element);
            }
            xmlDocument.Save(path);
        }

        public bool IsDefined(string name)
        {
            return _parameters.ContainsKey(name);
        }
    }
}