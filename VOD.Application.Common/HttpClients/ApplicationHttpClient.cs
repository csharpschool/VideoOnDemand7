using static System.Net.WebRequestMethods;

namespace VOD.Application.HttpClients;

public class ApplicationHttpClient
{
    public HttpClient Client { get; }

    public ApplicationHttpClient(HttpClient httpClient)
    {
        Client = httpClient;
    }

    public void AddBearerToken(string token)
    {
        Client.DefaultRequestHeaders.Remove("Authorization");
        Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }
}
