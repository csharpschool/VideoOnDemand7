namespace VOD.Authentication.DTOs;

public record AuthenticatedUserDTO(string? AccessToken, string? UserName);
public record LoginUserDTO(string Email, string Password);
public record RegisterUserDTO(string Email, string Password, List<string> Roles);