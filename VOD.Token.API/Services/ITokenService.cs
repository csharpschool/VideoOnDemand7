namespace VOD.Token.API.Services;

public interface ITokenService
{
    Task<string?> GenerateTokenAsync(UpdateUserTokenDTO updateTokenDTO);
    Task<string?> GetTokenAsync(LoginUserDTO loginUserDto, VODUser? user);
}
