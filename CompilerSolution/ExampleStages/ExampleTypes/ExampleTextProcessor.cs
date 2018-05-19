using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompilerUtilities.Plugins.Contract.Interfaces;

namespace ExampleStages.ExampleTypes
{
    public class ExampleTextProcessor : ITextProcessor
    {
        private List<string> _lines;

        public ExampleTextProcessor()
        {
            Presentation = Enumerable.Empty<string>();
        }

        public ExampleTextProcessor(IEnumerable<string> lines)
        {
            Presentation = lines;
        }

        public ExampleTextProcessor(string path)
        {
            LoadFromFile(path);
        }

        public IEnumerable<string> Presentation
        {
            get => _lines;
            set => _lines = value.ToList();
        }

        public string this[int index]
        {
            get => _lines[index];
            set => _lines[index] = value;
        }

        public int Length => _lines.Count;

        public string Cut(int lineIndex)
        {
            var tmp = _lines[lineIndex];
            _lines.RemoveAt(lineIndex);
            return tmp;
        }

        public string[] CutRange(int beginIndex, int endIndex)
        {
            string[] CutOperation(int begin, int end)
            {
                var tmpLines = _lines.GetRange(begin, end - begin + 1);
                _lines.RemoveRange(begin, end - begin + 1);
                return tmpLines.ToArray();
            }

            return RangeOperation(beginIndex, endIndex, CutOperation);
        }

        public void Insert(int lineIndex, string newLine)
        {
            _lines.Insert(lineIndex, newLine);
        }

        public void InsertRange(int beginIndex, string[] newLines)
        {
            _lines.InsertRange(beginIndex, newLines);
        }

        public int FindIndex(string targetLine)
        {
            return _lines.IndexOf(targetLine);
        }

        public int FindIndex(Predicate<string> predicate)
        {
            return _lines.FindIndex(predicate);
        }

        public string Find(Predicate<string> predicate)
        {
            return _lines.Find(predicate);
        }

        public string[] GetRange(int beginIndex, int endIndex)
        {
            string[] GetOperation(int begin, int end)
            {
                return _lines.GetRange(begin, end - begin + 1).ToArray();
            }

            return RangeOperation(beginIndex, endIndex, GetOperation);
        }

        public void LoadFromFile(string path)
        {
            _lines = File.ReadAllLines(path).ToList();
        }

        public void SaveToFile(string path)
        {
            File.WriteAllLines(path, _lines);
        }

        private void Swap(ref int first, ref int second)
        {
            var tmp = first;
            first = second;
            second = tmp;
        }

        private string[] RangeOperation(int beginIndex, int endIndex, Func<int, int, string[]> operation)
        {
            var resultReverse = false;

            if (endIndex < beginIndex)
            {
                resultReverse = true;
                Swap(ref beginIndex, ref endIndex);
            }

            IEnumerable<string> tmpLines = operation(beginIndex, endIndex);

            if (resultReverse)
                tmpLines = tmpLines.Reverse();

            return tmpLines.ToArray();
        }

        public override string ToString()
        {
            return string.Join("\n", _lines);
        }
    }
}