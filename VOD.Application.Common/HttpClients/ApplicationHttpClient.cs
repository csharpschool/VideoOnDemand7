namespace VOD.Application.HttpClients;

public class ApplicationHttpClient
{
    public HttpClient Client { get; }

    public ApplicationHttpClient(HttpClient httpClient)
    {
        Client = httpClient;
    }
}
