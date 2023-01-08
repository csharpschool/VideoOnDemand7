using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace VOD.Token.API.Services;

public class TokenService : ITokenService
{
    #region Properties
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly UserManager<VODUser> _userManager;
    private readonly SignInManager<VODUser> _signInManager;
    #endregion

    #region Constructors
    public TokenService(IConfiguration configuration, IUserService userService, UserManager<VODUser> userManager, SignInManager<VODUser> signInManager)
    {
        _configuration = configuration;
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    #endregion

    #region Helper Methods
    /*private async Task<List<Claim>> AddClaims(VODUser user)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var userClaims = await _userManager.GetClaimsAsync(user);

        foreach (var claim in claims)
        {
            if(!userClaims.Contains(claim))
                await _userManager.AddClaimAsync(user, claim);
        }
        
        return claims;
    }*/

    private string? CreateToken(IList<Claim> claims)
    {
        try
        {
            if(_configuration["Jwt:SigningSecret"] is null || 
               _configuration["Jwt:Duration"] is null || 
               _configuration["Jwt:Issuer"] is null || 
               _configuration["Jwt:Audience"] is null)
                throw new ArgumentException("JWT configuration missing.");

            var signingKey = Convert.FromBase64String(_configuration["Jwt:SigningSecret"]);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature);
            var duration = int.Parse(_configuration["Jwt:Duration"]);
            var now = DateTime.UtcNow;

            var jwtToken = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                notBefore: now,
                expires: now.AddDays(duration),
                claims: claims,
                signingCredentials: credentials
            );

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.WriteToken(jwtToken);
            return token;
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region Token Methods
    public async Task<string?> GenerateTokenAsync(LoginUserDTO loginUserDto)
    {
        try
        {
            var user = await _userService.GetUserAsync(loginUserDto);

            if (user is null) throw new UnauthorizedAccessException();

            var claims = await _userManager.GetClaimsAsync(user);
            var token = CreateToken(claims);

            var result = await _userManager.SetAuthenticationTokenAsync(user, "VOD", "UserToken", token);

            if (result != IdentityResult.Success)
                throw new SecurityTokenException("Could not add token to user");

            return token;
        }
        catch
        {
            throw;
        }
    }

    public async Task<string?> GetTokenAsync(LoginUserDTO loginUserDto, VODUser? user)
    {
        try
        {
            //var user = await _userService.GetUserAsync(loginUserDto);

            if (user is null) throw new UnauthorizedAccessException();

            var token = await _userManager.GetAuthenticationTokenAsync(user, "VOD", "UserToken");

            return token;
        }
        catch
        {
            throw;
        }
    }
    #endregion

    /*public async Task<IResult> Login([FromBody] LoginUserDTO loginUser)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            var signIn = await this._signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (signIn.Succeeded)
            {
                string jwt = CreateJWT(user);
                AppendRefreshTokenCookie(user, HttpContext.Response.Cookies);

                return new LoginResponse(true, jwt);
            }
            else
            {
                return LoginResponse.Failed;
            }
        }
        else
        {
            return LoginResponse.Failed;
        }
    }*/
}
