using System.ComponentModel.DataAnnotations;

namespace VOD.Application.Common.DTOs;

public class VideoDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public string Thumbnail { get; set; }
    public string Url { get; set; }

    // Side-step from 3rd normal form for easier 
    // access to a video’s course and Module
    public int SectionId { get; set; }
    public int ModuleId { get; set; }
    public string Course { get; set; }
    public string Module { get; set; }

}
