using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VOD.Application.Database.Entities;

public class Course : IEntity
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string ImageUrl { get; set; }
    [MaxLength(255)]
    public string MarqueeImageUrl { get; set; }
    [MaxLength(80), Required]
    public string Title { get; set; }
    [MaxLength(1024)]
    public string Description { get; set; }
    public bool Free { get; set; }

    public int InstructorId { get; set; }
    /*[JsonIgnore, NotMapped]*/
    public virtual Instructor Instructor { get; set; }
    /*[JsonIgnore, NotMapped]*/
    public virtual ICollection<Section> Sections { get; set; }
}