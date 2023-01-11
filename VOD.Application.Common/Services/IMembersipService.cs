using VOD.Application.Common.DTOs;

namespace VOD.Application.Common.Services
{
    public interface IMembersipService
    {
        Task<CourseDTO> GetCourse(int? courseId);
        Task<List<CourseDTO>> GetCourses();
        Task<VideoDTO> GetVideo(int? videoId);
    }
}