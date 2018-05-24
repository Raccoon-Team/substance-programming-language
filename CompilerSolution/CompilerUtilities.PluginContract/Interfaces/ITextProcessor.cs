using System;
using System.Collections.Generic;

namespace CompilerUtilities.Plugins.Contract
{
    public interface ITextProcessor
    {
        string this[int index] { get; set; }
        int Length { get; }

        IEnumerable<string> Presentation { get; set; }

        string Cut(int lineIndex);
        List<string> CutRange(int beginIndex, int endIndex);
        List<string> CutAll(Predicate<string> predicate);

        bool Remove(string targetLine);
        void RemoveRange(int beginIndex, int endIndex);
        void RemoveAt(int index);
        void RemoveAll(Predicate<string> predicate);

        void Insert(int lineIndex, string newLine);
        void InsertRange(int beginIndex, string[] newLines);

        int FindIndex(string targetLine);
        int FindIndex(Predicate<string> predicate);
        int[] FindIndexes(Predicate<string> predicate);

        string Find(Predicate<string> predicate);
        List<string> FindAll(Predicate<string> predicate);

        List<string> GetRange(int beginIndex, int endIndex);

        void LoadFromFile(string path);
        void SaveToFile(string path);
    }
}