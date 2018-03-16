using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using CompilerUtilities.Notifications.Interfaces;
using CompilerUtilities.Notifications.Structs.Enums;

namespace CompilerUtilities.Notifications
{
    public class FileNotifier : INotifier
    {
        private readonly INotifier _decoratedNotifier;
        private readonly StreamWriter _fileWriter;
        private readonly BlockingCollection<(NotifyLevel level, string message)> _queueMessages;

        private Task _messageLoopTask = Task.Run(() => { });

        public FileNotifier(string path)
        {
            var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write, 4096, true);
            _fileWriter = new StreamWriter(stream);

            _queueMessages = new BlockingCollection<(NotifyLevel level, string message)>();
        }


        public FileNotifier(INotifier decoratedNotifier, string path) : this(path)
        {
            _decoratedNotifier = decoratedNotifier;
        }

        public void Notify(NotifyLevel level, string message)
        {
            _decoratedNotifier?.Notify(level, message);
            _queueMessages.Add((level, message));

            if (_messageLoopTask.IsCompleted)
                _messageLoopTask = Task.Run(() => MessageProccessingLoop());
        }

        private void NotifyAsync(NotifyLevel level, string message)
        {
            _fileWriter.WriteLine($"{level.ToString()}:{message}");
            _fileWriter.Flush();
        }

        private void MessageProccessingLoop()
        {
            while (_queueMessages.Count > 0)
            {
                var args = _queueMessages.Take();
                NotifyAsync(args.level, args.message);
            }
        }
    }
}