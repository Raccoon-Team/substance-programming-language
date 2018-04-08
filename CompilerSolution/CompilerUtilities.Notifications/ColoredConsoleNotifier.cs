using System;
using CompilerUtilities.Notifications.Interfaces;
using CompilerUtilities.Notifications.Structs.Enums;

namespace CompilerUtilities.Notifications
{
    public class ColoredConsoleNotifier:INotifier
    {
        private readonly INotifier _decoratedNotifier;
        private ConsoleColor _backupForegroundColor = Console.ForegroundColor;

        public ColoredConsoleNotifier() { }

        public ColoredConsoleNotifier(INotifier decoratedNotifier)
        {
            _decoratedNotifier = decoratedNotifier;
        }

        public async void Notify(NotifyLevel level, string message)
        {
            _decoratedNotifier?.Notify(level, message);

            SetColorByLevel(level);
            await Console.Out.WriteLineAsync($"{message}");
            ResetColor();
        }

        private void SetColorByLevel(NotifyLevel level)
        {
            _backupForegroundColor = Console.ForegroundColor;
            var newColor = Console.ForegroundColor;

            switch (level)
            {
                case NotifyLevel.Debug:
                    newColor = ConsoleColor.DarkGray;
                    break;
                case NotifyLevel.Info:
                    newColor = ConsoleColor.White;
                    break;
                case NotifyLevel.Warn:
                    newColor = ConsoleColor.DarkYellow;
                    break;
                case NotifyLevel.Error:
                    newColor = ConsoleColor.Red;
                    break;
                case NotifyLevel.Fatal:
                    newColor = ConsoleColor.DarkRed;
                    break;
            }

            Console.ForegroundColor = newColor;
        }

        private void ResetColor()
        {
            Console.ForegroundColor = _backupForegroundColor;
        }
    }
}