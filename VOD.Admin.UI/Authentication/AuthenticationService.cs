namespace VOD.Admin.UI.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticationHttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ISessionStorage _sessionStorage;

    public AuthenticationService(AuthenticationHttpClient httpClient, AuthenticationStateProvider authStateProvider, ISessionStorage sessionStorage)
    {
        _http = httpClient;
        _authStateProvider = authStateProvider;
        _sessionStorage = sessionStorage;
    }

    public async Task<AuthenticatedUserDTO?> Login(AuthenticationUserModel userForAuthentication)
    {
        try
        {
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
                var updateTokenUser = new UpdateUserTokenDTO(userForAuthentication.Email);
                using StringContent jsonUpdateTokenUser = new(
                    JsonSerializer.Serialize(updateTokenUser),
                    Encoding.UTF8,
                    "application/json");

                using HttpResponseMessage createResponse = await _http.Client.PostAsync("token/create", jsonUpdateTokenUser);

                createResponse.EnsureSuccessStatusCode();

                using HttpResponseMessage fetchResponse = await _http.Client.PostAsync("token", jsonContent);
                fetchResponse.EnsureSuccessStatusCode();
                responseContent = await fetchResponse.Content.ReadAsStringAsync();
            }

            var result = JsonSerializer.Deserialize<AuthenticatedUserDTO>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result is null) return default;

            await _sessionStorage.SetAsync("authToken", result.AccessToken);

            ((ServerAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.AccessToken);

            _http.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);

            return result;
        }
        catch
        {
            return default;
        }
    }

    public async Task Logout()
    {
        await _sessionStorage.RemoveAsync("authToken");
        ((ServerAuthStateProvider)_authStateProvider).NotifyUserLogout();
        _http.Client.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<TokenUserDTO?> GetUserFromToken()
    {
        var token = await _sessionStorage.GetAsync("authToken");

        if (string.IsNullOrWhiteSpace(token)) return default;

        return JwtParser.ParseUserInfoFromJWT(token);
    }
}
