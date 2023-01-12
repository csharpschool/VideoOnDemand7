using System.Runtime.CompilerServices;
using VOD.Application.Common.DTOs;

namespace VOD.Application.Common.Services
{
    public interface IAdminService
    {
        bool IsAdmin { get; set; }

        Task<List<CourseDTO>> GetCourses();
        Task<bool> HasAdminRole();
        Task SetToken();
    }
}