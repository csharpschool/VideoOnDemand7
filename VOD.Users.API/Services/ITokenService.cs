using VOD.Token.Common.DTOs;

namespace VOD.Token.API.Services;

public interface ITokenService
{
    Task<string?> GenerateTokenAsync(LoginUserDTO loginUserDto);
    Task<string?> GetTokenAsync(LoginUserDTO loginUserDto, VODUser? user);
}
