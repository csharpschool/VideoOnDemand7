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

    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; }
    public ICollection<Section> Sections { get; set; }
}