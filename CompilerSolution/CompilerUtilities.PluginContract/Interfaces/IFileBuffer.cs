using System.Collections.Generic;

namespace CompilerUtilities.Plugins.Contract
{
    public interface IFileBuffer
    {
        List<string> GetAllLines(string path);
        string GetAllText(string path);
        byte[] this[string path] { get; set; }
        IEnumerable<string> GetLines(string path);
        void Refresh(string path);
        void Refresh(string path, IList<string> lines);
        void RefreshAll();
    }
}