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
            var TodayLogDirectoryPath = Path.Combine(DefaultLogDirectory, DateTime.Now.ToShortDateString());
            Directory.CreateDirectory(TodayLogDirectoryPath);

            var TimeForLogFileName = DateTime.Now.ToShortTimeString().Replace(':', ';');
            var LogFilePath = Path.Combine(TodayLogDirectoryPath, $"{TimeForLogFileName}.txt");
            _defaultNotifier = new FileNotifier(anotherNotifier, LogFilePath);
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