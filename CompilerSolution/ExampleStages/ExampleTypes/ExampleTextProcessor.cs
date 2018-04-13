using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompilerUtilities.BaseTypes.Interfaces;

namespace ExampleStages.ExampleTypes
{
    public class ExampleTextProcessor : ITextProcessor
    {
        private IList<string> _presentation;

        public ExampleTextProcessor(string fileName)
        {
            Presentation = File.ReadAllLines(fileName);
            Length = Presentation.Count();
        }

        public ExampleTextProcessor()
        {
            
        }

        public string this[int index]
        {
            get => _presentation[index];
            set => _presentation[index] = value;
        }

        public int Length { get; private set; }

        public IEnumerable<string> Presentation
        {
            get => _presentation;
            set
            {
                _presentation = value.ToList();
                Length = _presentation.Count;
            }
        }

        public string Cut(int lineIndex)
        {
            throw new NotImplementedException();
        }

        public string[] CutRange(int beginIndex, int endIndex)
        {
            throw new NotImplementedException();
        }

        public void Insert(int lineIndex, string newLine)
        {
            throw new NotImplementedException();
        }

        public void InsertRange(int beginIndex, string[] newLines)
        {
            throw new NotImplementedException();
        }

        public int FindIndex(string targetLine)
        {
            throw new NotImplementedException();
        }

        public int FindIndex(Predicate<string> predicate)
        {
            throw new NotImplementedException();
        }

        public string Find(Predicate<string> predicate)
        {
            throw new NotImplementedException();
        }

        public string[] GetRange(int beginIndex, int endIndex)
        {
            throw new NotImplementedException();
        }

        public void LoadFromFile(string path)
        {
            throw new NotImplementedException();
        }

        public void SaveToFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}