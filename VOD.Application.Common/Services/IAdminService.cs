using System.Runtime.CompilerServices;
using VOD.Application.Common.DTOs;

namespace VOD.Application.Common.Services
{
    public interface IAdminService
    {
        bool IsAdmin { get; set; }

        Task CreateCourse(CourseDTO course);
        Task<List<CourseDTO>> GetCourses();
        Task<List<InstructorDTO>> GetInstructors();
        Task<bool> HasAdminRole();
        Task SetToken();
    }
}