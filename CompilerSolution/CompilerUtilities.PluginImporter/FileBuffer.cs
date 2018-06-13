using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CompilerUtilities.Plugins.Contract;

namespace CompilerUtilities.PluginImporter
{
    internal class FileBuffer : IFileBuffer
    {
        private readonly Dictionary<string, byte[]> files;

        public FileBuffer()
        {
            files = new Dictionary<string, byte[]>();
        }

        public string GetAllText(string path)
        {
            return Encoding.UTF8.GetString(this[path]);
        }

        public IEnumerable<string> GetLines(string path)
        {
            var sb = new StringBuilder();
            var bytes = this[path];

            var length = bytes.Length;
            for (var i = 0; i < length; i++)
            {
                var @byte = bytes[i];

                var isR = @byte == '\r';
                if (isR || @byte == '\n')
                {
                    var nextIndex = i + 1;
                    if (isR && nextIndex < length && bytes[nextIndex] == '\n')
                        i = nextIndex;

                    var str = sb.ToString();
                    sb.Clear();
                    yield return str;
                }

                if (i == length) break;

                sb.Append(@byte);
            }
        }

        public List<string> GetAllLines(string path)
        {
            return GetLines(path).ToList();
        }

        public void Refresh(string path)
        {
            path = Path.GetFullPath(path);
            files[path] = File.ReadAllBytes(path);
        }

        public void Refresh(string path, IList<string> lines)
        {
            path = Path.GetFullPath(path);
            files[path] = lines.Select(s => s.Select(c => (byte) c)).SelectMany(bytes => bytes).ToArray();
        }

        public void Refresh(string path, byte[] bytes)
        {
            path = Path.GetFullPath(path);
            files[path] = bytes;
        }

        public void RefreshAll()
        {
            var keys = files.Keys;

            foreach (var key in keys)
                files[key] = File.ReadAllBytes(key);
        }

        public byte[] this[string path]
        {
            get
            {
                path = Path.GetFullPath(path);
                return files[path];
            }
            set
            {
                path = Path.GetFullPath(path);
                files[path] = value;
            }
        }
    }
}