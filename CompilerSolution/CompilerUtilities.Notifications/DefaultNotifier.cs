using CompilerUtilities.Notifications.Interfaces;
using CompilerUtilities.Notifications.Structs.Enums;

namespace CompilerUtilities.Notifications
{
    public static class DefaultNotifier
    {
        private static INotifier _defaultNotifier;

        public static void InitializeNotifier(INotifier newNotifier)
        {
            _defaultNotifier = newNotifier;
        }

        public static void Notify(NotifyLevel level, string message)
        {
            _defaultNotifier.Notify(level, message);
        }
    }
}