namespace VOD.Token.API.Controllers;

[ApiController]
/*[Route("token/[Action]")]*/
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly ILogger<TokenController> _logger;

    public TokenController(ITokenService service, IUserService userService, ILogger<TokenController> logger)
    {
        _tokenService = service;
        _userService = userService;
        _logger = logger;
    }

    [Route("token")]
    [HttpPost]
    public async Task<IResult> Get([FromBody]LoginUserDTO loginUser)
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

    [Route("token/create")]
    [HttpPost]
    public async Task<IResult> Create(UpdateUserTokenDTO updateTokenDTO)
    {
        try
        {
            var jwt = await _tokenService.GenerateTokenAsync(updateTokenDTO);
            if (jwt == null) return Results.Unauthorized();
            return Results.Created("token", jwt); 
        }
        catch
        {
            return Results.Unauthorized();
        }
    }
}