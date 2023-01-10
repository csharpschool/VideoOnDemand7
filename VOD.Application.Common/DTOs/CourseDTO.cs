using System.ComponentModel.DataAnnotations;

namespace VOD.Application.Common.DTOs;
public class CourseDTO
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string MarqueeImageUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public int InstructorId { get; set; }
    public string Instructor { get; set; }
    public List<SectionDTO> Sections { get; set; }
}
