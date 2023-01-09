namespace VOD.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<AuthenticatedUserDTO?> Login(AuthenticationUserModel userForAuthentication)
    {
        try
        {

            var data = new LoginUserDTO(userForAuthentication.Email, userForAuthentication.Password);
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync("token", jsonContent);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<AuthenticatedUserDTO>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result is null) return default;

            await _localStorage.SetItemAsync("authToken", result.AccessToken);

            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.AccessToken);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);

            return result;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
