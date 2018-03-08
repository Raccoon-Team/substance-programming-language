using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CompilerUtilities.BaseTypes.Abstract;

namespace CompilerUtilities.BaseTypes
{
    public class CodeProcessor:TextProcessor
    {
        private List<string> _lines;

        public override string Cut(int lineIndex)
        {
            var tmp = _lines[lineIndex];
            _lines.RemoveAt(lineIndex);
            return tmp;
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

            var tmpLines = operation(beginIndex, endIndex);
            
            if (resultReverse)
                tmpLines.Reverse();

            return tmpLines;
        }

        public override string[] CutRange(int beginIndex, int endIndex)
        {
            string[] CutOperation(int begin, int end)
            {
                var tmpLines = _lines.GetRange(beginIndex, endIndex - beginIndex + 1);
                _lines.RemoveRange(beginIndex, endIndex - beginIndex + 1);
                return tmpLines.ToArray();
            }

            return RangeOperation(beginIndex, endIndex, CutOperation);
        }

        public override void Insert(int lineIndex, string newLine) 
            => _lines.Insert(lineIndex, newLine);

        public override void InsertRange(int beginIndex, string[] newLines) 
            => _lines.InsertRange(beginIndex, newLines);

        public override int FindIndex(string targetLine) 
            => _lines.IndexOf(targetLine);

        public override int FindIndex(Predicate<string> predicate) 
            => _lines.FindIndex(predicate);

        public override string Find(Predicate<string> predicate) 
            => _lines.Find(predicate);

        public override string[] GetRange(int beginIndex, int endIndex)
        {
            string[] GetOperation(int begin, int end) => _lines.GetRange(begin, end - begin + 1).ToArray();
            return RangeOperation(beginIndex, endIndex, GetOperation);
        }

        public override void LoadFromFile(string path)
            => _lines = File.ReadAllLines(path).ToList();

        public override void SaveToFile(string path)
            => File.WriteAllLines(path, _lines);
        

        public override string this[int index]
        {
            get => _lines[index];
            set => _lines[index] = value;
        }

        public override int Length
        {
            get => _lines.Count;
        }

        public override string ToString()
            => String.Join("\n", _lines);

        public override IEnumerable<string> GetPresentation()
        {
            return _lines;
        }
    }
}
