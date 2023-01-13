namespace VOD.Authentication.Models;

public class CreateUserModel
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required, DisplayName("Confirm Password"), Compare(nameof(Password), ErrorMessage = "Paswords don't match.")]
    public string ConfirmPassword { get; set; }

    public bool IsCustomer { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
}
