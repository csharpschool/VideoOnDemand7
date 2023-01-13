using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VOD.Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IDbService _db;
        public InstructorsController(IDbService db) => _db = db;

        // GET: api/<InstructorsController>
        [HttpGet]
        [Authorize(Policy = "Registered")]
        public async Task<IResult> Get()
        {
            try
            {
                List<InstructorDTO>? instructors = await _db.GetAsync<Instructor, InstructorDTO>();

                return Results.Ok(instructors);
            }
            catch
            {
            }

            return Results.NotFound();
        }

        // GET api/<InstructorsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InstructorsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InstructorsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InstructorsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
