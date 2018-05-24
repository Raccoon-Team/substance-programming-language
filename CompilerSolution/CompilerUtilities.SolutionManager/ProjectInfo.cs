using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace CompilerUtilities.SolutionManager
{
    [DataContract]
    public class ProjectInfo
    {
        [DataMember(Name = "Extension files")] public List<string> ExtensionFiles;

        public bool IsChanged;
        [DataMember(Name = "Name")] public string Name;
        public string Path;
        [DataMember(Name = "Plugins")] public ExtensionInfoCollection Plugins;
        [DataMember(Name = "Source files")] public List<string> SourceFiles;
        [DataMember(Name = "Stages")] public ExtensionInfoCollection Stages;

        public ProjectInfo(string name)
        {
            Plugins = new ExtensionInfoCollection();
            Stages = new ExtensionInfoCollection();
            SourceFiles = new List<string>();
            ExtensionFiles = new List<string>();
            Path = string.Empty;
            Name = name;
            IsChanged = true;
        }

        public static ProjectInfo LoadProject(string projectFile)
        {
            var serializer = new DataContractJsonSerializer(typeof(ProjectInfo));

            ProjectInfo obj;

            using (var fs = new FileStream(projectFile, FileMode.Open))
            {
                obj = (ProjectInfo) serializer.ReadObject(fs);
            }
            obj.Path = projectFile;
            obj.IsChanged = false;
            return obj;
        }

        public void Save(string fileName = null)
        {
            if (!IsChanged) return;

            if (fileName is null)
                fileName = Path;

            var serializer = new DataContractJsonSerializer(typeof(ProjectInfo));
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                serializer.WriteObject(fs, this);
            }
            var text = File.ReadAllText(Path);

            text = JsonFormatter.FormatJson(text);
            File.WriteAllText(Path, text);
        }
    }
}