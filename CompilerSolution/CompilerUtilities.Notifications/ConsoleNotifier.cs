using System;
using System.Threading;
using CompilerUtilities.Notifications.Interfaces;
using CompilerUtilities.Notifications.Structs.Enums;

namespace CompilerUtilities.Notifications
{
    public class ConsoleNotifier:INotifier
    {
        private INotifier _decoratedNotifier;

        public ConsoleNotifier() {}

        public ConsoleNotifier(INotifier decoratedNotifier)
        {
            _decoratedNotifier = decoratedNotifier;
        }

        public async void Notify(NotifyLevel level, string message)
        {
            _decoratedNotifier?.Notify(level, message);
            await Console.Out.WriteLineAsync($"{level.ToString()}:{message}");
        }
    }
}