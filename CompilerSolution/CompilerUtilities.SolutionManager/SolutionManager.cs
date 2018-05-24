using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace CompilerUtilities.SolutionManager
{
    [DataContract]
    public class SolutionManager
    {
        private string _path;

        [DataMember(Name = "Project files")] private List<string> _projectFiles;

        public List<ProjectInfo> Projects;

        public SolutionManager()
        {
            Projects = new List<ProjectInfo>();
            _projectFiles = new List<string>();
        }

        public void AddProject(ProjectInfo projectInfo)
        {
            Projects.Add(projectInfo);
            _projectFiles.Add(projectInfo.Path);
        }

        public static SolutionManager Load(string projectFile)
        {
            var serializer = new DataContractJsonSerializer(typeof(SolutionManager));

            SolutionManager obj;

            using (var fs = new FileStream(projectFile, FileMode.Open))
            {
                obj = (SolutionManager) serializer.ReadObject(fs);
            }
            obj._path = projectFile;
            obj.Projects = obj._projectFiles.Select(ProjectInfo.LoadProject).ToList();
            return obj;
        }

        public void Save(string fileName = null)
        {
            if (fileName is null)
                fileName = _path;

            var serializer = new DataContractJsonSerializer(typeof(SolutionManager));
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                serializer.WriteObject(fs, this);
            }
            var text = File.ReadAllText(fileName);

            text = JsonFormatter.FormatJson(text);
            File.WriteAllText(fileName, text);

            var projectCoun = Projects.Count;
            for (var i = 0; i < projectCoun; i++)
                Projects[i].Save(Path.Combine(Path.GetDirectoryName(fileName), Projects[i].Path));
        }
    }
}