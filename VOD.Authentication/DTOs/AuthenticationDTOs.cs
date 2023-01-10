namespace VOD.Authentication.DTOs;

public record AuthenticatedUserDTO(string? AccessToken, string? UserName);
public record LoginUserDTO(string Email, string Password);
public record RegisterUserDTO(string Email, string Password, List<string> Roles);
public record TokenUserDTO(string? Email, List<Claim>? Roles);
public record UpdateUserTokenDTO(string Email);
public class SignInModel { public bool IsCustomer { get; set; } };
