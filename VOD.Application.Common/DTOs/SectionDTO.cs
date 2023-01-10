using System.ComponentModel.DataAnnotations;

namespace VOD.Application.Common.DTOs;
public class SectionDTO
{
    public int Id { get; set; }
    [MaxLength(80), Required]
    public string Title { get; set; }

    public int CourseId { get; set; }
    public string Course { get; set; }
    public List<VideoDTO> Videos { get; set; }
    
    public string CourseAndModule { get { return $"{Title} ({Course})"; } }
}
