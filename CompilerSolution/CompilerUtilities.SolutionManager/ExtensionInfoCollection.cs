using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace CompilerUtilities.SolutionManager
{
    [DataContract]
    public class ExtensionInfoCollection : IEnumerable<ExtensionInfo>
    {
        [DataMember(Name = "List")]
        private readonly List<ExtensionInfo> _extensions;

        public ExtensionInfoCollection()
        {
            _extensions = new List<ExtensionInfo>();
        }

        public int Count => _extensions.Count;
        public ExtensionInfo this[int index] => _extensions[index];

        public IEnumerator<ExtensionInfo> GetEnumerator()
        {
            return _extensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ExtensionInfo item)
        {
            _extensions.Add(item);
        }

        public void Add(string path)
        {
            var extAsm = Assembly.LoadFile(path);
            var ext = new ExtensionInfo(extAsm);
            _extensions.Add(ext);
        }

        public void Clear()
        {
            _extensions.Clear();
        }

        public void RemoveAt(int index)
        {
            _extensions.RemoveAt(index);
        }
    }
}