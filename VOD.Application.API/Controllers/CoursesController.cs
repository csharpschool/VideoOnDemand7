using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VOD.Application.Common.DTOs;
using VOD.Authentication.Classes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VOD.Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IDbService _db;

        public CoursesController(IDbService db) => _db = db;

        [HttpGet]
        [Authorize(Policy = "Registered")]
        public async Task<IResult> Get(bool freeOnly)
        {
            /*** Implemented for Membership Pages ***/
            try
            {
                _db.Include<Instructor>();
                List<CourseDTO>? courses = freeOnly ?
                    await _db.GetAsync<Course, CourseDTO>(c => c.Free.Equals(freeOnly)) :
                    await _db.GetAsync<Course, CourseDTO>();

                return Results.Ok(courses);
            }
            catch
            {
            }

            return Results.NotFound();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Registered")]
        public async Task<IResult> Get(int id)
        {
            /*** Implemented for Membership Pages ***/
            try
            {
                _db.Include<Instructor>();
                _db.Include<Section>();
                _db.Include<Video>();
                var courses = await _db.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(id));

                return Results.Ok(courses);
            }
            catch
            {
            }
            return Results.NotFound();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IResult> Post([FromBody] CourseCreateDTO dto)
        {
            try
            {
                if (dto == null) return Results.BadRequest();

                var course = await _db.AddAsync<Course, CourseCreateDTO>(dto);

                var success = await _db.SaveChangesAsync();

                if(!success) return Results.BadRequest();

                return Results.Created(_db.GetURI<Course>(course), course);
            }
            catch
            {
            }

            return Results.BadRequest();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IResult> Put(int id, [FromBody] CourseEditDTO dto)
        {
            try
            {
                if (dto == null) return Results.BadRequest("No entity provided");
                if (!id.Equals(dto.Id)) return Results.BadRequest("Differing ids");

                var exists = await _db.AnyAsync<Instructor>(i => i.Id.Equals(dto.InstructorId));
                if (!exists) return Results.NotFound("Could not find related entity");

                exists = await _db.AnyAsync<Course>(c => c.Id.Equals(id));
                if (!exists) return Results.NotFound("Could not find entity");

                _db.Update<Course, CourseEditDTO>(dto.Id, dto);

                var success = await _db.SaveChangesAsync();

                if (!success) return Results.BadRequest();

                return Results.NoContent();
            }
            catch
            {
            }

            return Results.BadRequest("Unable to update the entity");

        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IResult> Delete(int id)
        {
            try
            {
                var success = await _db.DeleteAsync<Course>(id);
            
                if (!success) return Results.NotFound();

                success = await _db.SaveChangesAsync();

                if (!success) return Results.BadRequest();

                return Results.NoContent();
            }
            catch
            {
            }

            return Results.BadRequest();
        }
    }
}
