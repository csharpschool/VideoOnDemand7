namespace VOD.Admin.UI.Authentication;

public interface IAuthenticationService
{
    Task<TokenUserDTO?> GetUserFromToken();
    Task<AuthenticatedUserDTO?> Login(AuthenticationUserModel userForAuthentication);
    Task Logout();
}