using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;

namespace VOD.UI.Extensions;
static class UserRoles
{
    public static string Admin => "Admin";
    public static string Customer => "Customer";
    public static string Registered => "Registered";
}

public static class UserHttpClientExtensions
{
    public static async Task CreateUser(this UserHttpClient http, CreateUserModel model)
    {
        try
        {
            if (model == null) throw new ArgumentException("CreateUserModel is null.");

            var user = new RegisterUserDTO(model.Email, model.Password, new List<string> { UserRoles.Registered });

            using StringContent jsonContent = new(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8,
                    "application/json");

            using (HttpResponseMessage response = await http.Client.PostAsync("users/register", jsonContent))
            {
                if(!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
            }
        }
        catch(Exception ex)
        {
            throw;
        }
    }
}
