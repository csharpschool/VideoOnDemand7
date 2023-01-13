using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VOD.Application.Common.DTOs;
public class CourseDTO
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string MarqueeImageUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Free { get; set; }

    public int InstructorId { get; set; }
    /*[JsonIgnore, NotMapped]*/
    public InstructorDTO Instructor { get; set; } = new();
    /*[JsonIgnore, NotMapped]*/
    public List<SectionDTO> Sections { get; set; } = new();
}

public class CourseCreateDTO
{
    public string ImageUrl { get; set; }
    public string MarqueeImageUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Free { get; set; }

    public int InstructorId { get; set; }
}

public class CourseEditDTO : CourseCreateDTO
{
    public int Id { get; set; }
}


public record ClickModel(string PageType, int id);