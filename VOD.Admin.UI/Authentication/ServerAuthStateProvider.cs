namespace VOD.Admin.UI.Authentication;

public class ServerAuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationHttpClient _http;
    private readonly ISessionStorage _sessionStorage;
    private readonly AuthenticationState _anonymous;

    public ServerAuthStateProvider(AuthenticationHttpClient httpClient, ISessionStorage sessionStorage)
    {
        _http = httpClient;
        _sessionStorage = sessionStorage;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _sessionStorage.GetAsync("authToken");

        if (string.IsNullOrWhiteSpace(token)) return _anonymous;

        _http.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJWT(token), "jwtAuthType")));
    }

    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJWT(token), "jwtAuthType"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(_anonymous);
        NotifyAuthenticationStateChanged(authState);
    }
}
