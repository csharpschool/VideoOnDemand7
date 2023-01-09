namespace VOD.Authentication.HttpClients;

public class UserHttpClient
{
    public HttpClient Client { get; }

    public UserHttpClient(HttpClient httpClient)
    {
        Client = httpClient;
    }
}
