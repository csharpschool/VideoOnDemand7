using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace VOD.Application.Common.Services;

public class MembersipService : IMembersipService
{
    protected readonly ApplicationHttpClient _http;
    protected readonly ILocalStorageService _localStorage;

    public MembersipService(ApplicationHttpClient httpClient, ILocalStorageService localStorage)
    {
        _http = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<CourseDTO>> GetCourses()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            bool freeOnly = JwtParser.ParseIsNotInRoleFromJWT(token, UserRole.Customer);

            _http.AddBearerToken(token);
            using HttpResponseMessage courseResponse = await _http.Client.GetAsync($"courses?freeOnly={freeOnly}");
            courseResponse.EnsureSuccessStatusCode();
           
            var result = JsonSerializer.Deserialize<List<CourseDTO>>(await courseResponse.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new List<CourseDTO>();
        }
        catch
        {
            return new List<CourseDTO>();
        }
    }

    public async Task<CourseDTO> GetCourse(int? courseId)
    {
        try
        {
            
            if(courseId is null) return new CourseDTO();

            var token = await _localStorage.GetItemAsync<string>("authToken");

            _http.AddBearerToken(token);
            using HttpResponseMessage courseResponse = await _http.Client.GetAsync($"courses/{courseId}");
            courseResponse.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<CourseDTO>(await courseResponse.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new CourseDTO();
        }
        catch (Exception ex)
        {
            return new CourseDTO();
        }
    }

    public async Task<VideoDTO> GetVideo(int? videoId)
    {
        try
        {
            if (videoId is null) return new VideoDTO();

            var token = await _localStorage.GetItemAsync<string>("authToken");

            _http.AddBearerToken(token);
            using HttpResponseMessage courseResponse = await _http.Client.GetAsync($"videos/{videoId}");
            courseResponse.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<VideoDTO>(await courseResponse.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new VideoDTO();
        }
        catch (Exception ex)
        {
            return new VideoDTO();
        }
    }

}
