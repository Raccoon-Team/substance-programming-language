using System;
using System.Collections.Generic;

namespace CompilerUtilities.BaseTypes.Abstract
{
    public abstract class TextProcessor
    {
        public abstract string this[int index] { get; set; }
        public abstract int Length { get; }

        public abstract IEnumerable<string> Presentation { get; set; }
        public abstract string Cut(int lineIndex);
        public abstract string[] CutRange(int beginIndex, int endIndex);

        public abstract void Insert(int lineIndex, string newLine);
        public abstract void InsertRange(int beginIndex, string[] newLines);

        public abstract int FindIndex(string targetLine);
        public abstract int FindIndex(Predicate<string> predicate);

        public abstract string Find(Predicate<string> predicate);

        public abstract string[] GetRange(int beginIndex, int endIndex);

        public abstract void LoadFromFile(string path);
        public abstract void SaveToFile(string path);

        public abstract override string ToString();
    }
}