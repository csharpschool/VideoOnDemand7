namespace VOD.Application.Common.DTOs;

public class CoursePageDTO
{
    public CourseDTO Course { get; set; } = new CourseDTO();
    public List<SectionDTO> Sections { get; set; } = new List<SectionDTO>();
}
