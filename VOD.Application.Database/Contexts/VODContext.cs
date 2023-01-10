using Microsoft.EntityFrameworkCore;
using VOD.Application.Database.Entities;

namespace VOD.Application.Database.Contexts;

public class VODContext : DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Video> Videos { get; set; }

    public VODContext(DbContextOptions<VODContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

}

