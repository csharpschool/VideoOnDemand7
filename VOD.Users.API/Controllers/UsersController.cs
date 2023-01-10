using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace VOD.Users.API.Controllers
{
    /*[Route("api/[controller]")]*/
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<VODUser> _userManager;

        public UsersController(UserManager<VODUser> userManager) => _userManager = userManager;

        [Route("users/register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IResult> Register(RegisterUserDTO registerUserDto)
        {
            try
            {
                if (!ModelState.IsValid) return Results.BadRequest();

                var existingUser = await _userManager.FindByEmailAsync(registerUserDto.Email);
                if (existingUser is not null) return Results.BadRequest();

                VODUser newUser = new()
                {
                    Email = registerUserDto.Email,
                    EmailConfirmed = true,
                    UserName = registerUserDto.Email
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, registerUserDto.Password);

                if (result.Succeeded)
                {
                    result = await _userManager.AddToRolesAsync(newUser, registerUserDto.Roles);

                    if (result.Succeeded) return Results.Ok();
                }
            }
            catch { }

            return Results.BadRequest();
        }

        [Route("users/paid")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IResult> Paid(UpdateUserTokenDTO paidCustomerDTO)
        {
            try
            {
                if (!ModelState.IsValid) return Results.BadRequest();

                var paidUser = await _userManager.FindByEmailAsync(paidCustomerDTO.Email);
                if (paidUser is null) return Results.BadRequest();

                IdentityResult result = await _userManager.AddToRoleAsync(paidUser, "customer");

                if (result.Succeeded) return Results.Ok();
            }
            catch (Exception ex)
            {
            }

            return Results.BadRequest();
        }
    }
}
