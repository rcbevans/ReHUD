namespace ReHUD.Interfaces
{
    public enum MessageBoxType
    {
        none,
        info,
        error,
        question,
        warning
    }

    public interface IMessageBoxService
    {
        Task<bool> ShowYesNoDialog(string title, string message, MessageBoxType? type);
        Task<bool> ShowOkCancelDialog(string title, string message, MessageBoxType? type);
    }
}
