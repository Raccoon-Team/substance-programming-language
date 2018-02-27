using CompilerUtilities.Notifications.Structs.Enums;

namespace CompilerUtilities.Notifications.Interfaces
{
    public interface INotifier
    {
        void Notify(NotifyLevel level, string message);
    }
}