using System.IO;
using CompilerUtilities.Notifications.Interfaces;
using CompilerUtilities.Notifications.Structs.Enums;

namespace CompilerUtilities.Notifications
{
    public class FileNotifier:INotifier
    {
        private readonly StreamWriter _fileWriter;
        private readonly INotifier _decoratedNotifier;

        public FileNotifier(string path, FileMode fileMode)
        {
            _fileWriter = new StreamWriter(new FileStream(path, fileMode));
        }

        public FileNotifier(INotifier decoratedNotifier, string path, FileMode fileMode):this(path, fileMode)
        {
            _decoratedNotifier = decoratedNotifier;
        }

        public async void Notify(NotifyLevel level, string message)
        {
            _decoratedNotifier?.Notify(level, message);
            await _fileWriter.WriteAsync($"{level.ToString()}:{message}");
        }
    }
}