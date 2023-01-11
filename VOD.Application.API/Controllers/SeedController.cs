using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VOD.Application.Database.Extensions;

namespace VOD.Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly IDbService _db;

        public SeedController(IDbService db) => _db = db;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IResult> Seed()
        {
            try
            {
                await _db.SeedMembershipData();
                return Results.NoContent();
            }
            catch { }

            return Results.BadRequest();
        }
    }
}
