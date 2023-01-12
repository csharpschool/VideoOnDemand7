using Microsoft.AspNetCore.Mvc;
using VOD.Application.Common.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VOD.Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IDbService _db;

        public CoursesController(IDbService db) => _db = db;
        

        // GET: api/<CoursesController>
        [HttpGet]
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

        // GET api/<CoursesController>/5
        [HttpGet("{id}")]
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

        // POST api/<CoursesController>
        [HttpPost]
        public async Task<IResult> Post([FromBody] CourseCreateDTO dto)
        {
            try
            {
                if (dto == null) return Results.BadRequest();

                CourseDTO courseDTO = new CourseDTO { Description = dto.Description, Free = dto.Free, ImageUrl = dto.ImageUrl, InstructorId = dto.InstructorId, MarqueeImageUrl = dto.MarqueeImageUrl, Title = dto.Title };

                var course = await _db.AddAsync<Course, CourseDTO>(courseDTO);

                var success = await _db.SaveChangesAsync();

                if(!success) return Results.BadRequest();

                return Results.Created(_db.GetURI<Course>(course), course);
            }
            catch
            {
            }

            return Results.BadRequest();
        }

        // PUT api/<CoursesController>/5
        [HttpPut("{id}")]
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

                CourseDTO courseDTO = new CourseDTO { Id = dto.Id, Description = dto.Description, Free = dto.Free, ImageUrl = dto.ImageUrl, InstructorId = dto.InstructorId, MarqueeImageUrl = dto.MarqueeImageUrl, Title = dto.Title };

                _db.Update<Course, CourseDTO>(dto.Id, courseDTO);

                var success = await _db.SaveChangesAsync();

                if (!success) return Results.BadRequest();

                return Results.NoContent();
            }
            catch
            {
            }

            return Results.BadRequest("Unable to update the entity");

        }

        // DELETE api/<CoursesController>/5
        [HttpDelete("{id}")]
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
