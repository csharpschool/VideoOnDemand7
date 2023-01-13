namespace VOD.Admin.UI.Authentication
{
    public interface ISessionStorage
    {
        Task<string> GetAsync(string key);
        Task RemoveAsync(string key);
        Task SetAsync(string key, object value);
    }
}