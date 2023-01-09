namespace VOD.Authentication.HttpClients;

public  class AuthenticationHttpClient
{
    public HttpClient Client { get; }

    public AuthenticationHttpClient(HttpClient httpClient)
    {
        Client = httpClient;
    }
}
