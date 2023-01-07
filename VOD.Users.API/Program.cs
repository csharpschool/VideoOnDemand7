using Microsoft.AspNetCore.Identity;
using VOD.Token.API.Services;
using VOD.User.Database.Entities;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
RegisterServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<VODUserContext>(
        options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("VODUserConnection")));

    /*builder.Services.AddDefaultIdentity<VODUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<VODUserContext>();*/

    builder.Services.AddIdentity<VODUser, IdentityRole>()
            .AddEntityFrameworkStores<VODUserContext>()
            .AddDefaultTokenProviders();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ITokenService, TokenService>();

}
