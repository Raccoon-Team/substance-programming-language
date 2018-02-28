using System;
using System.IO;
using CompilerUtilities.Notifications.Interfaces;
using CompilerUtilities.Notifications.Structs.Enums;

namespace CompilerUtilities.Notifications
{
    public class FileNotifier:INotifier
    {
        private readonly StreamWriter _fileWriter;
        private readonly INotifier _decoratedNotifier;
        private object _syncWriteObject = new object();

        public FileNotifier(string path)
        {
            _fileWriter = new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write, 4096, true));
        }

        public FileNotifier(INotifier decoratedNotifier, string path):this(path)
        {
            _decoratedNotifier = decoratedNotifier;
        }

        public async void Notify(NotifyLevel level, string message)
        {
            _decoratedNotifier?.Notify(level, message);
            await _fileWriter.WriteLineAsync($"{level.ToString()}:{message}\n");
            await _fileWriter.FlushAsync();
        }
    }
}