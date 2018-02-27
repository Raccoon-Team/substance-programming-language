using System;
using CompilerUtilities.Notifications.Interfaces;
using CompilerUtilities.Notifications.Structs.Enums;

namespace CompilerUtilities.Notifications
{
    public class ColoredConsoleNotifier:INotifier
    {
        private INotifier _decoratedNotifier;
        private ConsoleColor _backupForegroundColor = Console.ForegroundColor;

        public ColoredConsoleNotifier() { }

        public ColoredConsoleNotifier(INotifier decoratedNotifier)
        {
            _decoratedNotifier = decoratedNotifier;
        }

        public void Notify(NotifyLevel level, string message)
        {
            SetColorByLevel(level);
            Console.Out.WriteLine($"{level.ToString()}:{0}");
            ResetColor();

            _decoratedNotifier?.Notify(level, message);
        }

        private void SetColorByLevel(NotifyLevel level)
        {
            _backupForegroundColor = Console.ForegroundColor;
            var newColor = Console.ForegroundColor;

            switch (level)
            {
                case NotifyLevel.Debug:
                    newColor = ConsoleColor.Gray;
                    break;
                case NotifyLevel.Info:
                    newColor = ConsoleColor.White;
                    break;
                case NotifyLevel.Warn:
                    newColor = ConsoleColor.DarkYellow;
                    break;
                case NotifyLevel.Error:
                case NotifyLevel.Fatal:
                    newColor = ConsoleColor.Red;
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