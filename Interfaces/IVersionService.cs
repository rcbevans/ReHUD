namespace ReHUD.Interfaces
{
    public interface IVersionService
    {
        Task<string> GetCurrentAppVersion();
        Task CheckForUpdates();
    }
}
