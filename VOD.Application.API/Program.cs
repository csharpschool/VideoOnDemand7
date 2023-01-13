using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);
ConfigureAutoMapper(builder.Services);

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

void ConfigureServices(IServiceCollection services)
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

    services.AddScoped<IDbService, DbService>();
}

void ConfigureAutoMapper(IServiceCollection services)
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
    services.AddSingleton(mapper);
}

