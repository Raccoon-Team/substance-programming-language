using System;
using System.Collections.Generic;

namespace CompilerUtilities.BaseTypes.Interfaces
{
    public interface ITextProcessor
    {
         string this[int index] { get; set; }
         int Length { get; }

         IEnumerable<string> Presentation { get; set; }
         string Cut(int lineIndex);
         string[] CutRange(int beginIndex, int endIndex);

         void Insert(int lineIndex, string newLine);
         void InsertRange(int beginIndex, string[] newLines);

         int FindIndex(string targetLine);
         int FindIndex(Predicate<string> predicate);

         string Find(Predicate<string> predicate);

         string[] GetRange(int beginIndex, int endIndex);

         void LoadFromFile(string path);
         void SaveToFile(string path);
    }
}