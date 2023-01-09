namespace VOD.User.Database.Services;

public interface IUserService
{
    Task<VODUser?> GetUserAsync(LoginUserDTO loginUser);
    /*Task<IEnumerable<UserDTO>> GetUsersAsync();
    Task<UserDTO> GetUserAsync(string userId);
    Task<IdentityResult> AddUserAsync(RegisterUserDTO user);
    Task<bool> UpdateUserAsync(UserDTO user);
    Task<bool> DeleteUserAsync(string userId);
    Task<VODUser> GetUserAsync(LoginUserDTO loginUser, bool includeClaims = false);*/
}
