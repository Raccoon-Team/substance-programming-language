using System;
using System.IO;
using CompilerUtilities.Notifications.Interfaces;
using CompilerUtilities.Notifications.Structs.Enums;

namespace CompilerUtilities.Notifications
{
    public static class DefaultNotifier
    {
        public static string DefaultLogDirectory = "Logs";

        private static INotifier _defaultNotifier;

        public static void InitializeFileLoggerNotifier(INotifier anotherNotifier)
        {
            var todayLogDirectoryPath = Path.Combine(DefaultLogDirectory, DateTime.Now.ToShortDateString());
            Directory.CreateDirectory(todayLogDirectoryPath);

            var timeForLogFileName = DateTime.Now.ToShortTimeString().Replace(':', ';');
            var logFilePath = Path.Combine(todayLogDirectoryPath, $"{timeForLogFileName}.txt");
            _defaultNotifier = new FileNotifier(anotherNotifier, logFilePath);
        }

        public static void InitializeNotifier(INotifier newNotifier)
        {
            _defaultNotifier = newNotifier;
        }

        public static void Notify(NotifyLevel level, string message)
        {
            _defaultNotifier?.Notify(level, message);
        }
    }
}