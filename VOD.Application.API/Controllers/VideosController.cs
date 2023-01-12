using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VOD.Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IDbService _db;

        public VideosController(IDbService db) => _db = db;

        // GET: api/<VideosController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<VideosController>/5
        [HttpGet("{id}")]
        public async Task<IResult> Get(int id)
        {
            /*** Implemented for Membership Pages ***/
            try
            {
                var video = await _db.SingleAsync<Video, VideoDTO>(c => c.Id.Equals(id));
                _db.Include<Course>();
                var section = await _db.SingleAsync<Section, SectionDTO>(c => c.Id.Equals(video.SectionId));
                video.CourseId = section.CourseId;
                video.Course = section.Course;

                return Results.Ok(video);
            }
            catch
            {
            }
            return Results.NotFound();

        }

        // POST api/<VideosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<VideosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VideosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
