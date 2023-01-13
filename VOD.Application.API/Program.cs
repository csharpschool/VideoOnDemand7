using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using VOD.Authentication.Classes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices();
ConfigureJWTAuthentication();
ConfigureAthorizationPolicies();
ConfigureAutoMapper();

// Configure the HTTP request pipeline.
ConfigureMiddleware();

void ConfigureMiddleware()
{
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("CorsAllAccessPolicy");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

void ConfigureServices()
{
    builder.Services.AddCors(policy => {
        policy.AddPolicy("CorsAllAccessPolicy", opt =>
            opt.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod()
        );
    });

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<VODContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("VODConnection")));

    builder.Services.AddScoped<IDbService, DbService>();
}

void ConfigureAthorizationPolicies()
{
    #region Policies for Authentication
    builder.Services.AddAuthorizationCore(options =>
    {
        options.AddPolicy(UserPolicy.Registered, policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(ClaimTypes.Role, UserRole.Registered) ||
            context.User.HasClaim(ClaimTypes.Role, UserRole.Customer) ||
            context.User.HasClaim(ClaimTypes.Role, UserRole.Admin)
        ));

        options.AddPolicy(UserPolicy.Customer, policy => policy.RequireRole(UserRole.Customer));
        options.AddPolicy(UserPolicy.Admin, policy => policy.RequireRole(UserRole.Admin));
    });
    #endregion
}

void ConfigureJWTAuthentication()
{
    #region JWT Token Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        var signingKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Jwt:SigningSecret"]));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero
        };
        options.RequireHttpsMetadata = false;
    });
    #endregion
}

void ConfigureAutoMapper()
{
    var config = new AutoMapper.MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Video, VideoDTO>().ReverseMap();

        cfg.CreateMap<Instructor, InstructorDTO>()
        .ReverseMap()
        .ForMember(dest => dest.Courses, src => src.Ignore());

        cfg.CreateMap<Course, CourseDTO>()
            //.ForMember(dest => dest.Instructor, src => src.MapFrom(s => s.Instructor.Name))
            .ReverseMap();
        //.ForMember(dest => dest.Instructor , src => src.Ignore());

        cfg.CreateMap<CourseEditDTO, Course>()
            .ForMember(dest => dest.Instructor , src => src.Ignore())
            .ForMember(dest => dest.Sections, src => src.Ignore());

        cfg.CreateMap<CourseCreateDTO, Course>()
            .ForMember(dest => dest.Instructor, src => src.Ignore())
            .ForMember(dest => dest.Sections, src => src.Ignore());

        cfg.CreateMap<Section, SectionDTO>()
            .ForMember(dest => dest.Course, src => src.MapFrom(s => s.Course.Title))
            .ReverseMap()
            .ForMember(dest => dest.Course, src => src.Ignore());
    });
    var mapper = config.CreateMapper();
    builder.Services.AddSingleton(mapper);
}

