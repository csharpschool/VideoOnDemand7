using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/login", (IUserService service, [FromBody] LoginUserDTO user) =>
{
    //if (user is null) return Results.BadRequest();
    var forecast = "";
    return forecast;
})
.WithName("Login")
.WithOpenApi();

app.Run();

void RegisterServices(WebApplicationBuilder builder)
{
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<VODUserContext>(
        options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("VODUserConnection")));

    builder.Services.AddScoped<IUserService, UserService>();
}