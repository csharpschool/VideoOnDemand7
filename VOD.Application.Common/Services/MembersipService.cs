using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using VOD.Application.Common.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using VOD.Authentication.Classes;
using VOD.Authentication;
using Blazored.LocalStorage;
using VOD.Authentication.DTOs;

namespace VOD.Application.Common.Services;

public class MembersipService : IMembersipService
{
    private readonly ApplicationHttpClient _http;
    private readonly ILocalStorageService _localStorage;

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

            using HttpResponseMessage courseResponse = await _http.Client.GetAsync($"courses?freeOnly={freeOnly}");
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

    public async Task<CourseDTO> GetCourse(int? courseId)
    {
        try
        {
            if(courseId is null) return new CourseDTO();

            using HttpResponseMessage courseResponse = await _http.Client.GetAsync($"courses/{courseId}");
            courseResponse.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<CourseDTO>(await courseResponse.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new CourseDTO();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<VideoDTO> GetVideo(int? videoId)
    {
        try
        {
            if (videoId is null) return new VideoDTO();

            using HttpResponseMessage courseResponse = await _http.Client.GetAsync($"videos/{videoId}");
            courseResponse.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<VideoDTO>(await courseResponse.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new VideoDTO();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

}
