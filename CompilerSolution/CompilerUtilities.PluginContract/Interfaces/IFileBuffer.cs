using System.Collections.Generic;

namespace CompilerUtilities.Plugins.Contract
{
    public interface IFileBuffer
    {
        List<string> GetAllLines(string path);
        byte[] GetBytes(string path);
        IEnumerable<string> GetLines(string path);
        void Refresh(string path);
        void Refresh(string path, IList<string> lines);
        void RefreshAll();
    }
}