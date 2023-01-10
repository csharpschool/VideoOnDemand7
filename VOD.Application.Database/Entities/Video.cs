namespace VOD.Application.Database.Entities;

public class Video : IEntity
{
    public int Id { get; set; }
    [MaxLength(80), Required]
    public string Title { get; set; }
    [MaxLength(1024)]
    public string Description { get; set; }
    public int Duration { get; set; }
    [MaxLength(1024)]
    public string Thumbnail { get; set; }
    [MaxLength(1024)]
    public string Url { get; set; }

    public int SectionId { get; set; }
/*     public int CourseId { get; set; }
   public Course Course { get; set; }
    public Section Section { get; set; }*/
}