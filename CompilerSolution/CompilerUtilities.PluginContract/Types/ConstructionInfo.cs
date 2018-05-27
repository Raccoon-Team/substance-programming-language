using System;
using System.Collections.Generic;
using System.Xml;

namespace CompilerUtilities.Plugins.Contract
{
    public sealed class ConstructionInfo
    {
        public ConstructionInfo(string @interface, string implementation, ConstructionParameter[] parameters)
        {
            Interface = @interface;
            Implementation = implementation;
            Parameters = parameters;
        }

        private ConstructionInfo()
        {
        }

        public string Interface { get; private set; }
        public string Implementation { get; private set; }
        public ConstructionParameter[] Parameters { get; private set; }

        public static ConstructionInfo ParseFromFile(string fileName)
        {
            var xml = new XmlDocument();
            xml.Load(fileName);

            return Parse(xml);
        }

        public static ConstructionInfo ParseFromXml(string xmlText)
        {
            var xml = new XmlDocument();
            xml.LoadXml(xmlText);

            return Parse(xml);
        }

        private static ConstructionInfo Parse(XmlDocument doc)
        {
            var outp = new ConstructionInfo();
            var root = doc.DocumentElement;

            if (root is null || root.Name != "instruction")
                throw new ArgumentNullException();

            var nodesCount = root.ChildNodes.Count;
            for (var i = 0; i < nodesCount; i++)
            {
                var childNode = root.ChildNodes[i];
                switch (childNode.Name)
                {
                    case "parameters":
                        var parameters = new List<ConstructionParameter>();
                        var parametersCount = childNode.ChildNodes.Count;

                        for (var j = 0; j < parametersCount; j++)
                        {
                            var parameter = childNode.ChildNodes[j];
                            parameters.Add(ParseParameter(parameter));
                        }
                        outp.Parameters = parameters.ToArray();
                        break;
                    case "interface":
                        outp.Interface = childNode.InnerText;
                        break;
                    case "implementation":
                        outp.Implementation = childNode.InnerText;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            return outp;
        }

        private static ConstructionParameter ParseParameter(XmlNode node)
        {
            if (!Enum.TryParse(node.Name, true, out ConstructionType constructionType))
                throw new ArgumentException();

            if (node.Attributes is null)
                throw new ArgumentNullException();

            var nameAtr = node.Attributes["name"];
            if (nameAtr is null)
                throw new ArgumentNullException();

            var name = nameAtr.Value;

            string retType;
            if (node.Name == "expression")
                retType = node.Attributes["type"].Value;
            else if (node.Name == "block") retType = string.Empty;
            else throw new ArgumentException();

            return new ConstructionParameter(constructionType, retType, name);
        }
    }
}