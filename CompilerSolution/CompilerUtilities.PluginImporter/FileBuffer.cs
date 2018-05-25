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

        public byte[] GetBytes(string path)
        {
            if (files.ContainsKey(path))
                return files[path];
            var bytes = File.ReadAllBytes(path);
            files.Add(path, bytes);
            return bytes;
        }

        public IEnumerable<string> GetLines(string path)
        {
            var sb = new StringBuilder();
            var bytes = files[path];

            var length = bytes.Length;
            for (var i = 0; i < length; i++)
            {
                var isR = bytes[i] == '\r';
                if (isR || bytes[i] == '\n')
                {
                    var nextIndex = i + 1;
                    if (isR && nextIndex < length && bytes[nextIndex] == '\n')
                        i = nextIndex;

                    var str = sb.ToString();
                    sb.Clear();
                    yield return str;
                }

                if (i == length) break;

                sb.Append(bytes[i]);
            }
        }

        public List<string> GetAllLines(string path)
        {
            return GetLines(path).ToList();
        }

        public void Refresh(string path)
        {
            files[path] = File.ReadAllBytes(path);
        }

        public void Refresh(string path, IList<string> lines)
        {
            throw new NotImplementedException();
        }

        public void RefreshAll()
        {
            var keys = files.Keys;

            foreach (var key in keys)
                files[key] = File.ReadAllBytes(key);
        }
    }
}