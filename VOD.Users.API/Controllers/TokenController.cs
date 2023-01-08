using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using VOD.Token.API.Services;
using VOD.Token.Common.DTOs;
using VOD.UI.Models;

namespace VOD.Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<TokenController> _logger;
        private readonly IUserService _userService;
        private readonly UserManager<VODUser> _userManager;
        private readonly SignInManager<VODUser> _signInManager;
        

        public TokenController(ITokenService service, ILogger<TokenController> logger, IUserService userService, UserManager<VODUser> userManager, SignInManager<VODUser> signInManager)
        {
            _tokenService = service;
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IResult> GetToken([FromBody]LoginUserDTO loginUser)
        {
            try
            {
                if (loginUser is null) return Results.Unauthorized();

                var user = await _userService.GetUserAsync(loginUser);

                if (user is null) return Results.Unauthorized();

                var jwt = await _tokenService.GetTokenAsync(loginUser, user);

                if (string.IsNullOrWhiteSpace(jwt)) return Results.Unauthorized();

                return Results.Ok(new AuthenticatedUserDTO(jwt, user.UserName));
            }
            catch
            {
            }

            return Results.Unauthorized();
        }

        /*[HttpPost(Name = "CreateToken")]
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
        }*/

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