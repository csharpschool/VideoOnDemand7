using System.ComponentModel.DataAnnotations;

namespace VOD.Application.Common.DTOs;
public class InstructorDTO
{
    public int Id { get; set; }
    [MaxLength(80), Required]
    public string Name { get; set; }
    [MaxLength(1024)]
    public string Description { get; set; }
    [MaxLength(1024)]
    public string Avatar { get; set; }
}
