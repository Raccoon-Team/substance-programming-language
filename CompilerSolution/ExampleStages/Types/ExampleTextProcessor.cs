using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompilerUtilities.Plugins.Contract.Interfaces;

namespace ExampleStages.Types
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

        public List<string> CutRange(int beginIndex, int endIndex)
        {
            string[] CutOperation(int begin, int end)
            {
                var count = end - begin + 1;
                var tmpLines = _lines.GetRange(begin, count);
                _lines.RemoveRange(begin, count);
                return tmpLines.ToArray();
            }

            return RangeOperation(beginIndex, endIndex, CutOperation);
        }

        public List<string> CutAll(Predicate<string> predicate)
        {
            var length = _lines.Count;
            var outp = new List<string>();
            for (var i = 0; i < length; i++)
            {
                var line = _lines[i];

                if (!predicate(line))
                    continue;

                outp.Add(line);
                _lines.RemoveAt(i);
                i--;
            }
            return outp;
        }

        public bool Remove(string targetLine)
        {
            return _lines.Remove(targetLine);
        }

        public void RemoveRange(int beginIndex, int endIndex)
        {
            _lines.RemoveRange(beginIndex, endIndex - beginIndex + 1);
        }

        public void RemoveAt(int index)
        {
            _lines.RemoveAt(index);
        }

        public void RemoveAll(Predicate<string> predicate)
        {
            _lines.RemoveAll(predicate);
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

        public int[] FindIndexes(Predicate<string> predicate)
        {
            var indexes = new List<int>();
            for (var i = 0; i < _lines.Count; i++)
                if (predicate(_lines[i]))
                    indexes.Add(i);
            return indexes.ToArray();
        }

        public string Find(Predicate<string> predicate)
        {
            return _lines.Find(predicate);
        }

        public List<string> FindAll(Predicate<string> predicate)
        {
            return _lines.FindAll(predicate);
        }

        public List<string> GetRange(int beginIndex, int endIndex)
        {
            List<string> GetOperation(int begin, int end)
            {
                return _lines.GetRange(begin, end - begin + 1);
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

        private static void Swap(ref int first, ref int second)
        {
            var tmp = first;
            first = second;
            second = tmp;
        }

        private static List<string> RangeOperation(int beginIndex, int endIndex,
            Func<int, int, IList<string>> operation)
        {
            var resultReverse = false;

            if (endIndex < beginIndex)
            {
                resultReverse = true;
                Swap(ref beginIndex, ref endIndex);
            }

            var tmpLines = operation(beginIndex, endIndex).ToList();

            if (resultReverse)
                tmpLines.Reverse();

            return tmpLines;
        }

        private static void RangeOperation(int beginIndex, int endIndex,
            Action<int, int> operation)
        {
            if (endIndex < beginIndex)
                Swap(ref beginIndex, ref endIndex);

            operation(beginIndex, endIndex);
        }

        public override string ToString()
        {
            return string.Join("\r\n", _lines);
        }
    }
}