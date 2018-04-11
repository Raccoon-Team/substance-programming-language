using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Substance.PluginManager.Backend.Configs
{
    public class Configuration
    {
        private Dictionary<string, ConfigParameter> _parameters = new Dictionary<string, ConfigParameter>();

        private const string GlobalConfigName = "global.xml";
        private string _source;
        
        public Configuration():this(GlobalConfigName)
        {
        }

        public Configuration(string path)
        {
            ParseSettings(path);
            _source = path;
        }

        private void ParseSettings(string path)
        {
            if (File.Exists(path))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                foreach (XmlNode node in xmlDocument.SelectNodes("//*/*"))
                {
                    try
                    {
                        var parameter = ConfigParameter.XmlParse(node);
                        _parameters.Add(parameter.Name, parameter);
                    }
                    catch (XmlException e)
                    {
                        throw new InvalidDataException("Invalid node format");
                    }
                }
            }
            else
            {
                throw new ArgumentException($"\"{Path.GetFullPath(path)}\" not found");
            }
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
                XmlElement element = xmlDocument.CreateElement(parameter.Name);
                element.InnerText = parameter.Value;
                var configType = parameter.ConfigType.ToString();
                element.SetAttribute("Type", configType);

                if (parameter.ConfigType == ConfigType.Values)
                    element.SetAttribute("Values", string.Join(",", parameter.PossibleValues));

                root.AppendChild(element);
            }
            xmlDocument.Save(path);
        }

        public ConfigParameter this[string name] => _parameters[name];

        public bool IsDefined(string name)
        {
            return _parameters.ContainsKey(name);
        }
    }
}
