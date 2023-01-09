namespace VOD.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticatedUserDTO?> Login(AuthenticationUserModel userForAuthentication);
    Task Logout();
}