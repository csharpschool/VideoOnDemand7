using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using VOD.Token.API.Services;

namespace VOD.Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<TokenController> _logger;
        private readonly UserManager<VODUser> _userManager;
        private readonly SignInManager<VODUser> _signInManager;
        

        public TokenController(ITokenService service, ILogger<TokenController> logger, UserManager<VODUser> userManager, SignInManager<VODUser> signInManager)
        {
            _tokenService = service;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet(Name = "GetToken")]
        public async Task<IResult> Get([FromQuery]LoginUserDTO loginUser)
        {
            try
            {
                if (loginUser is null) return Results.Unauthorized();

                var jwt = await _tokenService.GetTokenAsync(loginUser);

                if (string.IsNullOrWhiteSpace(jwt)) return Results.Unauthorized();

                return Results.Ok(jwt);
            }
            catch
            {
            }

            return Results.Unauthorized();
        }

        [HttpPost(Name = "CreateToken")]
        public async Task<IResult> GenerateTokenAsync(LoginUserDTO loginUserDto)
        {
            try
            {
                var jwt = await _tokenService.GenerateTokenAsync(loginUserDto);
                if (jwt == null) return Results.Unauthorized();
                return Results.Created("", jwt);
            }
            catch
            {
                return Results.Unauthorized();
            }
        }

        /*[Route("Login")]
        [HttpPost(Name = "Login")]
        public async Task<IResult> Login([FromBody] LoginUserDTO loginUser)
        {

            try
            {
                var user = await _userManager.FindByEmailAsync(loginUser.Email);
                if (user != null)
                {
                    var signIn = await _signInManager.CheckPasswordSignInAsync(user, loginUser.Password, false);
                    if (!signIn.Succeeded) return Results.Unauthorized();

                    var jwt = await _tokenService.GetTokenAsync(loginUser);
                    //AppendRefreshTokenCookie(user, HttpContext.Response.Cookies);

                    return Results.Ok(jwt);
                }
            }
            catch
            {
            }

            return Results.Unauthorized();
        }*/
    }
}