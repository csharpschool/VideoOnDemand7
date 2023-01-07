using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VOD.User.Database.Entities;

namespace VOD.User.Database.Contexts;

public class VODUserContext : IdentityDbContext<VODUser>
{
    public VODUserContext(DbContextOptions<VODUserContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //SeedData(builder);
        base.OnModelCreating(builder);
    }

    private void SeedData(ModelBuilder builder)
    {
        CreateUser(builder, "john@vod.com", "Pass123__");
        CreateUser(builder, "jane@vod.com", "Pass123__");
    }

    private void CreateUser(ModelBuilder builder, string email, string password)
    {
        #region Admin Credentials Properties
        var user = new VODUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            NormalizedEmail = email.ToUpper(),
            UserName = email,
            NormalizedUserName = email.ToUpper(),
            EmailConfirmed = true
        };

        var passwordHasher = new PasswordHasher<VODUser>();
        user.PasswordHash = passwordHasher.HashPassword(user, password);

        // Add user to database
        builder.Entity<VODUser>().HasData(user);
        #endregion
    }

}
