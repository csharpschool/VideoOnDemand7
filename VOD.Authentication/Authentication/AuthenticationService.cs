using VOD.Authentication.HttpClients;

namespace VOD.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticationHttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(AuthenticationHttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _http = httpClient;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<AuthenticatedUserDTO?> Login(AuthenticationUserModel userForAuthentication)
    {
        try
        {
            string? accessToken;

            var user = new LoginUserDTO(userForAuthentication.Email, userForAuthentication.Password);
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _http.Client.PostAsync("token", jsonContent);

            //response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized) || string.IsNullOrWhiteSpace(responseContent))
            {
                using HttpResponseMessage createResponse = await _http.Client.PostAsync("token/create", jsonContent);

                createResponse.EnsureSuccessStatusCode();

                using HttpResponseMessage fetchResponse = await _http.Client.PostAsync("token", jsonContent);
                fetchResponse.EnsureSuccessStatusCode();
                responseContent = await fetchResponse.Content.ReadAsStringAsync();
            }

            var result = JsonSerializer.Deserialize<AuthenticatedUserDTO>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result is null) return default;

            await _localStorage.SetItemAsync("authToken", result.AccessToken);

            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.AccessToken);

            _http.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);

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
        _http.Client.DefaultRequestHeaders.Authorization = null;
    }
}
