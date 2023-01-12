using VOD.Authentication.Classes;
using VOD.Authentication;
using System.Text.Json;
using VOD.Application.Common.DTOs;

namespace VOD.Application.Common.Services;

public class AdminService : IAdminService
{
    public bool IsAdmin { get; set; }
    protected readonly ApplicationHttpClient _http;
    protected readonly ILocalStorageService _localStorage;

    public AdminService(ApplicationHttpClient httpClient, ILocalStorageService localStorage)
    {
        _http = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<CourseDTO>> GetCourses()
    {
        try
        {
            using HttpResponseMessage courseResponse = await _http.Client.GetAsync($"courses?freeOnly=false");
            courseResponse.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<List<CourseDTO>>(await courseResponse.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new List<CourseDTO>();
        }
        catch (Exception ex)
        {
            throw;
        }
    }




    #region For development purposes only
    public async Task SetToken()
    {
        var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwOi8veW91ci1kb21haW4uY29tIiwiYXVkIjoiaHR0cDovL2F1ZGllbmNlLWRvbWFpbi5jb20iLCJuYmYiOiIxNjczNDQxMDkyIiwiZXhwIjoiMTY3NDMwNTA5MiIsInN1YiI6ImpvaG5Adm9kLmNvbSIsImVtYWlsIjoiam9obkB2b2QuY29tIiwianRpIjoiZjg5YmJkZGItNGNlYy00NTEwLTlmYzEtM2EzZGEwNTBkN2Y5IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpbIkFkbWluIiwiQ3VzdG9tZXIiXX0.Avl3AmOwkHmVCPgox_to-oA9KZ22V1EPlLqnaxrg1bY";
        await _localStorage.SetItemAsync("authToken", token);
    }
    public async Task<bool> HasAdminRole()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            IsAdmin = JwtParser.ParseIsInRoleFromJWT(token, UserRole.Admin);

            return IsAdmin;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    #endregion
}
