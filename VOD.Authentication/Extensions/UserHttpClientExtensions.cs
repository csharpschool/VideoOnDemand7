namespace VOD.Authentication.Extensions;
public static class UserHttpClientExtensions
{
    public static async Task CreateUser(this UserHttpClient http, CreateUserModel model)
    {
        try
        {
            if (model == null) throw new ArgumentException("CreateUserModel is null.");

            var roles = new List<string> { UserRole.Registered };
            if(model.IsCustomer) roles.Add(UserRole.Customer);
            if(model.IsAdmin) roles.Add(UserRole.Admin);

            var user = new RegisterUserDTO(model.Email, model.Password, roles);

            using StringContent jsonContent = new(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8,
                    "application/json");

            using HttpResponseMessage response = await http.Client.PostAsync("users/register", jsonContent);
            if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
        }
        catch
        {
            throw;
        }
    }
    public static async Task PaidCustomer(this UserHttpClient http, UpdateUserTokenDTO user, AuthenticationHttpClient authHttp)
    {
        try
        {
            if (user == null) throw new ArgumentException("UpdateUserTokenDTO is null.");

            using StringContent jsonContent = new(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8,
                    "application/json");

            using HttpResponseMessage response = await http.Client.PostAsync("users/paid", jsonContent);
            if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

            using HttpResponseMessage tokenResponse = await authHttp.Client.PostAsync("token/create", jsonContent);
            if (!tokenResponse.IsSuccessStatusCode) throw new Exception(tokenResponse.ReasonPhrase);
        }
        catch
        {
            throw;
        }
    }
}
