using System.IO;
using CompilerUtilities.Notifications.Interfaces;
using CompilerUtilities.Notifications.Structs.Enums;

namespace CompilerUtilities.Notifications
{
    public class FileNotifier:INotifier
    {
        private StreamWriter _fileWriter;
        private INotifier _decoratedNotifier;
        private static object _notifySyncObject;

        public FileNotifier(string path, FileMode fileMode)
        {
            _fileWriter = new StreamWriter(new FileStream(path, fileMode));
        }

        public FileNotifier(INotifier decoratedNotifier, string path, FileMode fileMode):this(path, fileMode)
        {
            _decoratedNotifier = decoratedNotifier;
        }

        public void Notify(NotifyLevel level, string message)
        {
            lock (_notifySyncObject)
                _fileWriter.Write($"{level.ToString()}:{message}");
            _decoratedNotifier?.Notify(level, message);
        }
    }
}