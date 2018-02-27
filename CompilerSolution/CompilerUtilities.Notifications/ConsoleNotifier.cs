using System;
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

        public void Notify(NotifyLevel level, string message)
        {
            Console.Out.WriteLine($"{level.ToString()}:{0}");
            _decoratedNotifier?.Notify(level, message);
        }
    }
}