using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompilerUtilities.Plugins.Contract.Interfaces;

namespace CompilerUtilities.BaseTypes
{
    public class CodeProcessor : ITextProcessor
    {
        private List<string> _lines;

        public CodeProcessor()
        {
            Presentation = Enumerable.Empty<string>();
        }

        public CodeProcessor(IEnumerable<string> lines)
        {
            Presentation = lines;
        }

        public CodeProcessor(string path)
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
                var tmpLines = _lines.GetRange(begin, end - begin + 1);
                _lines.RemoveRange(begin, end - begin + 1);
                return tmpLines.ToArray();
            }

            return RangeOperation(beginIndex, endIndex, CutOperation);
        }

        public List<string> CutAll(Predicate<string> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string targetLine)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(int beginIndex, int endIndex)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(Predicate<string> predicate)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public string Find(Predicate<string> predicate)
        {
            return _lines.Find(predicate);
        }

        public List<string> FindAll(Predicate<string> predicate)
        {
            throw new NotImplementedException();
        }

        public List<string> GetRange(int beginIndex, int endIndex)
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

        private static void Swap(ref int first, ref int second)
        {
            var tmp = first;
            first = second;
            second = tmp;
        }

        private static List<string> RangeOperation(int beginIndex, int endIndex, Func<int, int, IList<string>> operation)
        {
            var resultReverse = false;

            if (endIndex < beginIndex)
            {
                resultReverse = true;
                Swap(ref beginIndex, ref endIndex);
            }

            var tmpLines = operation(beginIndex, endIndex);

            if (resultReverse)
                tmpLines.Reverse();

            return tmpLines.ToList();
        }

        public override string ToString()
        {
            return string.Join("\n", _lines);
        }
    }
}