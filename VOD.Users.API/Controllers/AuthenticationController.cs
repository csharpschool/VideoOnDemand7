using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using VOD.Token.API.Services;

namespace VOD.Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<TokenController> _logger;
        private readonly UserManager<VODUser> _userManager;
        private readonly SignInManager<VODUser> _signInManager;

        public AuthenticationController(ITokenService service, ILogger<TokenController> logger, UserManager<VODUser> userManager, SignInManager<VODUser> signInManager)
        {
            _tokenService = service;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

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
        }
    }
}