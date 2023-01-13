using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace VOD.Admin.UI.Authentication;

/* ONLY for Blazor Server-Side */
public class SessionStorage : ISessionStorage
{
    private readonly ProtectedSessionStorage _store;

    public SessionStorage(ProtectedSessionStorage store)
    {
        _store = store;
    }

    public async Task SetAsync(string key, object value) => await _store.SetAsync(key, value);
    public async Task<string> GetAsync(string key)
    {
        try
        {
            var result = await _store.GetAsync<string>(key);

            if (result.Success)
                return result.Value ?? string.Empty;
        }
        catch
        {
        }

        return string.Empty;
    }
    public async Task RemoveAsync(string key)
    {
        try
        {
            await _store.DeleteAsync(key);
        }
        catch
        {
            throw;
        }
    }

}
