namespace VOD.User.Database.Services;

public class UserService : IUserService
{
    private readonly VODUserContext _db;
    private readonly UserManager<VODUser> _userManager;

    public UserService(VODUserContext db, UserManager<VODUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<VODUser?> GetUserAsync(LoginUserDTO loginUser)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);

            if (user is null) return default;
        
            var hasher = new PasswordHasher<VODUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);
        
            if (result.Equals(PasswordVerificationResult.Success)) return user;
        }
        catch
        {
        }

        return default;
    }
}
