using System.ComponentModel.DataAnnotations;

namespace VOD.UI.Models;

public class AuthenticationUserModel
{
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}
