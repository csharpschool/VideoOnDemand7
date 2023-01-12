using Microsoft.AspNetCore.Mvc;

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
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CoursesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CoursesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
